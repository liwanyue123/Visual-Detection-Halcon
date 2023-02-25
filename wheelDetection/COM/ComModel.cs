using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.ComponentModel;
using System.Threading;

namespace wheelDetection
{
    public delegate void SerialPortEventHandler(Object sender, SerialPortEventArgs e);//声明事件句柄

    public class SerialPortEventArgs : EventArgs//继承事件基类
    {
        public bool isOpend = false;
        public Byte[] receivedBytes = null;
    }

    public delegate void DetectEventHandler(bool detectionRoller);
    public class ComModel
    {
        /// <summary>
        /// ///////////////////////
        /// </summary>
        int delayRubbishDetectionTime = 1000;
        int delayRollerDetectionTime = 300;
        private SerialPort sp = new SerialPort();

        public event DetectEventHandler StartDetectEvent = null;
        public event SerialPortEventHandler comReceiveDataEvent = null;
        public event SerialPortEventHandler comOpenEvent = null;
        public event SerialPortEventHandler comCloseEvent = null;

        //modbus发送的数据提前写好
        static Byte[] OpenQualifiedRelay = new byte[] { 0x01, 0x05, 0x00, 0x00, 0xFF, 0x00, 0x8C, 0x3A };//01 05 00 00 FF 00 8C 3A 打开继电器1
        static Byte[] CloseQualifiedRelay = new byte[] { 0x01, 0x05, 0x00, 0x00, 0x00, 0x00, 0xCD, 0xCA };// 01 05 00 00 00 00 CD CA 关闭继电器1
        static Byte[] OpenUnqualifiedRelay = new byte[] { 0x01, 0x05, 0x00, 0x01, 0xFF, 0x00, 0xDD, 0xFA };// 01 05 00 01 FF 00 DD FA  打开继电器2
        static Byte[] CloseUnqualifiedRelay = new byte[] { 0x01, 0x05, 0x00, 0x01, 0x00, 0x00, 0x9C, 0x0A };//01 05 00 01 00 00 9C 0A 关闭继电器2

        public void GiveComDataEvent(int delayRubbishDetectionTime, int delayRollerDetectionTime)
        {
            this.delayRubbishDetectionTime = delayRubbishDetectionTime;
            this.delayRollerDetectionTime = delayRollerDetectionTime;
        }

        Thread delayCloseRelay;//延迟关闭继电器
        public void UnqualifiedNotice()
        {

            delayCloseRelay = new Thread(delayCloseRelayThread);
            delayCloseRelay.Start();
        }
        public void delayCloseRelayThread()
        {
            Send(OpenUnqualifiedRelay);
            Thread.Sleep(500);
            Send(CloseUnqualifiedRelay);
            //Thread.
            return;
        }

        public void starRubbishDetection()
        {
            delayRubbishDetetion = new Thread(delayRubbishDetetionThread);
            delayRubbishDetetion.Start();
        }

        public void starRollerDetection()
        {
            delayRollerDetetion = new Thread(delayRollerDetetionThread);
            delayRollerDetetion.Start();
        }

        Thread delayRollerDetetion;//延迟检测
        public void delayRollerDetetionThread()
        {

            Thread.Sleep(delayRollerDetectionTime);
             StartDetectEvent(true);
            //Thread.
            return;
        }

        Thread delayRubbishDetetion;//延迟检测
        public void delayRubbishDetetionThread()
        {

            Thread.Sleep(delayRubbishDetectionTime);
            StartDetectEvent(false);
            //Thread.
            return;
        }
        private Object thisLock = new Object();

        IDetect detect;
        public ComModel(IDetect detect)
        {
            this.detect = detect;
        }


        /// <summary>
        /// When serial received data, will call this method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (sp.BytesToRead <= 0)
            {
                return;
            }
            //线程安全在 MSDN 中解释：
            //此类型的任何公共静态（在视觉基础中共享）成员都是线程安全的。
            //任何实例成员不保证线程安全。
            //因此，我们需要同步 I/ O
            lock (thisLock)
            {
                int len = sp.BytesToRead;
                Byte[] data = new Byte[len];
                try
                {
                    sp.Read(data, 0, len);
                }
                catch (System.Exception)
                {
                    //catch read exception
                }
                SerialPortEventArgs args = new SerialPortEventArgs();
                args.receivedBytes = data;
                if (comReceiveDataEvent != null)
                {


                    //////BUG
                    comReceiveDataEvent.Invoke(this, args);
                }
                //当 X1 有效时，将会向串口发送十六进制数据 01 02 01 01 60 48 
                //当 X2 有效时，将会向串口发送十六进制数据 01 02 01 02 20 49
                if (data[0] == 0x1 && data[1] == 0x2 && data[2] == 0x1)//是检测信号
                {
                    if (data[3] == 0x1)
                    {
                        starRubbishDetection();
                    }
                    else if (data[3] == 0x2)
                    {
                        starRollerDetection();
                    }



                }

            }
        }

