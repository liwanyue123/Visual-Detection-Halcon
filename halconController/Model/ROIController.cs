using System;
using HalconDotNet;
using System.Collections;
using ChoiceTech.Halcon.Control;
using SharedData;
namespace ViewWindow.Model
{

    public delegate void FuncROIDelegate();

    /// <summary>
    /// ���ഴ���͹��� ROI ����
    /// ��ʹ��������²���������ƶ������ķ�����Ӧ����豸���롣
    /// �����Լ��� C# ��Ŀʱ��������ϸ�˽���ࡣ
    /// ���ǣ����Ҫ��Ӧ�ó�����ʹ�ý���ʽ ROI������뿼��һЩ���ROIController �� HWndCtrl ��֮������൱���е����ӣ�
    /// ����ζ��������ʹ�� HWndCtrl"ע��"ROIController����� HWndCtrl ֪�����뽫�û����루������¼���ת���� ROIController �ࡣ
    /// ROI ����Ŀ��ӻ��Ͳ����� ROI ��������ɡ�
    ///����ͨ���� ROI �б��м���ģ������Ϊƥ���Ӧ�ó����ṩ����֧�֡�Ϊ�ˣ��������ǵı�־��Ӻͼ�ȥ ROI��
	/// </summary>
	public class ROIController
    {
        public event RoiLinePositionHandler notifyCurrentRoiLinePositionEvent = null;//��������֪ͨ����ֱ�ߵ���β����
        public event RoiRectangle1PositionHandler notifyCurrentRoiRectangle1PositionEvent = null;//��������֪ͨ����ֱ�ߵ���β����
        public event RoiRectangle2PositionHandler notifyCurrentRoiRectangle2PositionEvent = null;//��������֪ͨ����ֱ�ߵ���β����
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
        /// �������roi��id�����ֻ����һ��������
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
        /// ί�У�֪ͨ��ģ�������������ĸ���
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
        /// Ҫ�����µ� ROI ����Ӧ�ó����ཫ��ʼ��"����"ROI ʵ�������䴫�ݸ� ROIController��
        /// ROIController ����ͨ���������µ� ROI ʵ������Ӧ��
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
        ///"��갴��"�����roi��Ҫ�����һ�û�б�������ԭ�ش���һ���µ�roi����������ӵ� ROI �б�
        /// ��� ROI�Ѿ����ڣ����㵽���ɼ��� ROI
        /// ͬʱ��������ֱ��activeHandleId����������һ���㣨ͷ��β�����֣���ѡ�У���distToClosestHandle��ʵ�֣�
        /// </summary>
        /// <param name="imgX">����¼���x����</param>
        /// <param name="imgY">����¼���y����</param>
        /// <returns></returns>
        public int mouseDownAction(double imgX, double imgY)
        {
            int idxROI = -1;
            double max = 10000, dist = 0;
            double epsilon = 35.0;          //�������ĸ�roi�������//maximal shortest distance to one of
                                            //the handles

            if (roiMode != null)             //�����Ҫ����һ��roi ����ΪҪ����roiMode�д���һ�����ӣ�����roiLine������ֻҪ��Ϊ�գ�����Ҫ����
            {
                roiMode.createROI(imgX, imgY);//���λ�ô���һ��roi
                ROIList.Add(roiMode);//�����б�

                roiMode = null;//roiMode = null��ʾ���ô����ˣ������ӡ��õ���
                activeROIidx = ROIList.Count - 1;//���µ����roi��id
                viewController.repaint();

                NotifyRCObserver(ROIController.EVENT_CREATED_ROI);//���߱��ˣ����ﴴ����һ��roi
            }
            else if (ROIList.Count > 0)     //  ���ڲٿ�һ���Ѿ����ڵ�roi 
            {
                activeROIidx = -1;
                //ÿ��roi�ؼ������͵�ǰ��һ��ȽϾ��룬�ҵ�����ľ��룬����max
                //��Ӧroi��id������idxROI
                //�����roi����ҪС����ֵepsilon��35���У�
                for (int i = 0; i < ROIList.Count; i++)
                {
                    dist = ((ROI)ROIList[i]).distToClosestHandle(imgX, imgY);
                    if ((dist < max) && (dist < epsilon))
                    {
                        max = dist;
                        idxROI = i;
                    }
                }//end of for

                //����ҵ���
                if (idxROI >= 0)
                {
                    activeROIidx = idxROI;//��Ϊ����roi
                    NotifyRCObserver(ROIController.EVENT_ACTIVATED_ROI);//���߱��ˣ����Ｄ����һ��roi
                }

                viewController.repaint();
            }
            return activeROIidx;//�����֮�󣬷��ؼ����roi��id
        }

