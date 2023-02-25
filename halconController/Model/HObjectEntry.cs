using System;
using HalconDotNet;
using System.Collections;

namespace ViewWindow.Model
{

    /// <summary>
    /// 此类是辅助类，用于将图形上下文链接到 HALCON 对象上。
    /// 图形上下文由哈希表描述，其中包含图形模式列表（例如GC_COLOR、GC_LINEWIDTH和GC_PAINT）及其相应的值（例如"blue"，"4"，"3D-plot"）。
    /// 在显示对象之前，这些图形状态将应用于窗口。
    /// </summary>
    public class HObjectEntry
	{
		/// <summary>Hashlist defining the graphical context for HObj</summary>
		public Hashtable	gContext;

		/// <summary>HALCON object</summary>
		public HObject		HObj;



		/// <summary>Constructor</summary>
		/// <param name="obj">
		/// HALCON object that is linked to the graphical context gc. 
		/// </param>
		/// <param name="gc"> 
		/// Hashlist of graphical states that are applied before the object
		/// is displayed. 
		/// </param>
		public HObjectEntry(HObject obj, Hashtable gc)
		{
			gContext = gc;
			HObj = obj;
		}

		/// <summary>
		/// Clears the entries of the class members Hobj and gContext
		/// </summary>
		public void clear()
		{
			gContext.Clear();
			HObj.Dispose();
		}

	}//end of class
}//end of namespace
