using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HalconDotNet;
using ChoiceTech.Halcon.Control;

namespace ViewWindow
{
    public class ViewWindow : Model.IViewWindow
    {
       // public event RoiLinePositionHandler notifyCurrentRoiLinePositionEvent = null;//向主窗体通知现在直线的首尾坐标
        public Model.HWndCtrl _hWndControl;//这里设置为公共的(20191030)

        public Model.ROIController _roiController;//20200226

   
        public ViewWindow(HWindowControl window)
        {
            this._hWndControl = new Model.HWndCtrl(window);
            this._roiController = new Model.ROIController();
            this._hWndControl.setROIController(this._roiController);
            this._hWndControl.setViewState(Model.HWndCtrl.MODE_VIEW_NONE);
           //_roiController.notifyCurrentRoiLinePositionEvent += notifyCurrentRoiLinePositionEvent;
        }
 
        //清空所有显示内容
        public void ClearWindow()
        {
            //清空显示image
            _hWndControl.clearList();
            //清空hobjectList
            _hWndControl.clearHObjectList();
        }
        public void ClearObj()
        {
            //清空hobjectList
            _hWndControl.clearHObjectList();
        }
    
        public void displayImage(HObject img)
        {
            //添加背景图片
            this._hWndControl.addImageShow(img);
            //清空roi容器,不让roi显示
            this._roiController.reset();
            //显示图片
           this._roiController.resetWindowImage();


            //this._hWndControl.resetWindow();
           // this._hWndControl.resetAll();
            //this._hWndControl.repaint();
        }
        public void notDisplayRoi()
        {

            this._roiController.reset();
            //显示图片
            this._roiController.resetWindowImage();

        }
        //获取当前窗口显示的roi数量
        public int getRoiCount()
        {
            return _roiController.ROIList.Count;
        }
        //是否开启缩放事件
        public void setDrawModel(bool flag)
        {
            _hWndControl.drawModel = flag;
        }
        //是否开启编辑事件
        public void setEditModel(bool flag)
        {
            _roiController.EditModel = flag;
          //  _hWndControl.drawModel = flag;
        }
        public void repaint()
        {
            this._hWndControl.repaint();
        }

        public void resetWindowImage()
        {
            this._hWndControl.resetWindow();
            this._roiController.resetWindowImage();
        }

        public void mouseleave()
        {
            _hWndControl.raiseMouseup();
        }

        public void zoomWindowImage()
        {
            this._roiController.zoomWindowImage();
        }

        public void moveWindowImage()
        {
            this._roiController.moveWindowImage();
        }

        public void noneWindowImage()
        {
            this._roiController.noneWindowImage();
        }

        public void genRect1(double row1, double col1, double row2, double col2, ref List<Model.ROI> rois)
        {
            this._roiController.genRect1(row1, col1, row2, col2, ref rois);
        }

        public void genRect2(double row, double col, double phi, double length1, double length2, ref List<Model.ROI> rois)
        {
            this._roiController.genRect2(row, col, phi, length1, length2, ref rois);
        }

        public void genCircle(double row, double col, double radius, ref List<Model.ROI> rois)
        {
            this._roiController.genCircle(row, col, radius, ref rois);
        }
        public void genPoint(double row, double col,  ref List<Model.ROI> rois)
        {
            this._roiController.genPoint(row, col, ref rois);
        }
        public void genCircularArc(double row, double col, double radius, double startPhi, double extentPhi, string direct,ref List<Model.ROI> rois)
        {
            this._roiController.genCircularArc(row, col, radius,  startPhi, extentPhi,direct,ref rois);
        }
        public void genLine(double beginRow, double beginCol, double endRow, double endCol, ref List<Model.ROI> rois)
        {
            this._roiController.genLine(beginRow, beginCol, endRow, endCol, ref rois);
        }
   
        public List<HTuple> smallestActiveROI(out string name, out int index)
        {
            List<HTuple> resual = this._roiController.smallestActiveROI(out name,out index);
            return resual;
        }
        /// <summary>
        /// 返回当前被激活的roi的信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public Model.ROI smallestActiveROI(out List<HTuple> data, out int index)
        {
            Model.ROI roi = this._roiController.smallestActiveROI(out data, out index);
            return roi;//返回被激活的roi
        }

