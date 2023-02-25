namespace wheelDetection
{
    partial class frm_Unit_Measure
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.txt_grayVal = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.txt_NumMiss = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.txt_delay_Roller = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.txt_PixPercentOfRoller = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.txt_threadold = new System.Windows.Forms.NumericUpDown();
            this.txt_delay_Rubbish = new System.Windows.Forms.NumericUpDown();
            this.txt_lineWidth = new System.Windows.Forms.NumericUpDown();
            this.txt_PixPercentOfRubbish = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.btn_drawDetection = new System.Windows.Forms.Button();
            this.btn_MainFormDraw = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txt_DetAngle = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txt_DetLen2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_DetLen1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_DetStartY = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_DetStartX = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txt_EndY = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txt_EndX = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txt_StartY = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txt_StartX = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_NumMiss)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_delay_Roller)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_threadold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_delay_Rubbish)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_lineWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 204);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 15);
            this.label3.TabIndex = 38;
            this.label3.Text = "阈值  :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 160);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 15);
            this.label7.TabIndex = 34;
            this.label7.Text = "宽度(pix) :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 30);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 15);
            this.label4.TabIndex = 32;
            this.label4.Text = "延迟检测(杂物)：";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(697, 797);
            this.tabControl1.TabIndex = 49;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox8);
            this.tabPage1.Controls.Add(this.groupBox7);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(689, 768);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.txt_grayVal);
            this.groupBox8.Controls.Add(this.label10);
            this.groupBox8.Location = new System.Drawing.Point(31, 679);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(302, 67);
            this.groupBox8.TabIndex = 52;
            this.groupBox8.TabStop = false;
            // 
            // txt_grayVal
            // 
            this.txt_grayVal.Location = new System.Drawing.Point(143, 27);
            this.txt_grayVal.Margin = new System.Windows.Forms.Padding(4);
            this.txt_grayVal.Name = "txt_grayVal";
            this.txt_grayVal.Size = new System.Drawing.Size(132, 25);
            this.txt_grayVal.TabIndex = 41;
            this.txt_grayVal.Text = "10";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(51, 30);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 15);
            this.label10.TabIndex = 40;
            this.label10.Text = "灰度值 :";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label18);
            this.groupBox7.Controls.Add(this.checkBox1);
            this.groupBox7.Controls.Add(this.txt_NumMiss);
            this.groupBox7.Controls.Add(this.label17);
            this.groupBox7.Controls.Add(this.txt_delay_Roller);
            this.groupBox7.Controls.Add(this.label12);
            this.groupBox7.Controls.Add(this.txt_PixPercentOfRoller);
            this.groupBox7.Controls.Add(this.label11);
            this.groupBox7.Controls.Add(this.trackBar2);
            this.groupBox7.Controls.Add(this.txt_threadold);
            this.groupBox7.Controls.Add(this.txt_delay_Rubbish);
            this.groupBox7.Controls.Add(this.txt_lineWidth);
            this.groupBox7.Controls.Add(this.txt_PixPercentOfRubbish);
            this.groupBox7.Controls.Add(this.label9);
            this.groupBox7.Controls.Add(this.trackBar1);
            this.groupBox7.Controls.Add(this.btn_drawDetection);
            this.groupBox7.Controls.Add(this.label4);
            this.groupBox7.Controls.Add(this.btn_MainFormDraw);
            this.groupBox7.Controls.Add(this.label7);
            this.groupBox7.Controls.Add(this.label3);
            this.groupBox7.Location = new System.Drawing.Point(31, 7);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(636, 346);
            this.groupBox7.TabIndex = 51;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "设定";
            // 
            // txt_NumMiss
            // 
            this.txt_NumMiss.Location = new System.Drawing.Point(161, 111);
            this.txt_NumMiss.Name = "txt_NumMiss";
            this.txt_NumMiss.Size = new System.Drawing.Size(120, 25);
            this.txt_NumMiss.TabIndex = 58;
            this.txt_NumMiss.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.txt_NumMiss.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(26, 116);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(128, 15);
            this.label17.TabIndex = 57;
            this.label17.Text = "缺失次数(报错)：";
            // 
            // txt_delay_Roller
            // 
            this.txt_delay_Roller.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.txt_delay_Roller.Location = new System.Drawing.Point(161, 66);
            this.txt_delay_Roller.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.txt_delay_Roller.Name = "txt_delay_Roller";
            this.txt_delay_Roller.Size = new System.Drawing.Size(120, 25);
            this.txt_delay_Roller.TabIndex = 56;
            this.txt_delay_Roller.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.txt_delay_Roller.ValueChanged += new System.EventHandler(this.txt_delay_Roller_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(26, 72);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(128, 15);
            this.label12.TabIndex = 55;
            this.label12.Text = "延迟检测(轮子)：";
            // 
            // txt_PixPercentOfRoller
            // 
            this.txt_PixPercentOfRoller.Location = new System.Drawing.Point(541, 140);
            this.txt_PixPercentOfRoller.Margin = new System.Windows.Forms.Padding(4);
            this.txt_PixPercentOfRoller.Name = "txt_PixPercentOfRoller";
            this.txt_PixPercentOfRoller.Size = new System.Drawing.Size(68, 25);
            this.txt_PixPercentOfRoller.TabIndex = 54;
            this.txt_PixPercentOfRoller.Text = "80%";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(357, 146);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(159, 15);
            this.label11.TabIndex = 53;
            this.label11.Text = "轮子像素比例 (大于):";
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(349, 176);
            this.trackBar2.Maximum = 100;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(269, 56);
            this.trackBar2.SmallChange = 10;
            this.trackBar2.TabIndex = 52;
            this.trackBar2.Value = 80;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // txt_threadold
            // 
            this.txt_threadold.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.txt_threadold.Location = new System.Drawing.Point(161, 198);
            this.txt_threadold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.txt_threadold.Name = "txt_threadold";
            this.txt_threadold.Size = new System.Drawing.Size(120, 25);
            this.txt_threadold.TabIndex = 51;
            this.txt_threadold.Value = new decimal(new int[] {
            190,
            0,
            0,
            0});
            this.txt_threadold.ValueChanged += new System.EventHandler(this.txt_threadold_ValueChanged);
            // 
            // txt_delay_Rubbish
            // 
            this.txt_delay_Rubbish.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.txt_delay_Rubbish.Location = new System.Drawing.Point(161, 23);
            this.txt_delay_Rubbish.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.txt_delay_Rubbish.Name = "txt_delay_Rubbish";
            this.txt_delay_Rubbish.Size = new System.Drawing.Size(120, 25);
            this.txt_delay_Rubbish.TabIndex = 50;
            this.txt_delay_Rubbish.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.txt_delay_Rubbish.ValueChanged += new System.EventHandler(this.txt_delay_Rubbish_ValueChanged);
            // 
            // txt_lineWidth
            // 
            this.txt_lineWidth.Location = new System.Drawing.Point(161, 154);
            this.txt_lineWidth.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.txt_lineWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txt_lineWidth.Name = "txt_lineWidth";
            this.txt_lineWidth.Size = new System.Drawing.Size(120, 25);
            this.txt_lineWidth.TabIndex = 49;
            this.txt_lineWidth.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.txt_lineWidth.ValueChanged += new System.EventHandler(this.txt_lineWidth_ValueChanged);
            // 
            // txt_PixPercentOfRubbish
            // 
            this.txt_PixPercentOfRubbish.Location = new System.Drawing.Point(541, 23);
            this.txt_PixPercentOfRubbish.Margin = new System.Windows.Forms.Padding(4);
            this.txt_PixPercentOfRubbish.Name = "txt_PixPercentOfRubbish";
            this.txt_PixPercentOfRubbish.Size = new System.Drawing.Size(68, 25);
            this.txt_PixPercentOfRubbish.TabIndex = 47;
            this.txt_PixPercentOfRubbish.Text = "80%";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(357, 29);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(159, 15);
            this.label9.TabIndex = 46;
            this.label9.Text = "杂物像素比例 (小于):";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(349, 60);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(269, 56);
            this.trackBar1.SmallChange = 10;
            this.trackBar1.TabIndex = 45;
            this.trackBar1.Value = 80;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // btn_drawDetection
            // 
            this.btn_drawDetection.Location = new System.Drawing.Point(388, 289);
            this.btn_drawDetection.Name = "btn_drawDetection";
            this.btn_drawDetection.Size = new System.Drawing.Size(190, 38);
            this.btn_drawDetection.TabIndex = 41;
            this.btn_drawDetection.Text = "生成检测结果";
            this.btn_drawDetection.UseVisualStyleBackColor = true;
            this.btn_drawDetection.Click += new System.EventHandler(this.btn_drawDetection_Click);
            // 
            // btn_MainFormDraw
            // 
            this.btn_MainFormDraw.Location = new System.Drawing.Point(68, 289);
            this.btn_MainFormDraw.Name = "btn_MainFormDraw";
            this.btn_MainFormDraw.Size = new System.Drawing.Size(190, 38);
            this.btn_MainFormDraw.TabIndex = 40;
            this.btn_MainFormDraw.Text = "重新绘制";
            this.btn_MainFormDraw.UseVisualStyleBackColor = true;
            this.btn_MainFormDraw.Click += new System.EventHandler(this.btn_MainFormDraw_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txt_DetAngle);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.groupBox6);
            this.groupBox1.Location = new System.Drawing.Point(365, 372);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(302, 374);
            this.groupBox1.TabIndex = 50;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "判断框(小)";
            // 
            // txt_DetAngle
            // 
            this.txt_DetAngle.Location = new System.Drawing.Point(143, 312);
            this.txt_DetAngle.Margin = new System.Windows.Forms.Padding(4);
            this.txt_DetAngle.Name = "txt_DetAngle";
            this.txt_DetAngle.Size = new System.Drawing.Size(132, 25);
            this.txt_DetAngle.TabIndex = 41;
            this.txt_DetAngle.Text = "10";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(51, 316);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 15);
            this.label8.TabIndex = 40;
            this.label8.Text = "角度(度) :";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txt_DetLen2);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.txt_DetLen1);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Location = new System.Drawing.Point(16, 158);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(269, 125);
            this.groupBox5.TabIndex = 38;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "尺寸";
            // 
            // txt_DetLen2
            // 
            this.txt_DetLen2.Location = new System.Drawing.Point(127, 83);
            this.txt_DetLen2.Margin = new System.Windows.Forms.Padding(4);
            this.txt_DetLen2.Name = "txt_DetLen2";
            this.txt_DetLen2.Size = new System.Drawing.Size(132, 25);
            this.txt_DetLen2.TabIndex = 39;
            this.txt_DetLen2.Text = "10";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 85);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 15);
            this.label1.TabIndex = 38;
            this.label1.Text = "高度 (pix) :";
            // 
            // txt_DetLen1
            // 
            this.txt_DetLen1.Location = new System.Drawing.Point(127, 35);
            this.txt_DetLen1.Margin = new System.Windows.Forms.Padding(4);
            this.txt_DetLen1.Name = "txt_DetLen1";
            this.txt_DetLen1.Size = new System.Drawing.Size(132, 25);
            this.txt_DetLen1.TabIndex = 37;
            this.txt_DetLen1.Text = "10";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 38);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 15);
            this.label2.TabIndex = 36;
            this.label2.Text = "宽度 (pix) :";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.txt_DetStartY);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.txt_DetStartX);
            this.groupBox6.Location = new System.Drawing.Point(16, 25);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox6.Size = new System.Drawing.Size(269, 125);
            this.groupBox6.TabIndex = 37;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "中心坐标";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 36);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 15);
            this.label5.TabIndex = 36;
            this.label5.Text = "X (pix) :";
            // 
            // txt_DetStartY
            // 
            this.txt_DetStartY.Location = new System.Drawing.Point(127, 81);
            this.txt_DetStartY.Margin = new System.Windows.Forms.Padding(4);
            this.txt_DetStartY.Name = "txt_DetStartY";
            this.txt_DetStartY.Size = new System.Drawing.Size(132, 25);
            this.txt_DetStartY.TabIndex = 35;
            this.txt_DetStartY.Text = "5";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(40, 84);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 15);
            this.label6.TabIndex = 34;
            this.label6.Text = "Y (pix) :";
            // 
            // txt_DetStartX
            // 
            this.txt_DetStartX.Location = new System.Drawing.Point(127, 32);
            this.txt_DetStartX.Margin = new System.Windows.Forms.Padding(4);
            this.txt_DetStartX.Name = "txt_DetStartX";
            this.txt_DetStartX.Size = new System.Drawing.Size(132, 25);
            this.txt_DetStartX.TabIndex = 33;
            this.txt_DetStartX.Text = "5";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(31, 372);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(302, 301);
            this.groupBox2.TabIndex = 49;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "匹配框(大)";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txt_EndY);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.txt_EndX);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Location = new System.Drawing.Point(16, 158);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(269, 125);
            this.groupBox4.TabIndex = 38;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "终点坐标";
            // 
            // txt_EndY
            // 
            this.txt_EndY.Location = new System.Drawing.Point(127, 83);
            this.txt_EndY.Margin = new System.Windows.Forms.Padding(4);
            this.txt_EndY.Name = "txt_EndY";
            this.txt_EndY.Size = new System.Drawing.Size(132, 25);
            this.txt_EndY.TabIndex = 39;
            this.txt_EndY.Text = "10";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(35, 85);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(79, 15);
            this.label14.TabIndex = 38;
            this.label14.Text = "Y (pix) :";
            // 
            // txt_EndX
            // 
            this.txt_EndX.Location = new System.Drawing.Point(127, 35);
            this.txt_EndX.Margin = new System.Windows.Forms.Padding(4);
            this.txt_EndX.Name = "txt_EndX";
            this.txt_EndX.Size = new System.Drawing.Size(132, 25);
            this.txt_EndX.TabIndex = 37;
            this.txt_EndX.Text = "10";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(35, 38);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(79, 15);
            this.label15.TabIndex = 36;
            this.label15.Text = "X (pix) :";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.txt_StartY);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.txt_StartX);
            this.groupBox3.Location = new System.Drawing.Point(16, 25);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(269, 125);
            this.groupBox3.TabIndex = 37;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "起点坐标";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(40, 36);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(79, 15);
            this.label16.TabIndex = 36;
            this.label16.Text = "X (pix) :";
            // 
            // txt_StartY
            // 
            this.txt_StartY.Location = new System.Drawing.Point(127, 81);
            this.txt_StartY.Margin = new System.Windows.Forms.Padding(4);
            this.txt_StartY.Name = "txt_StartY";
            this.txt_StartY.Size = new System.Drawing.Size(132, 25);
            this.txt_StartY.TabIndex = 35;
            this.txt_StartY.Text = "5";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(40, 84);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 15);
            this.label13.TabIndex = 34;
            this.label13.Text = "Y (pix) :";
            // 
            // txt_StartX
            // 
            this.txt_StartX.Location = new System.Drawing.Point(127, 32);
            this.txt_StartX.Margin = new System.Windows.Forms.Padding(4);
            this.txt_StartX.Name = "txt_StartX";
            this.txt_StartX.Size = new System.Drawing.Size(132, 25);
            this.txt_StartX.TabIndex = 33;
            this.txt_StartX.Text = "5";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(689, 719);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(161, 249);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(18, 17);
            this.checkBox1.TabIndex = 59;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(26, 250);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(113, 15);
            this.label18.TabIndex = 60;
            this.label18.Text = "是否重新匹配 :";
            // 
            // frm_Unit_Measure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 813);
            this.Controls.Add(this.tabControl1);
            this.Location = new System.Drawing.Point(100, 100);
            this.Name = "frm_Unit_Measure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frm_Unit_Measure";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_Unit_Measure_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_NumMiss)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_delay_Roller)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_threadold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_delay_Rubbish)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_lineWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txt_EndY;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txt_EndX;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txt_StartY;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txt_StartX;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btn_MainFormDraw;
        private System.Windows.Forms.Button btn_drawDetection;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txt_DetLen2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_DetLen1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_DetStartY;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_DetStartX;
        private System.Windows.Forms.TextBox txt_DetAngle;
        private System.Windows.Forms.Label label8;
        protected System.Windows.Forms.TextBox txt_PixPercentOfRubbish;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TextBox txt_grayVal;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown txt_threadold;
        private System.Windows.Forms.NumericUpDown txt_delay_Rubbish;
        private System.Windows.Forms.NumericUpDown txt_lineWidth;
        protected System.Windows.Forms.TextBox txt_PixPercentOfRoller;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.NumericUpDown txt_NumMiss;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown txt_delay_Roller;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}