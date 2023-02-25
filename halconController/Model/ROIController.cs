using System;
using HalconDotNet;
using System.Collections;
using ChoiceTech.Halcon.Control;
using SharedData;
namespace ViewWindow.Model
{

    public delegate void FuncROIDelegate();

    /// <summary>
    /// 此类创建和管理 ROI 对象
    /// 它使用鼠标向下操作和鼠标移动操作的方法响应鼠标设备输入。
    /// 构建自己的 C# 项目时，不必详细了解此类。
    /// 但是，如果要在应用程序中使用交互式 ROI，则必须考虑一些事项：ROIController 和 HWndCtrl 类之间存在相当密切的连接，
    /// 这意味着您必须使用 HWndCtrl"注册"ROIController，因此 HWndCtrl 知道必须将用户输入（如鼠标事件）转发到 ROIController 类。
    /// ROI 对象的可视化和操作由 ROI 控制器完成。
    ///此类通过从 ROI 列表中计算模型区域，为匹配的应用程序提供特殊支持。为此，根据它们的标志添加和减去 ROI。
	/// </summary>
	public class ROIController
    {
        public event RoiLinePositionHandler notifyCurrentRoiLinePositionEvent = null;//向主窗体通知现在直线的首尾坐标
        public event RoiRectangle1PositionHandler notifyCurrentRoiRectangle1PositionEvent = null;//向主窗体通知现在直线的首尾坐标
        public event RoiRectangle2PositionHandler notifyCurrentRoiRectangle2PositionEvent = null;//向主窗体通知现在直线的首尾坐标
        public bool EditModel = true;

        /// <summary>
        /// Constant for setting the ROI mode: positive ROI sign.
        /// </summary>
        public const int MODE_ROI_POS = 21;

        /// <summary>
        /// Constant for setting the ROI mode: negative ROI sign.
        /// </summary>
        public const int MODE_ROI_NEG = 22;

        /// <summary>
        /// Constant for setting the ROI mode: no model region is computed as
        /// the sum of all ROI objects.
        /// </summary>
        public const int MODE_ROI_NONE = 23;

        /// <summary>Constant describing an update of the model region</summary>
        public const int EVENT_UPDATE_ROI = 50;

        public const int EVENT_CHANGED_ROI_SIGN = 51;

        /// <summary>Constant describing an update of the model region</summary>
        public const int EVENT_MOVING_ROI = 52;

        public const int EVENT_DELETED_ACTROI = 53;

        public const int EVENT_DELETED_ALL_ROIS = 54;

        public const int EVENT_ACTIVATED_ROI = 55;

        public const int EVENT_CREATED_ROI = 56;


        public ROI roiMode;
        private int stateROI;
        private double currX, currY;


        /// <summary>
        /// 被激活的roi的id，最多只能有一个被激活
        /// </summary>     
        public int activeROIidx;
        public int deletedIdx;

        /// <summary>List containing all created ROI objects so far</summary>
        public ArrayList ROIList;

        /// <summary>
        /// Region obtained by summing up all negative 
        /// and positive ROI objects from the ROIList 
        /// </summary>
        public HRegion ModelROI;

        private string activeCol = "green";
        private string activeHdlCol = "red";
        private string inactiveCol = "yellow";

        /// <summary>
        /// Reference to the HWndCtrl, the ROI Controller is registered to
        /// </summary>
        public HWndCtrl viewController;

        /// <summary>
        /// 委托：通知在模型区域中所做的更改
        /// </summary>
        public IconicDelegate NotifyRCObserver;

        public int gerROIListNum()
        {
            return ROIList.Count;
        }
        /// <summary>Constructor</summary>
        protected internal ROIController()
        {
            stateROI = MODE_ROI_NONE;
            ROIList = new ArrayList();
            activeROIidx = -1;
            ModelROI = new HRegion();
            NotifyRCObserver = new IconicDelegate(dummyI);
            deletedIdx = -1;
            currX = currY = -1;
        }

        /// <summary>Registers the HWndCtrl to this ROIController instance</summary>
        public void setViewController(HWndCtrl view)
        {
            viewController = view;
        }

