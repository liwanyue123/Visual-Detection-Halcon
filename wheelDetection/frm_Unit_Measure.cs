using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
 
using System.Windows.Forms;
using HalconDotNet;
using FormFinalDesign;
using SharedData;
namespace wheelDetection
{

    public delegate void   MeasureFormDrawHandle();

    public delegate void MeasureFormDetectionHandle();//我关闭的时候，主窗体要执行什么

    public delegate void MeasureFormClosingHandle();
    public delegate void SendInfToMainFormHandle(MeasureRectangleData data);

    public partial class frm_Unit_Measure : Form
    {
        //Form1 Mainform;
        public event MeasureFormDrawHandle MeasureFormDrawEvent;
        public event MeasureFormDetectionHandle MeasureFormDetectionEvent;//我关闭的时候，主窗体要执行什么
        public event MeasureFormClosingHandle MeasureFormClosing;
        public event SendInfToMainFormHandle SendInfToMainFormEvent;

        MeasureRectangleData localData;
        //Rectangle1Inf localData.rectangle1Inf;
        //Rectangle2Inf localData.rectangle2Inf;
        //localData.otherRectParam localData.otherRectParam;//其他框子需要的信息
        Form1 Mainform;
        public frm_Unit_Measure(Form1 Mainform, MeasureRectangleData e)
        {
            InitializeComponent();
            this.Mainform = Mainform;
            MeasureFormClosing += Mainform.MeasureFormClosing;
            MeasureFormDrawEvent+= Mainform.MeasureFormDrawEvent   ;
            MeasureFormDetectionEvent+=Mainform.MeasureFormDetectionEvent ;
            SendInfToMainFormEvent += Mainform.SendInfToMainFormEvent;

            this.StartPosition = FormStartPosition.Manual; //窗体的位置由Location属性决定
            this.Location = (Point)new Size(100, 100); //窗体的起始位置为0,0 
                                                       //this.Mainform = Mainform;


            localData = e;
            if (localData.rectangle1Inf == null)
            {
                localData.rectangle1Inf = new Rectangle1Inf();
            }
            if(localData.rectangle2Inf  == null)
            {
                localData.rectangle2Inf = new Rectangle2Inf();
            }
            if (localData.otherRectParam == null)
            {
                localData.otherRectParam = new OtherRectParam(40,90,5,190,1000,1000,5,true);
            }
           


            //this.Location = new System.Drawing.Point(700, 100);
            // btn_drawDetection.Enabled = false;
            ShowInitData();
            //setCurrentRectPosToForm();
            //大框子
           //赋值并显示出来

            //小框子
        }
        /// <summary>
        /// ///////////////////控件事件///////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //Mainform.pixPercent = trackBar1.Value;
            txt_PixPercentOfRubbish.Text = trackBar1.Value.ToString() + "%";
            this.localData.otherRectParam.pixPercentOfRubbish = trackBar1.Value;
        }
        
 

        private void txt_lineWidth_ValueChanged(object sender, EventArgs e)
        {
            this.localData.otherRectParam.lineWidth = (int)txt_lineWidth.Value;
        }

        private void txt_threadold_ValueChanged(object sender, EventArgs e)
        {
            this.localData.otherRectParam.threadold = (double)txt_threadold.Value;
        }

        private void btn_MainFormDraw_Click(object sender, EventArgs e)
        {
            MeasureFormDrawEvent();
            // 0:点 //1:直线//2:圆//3:矩形//4:角度矩形//5:扇形
            // btn_drawDetection.Enabled = true;
        }
        //生成检测结果
        private void btn_drawDetection_Click(object sender, EventArgs e)
        {
   
            //不只是检测按钮，还顺便保存之前设定的信息

            //data.localData.otherRectParam.pixPercentOfRubbish = trackBar1.Value;
            //data.localData.otherRectParam.pixPercentOfRoller = trackBar2.Value;
            //给主form数据
            ////////////////////////
            //Mainform.GiveFormDataEvent(data);

            SendInfToMainFormEvent(localData);
            MeasureFormDetectionEvent();
            //存储
           SaveDataTool.Save2File(localData, @"C:/detectionData/RectData.ev");//存储一下


        }

        private void frm_Unit_Measure_FormClosing(object sender, FormClosingEventArgs e)//把框子删掉
        {

            MeasureFormClosing();
            MeasureFormClosing -= Mainform.MeasureFormClosing;
            MeasureFormDrawEvent -= Mainform.MeasureFormDrawEvent;
            MeasureFormDetectionEvent -= Mainform.MeasureFormDetectionEvent;
            //主窗体要执行


        }



 
        /// <summary>
        /// /////////////////////parentForm->chlidForm///////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="localData.rectangle1Inf"></param>
        /// 

