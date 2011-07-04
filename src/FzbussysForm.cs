using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FzBusSystem.BusLineParser;
using FzBusSystem.Base;
using System.Net;
using System.Xml.Linq;
using System.IO;
using System.Threading;

namespace FzBusSystem
{
    public partial class FzbussysForm : Form
    {
        private static readonly Uri crackUri = new Uri("http://www.fz-bus.cn/Default.asp");
        //private static readonly Uri indexUri = new Uri("http://www.fz-bus.cn/Index.asp");
        private static readonly int TIMEOUT = 1000 * 100;
        private static readonly string initUri = @"http://www.fz-bus.cn/line_Search.asp?xianl=";
        private string sawContent = null;
        private CookieCollection sessionCookie = null;


        private string[] stopnames = null;
        private string[] linenames = null;
        private string htmlpath = System.Environment.CurrentDirectory + @"/map.html";
        private DataRepository db = null;

        public FzbussysForm()
        {
            InitializeComponent();
            this.db = new DataRepository();
            var stname = from st in db.GetAllStop()
                         select st.stopname;
            var liname = from li in db.GetAllLine()
                         select li.linename;
            stopnames = stname.ToArray<string>();
            linenames = liname.ToArray<string>();
        }

        /// <summary>
        /// 初始化各控件
        /// </summary>
        private void FzbussysForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.comboBox_stop.Items.AddRange(stopnames);
                this.comboBox_line.Items.AddRange(linenames);
                this.comboBox_start.Items.AddRange(stopnames);
                this.comboBox_end.Items.AddRange(stopnames);

