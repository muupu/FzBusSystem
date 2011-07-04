using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FzBusSystem.BusLineParser
{
    public class LineInfo:IComparable
    {
        #region 属性
              
        public string LineNo { get; set; }

        public string Start { get; set; }

        public string StartBeginTime { get; set; }

        public string StartEndTime { get; set; }

        public string End { get; set; }

        public string EndBeginTime { get; set; }

        public string EndTime { get; set; }

        public string LineType
        {
            get { return LineType; }
            set { LineType = value; }
        }

        public string NonAirconditionerPrice { get; set; }

        public string AirconditionerPrice { get; set; }

        private List<string> upLine = new List<string>();

        public List<string> UpLine
        {
            get { return upLine; }
        }

        private List<string> downLine = new List<string>();

        public List<string> DownLine
        {
            get { return downLine; }
        }

        public int DownLineCout
        {
            get { return this.downLine.Count; }
        }

        public string Remark { get; set; }

        #endregion

        #region 方法



        public void AddUpLine(string busStation)
        {
            if (string.IsNullOrEmpty(busStation))
                throw new ArgumentNullException(busStation, "公交站点不得为空");

            this.upLine.Add(busStation);
        }


        public void AddDownLine(string busStation)
        {
            if (string.IsNullOrEmpty(busStation))
                throw new ArgumentNullException(busStation, "公交站点不得为空");

            this.downLine.Add(busStation);
        }

        public int UpLineCount()
        {
            return this.upLine.Count;
        }

        public List<XElement> UpLineToXels()
        {
            List<XElement> xes = new List<XElement>() ;
            int seq = 1;
            foreach (var station in UpLine)
            {
                XElement xe = new XElement("station", new XAttribute("seq", (seq++).ToString()),station);
                xes.Add(xe);
               
            }
            return xes;
        }

        public List<XElement> DownLineToXels()
        {
            List<XElement> xes = new List<XElement>();
            int seq = 1;
            foreach (var station in DownLine)
            {
                XElement xe = new XElement("station", new XAttribute("seq", (seq++).ToString()), station);
                xes.Add(xe);

            }
            return xes;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("线路名称: ").Append(this.LineNo).Append("\r\n");
            sb.Append("运营路线: ").Append(this.Start).Append("<---->").Append(this.End).Append("\r\n");
            sb.Append("始发站时刻: ").Append(this.StartBeginTime).Append("--").Append(this.StartEndTime).Append("\r\n");
            sb.Append("终点站时刻: ").Append(this.EndBeginTime).Append("--").Append(this.EndTime).Append("\r\n");
            sb.Append("非空调车价格: ").Append(this.NonAirconditionerPrice).Append("\r\n");
            sb.Append("空调车价格: ").Append(this.AirconditionerPrice).Append("\r\n");

            sb.Append("上行线站点数:").Append(this.upLine.Count).Append("个  ");
            foreach (string station in this.upLine)
            {
                sb.Append(station).Append("-->");
            }
            sb.Remove(sb.Length - 3, 3);
            sb.Append("\r\n");
            sb.Append("\r\n下行线站点数:").Append(this.downLine.Count).Append("个  ");
            foreach (string station in this.downLine)
            {
                sb.Append(station).Append("-->");
            }
            sb.Remove(sb.Length - 3, 3);

            sb.Append("\r\n◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆\r\n\r\n");
            return sb.ToString();
        }


        public XElement ToXElement()
        {
            XElement lineInfo = new XElement("Line",
                                    new XElement("LineNo", LineNo),
                                    new XElement("Start", Start),
                                    new XElement("StartBeginTime", StartBeginTime),
                                    new XElement("StartEndTime", StartEndTime),
                                    new XElement("End", End),
                                    new XElement("EndBeginTime", EndBeginTime),
                                    new XElement("EndEndTime", EndTime),
                                    new XElement("NonAirconditionerPrice", NonAirconditionerPrice),
                                    new XElement("AirconditionerPrice", AirconditionerPrice),
                                    new XElement("UpLine", UpLineToXels()),
                                    new XElement("DownLine", DownLineToXels())
                                    );
            return lineInfo;
        }
        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is LineInfo)
            {
                LineInfo otherLineInfo = (LineInfo)obj;
                try
                {
                    int other = int.Parse(otherLineInfo.LineNo);
                    int thisLine = int.Parse(this.LineNo);
                    return thisLine.CompareTo(other);
                }
                catch
                {
                    return this.LineNo.CompareTo(otherLineInfo.LineNo);
                }
            }
            else
            {
                throw new ArgumentException("object is not a LineInfo");
            }
        }

        #endregion
    }
}
