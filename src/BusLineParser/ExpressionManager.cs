using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FzBusSystem.BusLineParser
{
    internal class ExpressionManager
    {
        /// <summary>
        /// 查询不到的正则
        /// </summary>
        internal static string NotFound = @"<div\s*align=\s*'center'>\s*找不到你要查询的线路\s*</div>";
        /// <summary>
        /// 分割成3个table   <table width="520" .*>
        /// </summary>
        internal static string Fragmentation = "<table width=" + "\"" + "520" + "\"" + " .*>";
        #region 头部Table1

        /// <summary>
        /// 线路名称包含字母
        /// </summary>
        internal static string LineNo = @"(?<=>)[\w]+(?=</div)";
        /// <summary>
        /// 起点站-->终点站
        /// </summary>
        internal static string BeginToEnd = @"(?<begin>[\w（）(·)]+)--→\s*(?<end>[\w（）(·)]+)";
        /// <summary>
        /// 杂项Miscellaneous(废弃)
        /// </summary>
        internal static string Miscellaneous = @"(<div align=" + "\"" + "center" + "\"" + "(?s).*</div>)";
        /// <summary>
        /// 夏季时刻表,冬季还要吗?
        /// </summary>
        internal static string Schedule = @"(?<=夏季:)(?<start>\d{1,2}[:：]\s*\d{2})--\s*(?<end>\d{1,2}[:：]\s*\d{2})(?#TNND垃圾程序员)";
        /// <summary>
        /// 特殊的时刻表情况,比如时刻表为空
        /// </summary>
        internal static string ExSchedule = @"(?<=夏季:)(?<start>)--\s*(?<end>)(?#TNND垃圾程序员)";
        /// <summary>
        /// 票价
        /// </summary>
        internal static string Price = @"(?<=>)\d元(?=<)";

        #endregion

        #region 上行线路Table2 && 下行线路Table3

        /// <summary>
        /// 公交车站
        /// </summary>
        internal static string BusStation = @"(?<=>)\s*[\w（）(·)-[\d]]+(?=</div>)";
        /// <summary>
        /// 候选线路查询
        /// </summary>
        internal static string CandidateLine = @"(?<=xianl=)\w+(?='>)";

        #endregion

    }
}
