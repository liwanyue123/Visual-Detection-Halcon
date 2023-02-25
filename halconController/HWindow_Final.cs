// 版权所有(C) ChoiceTech Corporation。保留所有权利。
// 此代码的发布遵从
// ChoiceTech 公共许可(HY-PL，http://choicetech.cn/hy-pl.html)的条款。
//
//版权所有(C) ChoiceTech Corporation。保留所有权利。

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
 
using System.Diagnostics;
 

using HalconDotNet;
using SharedData;
namespace ChoiceTech.Halcon.Control
{
    /// <summary>
    /// halcon鼠标缩放控件
    /// 
    /// 描述:
    ///      1, 必须首先通过this.HobjectToHimage(HObject hobject)传入图片,此图片称为"背景图"
    ///      2, 有了背景图,就可以通过本控件自定义的 this.DispObj(HObject hObj)显示HObject,类似原方法
    ///      3,默认显示红色,DispObj(HObject hObj,string color)可显示其他颜色
    /// </summary>
    public delegate void RoiLinePositionHandler(Object sender, double row1, double col1, double row2, double col2);
    public delegate void RoiRectangle1PositionHandler(Object sender, Rectangle1Inf rect1);
    public delegate void RoiRectangle2PositionHandler(Object sender, Rectangle2Inf rect2);
    public partial class HWindow_Final : UserControl
    {

        // public event RoiLinePositionHandler notifyCurrentRoiLinePositionEvent = null;//向主窗体通知现在直线的首尾坐标
        #region 私有变量定义.

        private HWindow /**/                 hv_window;                                       //halcon窗体控件的句柄 this.mCtrl_HWindow.HalconWindow;
        private ContextMenuStrip /**/        hv_MenuStrip;                                    //右键菜单控件
        // 窗体控件右键菜单内容
        ToolStripMenuItem fit_strip;//自适应
        ToolStripMenuItem saveImg_strip;//保存图像
        ToolStripMenuItem saveWindow_strip;//保存截图
        ToolStripMenuItem barVisible_strip;//显示状态

        //ToolStripMenuItem displayPositionP1;//显示坐标位置----截图当前坐标位置1
        //ToolStripMenuItem displayPositionP2;//显示坐标位置----截图当前坐标位置2
        //ToolStripMenuItem displayPositionP3;//显示坐标位置----截图当前坐标位置3

        //ToolStripMenuItem updateP1toP2;//更新第一点为第二点坐标
        //ToolStripMenuItem updateP1toP3;//更新第一点为第三点坐标

        //ToolStripMenuItem updateP3toP1;//更新第三点为第一点坐标


        //public PointF?[] PointPostion = new PointF?[3];//用来记录三个点位置--可以是null
        //private PointF? curps = null;//一个可null当前位置

        //  ToolStripMenuItem histogram_strip;

        private HImage  /**/                 hv_image;                                        //缩放时操作的图片  此处千万不要使用hv_image = new HImage(),不然在生成控件dll的时候,会导致无法序列化,去你妈隔壁,还好老子有版本控制,不然都找不到这种恶心问题
        private int /**/                     hv_imageWidth, hv_imageHeight;                   //图片宽,高
        private string /**/                  str_imgSize;                                     //图片尺寸大小 5120X3840
        private bool    /**/                 drawModel = false;                                //绘制模式下,不允许缩放和鼠标右键菜单

        public ViewWindow.ViewWindow viewWindow;    /**/                                      //ViewWindow
        public HWindowControl hWindowControl;   /**/                                           // 当前halcon窗口

        #endregion

        public static bool IsDesignMode()
        {
            bool returnFlag = false;
#if DEBUG
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                returnFlag = true;
            }
            else if (Process.GetCurrentProcess().ProcessName == "devenv")
            {
                returnFlag = true;
            }
            return returnFlag;
#endif
        }