                this.InitMap();
            }
            catch (Exception ex)
            {
                MessageBox.Show("载入数据失败!\n" + ex.Message, "error", MessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// 初始化WebBrowser控件
        /// </summary>
        public void InitMap()
        {
            if (System.IO.File.Exists(htmlpath))
            {
                //this.webBrowser.ObjectForScripting = this;
                this.webBrowser.Url = new Uri(htmlpath);
            }
            else
            {
                throw new Exception("HTML file (\"map.html\") not found!");
            }
        }

        #region Parser
        private List<string> candidateList = new List<string>();

        /// <summary>
        /// 候选要爬的线路
        /// </summary>
        public List<string> CandidateList
        {
            get { return this.candidateList; }
        }

        /// <summary>
        /// 路线列表
        /// </summary>
        private List<LineInfo> lineList = new List<LineInfo>();

        /// <summary>
        /// 添加路线信息
        /// </summary>
        /// <param name="lineInfo"></param>
        private void AddLineInfo(LineInfo lineInfo)
        {
            this.lineList.Add(lineInfo);
        }

        private delegate void invokeDelegate();
        private delegate void SetCurrentLineDelegate(string busline);

        /// <summary>
        /// 显示抓取的总站点数
        /// </summary>
        private void SetCountLabel()
        {
            if (!this.labelCount.InvokeRequired)
                this.labelCount.Text = "抓取的站点有: " + this.lineList.Count + " 个";
            else
            {
                this.labelCount.Invoke(new invokeDelegate(this.SetCountLabel));
            }
        }

        /// <summary>
        /// 显示当前正在解析的线路
        /// </summary>
        /// <param name="busLine"></param>
        private void SetCurrentLine(string busLine)
        {
            if (!this.lbCurrentLine.InvokeRequired)
                this.lbCurrentLine.Text = "当前正在解析的线路: " + busLine;
            else
            {
                this.lbCurrentLine.Invoke(new SetCurrentLineDelegate(this.SetCurrentLine), new object[] { busLine });
            }
        }

        /// <summary>
        /// 后台采集线程
        /// </summary>
        private void Parser()
        {
            try
            {
                for (int i = 0; i < this.candidateList.Count; i++)
                {
                    this.SetCurrentLine(this.candidateList[i]);
                    string escapeQuery = System.Web.HttpUtility.UrlEncode(this.candidateList[i], Encoding.GetEncoding("gb2312"));
                    Uri siteUri = new Uri(initUri + escapeQuery);
                    this.sawContent = this.GetContent(siteUri);

                    // 解析
                    LineInfo lineinfo = LineParser.GetLineInfo(this.sawContent, this.candidateList[i], this.CandidateList);
                    this.AddLineInfo(lineinfo);
                    this.SetCountLabel();
                }
                if (this.btnGo.InvokeRequired)
                    this.btnGo.Invoke(new EventHandler(this.SetButtonState));
                else
                    this.btnGo.Enabled = this.btnGo.Enabled ? false : true;
                // 数据输出到文件
                this.OutPutToXML();

                this.DisplayMessage("本次线路抓取解析完成,总共抓取线路:" + this.lineList.Count + " 条!", false);
            }
            catch (Exception err)
            {
                this.DisplayMessage(err.Message, true);
                if (this.btnGo.InvokeRequired)
                    this.btnGo.Invoke(new EventHandler(this.SetButtonState));
                else
                    this.btnGo.Enabled = this.btnGo.Enabled ? false : true;
            }
        }


        private void SetButtonState(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
                btn.Enabled = btn.Enabled ? false : true;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            this.lineList.Clear();
            this.candidateList.Clear();
            LineParser.ClearLineDics();
            string startLine = this.GetStartLine();
            if (string.IsNullOrEmpty(startLine))
                return;
            this.candidateList.Add(startLine);

            this.btnGo.Enabled = false;
            //
            LineParser.SetFirstLine(startLine);
            Thread workThread = new Thread(new ThreadStart(this.Parser));
            workThread.IsBackground = true;
            workThread.Start();

        }

        /// <summary>
        /// 获取起始线路
        /// </summary>
        /// <returns></returns>
        private string GetStartLine()
        {
            string startLine = this.tbBusLine.Text.Trim();
            if (string.IsNullOrEmpty(startLine))
                this.DisplayMessage("起始线路不能为空", true);
            return startLine;
        }

        /// <summary>
        /// 获取会话Cookie以便登录
        /// </summary>
        /// <returns>CookieCollection</returns>
        private void GetSessionCookie()
        {
            if (this.sessionCookie == null || this.sessionCookie.Count == 0)
            {
                HttpWebRequest req = null;
                try
                {
                    req = (HttpWebRequest)WebRequest.Create(FzbussysForm.crackUri);
                    req.Timeout = FzbussysForm.TIMEOUT;
                    req.KeepAlive = true;
                    req.CookieContainer = new CookieContainer();
                    HttpWebResponse response = (HttpWebResponse)req.GetResponse();

                    //this.DisplayMessage("cookies count is: " + response.Cookies.Count, false);
                    foreach (Cookie c in response.Cookies)
                    {
                        System.Diagnostics.Debug.WriteLine("cookieName: " + c.Name + " " + "cookieValue: " + c.Value);
                    }
                    if (response.Cookies.Count == 0)
                    {
                        throw new Exception("网站修改了规则,请重新分析源站点");
                    }
                    this.sessionCookie = response.Cookies;
                    response.Close();
                }
                catch (Exception e)
                {
                    this.DisplayMessage(e.Message, true);
                }
            }
        }

        /// <summary>
        /// 获取页面内容
        /// </summary>
        /// <param name="siteUri">站点URI</param>
        /// <returns>返回站点内容</returns>
        private string GetContent(Uri siteUri)
        {
            HttpWebRequest req = null;
            string content = null;
            try
            {
                this.GetSessionCookie();
                req = (HttpWebRequest)WebRequest.Create(siteUri);
                req.Timeout = FzbussysForm.TIMEOUT;
                req.CookieContainer = new CookieContainer();
                req.CookieContainer.Add(this.sessionCookie);
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("gB2312"));

                content = reader.ReadToEnd();
                reader.Close();
                response.Close();
            }
            catch (Exception e)
            {
                this.DisplayMessage(e.Message, true);
            }
            return content;
        }

        /// <summary>
        /// 获取uri
        /// </summary>
        /// <returns></returns>
        private Uri GetUri()
        {
            string strUri = this.tbBusLine.Text.Trim();
            Uri siteUri = null;
            if (string.IsNullOrEmpty(strUri))
            {
                this.DisplayMessage("URL不得为空", true);
            }
            else
            {
                try
                {
                    siteUri = new Uri(strUri);
                }
                catch (ArgumentNullException e)
                {
                    this.DisplayMessage(e.Message, true);
                }
                catch (UriFormatException e)
                {
                    this.DisplayMessage(e.Message, true);

                }
            }
            return siteUri;

        }

        /// <summary>
        /// 显示出错信息或者提示信息
        /// </summary>
        /// <param name="message">要显示的消息</param>
        /// <param name="isWarning">是否是警告或者普通提示</param>
        private void DisplayMessage(string message, bool isWarning)
        {
            MessageBox.Show(message, "公交线路解析", MessageBoxButtons.OK, isWarning ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
        }

        private void OutPutToXML()
        {
            try
            {
                lineList.Sort();
                int i = 1;
                XElement lineroot = new XElement("LineInfo");
                XElement stoproot = new XElement("StopInfo");

                XElement stopsroot = new XElement("Stops");
                XElement stoplinesroot = new XElement("StopLines");
                XElement linesroot = new XElement("Lines");
                XElement lineinfosroot = new XElement("Lineinfos");

                // lineroot
                foreach (LineInfo li in this.lineList)
                {
                    var lixe = li.ToXElement();

                    lixe.SetAttributeValue("LineID", (i++).ToString());

                    lineroot.Add(lixe);
                }

                // stoproot
                stoproot = CreatStop(lineroot, stoproot);

                // stops
                var stops =
                from stopxel in stoproot.Elements("Station")
                select new XElement
                ("stop",
                  new XElement("stopid", (int)stopxel.Attribute("staid")),
                  new XElement("stopname", (string)stopxel.Element("stationname"))
                );
                stopsroot.Add(stops);

                // lines
                var lines =
                from linexel in lineroot.Elements("Line")
                select new XElement
                (
                    "line",
                    new XElement("lineid", (int)linexel.Attribute("LineID")),
                    new XElement("linename", (string)linexel.Element("LineNo"))
                );
                linesroot.Add(lines);

                // stoplines
                i = 1;

                var stoplines =
                    from slxle in lineroot.Descendants("UpLine")
                    from stopxel in slxle.Elements("station")
                    let sid = from sxel in stoproot.Elements("Station")
                              where (string)sxel.Element("stationname") == (string)stopxel
                              select (int)sxel.Attribute("staid")
                    select new XElement
                    (
                        "stopline",
                        new XElement("stoplineid", i++),
                        new XElement("lineid", (int)stopxel.Ancestors("Line").First().Attribute("LineID")),
                        new XElement("stopid", sid.First()),
                        new XElement("seq", (int)stopxel.Attribute("seq"))
                    );
                stoplinesroot.Add(stoplines);

                // lineinfos
                var lineinfos =
                from linexel in lineroot.Elements("Line")
                select new XElement
                (
                    "lineinfo",
                    new XElement("lineid", (int)linexel.Attribute("LineID")),
                    new XElement("startstop", (string)linexel.Element("Start")),
                    new XElement("endstop", (string)linexel.Element("End")),
                    new XElement("starttime", (string)linexel.Element("StartBeginTime")),
                    new XElement("endtime", (string)linexel.Element("StartEndTime")),
                    new XElement("airconditioner", (string)linexel.Element("AirconditionerPrice")),
                    new XElement("nonairconditioner", (string)linexel.Element("NonAirconditionerPrice"))
                );

                lineinfosroot.Add(lineinfos);

                XElement root = new XElement("BusInfo", stopsroot, stoplinesroot, linesroot, lineinfosroot);

                root.Save("BusInfo.xml");
                DisplayMessage("数据输出完毕", false);
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message, true);
            }
        }

        public XElement CreatStop(XElement linexe, XElement stopxe)
        {
            List<Station> stations = new List<Station>();

            var stationGroups = from stationElement in linexe.Descendants("station")
                                group stationElement by (string)stationElement;
            // Fill Stations
            foreach (var group in stationGroups)
            {
                List<string> linespast = new List<string>();
                foreach (var xelement in group)
                {
                    var linename = (string)xelement.Ancestors("Line").First().Element("LineNo");
                    if (!linespast.Contains(linename))
                    {
                        linespast.Add(linename);
                    }
                }
                stations.Add(new Station { StationName = group.Key, LinesPast = linespast });
            }

            // Fill stopxe
            int i = 1;

            foreach (var station in stations)
            {
                List<XElement> linepast = new List<XElement>();

                foreach (var line in station.LinesPast)
                {
                    XElement eachline = new XElement("linepast", line);
                    linepast.Add(eachline);
                }

                XElement sta = new XElement("Station",
                               new XElement("stationname", station.StationName),
                               linepast
                );

                sta.SetAttributeValue("staid", (i++).ToString());

                stopxe.Add(sta);
            }
            return stopxe;
        }

        # endregion

        #region Search Controllers Event Handler
        /// <summary>
        /// 点击公交车次查询后调用该方法
        /// </summary>
        private void btn_linesearch_Click(object sender, EventArgs e)
        {
            string linename = this.comboBox_line.Text;
            lineinfo linesearched = db.GetLineinfoByLineName(linename);
            if (linesearched != null)
            {
                this.textBox_searchresult.Text = "查找成功!\n" + linesearched.ToLineString(db);
                this.webBrowser.Document.InvokeScript("routSearchByBusLineName", new object[] { "0591", linename });
            }
            else
            {
                this.textBox_searchresult.Text = "查找失败！没有该车次。";
            }
        }

        /// <summary>
        /// 点击公交站点查询后调用该方法
        /// </summary>
        private void btn_stopsearch_Click(object sender, EventArgs e)
        {
            string stopname = this.comboBox_stop.Text;
            stop stopsearched = db.GetStopByStopname(stopname);
            if (stopsearched != null)
            {
                this.textBox_searchresult.Text = "查找成功!\n " + stopsearched.ToStopString(db);
            }
            else
            {
                this.textBox_searchresult.Text = "查找失败！没有该站点。";
            }
        }

        /// <summary>
        /// 点击公交站站查询后调用该方法
        /// </summary>
        private void btn_stoptostop_Click(object sender, EventArgs e)
        {
            stop startstop = db.GetStopByStopname(this.comboBox_start.Text);
            stop endstop = db.GetStopByStopname(this.comboBox_end.Text);

            if (startstop != null && endstop != null)
            {
                SearchPath sp = new SearchPath(db, startstop, endstop);

                SearchResult searchresult = sp.FindPath();

                if (searchresult != null)
                {
                    this.textBox_searchresult.Text = "查找成功！\r\n" + searchresult.Result;
                }
                else
                {
                    this.textBox_searchresult.Text = "查找失败！没有找到直达的或要转乘一次的路线！";
                }
            }
            else if (startstop != null && endstop == null)
            {
                this.textBox_searchresult.Text = "查找失败！没有查找到终点站！";
            }
            else if (startstop == null && endstop != null)
            {
                this.textBox_searchresult.Text = "查找失败！没有查找到起点站！";
            }
            else
            {
                this.textBox_searchresult.Text = "查找失败！没有查找到起点站和终点站！";
            }
        }

        /// <summary>
        /// WebBrowser控件加载完成后发生，调用JavaScript初始化函数mapInit()
        /// </summary>
        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
        #endregion
    }
}
