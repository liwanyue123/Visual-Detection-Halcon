
using System;
using HalconDotNet;

using System.Collections.Generic;
namespace SharedData
{


    [Serializable]
    public class MeasureRectangleData
    {
        //大框子
        public Rectangle1Inf rectangle1Inf;

        //小框子
        //public Rectangle2Inf rect2Inf;//轮子框的各种信息，因为无法直接从rectangle2中提取这些信息，所以我单独写了一个类,但是序列化不能类中类，必须要拆开。。。。
        public Rectangle2Inf rectangle2Inf;
        //线宽 ，阈值等。。。
        public OtherRectParam otherRectParam;

    }
    [Serializable]
    public class MeasureModelData
    {
        public HShapeModel ModelID;//模板匹配的模型
        public double RectbiasAngle;//轮子框角度偏移（轮子框角度-模型的角度，弧度制（0，2pi））
        public double biasAngleOfRectCenterPoint;//轮子框中心点角度偏移（轮子框中心到模型中心连线的角度-模型的角度）
        public double distanceMove;//轮子框位置偏移（轮子框中心到模型中心的距离）
       // public double RectTrueRow;//轮子框真正中心位置 ，为了第一次没有任何匹配时候可用
        //public double RectTrueCol;//轮子框真正中心位置 



    }
    [Serializable]
    public class ComData
    {
        public ComData(int ArrayComPortsNamesChoose, int BaudRateChoose,int DataBitsChoose,int StopBitsChoose,int ParityChoose,int HandshakingsChoose)
        {
            this.BaudRateChoose = BaudRateChoose;
            this.DataBitsChoose = DataBitsChoose;
            this.StopBitsChoose = StopBitsChoose;
            this.ParityChoose = ParityChoose;
            this.HandshakingsChoose = HandshakingsChoose;
            this.ArrayComPortsNamesChoose = ArrayComPortsNamesChoose;
        }
        public ComData()
        {
   
        }
        public int BaudRateChoose;
        public int DataBitsChoose;
        public int StopBitsChoose;
        public int ParityChoose;
        public int HandshakingsChoose;
        public int ArrayComPortsNamesChoose;//串口选的什么
                                           

    }
    public class ComConfigureInf//串口信息
    {
        public List<int> BaudRate;
        public List<int> DataBits;
        public List<string> StopBits;
        public List<string> Parity;
        public List<string> Handshakings;
        public string[] ArrayComPortsNames;
        public ComData comData;

    }



    [Serializable]
    public class Rectangle1Inf
    {
        //大框子
        public double mainRow1;//左上
        public double mainColumn1;
        public double mainRow2;//右下
        public double mainColumn2;
        public Rectangle1Inf(double row1, double col1, double row2, double col2)
        {
            mainRow1 = row1;
            mainRow2 = row2;
            mainColumn1 = col1;
            mainColumn2 = col2;
        }
        public Rectangle1Inf()
        {

        }
    }
    [Serializable]
    public class Rectangle2Inf
    {
        //小框子
        //public Rectangle2Inf rect2Inf;//轮子框的各种信息，因为无法直接从rectangle2中提取这些信息，所以我单独写了一个类,但是序列化不能类中类，必须要拆开。。。。
        public double DetRow;//小框子中心点坐标
        public double DetCol;
        public double DetAngle;
        public double Detlen1;
        public double Detlen2;
        public Rectangle2Inf(double DetRow, double DetCol, double DetAngle, double Detlen1, double Detlen2)
        {
            this.DetRow = DetRow;
            this.DetCol = DetCol;
            this.DetAngle = DetAngle;
            this.Detlen1 = Detlen1;
            this.Detlen2 = Detlen2;
        }
        public Rectangle2Inf()
        {
            
        }
    }
    [Serializable]
    public class OtherRectParam//其他框子需要的信息
    {
        public OtherRectParam(int pixPercentOfRubbish, int pixPercentOfRoller, int lineWidth, double threadold, int delayRubbishDetectionTime, int delayRollerDetectionTime,int numMiss, bool isMatch)
        {
            this.pixPercentOfRubbish = pixPercentOfRubbish;
            this.pixPercentOfRoller = pixPercentOfRoller;
            this.lineWidth = lineWidth;
            this.threadold = threadold;
            this.delayRubbishDetectionTime =delayRubbishDetectionTime;
            this.delayRollerDetectionTime = delayRollerDetectionTime;
            this.numMiss = numMiss;
            this.isMatch = isMatch;

        }
        public OtherRectParam()
        {
           
        }
        public int pixPercentOfRubbish;//高于这个就是卡了东西（检测杂物时）
        public int pixPercentOfRoller;//低于这个就是没料（检测物体时）
        public int lineWidth;
        public double threadold;
        public int delayRubbishDetectionTime;
        public int delayRollerDetectionTime ;
        public int numMiss;
        public bool isMatch;//是否重新匹配
        // public int com;
    }

}