        /// <summary>
        /// roi��Ӧ����ƶ��¼�: �ƶ������roi�е�Ԫ�أ�
        /// �ƶ��ĸ�roi��activeROIidx
        /// �ƶ����roi���ĸ��㣺activeHandleIdx����2��id����ROI����
        /// ��ΪactiveHandleIdx��ȫ�ֵı������������ﲻ��Ҫ����ָ����roi����
        /// </summary>
        /// <param name="newX">x coordinate of mouse event</param>
        /// <param name="newY">y coordinate of mouse event</param>
        public void mouseMoveAction(double newX, double newY)
        {
            try
            {
                if (EditModel == false) return;//�����ֹ�༭��ʲô������
                if ((newX == currX) && (newY == currY))//������û���ƶ���Ҳʲô������
                    return;

                ((ROI)ROIList[activeROIidx]).moveByHandle(newX, newY);
                //����ͨ���ı�activeHandleIdx����ֱ�ߵ�ĳһ�˵㣨��3�㣩�����ƶ�     //0:  first end point      1:  last end point       2:  midpoint
                //�����down��ʱ����Ѿ�ָ���������ĸ�roi���ĸ�����
                viewController.repaint();//ÿ�ƶ�һ����ػ�һ��
                currX = newX;
                currY = newY;
                NotifyRCObserver(ROIController.EVENT_MOVING_ROI);//���߱������﷢����roi�ƶ��¼�
            }
            catch (Exception)
            {
                //û����ʾroi��ʱ�� �ƶ����ᱨ��
            }

        }

        /***********************************************************/
        public void dummyI(int v)
        {
        }

        /*****************************/
        /// <summary>
        /// ��ָ��λ����ʾROI--Rectangle1
        /// </summary>
        /// <param name="row1"></param>
        /// <param name="col1"></param>
        /// <param name="row2"></param>
        /// <param name="col2"></param>
        /// <param name="rois"></param>
        public void displayRect1(string color, double row1, double col1, double row2, double col2)
        {
            setROIShape(new ROIRectangle1());//���ɾ���1������

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
        /// ��ָ��λ����ʾROI--Rectangle2
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
        /// ��ָ��λ������ROI--Circle
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
        /// ��ָ��λ����ʾROI--Line����genLine��࣬����������Ҫ����ʾ����������û�з���roi�б�
        /// </summary>
        /// <param name="beginRow"></param>
        /// <param name="beginCol"></param>
        /// <param name="endRow"></param>
        /// <param name="endCol"></param>
        /// <param name="rois"></param>
        public void displayLine(string color, double beginRow, double beginCol, double endRow, double endCol)
        {
            this.setROIShape(new ROILine());//���롰���ӡ�roi�����ﴫһ��ROILine������roiMode�Ͳ�Ϊ����

            if (roiMode != null)			 //һ����Ϊ��
            {
                roiMode.notifyCurrentRoiLinePositionEvent += notifyCurrentRoiLinePositionEvent;   
                
                roiMode.createLine(beginRow, beginCol, endRow, endCol);//ROILine.createLine����ʵֻ������ȫ����β������
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
        /// ��ָ��λ������ROI--Rectangle1
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
        /// ��ָ��λ������ROI--Rectangle2
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
        /// ��ָ��λ������ROI--Circle
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
        /// ��ָ��λ������ROI--Line
        /// </summary>
        /// <param name="beginRow"></param>
        /// <param name="beginCol"></param>
        /// <param name="endRow"></param>
        /// <param name="endCol"></param>
        /// <param name="rois"></param>
        protected internal void genLine(double beginRow, double beginCol, double endRow, double endCol, ref System.Collections.Generic.List<ROI> rois)
        {
            this.setROIShape(new ROILine());//���á����ӡ���ROI roiMode = ROILine;

            if (rois == null)//���roi��list�����ڣ��ʹ���һ��
            {
                rois = new System.Collections.Generic.List<ROI>();
            }

            if (roiMode != null)			 //������Ϊ�գ���һ�仰�Ѿ�roiMode = ROILine
            {
                roiMode.createLine(beginRow, beginCol, endRow, endCol);//���ֻ��ָ������β���꣬��Ҫ���Ƶ������ϲ���
                roiMode.Type = roiMode.GetType().Name;

                rois.Add(roiMode);
                ROIList.Add(roiMode);
                roiMode = null;//��������������
                activeROIidx = ROIList.Count - 1;//�����µ������Ϊ����
                viewController.repaint();//���Ƴ���

                NotifyRCObserver(ROIController.EVENT_CREATED_ROI);
            }
        }
        /// <summary>
        /// ��ȡ��ǰѡ��ROI����Ϣ
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

                if (activeROIIdx > -1)//��roi�Ǽ���״̬
                {
                    ROI region = this.getActiveROI();
                    Type type = region.GetType();

                    HTuple smallest = region.getModelData();//����ֱ�ߵ�row1, col1, row2, col2

                    for (int i = 0; i < smallest.Length; i++)//ֱ��2�㣬4�����ꡣLength=4
                    {
                        if (i < 5)//���ǽ�row1, col1, row2, col2����data
                        {
                            data.Add(smallest[i].D);
                        }
                        else
                        {
                            data.Add(smallest[i].S);
                        }

                    }

                    return region;//���ر������roi
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
        /// �Ƴ�ָ����roi
        /// </summary>
        public void removeRoiById(int id)
        {
            if (id != -1)
            {
                if (id == activeROIidx)//���Ҫɾ�ĸպ��ǵ�ǰ�����
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
        /// �Ƴ��������roi
        /// ���û���˱������ʲô������ 
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
        /// ɾ����ǰѡ��ROI
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
        /// ɾ����ǰѡ��ROI
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
        /// ѡ�м���ROI
        /// </summary>
        /// <param name="index"></param>
        protected internal void selectROI(int index)
        {
            this.activeROIidx = index;
            this.NotifyRCObserver(ROIController.EVENT_ACTIVATED_ROI);
            this.viewController.repaint();
        }
        /// <summary>
        /// ��λ������ʾ
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
