using System;
using HalconDotNet;
using System.Xml.Serialization;

namespace ViewWindow.Model
{
	/// <summary>
	/// This class demonstrates one of the possible implementations for a 
	/// linear ROI. ROILine inherits from the base class ROI and 
	/// implements (besides other auxiliary methods) all virtual methods 
	/// defined in ROI.cs.
	/// </summary>
    [Serializable]
    public class ROILine : ROI
    {
        //public event RoiLinePositionHandler notifyCurrentRoiLinePositionEvent = null;

        [XmlElement(ElementName = "RowBegin")]
        public double RowBegin
        {
            get { return this.row1; }
            set { this.row1 = value; }
        }

        [XmlElement(ElementName = "ColumnBegin")]
        public double ColumnBegin
        {
            get { return this.col1; }
            set { this.col1 = value; }
        }
        [XmlElement(ElementName = "RowEnd")]
        public double RowEnd
        {
            get { return this.row2; }
            set { this.row2 = value; }
        }

        [XmlElement(ElementName = "ColumnEnd")]
        public double ColumnEnd
        {
            get { return this.col2; }
            set { this.col2 = value; }
        }

		private double row1, col1;   // first end point of line
		private double row2, col2;   // second end point of line
		private double midR, midC;   // midPoint of line

		private HObject arrowHandleXLD;//箭头轮廓

		public ROILine()
		{
			NumHandles = 3;        // two end points of line
			activeHandleIdx = 1  ;//开始时，选中的是中间的点，我改成了最后一点
			arrowHandleXLD = new HXLDCont();
			arrowHandleXLD.GenEmptyObj();
		}

        public ROILine(double beginRow, double beginCol, double endRow, double endCol)
        {
            createLine(beginRow, beginCol, endRow, endCol);
        }

        /// <summary>画线，这里只是指定好坐标</summary>
        public override void createLine(double beginRow, double beginCol, double endRow, double endCol)
        {
            base.createLine(beginRow, beginCol, endRow, endCol);
            
            row1 = beginRow;
            col1 = beginCol;
            row2 = endRow;
            col2 = endCol;

            midR = (row1 + row2) / 2.0;
            midC = (col1 + col2) / 2.0;

            updateArrowHandle();
        }

		/// <summary>在鼠标位置上创建roi实体，这里只是指定好坐标.</summary>
		public override void createROI(double midX, double midY)
		{
			midR = midY;
			midC = midX;

			row1 = midR;
			col1 = midC - 50;
			row2 = midR;
			col2 = midC + 50;

            //因为画线用的坐标都是全局的，所以先改变端点坐标，再更新端点箭头形状，之后draw一下就可显示了
			updateArrowHandle();
		}
		/// <summary>将roi画在所要的窗体上</summary>
		public override void draw(HalconDotNet.HWindow window)
		{

			window.DispLine(row1, col1, row2, col2);//画出主干直线

			window.DispRectangle2(row1, col1, 0, Msize.Width, Msize.Height);//画尾部方块
            window.DispObj(arrowHandleXLD); //画头部箭头 //window.DispRectangle2( row2, col2, 0, 25, 25);
            window.DispRectangle2(midR, midC, 0, Msize.Width, Msize.Height);//画中部方块
        }

        /// <summary> 
        /// 将激活的poi画在所要的窗体上，叠加在draw画的的上面，
        /// 点到哪个部位，刷红哪个
        /// </summary>
        public override void displayActive(HalconDotNet.HWindow window)
        {

            switch (activeHandleIdx)//鼠标激活了哪个部位，就将该位置刷成红色
            {
                case 0:
                    window.DispRectangle2(row1, col1, 0, Msize.Width, Msize.Height);//画尾部方块
                    break;
                case 1:
                    window.DispObj(arrowHandleXLD); //画头部箭头
                                                    //window.DispRectangle2(row2, col2, 0, 25, 25);
                    break;
                case 2:
                    window.DispRectangle2(midR, midC, 0, Msize.Width, Msize.Height);//画中部方块
                    break;
            }
        }

