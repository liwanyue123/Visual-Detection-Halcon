using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
using System.Drawing;
using HalconDotNet;
using SharedData;

namespace wheelDetection
{
    public class DetectionResultEventArgs : EventArgs//继承事件基类,将检测结果传回去，包括是否正确，匹配的位置，轮子检测的位置
    {

        public HXLDCont ShapeModelContour;//轮廓形状
        public HXLDCont xldCrossPoint;//轮廓中心位置
        public HXLDCont ROI_R_AfterTrans;//找轮子的矩形
        public HRegion BinaryRegion;//二值化轮子区域
        public int pixieTotalNum;//轮子部分阴影面积
        public int pixieOfRoller;//整个矩形框面积
        public bool isQualified;//是否合格
        public bool isNotice;//是否通知了串口
        public bool detectionRoller;//检测的类型true：轮子，false：杂物
        
        public int numMiss;//第几次缺料了

    }
    //子窗体measure要用到的所有数据
     

    /// 和comModel交互
    public interface IDetect
    {
        void StartDetect(bool detectionRoller);//让comModel来调用，串口收到检测图片信号时，调用这个，就开始检测
        //void resetShapeModelByHand(HRegion ROI_Match, HImage hImage);//form画完模板框子roi后，调用这个重设model
    }

    //和form交互，通过control做中介
    //声明委托 ，用来将检测结果反馈会form，让它改变窗体
    public delegate void StopBtnChangeHandler(bool statue);
    //发送当前图片的检测结果
    public delegate void FormDetectEventHandler(Object sender, DetectionResultEventArgs e);

    public delegate HObject RequestShootPicHandler();//委托，让form赶快把截图送过来
    public delegate void GiveFormDataHandler(MeasureRectangleData e);
    public delegate void GiveComDataHandler(int delay);

    public delegate void UnqualifiedNoticeHandler();//检测不合格，通知com
    public class Detection : IDetect
    {
        MeasureRectangleData saveData;
        MeasureModelData modelData;

        HObject PicByRequest;//请求所得的图片
        DetectionResultEventArgs detectionResultEvent ;


        /// <summary>
        /// //////////////////////////////////
        /// </summary>
        // double threadParam = 190;

        //下面的这些全写进saveData中了
        //HShapeModel ModelID;
        //double RectbiasAngle;//轮子框自身的角度偏差（和模板角度之间的差）
        //Rectangle2Inf rect2Inf;//轮子框
        //double distanceMove;//轮子框中心到模板中心的距离
        //double biasAngleOfRectCenterPoint;//轮子框中心到模板中心连线的角度-模板的角度

        //用来和form中的事件绑定，control来做
        //public event GiveFormDataHandler GiveFormDataEvent = null;//发送一些数据给form，目前是轮子检测成功的像素比例
        //public event GiveComDataHandler GiveComDataEvent = null;//发送一些数据给Com，
        public event FormDetectEventHandler DetectQualifiedEvent = null;//发送当前图片的检测结果
        public event RequestShootPicHandler RequestShootPicEvent = null;//让form赶快把截图送过来

        //det->com
        public event UnqualifiedNoticeHandler UnqualifiedNotice = null;//检测不合格，通知com
        //det->form
        public event StopBtnChangeHandler StopBtnChangeEvent =null;//改变启动按钮的字体
        public Detection()//构造函数，将主窗体传过来，用其中的接口函数
        {

            //myData data=SaveDataTool.FormBetyFile<myData>("22.ev");
            //int c = data.a;
            //读取初始化模型
            //saveData = SaveDataTool.FormBetyFile<SaveData>(@"../../1.ev");//反序列号读取匹配模型C:/detectionData/
            modelData = SaveDataTool.FormBetyFile<MeasureModelData>(@"C:/detectionData/Model.ev");
            saveData = SaveDataTool.FormBetyFile<MeasureRectangleData>(@"C:/detectionData/RectData.ev");//读取几个框子数据
            if (modelData==null)
            {
                modelData = new MeasureModelData();
                
            }
            if(saveData==null)
            {
                saveData = new MeasureRectangleData();
            }
            detectionResultEvent = new DetectionResultEventArgs();
        }
 
      
        /// <summary>
        /// 因为读取，写入savedata都写在了这个类里面，但是form和它的子窗体创建时要里面的信息呀
        /// 本来是要在构造函数中把savedata的数据给form的
        /// 但是构造函数这表示这个类还没建好，所以委托之类的完全还没人帮你接上
        /// 把检测的所有参数存入主窗体中，这样它生成子窗体时就可以显示最新的数据
        /// </summary>
        /// 
 