        //  public event Action GenCrossPhoto=null;
        /// <summary>
        /// 初始化控件
        /// </summary>
        public HWindow_Final()
        {
            InitializeComponent();
            //
            viewWindow = new ViewWindow.ViewWindow(mCtrl_HWindow);
            hWindowControl = this.mCtrl_HWindow;
            if (!IsDesignMode())
                hv_window = this.mCtrl_HWindow.HalconWindow;

            //              设定鼠标按下时图标的形状
            //              'arrow'  'default' 'crosshair' 'text I-beam' 'Slashed circle' 'Size All'
            //              'Size NESW' 'Size S' 'Size NWSE' 'Size WE' 'Vertical Arrow' 'Hourglass'
            //
            // hv_window.SetMshape("Hourglass");

            fit_strip = new ToolStripMenuItem("适应窗口");
            fit_strip.Click += new EventHandler((s, e) => DispImageFit(mCtrl_HWindow));

            barVisible_strip = new ToolStripMenuItem("显示StatusBar");
            barVisible_strip.CheckOnClick = true;
            barVisible_strip.CheckedChanged += new EventHandler(barVisible_strip_CheckedChanged);
            m_CtrlHStatusLabelCtrl.Visible = false;
            mCtrl_HWindow.Height = this.Height;

            saveImg_strip = new ToolStripMenuItem("保存原始图像");
            saveImg_strip.Click += new EventHandler((s, e) => SaveImage());

            saveWindow_strip = new ToolStripMenuItem("保存窗口缩略图");
            saveWindow_strip.Click += new EventHandler((s, e) => SaveWindowDump());



            //histogram_strip = new ToolStripMenuItem("显示直方图(H)");
            //histogram_strip.CheckOnClick = true;
            //histogram_strip.Checked = false;

            //displayPositionP1 = new ToolStripMenuItem("起点/打点(红色)");
            //displayPositionP1.Click+= new EventHandler((s, e) => { PointPostion[0] = curps; GenCrossPhoto?.Invoke();/*MessageBox.Show(PointPostion[0].Value.X.ToString());*/ });


            //displayPositionP2 = new ToolStripMenuItem("中间点/圆弧点(绿色)");
            //displayPositionP2.Click += new EventHandler((s, e) => { PointPostion[1] = curps; GenCrossPhoto?.Invoke();/*MessageBox.Show(PointPostion[0].Value.X.ToString());*/ });

            //displayPositionP3 = new ToolStripMenuItem("终点(蓝色)");
            //displayPositionP3.Click += new EventHandler((s, e) => { PointPostion[2] = curps; GenCrossPhoto?.Invoke();/*MessageBox.Show(PointPostion[0].Value.X.ToString());*/ });

            //updateP1toP2 = new ToolStripMenuItem("更新\"起点/打点\"为\"中间点/圆弧点\"坐标");
            //updateP1toP2.Click += new EventHandler((s, e) => { if (PointPostion[1] != null) { PointPostion[0] = new PointF(PointPostion[1].Value.X, PointPostion[1].Value.Y); }  GenCrossPhoto?.Invoke(); });

            //updateP1toP3 = new ToolStripMenuItem("更新\"起点/打点\"为\"终点\"坐标");
            //updateP1toP3.Click += new EventHandler((s, e) => { if (PointPostion[2] != null) { PointPostion[0] = new PointF(PointPostion[2].Value.X, PointPostion[2].Value.Y); } GenCrossPhoto?.Invoke(); });

            //updateP3toP1 = new ToolStripMenuItem("更新\"终点\"为\"打点/起点\"坐标");
            //updateP3toP1.Click += new EventHandler((s, e) => { if (PointPostion[1] != null) { PointPostion[2] = new PointF(PointPostion[0].Value.X, PointPostion[0].Value.Y); } GenCrossPhoto?.Invoke(); });

            hv_MenuStrip = new ContextMenuStrip();
            hv_MenuStrip.Items.Add(fit_strip);
            hv_MenuStrip.Items.Add(barVisible_strip);
            hv_MenuStrip.Items.Add(new ToolStripSeparator());
            hv_MenuStrip.Items.Add(saveImg_strip);
            hv_MenuStrip.Items.Add(saveWindow_strip);
            //hv_MenuStrip.Items.Add(new ToolStripSeparator());
            //hv_MenuStrip.Items.Add(displayPositionP1);
            //hv_MenuStrip.Items.Add(displayPositionP2);
            //hv_MenuStrip.Items.Add(displayPositionP3);
            //hv_MenuStrip.Items.Add(new ToolStripSeparator());
            //hv_MenuStrip.Items.Add(updateP1toP2);
            //hv_MenuStrip.Items.Add(updateP1toP3);
            //hv_MenuStrip.Items.Add(updateP3toP1);

            barVisible_strip.Enabled = true;
            fit_strip.Enabled = false;
            // histogram_strip.Enabled = false;
            saveImg_strip.Enabled = false;
            saveWindow_strip.Enabled = false;
            //displayPositionP1.Enabled = false;
            //displayPositionP2.Enabled = false;
            //displayPositionP3.Enabled = false;
            mCtrl_HWindow.ContextMenuStrip = hv_MenuStrip;
            mCtrl_HWindow.SizeChanged += new EventHandler((s, e) => DispImageFit(mCtrl_HWindow));



        }


        /// <summary>
        /// 绘制模式下,不允许缩放和鼠标右键菜单
        /// </summary>
        public bool DrawModel
        {
            get { return drawModel; }
            set
            {
                //缩放控制
                viewWindow.setDrawModel(value);
                //绘制模式 不现实右键
                if (value == true)
                {

                    mCtrl_HWindow.ContextMenuStrip = null;
                }
                else
                {
                    //恢复
                    mCtrl_HWindow.ContextMenuStrip = hv_MenuStrip;
                }
                drawModel = value;
            }
        }
        private bool _EditModel = true;//绘制的图形是否可以编辑
        public bool EditModel
        {
            get
            {
                return _EditModel;
            }
            set
            {
                viewWindow.setEditModel(value);
                _EditModel = value;
            }
        }