        /// <summary>
        /// 激活指定的ROI
        /// </summary>
        /// <param name="index"></param>
        public void selectROI(int index)
        {
            this._roiController.selectROI(index);
        }
        /// <summary>
        /// 激活指定的ROI
        /// </summary>
        /// <param name="index"></param>
        public void selectROI( List<Model.ROI> rois, int index)
        {
            //this._roiController.selectROI(index);
            if ((rois.Count > index)&&(index>=0))
            {
                this._hWndControl.resetAll();
                this._hWndControl.repaint();

                HTuple m_roiData = null;
                m_roiData = rois[index].getModelData();

                switch (rois[index].Type)
                {
                    case "ROIRectangle1":

                        if (m_roiData != null)
                        {
                            this._roiController.displayRect1(rois[index].Color, m_roiData[0].D, m_roiData[1].D, m_roiData[2].D, m_roiData[3].D);
                        }
                        break;
                    case "ROIRectangle2":

                        if (m_roiData != null)
                        {
                            this._roiController.displayRect2(rois[index].Color, m_roiData[0].D, m_roiData[1].D, m_roiData[2].D, m_roiData[3].D, m_roiData[4].D);
                        }
                        break;
                    case "ROICircle":

                        if (m_roiData != null)
                        {
                            this._roiController.displayCircle(rois[index].Color, m_roiData[0].D, m_roiData[1].D, m_roiData[2].D);
                        }
                        break;

                    case "ROIPoint":

                        if (m_roiData != null)
                        {
                            this._roiController.displayPoint(rois[index].Color, m_roiData[0].D, m_roiData[1].D);
                        }
                        break;
                    case "ROICircularArc":

                        if (m_roiData != null)
                        {
                            this._roiController.displayCircularArc(rois[index].Color, m_roiData[0].D, m_roiData[1].D, m_roiData[2].D, m_roiData[3].D, m_roiData[4].D, m_roiData[5].S);
                        }
                        break;
                    case "ROILine":

                        if (m_roiData != null)
                        {
                            this._roiController.displayLine(rois[index].Color, m_roiData[0].D, m_roiData[1].D, m_roiData[2].D, m_roiData[3].D);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 显示全部roi
        /// </summary>
        /// <param name="rois"></param>
        public void displayROI(List<Model.ROI> rois)
        {
            if (rois == null)
            {
                return;
            }
            //this._hWndControl.resetAll();
            //this._hWndControl.repaint();

            foreach (var roi in rois)
            {
                HTuple m_roiData = null;
                m_roiData = roi.getModelData();

                switch (roi.Type)
                {
                    case "ROIRectangle1":

                        if (m_roiData != null)
                        {
                            this._roiController.displayRect1(roi.Color, m_roiData[0].D, m_roiData[1].D, m_roiData[2].D, m_roiData[3].D);
                        }
                        break;
                    case "ROIRectangle2":

                        if (m_roiData != null)
                        {
                            this._roiController.displayRect2(roi.Color, m_roiData[0].D, m_roiData[1].D, m_roiData[2].D, m_roiData[3].D, m_roiData[4].D);
                     
                        }
                        break;
                    case "ROICircle":

                        if (m_roiData != null)
                        {
                            this._roiController.displayCircle(roi.Color, m_roiData[0].D, m_roiData[1].D, m_roiData[2].D);
                        }
                        break;
                    case "ROIPoint":

                        if (m_roiData != null)
                        {
                            this._roiController.displayPoint(roi.Color, m_roiData[0].D, m_roiData[1].D);
                        }
                        break;
                    case "ROICircularArc":

                        if (m_roiData != null)
                        {
                           
                            this._roiController.displayCircularArc(roi.Color, m_roiData[0].D, m_roiData[1].D, m_roiData[2].D, m_roiData[3].D, m_roiData[4].D, m_roiData[5].S);
                        }
                        break;
                    case "ROILine":

                        if (m_roiData != null)
                        {
                            this._roiController.displayLine(roi.Color, m_roiData[0].D, m_roiData[1].D, m_roiData[2].D, m_roiData[3].D);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// 删除指定的ROI
        /// </summary>
        /// <param name="roi"></param>
        public void removeROI(int id)
        {
            this._roiController.removeRoiById(id);
        }
        public int gerROIListNum()
        {
            return this._roiController.gerROIListNum();
        }

        /// <summary>
        /// 删除当前选中ROI
        /// </summary>
        /// <param name="roi"></param>
        public void removeActiveROI()
        {
            this._roiController.removeActiveROI();
        }

        /// <summary>
        /// 删除当前选中ROI
        /// </summary>
        /// <param name="roi"></param>
        public void removeActiveROI(ref List<Model.ROI> rois)
        {
            this._roiController.removeActiveROI(ref rois);
        }

        public void setActiveRoi(int index)
        {
            this._roiController.activeROIidx = index;
        }

        /// <summary>
        ///将rois信息序列化，保存到指定的文件中
        /// </summary>
        /// <param name="rois"></param>
        /// <param name="fileNmae"></param>
        public void saveROI(List<Model.ROI> rois, string fileNmae)
        {
            List<Model.RoiData> m_RoiData = new List<Model.RoiData>();
            for (int i = 0; i < rois.Count; i++)
            {
                m_RoiData.Add(new Model.RoiData(i, rois[i]));
            }

            Config.SerializeHelper.Save(m_RoiData, fileNmae);
        }

        /// <summary>
        ///从序列化文件中，读取将rois信息
        /// </summary>
        /// <param name="rois"></param>
        /// <param name="fileNmae"></param>
        public void loadROI(string fileName, out List<Model.ROI> rois)
        {
            rois = new List<Model.ROI>();
            List<Model.RoiData> m_RoiData = new List<Model.RoiData>();
            m_RoiData = (List<Model.RoiData>)Config.SerializeHelper.Load(m_RoiData.GetType(), fileName);


            for (int i = 0; i < m_RoiData.Count; i++)
            {
                switch (m_RoiData[i].Name)
                {
                    case "Rectangle1":
                        this._roiController.genRect1(m_RoiData[i].Rectangle1.Row1, m_RoiData[i].Rectangle1.Column1,
                            m_RoiData[i].Rectangle1.Row2, m_RoiData[i].Rectangle1.Column2, ref rois);
                        rois.Last().Color = m_RoiData[i].Rectangle1.Color;
                        //rois.Last().PointState = m_RoiData[i].Rectangle1.PointState;
                        //rois.Last().Speed = m_RoiData[i].Rectangle1.Speed;
                        //rois.Last().Zpostion = m_RoiData[i].Rectangle1.Zpostion;
                        //rois.Last().LineType = m_RoiData[i].Rectangle1.LineType;
                        break;
                    case "Rectangle2":
                        this._roiController.genRect2(m_RoiData[i].Rectangle2.Row, m_RoiData[i].Rectangle2.Column,
                            m_RoiData[i].Rectangle2.Phi, m_RoiData[i].Rectangle2.Lenth1, m_RoiData[i].Rectangle2.Lenth2, ref rois);
                        rois.Last().Color = m_RoiData[i].Rectangle2.Color;
                        //rois.Last().PointState = m_RoiData[i].Rectangle2.PointState;
                        //rois.Last().Speed = m_RoiData[i].Rectangle2.Speed;
                        //rois.Last().Zpostion = m_RoiData[i].Rectangle2.Zpostion;
                        //rois.Last().LineType = m_RoiData[i].Rectangle2.LineType;
                        break;
                    case "Circle":
                        this._roiController.genCircle(m_RoiData[i].Circle.Row, m_RoiData[i].Circle.Column, m_RoiData[i].Circle.Radius, ref rois);
                        rois.Last().Color = m_RoiData[i].Circle.Color;
                        //rois.Last().PointState = m_RoiData[i].Circle.PointState;
                        //rois.Last().Speed = m_RoiData[i].Circle.Speed;
                        //rois.Last().Zpostion = m_RoiData[i].Circle.Zpostion;
                        //rois.Last().LineType = m_RoiData[i].Circle.LineType;
                        break;

                    case "Point":
                        this._roiController.genPoint(m_RoiData[i].Point.Row, m_RoiData[i].Point.Column, ref rois);
                        rois.Last().Color = m_RoiData[i].Point.Color;
                        //rois.Last().PointState = m_RoiData[i].Point.PointState;
                        //rois.Last().Speed = m_RoiData[i].Point.Speed;
                        //rois.Last().Zpostion = m_RoiData[i].Point.Zpostion;
                        //rois.Last().LineType = m_RoiData[i].Point.LineType;
                        break;
                    case "CircularArc":
                        this._roiController.genCircularArc(m_RoiData[i].CircularArc.Row, m_RoiData[i].CircularArc.Column, m_RoiData[i].CircularArc.Radius, m_RoiData[i].CircularArc.StartPhi, m_RoiData[i].CircularArc.ExtentPhi, m_RoiData[i].CircularArc.Direct, ref rois);
                        rois.Last().Color = m_RoiData[i].CircularArc.Color;
                        //rois.Last().PointState = m_RoiData[i].CircularArc.PointState;
                        //rois.Last().Speed = m_RoiData[i].CircularArc.Speed;
                        //rois.Last().Zpostion = m_RoiData[i].CircularArc.Zpostion;
                        //rois.Last().LineType = m_RoiData[i].CircularArc.LineType;
                        break;
                    case "Line":
                        this._roiController.genLine(m_RoiData[i].Line.RowBegin, m_RoiData[i].Line.ColumnBegin,
                            m_RoiData[i].Line.RowEnd, m_RoiData[i].Line.ColumnEnd, ref rois);
                        rois.Last().Color = m_RoiData[i].Line.Color;
                        //rois.Last().PointState = m_RoiData[i].Line.PointState;
                        //rois.Last().Speed = m_RoiData[i].Line.Speed;
                        //rois.Last().Zpostion = m_RoiData[i].Line.Zpostion;
                        //rois.Last().LineType = m_RoiData[i].Line.LineType;
                        break;
                    default:
                        break;
                }
            }

            //this._hWndControl.resetAll();
            // this._hWndControl.repaint();
        }

        
        /// <summary>
        /// 专门用于 显示region 和xld的方法
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="color"></param>
        public void displayHobject(HObject obj,string color)
        {
            _hWndControl.DispObj(obj, color);
        
        }
        public void displayHobject(HObject obj)
        {
            _hWndControl.DispObj(obj, null);
        }
 
        public void SetLineWidth(int width)
        {
            _hWndControl.SetLineWidth(width);
        }
 
    }
}
