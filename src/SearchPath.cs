using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FzBusSystem.Base;

namespace FzBusSystem
{
    public class SearchPath
    {
        private int startid;
        private int endid;
        private DataRepository db;
        const int NUM = 1;
        private IEnumerable<stopline> stoplines;

        public SearchPath(DataRepository db, stop start, stop end)
        {
            this.db = db;
            this.startid = start.stopid;
            this.endid = end.stopid;
            stoplines = db.GetAllStopline();
        }

        /// <summary>
        /// 查找直达路线
        /// </summary>
        /// <param name="startid">起点站id</param>
        /// <param name="endid">终点站id</param>
        /// <returns>找到则返回所有路线的id，否则返回null</returns>
        private int[] FindDirect(int startid, int endid)
        {
            // 求经过起点站的所有线路
            var startset = from slstart in stoplines
                           where slstart.stopid == startid
                           select slstart.lineid;
            // 求经过终点站的所有线路
            var endset = from slend in stoplines
                         where slend.stopid == endid
                         select slend.lineid;

            var starts = startset.ToList();
            var ends = endset.ToList();
            // 求交集
            var compareresult = starts.Intersect(ends);
            // 若未找到直达线路，返回null
            if (compareresult.Count() == 0) return null;

            return compareresult.ToArray<int>();
        }

        /// <summary>
        /// 查找一次换乘路线
        /// </summary>
        /// <param name="startid">起点站id</param>
        /// <param name="endid">终点站id</param>
        /// <returns>找到则返回所有路线的信息，否则返回null</returns>
        private List<string> FindChangeOnce(int startid, int endid)
        {
            var stoplines = db.GetAllStopline();

            List<string> changeonces = new List<string>();

            // 求经过起点站的所有路线
            var startlineidset = from slstart in stoplines
                               where slstart.stopid == startid
                               select slstart.lineid;
            // 遍历每条选中的路线
            foreach (var startlineid in startlineidset)
            {
                // 每条路线上的除了startid的站点
                var stoppastids = from sl in stoplines
                                  where sl.lineid == startlineid
                                        && sl.stopid != startid
                                  select sl.stopid;
                // 遍历每个站点，即为中间站点
                foreach (var stoppastid in stoppastids)
                {
                    var pathbuff = new List<string>();

                    // 每个中间站点的路线，即为终点路线
                    var midlineidset = from sl in stoplines
                                       where sl.stopid == stoppastid
                                       select sl.lineid;
                    // 遍历每条终点路线
                    foreach (var ml in midlineidset)
                    {
                        var midlines = from sl in stoplines
                                       where sl.stopid == endid
                                             && sl.lineid == ml
                                       select sl.lineid;
                        // 终点路线不为0条时
                        if (midlines.Count() != 0)
                        {
                            string path = "搭< " + db.GetLineByLineid(startlineid).linename + " >路车至< " + db.GetStopByStopid(stoppastid).stopname + " >转< " + db.GetLineByLineid(midlines.First()).linename + " >路车";
                            pathbuff.Add(path);
                            break;
                        }
                    }
                    if (pathbuff.Count() != 0)
                    {
                        changeonces.AddRange(pathbuff);
                        break;
                        // 跳到遍历起始路线
                    }
                }
                if (changeonces.Count == NUM)
                    break;
            }

            if (changeonces.Count == 0) return null;

            return changeonces;
        }

        /// <summary>
        /// 查找换乘线路
        /// </summary>
        /// <returns>查找结果</returns>
        public SearchResult FindPath()
        {
            var direct = FindDirect(startid, endid);

            if (direct != null)
            {
                // 查找到直达线路
                StringBuilder sb = new StringBuilder();
                sb.Append("直达线路: ");
                foreach (var dir in direct)
                {
                    sb.Append(db.GetLineByLineid(dir).linename).Append(" 、");
                }
                sb.Remove(sb.Length - 2, 2);
                sb.Append("\r\n");

                return new SearchResult { Result = sb.ToString() };
            }

            var changeonce = FindChangeOnce(startid, endid);
            if (changeonce != null)
            {
                // 查找到一次换乘线路
                StringBuilder sb = new StringBuilder();
                sb.Append("一次换乘: \r\n");
                foreach (var co in changeonce)
                {
                    sb.Append(co).Append("\r\n或\r\n");
                }
                sb.Remove(sb.Length - 6, 6);
                sb.Append("\r\n");
                return new SearchResult { Result = sb.ToString() };
            }

            return null;
        }

    }
}