        /// <summary>
        /// 设置image,初始化控件参数
        /// </summary>
        public HImage Image
        {
            get {
                return this.hv_image;
            }
            set
            {
                if (value != null)
                {
                    if (this.hv_image != null)
                    {
                        this.hv_image.Dispose();
                    }

                    this.hv_image = value;
                    hv_image.GetImageSize(out hv_imageWidth, out hv_imageHeight);
                    str_imgSize = String.Format("{0}X{1}", hv_imageWidth, hv_imageHeight);

                    //DispImageFit(mCtrl_HWindow);
                    try
                    {
                        barVisible_strip.Enabled = true;
                        fit_strip.Enabled = true;
                        // histogram_strip.Enabled = true;
                        //displayPositionP1.Enabled = true;
                        //displayPositionP2.Enabled = true;
                        //displayPositionP3.Enabled = true;
                        saveImg_strip.Enabled = true;
                        saveWindow_strip.Enabled = true;
                    }
                    catch (Exception)
                    {
                    }

                    viewWindow.displayImage(hv_image);
                }
            }
        }

        /// <summary>
        /// 获得halcon窗体控件的句柄
        /// </summary>
        public IntPtr HWindowHalconID
        {
            get { return this.mCtrl_HWindow.HalconID; }
        }

        public HWindowControl getHWindowControl()
        {
            return this.mCtrl_HWindow;
        }

        /// <summary>
        /// 状态条 显示/隐藏 CheckedChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void barVisible_strip_CheckedChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem strip = sender as ToolStripMenuItem;

            this.SuspendLayout();

            if (strip.Checked)
            {
                m_CtrlHStatusLabelCtrl.Visible = true;
                //mCtrl_HWindow.Height = this.Height - m_CtrlHStatusLabelCtrl.Height - m_CtrlHStatusLabelCtrl.Margin.Top - m_CtrlHStatusLabelCtrl.Margin.Bottom;
                mCtrl_HWindow.HMouseMove += HWindowControl_HMouseMove;
            }
            else
            {
                m_CtrlHStatusLabelCtrl.Visible = false;
                //mCtrl_HWindow.Height = this.Height;
                mCtrl_HWindow.HMouseMove -= HWindowControl_HMouseMove;
            }

            //DispImageFit(mCtrl_HWindow);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public void showStatusBar()
        {
            barVisible_strip.Checked = true;
        }

        /// <summary>
        /// 保存窗体截图到本地
        /// </summary>
        private void SaveWindowDump()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PNG图像|*.png|所有文件|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (String.IsNullOrEmpty(sfd.FileName))
                    return;

