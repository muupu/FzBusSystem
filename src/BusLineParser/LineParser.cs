using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FzBusSystem.BusLineParser
{
    public class LineParser
    {
        /// <summary>
        /// 辅助查找
        /// </summary>
        private static Dictionary<string, int> lineDics = new Dictionary<string, int>();
        /// <summary>
        /// 设置初始查找的第一条线路
        /// </summary>
        /// <param name="lineNo"></param>
        public static void SetFirstLine(string lineNo)
        {
            if (!LineParser.lineDics.ContainsKey(lineNo))
                LineParser.lineDics.Add(lineNo, 0);
        }
        /// <summary>
        /// 清理
        /// </summary>
        public static void ClearLineDics()
        {
            LineParser.lineDics.Clear();
        }
        /// <summary>
        /// 获取一条线路信息
        /// </summary>
        /// <param name="html">页面源内容</param>
        /// <param name="currentLine">当前解析的线路</param>
        /// <param name="candidateList">候选要解析的线路</param>
        /// <returns></returns>
        public static LineInfo GetLineInfo(string html, string currentLine, List<string> candidateList)
        {
            if (string.IsNullOrEmpty(html))
                throw new ArgumentNullException(html, "页面源内容不得为空");

            if (string.IsNullOrEmpty(currentLine))
                throw new ArgumentNullException(currentLine, "当前线路不能为空");
            //查询不到站点
            Regex notFound = new Regex(ExpressionManager.NotFound, RegexOptions.Compiled);
            Match errorLine = notFound.Match(html);
            if (errorLine.Success)
            {
                if (candidateList.Contains(currentLine))
                    candidateList.Remove(currentLine);
                throw new Exception("查询不到该线路信息: " + currentLine);
            }

            LineInfo lineInfo = new LineInfo();
            Regex fragmentation = new Regex(ExpressionManager.Fragmentation, RegexOptions.Compiled);

            string[] fragmentString = fragmentation.Split(html);
            if (fragmentString.Length != 4)
                throw new Exception("页面解析出错,请重新分析");
            //站名
            Regex lineNo = new Regex(ExpressionManager.LineNo, RegexOptions.Compiled);
            Match lineNoMatch = lineNo.Match(fragmentString[1]);
            if (lineNoMatch.Success)
            {
                lineInfo.LineNo = lineNoMatch.Value;
            }
            else
                throw new Exception(currentLine + "解析出错");
            //起点站-->终点站
            Regex beginToEnd = new Regex(ExpressionManager.BeginToEnd, RegexOptions.Compiled);
            Match beginToEndMatch = beginToEnd.Match(fragmentString[1]);
            if (beginToEndMatch.Success)
            {
                lineInfo.Start = beginToEndMatch.Groups["begin"].Value.Replace('（', '(').Replace('）', ')'); ;
                lineInfo.End = beginToEndMatch.Groups["end"].Value.Replace('（', '(').Replace('）', ')'); ;
            }
            else
                throw new Exception(currentLine + "解析出错");
            // 夏季时刻表
            Regex summerTime = new Regex(ExpressionManager.Schedule, RegexOptions.Compiled);
            Match schedule = summerTime.Match(fragmentString[1]);
            if (schedule.Success)
            {
                //起点站
                lineInfo.StartBeginTime = schedule.Groups["start"].Value.Replace('：', ':');
                lineInfo.StartEndTime = schedule.Groups["end"].Value.Replace('：', ':');
                schedule = schedule.NextMatch();
                //终点站
                lineInfo.EndBeginTime = schedule.Groups["start"].Value.Replace('：', ':');
                lineInfo.EndTime = schedule.Groups["end"].Value.Replace('：', ':');
            }
            else
            {
                Regex exSummerTime = new Regex(ExpressionManager.ExSchedule, RegexOptions.Compiled);
                Match exschedule = exSummerTime.Match(fragmentString[1]);
                if (exschedule.Success)
                {
                    //起点站
                    lineInfo.StartBeginTime = exschedule.Groups["start"].Value.Replace('：', ':');
                    lineInfo.StartEndTime = exschedule.Groups["end"].Value.Replace('：', ':');
                    schedule = schedule.NextMatch();
                    //终点站
                    lineInfo.EndBeginTime = exschedule.Groups["start"].Value.Replace('：', ':');
                    lineInfo.EndTime = exschedule.Groups["end"].Value.Replace('：', ':');
                }
                else
                    throw new Exception(currentLine + "解析出错");
            }
            // 票价
            Regex price = new Regex(ExpressionManager.Price, RegexOptions.Compiled);
            Match matchPrice = price.Match(fragmentString[1]);
            if (!matchPrice.Success)
            {
                //throw new Exception(currentLine + "解析出错");
                lineInfo.NonAirconditionerPrice = lineInfo.AirconditionerPrice = "分段计价";
            }
            else
            {
                lineInfo.NonAirconditionerPrice = matchPrice.Value;
                matchPrice = matchPrice.NextMatch();
                lineInfo.AirconditionerPrice = matchPrice.Value;
            }

            //上行线路Table2 && 下行线路Table3
            Regex BusStation = new Regex(ExpressionManager.BusStation, RegexOptions.Compiled);
            for (int i = 2; i <= 3; i++)
            {
                Match station = BusStation.Match(fragmentString[i]);
                while (station.Success)
                {
                    if (i % 2 == 0)
                        lineInfo.AddUpLine(station.Value.Trim().Replace('（', '(').Replace('）', ')'));
                    else
                        lineInfo.AddDownLine(station.Value.Trim().Replace('（', '(').Replace('）', ')'));

                    station = station.NextMatch();
                }
            }

            //System.Diagnostics.Debug.WriteLine(lineInfo.ToString());
            //抓取候选线路
            Regex lines = new Regex(ExpressionManager.CandidateLine, RegexOptions.Compiled);
            for (int i = 2; i <= 3; i++)
            {
                Match line = lines.Match(fragmentString[i]);
                while (line.Success)
                {
                    if (!lineDics.ContainsKey(line.Value))
                    {
                        lineDics.Add(line.Value, 0);
                        candidateList.Add(line.Value);
                    }
                    line = line.NextMatch();
                }
            }
            System.Threading.Thread.Sleep(50);
            return lineInfo;
        }

        private void DebugOutput(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }
    }
}
