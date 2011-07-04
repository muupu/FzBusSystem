using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;
using FzBusSystem.Base;

namespace FzBusSystem
{
    public class DataRepository
    {
        private string xmlpath = System.Environment.CurrentDirectory + @"/BusInfo.xml";
        private XElement db ;

        public DataRepository()
        {
            try
            {
                db = XElement.Load(this.xmlpath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取公交数据失败！请确定BusInfo.xml文件是否存在。" + Environment.NewLine + Environment.NewLine + ex.Message);
            }
        }

        public IEnumerable<stop> GetAllStop()
        {
            var stops = from stopxe in db.Element("Stops").Elements("stop")
                        select new stop
                        {
                            stopid = (int)stopxe.Element("stopid"),
                            stopname = (string)stopxe.Element("stopname")
                        };
            return stops;
        }

        public IEnumerable<stopline> GetAllStopline()
        {
            var stoplines = from slxe in db.Element("StopLines").Elements("stopline")
                            select new stopline
                            {
                                stoplineid = (int)slxe.Element("stoplineid"),
                                lineid = (int)slxe.Element("lineid"),
                                stopid = (int)slxe.Element("stopid")
                            };
            return stoplines;
        }

        public IEnumerable<line> GetAllLine()
        {
            var lines = from linexe in db.Element("Lines").Elements("line")
                        select new line
                        {
                            lineid = (int)linexe.Element("lineid"),
                            linename = (string)linexe.Element("linename")
                        };
            return lines;
        }

        public IEnumerable<lineinfo> GetAllLineinfo()
        {
            var lineinfos = from lixe in db.Element("Lineinfos").Elements("lineinfo")
                            select new lineinfo
                            {
                                lineid = (int)lixe.Element("lineid"),
                                startstop = (string)lixe.Element("startstop"),
                                endstop = (string)lixe.Element("endstop"),
                                starttime = (string)lixe.Element("starttime"),
                                endtime = (string)lixe.Element("endtime"),
                                airconditioner = (string)lixe.Element("airconditioner"),
                                nonairconditioner = (string)lixe.Element("nonairconditioner")
                            };
            return lineinfos;
        }

        public lineinfo GetLineinfoByLineName(string linename)
        {
            var lis = from lf in GetAllLineinfo()
                      join li in GetAllLine()
                      on lf.lineid equals li.lineid
                      where li.linename == linename
                      select lf;
            // 查找失败返回null
            if (lis.ToList().Count == 0) return null;

            return lis.First();
        }

        public stop GetStopByStopname(string stopname)
        {
            var sts = from st in GetAllStop()
                      where st.stopname == stopname
                      select st;
            // 查找失败返回null
            if (sts.Count() == 0) return null;

            return sts.First();
        }

        public line GetLineByLineid(int lineid)
        {
            var lis = from li in GetAllLine()
                      where li.lineid == lineid
                      select li;

            if (lis.Count() == 0) return null;

            return lis.First();
        }

        public stop GetStopByStopid(int stopid)
        {
            var sis = from s in GetAllStop()
                      where s.stopid == stopid
                      select s;
            if (sis.Count() == 0) return null;

            return sis.First();
        }

        public List<string> Getlinespast(string stopname)
        {
            var linespast = from st in GetAllStop()
                            join sl in GetAllStopline()
                            on st.stopid equals sl.stopid
                            join li in GetAllLine()
                            on sl.lineid equals li.lineid
                            where st.stopname == stopname
                            select li.linename;
            return linespast.ToList<string>();
        }

        public List<string> GetStopsOfLine(string linename)
        {
            var sol = from sl in GetAllStopline()
                      join li in GetAllLine()
                      on sl.lineid equals li.lineid
                      join st in GetAllStop()
                      on sl.stopid equals st.stopid
                      where li.linename == linename
                      select st.stopname;

            return sol.ToList<string>();
        }
    }
}
