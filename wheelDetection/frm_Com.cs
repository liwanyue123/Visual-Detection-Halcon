using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.IO.Ports;
using SharedData;
namespace wheelDetection
{
    public partial class frm_Com : Form
    {
        

        Form1 Mainform;
        public frm_Com(Form1 mainform, ComData comData)
        {
            InitializeComponent();
            this.Mainform =mainform;
            
            InitializeCOMCombox();
            if(mainform.comStatue==true)
            {
                comListCbx.Enabled = false;
                baudRateCbx.Enabled = false;
                dataBitsCbx.Enabled = false;
                stopBitsCbx.Enabled = false;
                parityCbx.Enabled = false;
                handshakingcbx.Enabled = false;
                refreshbtn.Enabled = false;
            }
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
                //Mainform.setStatuslabel(comListCbx.Text + " Opend");
 
                openCloseSpbtn.Text = "Close";
                sendbtn.Enabled = true;
                //autoSendcbx.Enabled = true;
                //autoReplyCbx.Enabled = true;

                comListCbx.Enabled = false;
                baudRateCbx.Enabled = false;
                dataBitsCbx.Enabled = false;
                stopBitsCbx.Enabled = false;
                parityCbx.Enabled = false;
                handshakingcbx.Enabled = false;
                refreshbtn.Enabled = false;
/*
                if (autoSendcbx.Checked)
                {
                    autoSendtimer.Start();
                    sendtbx.ReadOnly = true;
                }
 */
                //Mainform.setMessage("通信串口打开成功\r\n");
            }
            else    //Open failed
            {
                //Mainform.setStatuslabel("Open failed !");
 
                sendbtn.Enabled = false;
               // autoSendcbx.Enabled = false;
               // autoReplyCbx.Enabled = false;
                //messageBox.AppendText("通信串口打开失败\r\n");
                //Mainform.setMessage("通信串口打开失败\r\n");
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
                //Mainform.setStatuslabel(comListCbx.Text + " Closed");
 
                openCloseSpbtn.Text = "Open";
                //Mainform.setMessage("通信串口已关闭\r\n");

                sendbtn.Enabled = false;
                sendtbx.ReadOnly = false;
               // autoSendcbx.Enabled = false;
               // autoSendtimer.Stop();

                comListCbx.Enabled = true;
                baudRateCbx.Enabled = true;
                dataBitsCbx.Enabled = true;
                stopBitsCbx.Enabled = true;
                parityCbx.Enabled = true;
                handshakingcbx.Enabled = true;
                refreshbtn.Enabled = true;
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

            //display as hex
            //显示收到的信息
            if (receivetbx.Text.Length > 0)
            {
                receivetbx.AppendText("-");
            }
            receivetbx.AppendText(IController.Bytes2Hex(e.receivedBytes)+ "\r\n");
 
        }
        /// <summary>
        /// Initialize serial port information
        /// </summary>
        private void InitializeCOMCombox()
        {
            //BaudRate
            foreach(int e in Mainform.comConfigureInf.BaudRate)
            {

                baudRateCbx.Items.Add(e);
            }
            //baudRateCbx.Items.ToString(); 
            int i = Mainform.comConfigureInf.comData.BaudRateChoose;
            baudRateCbx.Text = baudRateCbx.Items[i].ToString();//get 9600 print in text


            //Data bits
            foreach (int e in Mainform.comConfigureInf.DataBits)
            {
                dataBitsCbx.Items.Add(e);
            }
            i = Mainform.comConfigureInf.comData.DataBitsChoose;
            dataBitsCbx.Text = dataBitsCbx.Items[i].ToString();//get the 8bit item print it in the text 


            //Stop bits
            foreach (string e in Mainform.comConfigureInf.StopBits)
            {
                stopBitsCbx.Items.Add(e);
            }
            i = Mainform.comConfigureInf.comData.StopBitsChoose; 
            stopBitsCbx.Text = stopBitsCbx.Items[i].ToString();//get the One item print in the text


            //Parity
            foreach (string e in Mainform.comConfigureInf.Parity)
            {
                parityCbx.Items.Add(e);
            }
            i = Mainform.comConfigureInf.comData.ParityChoose;
            parityCbx.Text = parityCbx.Items[i].ToString();//get the first item print in the text


            //Handshaking
            foreach (string e in Mainform.comConfigureInf.Handshakings)
            {
                handshakingcbx.Items.Add(e);
            }
            i = Mainform.comConfigureInf.comData.HandshakingsChoose;    
            handshakingcbx.Text = handshakingcbx.Items[i].ToString();


            //Com Ports 
            if (Mainform.comConfigureInf.ArrayComPortsNames.Length == 0)
            {
                //statuslabel.Text = "No COM found !";
                openCloseSpbtn.Enabled = false;
            }
            else
            {
                Array.Sort(Mainform.comConfigureInf.ArrayComPortsNames);
                for (int j = 0; j < Mainform.comConfigureInf.ArrayComPortsNames.Length; j++)
                {
                    comListCbx.Items.Add(Mainform.comConfigureInf.ArrayComPortsNames[j]);
                }
                if(Mainform.comConfigureInf.ArrayComPortsNames.Length> Mainform.comConfigureInf.comData.ArrayComPortsNamesChoose)
                     comListCbx.Text = Mainform.comConfigureInf.ArrayComPortsNames[Mainform.comConfigureInf.comData.ArrayComPortsNamesChoose];
                openCloseSpbtn.Enabled = true;
            }
            if(Mainform.comStatue==false)//当前串口关闭状态
            {
                openCloseSpbtn.Text = "Open";//可以打开
            }
            else
            {
                openCloseSpbtn.Text = "Close";//可以关闭
                refreshbtn.Enabled = false;
            }
        }


