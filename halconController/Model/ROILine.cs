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

		private HObject arrowHandleXLD;//��ͷ����

		public ROILine()
		{
			NumHandles = 3;        // two end points of line
			activeHandleIdx = 1  ;//��ʼʱ��ѡ�е����м�ĵ㣬�Ҹĳ������һ��
			arrowHandleXLD = new HXLDCont();
			arrowHandleXLD.GenEmptyObj();
		}

        public ROILine(double beginRow, double beginCol, double endRow, double endCol)
        {
            createLine(beginRow, beginCol, endRow, endCol);
        }

        /// <summary>���ߣ�����ֻ��ָ��������</summary>
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

		/// <summary>�����λ���ϴ���roiʵ�壬����ֻ��ָ��������.</summary>
		public override void createROI(double midX, double midY)
		{
			midR = midY;
			midC = midX;

			row1 = midR;
			col1 = midC - 50;
			row2 = midR;
			col2 = midC + 50;

            //��Ϊ�����õ����궼��ȫ�ֵģ������ȸı�˵����꣬�ٸ��¶˵��ͷ��״��֮��drawһ�¾Ϳ���ʾ��
			updateArrowHandle();
		}
		/// <summary>��roi������Ҫ�Ĵ�����</summary>
		public override void draw(HalconDotNet.HWindow window)
		{

			window.DispLine(row1, col1, row2, col2);//��������ֱ��

			window.DispRectangle2(row1, col1, 0, Msize.Width, Msize.Height);//��β������
            window.DispObj(arrowHandleXLD); //��ͷ����ͷ //window.DispRectangle2( row2, col2, 0, 25, 25);
            window.DispRectangle2(midR, midC, 0, Msize.Width, Msize.Height);//���в�����
        }

        /// <summary> 
        /// �������poi������Ҫ�Ĵ����ϣ�������draw���ĵ����棬
        /// �㵽�ĸ���λ��ˢ���ĸ�
        /// </summary>
        public override void displayActive(HalconDotNet.HWindow window)
        {

            switch (activeHandleIdx)//��꼤�����ĸ���λ���ͽ���λ��ˢ�ɺ�ɫ
            {
                case 0:
                    window.DispRectangle2(row1, col1, 0, Msize.Width, Msize.Height);//��β������
                    break;
                case 1:
                    window.DispObj(arrowHandleXLD); //��ͷ����ͷ
                                                    //window.DispRectangle2(row2, col2, 0, 25, 25);
                    break;
                case 2:
                    window.DispRectangle2(midR, midC, 0, Msize.Width, Msize.Height);//���в�����
                    break;
            }
        }

        /// <summary> 
        /// ����ͼ���Ͼ�����һ�㣬�����roi�����ͬʱ��������ֱ��activeHandleId������ѡ�������һ���㣨ͷ��β�����֣�
        /// </summary>
        public override double distToClosestHandle(double x, double y)
            //����ֱ����˵����굽���ֱ��roi�ľ���Ҫ�֣���ͷ��β�����ֵľ���
		{

			double max = 10000;
			double [] val = new double[NumHandles];

			val[0] = HMisc.DistancePp(y, x, row1, col1); // upper left 
			val[1] = HMisc.DistancePp(y, x, row2, col2); // upper right 
			val[2] = HMisc.DistancePp(y, x, midR, midC); // midpoint 

			for (int i=0; i < NumHandles; i++)//�ҳ�����ͷ��β�����֣��ĸ����
            {
				if (val[i] < max)
				{
					max = val[i];
					activeHandleIdx = i;
				}
			}// end of for 

			return val[activeHandleIdx];//������С�ľ��롣
		}

       

		/// <summary>Gets the HALCON region described by the ROI.</summary>
		public override HRegion getRegion()
		{
			HRegion region = new HRegion();
			region.GenRegionLine(row1, col1, row2, col2);
			return region;
		}
        /// <summary>
        /// ���ظ����㵽ֱ�ߵ�һ��ľ���
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
		public override double getDistanceFromStartPoint(double row, double col)
		{
			double distance = HMisc.DistancePp(row, col, row1, col1);//row1, col1�ǵ�һ������
            return distance;
		}
		/// <summary>
		/// ���ֱ����β2������
		/// </summary> 
		public override HTuple getModelData()
		{
			return new HTuple(new double[] { row1, col1, row2, col2 });
		}

        /// <summary> 
        /// ��roi�е�Ԫ�أ��㣩,�ƶ������꣨x��y����.,���������ƶ���ֻ�ı䲿��
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
            
            notifyCurrentRoiLine(this, row1,  col1, row2, col2);//���һ���͸��ߴ���

             updateArrowHandle();
		}

		/// <summary> Auxiliary method </summary>
		private void updateArrowHandle()//ֻ�Ǹ��ݴ������������һ����ͷ������������arrowHandleXLD����û����ʾ
        {
			double length,dr,dc, halfHW;
			double rrow1, ccol1,rowP1, colP1, rowP2, colP2;

			double headLength = 25;
			double headWidth  = 25;


			arrowHandleXLD.Dispose();
			arrowHandleXLD.GenEmptyObj();

            //��������һ����ͷ������
			rrow1 = row1 + (row2 - row1) * 0.9;//2��֮���һ����
			ccol1 = col1 + (col2 - col1) * 0.9;

			length = HMisc.DistancePp(rrow1, ccol1, row2, col2);//����2�����
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
            //GenContourPolygonXld(��һ��row�� col�и����Ķ��������XLD����  ����������һ����ͷ��״


        }

    }//end of class
}//end of namespace
