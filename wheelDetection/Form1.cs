using System;

 
using System.Windows.Forms;

using AForge.Video.DirectShow;

 
using System.Collections.Generic;
 
using System.Drawing;
 
using System.IO.Ports;
 
using HalconDotNet;
using System.Threading;
using FormFinalDesign;
using SharedData;
//using System.Threading.Tasks;

namespace wheelDetection
{

    // public interface IVidieoView//视频事件接口，因为视频之类的函数全在主窗体，所以没必要订阅了，这里只是为了好看
    public interface IView//串口事件接口
    {
        //视频
        void DetectQualifiedEvent(Object sender, DetectionResultEventArgs e);//得到检测结果后，主窗体进行对应的显示
       
        HObject RequestShootPicEvent();//提供当前的视频截图给detetion

      //void GiveFormDataEvent(MeasureRectangleData e);//如果要给我一些数据，就通过这个接口；

        // SaveData RequestInitialModelEvent();//提供初始化时读取的数据


        //串口
        void SetController(IController controller);

        //Open serial port event
        //void OpenComEvent(Object sender, SerialPortEventArgs e);
        //Close serial port event
        //void CloseComEvent(Object sender, SerialPortEventArgs e);
        //Serial port receive data event
        //void ComReceiveDataEvent(Object sender, SerialPortEventArgs e);

    }

    //public delegate void ComCamBothOpenHandel();
   // public delegate void ComCamBothCloseHandel();
    //form->frm
    public delegate void SetDataToChildFormHandel(int grayVal);
    //form->det
    public delegate void SetCalibrationStateHandel(bool isCalibrationg);//告诉detection可不可以检测
    public delegate void SetNewShapeModelHandel(MeasureRectangleData InitialData, HObject hImage) ;//让detection重新设置模型
    public delegate void GiveComDataHandel(int delayRubbishDetectionTime, int delayRollerDetectionTime);

    //public delegate void SetInitParamHandel(int PixPercent);//让detection重新设置参数
    //form->com
    public delegate void ComHandel(Object sender, SerialPortEventArgs e);
    public partial class Form1 : Form, IView
    {
        #region 常量

       
        static Semaphore sema = new Semaphore(1, 1);//解决每一帧照片都要清除和检测方要求图片的问题

        //public event ComCamBothOpenHandel ComCamBothOpenEvent=null;
        //public event ComCamBothCloseHandel ComCamBothCloseEvent = null;
        public event SetCalibrationStateHandel SetCalibrationStateEvent = null;
        public event SetDataToChildFormHandel SetDataToChildFormEvent = null;
        public event SetNewShapeModelHandel SetNewShapeModelEvent = null;//在frm_Unit_Measure点击生成检测后调用,让detection重新设置模型
        public event GiveComDataHandel GiveComDataEvent = null;//告诉com延时


        //public event SetInitParamHandel SetInitParamEvent = null; //让detection设置多少百分比像素才成功
        ////显示////
        HObject shootImg, CurrentFrameImage, ImageSource; //截图
        ViewWindow.Model.ROI region;
        bool isExist;//如果打算只在屏幕上显示一个roi，就要删除之前的，将它的isExist设为false
        
        public bool draw = false;//是否要绘制
        public int style = 0;//要绘制的类型0--直线，1圆，2，矩形，3，角度矩形，4，扇形

        //private Rectangle depart;//为了记录图像的原始状态（20191030)
        //子窗体确定生成模型的时候会设定这个
        public MeasureRectangleData InitialData;//就是子窗体measure要用到的所有参数
        ComData comData;
        //public double row1, col1, row2, col2; 
        //public  double DetCol, DetRow, Detlen1, Detlen2, DetAngle;
        //public int delay,lineWidth;
        //public double threadold;
        frm_Unit_Measure  m_frm_Unit;
        frm_Com m_frm_Com;

        //public SaveData saveData ;//序列化保存的信息

        ////视频////
        HTuple  hwindow;
        public HObject src;
        //Thread dispig;
        HTuple width = null, height = null;
        Thread cameraTask;
        // public bool isStop = false;//要暂停相机吗？
        private CameraState cameraState = CameraState.关闭连接;
        public FilterInfoCollection videoDevices;//枚举视频设备 
        public VideoCaptureDevice videoDevice;

        ////串口////
        public ComConfigureInf comConfigureInf;
        public IController controller;
        private int sendBytesCount = 0;
        private int receiveBytesCount = 0;

        #endregion
 

        public Form1()
        {

            InitializeComponent();
            InitialData = new MeasureRectangleData();
 
            this.controller = new IController(this);
            //IController con = new IController();
            //SetController(con);
            
            this.statusTimeLabel.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            this.toolStripStatusTx.Text = "Sent: 0";
            this.toolStripStatusRx.Text = "Received: 0";
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            ////视频////
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count != 0)
            {
                foreach (FilterInfo device in videoDevices)
                {
                    cmbCamera.Items.Add(device.Name);
                }
            }
            else
            {
                cmbCamera.Items.Add("没有找到摄像头");
            }

