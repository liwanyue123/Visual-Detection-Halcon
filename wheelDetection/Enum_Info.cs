using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 

namespace FormFinalDesign
{
///////////////////////相机信息//////////////////////////
public enum CameraState// 相机状态
    {
        正常连接,
            暂停,
            关闭连接
    }
 
    /// <summary>
    /// 图像位深
    /// </summary>
    public enum PIXEL_DEPTH
    {
        PIXEL_DEPTH_8,          ///8位
        PIXEL_DEPTH_12,         ///12位
        PIXEL_DEPTH_16          ///16位
    }

    /// <summary>
    /// 图像彩色信息
    /// </summary>
    public enum PIXEL_TYPE
    {
        PIX_GRAY8,              ///灰度图8位，定义时可以不要位深信息
        PIX_RGB8                ///彩色图8位
    }


/// ////////////////////////////////////////////////////////////

    public enum CellCatagory
    {
        /// 前十为保留类型
        图像单元 = 0,
        直线单元,              ///直线检测单元
        
        建立坐标系,            ///建立直角坐标系
      
        MODBUS通讯,            ///TCP/IP通讯
       
        数据计算,               ///数据计算单元
        数据存储,               ///数据存储
       
        结果显示,               ///结果显示
    
    }

    /// <summary>
    /// 数据类型
    /// </summary>
    public enum DataType         ///复数全部用list来存储
    {
        数值型 = 0,                ///数值类型   float
        字符串,                  ///CString 字符串类型
        点2D,                    ///2D点
         
        直线,                    ///直线
       
        图像,                    ///图像
       
    }

    /// <summary>
    /// 自定义变量数据类型
    /// </summary>
    public enum DataGroup
    {
        单量 = 0,           ///单个变量
        数组,              ///数组类型
    }

    /// <summary>
    /// 变量归属
    /// </summary>
    public enum DataAtrribution
    {
        全局变量 = 0,              ///全局变量，但无需保存
        系统变量,                  ///系统变量，需要保存到本地
        常量,                      ///常量
    }

    /// <summary>
    /// 展示图像分类
    /// </summary>
    public enum ImageCatagory
    {
        当前图像,
        注册图像
    }

 

    /// <summary>
    /// 运行模式
    /// </summary>
    public enum RunMode
    {
        单步运行 = 0,
        执行一次,
        循环运行,
    }

 
 

    /// <summary>
    /// 内部常量定义
    /// </summary>
    public static class ConstVavriable
    {
        #region 单元输出变量 定义和赋值不允许重复
        public const string outLine = "outLine";
        public const string outCircle = "outCircle";
        public const string outEllipse = "outEllipse";
        public const string outCol = "outCol";
        public const string outRow = "outRow";
        public const string outPointF = "outPointF";
        public const string outCoord = "outCoord";
        public const string outRectInfo = "outRectInfo";
        public const string outHomMat2D = "outHomMat2D";
        public const string outCalib = "outCalib";
        #endregion
    }
}
