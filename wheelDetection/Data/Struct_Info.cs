using System;
 
using HalconDotNet;
using System.Drawing;

namespace FormFinalDesign
{
    /// <summary>
    /// 直线信息
    /// </summary>
    public struct Line_INFO
    {
        public double start_Row;//起点行坐标
        public double start_Col;//起点列坐标
        public double end_Row; //终点行坐标
        public double end_Col;//终点列坐标
        public double Nr;//行向量
        public double Nc;//列向量
        public double Dist;//距离

        public Line_INFO(double m_start_Row, double m_start_Col, double m_end_Row, double m_end_Col)
        {
            //r*Nr+c*Nc-Dist=0
            ///AX+BY+C=0        
            //A = Y2 - Y1
            //B = X1 - X2
            //C = X2*Y1 - X1*Y2
            this.start_Row = m_start_Row;
            this.start_Col = m_start_Col;
            this.end_Row = m_end_Row;
            this.end_Col = m_end_Col;
            this.Nr = m_start_Col - m_end_Col;
            this.Nc = m_end_Row - m_start_Row;
            this.Dist = m_start_Col * m_end_Row - m_end_Col * m_start_Row;
        }
    };

    /// <summary>
    /// 测量信息
    /// </summary>
    public struct Metrology_INFO
    {
        public double Length1, Length2, Threshold, MeasureDis;//测量区域长宽，判断阈值，每个测量点距离
        public HTuple ParamName, ParamValue;
        public int PointsOrder;
        public Metrology_INFO(double _length1, double _length2, double _threshold, double _measureDis, HTuple _paraName, HTuple _paraValue, int _pointsOrder)
        {
            this.Length1 = _length1;                        // 长/2
            this.Length2 = _length2;                        // 宽/2
            this.Threshold = _threshold;                    // 阈值
            this.MeasureDis = _measureDis;                  //间隔
            this.ParamName = _paraName;                     //参数名
            this.ParamValue = _paraValue;                   //参数值
            this.PointsOrder = _pointsOrder;                //点顺序 0位默认，1 顺时针，2 逆时针
        }
    }

    /// <summary>
    /// 数据单元
    /// </summary>
    public struct F_DATA_CELL
    {
        private int _Data_CellID; ///所属单元ID，0 --表示全局变量
        public int m_Data_CellID
        {
            set { _Data_CellID = value; }
            get { return _Data_CellID; }
        }

        private DataGroup _Data_Group; //数据组合类型，0--单个变量，1--数组变量
        public DataGroup m_Data_Group
        {
            set { _Data_Group = value; }
            get { return _Data_Group; }
        }

        private DataType _Data_Type; //0-数值型,1--CString字符串型，2--2D点，3--3D点，4－直线，5－面
        public DataType m_Data_Type
        {
            set { _Data_Type = value; }
            get { return _Data_Type; }
        }

        private int _Data_Num; //变量组合数据的个数，单个变量为1，
        public int m_Data_Num
        {
            set { _Data_Num = value; }
            get { return _Data_Num; }
        }

        private DataAtrribution _Data_Atrr;//变量属性
        public DataAtrribution m_Data_Atrr
        {
            set { _Data_Atrr = value; }
            get { return _Data_Atrr; }
        }

        private String _Data_Name; //变量名称，不可重复，
        public String m_Data_Name
        {
            set { _Data_Name = value; }
            get { return _Data_Name; }
        }

        private String _DataTip; //注释
        public String m_DataTip
        {
            set { _DataTip = value; }
            get { return _DataTip; }
        }

        private String _Data_InitValue; //变量初值,为了兼容所有变量，将初值类型设为字符串型
        public String m_Data_InitValue
        {
            set { _Data_InitValue = value; }
            get { return _Data_InitValue; }
        }

        private Boolean _bUserDefineVariable;//是否用户自定义代码
        public Boolean m_bUserDefineVariable
        {
            set { _bUserDefineVariable = value; }
            get { return _bUserDefineVariable; }
        }


        //void* m_Data_Value; //变量的值,支持单量和数组形式
        private Object _Data_Value; //变量的值,支持单量和数组形式
        public Object m_Data_Value
        {
            set { _Data_Value = value; }
            get { return _Data_Value; }
        }

        public F_DATA_CELL(int _CellID, DataGroup _Group, DataType _type, String _Name, String _Tip,
                            String _InitValue, int _Num, Object _Value, DataAtrribution _Atrr)
        {
            _Data_CellID = _CellID;
            _Data_Group = _Group;
            _Data_Type = _type;
            _Data_Name = _Name;
            _DataTip = _Tip;
            _Data_InitValue = _InitValue;
            _Data_Num = _Num;
            _Data_Value = _Value;
            _bUserDefineVariable = false;
            _Data_Atrr = _Atrr;

        }
    }

    /// <summary>
    /// 注册图像信息
    /// </summary>
    public struct RegisterIMG_Info
    {
        public string m_ImageID;       ///注册图像ID，唯一,  -1--表示当前图像，其他表示注册图像ID
        public HImage m_Image;         ///注册图像

        public RegisterIMG_Info(string _ImageID, HImage _Image)
        {
            m_ImageID = _ImageID;
            m_Image = _Image;
        }
    }
    /// <summary>
    /// 系统状态
    /// </summary>
    public struct Sys_Status
    {
        private RunMode _RunMode;   
        private bool _bAutoRun;   ///是否自动运行

        public RunMode m_RunMode
        {
            set { _RunMode = value; }
            get { return _RunMode; }
        }


        public bool m_bAutoRun
        {
            set { _bAutoRun = value; }
            get { return _bAutoRun; }
        }
    }

    /// <summary>
    /// 结果显示信息
    /// </summary>
    public struct ResultView_Info
    {
        private string _dataType;
        private string _variableName;
        private string _conditionVarName;
        private string _disPosition;
        private Font _normalStyle;
        private string _normalColor;
        private string _ngColor;

        public string m_DataType
        {
            set { _dataType = value; }
            get { return _dataType; }
        }

        public string m_VariableName
        {
            set { _variableName = value; }
            get { return _variableName; }
        }

        public string m_ConditionVarName
        {
            set { _conditionVarName = value; }
            get { return _conditionVarName; }
        }

        public string m_DisPosition
        {
            set { _disPosition = value; }
            get { return _disPosition; }
        }

        public Font m_NormalStyle
        {
            set { _normalStyle = value; }
            get { return _normalStyle; }
        }

        public string m_NormalColor
        {
            set { _normalColor = value; }
            get { return _normalColor; }
        }

        public string m_NgColor
        {
            set { _ngColor = value; }
            get { return _ngColor; }
        }
    }

    /// <summary>
    /// 通讯信息
    /// </summary>
    public struct Communication_Info
    {
        private string _dataType;
        private string _unitID;
        private string _variableName;
        private string _spiltStr;
        private string _endStr;

        public string m_DataType
        {
            set { _dataType = value; }
            get { return _dataType; }
        }
        public string m_UnitID
        {
            set { _unitID = value; }
            get { return _unitID; }
        }
        public string m_VariableName
        {
            set { _variableName = value; }
            get { return _variableName; }
        }
        public string m_SpiltStr
        {
            set { _spiltStr = value; }
            get { return _spiltStr; }
        }
        public string m_EndStr
        {
            set { _endStr = value; }
            get { return _endStr; }
        }
    }
}