        private void refreshbtn_Click(object sender, EventArgs e)
        {
            comListCbx.Items.Clear();
            //Com Ports
            string[] ArrayComPortsNames = SerialPort.GetPortNames();
            if (ArrayComPortsNames.Length == 0)
            {
                //statuslabel.Text = "No COM found !";
                openCloseSpbtn.Enabled = false;
            }
            else
            {
                Array.Sort(ArrayComPortsNames);
                for (int i = 0; i < ArrayComPortsNames.Length; i++)
                {
                    comListCbx.Items.Add(ArrayComPortsNames[i]);
                }
                comListCbx.Text = ArrayComPortsNames[0];
                openCloseSpbtn.Enabled = true;
                //statuslabel.Text = "OK !";
            }

        }

        private void openCloseSpbtn_Click(object sender, EventArgs e)
        {
            if (openCloseSpbtn.Text == "Open")
            {
                Mainform.openCloseCom();
            }
            else
            {
                Mainform.controller.CloseSerialPort();
                //controller.CloseSerialPort();
            }
        }

        private void sendbtn_Click(object sender, EventArgs e)
        {
            String sendText = sendtbx.Text;
            //bool flag = false;
            if (sendText == null)//没有内容就不发送了
            {
                return;
            }
            //set select index to the end
            sendtbx.SelectionStart = sendtbx.TextLength;//设置光标到最后
            sendbtn.Enabled = false;//wait return
            Mainform.sendInf(sendText);
            sendbtn.Enabled = true;
        }

 


        /// <summary>
        /// Add CRC checkbox changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addCRCcbx_CheckedChanged(object sender, EventArgs e)
        {
            String sendText = sendtbx.Text;
            if (sendText == null || sendText == "")
            {
                addCRCcbx.Checked = false;
                return;
            }
            if (addCRCcbx.Checked)
            {
                //Add 2 bytes CRC to the end of the data
                Byte[] senddata = IController.Hex2Bytes(sendText);
                Byte[] crcbytes = BitConverter.GetBytes(CRC16.Compute(senddata));
                sendText += "-" + BitConverter.ToString(crcbytes, 1, 1);
                sendText += "-" + BitConverter.ToString(crcbytes, 0, 1);
            }
            else
            {
                //Delete 2 bytes CRC to the end of the data
                if (sendText.Length >= 6)
                {
                    sendText = sendText.Substring(0, sendText.Length - 6);
                }
            }
            sendtbx.Text = sendText;
        }


        /// <summary>
        /// clear text in send area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearSendbtn_Click(object sender, EventArgs e)
        {
            sendtbx.Text = "";
            //toolStripStatusTx.Text = "Sent: 0";
            //sendBytesCount = 0;
            addCRCcbx.Checked = false;
        }

        /// <summary>
        /// clear receive text in receive area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearReceivebtn_Click(object sender, EventArgs e)
        {
            receivetbx.Text = "";
            //toolStripStatusRx.Text = "Received: 0";
            //receiveBytesCount = 0;
        }

        private void frm_Com_FormClosing(object sender, FormClosingEventArgs e)
        {
            Mainform.ConfigureComClose();
        }

        private void comListCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mainform.comConfigureInf.comData.ArrayComPortsNamesChoose = comListCbx.SelectedIndex;  
// Mainform.InitialData.com = comListCbx.SelectedIndex;
        }

        private void baudRateCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mainform.comConfigureInf.comData.BaudRateChoose = baudRateCbx.SelectedIndex;
        }

        private void dataBitsCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mainform.comConfigureInf.comData.DataBitsChoose = dataBitsCbx.SelectedIndex;
        }

        private void stopBitsCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mainform.comConfigureInf.comData.StopBitsChoose = stopBitsCbx.SelectedIndex;
        }

        private void parityCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mainform.comConfigureInf.comData.ParityChoose = parityCbx.SelectedIndex;
        }

        private void handshakingcbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mainform.comConfigureInf.comData.HandshakingsChoose = handshakingcbx.SelectedIndex;
        }
    }

}