        public void ShowInitData()
        {
            if(localData.rectangle1Inf!=null)
            {
                txt_StartX.Text = ((int)localData.rectangle1Inf.mainColumn1).ToString();//bug,rect 不存在
                txt_StartY.Text = ((int)localData.rectangle1Inf.mainRow1).ToString();
                txt_EndX.Text = ((int)localData.rectangle1Inf.mainColumn2).ToString();
                txt_EndY.Text = ((int)localData.rectangle1Inf.mainRow2).ToString();
            }
          

            if(localData.rectangle2Inf!=null)
            {
                txt_DetStartX.Text = ((int)localData.rectangle2Inf.DetRow).ToString();
                txt_DetStartY.Text = ((int)localData.rectangle2Inf.DetCol).ToString();
                txt_DetLen1.Text = ((int)localData.rectangle2Inf.Detlen1).ToString();
                txt_DetLen2.Text = ((int)localData.rectangle2Inf.Detlen2).ToString();
                txt_DetAngle.Text = (localData.rectangle2Inf.DetAngle).ToString();
            }
          
            if(localData.otherRectParam!=null)
            {

                txt_delay_Rubbish.Value = localData.otherRectParam.delayRubbishDetectionTime;//延时
                txt_delay_Roller.Value = localData.otherRectParam.delayRollerDetectionTime;
                txt_lineWidth.Value = localData.otherRectParam.lineWidth;//线宽
                txt_threadold.Value = (int)localData.otherRectParam.threadold;//阈值
                txt_PixPercentOfRubbish.Text = localData.otherRectParam.pixPercentOfRubbish.ToString() + "%";//垃圾百分比
                trackBar1.Value = localData.otherRectParam.pixPercentOfRubbish;
                txt_PixPercentOfRoller.Text = localData.otherRectParam.pixPercentOfRoller.ToString() + "%";//轮子百分比
                trackBar2.Value = localData.otherRectParam.pixPercentOfRoller;
                checkBox1.Checked = localData.otherRectParam.isMatch;//是否匹配
                txt_NumMiss.Value = localData.otherRectParam.numMiss;
            }

        }
 
        public void GetGrayValFromMainForm(int grayVal)
        {
            txt_grayVal.Text = grayVal.ToString();
        }
        public void GetRect1PosFromMainForm(Object sender, Rectangle1Inf rect1)
        {
            this.localData.rectangle1Inf = rect1;
            txt_StartX.Text = ((int)localData.rectangle1Inf.mainColumn1).ToString();
            txt_StartY.Text = ((int)localData.rectangle1Inf.mainRow1).ToString();
            txt_EndX.Text = ((int)localData.rectangle1Inf.mainColumn2).ToString();
            txt_EndY.Text = ((int)localData.rectangle1Inf.mainRow2).ToString();
        }
        public void GetRect2PosFromMainForm(Object sender, Rectangle2Inf rect2)
        {
            this.localData.rectangle2Inf = rect2;
            txt_DetStartY.Text = ((int)localData.rectangle2Inf.DetRow).ToString();
            txt_DetStartX.Text = ((int)localData.rectangle2Inf.DetCol).ToString();
            txt_DetLen1.Text = ((int)localData.rectangle2Inf.Detlen1).ToString();
            txt_DetLen2.Text = ((int)localData.rectangle2Inf.Detlen2).ToString();
            txt_DetAngle.Text = (localData.rectangle2Inf.DetAngle).ToString();
        }

 
        /// <summary>
        /// /////////////////////chlidForm->parentForm///////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="localData.rectangle1Inf"></param>
        //public void SetDataToParentForm( )




        //矩形和线通用，矩形返回左上，右下2点。线返回首尾2点
        public void setCurrentLinePositionToForm(Object sender, double row1, double col1, double row2, double col2)
        {
            //this.MatchCol1 = col1;
            //this.MatchRow1 = row1;
            //this.MatchCol2 = col2;
            //this.MatchRow2 = row2;

            txt_StartX.Text = ((int)col1).ToString();
            txt_StartY.Text = ((int)row1).ToString();
            txt_EndX.Text = ((int)col2).ToString();
            txt_EndY.Text = ((int)row2).ToString();
        }

        private void txt_delay_Rubbish_ValueChanged(object sender, EventArgs e)
        {
            this.localData.otherRectParam.delayRubbishDetectionTime = (int)txt_delay_Rubbish.Value;
        }

        private void txt_delay_Roller_ValueChanged(object sender, EventArgs e)
        {
            this.localData.otherRectParam.delayRollerDetectionTime = (int)txt_delay_Roller.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            txt_PixPercentOfRoller.Text = trackBar2.Value.ToString() + "%";
            this.localData.otherRectParam.pixPercentOfRoller = trackBar2.Value;


        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            this.localData.otherRectParam.numMiss = (int)txt_NumMiss.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.localData.otherRectParam.isMatch = checkBox1.Checked;
        }

        //this.localData.otherRectParam.delay = (int)txt_delay_Rubbish.Value;
    }
}