            cmbCamera.SelectedIndex = 0;
            
        }

        public void RePaint(HImage image)
        {
            if (image.IsInitialized())
            {

                int current_beginRow, current_beginCol, current_endRow, current_endCol;
                hWindow_Final1.hWindowControl.HalconWindow.GetPart(out current_beginRow, out current_beginCol, out current_endRow, out current_endCol);
                hWindow_Final1.Image = image;
                hWindow_Final1.hWindowControl.HalconWindow.SetPart(current_beginRow, current_beginCol, current_endRow, current_endCol);
                hWindow_Final1.hWindowControl.HalconWindow.DispObj(image);
            }
        }
        
        private void ReadDataFromEV()//读取数据
        {
            InitialData = SaveDataTool.FormBetyFile<MeasureRectangleData>(@"C:/detectionData/RectData.ev");//读取几个框子数据
            comData = SaveDataTool.FormBetyFile<ComData>(@"C:/detectionData/ComData.ev");
            

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            ////串口////
            
            ReadDataFromEV();//读取ev文件中的数据
            
            InitializeCOMCombox();
            //CurrentFrameImage = new HImage(@"../../Camera/0.png");
            CurrentFrameImage = new HImage("C:/detectionData/0.png");
            //显示背景图
            hWindow_Final1.HobjectToHimage(CurrentFrameImage);
            //注册窗口鼠标事件
            hWindow_Final1.hWindowControl.MouseUp += Hwindow_MouseUp;
            hWindow_Final1.hWindowControl.MouseDown += HWindowControl_MouseDown;
            hWindow_Final1.hWindowControl.MouseMove += HWindowControl_MouseMove;
            //form->det

            this.SetNewShapeModelEvent += controller.detection.resetShapeModelByHand;//使用给定的图片和轮廓来生成新的匹配模型
            this.SetCalibrationStateEvent += controller.detection.SetCalibrationState;//
            //this.GiveDetInitRectInfEvent += controller.detection.GetDetInitRectInfEvent;//因为框子数据是mainform读取的，为了保证数据一致性，所以要发给det，而不让它自己读取
            //com->form
            controller. comModel.comOpenEvent += this.OpenComEvent;
            controller.comModel.comCloseEvent += this.CloseComEvent;
            controller.comModel.comReceiveDataEvent += this.ComReceiveDataEvent;

            //det->form
            controller.detection.StopBtnChangeEvent+=this.StopBtnChangeEvent;
            //form->com
            this.GiveComDataEvent += controller.comModel.GiveComDataEvent;

            this.region = new ViewWindow.Model.ROI();
            isExist = false;//场上只能有一个画出来的线

        }
        private void StopBtnChangeEvent(bool statue)//statue就是det中的errorStop的值
        {

            if (this.InvokeRequired)//this是指MainForm
            {
                Invoke(new Action<bool>(StopBtnChangeEvent),statue);
                return;
            }
            if (statue==true)//暂停一下
            {
                btn_All.Text = "启动";
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DisConnect();//相机关闭
            //if (openCloseSpbtn.Text != "Open")
            //  controller.CloseSerialPort();//串口关闭
            //Application.ExitThread();
            if (cameraState != CameraState.关闭连接)
            {
                cameraTask.Abort();
            }
            System.Environment.Exit(0);
        }

        public void ComCamBothOpenEvent()
        {
            if (this.InvokeRequired)//this是指MainForm
            {
                Invoke(new Action(ComCamBothOpenEvent));
                return;
            }
            if (controller.detection.errorStop==false)//正常运行中
               btn_All.Text = "暂停";
        }
        public void ComCamBothCloseEvent()
        {
            if (this.InvokeRequired)//this是指MainForm
            {
                Invoke(new Action(ComCamBothCloseEvent));
                return;
            }
            //if (controller.detection.errorStop ==true)
            btn_All.Text = "启动";
        }
        /// <summary>
        /// update status bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 串口打开事件
        public void OpenComEvent(Object sender, SerialPortEventArgs e)//e是自定义的，有isOpend：bool和receivedBytes：byte[]
        {
            if (this.InvokeRequired)//this是指MainForm
            {
                Invoke(new Action<Object, SerialPortEventArgs>(OpenComEvent), sender, e);
                return;
            }

            if (e.isOpend)  //Open successfully
            {
                openCloseSpbtn.Text = "Close";
                statuslabel.Text =  comConfigureInf.ArrayComPortsNames[comConfigureInf.comData.ArrayComPortsNamesChoose] + " Opend"; 
                messageBox.AppendText("通信串口打开成功\r\n");
                comStatue = true;
                if(camStatue==true)
                {
                    ComCamBothOpenEvent();
                }
            }
            else    //Open failed
            {
                statuslabel.Text = "Open failed !";   
                messageBox.AppendText("通信串口打开失败\r\n");
                comStatue = false;
            }
        }

        /// <summary>
        /// update status bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 关闭串口事件
        public void CloseComEvent(Object sender, SerialPortEventArgs e)
        {
            if (this.InvokeRequired)
            {
                Invoke(new Action<Object, SerialPortEventArgs>(CloseComEvent), sender, e);
                return;
            }

            if (!e.isOpend) //close successfully
            {
                openCloseSpbtn.Text = "Open";
                statuslabel.Text = comConfigureInf.ArrayComPortsNames[comConfigureInf.comData.ArrayComPortsNamesChoose] + " Closed";
                messageBox.AppendText("通信串口已关闭\r\n");
                comStatue = false;
                if (camStatue == false)
                {
                    ComCamBothCloseEvent();
                }

            }
        }

        /// <summary>
        /// Display received data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 串口收到数据事件
        public void ComReceiveDataEvent(object sender, SerialPortEventArgs e)
        {

            /*C#中禁止跨线程直接访问控件，InvokeRequired是为了解决这个问题而产生的，
            当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它。
            此时它将会在内部调用new MethodInvoker(LoadGlobalImage)来完成下面的步骤，
            这个做法保证了控件的安全，你可以这样理解，有人想找你借钱，他可以直接在你的钱包中拿，这样太不安全，
            因此必须让别人先要告诉你，你再从自己的钱包把钱拿出来借给别人，这样就安全了*/

            if (this.InvokeRequired)//如果InvokeRequired==true表示其它线程需要访问控件，那么调用invoke来转给控件owner处理。
            {
                try
                {
                    Invoke(new Action<Object, SerialPortEventArgs>(ComReceiveDataEvent), sender, e);
                }
                catch (System.Exception)
                {
                    //disable form destroy exception
                }
                return;
            }

            //update status bar
            receiveBytesCount += e.receivedBytes.Length;
            toolStripStatusRx.Text = "Received: " + receiveBytesCount.ToString();


        }


        private void HWindowControl_MouseDown(object sender, MouseEventArgs e)
        {
            int index;

            List<HTuple> data;
            ViewWindow.Model.ROI roi = hWindow_Final1.viewWindow.smallestActiveROI(out data, out index);//可以被激活的roi对象

            if (index > -1)//如果选中了对象
            {
                string name = roi.GetType().Name;//得到最后一次选中的对象名字

                this.region = roi;//将roi存到列表中？更新
            }

            if (draw == false)//这个鼠标按下函数只是先设一个位置，具体拖拽要在ROIController中的鼠标按下函数中实现
                return;
            //先将之前的那个清空
            if (isExist == true)
            {
                for (int i = hWindow_Final1.viewWindow.gerROIListNum() - 1; i >= 0; i--)
                {
                    hWindow_Final1.viewWindow.removeROI(i);
                }
 
                isExist = false;
            }

            hWindow_Final1.hWindowControl.HalconWindow.GetMposition(out int y, out int x, out int b);//鼠标点击的位置

 
            if (b == 4)//按下右键立即结束
            {
                draw = false;
                return;
            }

            switch (style)
            {
                case 0://点
                    hWindow_Final1.viewWindow._roiController.displayPoint("blue", y, x);
                    break;

                case 1://直线
                    hWindow_Final1.viewWindow._roiController.displayLine("blue", y, x - 20, y, x + 20);
                    break;

                case 2://圆
                    hWindow_Final1.viewWindow._roiController.displayCircle("blue", y, x, 60.0);
                    break;
                case 3://矩形
                       // hWindow_Final1.viewWindow.genLine(y, x - 20, y, x + 20, ref this.region1s);
                    hWindow_Final1.viewWindow._roiController.displayRect1("blue", y - 50, x - 50, y + 50, x + 50);
                    break;
                case 4://角度矩形
                    hWindow_Final1.viewWindow._roiController.displayRect2("blue", y, x, 30.0 / 180.0 * Math.PI, 60.0, 30.0);
                    break;
                case 5://扇形
                    hWindow_Final1.viewWindow._roiController.displayCircularArc("blue", y, x, 40.0, 0, -3.14, "negative");
                    break;
                case 6://同心双黄矩形（内部角度矩形）

                    hWindow_Final1.viewWindow._roiController.displayRect2("blue", y, x, 0, 60.0, 30.0);
                    hWindow_Final1.viewWindow._roiController.displayRect1("blue", y - 50, x - 50, y + 50, x + 50);


                    break;
                    // default:
                    // draw = false;//退出
                    //return;


            }
            //hWindow_Final1.viewWindow._roiController.displayLine("blue", y, x - 20, y, x + 20);
            this.region.Color = "blue";
            isExist = true;
            /*
                        XStar_textBox.Text = (x - 20).ToString();
                        YStar_textBox.Text = y.ToString();
                        XEnd_textBox.Text = (x + 20).ToString();
                        YEnd_textBox.Text = y.ToString();
                        */

            draw = false;//画完也结束
        }
        /// <summary>
        /// 注册haclon窗体的鼠标弹起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hwindow_MouseUp(object sender, MouseEventArgs e)//没有在里面写代码，写到down事件下了
        {

        }
        HImage GrayImage;
         
        int grayVal=0;
 
        bool needGrabGrayVal = false;

        int MouseY=0, MouseX=0, b;
        int i = 0;
        private void HWindowControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (needGrabGrayVal == true)
            {
                try
                {
                    hWindow_Final1.hWindowControl.HalconWindow.GetMposition(out MouseY, out MouseX, out b);//鼠标点击的位置  
                    grayVal = GrayImage.GetGrayval(MouseY, MouseX);
                    //cmbCamera.Text = MouseY.ToString() + "+++++" + MouseX.ToString();
                    SetDataToChildFormEvent(grayVal);
                }
               catch
                {
           
                }
                   
                   
                //hWindow_Final1.hWindowControl.HalconWindow.GetMposition(out MouseY, out MouseX, out int b);//鼠标点击的位置      
 
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

        public void SetModelByHand()//让frm_Unit_Measure调用，它点击生成匹配模型后调用（矩形框就是主窗体生成的，位置信息已经有了）
        {

            //HRegion MatchRegion = new HRegion(row1, col1, row2, col2);//大的匹配框子

            // int roiRegion = MatchRegion.AreaCenter(out HTuple x, out HTuple y);
            //HRegion DetRegion = new HRegion();
            // DetRegion.GenRectangle2(DetCol1, DetRow1, DetAngle, Detlen1, Detlen2 );//小的检测框子
            //hWindow_Final1.ClearWindow();//清屏
           

            //  otherParam.com = comConfigureInf.ArrayComPortsNamesChoose;
            GiveComDataEvent(InitialData.otherRectParam.delayRubbishDetectionTime  , InitialData.otherRectParam.delayRollerDetectionTime);
            SetNewShapeModelEvent(InitialData, CurrentFrameImage);//大框，小框和当前图片
        }


        //由点击按钮事件，触发ComModel中的事件，再传回主函数，以实现界面和逻辑分离
        //所有下面这些函数只需要改变控件界面，如串口打开事件：现在串口开了，是谁干的不重要，
        //我只要将按钮上的文字和其他界面改为对应的就行了//

        ////////////////////////////////////界面事件///////////////////////////////////////////

        #region ////////////////////////////////相机//////////////////////////////////////

        /// <summary>
        /// //检测方要求截图过去事件
        /// </summary>
        /// <returns></returns>
        public HObject RequestShootPicEvent()
        {

            //要截图给detection去检测
            if (cameraState == CameraState.正常连接)
            {
                sema.WaitOne();
                shootImg = CurrentFrameImage.Clone(); ;
                sema.Release();
 
                return shootImg;
                
            }
            else
            {
                //return CurrentFrameImage;//调试时使用，之后删除
                return null;
            }
               // return null;



        }
        
 

        long  thingNum = 0;
        bool detectionRoller = false;
        /// <summary>
        /// //检测结果事件,在屏幕上显示结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="isQualified"></param>
        public void DetectQualifiedEvent(Object sender, DetectionResultEventArgs e)
        {


            if (this.InvokeRequired)//this是指MainForm
            {
                BeginInvoke(new Action<Object, DetectionResultEventArgs>(DetectQualifiedEvent), sender, e);

                //messageBox.AppendText(i.ToString());
                return;

            }
            else
            {
                if (e.isQualified == true)//检测成功
                {
                    messageBox.AppendText("检测成功\r\n");
                    lab_light.ForeColor = Color.Green;
                     //controller.SendDataToCom(OpenQualifiedRelay);//向串口发信号
                    
                }
                else//检测失败
                {
                    messageBox.AppendText("检测失败\r\n");
                    lab_light.ForeColor = Color.Red;
  
                   // controller.SendDataToCom(OpenUnqualifiedRelay);//向串口发信号
                }
                ////////显示结果////////////
                //hWindow_Final1.viewWindow.ClearObj();//有时候会出错
                //显示部分让相机线程做
                frameNum = 15;//显示20帧
                ShapeModelContour= e.ShapeModelContour ;
                xldCrossPoint=e.xldCrossPoint ;
                ROI_R_AfterTrans = e.ROI_R_AfterTrans;
                BinaryRegion = e.BinaryRegion;

                detectionRoller=e.detectionRoller;
                drawResultObj();
                messageBox.AppendText("####" +thingNum.ToString()+ "####" + "\r\n");
                if(e.detectionRoller==true)
                {
                    messageBox.AppendText("正在检测——轮子"  + "\r\n");
                    if(e.numMiss>0)
                        messageBox.AppendText("第"+ e.numMiss+ "次缺料"+ "\r\n");
                }
                else
                {
                    messageBox.AppendText("正在检测——杂物" + "\r\n");
                }
                if(e.isNotice==true)
                {
                    messageBox.AppendText("已经通知机器停止" + "\r\n");
                }
                messageBox.AppendText("总像素" + e.pixieTotalNum.ToString()+"\r\n");
                messageBox.AppendText("所占像素" + e.pixieOfRoller.ToString() + "\r\n");
                messageBox.AppendText("像素比"+((double)e.pixieOfRoller/(double) e.pixieTotalNum).ToString() + "\r\n\r\n");
                thingNum++;
            }

        }

        #endregion


        #region////////////////////////////////串口//////////////////////////////////////
        public void SetController(IController controller)
        {
            this.controller = controller;
        }

        /// <summary>
        /// Initialize serial port information
        /// </summary>
        private void InitializeCOMCombox()
        {
            comConfigureInf = new ComConfigureInf();//填充串口信息列表
            if(comData==null)
            {
                comData = new ComData(0,1,1, 0,0, 0);
            }
            comConfigureInf.comData = comData;

            int[] BaudRatemArr = { 4800, 9600, 19200, 38400, 57600, 115200 };
            comConfigureInf.BaudRate = new List<int>(BaudRatemArr);

            int[] DataBitsArr = {7,8 };
            comConfigureInf.DataBits = new List<int>(DataBitsArr);

            string[] StopBitsArr = { "One", "OnePointFive", "Two" };
            comConfigureInf.StopBits = new List<string>(StopBitsArr);

            string[] ParityArr = { "None", "Even", "Mark", "Odd", "Space" };
            comConfigureInf.Parity = new List<string>(ParityArr);

            string[] HandshakingsArr = { "None", "XOnXOff", "RequestToSend", "RequestToSendXOnXOff" };
            comConfigureInf.Handshakings = new List<string>(HandshakingsArr);


            comConfigureInf.ArrayComPortsNames = SerialPort.GetPortNames();
            if (comConfigureInf.ArrayComPortsNames.Length == 0)
            {
                statuslabel.Text = "No COM found !";
                openCloseSpbtn.Enabled = false;
            }
            else
            {
                openCloseSpbtn.Enabled = true;
            }
        }


        #endregion

        /////////////////////////////////measure///////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// 
        public void MeasureFormDrawEvent()
        {
             draw = true;
             style = 6;
        }
        public void MeasureFormDetectionEvent()
        {
            //SaveDataTool.Save2File(InitialData, @"C:/detectionData/RectData.ev");//存储一下
            SetModelByHand();
        }
        public void MeasureFormClosing()
        {
            for (int i =  hWindow_Final1.viewWindow.gerROIListNum() - 1; i >= 0; i--)
            {
                 hWindow_Final1.viewWindow.removeROI(i);
            }
            calibrationClose();
        }



        //////////////////////////////控件响应事件///////////////////////////////////////////
        #region//////////////////////////////相机控件///////////////////////////////////////////
        ////图像////
        bool camStatue = false;
        /// <summary>
        /// //相机打开事件
        /// </summary>
        /// <param name="status"></param>
        public void OpenVidieoEvent(bool status)
        {
             
            if (status == true)
            {
                camStatue = true;
                messageBox.AppendText("相机已经正常打开\r\n");
                if(comStatue == true)//串口也正常启动了
                {
                    ComCamBothOpenEvent();
                }
            }
            
            else
                messageBox.AppendText("相机打开失败\r\n");
        }
        /// <summary>
        /// //相机关闭事件
        /// </summary>
        /// <param name="status"></param>
        public void CloseVidieoEvent(bool status)
        {
            
            if (status == true)
            {
                camStatue = false;
                messageBox.AppendText("相机已经正常关闭\r\n");
                if (comStatue == false)//串口也正常关闭了
                {
                    ComCamBothCloseEvent();
                }
            }
               
            else
                messageBox.AppendText("相机关闭失败\r\n");
        }

        int frameNum = 0;//把图形画出来多少帧
        public void drawResultObj()
        {
            ////////显示结果////////////
            //hWindow_Final1.viewWindow.ClearObj();
            if(InitialData.otherRectParam.  lineWidth >0)
            {
                hWindow_Final1.viewWindow.SetLineWidth(InitialData.otherRectParam.lineWidth);
            }
   
            hWindow_Final1.DispObj(ShapeModelContour, "red");
            hWindow_Final1.DispObj(xldCrossPoint);


            hWindow_Final1.DispObj(ROI_R_AfterTrans, "blue");

            if (detectionRoller == true)
            {
                hWindow_Final1.DispObj(BinaryRegion, "cyan");
            }
            else
            {
                hWindow_Final1.DispObj(BinaryRegion, "spring green"); 
            }
           
            
        }

        public HXLDCont ShapeModelContour;//轮廓形状
        public HXLDCont xldCrossPoint;//轮廓中心位置
        public HXLDCont ROI_R_AfterTrans;//找轮子的矩形
        public HRegion BinaryRegion;//二值化轮子部分给大家看看

        /// <summary>
        /// 相机线程
        /// </summary>
        public void CameraThread()
        {
            HOperatorSet.GenEmptyObj(out CurrentFrameImage);//创建一个空的对象元组。          
            while (true)
            {
                
                if (cameraState==CameraState.正常连接)//启动状态
                {
                    sema.WaitOne();
                    if (CurrentFrameImage != null)
                        CurrentFrameImage.Dispose();
                    
                    if (ImageSource != null)
                        ImageSource.Dispose();
                    HOperatorSet.GrabImageAsync(out ImageSource, hwindow, -1);//异步从相机抓取图片
                    HOperatorSet.ZoomImageFactor(ImageSource, out CurrentFrameImage, 0.5, 0.5, "constant");
                    sema.Release();
                    
                    hWindow_Final1.HobjectToHimage(CurrentFrameImage);
     
                    if(frameNum >0)
                    {
                        drawResultObj();
                        frameNum--;
                    }
                    
                    Thread.Sleep(20);
                }
                else if (cameraState == CameraState.暂停)//启动状态// 暂停状态
                {
                    Thread.Sleep(80);
                }

            }
        }

        private void btnConnect_Click(object sender, EventArgs e)//视频连接按钮O
        {
          if(btnConnect.Text == "连接")
            {
                if (videoDevice != null)
                {
                    if(cameraTask==null)
                    {

                        //使用halcon打开
                        HOperatorSet.OpenFramegrabber("DirectShow",//string name
                            1, 1,//int horizontalResolution, int verticalResolution
                            0, 0,//int imageWidth, int imageHeight
                            0, 0,//int startRow, int startColumn,
                            "default",// string field
                            8, // HTuple bitsPerChannel
                            "rgb",// HTuple colorSpace
                            -1,// HTuple generic泛型
                            "false",//string externalTrigger外部触发
                            "default", //   HTuple cameraType
                            videoDeviceNum.ToString(),// HTuple device
                            0, // HTuple port
                            -1,//HTuple lineIn
                            out hwindow);// HTuple  acqHandle

                        HOperatorSet.GrabImageStart(hwindow, -1);//从指定的图像采集设备开始异步抓取。
                        HOperatorSet.GrabImageAsync(out ImageSource, hwindow, -1);
                        //HOperatorSet.GrabImage(out CurrentFrameImage, hwindow);
                        HOperatorSet.ZoomImageFactor(ImageSource, out CurrentFrameImage, 0.5, 0.5, "constant");
                        HOperatorSet.GetImageSize(CurrentFrameImage, out width, out height);
                        HOperatorSet.SetPart(hWindow_Final1.hWindowControl.HalconWindow, 0, 0, height, width);

                       

                        cameraTask = new Thread(CameraThread);
                        cameraTask.Start();
                    }




                    cameraState = CameraState.正常连接;
                    hWindow_Final1.DrawModel = true;//禁止缩放
                    EnableControlStatus(false);
                    OpenVidieoEvent(true);
                    btnConnect.Text = "断开连接";

                }

                else
                    OpenVidieoEvent(false);
            }
          else if(btnConnect.Text == "断开连接")
            {
                //DisConnect();
                cameraState = CameraState.暂停;
                EnableControlStatus(true);//相机按钮
                btnConnect.Text = "连接";
                //cameraTask.Abort();
                CloseVidieoEvent(true);//告诉大家相机关闭了

            }
            
        }
        int videoDeviceNum = 0;
        private void cmbCamera_SelectedIndexChanged(object sender, EventArgs e)//相机选择
        {
            if (videoDevices.Count != 0)
            {
                videoDeviceNum = cmbCamera.SelectedIndex;
                videoDevice = new VideoCaptureDevice(videoDevices[cmbCamera.SelectedIndex].MonikerString);
 
            }
        }

        private void EnableControlStatus(bool status)
        {
            cmbCamera.Enabled = status;
             
            //btnConnect.Enabled = status;    
            //btnDisconnect.Enabled = !status;
        }
 

        private void DisConnect()//这个是关闭视频连接，不是暂停
        {
            cameraTask.Abort();
            HOperatorSet.CloseFramegrabber(hwindow);
        }

        private void btnShoot_Click(object sender, EventArgs e)//点击截图按钮
        {

            Bitmap img = null;
            System.DateTime currentTime = new DateTime();
            currentTime = System.DateTime.Now;
            int hour = currentTime.Hour;
            int second = currentTime.Second;
            int weight, height;
            height = img.Size.Height;
            weight = img.Size.Height;
            string picName = @"E:\" + hour + second + "x" + height + "x" + weight + ".jpg";
            img.Save(picName);
        }

        public void SendInfToMainFormEvent(MeasureRectangleData data)//measure窗体传一些数据过来
        {
            this.InitialData = data;
        }
        public void calibrationClose()//相机标定关闭时调用
        {
            hWindow_Final1.viewWindow._roiController.notifyCurrentRoiRectangle1PositionEvent -= m_frm_Unit.GetRect1PosFromMainForm;//将主窗体画的线/矩形的信息告诉子窗体
            hWindow_Final1.viewWindow._roiController.notifyCurrentRoiRectangle2PositionEvent -= m_frm_Unit.GetRect2PosFromMainForm;
            this.SetDataToChildFormEvent -= m_frm_Unit.GetGrayValFromMainForm;


            cameraState = CameraState.正常连接;
            //Mainform.SetInitParam();//让detection设置多少百分比像素才成功
            hWindow_Final1.DrawModel = true;//禁止缩放
            //openCloseCom();//打开串口
            if (SetCalibrationStateEvent != null)
                SetCalibrationStateEvent(false);//不操作串口了，直接让检测部分不要检测
            btn_calibration.Enabled = true;
            needGrabGrayVal = false;
        }
        #endregion

        #region//////////////////////////////串口控件///////////////////////////////////////////
        public bool comStatue=false;//true表示当前是开启的状态，false是关闭
        public void openCloseCom()//打开串口之前，先设置好form的全局变量 comConfigureInf中的列表和所选编号
        {

            SaveDataTool.Save2File(comConfigureInf.comData, @"C:/detectionData/ComData.ev");//存储一下
            if (comConfigureInf.ArrayComPortsNames.Length > comConfigureInf.comData.ArrayComPortsNamesChoose)
            {
                controller.OpenSerialPort(comConfigureInf.ArrayComPortsNames[comConfigureInf.comData.ArrayComPortsNamesChoose],
             comConfigureInf.BaudRate[comConfigureInf.comData.BaudRateChoose].ToString(),
             comConfigureInf.DataBits[comConfigureInf.comData.DataBitsChoose].ToString(),
             comConfigureInf.StopBits[comConfigureInf.comData.StopBitsChoose],
             comConfigureInf.Parity[comConfigureInf.comData.ParityChoose],
             comConfigureInf.Handshakings[comConfigureInf.comData.HandshakingsChoose]);
            }
            else
            {
                messageBox.AppendText("所选通信串口不存在，请重新选择\r\n");
            }


        }

        private void openCloseSpbtn_Click(object sender, EventArgs e)
        {
            if (openCloseSpbtn.Text == "Open")
            {
                openCloseCom();
                
            }
            else if (openCloseSpbtn.Text == "Close")
            {
                controller.CloseSerialPort();
            }
        }

        private void refreshbtn_Click(object sender, EventArgs e)
        {
           

        }


        public void sendInf(String sendText)
        {
            bool flag = false;
            Byte[] bytes = IController.Hex2Bytes(sendText);
            flag = controller.SendDataToCom(bytes);
            sendBytesCount += bytes.Length;

            if (flag)
            {
                statuslabel.Text = "Send OK !";
            }
            else
            {
                statuslabel.Text = "Send failed !";
            }
            //update status bar
            toolStripStatusTx.Text = "Sent: " + sendBytesCount.ToString();
        }

   
        //更新状态
        private void statustimer_Tick(object sender, EventArgs e)
        {
            this.statusTimeLabel.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            //this.statuslabel.Text = hWindow_Final1.viewWindow._hWndControl.startX.ToString();
            //this.toolStripStatusRx.Text = hWindow_Final1.viewWindow._hWndControl.startY.ToString();
        }



        public void ConfigureComClose()//设定串口界面关闭时要把委托全卸掉
        {
            controller.comModel.comOpenEvent -= m_frm_Com.OpenComEvent;
            controller.comModel.comCloseEvent -= m_frm_Com.CloseComEvent;
            controller.comModel.comReceiveDataEvent -= m_frm_Com.ComReceiveDataEvent;
        }
        private void btn_ConfigureCom_Click(object sender, EventArgs e)
        {
            
            m_frm_Com = new frm_Com(this, comConfigureInf.comData);//Rectangle

            //com->form
            controller.comModel.comOpenEvent += m_frm_Com.OpenComEvent;
            controller.comModel.comCloseEvent += m_frm_Com.CloseComEvent;
            controller.comModel.comReceiveDataEvent += m_frm_Com.ComReceiveDataEvent;

            //hWindow_Final1.viewWindow._roiController.notifyCurrentRoiLinePositionEvent += m_frm_Unit.setCurrentLinePositionToForm;//将主窗体画的线/矩形的信息告诉子窗体
            //hWindow_Final1.viewWindow._roiController.notifyCurrentRoiRectanglePositionEvent += m_frm_Unit.setCurrentRectanglePositionToForm;//将主窗体画的线/矩形的信息告诉子窗体
            m_frm_Com.Show(this);
        }

        private void sendbtn_Click(object sender, EventArgs e)
        {

        }

        private void comGroup_Enter(object sender, EventArgs e)
        {

        }

      
        private void button1_Click(object sender, EventArgs e)
        {
            if (btn_All.Text == "启动")
            {
                if(btnConnect.Text == "连接")//若相机还没打开，就调用打开按钮
                {
                    btnConnect_Click(sender, e);//打开相机
                }
                if (openCloseSpbtn.Text == "Open")//串口同理
                {
                    openCloseSpbtn_Click(sender, e);//打开串口
                }
                btn_All.Text = "暂停";
                controller.detection.errorStop = false;


            }
            else if (btn_All.Text == "暂停")
            {

                //if (btnConnect.Text == "断开连接")//若相机打开，就调用关闭按钮
                //{
                //    btnConnect_Click(sender, e);//打开相机
               // }
                if (openCloseSpbtn.Text == "Close")//串口同理
                {
                    openCloseSpbtn_Click(sender, e);//关闭串口
                }
                btn_All.Text = "启动";
                controller.detection.errorStop = true;
            }

        }

        private void btn_calibration_Click(object sender, EventArgs e)//点击标定按钮
        {
            cameraState = CameraState.暂停;//视频暂停
            if(SetCalibrationStateEvent!=null)
            SetCalibrationStateEvent(true);//不操作串口了，直接让检测部分不要检测
                                           //if(openCloseSpbtn.Text=="Close")
                                           //   controller.CloseSerialPort();//如果串口是开启状态，就关闭串口，免得这时候收到检测的消息
            hWindow_Final1.DrawModel =false;//允许缩放，编辑


 

            m_frm_Unit = new  frm_Unit_Measure  (this,InitialData);//Rectangle
            hWindow_Final1.viewWindow._roiController.notifyCurrentRoiRectangle1PositionEvent += m_frm_Unit.GetRect1PosFromMainForm;//将主窗体画的线/矩形的信息告诉子窗体
            hWindow_Final1.viewWindow._roiController.notifyCurrentRoiRectangle2PositionEvent += m_frm_Unit.GetRect2PosFromMainForm;
            this.SetDataToChildFormEvent += m_frm_Unit.GetGrayValFromMainForm;
 
            btn_calibration.Enabled = false;//不允许点2次这个按钮
            m_frm_Unit.Show(this);
            if(InitialData.rectangle1Inf==null||InitialData.rectangle2Inf==null||InitialData.otherRectParam==null)//什么都没有，画不出来，就不画
            {
                messageBox.AppendText("标定框无数据，无法显示\r\n");
                return;
            }
            if(InitialData.rectangle1Inf.mainRow2 - InitialData.rectangle1Inf.mainRow1 == 0 || InitialData.rectangle1Inf.mainColumn2 - InitialData.rectangle1Inf.mainColumn1 == 0 || InitialData.rectangle2Inf.Detlen1 == 0|| InitialData.rectangle2Inf.Detlen2 == 0)//框子没大小，也不画
            {
                messageBox.AppendText("标定框大小为0，无法显示\r\n");
                return;
            }
            hWindow_Final1.viewWindow._roiController.displayRect2("blue", InitialData.rectangle2Inf .DetRow, InitialData.rectangle2Inf.DetCol, InitialData.rectangle2Inf.DetAngle, InitialData.rectangle2Inf.Detlen1, InitialData.rectangle2Inf.Detlen2);//小框子 角度0.74
            hWindow_Final1.viewWindow._roiController.displayRect1("blue", InitialData.rectangle1Inf.  mainRow1, InitialData.rectangle1Inf.mainColumn1, InitialData.rectangle1Inf.mainRow2, InitialData.rectangle1Inf.mainColumn2);
            isExist = true;//已经存在了roi
            if(CurrentFrameImage!=null)
            {
                try
                {
                    HImage currentImage = HObject2HImage1(CurrentFrameImage);
                    GrayImage = currentImage.Rgb1ToGray();
                    needGrabGrayVal = true;
                }
                catch(Exception )
                {

                }
              
            }
          
        }
        
 

        private void cmbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }


    }
    #endregion



}