        public void DetectionResult(DetectionResultEventArgs e)
        {
            if (DetectQualifiedEvent != null)
            {
                DetectQualifiedEvent.Invoke(this, e);
            }
        }

        /// <summary>
        /// 使用内置的图片来生成
        /// 不向form返回检测结果
        /// </summary>
        public void resetShapeModelByInner()//等会改成完全读取
        {
            ////////////////////设置好ModelID//////////////////
            modelData = SaveDataTool.FormBetyFile<MeasureModelData>(@"C:/detectionData/inner.ev");
 
           // isNeedMatch = true;//允许重新匹配定位
 

        }
        public void GetDetInitRectInfEvent(MeasureRectangleData initialData )

        {
            this.saveData = initialData;
        }

        int round = 0;//表示还没有标定，这时候匹配大概率匹配不到
        /// <summary>
        /// 使用给定的图片和轮廓来做，
        /// 手工标定。不仅要给图片，还要圈住roi
        /// 向form返回检测结果，因为要让操作者看到设定的结果
        /// </summary>
        /// <param name="ROI_Match"></param>
        /// <param name="hImage"></param>
        public void resetShapeModelByHand(MeasureRectangleData InitialData, HObject hImage)
        {
            HRegion ROI_Match = new HRegion(InitialData.rectangle1Inf.mainRow1, InitialData.rectangle1Inf.mainColumn1, InitialData.rectangle1Inf.mainRow2, InitialData.rectangle1Inf.mainColumn2);
            HImage img = HObject2HImage1(hImage);//将object转himage
            ////////////////////设置好ModelID//////////////////
            //int roiRegion=  ROI_Match.AreaCenter(out HTuple x, out HTuple y);
            //int roiRegion2 = ROI_Det.AreaCenter(out HTuple x1, out HTuple y1);
            HImage GrayImage = img.Rgb1ToGray();//先二值化

            HImage ImageROI = GrayImage.ReduceDomain(ROI_Match);//初步裁出roi


            HXLDCont Edges = ImageROI.EdgesSubPix("canny", 1, 20, 40);//提取边缘
            Edges.SelectShapeXld("area_points", "and", 50, 99999);//筛去小的轮廓线
            int n = Edges.CountObj();
            if (n == 0)
            {
                return;
                //没有找到边界
            }
            modelData.ModelID = Edges.CreateShapeModelXld("auto", -0.39, 0.79, "auto", "auto", "ignore_local_polarity", 5);//创建模板匹配模型
            saveData=InitialData;
 
            //GiveComData();//把延时信息告诉串口
            if (saveData .rectangle2Inf .DetAngle < 0)
            {
                saveData.rectangle2Inf.DetAngle += 2 * Math.PI;
            }
            isNeedMatch = true;//允许重新匹配定位
            if (round==0)
            {
                isNeedMatch = false;//允许重新匹配定位
            }
            round = 1;
            ModelMatch(GrayImage, true,false,false);//检测一次返回结果看看
        }