        /// <summary>
        /// Send bytes to device
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public bool Send(Byte[] bytes)
        {
            if (!sp.IsOpen)
            {
                return false;
            }

            try
            {
                sp.Write(bytes, 0, bytes.Length);
            }
            catch (System.Exception)
            {
                return false;   //write failed
            }
            return true;        //write successfully
        }

        /// <summary>
        /// Open Serial port
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baudRate"></param>
        /// <param name="dataBits"></param>
        /// <param name="stopBits"></param>
        /// <param name="parity"></param>
        /// <param name="handshake"></param>
        public void Open(string portName, String baudRate,
            string dataBits, string stopBits, string parity,
            string handshake)
        {
            if (sp.IsOpen)
            {
                Close();
            }
            sp.PortName = portName;
            sp.BaudRate = Convert.ToInt32(baudRate);
            sp.DataBits = Convert.ToInt16(dataBits);

            /**
             *  If the Handshake property is set to None the DTR and RTS pins 
             *  are then freed up for the common use of Power, the PC on which
             *  this is being typed gives +10.99 volts on the DTR pin & +10.99
             *  volts again on the RTS pin if set to true. If set to false 
             *  it gives -9.95 volts on the DTR, -9.94 volts on the RTS. 
             *  These values are between +3 to +25 and -3 to -25 volts this 
             *  give a dead zone to allow for noise immunity.
             *  http://www.codeproject.com/Articles/678025/Serial-Comms-in-Csharp-for-Beginners
             */
            if (handshake == "None")
            {
                //Never delete this property
                sp.RtsEnable = true;
                sp.DtrEnable = true;
            }

            SerialPortEventArgs args = new SerialPortEventArgs();
            try
            {
                sp.StopBits = (StopBits)Enum.Parse(typeof(StopBits), stopBits);
                sp.Parity = (Parity)Enum.Parse(typeof(Parity), parity);
                sp.Handshake = (Handshake)Enum.Parse(typeof(Handshake), handshake);
                sp.WriteTimeout = 1000; /*Write time out*/
                sp.Open();
                sp.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
                args.isOpend = true;
            }
            catch (System.Exception)
            {
                args.isOpend = false;
            }
            if (comOpenEvent != null)
            {
                comOpenEvent.Invoke(this, args);
            }

        }


        /**         

         在响应 GUI 事件时调用串行端口上的关闭时，注意避免死锁。
         关闭串行端口时，涉及 UI 和串行端口的应用将冻结
         如果在串行端口事件处理程序中使用 Control.Invoke（），则可能发生死锁
         我们遇到的典型方案是在应用中偶尔死锁，该应用中收到的数据处理程序尝试在 GUI 线程尝试关闭串行端口的同时尝试更新 GUI（例如，为了响应用户单击"关闭"按钮）。

         发生死锁的原因是 Close（） 等待事件完成执行，然后再关闭端口。您可以通过两种方式在应用中解决此问题：
         (1)在事件处理程序中，将每个控件.Invoke 调用替换为控件.BeginInvoke，它异步执行并避免死锁条件。这通常用于使用 GUI 时避免死锁。
         (2)在单独的线程上调用串行端口.Close（）。您可能更喜欢这样，因为这比更新 Invoke 调用的侵入性要小。
    */

        /// <summary>
        /// Close serial port
        /// </summary>
        public void Close()
        {
            Thread closeThread = new Thread(new ThreadStart(CloseSpThread));
            closeThread.Start();
        }

        /// <summary>
        /// Close serial port thread
        /// </summary>
        private void CloseSpThread()
        {
            SerialPortEventArgs args = new SerialPortEventArgs();
            args.isOpend = false;
            try
            {
                sp.Close(); //close the serial port
                sp.DataReceived -= new SerialDataReceivedEventHandler(DataReceived);
            }
            catch (Exception)
            {
                args.isOpend = true;
            }
            if (comCloseEvent != null)
            {
                comCloseEvent.Invoke(this, args);
            }

        }

    }
}
