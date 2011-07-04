using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FzBusSystem.Base;

namespace FzBusSystem
{
    /// <summary>
    /// 将类对象转化成字符串形式的扩展方法
    /// </summary>
    public static class HelperExtentions
    {
        /// <summary>
        /// 将lineinfo转化成字符串形式
        /// </summary>
        public static string ToLineString(this lineinfo li, DataRepository db)
        {
            var lineselect = from lin in db.GetAllLine()
                             where lin.lineid == li.lineid
                             select lin.linename;

            string linename = lineselect.First<string>();

            List<string> stopsofline = db.GetStopsOfLine(linename);

            StringBuilder sb = new StringBuilder();
            sb.Append("线路名:  ").Append(linename).Append("\r\n");
            sb.Append("运营路线: ").Append(li.startstop).Append("<---->").Append(li.endstop).Append("\r\n");
            sb.Append("始发站时刻: ").Append(li.starttime).Append("\r\n");
            sb.Append("终点站时刻: ").Append(li.endtime).Append("\r\n");
            sb.Append("非空调车价格: ").Append(li.nonairconditioner).Append("\r\n");
            sb.Append("空调车价格: ").Append(li.airconditioner).Append("\r\n");
            sb.Append("站点数:").Append(stopsofline.Count).Append("个  ");
            foreach (string stopname in stopsofline)
            {
                sb.Append(stopname).Append("<-->");
            }
            sb.Remove(sb.Length - 4, 4);
            sb.Append("\r\n");

            return sb.ToString();
        }

        /// <summary>
        /// 将stop转化成字符串形式
        /// </summary>
        public static string ToStopString(this stop st, DataRepository db)
        {
            List<string> linespast = db.Getlinespast(st.stopname);

            StringBuilder sb = new StringBuilder();
            sb.Append("车站名：").Append(st.stopname).Append("\r\n");
            sb.Append("经过该站点的所有路线: ");
            foreach (var line in linespast)
            {
                sb.Append(line).Append(" 、");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append("\r\n");

            return sb.ToString();
        }
    }
}
