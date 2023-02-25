using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Timers;

namespace wheelDetection
{

    public class IController
    {
        public Detection detection;
        //IDetect detection;
        public ComModel comModel ;//C#规定在类内部只能定义属性或者变量，并初始化，不能直接变量引用变量。
        //Detection detection = new Detection();
        IView view;
        //Form1 MainForm;//有什么方法实现双向调用？接口和委托一个出一个进，反向怎么做？

        public IController(IView view)
        {
            detection = new Detection();
            comModel = new ComModel(detection);
            
            
 
            this.view = view;//this表示当前类，所以这句话表示将形参赋值给类中的view
            view.SetController(this);

            //com->det
            comModel.StartDetectEvent += detection.StartDetect;
            //det->form
            detection.RequestShootPicEvent += view.RequestShootPicEvent;//委托绑定,detection中使用request图片，就会调用view中的函数
            detection.DetectQualifiedEvent += view.DetectQualifiedEvent;
            //detection.GiveFormDataEvent += view.GiveFormDataEvent;
            
 
            
            //detection.setInfToForm();//把一些读取的数据发给form
            //detection.RequestInitialModelEvent += view.RequestInitialModelEvent;
            //det->com
            detection.UnqualifiedNotice += comModel.UnqualifiedNotice;
            // detection.GiveComDataEvent += comModel.GiveComDataEvent;

            //detection.GiveFormData();//告诉主窗体所有的参数信息
            // detection.GiveComData();//告诉串口检测延时


        }

        /// <summary>
        /// Hex to byte
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        private static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                try
                {
                    raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
                }
                catch (System.Exception)
                {
                    //Do Nothing
                }

            }
            return raw;
        }

        /// <summary>
        /// Hex string to string
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static String Hex2String(String hex)
        {
            byte[] data = FromHex(hex);
            return Encoding.Default.GetString(data);
        }

        /// <summary>
        /// String to hex string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String String2Hex(String str)
        {
            Byte[] data = Encoding.Default.GetBytes(str);
            return BitConverter.ToString(data);
        }

        /// <summary>
        /// Hex string to bytes
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static Byte[] Hex2Bytes(String hex)
        {
            return FromHex(hex);
        }

        /// <summary>
        /// Bytes to Hex String
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static String Bytes2Hex(Byte[] bytes)
        {
            return BitConverter.ToString(bytes);
        }

        /// <summary>
        /// send bytes to serial port
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool SendDataToCom(Byte[] data)
        {
            return comModel.Send(data);
        }

        /// <summary>
        /// Send string to serial port
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool SendDataToCom(String str)
        {
            if (str != null && str != "")
            {
                return comModel.Send(Encoding.Default.GetBytes(str));
            }
            return true;
        }

        /// <summary>
        /// Open serial port in comModel
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baudRate"></param>
        /// <param name="dataBits"></param>
        /// <param name="stopBits"></param>
        /// <param name="parity"></param>
        /// <param name="handshake"></param>
        public void OpenSerialPort(string portName, String baudRate,
            string dataBits, string stopBits, string parity, string handshake)
        {
            if (portName != null && portName != "")
            {
                comModel.Open(portName, baudRate, dataBits, stopBits, parity, handshake);
            }
        }

        /// <summary>
        /// Close serial port in comModel
        /// </summary>
        public void CloseSerialPort()
        {
            comModel.Close();
        }

    }
}