        public bool isNeedMatch { get; set; } = false;
        HTuple ModelCenterRow, ModelCenterColumn, modelAngle;//模板匹配的输出
        double RectTrueAngle, rowBias, colBias;
        double RectTrueRow, RectTrueCol;
        HXLDCont xldCrossPoint,ShapeModelContour;
        bool isQualified;
        int missNum = 0;//5次检测不到轮子就报警
        public bool errorStop = false;//因为检测到错误而暂停吗？true表示暂停


                               // int round = 0;//检测一次轮子和杂物，算一个周期
                               //HRegion BinaryRegion;
                               /// <summary>
                               /// /在设置好模板（ModelID和ShapeModel）后，给出图片，调用这个开始匹配，顺便完成轮子部分像素检测
                               /// 向form返回检测结果
                               /// 但在手动生成模板的时候检测的结果isQulified不会触发报警（直接true，不计数）
                               /// detectionRoller==false表示检查杂物，为true表示检查轮子
                               /// </summary>
                               /// <param name="GrayImage"></param>
        public void ModelMatch(HImage GrayImage, bool isResetModel, bool isNoticeCom, bool detectionRoller)//设定模型时要计算角度偏差值，写入文件，且不通知串口，平时检测不用写，但要通知串口
        {
            //从模型中得到匹配的轮廓形状
            HXLDCont ShapeModel = modelData.ModelID.GetShapeModelContours(1);

            ///////////////////////////////模板匹配///////////////////////////////
            if (isNeedMatch == true&& detectionRoller==false)//现在需要匹配(每20张匹配几张),不需要匹配时就只计算一下轮子区域的像素比，返还结果
            {
                //在图片中匹配  
                int[] sArray = { 0, -1 };
                modelData.ModelID.FindShapeModel(GrayImage, -0.39, 0.79, 0.8, 1, 0.8, new HTuple("least_squares"), new HTuple(sArray), 0.9, out ModelCenterRow, out ModelCenterColumn, out modelAngle, out HTuple score);//这里的angle 是弧度制（0，2pi）
                isNeedMatch = false;
                if(ModelCenterRow.Length ==0)//没找到
                {
                    return;
                }

                if (isResetModel == true)//现在是重置模型的状态，计算各种数值载入saveData
                {

 
                    ///////////////////////////计算中心位置偏差////////////////////////////////RectbiasAngle/
                    HOperatorSet.DistancePp(ModelCenterRow, ModelCenterColumn, saveData.rectangle2Inf .DetRow, saveData.rectangle2Inf.DetCol, out HTuple distance);//row, column是模板中心,另一个点是轮子检测框中心
                    modelData .distanceMove = (double)distance;

                    ///////////////////////////计算矩形角度偏差/////////////////////////////////
                    modelData.RectbiasAngle = saveData.rectangle2Inf .DetAngle - modelAngle;//计算角度偏差值

                    ///////////////////////////计算中心角度偏差/////////////////////////////////
                    HOperatorSet.AngleLx(ModelCenterRow, ModelCenterColumn, saveData.rectangle2Inf.DetRow, saveData.rectangle2Inf.DetCol, out HTuple RectCenterPointTrueAngle);//后面的是终点（计算角度有方向）
                    double angle = RectCenterPointTrueAngle;
                    if (angle < 0)//因为这个范围是（-pi,pi）改成（0，2pi）
                    {
                        angle += 2 * Math.PI;//之后要改成和model的偏差
                    }
                    modelData.biasAngleOfRectCenterPoint = angle - modelAngle;
                }


                //////////////////////////结果可视化（要送给form画出来）///////////////////
                //匹配轮廓的中心
                xldCrossPoint = new HXLDCont();//bug ModelCenterRow, ModelCenterColumn, 6, modelAngle都是空
                xldCrossPoint.GenCrossContourXld(ModelCenterRow, ModelCenterColumn, 6, modelAngle);//匹配轮廓的中心打个×
                                                                                                   //////////////////////模板轮廓/////////////////
                                                                                                   //轮廓进行仿射变换  ShapeModelContou是轮廓形状，xldCrossPoint是中心位置
                HHomMat2D MovementOfResult = new HHomMat2D();
                MovementOfResult.VectorAngleToRigid(0, 0, 0, ModelCenterRow, ModelCenterColumn, modelAngle);
                ShapeModelContour = ShapeModel.AffineTransContourXld(MovementOfResult);//100ms


                //////////////////////////////转成矩形2实际角度和坐标///////////////////////////////////
                RectTrueAngle = modelData.RectbiasAngle + modelAngle;//矩形2的角度
                rowBias = -modelData.distanceMove * Math.Sin(modelData.biasAngleOfRectCenterPoint);
                colBias = modelData.distanceMove * Math.Cos(modelData.biasAngleOfRectCenterPoint);
                RectTrueRow = ModelCenterRow + rowBias;//矩形2的中心点坐标
                RectTrueCol = ModelCenterColumn + colBias;
            }

            ///////////////////////////////测窗轮像素///////////////////////////////
            HRegion ROI_R = new HRegion();//生成一个矩形框，根据手画的结果定
            //saveData.rectangle2Inf为null 238 504 0 93 98
            ROI_R.GenRectangle2(saveData.rectangle2Inf .DetRow, saveData.rectangle2Inf.DetCol, (double)RectTrueAngle, saveData.rectangle2Inf.Detlen1, saveData.rectangle2Inf.Detlen2);//角度用计算出的

            //标定框子长这个样子hWindow_Final1.viewWindow._roiController.displayRect2("blue", InitialData.rectangle2Inf.DetRow, InitialData.rectangle2Inf.DetCol, InitialData.rectangle2Inf.DetAngle, InitialData.rectangle2Inf.Detlen1, InitialData.rectangle2Inf.Detlen2);//小框子
             
            int pixieTotalNum = ROI_R.AreaCenter(out double mainRow2, out double mainColumn2);//整个区域像素值，中心点坐标

            //移动矩形roi到对应位置上 
            HHomMat2D MovementOfRectangle = new HHomMat2D();
            MovementOfRectangle.VectorAngleToRigid(mainRow2, mainColumn2, 0, RectTrueRow, RectTrueCol, 0);//角度之前已经计算并且旋转好了
            HRegion ROI_R_AfterTrans = ROI_R.AffineTransRegion(MovementOfRectangle, "nearest_neighbor");

            HImage roller = GrayImage.ReduceDomain(ROI_R_AfterTrans);
 
            //HRegion rollerAfterBinary = roller.Threshold(0, threadParam);
            HRegion rollerAfterBinary = roller.Threshold(0, saveData.otherRectParam .threadold);
            //HRegion rollerAfterBinary = roller.BinaryThreshold("max_separability", "dark", out HTuple usedThreadold);
            //HRegion rollerAfterBinary = roller.AutoThreshold(2.0);
            int pixieOfRoller = rollerAfterBinary.AreaCenter(out double row3, out double column3);//18ms


            //ROI_R_AfterTrans是用来找轮子的矩形，pixieOfRoller是轮子部分阴影面积，pixieTotalNum是整个矩形框面积
            detectionResultEvent.isNotice = false;
            if (detectionRoller==true)//检测轮子，大于像素阈值才行
            {
                if ((double)pixieOfRoller / (double)pixieTotalNum >= (double)saveData.otherRectParam.pixPercentOfRoller / 100.0)//这里目的是找轮子
                {
                    isQualified = true;
                    missNum = 0;
                }
                else//没找到
                {
                    isQualified = false;
                    missNum++;
                    if (isNoticeCom == true && UnqualifiedNotice != null&&missNum>=saveData.otherRectParam.numMiss)//如果要通知串口
                    {
              
                        UnqualifiedNotice();
                        errorStop = true;
                        StopBtnChangeEvent(true);
                        detectionResultEvent.isNotice = true;
                        missNum = 0;
                         
             
                    }
                       
                }
            }
            else//检测杂物，小于像素阈值才行
            {
                if ((double)pixieOfRoller / (double)pixieTotalNum <= (double)saveData.otherRectParam.pixPercentOfRubbish / 100.0)//这里目的不是找轮子，而是看有没有杂物
                {
                    isQualified = true;
                }
                else
                {
                    isQualified = false;
                    if (isNoticeCom == true && UnqualifiedNotice != null)//如果要通知串口
                    {
                       

                        UnqualifiedNotice();
                        errorStop = true;
                        StopBtnChangeEvent(true);
                        detectionResultEvent.isNotice = true;
                    }
                        
                }
            }
          

            //结果告诉Form,
            HXLDCont rectXld = new HXLDCont();
            rectXld.GenRectangle2ContourXld(RectTrueRow, RectTrueCol, RectTrueAngle, saveData.rectangle2Inf .Detlen1, saveData.rectangle2Inf.Detlen2);
            
            detectionResultEvent.pixieOfRoller = pixieOfRoller;
            detectionResultEvent.pixieTotalNum = pixieTotalNum;
            detectionResultEvent.ROI_R_AfterTrans = rectXld;
            detectionResultEvent.BinaryRegion = rollerAfterBinary;
            detectionResultEvent.ShapeModelContour = ShapeModelContour;
            detectionResultEvent.xldCrossPoint = xldCrossPoint;
            detectionResultEvent.isQualified = isQualified;
            detectionResultEvent.detectionRoller = detectionRoller;
            detectionResultEvent.numMiss = missNum;

            DetectionResult(detectionResultEvent);//结果返回回去

            if (isResetModel == true)//现在是重置模型的状态
            {
                //GiveFormData();//告诉主窗体所有的参数信息，存入其中，生成子窗体frm_Unit_Measure时就会显示最新的数据
                //GiveComData();//告诉串口检测延时
                SaveDataTool.Save2File(modelData, @"C:/detectionData/Model.ev");//存储一下

            }

        }

     
        public void SetCalibrationState(bool isCalibrationg)
        {
            this.isCalibrationg = isCalibrationg;
        }
        int NeedMatchNum = 0;
        bool isCalibrationg = false;
        public void StartDetect(bool detectionRoller)//开始检测一次,detectionRoller==false表示检查杂物，为true表示检查轮子
        {
            if(errorStop==false)//没有故障暂停
            {
                if (RequestShootPicEvent != null)//要一张截图
                {
                    PicByRequest = RequestShootPicEvent();
                }
                if (PicByRequest == null)//没要到截图就不检测了
                {
                    return;
                }
                if (isCalibrationg == true)//是不是在标定中，如果是就不检测
                {
                    return;
                }
                ///设置检测模板，等会转移到初始化中
                HImage GrayImage = (HObject2HImage1(PicByRequest)).Rgb1ToGray();//先二值化 

                if (modelData.ModelID == null)//第一次要么生成，要么加载。之后的就用这里的
                {
                    //存储的数据中没有模型，就新建一个，图片和roi都是内定好的  
                    resetShapeModelByInner();//默认用内置，有手动按钮 
                }
                if (NeedMatchNum <= 0)
                {
                    isNeedMatch = true;//允许定位
                    NeedMatchNum = 20;//重新计数

                }
                else if (NeedMatchNum > 0 && saveData.otherRectParam.isMatch)//暂时还没有计数满20个，且允许定位
                {
                    NeedMatchNum--;
                }

                ModelMatch(GrayImage, false, true, detectionRoller);//调用模板匹配
            }
          

        }

        /// <summary>
        /// 灰度图像 HObject -> HImage1
        /// </summary>
        public HImage HObject2HImage1(HObject hObj)
        {
            HImage image = new HImage();
            HTuple type, width, height, pointer;
            HOperatorSet.GetImagePointer1(hObj, out pointer, out type, out width, out height);
            image.GenImage1(type, width, height, pointer);
            return image;
        }

  
    }
}
