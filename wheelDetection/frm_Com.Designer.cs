namespace wheelDetection
{
    partial class frm_Com
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
            this.comGroup = new System.Windows.Forms.GroupBox();
            this.addCRCcbx = new System.Windows.Forms.CheckBox();
            this.clearReceivebtn = new System.Windows.Forms.Button();
            this.clearSendbtn = new System.Windows.Forms.Button();
            this.receivetbx = new System.Windows.Forms.TextBox();
            this.sendtbx = new System.Windows.Forms.TextBox();
            this.sendbtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.handshakingcbx = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.refreshbtn = new System.Windows.Forms.Button();
            this.dataBitsCbx = new System.Windows.Forms.ComboBox();
            this.comListCbx = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.openCloseSpbtn = new System.Windows.Forms.Button();
            this.baudRateCbx = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.parityCbx = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.stopBitsCbx = new System.Windows.Forms.ComboBox();
            this.comGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // comGroup
            // 
            this.comGroup.Controls.Add(this.addCRCcbx);
            this.comGroup.Controls.Add(this.clearReceivebtn);
            this.comGroup.Controls.Add(this.clearSendbtn);
            this.comGroup.Controls.Add(this.receivetbx);
            this.comGroup.Controls.Add(this.sendtbx);
            this.comGroup.Controls.Add(this.sendbtn);
            this.comGroup.Controls.Add(this.label2);
            this.comGroup.Controls.Add(this.label10);
            this.comGroup.Controls.Add(this.handshakingcbx);
            this.comGroup.Controls.Add(this.label9);
            this.comGroup.Controls.Add(this.refreshbtn);
            this.comGroup.Controls.Add(this.dataBitsCbx);
            this.comGroup.Controls.Add(this.comListCbx);
            this.comGroup.Controls.Add(this.label3);
            this.comGroup.Controls.Add(this.openCloseSpbtn);
            this.comGroup.Controls.Add(this.baudRateCbx);
            this.comGroup.Controls.Add(this.label7);
            this.comGroup.Controls.Add(this.label4);
            this.comGroup.Controls.Add(this.parityCbx);
            this.comGroup.Controls.Add(this.label5);
            this.comGroup.Controls.Add(this.label6);
            this.comGroup.Controls.Add(this.stopBitsCbx);
            this.comGroup.Location = new System.Drawing.Point(12, 12);
            this.comGroup.Name = "comGroup";
            this.comGroup.Size = new System.Drawing.Size(651, 511);
            this.comGroup.TabIndex = 57;
            this.comGroup.TabStop = false;
            this.comGroup.Text = "串口";
            // 
            // addCRCcbx
            // 
            this.addCRCcbx.AutoSize = true;
            this.addCRCcbx.Enabled = false;
            this.addCRCcbx.Location = new System.Drawing.Point(407, 431);
            this.addCRCcbx.Margin = new System.Windows.Forms.Padding(4);
            this.addCRCcbx.Name = "addCRCcbx";
            this.addCRCcbx.Size = new System.Drawing.Size(85, 19);
            this.addCRCcbx.TabIndex = 53;
            this.addCRCcbx.Text = "Add CRC";
            this.addCRCcbx.UseVisualStyleBackColor = true;
            this.addCRCcbx.CheckedChanged += new System.EventHandler(this.addCRCcbx_CheckedChanged);
            // 
            // clearReceivebtn
            // 
            this.clearReceivebtn.AutoSize = true;
            this.clearReceivebtn.Location = new System.Drawing.Point(542, 45);
            this.clearReceivebtn.Margin = new System.Windows.Forms.Padding(4);
            this.clearReceivebtn.Name = "clearReceivebtn";
            this.clearReceivebtn.Size = new System.Drawing.Size(77, 35);
            this.clearReceivebtn.TabIndex = 45;
            this.clearReceivebtn.Text = "Clear";
            this.clearReceivebtn.UseVisualStyleBackColor = true;
            this.clearReceivebtn.Click += new System.EventHandler(this.clearReceivebtn_Click);
            // 
            // clearSendbtn
            // 
            this.clearSendbtn.Location = new System.Drawing.Point(542, 266);
            this.clearSendbtn.Margin = new System.Windows.Forms.Padding(4);
            this.clearSendbtn.Name = "clearSendbtn";
            this.clearSendbtn.Size = new System.Drawing.Size(77, 31);
            this.clearSendbtn.TabIndex = 44;
            this.clearSendbtn.Text = "Clear";
            this.clearSendbtn.UseVisualStyleBackColor = true;
            this.clearSendbtn.Click += new System.EventHandler(this.clearSendbtn_Click);
            // 
            // receivetbx
            // 
            this.receivetbx.BackColor = System.Drawing.SystemColors.InfoText;
            this.receivetbx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.receivetbx.ForeColor = System.Drawing.SystemColors.Info;
            this.receivetbx.Location = new System.Drawing.Point(168, 88);
            this.receivetbx.Margin = new System.Windows.Forms.Padding(4);
            this.receivetbx.Multiline = true;
            this.receivetbx.Name = "receivetbx";
            this.receivetbx.ReadOnly = true;
            this.receivetbx.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.receivetbx.Size = new System.Drawing.Size(471, 174);
            this.receivetbx.TabIndex = 43;
            this.receivetbx.TabStop = false;
            // 
            // sendtbx
            // 
            this.sendtbx.BackColor = System.Drawing.SystemColors.InfoText;
            this.sendtbx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sendtbx.ForeColor = System.Drawing.SystemColors.Info;
            this.sendtbx.Location = new System.Drawing.Point(168, 299);
            this.sendtbx.Margin = new System.Windows.Forms.Padding(4);
            this.sendtbx.Multiline = true;
            this.sendtbx.Name = "sendtbx";
            this.sendtbx.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.sendtbx.Size = new System.Drawing.Size(471, 122);
            this.sendtbx.TabIndex = 42;
            // 
            // sendbtn
            // 
            this.sendbtn.AutoSize = true;
            this.sendbtn.Enabled = false;
            this.sendbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sendbtn.Location = new System.Drawing.Point(542, 429);
            this.sendbtn.Margin = new System.Windows.Forms.Padding(4);
            this.sendbtn.Name = "sendbtn";
            this.sendbtn.Size = new System.Drawing.Size(81, 45);
            this.sendbtn.TabIndex = 41;
            this.sendbtn.Text = "Send";
            this.sendbtn.UseVisualStyleBackColor = true;
            this.sendbtn.Click += new System.EventHandler(this.sendbtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(166, 276);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 15);
            this.label2.TabIndex = 40;
            this.label2.Text = "Send:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(166, 64);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 15);
            this.label10.TabIndex = 39;
            this.label10.Text = "Received:";
            // 
            // handshakingcbx
            // 
            this.handshakingcbx.FormattingEnabled = true;
            this.handshakingcbx.Location = new System.Drawing.Point(37, 287);
            this.handshakingcbx.Margin = new System.Windows.Forms.Padding(4);
            this.handshakingcbx.Name = "handshakingcbx";
            this.handshakingcbx.Size = new System.Drawing.Size(97, 23);
            this.handshakingcbx.TabIndex = 38;
            this.handshakingcbx.SelectedIndexChanged += new System.EventHandler(this.handshakingcbx_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(27, 268);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(103, 15);
            this.label9.TabIndex = 37;
            this.label9.Text = "HandShaking:";
            // 
            // refreshbtn
            // 
            this.refreshbtn.Location = new System.Drawing.Point(37, 327);
            this.refreshbtn.Margin = new System.Windows.Forms.Padding(4);
            this.refreshbtn.Name = "refreshbtn";
            this.refreshbtn.Size = new System.Drawing.Size(99, 40);
            this.refreshbtn.TabIndex = 36;
            this.refreshbtn.Text = "Refersh";
            this.refreshbtn.UseVisualStyleBackColor = true;
            this.refreshbtn.Click += new System.EventHandler(this.refreshbtn_Click);
            // 
            // dataBitsCbx
            // 
            this.dataBitsCbx.FormattingEnabled = true;
            this.dataBitsCbx.Location = new System.Drawing.Point(37, 137);
            this.dataBitsCbx.Margin = new System.Windows.Forms.Padding(4);
            this.dataBitsCbx.Name = "dataBitsCbx";
            this.dataBitsCbx.Size = new System.Drawing.Size(97, 23);
            this.dataBitsCbx.TabIndex = 29;
            this.dataBitsCbx.SelectedIndexChanged += new System.EventHandler(this.dataBitsCbx_SelectedIndexChanged);
            // 
            // comListCbx
            // 
            this.comListCbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comListCbx.FormattingEnabled = true;
            this.comListCbx.Location = new System.Drawing.Point(37, 37);
            this.comListCbx.Margin = new System.Windows.Forms.Padding(4);
            this.comListCbx.Name = "comListCbx";
            this.comListCbx.Size = new System.Drawing.Size(97, 23);
            this.comListCbx.TabIndex = 25;
            this.comListCbx.SelectedIndexChanged += new System.EventHandler(this.comListCbx_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 18);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 15);
            this.label3.TabIndex = 26;
            this.label3.Text = "Port Name:";
            // 
            // openCloseSpbtn
            // 
            this.openCloseSpbtn.Enabled = false;
            this.openCloseSpbtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.openCloseSpbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openCloseSpbtn.Location = new System.Drawing.Point(37, 382);
            this.openCloseSpbtn.Margin = new System.Windows.Forms.Padding(4);
            this.openCloseSpbtn.Name = "openCloseSpbtn";
            this.openCloseSpbtn.Size = new System.Drawing.Size(99, 45);
            this.openCloseSpbtn.TabIndex = 35;
            this.openCloseSpbtn.Text = "Open";
            this.openCloseSpbtn.UseVisualStyleBackColor = true;
            this.openCloseSpbtn.Click += new System.EventHandler(this.openCloseSpbtn_Click);
            // 
            // baudRateCbx
            // 
            this.baudRateCbx.FormattingEnabled = true;
            this.baudRateCbx.Location = new System.Drawing.Point(37, 87);
            this.baudRateCbx.Margin = new System.Windows.Forms.Padding(4);
            this.baudRateCbx.Name = "baudRateCbx";
            this.baudRateCbx.Size = new System.Drawing.Size(97, 23);
            this.baudRateCbx.TabIndex = 27;
            this.baudRateCbx.SelectedIndexChanged += new System.EventHandler(this.baudRateCbx_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 218);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 15);
            this.label7.TabIndex = 34;
            this.label7.Text = "Parity:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 68);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 15);
            this.label4.TabIndex = 28;
            this.label4.Text = "Baud Rate:";
            // 
            // parityCbx
            // 
            this.parityCbx.FormattingEnabled = true;
            this.parityCbx.Location = new System.Drawing.Point(37, 237);
            this.parityCbx.Margin = new System.Windows.Forms.Padding(4);
            this.parityCbx.Name = "parityCbx";
            this.parityCbx.Size = new System.Drawing.Size(97, 23);
            this.parityCbx.TabIndex = 33;
            this.parityCbx.SelectedIndexChanged += new System.EventHandler(this.parityCbx_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 118);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 15);
            this.label5.TabIndex = 30;
            this.label5.Text = "Data Bits:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 168);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 15);
            this.label6.TabIndex = 32;
            this.label6.Text = "Stop Bits:";
            // 
            // stopBitsCbx
            // 
            this.stopBitsCbx.FormattingEnabled = true;
            this.stopBitsCbx.Location = new System.Drawing.Point(37, 187);
            this.stopBitsCbx.Margin = new System.Windows.Forms.Padding(4);
            this.stopBitsCbx.Name = "stopBitsCbx";
            this.stopBitsCbx.Size = new System.Drawing.Size(97, 23);
            this.stopBitsCbx.TabIndex = 31;
            this.stopBitsCbx.SelectedIndexChanged += new System.EventHandler(this.stopBitsCbx_SelectedIndexChanged);
            // 
            // frm_Com
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 535);
            this.Controls.Add(this.comGroup);
            this.Name = "frm_Com";
            this.Text = "frm_Com";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_Com_FormClosing);
            this.comGroup.ResumeLayout(false);
            this.comGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox comGroup;
        private System.Windows.Forms.ComboBox handshakingcbx;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button refreshbtn;
        private System.Windows.Forms.ComboBox dataBitsCbx;
        private System.Windows.Forms.ComboBox comListCbx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button openCloseSpbtn;
        private System.Windows.Forms.ComboBox baudRateCbx;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox parityCbx;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox stopBitsCbx;
        private System.Windows.Forms.CheckBox addCRCcbx;
        private System.Windows.Forms.Button clearReceivebtn;
        private System.Windows.Forms.Button clearSendbtn;
        private System.Windows.Forms.TextBox receivetbx;
        private System.Windows.Forms.TextBox sendtbx;
        private System.Windows.Forms.Button sendbtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label10;
    }
}