        /// <summary> 
        /// 返回图像上距离这一点，最近的roi句柄，同时设置这条直线activeHandleId，告诉选择的是哪一个点（头中尾三部分）
        /// </summary>
        public override double distToClosestHandle(double x, double y)
            //对于直线来说，鼠标到这个直线roi的距离要分，到头中尾三部分的距离
		{

			double max = 10000;
			double [] val = new double[NumHandles];

			val[0] = HMisc.DistancePp(y, x, row1, col1); // upper left 
			val[1] = HMisc.DistancePp(y, x, row2, col2); // upper right 
			val[2] = HMisc.DistancePp(y, x, midR, midC); // midpoint 

			for (int i=0; i < NumHandles; i++)//找出，到头中尾三部分，哪个最近
            {
				if (val[i] < max)
				{
					max = val[i];
					activeHandleIdx = i;
				}
			}// end of for 

			return val[activeHandleIdx];//返回最小的距离。
		}

       

		/// <summary>Gets the HALCON region described by the ROI.</summary>
		public override HRegion getRegion()
		{
			HRegion region = new HRegion();
			region.GenRegionLine(row1, col1, row2, col2);
			return region;
		}
        /// <summary>
        /// 返回给定点到直线第一点的距离
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
		public override double getDistanceFromStartPoint(double row, double col)
		{
			double distance = HMisc.DistancePp(row, col, row1, col1);//row1, col1是第一点坐标
            return distance;
		}
		/// <summary>
		/// 获得直线首尾2点坐标
		/// </summary> 
		public override HTuple getModelData()
		{
			return new HTuple(new double[] { row1, col1, row2, col2 });
		}

        /// <summary> 
        /// 将roi中的元素（点）,移动到坐标（x，y）上.,不是整体移动，只改变部分
        /// </summary>
        public override void moveByHandle(double newX, double newY)
		{
			double lenR, lenC;

			switch (activeHandleIdx)
			{
				case 0: // first end point
					row1 = newY;
					col1 = newX;

					midR = (row1 + row2) / 2;
					midC = (col1 + col2) / 2;
					break;
				case 1: // last end point
					row2 = newY;
					col2 = newX;

					midR = (row1 + row2) / 2;
					midC = (col1 + col2) / 2;
					break;
				case 2: // midpoint 
					lenR = row1 - midR;
					lenC = col1 - midC;

					midR = newY;
					midC = newX;

					row1 = midR + lenR;
					col1 = midC + lenC;
					row2 = midR - lenR;
					col2 = midC - lenC;
					break;
			}
            
            notifyCurrentRoiLine(this, row1,  col1, row2, col2);//鼠标一动就告诉窗体

             updateArrowHandle();
		}

		/// <summary> Auxiliary method </summary>
		private void updateArrowHandle()//只是根据传入参数，画了一个箭头轮廓，更新了arrowHandleXLD，并没有显示
        {
			double length,dr,dc, halfHW;
			double rrow1, ccol1,rowP1, colP1, rowP2, colP2;

			double headLength = 25;
			double headWidth  = 25;


			arrowHandleXLD.Dispose();
			arrowHandleXLD.GenEmptyObj();

            //下面生成一个箭头的轮廓
			rrow1 = row1 + (row2 - row1) * 0.9;//2点之间的一个点
			ccol1 = col1 + (col2 - col1) * 0.9;

			length = HMisc.DistancePp(rrow1, ccol1, row2, col2);//计算2点距离
			if (length == 0)
				length = -1;

			dr = (row2 - rrow1) / length;
			dc = (col2 - ccol1) / length;

			halfHW = headWidth / 2.0;
			rowP1 = rrow1 + (length - headLength) * dr + halfHW * dc;
			rowP2 = rrow1 + (length - headLength) * dr - halfHW * dc;
			colP1 = ccol1 + (length - headLength) * dc - halfHW * dr;
			colP2 = ccol1 + (length - headLength) * dc + halfHW * dr;

			if (length == -1)
                HOperatorSet.GenContourPolygonXld(out arrowHandleXLD,rrow1, ccol1);
			else
                HOperatorSet.GenContourPolygonXld(out arrowHandleXLD, new HTuple(new double[] { rrow1, row2, rowP1, row2, rowP2, row2 }),
                                                    new HTuple(new double[] { ccol1, col2, colP1, col2, colP2, col2 }));
            //GenContourPolygonXld(从一组row和 col中给出的多边形生成XLD轮廓  ，这里生成一个箭头形状


        }

    }//end of class
}//end of namespace
