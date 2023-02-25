namespace wheelDetection
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCamera = new System.Windows.Forms.ComboBox();
            this.messageBox = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.openCloseSpbtn = new System.Windows.Forms.Button();
            this.toolStripStatusTx = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statuslabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusRx = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusTimeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statustimer = new System.Windows.Forms.Timer(this.components);
            this.comGroup = new System.Windows.Forms.GroupBox();
            this.btn_ConfigureCom = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.btn_calibration = new System.Windows.Forms.Button();
            this.btn_All = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lab_light = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.hWindow_Final1 = new ChoiceTech.Halcon.Control.HWindow_Final();
            this.statusStrip1.SuspendLayout();
            this.comGroup.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 30);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "摄像设备";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // cmbCamera
            // 
            this.cmbCamera.FormattingEnabled = true;
            this.cmbCamera.Location = new System.Drawing.Point(110, 27);
            this.cmbCamera.Margin = new System.Windows.Forms.Padding(4);
            this.cmbCamera.Name = "cmbCamera";
            this.cmbCamera.Size = new System.Drawing.Size(167, 26);
            this.cmbCamera.TabIndex = 1;
            this.cmbCamera.SelectedIndexChanged += new System.EventHandler(this.cmbCamera_SelectedIndexChanged);
            // 
            // messageBox
            // 
            this.messageBox.Location = new System.Drawing.Point(7, 623);
            this.messageBox.Margin = new System.Windows.Forms.Padding(4);
            this.messageBox.Multiline = true;
            this.messageBox.Name = "messageBox";
            this.messageBox.Size = new System.Drawing.Size(337, 222);
            this.messageBox.TabIndex = 3;
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConnect.Location = new System.Drawing.Point(59, 76);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(177, 42);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // openCloseSpbtn
            // 
            this.openCloseSpbtn.Enabled = false;
            this.openCloseSpbtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.openCloseSpbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openCloseSpbtn.Location = new System.Drawing.Point(59, 41);
            this.openCloseSpbtn.Margin = new System.Windows.Forms.Padding(4);
            this.openCloseSpbtn.Name = "openCloseSpbtn";
            this.openCloseSpbtn.Size = new System.Drawing.Size(177, 42);
            this.openCloseSpbtn.TabIndex = 35;
            this.openCloseSpbtn.Text = "Open";
            this.openCloseSpbtn.UseVisualStyleBackColor = true;
            this.openCloseSpbtn.Click += new System.EventHandler(this.openCloseSpbtn_Click);
            // 
            // toolStripStatusTx
            // 
            this.toolStripStatusTx.Name = "toolStripStatusTx";
            this.toolStripStatusTx.Size = new System.Drawing.Size(634, 20);
            this.toolStripStatusTx.Spring = true;
            this.toolStripStatusTx.Text = "Sent:";
            this.toolStripStatusTx.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statuslabel,
            this.toolStripStatusLabel1,
            this.toolStripStatusRx,
            this.toolStripStatusTx,
            this.statusTimeLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 906);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1924, 25);
            this.statusStrip1.TabIndex = 55;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statuslabel
            // 
            this.statuslabel.ActiveLinkColor = System.Drawing.SystemColors.ButtonHighlight;
            this.statuslabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statuslabel.Name = "statuslabel";
            this.statuslabel.Size = new System.Drawing.Size(634, 20);
            this.statuslabel.Spring = true;
            this.statuslabel.Text = "Not Connected";
            this.statuslabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 20);
            // 
            // toolStripStatusRx
            // 
            this.toolStripStatusRx.ActiveLinkColor = System.Drawing.SystemColors.Info;
            this.toolStripStatusRx.Name = "toolStripStatusRx";
            this.toolStripStatusRx.Size = new System.Drawing.Size(634, 20);
            this.toolStripStatusRx.Spring = true;
            this.toolStripStatusRx.Text = "Received:";
            this.toolStripStatusRx.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusTimeLabel
            // 
            this.statusTimeLabel.Name = "statusTimeLabel";
            this.statusTimeLabel.Size = new System.Drawing.Size(0, 20);
            // 
            // statustimer
            // 
            this.statustimer.Enabled = true;
            this.statustimer.Interval = 10;
            this.statustimer.Tick += new System.EventHandler(this.statustimer_Tick);
            // 
            // comGroup
            // 
            this.comGroup.Controls.Add(this.btn_ConfigureCom);
            this.comGroup.Controls.Add(this.openCloseSpbtn);
            this.comGroup.Font = new System.Drawing.Font("宋体", 10.8F);
            this.comGroup.Location = new System.Drawing.Point(23, 272);
            this.comGroup.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comGroup.Name = "comGroup";
            this.comGroup.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comGroup.Size = new System.Drawing.Size(296, 194);
            this.comGroup.TabIndex = 56;
            this.comGroup.TabStop = false;
            this.comGroup.Text = "串口";
            // 
            // btn_ConfigureCom
            // 
            this.btn_ConfigureCom.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_ConfigureCom.Location = new System.Drawing.Point(59, 118);
            this.btn_ConfigureCom.Margin = new System.Windows.Forms.Padding(4);
            this.btn_ConfigureCom.Name = "btn_ConfigureCom";
            this.btn_ConfigureCom.Size = new System.Drawing.Size(177, 42);
            this.btn_ConfigureCom.TabIndex = 62;
            this.btn_ConfigureCom.Text = "设置串口";
            this.btn_ConfigureCom.UseVisualStyleBackColor = true;
            this.btn_ConfigureCom.Click += new System.EventHandler(this.btn_ConfigureCom_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(7, 583);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 19);
            this.label12.TabIndex = 58;
            this.label12.Text = "设备信息";
            // 
            // btn_calibration
            // 
            this.btn_calibration.Location = new System.Drawing.Point(59, 138);
            this.btn_calibration.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_calibration.Name = "btn_calibration";
            this.btn_calibration.Size = new System.Drawing.Size(177, 42);
            this.btn_calibration.TabIndex = 60;
            this.btn_calibration.Text = "标定";
            this.btn_calibration.UseVisualStyleBackColor = true;
            this.btn_calibration.Click += new System.EventHandler(this.btn_calibration_Click);
            // 
            // btn_All
            // 
            this.btn_All.Font = new System.Drawing.Font("宋体", 10.8F);
            this.btn_All.Location = new System.Drawing.Point(68, 486);
            this.btn_All.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_All.Name = "btn_All";
            this.btn_All.Size = new System.Drawing.Size(205, 68);
            this.btn_All.TabIndex = 61;
            this.btn_All.Text = "启动";
            this.btn_All.UseVisualStyleBackColor = true;
            this.btn_All.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbCamera);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnConnect);
            this.groupBox2.Controls.Add(this.btn_calibration);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 10.8F);
            this.groupBox2.Location = new System.Drawing.Point(23, 38);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(297, 202);
            this.groupBox2.TabIndex = 62;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "相机";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lab_light);
            this.groupBox1.Controls.Add(this.comGroup);
            this.groupBox1.Controls.Add(this.btn_All);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.messageBox);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(1570, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(351, 875);
            this.groupBox1.TabIndex = 63;
            this.groupBox1.TabStop = false;
            // 
            // lab_light
            // 
            this.lab_light.AutoSize = true;
            this.lab_light.Font = new System.Drawing.Font("宋体", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_light.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.lab_light.Location = new System.Drawing.Point(114, 571);
            this.lab_light.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lab_light.Name = "lab_light";
            this.lab_light.Size = new System.Drawing.Size(68, 48);
            this.lab_light.TabIndex = 63;
            this.lab_light.Text = "●";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.49688F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.50312F));
            this.tableLayoutPanel1.Controls.Add(this.hWindow_Final1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 97.13024F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.869757F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1924, 906);
            this.tableLayoutPanel1.TabIndex = 64;
            // 
            // hWindow_Final1
            // 
            this.hWindow_Final1.BackColor = System.Drawing.Color.Transparent;
            this.hWindow_Final1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hWindow_Final1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindow_Final1.DrawModel = false;
            this.hWindow_Final1.EditModel = true;
            this.hWindow_Final1.Image = null;
            this.hWindow_Final1.Location = new System.Drawing.Point(5, 5);
            this.hWindow_Final1.Margin = new System.Windows.Forms.Padding(5);
            this.hWindow_Final1.Name = "hWindow_Final1";
            this.hWindow_Final1.Size = new System.Drawing.Size(1557, 869);
            this.hWindow_Final1.TabIndex = 59;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1924, 931);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.comGroup.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbCamera;
        private System.Windows.Forms.TextBox messageBox;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button openCloseSpbtn;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusTx;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statuslabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusRx;
        private System.Windows.Forms.ToolStripStatusLabel statusTimeLabel;
        private System.Windows.Forms.Timer statustimer;
        private System.Windows.Forms.GroupBox comGroup;
        private System.Windows.Forms.Label label12;
        public ChoiceTech.Halcon.Control.HWindow_Final hWindow_Final1;
        private System.Windows.Forms.Button btn_calibration;
        private System.Windows.Forms.Button btn_All;
        private System.Windows.Forms.Button btn_ConfigureCom;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lab_light;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