                //截取窗口图
                HOperatorSet.DumpWindow(HWindowHalconID, "png best", sfd.FileName);
            }
        }

        /// <summary>
        /// 保存原始图片到本地
        /// </summary>
        private void SaveImage()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "BMP图像|*.bmp|所有文件|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (String.IsNullOrEmpty(sfd.FileName))
                {
                    return;
                }

                HOperatorSet.WriteImage(hv_image, "bmp", 0, sfd.FileName);
            }
        }
        //  private void recordP1()
        /// <summary>
        /// 图片适应大小显示在窗体
        /// </summary>
        /// <param name="hw_Ctrl">halcon窗体控件</param>
        private void DispImageFit(HWindowControl hw_Ctrl)
        {

            try
            {
                this.viewWindow.resetWindowImage();
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// 鼠标在空间窗体里滑动,显示鼠标所在位置的灰度值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HWindowControl_HMouseMove(object sender, HMouseEventArgs e)
        {

            if (hv_image != null)
            {
                try
                {
                    int button_state;
                    double positionX, positionY;
                    string str_value;
                    string str_position;
                    bool _isXOut = true, _isYOut = true;
                    HTuple channel_count;
                    this.Cursor = Cursors.Cross;
                    HOperatorSet.CountChannels(hv_image, out channel_count);//获得图像通道数

                    hv_window.GetMpositionSubPix(out positionY, out positionX, out button_state);//返回输出窗口中鼠标精确坐标。无论鼠标按钮的状态（按下或未按下），都将返回这些值
                                                                                                 // curps = new PointF((float)positionX, (float)positionY);//这种方法赋值

                    str_position = String.Format("ROW: {0:0000.0}, COLUMN: {1:0000.0}", positionY, positionX);

                    _isXOut = (positionX < 0 || positionX >= hv_imageWidth);
                    _isYOut = (positionY < 0 || positionY >= hv_imageHeight);

                    if (!_isXOut && !_isYOut)
                    {
                        if ((int)channel_count == 1)
                        {
                            double grayVal;
                            grayVal = hv_image.GetGrayval((int)positionY, (int)positionX);
                            str_value = String.Format("Val: {0:000.0}", grayVal);
                        }
                        else if ((int)channel_count == 3)
                        {
                            double grayValRed, grayValGreen, grayValBlue;

                            HImage _RedChannel, _GreenChannel, _BlueChannel;

                            _RedChannel = hv_image.AccessChannel(1);
                            _GreenChannel = hv_image.AccessChannel(2);
                            _BlueChannel = hv_image.AccessChannel(3);

                            grayValRed = _RedChannel.GetGrayval((int)positionY, (int)positionX);
                            grayValGreen = _GreenChannel.GetGrayval((int)positionY, (int)positionX);
                            grayValBlue = _BlueChannel.GetGrayval((int)positionY, (int)positionX);

                            _RedChannel.Dispose();
                            _GreenChannel.Dispose();
                            _BlueChannel.Dispose();

                            str_value = String.Format("Val: ({0:000.0}, {1:000.0}, {2:000.0})", grayValRed, grayValGreen, grayValBlue);
                        }
                        else
                        {
                            str_value = "";
                        }
                        m_CtrlHStatusLabelCtrl.Text = str_imgSize + "    " + str_position + "    " + str_value;
                    }
                }
                catch (Exception ex)
                {
                    //不处理
                }
            }

        }

       
        public void ClearWindow()
        {
            try
            {
                this.Invoke(new Action(
                        () =>
                        {
                            //this.hv_image = null;
                            m_CtrlHStatusLabelCtrl.Visible = false;
                            barVisible_strip.Enabled = false;
                            fit_strip.Enabled = false;
                           // histogram_strip.Enabled = false;
                            saveImg_strip.Enabled = false;
                            saveWindow_strip.Enabled = false;

                            mCtrl_HWindow.HalconWindow.ClearWindow();
                            viewWindow.ClearWindow();

                        }
                    ));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


        }


        /// <summary>
        /// Hobject转换为的临时Himage,显示背景图
        /// </summary>
        /// <param name="hobject">传递Hobject,必须为图像</param>
        public void HobjectToHimage(HObject hobject)
        {
            if (hobject == null || !hobject.IsInitialized())
            {
                ClearWindow();
                return;
            }

            this.Image = new HImage(hobject);

        }

        #region 缩放后,再次显示传入的HObject


        /// <summary>
        /// 默认红颜色显示
        /// </summary>
        /// <param name="hObj">传入的region.xld,image</param>
        public void DispObj(HObject hObj)
        {

                lock (this)
                {
                    viewWindow.displayHobject(hObj, null);
                }


        }

       
        /// <summary>
        /// 重新开辟内存保存 防止被传入的HObject在其他地方dispose后,不能重现
        /// </summary>
        /// <param name="hObj">传入的region.xld,image</param>
        /// <param name="color">颜色</param>
        public void DispObj(HObject hObj, string color)
        {

            lock (this)
            {
                viewWindow.displayHobject(hObj, color);
            }


        }


        #endregion

        /// <summary>
        /// 鼠标离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mCtrl_HWindow_MouseLeave(object sender, EventArgs e)
        {
            //避免鼠标离开窗口,再返回的时候,图表随着鼠标移动
            viewWindow.mouseleave();
        }

        /// <summary>
        /// 在图像中绘制直线
        /// </summary>
        /// <param name="color"></param>
        /// <param name="beginRow"></param>
        /// <param name="beginCol"></param>
        /// <param name="endRow"></param>
        /// <param name="endCol"></param>
        public void DrawLine(string color, out double beginRow, out double beginCol, out double endRow, out double endCol)
        {
            try
            {

                Double _beginRow, _beginCol, _endRow, _endCol;
                //ShieldMouseEvent();

               this.viewWindow._hWndControl.isDrawLine = true;

                mCtrl_HWindow.Focus();
                hv_window.SetColor(color);
                hv_window.DrawLine(out _beginRow, out _beginCol, out _endRow, out _endCol);

                hv_window.DispLine(_beginRow, _beginCol, _endRow, _endCol);
               // ReloadMouseEvent();

                beginRow = _beginRow;
                beginCol = _beginCol;
                endRow = _endRow;
                endCol = _endCol;

                this.viewWindow._hWndControl.isDrawLine = false;
            }
            catch (Exception ex)
            {
                beginRow = 0.0;
                beginCol = 0.0;
                endRow = 0.0;
                endCol = 0.0;
                m_CtrlHStatusLabelCtrl.Text = ex.Message;
            }
        }

    }


}