        /// <summary>Gets the ModelROI object</summary>
        public HRegion getModelRegion()
        {
            return ModelROI;
        }

        /// <summary>Gets the List of ROIs created so far</summary>
        public ArrayList getROIList()
        {
            return ROIList;
        }

        /// <summary>Get the active ROI</summary>
        public ROI getActiveROI()
        {
            try
            {
                if (activeROIidx != -1)
                    return ((ROI)ROIList[activeROIidx]);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int getActiveROIIdx()
        {
            return activeROIidx;
        }

        public void setActiveROIIdx(int active)
        {
            activeROIidx = active;
        }

        public int getDelROIIdx()
        {
            return deletedIdx;
        }

        /// <summary>
        /// 要创建新的 ROI 对象，应用程序类将初始化"种子"ROI 实例并将其传递给 ROIController。
        /// ROIController 现在通过操作此新的 ROI 实例来响应。
        /// </summary>
        /// <param name="r">
        /// 'Seed' ROI object forwarded by the application forms class.
        /// </param>
        public void setROIShape(ROI r)
        {
            roiMode = r;
            roiMode.setOperatorFlag(stateROI);

        }


        /// <summary>
        /// Sets the sign of a ROI object to the value 'mode' (MODE_ROI_NONE,
        /// MODE_ROI_POS,MODE_ROI_NEG)
        /// </summary>
        public void setROISign(int mode)
        {
            stateROI = mode;

            if (activeROIidx != -1)
            {
                ((ROI)ROIList[activeROIidx]).setOperatorFlag(stateROI);
                viewController.repaint();
                NotifyRCObserver(ROIController.EVENT_CHANGED_ROI_SIGN);
            }
        }


        /// <summary>
        /// Calculates the ModelROI region for all objects contained 
        /// in ROIList, by adding and subtracting the positive and 
        /// negative ROI objects.
        /// </summary>
        public bool defineModelROI()
        {
            HRegion tmpAdd, tmpDiff, tmp;
            double row, col;

            if (stateROI == MODE_ROI_NONE)
                return true;

            tmpAdd = new HRegion();
            tmpDiff = new HRegion();
            tmpAdd.GenEmptyRegion();
            tmpDiff.GenEmptyRegion();

            for (int i = 0; i < ROIList.Count; i++)
            {
                switch (((ROI)ROIList[i]).getOperatorFlag())
                {
                    case ROI.POSITIVE_FLAG:
                        tmp = ((ROI)ROIList[i]).getRegion();
                        tmpAdd = tmp.Union2(tmpAdd);
                        break;
                    case ROI.NEGATIVE_FLAG:
                        tmp = ((ROI)ROIList[i]).getRegion();
                        tmpDiff = tmp.Union2(tmpDiff);
                        break;
                    default:
                        break;
                }//end of switch
            }//end of for

            ModelROI = null;

            if (tmpAdd.AreaCenter(out row, out col) > 0)
            {
                tmp = tmpAdd.Difference(tmpDiff);
                if (tmp.AreaCenter(out row, out col) > 0)
                    ModelROI = tmp;
            }

            //in case the set of positiv and negative ROIs dissolve 
            if (ModelROI == null || ROIList.Count == 0)
                return false;

            return true;
        }


        /// <summary>
        /// Clears all variables managing ROI objects
        /// </summary>
        public void reset()
        {
            ROIList.Clear();
            activeROIidx = -1;
            ModelROI = null;
            roiMode = null;
            NotifyRCObserver(EVENT_DELETED_ALL_ROIS);
        }


        /// <summary>
        /// Deletes this ROI instance if a 'seed' ROI object has been passed
        /// to the ROIController by the application class.
        /// 
        /// </summary>
        public void resetROI()
        {
            activeROIidx = -1;
            roiMode = null;
        }

        /// <summary>Defines the colors for the ROI objects</summary>
        /// <param name="aColor">Color for the active ROI object</param>
        /// <param name="inaColor">Color for the inactive ROI objects</param>
        /// <param name="aHdlColor">
        /// Color for the active handle of the active ROI object
        /// </param>
        public void setDrawColor(string aColor,
                                   string aHdlColor,
                                   string inaColor)
        {
            if (aColor != "")
                activeCol = aColor;
            if (aHdlColor != "")
                activeHdlCol = aHdlColor;
            if (inaColor != "")
                inactiveCol = inaColor;
        }


        /// <summary>
        /// Paints all objects from the ROIList into the HALCON window
        /// </summary>
        /// <param name="window">HALCON window</param>
        public void paintData(HalconDotNet.HWindow window)
        {
            window.SetDraw("margin");
            window.SetLineWidth(1);

            if (ROIList.Count > 0)
            {
                //
                //window.SetColor(inactiveCol);

                window.SetDraw("margin");

                for (int i = 0; i < ROIList.Count; i++)
                {
                    window.SetColor(((ROI)ROIList[i]).Color);
                    window.SetLineStyle(((ROI)ROIList[i]).flagLineStyle);
                    ((ROI)ROIList[i]).draw(window);
                }

                if (activeROIidx != -1)
                {
                    window.SetColor(activeCol);
                    window.SetLineStyle(((ROI)ROIList[activeROIidx]).flagLineStyle);
                    ((ROI)ROIList[activeROIidx]).draw(window);

                    window.SetColor(activeHdlCol);
                    ((ROI)ROIList[activeROIidx]).displayActive(window);
                }
            }
        }

        /// <summary>
        ///"鼠标按下"后，如果roi需要创建且还没有被创建，原地创建一个新的roi，并将其添加到 ROI 列表。
        /// 如果 ROI已经存在，被点到后变成激活 ROI
        /// 同时设置这条直线activeHandleId，告诉是哪一个点（头中尾三部分）被选中（在distToClosestHandle中实现）
        /// </summary>
        /// <param name="imgX">鼠标事件的x坐标</param>
        /// <param name="imgY">鼠标事件的y坐标</param>
        /// <returns></returns>
        public int mouseDownAction(double imgX, double imgY)
        {
            int idxROI = -1;
            double max = 10000, dist = 0;
            double epsilon = 35.0;          //看看离哪个roi距离最近//maximal shortest distance to one of
                                            //the handles

            if (roiMode != null)             //如果需要创建一个roi ，因为要先向roiMode中传入一个种子，比如roiLine，所以只要不为空，就是要创建
            {
                roiMode.createROI(imgX, imgY);//鼠标位置创建一个roi
                ROIList.Add(roiMode);//加入列表

                roiMode = null;//roiMode = null表示不用创建了，“种子”用掉了
                activeROIidx = ROIList.Count - 1;//最新的这个roi的id
                viewController.repaint();

                NotifyRCObserver(ROIController.EVENT_CREATED_ROI);//告诉别人，这里创建了一个roi
            }
            else if (ROIList.Count > 0)     //  正在操控一个已经存在的roi 
            {
                activeROIidx = -1;
                //每个roi控件都来和当前这一点比较距离，找到最近的距离，存入max
                //对应roi的id，存入idxROI
                //（这个roi距离要小于阈值epsilon：35才行）
                for (int i = 0; i < ROIList.Count; i++)
                {
                    dist = ((ROI)ROIList[i]).distToClosestHandle(imgX, imgY);
                    if ((dist < max) && (dist < epsilon))
                    {
                        max = dist;
                        idxROI = i;
                    }
                }//end of for

                //如果找到了
                if (idxROI >= 0)
                {
                    activeROIidx = idxROI;//设为激活roi
                    NotifyRCObserver(ROIController.EVENT_ACTIVATED_ROI);//告诉别人，这里激活了一个roi
                }

                viewController.repaint();
            }
            return activeROIidx;//鼠标点击之后，返回激活的roi的id
        }

        /// <summary>
        /// roi响应鼠标移动事件: 移动激活的roi中的元素，
        /// 移动哪个roi：activeROIidx
        /// 移动这个roi的哪个点：activeHandleIdx，这2个id存在ROI类中
        /// 因为activeHandleIdx是全局的变量，所以这里不需要传入指定的roi，在
        /// </summary>
        /// <param name="newX">x coordinate of mouse event</param>
        /// <param name="newY">y coordinate of mouse event</param>
        public void mouseMoveAction(double newX, double newY)
        {
            try
            {
                if (EditModel == false) return;//如果禁止编辑就什么都不做
                if ((newX == currX) && (newY == currY))//如果鼠标没有移动，也什么都不做
                    return;

                ((ROI)ROIList[activeROIidx]).moveByHandle(newX, newY);
                //可以通过改变activeHandleIdx，对直线的某一端点（就3点）单独移动     //0:  first end point      1:  last end point       2:  midpoint
                //在鼠标down的时候就已经指定好了是哪个roi的哪个部分
                viewController.repaint();//每移动一点就重绘一次
                currX = newX;
                currY = newY;
                NotifyRCObserver(ROIController.EVENT_MOVING_ROI);//告诉别人这里发生了roi移动事件
            }
            catch (Exception)
            {
                //没有显示roi的时候 移动鼠标会报错
            }

        }

        /***********************************************************/
        public void dummyI(int v)
        {
        }

        /*****************************/
        /// <summary>
        /// 在指定位置显示ROI--Rectangle1
        /// </summary>
        /// <param name="row1"></param>
        /// <param name="col1"></param>
        /// <param name="row2"></param>
        /// <param name="col2"></param>
        /// <param name="rois"></param>
        public void displayRect1(string color, double row1, double col1, double row2, double col2)
        {
            setROIShape(new ROIRectangle1());//生成矩形1的种子

            if (roiMode != null)			 //either a new ROI object is created
            {
                roiMode.notifyCurrentRoiRectangle1PositionEvent += notifyCurrentRoiRectangle1PositionEvent;

                roiMode.createRectangle1(row1, col1, row2, col2);
                roiMode.Type = roiMode.GetType().Name;
                roiMode.Color = color;
                ROIList.Add(roiMode);
                roiMode = null;
                activeROIidx = ROIList.Count - 1;
                viewController.repaint();

                Rectangle1Inf rect1 = new Rectangle1Inf();
                rect1.mainRow1 = row1;
                rect1.mainRow2 = row2;
                rect1.mainColumn1 = col1;
                rect1.mainColumn2 = col2;
                notifyCurrentRoiRectangle1PositionEvent(this, rect1);
                NotifyRCObserver(ROIController.EVENT_CREATED_ROI);
            }
        }
        /// <summary>
        /// 在指定位置显示ROI--Rectangle2
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="phi"></param>
        /// <param name="length1"></param>
        /// <param name="length2"></param>
        /// <param name="rois"></param>
        public void displayRect2(string color, double row, double col, double phi, double length1, double length2)
        {
            setROIShape(new ROIRectangle2());

            if (roiMode != null)			 //either a new ROI object is created
            {
                roiMode.notifyCurrentRoiRectangle2PositionEvent += notifyCurrentRoiRectangle2PositionEvent;

                roiMode.createRectangle2(row, col, phi, length1, length2);
                roiMode.Type = roiMode.GetType().Name;
                roiMode.Color = color;
                ROIList.Add(roiMode);
                roiMode = null;
                activeROIidx = ROIList.Count - 1;
                viewController.repaint();

                Rectangle2Inf rect2 = new Rectangle2Inf(row,col,phi,length1,length2);
            
                notifyCurrentRoiRectangle2PositionEvent(this, rect2);

                NotifyRCObserver(ROIController.EVENT_CREATED_ROI);
            }
        }
        /// <summary>
        /// 在指定位置生成ROI--Circle
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="radius"></param>
        /// <param name="rois"></param>
        public void displayCircle(string color, double row, double col, double radius)
        {
            setROIShape(new ROICircle());

            if (roiMode != null)			 //either a new ROI object is created
            {
                roiMode.createCircle(row, col, radius);
                roiMode.Type = roiMode.GetType().Name;
                roiMode.Color = color;
                ROIList.Add(roiMode);
                roiMode = null;
                activeROIidx = ROIList.Count - 1;
                viewController.repaint();

                NotifyRCObserver(ROIController.EVENT_CREATED_ROI);
            }
        }
        public void displayPoint(string color, double row, double col)
        {
            setROIShape(new ROIPoint());

            if (roiMode != null)			 //either a new ROI object is created
            {
                roiMode.createPoint(row, col);
                roiMode.Type = roiMode.GetType().Name;
                roiMode.Color = color;
                ROIList.Add(roiMode);
                roiMode = null;
                activeROIidx = ROIList.Count - 1;
                viewController.repaint();

                NotifyRCObserver(ROIController.EVENT_CREATED_ROI);
            }
        }
        public void displayCircularArc(string color, double row, double col, double radius, double startPhi, double extentPhi, string direct)
        {
            setROIShape(new ROICircularArc());

            if (roiMode != null)			 //either a new ROI object is created
            {
                roiMode.createCircularArc(row, col, radius, startPhi, extentPhi, direct);
                roiMode.Type = roiMode.GetType().Name;
                roiMode.Color = color;
                ROIList.Add(roiMode);
                roiMode = null;
                activeROIidx = ROIList.Count - 1;
                viewController.repaint();

                NotifyRCObserver(ROIController.EVENT_CREATED_ROI);
            }
        }
        public void displayCircularArc(string color, double row, double col, double radius, double startPhi, double extentPhi)
        {
            setROIShape(new ROICircularArc());

            if (roiMode != null)			 //either a new ROI object is created
            {
                roiMode.createCircularArc(row, col, radius, startPhi, extentPhi, "positive");
                roiMode.Type = roiMode.GetType().Name;
                roiMode.Color = color;
                ROIList.Add(roiMode);
                roiMode = null;
                activeROIidx = ROIList.Count - 1;
                viewController.repaint();

                NotifyRCObserver(ROIController.EVENT_CREATED_ROI);
            }
        }


        /// <summary>
        /// 在指定位置显示ROI--Line，和genLine差不多，但是这里主要是显示出来，所以没有返回roi列表
        /// </summary>
        /// <param name="beginRow"></param>
        /// <param name="beginCol"></param>
        /// <param name="endRow"></param>
        /// <param name="endCol"></param>
        /// <param name="rois"></param>
        public void displayLine(string color, double beginRow, double beginCol, double endRow, double endCol)
        {
            this.setROIShape(new ROILine());//传入“种子”roi，这里传一个ROILine，这样roiMode就不为空了

            if (roiMode != null)			 //一定不为空
            {
                roiMode.notifyCurrentRoiLinePositionEvent += notifyCurrentRoiLinePositionEvent;   
                
                roiMode.createLine(beginRow, beginCol, endRow, endCol);//ROILine.createLine，其实只是设置全局首尾点坐标
                roiMode.Type = roiMode.GetType().Name;
                roiMode.Color = color;
                ROIList.Add(roiMode);

                roiMode = null;
                activeROIidx = ROIList.Count - 1;
                viewController.repaint();

                NotifyRCObserver(ROIController.EVENT_CREATED_ROI);
            }
        }
        /// <summary>
        /// 在指定位置生成ROI--Rectangle1
        /// </summary>
        /// <param name="row1"></param>
        /// <param name="col1"></param>
        /// <param name="row2"></param>
        /// <param name="col2"></param>
        /// <param name="rois"></param>
        protected internal void genRect1(double row1, double col1, double row2, double col2, ref System.Collections.Generic.List<ROI> rois)
        {
            setROIShape(new ROIRectangle1());

            if (rois == null)
            {
                rois = new System.Collections.Generic.List<ROI>();
            }

            if (roiMode != null)			 //either a new ROI object is created
            {
                roiMode.createRectangle1(row1, col1, row2, col2);
                roiMode.Type = roiMode.GetType().Name;
                rois.Add(roiMode);
                ROIList.Add(roiMode);
                roiMode = null;
                activeROIidx = ROIList.Count - 1;
                viewController.repaint();

                NotifyRCObserver(ROIController.EVENT_CREATED_ROI);
            }
        }
        /// <summary>
        /// 在指定位置生成ROI--Rectangle2
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="phi"></param>
        /// <param name="length1"></param>
        /// <param name="length2"></param>
        /// <param name="rois"></param>
        protected internal void genRect2(double row, double col, double phi, double length1, double length2, ref System.Collections.Generic.List<ROI> rois)
        {
            setROIShape(new ROIRectangle2());

            if (rois == null)
            {
                rois = new System.Collections.Generic.List<ROI>();
            }

            if (roiMode != null)			 //either a new ROI object is created
            {
                roiMode.createRectangle2(row, col, phi, length1, length2);
                roiMode.Type = roiMode.GetType().Name;
                rois.Add(roiMode);
                ROIList.Add(roiMode);
                roiMode = null;
                activeROIidx = ROIList.Count - 1;
                viewController.repaint();

                NotifyRCObserver(ROIController.EVENT_CREATED_ROI);
            }
        }
        /// <summary>
        /// 在指定位置生成ROI--Circle
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="radius"></param>
        /// <param name="rois"></param>
        protected internal void genCircle(double row, double col, double radius, ref System.Collections.Generic.List<ROI> rois)
        {
            setROIShape(new ROICircle());

            if (rois == null)
            {
                rois = new System.Collections.Generic.List<ROI>();
            }

            if (roiMode != null)			 //either a new ROI object is created
            {
                roiMode.createCircle(row, col, radius);
                roiMode.Type = roiMode.GetType().Name;
                rois.Add(roiMode);
                ROIList.Add(roiMode);
                roiMode = null;
                activeROIidx = ROIList.Count - 1;
                viewController.repaint();

                NotifyRCObserver(ROIController.EVENT_CREATED_ROI);
            }
        }
        protected internal void genPoint(double row, double col, ref System.Collections.Generic.List<ROI> rois)
        {
            setROIShape(new ROIPoint());

            if (rois == null)
            {
                rois = new System.Collections.Generic.List<ROI>();
            }

            if (roiMode != null)			 //either a new ROI object is created
            {
                roiMode.createPoint(row, col);
                roiMode.Type = roiMode.GetType().Name;
                rois.Add(roiMode);
                ROIList.Add(roiMode);
                roiMode = null;
                activeROIidx = ROIList.Count - 1;
                viewController.repaint();

                NotifyRCObserver(ROIController.EVENT_CREATED_ROI);
            }
        }
        protected internal void genCircularArc(double row, double col, double radius, double startPhi, double extentPhi, string direct, ref System.Collections.Generic.List<ROI> rois)
        {
            setROIShape(new ROICircularArc());

            if (rois == null)
            {
                rois = new System.Collections.Generic.List<ROI>();
            }

            if (roiMode != null)			 //either a new ROI object is created
            {
                roiMode.createCircularArc(row, col, radius, startPhi, extentPhi, direct);
                roiMode.Type = roiMode.GetType().Name;
                rois.Add(roiMode);
                ROIList.Add(roiMode);
                roiMode = null;
                activeROIidx = ROIList.Count - 1;
                viewController.repaint();

                NotifyRCObserver(ROIController.EVENT_CREATED_ROI);
            }
        }


        /// <summary>
        /// 在指定位置生成ROI--Line
        /// </summary>
        /// <param name="beginRow"></param>
        /// <param name="beginCol"></param>
        /// <param name="endRow"></param>
        /// <param name="endCol"></param>
        /// <param name="rois"></param>
        protected internal void genLine(double beginRow, double beginCol, double endRow, double endCol, ref System.Collections.Generic.List<ROI> rois)
        {
            this.setROIShape(new ROILine());//设置“种子”，ROI roiMode = ROILine;

            if (rois == null)//如果roi的list不存在，就创建一个
            {
                rois = new System.Collections.Generic.List<ROI>();
            }

            if (roiMode != null)			 //不可能为空，第一句话已经roiMode = ROILine
            {
                roiMode.createLine(beginRow, beginCol, endRow, endCol);//这个只是指定好首尾坐标，还要绘制到窗体上才行
                roiMode.Type = roiMode.GetType().Name;

                rois.Add(roiMode);
                ROIList.Add(roiMode);
                roiMode = null;//种子用完消除掉
                activeROIidx = ROIList.Count - 1;//将最新的这个设为激活
                viewController.repaint();//绘制出来

                NotifyRCObserver(ROIController.EVENT_CREATED_ROI);
            }
        }
        /// <summary>
        /// 获取当前选中ROI的信息
        /// </summary>
        /// <returns></returns>
        protected internal System.Collections.Generic.List<HTuple> smallestActiveROI(out string name, out int index)
        {
            name = "";
            int activeROIIdx = this.getActiveROIIdx();
            index = activeROIIdx;
            if (activeROIIdx > -1)
            {
                ROI region = this.getActiveROI();
                Type type = region.GetType();
                name = type.Name;

                HTuple smallest = region.getModelData();
                System.Collections.Generic.List<HTuple> resual = new System.Collections.Generic.List<HTuple>();
                for (int i = 0; i < smallest.Length; i++)
                {
                    if (i < 5)
                    {
                        resual.Add(smallest[i].D);
                    }
                    else
                    {
                        resual.Add(smallest[i].S);
                    }
                }

                return resual;
            }
            else
            {
                return null;
            }
        }

        protected internal ROI smallestActiveROI(out System.Collections.Generic.List<HTuple> data, out int index)
        {
            try
            {
                int activeROIIdx = this.getActiveROIIdx();
                index = activeROIIdx;
                data = new System.Collections.Generic.List<HTuple>();

                if (activeROIIdx > -1)//有roi是激活状态
                {
                    ROI region = this.getActiveROI();
                    Type type = region.GetType();

                    HTuple smallest = region.getModelData();//返回直线的row1, col1, row2, col2

                    for (int i = 0; i < smallest.Length; i++)//直线2点，4个坐标。Length=4
                    {
                        if (i < 5)//就是将row1, col1, row2, col2存入data
                        {
                            data.Add(smallest[i].D);
                        }
                        else
                        {
                            data.Add(smallest[i].S);
                        }

                    }

                    return region;//返回被激活的roi
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                data = null;
                index = 0;
                return null;
            }
        }

        /// <summary>
        /// 移除指定的roi
        /// </summary>
        public void removeRoiById(int id)
        {
            if (id != -1)
            {
                if (id == activeROIidx)//如果要删的刚好是当前激活的
                {
                    removeActive();
                }
                else
                {
                    ROIList.RemoveAt(id);
                    viewController.repaint();
                    NotifyRCObserver(EVENT_DELETED_ACTROI);
                }

            }
        }

        /// <summary>
        /// 移除被激活的roi
        /// 如果没有人被激活，就什么都不做 
        /// </summary>
        public void removeActive()
        {
            if (activeROIidx != -1)
            {
                ROIList.RemoveAt(activeROIidx);
                deletedIdx = activeROIidx;
                activeROIidx = -1;
                viewController.repaint();
                NotifyRCObserver(EVENT_DELETED_ACTROI);
            }
        }

        /// <summary>
        /// 删除当前选中ROI
        /// </summary>
        /// <param name="roi"></param>
        protected internal void removeActiveROI()
        {
            int activeROIIdx = this.getActiveROIIdx();
            if (activeROIIdx > -1)
            {
                this.removeActive();
            }
        }

        /// <summary>
        /// 删除当前选中ROI
        /// </summary>
        /// <param name="roi"></param>
        protected internal void removeActiveROI(ref System.Collections.Generic.List<ROI> roi)
        {
            int activeROIIdx = this.getActiveROIIdx();
            if (activeROIIdx > -1)
            {
                this.removeActive();
                roi.RemoveAt(activeROIIdx);
            }
        }
        /// <summary>
        /// 选中激活ROI
        /// </summary>
        /// <param name="index"></param>
        protected internal void selectROI(int index)
        {
            this.activeROIidx = index;
            this.NotifyRCObserver(ROIController.EVENT_ACTIVATED_ROI);
            this.viewController.repaint();
        }
        /// <summary>
        /// 复位窗口显示
        /// </summary>
        protected internal void resetWindowImage()
        {
            //this.viewController.resetWindow();
            this.viewController.repaint();
        }

        protected internal void zoomWindowImage()
        {
            this.viewController.setViewState(HWndCtrl.MODE_VIEW_ZOOM);
        }

        protected internal void moveWindowImage()
        {
            this.viewController.setViewState(HWndCtrl.MODE_VIEW_MOVE);
        }

        protected internal void noneWindowImage()
        {
            this.viewController.setViewState(HWndCtrl.MODE_VIEW_NONE);
        }
    }//end of class
}//end of namespace
