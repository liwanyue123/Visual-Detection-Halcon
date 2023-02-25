using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ViewWindow.Config
{
    [Serializable]
    public class Point
    {
        private double _row;
        private double _column;
       
        [XmlElement(ElementName = "Row")]
        public double Row
        {
            get { return this._row; }
            set { this._row = value; }
        }

        [XmlElement(ElementName = "Column")]
        public double Column
        {
            get { return this._column; }
            set { this._column = value; }
        }

        private string color = "yellow";
        [XmlElement(ElementName = "Color")]
        public string Color
        {
            get { return this.color; }
            set { this.color = value; }
        }
      
        public Point()
        {

        }

        public Point(double row, double column)
        {
            this._row = row;
            this._column = column;
        }
        
    }
}
