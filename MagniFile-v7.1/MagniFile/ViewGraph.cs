using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// Chart
using System.Globalization;
using System.Windows.Forms.DataVisualization.Charting;

// Screen capture
using System.Drawing.Imaging;

// Printing
using System.Printing;
using System.Drawing.Printing;

// Access assembly (used by html help)
using System.Reflection;

using System.IO;

// NetStats
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Diagnostics;

// http://blogs.msdn.com/alexgor/archive/2009/10/06/setting-chart-series-colors.aspx

namespace MagniFile
{
    /// <summary>
    /// General Graph Dialog.
    /// Graph one or more ulong values extracted from a ListViewItem subitem tags
    /// 
    /// Author: Dennis Lang 2009
    /// https://landenlabs.com/
    /// 
    /// </summary>
    public partial class ViewGraph : Form
    {
        public ViewGraph(Image imageLogo)
        {
            InitializeComponent();

            logo = imageLogo;

            // Prevent viewer from being larger than working area.
            if (this.Height > Screen.GetWorkingArea(this).Height * 0.9)
                this.Height = (int)(Screen.GetWorkingArea(this).Height * 0.9);

            this.Text = this.title = "Graph - " + MainForm.ProductNameAndVersion();
            UpdateSysView();
            InitChart();
            InitDemo();

            NumMaxPrimYaxis.Value = maxPrimYaxis;
            Assembly a = Assembly.GetExecutingAssembly();
            Stream htmlStream = a.GetManifestResourceStream("MagniFile.HelpGraph.html");
            this.helpHtml.DocumentStream = htmlStream;

            ListViewHelper.EnableDoubleBuffer(this.legendView);

            this.legendView.ListViewItemSorter = lvSorter;
        }

        #region ==== Data Members

        Image logo;
        string title;
        ListView listView;
        ListViewColumnSorter lvSorter = new ListViewColumnSorter(ListViewColumnSorter.SortDataType.eAlpha);

        bool ignoreChanges  = false;
        ulong maxPrimYaxis  = 1000;
        int lineWidth       = 2;
        int[] idCols        = new int[] { ListViewProcesses.pNameCol, ListViewProcesses.pIdCol };
        int dateTimeCol     = ListViewProcesses.pDTCol;
        int firstParmCol    = ListViewProcesses.pIoReadCol;
        System.Windows.Forms.DataVisualization.Charting.SeriesChartType chartType =
             System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;

        Color demoColor = Color.FromArgb(255, 255, 210, 210);     //    Color.LightPink
        Color systemColor = Color.FromArgb(255, 255, 255, 210);   //    Color.LightYellow);

        struct SeriesAux
        {
            public double scale;
            public int offset;
        }

        public ListView GraphListView
        {
            get { return listView; }
            set { listView = value; UpdateAll(); }
        }

        /// <summary>
        /// Array of Process IDs actively being Logged.
        /// <Name><Pid>,...
        /// </summary>
        public string LogNamePids { set; get; }

        #endregion

        #region ==== Initialize Stuff

        public Color Invert(Color c)
        {
            if (c.ToArgb() == 0x000000ff)
                return  Color.FromArgb(-1);
            else if (c.ToArgb() == -1)
                return Color.FromArgb(0x000000ff);

            return Color.FromArgb(c.A, 255-c.R, 255-c.G, 255-c.B);
        }

        public void InitSeries(Series series, ulong n)
        {
            series.ChartType = chartType;
            series.BorderWidth = lineWidth;
            series.CustomProperties = "LineTension=0.0";

            // series.ShadowOffset = series.BorderWidth + 1;
            series.ShadowColor = Invert(chart.ChartAreas[0].BackColor);

            // series.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            series.YAxisType = n < maxPrimYaxis ?
                           System.Windows.Forms.DataVisualization.Charting.AxisType.Primary :
                           System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
        }

        /// <summary>
        /// Set axis color scheme.
        /// </summary>
        public void InitAxis()
        {
            Color axisXFontColor = Color.White;
            Color axisXColor = Color.Green;

            // chart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Minutes;
            // chart.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm:ss";
            chart.ChartAreas[0].AxisX.LabelStyle.ForeColor = axisXFontColor;
            chart.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            chart.ChartAreas[0].AxisX.MinorGrid.LineColor = axisXColor;
            chart.ChartAreas[0].AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dot;

            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chart.ChartAreas[0].AxisX.MajorGrid.LineColor = axisXColor;
            chart.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

            Color axisYFontColor = Color.White;
            Color axisY1Color = Color.Green;
            Color axisY2Color = Color.White;
            chart.ChartAreas[0].AxisY.LabelStyle.ForeColor = axisYFontColor;
            // chart.ChartAreas[0].AxisY.LabelStyle.Format = "F2";
            chart.ChartAreas[0].AxisY.LabelStyle.Format = "#,##0";
            chart.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            chart.ChartAreas[0].AxisY.MajorGrid.LineColor = axisY1Color;
            chart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid;

            chart.ChartAreas[0].AxisY2.LabelStyle.ForeColor = axisYFontColor;
            chart.ChartAreas[0].AxisY2.LabelStyle.Format = "#,##0";
            chart.ChartAreas[0].AxisY2.MajorGrid.Enabled = true;
            chart.ChartAreas[0].AxisY2.MajorGrid.LineColor = axisY2Color;
            chart.ChartAreas[0].AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Solid;
        }

        /// <summary>
        /// Set Chart color scheme.
        /// </summary>
        public void InitChart()
        {
            chart.Series.Clear();
            chart.BackColor = Color.DarkGray;
            chart.BackSecondaryColor = this.BackColor;
            chart.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;

            InitAxis();

            if (chart.Legends != null)
                this.chart.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
        }

        /// <summary>
        /// Update Series and ignore any exceptions (axis out of bounds with new data)
        /// </summary>
        private int AddXY(Series series, double x, double y)
        {
            try
            {
                return series.Points.AddXY(x, y);
            }
            catch 
            {
                // ToDo - update Yaxis so Y is inside min and max
                return series.Points.Count;
            }
        }

        #region ===== Demo Graph
        /// <summary>
        /// Create a demo graph so something appears when you open the window.
        /// </summary>
        const int numDemoValues = 60*5;
        double[] demoYvalues = new double[numDemoValues];
        public void InitDemo()
        {
            Series series = this.chart.Series.Add("Demo");
            // chart.ApplyPaletteColors();
            series.Tag = new SeriesAux { offset = 0, scale = 1.0 };
            series.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            InitSeries(series, 500);

            Random rand = new Random();
            DateTime now = DateTime.Now;
            TimeSpan ts = new TimeSpan(0, 0, 1);

            for (int i = 0; i < numDemoValues; i++)
            {
                double x = now.ToOADate();
                now -= ts;
                double y = rand.NextDouble() * 500;
                AddXY(series, x, y);
                demoYvalues[i] = y;
            }
            UpdateLegend(series, demoColor, false);   
            UpdateTitle();
        }


        /// <summary>
        /// Refill series with demo points using current time.
        /// </summary>
        public void RefillDemoSeries(Series series)
        {
            if (series.Tag == null)
                return;

            series.Points.Clear();
            SeriesAux aux = (SeriesAux)series.Tag;
            DateTime now = DateTime.Now;
            TimeSpan ts = new TimeSpan(0, 0, 1);

            for (int i = 0; i < numDemoValues; i++)
            {
                double xValue = now.ToOADate();
                now -= ts;
                double yValue = demoYvalues[i] * aux.scale + aux.offset;
                AddXY(series, xValue, yValue);
            }
        }
        #endregion
        #endregion

        public bool LvContains(string key, ListView.CheckedListViewItemCollection ckItems)
        {
            foreach (ListViewItem ckbox in ckItems)
            {
                if (ckbox.Text == key)
                    return true;
            }

            return false;
        }

        private string ListId(ListViewItem procItem)
        {
            string id = string.Empty;
            foreach (int col in idCols)
                id += procItem.SubItems[col].Text;
            return id;
        }

        #region ===== Manage Close (hide instead of close)
        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

#if false
            // Pause graph if not visible, resume if visible.
            this.pause = !this.Visible;
            this.pauseBtn.Text = this.pause ? "Resume" : "Pause";
            labelRight.ForeColor = pause ? Color.Red : labelCenter.ForeColor;
#endif
            if (this.Visible)
            {
                UpdateAll();
                tabs.SelectedTab = tabs.TabPages[procList.Items.Count != 0 ? 0 : 1];
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;        // cancel close, just hide dialog
            base.OnClosing(e);
            this.Visible = false;
        }
        #endregion

        public void UpdateAll()
        {
            if (listView != null)
            {
                maxPrimYaxis = (ulong)NumMaxPrimYaxis.Value;
                UpdateProcList();
                UpdateParmList();
                UpdateSeries();
                this.propertyGrid.SelectedObject = this.chart;
            }
        }

        public void UpdateGraph(ListViewItem item)
        {
            if (pause == false)
            {
                UpdateSeries(item);
                UpdateSysGraph();
                UpdateLegend(LastPointIndex);
                UpdateTitle();
                UpdateAxis();
            }
        }

        private void UpdateTitle()
        {
            this.labelLeft.Text = Environment.MachineName.ToString();
            this.labelCenter.Text = MainForm.ProductNameAndVersion();
            this.labelRight.Text = DateTime.Now.ToString("G");
        }

        #region ==== Process Data Parameters
        public void UpdateProcList()
        {
            Dictionary<string, bool> idList = IdList(listView);
            foreach (ListViewItem ckbox in procList.CheckedItems)
            {
                if (idList.ContainsKey(ckbox.Text))
                    idList[ckbox.Text] = true;
            }

            ignoreChanges = true;
            procList.BeginUpdate();
            procList.Items.Clear();
            foreach (string key in idList.Keys)
            {
                ListViewItem procLv = procList.Items.Add(key);
                procLv.Checked = idList[key];
                if (LogNamePids != null && !LogNamePids.Contains(key + ","))
                    procLv.ForeColor = Color.Red;
            }

            if (this.procList.Items.Count != 0)
                this.procList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            procList.EndUpdate();
            ignoreChanges = false;
        }

        public Dictionary<string, bool> IdList(ListView listView)
        {
            Dictionary<string, bool> idList = new Dictionary<string, bool>();

            foreach (ListViewItem item in listView.Items)
            {
                string id = ListId(item);
                idList[id] = false;
            }

            return idList;
        }

        public void UpdateParmList()
        {
            Dictionary<string, int> colList = new Dictionary<string, int>();
            foreach (ColumnHeader colHdr in listView.Columns)
            {
                if (colHdr.Index >= firstParmCol && colHdr.TextAlign == HorizontalAlignment.Right)
                    colList[colHdr.Text] = colHdr.Index;
            }

            foreach (ListViewItem ckbox in parmList.CheckedItems)
            {
                if (colList.ContainsKey(ckbox.Text))
                    colList[ckbox.Text] += -100;
            }

            ignoreChanges = true;
            parmList.BeginUpdate();
            parmList.Items.Clear();
            foreach (string key in colList.Keys)
            {
                int col = colList[key];
                bool on = col < 0;
                col = on ? col + 100 : col;
                ListViewItem item = parmList.Items.Add(key);
                item.Checked = on;
                item.Tag = col;
            }

            if (this.parmList.Items.Count != 0)
                this.parmList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            parmList.EndUpdate();
            parmList.Enabled = procList.Items.Count != 0;
            ppSplitter.Visible = procList.Items.Count != 0;

            ignoreChanges = false;
        }

        #endregion

        #region ==== Update System Graph Parameteres
        enum SysGraph
        {
            None, 
            DiskQueueDepth, DiskNumReads, DiskNumWrites, DiskBytesRead, DiskBytesWritten,

            TcpSegReceived, TcpSegSent, TcpSeqResent, 
            UdpDgramReceived, UdpDgramSent, UdpDgramDiscarded,

            ContextSwitches,DpcCount,DpcRate,TimeIncrement,
            DpcBypassCount,ApcBypassCount,

            ProcessLoadPercent, MegMemoryAvailable,
        };

        public void UpdateSysView()
        {
            sysList.Items.Clear();
            sysList.BeginUpdate();

            sysList.Items.Add("Process Load").Tag = SysGraph.ProcessLoadPercent;
            sysList.Items.Add("Memory Free").Tag = SysGraph.MegMemoryAvailable;

            DriveInfo[] diList = DriveInfo.GetDrives();
            foreach (DriveInfo di in diList)
            {
                if (di.DriveType == DriveType.Fixed && di.IsReady)
                {
                    sysList.Items.Add(di.Name + " Queue Depth").Tag = SysGraph.DiskQueueDepth;
                    sysList.Items.Add(di.Name + " #Reads").Tag = SysGraph.DiskNumReads;
                    sysList.Items.Add(di.Name + " #Writes").Tag = SysGraph.DiskNumWrites;
                    sysList.Items.Add(di.Name + " Bytes Read").Tag = SysGraph.DiskBytesRead;
                    sysList.Items.Add(di.Name + " Bytes Written").Tag = SysGraph.DiskBytesWritten;
                }
            }

            sysList.Items.Add("Context Switches").Tag = SysGraph.ContextSwitches;
            sysList.Items.Add("Dpc Count").Tag = SysGraph.DpcCount;
            // sysList.Items.Add("Dpc Rate").Tag = SysGraph.DpcRate;
            // sysList.Items.Add("Time Increment").Tag = SysGraph.TimeIncrement;
            // sysList.Items.Add("Dpc Bypass Count").Tag = SysGraph.DpcBypassCount;
            // sysList.Items.Add("Apc Bypass Count").Tag = SysGraph.ApcBypassCount;

            sysList.Items.Add("Tcp Segments Received").Tag = SysGraph.TcpSegReceived;
            sysList.Items.Add("Tcp Segments Sent").Tag = SysGraph.TcpSegSent;
            sysList.Items.Add("Tcp Segments Resent").Tag = SysGraph.TcpSeqResent;
            sysList.Items.Add("Udp Datagrams Received").Tag = SysGraph.UdpDgramReceived;
            sysList.Items.Add("Udp Datagrams Sent").Tag = SysGraph.UdpDgramSent;
            sysList.Items.Add("Udp Datagrams Discarded").Tag = SysGraph.UdpDgramDiscarded;
            sysList.EndUpdate();
            sysList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
        #endregion

        #region ==== Legend 

        // Legend View columns
        public enum LegCol : int 
        { 
            series = 0,
            color,
            width,
            yaxis,
            min,
            val,
            max,
            offset,
            scale,
            num,
            chartType,
            last
        }
        
#if false
        // this does not help, still need to cast enums to use as array index.
        public static implicit operator int(LegCol a)
        {
            return (int)a;
        }
#endif
        public void UpdateLegend()
        {
            // this.legendView.Items.Clear();

            foreach (Series series in chart.Series)
            {
                UpdateLegend(series, legendView.BackColor, false);
            }
        }

        public void UpdateLegend(Series series, Color backColor, bool clearMinMax)
        {
            SeriesAux aux = (SeriesAux)series.Tag;

            ListViewItem item = null;
            item = this.legendView.FindItemWithText(series.Name);
            if (item == null)
            {
                ignoreChanges = true;
                item = this.legendView.Items.Add(series.Name);
                item.BackColor = backColor;
                item.Checked = series.Enabled;
                item.UseItemStyleForSubItems = false;
                ignoreChanges = false;
            }

            item.Tag = series;

            while (item.SubItems.Count < (int)LegCol.last)
                item.SubItems.Add("").BackColor = item.BackColor;

            chart.ApplyPaletteColors();
            ChartColorPalette cp = series.Palette;
            Color c = series.Color;
            item.SubItems[(int)LegCol.color].BackColor = c;
            item.SubItems[(int)LegCol.width].Text = series.BorderWidth.ToString();
            item.SubItems[(int)LegCol.yaxis].Text = ToString(series.YAxisType);
            item.SubItems[(int)LegCol.chartType].Text = series.ChartType.ToString();

            item.SubItems[(int)LegCol.offset].Text = aux.offset.ToString();
            item.SubItems[(int)LegCol.scale].Text = aux.scale.ToString("F2");

            if (clearMinMax)
            {
                item.SubItems[(int)LegCol.val].Text = "-";
                item.SubItems[(int)LegCol.min].Text = string.Empty;
                item.SubItems[(int)LegCol.max].Text = string.Empty;
                item.SubItems[(int)LegCol.num].Text = series.Points.Count.ToString("#,##0");
            }

            ignoreChanges = true;
            item.Checked = series.Enabled;
            ignoreChanges = false;
        }

        const int LastPointIndex = -1;
        public void UpdateLegend(int pointIndex)
        {
            int seriesPtIdx = pointIndex;
            double xValue = seriesPtIdx;

            foreach (Series series in chart.Series)
            {
                ListViewItem item = this.legendView.FindItemWithText(series.Name);
                if (item != null)
                {
                    item.SubItems[(int)LegCol.val].Text = "";
                    seriesPtIdx = (pointIndex == LastPointIndex) ? series.Points.Count - 1 : pointIndex;

                    if (seriesPtIdx >= 0 && seriesPtIdx < series.Points.Count)
                    {
                        DataPoint dp = series.Points[seriesPtIdx];
                        xValue = dp.XValue;
                        ulong n = (ulong)dp.YValues[0];
                        item.SubItems[(int)LegCol.val].Text = n.ToString("#,##0");
                        item.SubItems[(int)LegCol.min].Text = series.Points.FindMinByValue().YValues[0].ToString("#,##0");
                        item.SubItems[(int)LegCol.max].Text = series.Points.FindMaxByValue().YValues[0].ToString("#,##0");
                        item.SubItems[(int)LegCol.num].Text = series.Points.Count.ToString("#,##0");
                    }
                }
            }

            DateTime dt = DateTime.FromOADate(xValue);
            this.legendView.Columns[(int)LegCol.val].Text = dt.ToString("hh:mm:ss");
        }
        #endregion

        #region ==== Chart
        private void UpdateAxis()
        {
            UpdateXAxis();
            UpdateYAxis();
        }

        private void UpdateXAxis()
        {
            // Adjust X span
            Axis xAxis = chart.ChartAreas[0].AxisX;
            double nowD = DateTime.Now.ToOADate();

            // Make sure now is within X time range.
            if (nowD < xAxis.Minimum || nowD > xAxis.Maximum)
            {
                DateTime min2DT = DateTime.Now - xTimeSpan;
                double minD = min2DT.ToOADate();

                if (xAxis.Minimum < minD)
                    xAxis.Minimum = minD;
                if (nowD > xAxis.Maximum)
                    xAxis.Maximum =nowD;
            }

            DateTime maxDT = DateTime.FromOADate(xAxis.Maximum);
            DateTime minDT = DateTime.FromOADate(xAxis.Minimum);
            TimeSpan span = maxDT - minDT;

            // Shrink time span to not exceed specifications.
            if (span > xTimeSpan)
            {
                // Proportionally shrink min and max from 'now'.
                double dScale = xTimeSpan.TotalMinutes / (span.TotalMinutes);

                xAxis.Minimum = nowD - (nowD - xAxis.Minimum) * dScale;
                xAxis.Maximum = nowD + (xAxis.Maximum - nowD) * dScale;
            }

            // Remove excess series data points.
            int maxSeriesPnts = (int)NumMaxPoints.Value;
            foreach (Series series in chart.Series)
            {
                if (series.Points.Count > maxSeriesPnts)
                {
                    // Remove extra points from left.
                    int ptIdx = 1;
                    while (series.Points.Count > maxSeriesPnts && series.Points[ptIdx].XValue < nowD)
                    {
                        series.Points.RemoveAt(ptIdx);
                        ptIdx = (ptIdx + 2) % series.Points.Count;
                    }
                }
            }

            //chart.Update();
            minXText.Text = minDT.ToString("G");
            maxXText.Text = maxDT.ToString("G");
        }

        private void UpdateYAxis()
        {
            // TODO - auto scale Y1 and Y2 axis to keep new data in view.
            // foreach (Series series in chart.Series)
        }

        private void UpdateYAxis(Series series, double yValue)
        {
            // TODO - only adjust axis if not in manual move/zoom mode.
            Axis yAxis = (series.YAxisType == AxisType.Primary) ?
                chart.ChartAreas[0].AxisY : chart.ChartAreas[0].AxisY2;

            bool updateUI = false;
            if (yValue > yAxis.Maximum)
            {
                yAxis.Maximum = yValue * 1.01;
                updateUI = true;
            }
            else if (yValue < yAxis.Minimum)
            {
                yAxis.Minimum = yValue * 0.99;
                updateUI = true;
            }

            if (updateUI)
            {
                if (series.YAxisType == AxisType.Primary)
                {
                    minY1axis.Text = yAxis.Minimum.ToString("#,##0");
                    maxY1axis.Text = yAxis.Maximum.ToString("#,##0");
                }
                else
                {
                    minY2axis.Text = yAxis.Minimum.ToString("#,##0");
                    maxY2axis.Text = yAxis.Maximum.ToString("#,##0");
                }
            }

            this.Text = "Graph " + yValue.ToString() + " " + series.Name;
        }

        public void UpdateSeriesSettings()
        {
            int maxSeriesCnt = 0;
            foreach (Series series in chart.Series)
            {
                if (series.Points.Count != 0)
                {
                    maxSeriesCnt = Math.Max(maxSeriesCnt, series.Points.Count);
                    DataPoint dp = series.Points.FindMaxByValue();
                    ulong n = (dp != null) ? (ulong)dp.YValues[0] : 0;

                    if (series.Points.Count == 1)
                        InitSeries(series, n);
                }
            }

            System.Windows.Forms.DataVisualization.Charting.Axis axis;
            axis = chart.ChartAreas[0].AxisX;
        }

        public void UpdateSeries()
        {
            if ((this.procList.CheckedItems.Count != 0 &&
                this.parmList.CheckedItems.Count != 0) ||
                this.sysList.CheckedItems.Count != 0)
            {
                //
                // . Clear Legends
                // . Clear series data points
                // . Add and missing series and add points to series
                // . Add system series
                // . Remove any empty series
                // . Create legends
                //

                Series demoSeries = this.chart.Series.FindByName("Demo");
                if (demoSeries != null)
                {
                    demoSeries.Enabled = false;
                    UpdateLegend(demoSeries, demoColor, false);
                }

                // legendView.Items.Clear();
                foreach (ListViewItem item in listView.Items)
                    UpdateSeries(item);

                foreach (ListViewItem ckBox in sysList.CheckedItems)
                {
                    Series series = chart.Series.FindByName(ckBox.Text);
                    if (series == null)
                    {
                        series = chart.Series.Add(ckBox.Text);
                        // chart.ApplyPaletteColors();
                        series.Tag = new SeriesAux { offset = 0, scale = 1.0 };
                        series.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
                        series.Enabled = true;
                    }
                }

                UpdateSysGraph();

                // Remove empty series
                for (int sIdx = 0; sIdx < chart.Series.Count; sIdx++)
                {
                    Series series = chart.Series[sIdx];
                    // if (series.Points.Count == 0)
                    //    chart.Series.Remove(series);
                }

                UpdateLegend();
            }
            else
            {
                // demo
                if (chart.Series.Count == 0)
                {
                    InitDemo();
                    UpdateLegend();
                }

                Series demoSeries = this.chart.Series.FindByName("Demo");
                if (demoSeries != null)
                {
                    demoSeries.Enabled = true;
                    UpdateLegend(demoSeries, demoColor, false);
                }

                RefillDemoSeries(chart.Series[0]);
            }
        }

        private void UpdateSeries(ListViewItem procItem)
        {
            string pname = ListId(procItem);
            if (LvContains(pname, procList.CheckedItems))
            {
                System.Windows.Forms.DataVisualization.Charting.Series series;

                foreach (ListViewItem ckBox in parmList.CheckedItems)
                {
                    int col = (int)ckBox.Tag;

                    DateTime dt;
                    string dtStr = procItem.SubItems[dateTimeCol].Text;
                    bool gotDt = (DateTime.TryParse(dtStr, out dt) ||
                        DateTime.TryParseExact(dtStr, "G", CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out dt));
                    if (!gotDt)
                        MessageBox.Show("Failed to parse time from:" + dtStr);

                    string parm = ckBox.Text;
                    string seriesName = pname + " " + parm;

                    series = chart.Series.FindByName(seriesName);
                    if (procItem.SubItems[col].Tag == null)
                    {
                        ckBox.Checked = false;
                        continue;
                    }

                    ulong n = (ulong)procItem.SubItems[col].Tag;

                    if (series == null)
                    {
                        series = chart.Series.Add(seriesName);
                        // chart.ApplyPaletteColors();
                        series.Tag = new SeriesAux { offset = 0, scale = 1.0 };
               
                        if (gotDt)
                            series.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
                    }

                    if (series != null)
                    {
                        SeriesAux aux = (SeriesAux)series.Tag;
                        double yValue = n * aux.scale + aux.offset;
                        double xValue = dt.ToOADate();

                        int lastPtIdx;
                        if (gotDt)
                        {

                            // Avoid add duplicate points.
                            lastPtIdx = series.Points.Count -1;
                            if (lastPtIdx >= 0 && series.Points[lastPtIdx].XValue >= xValue)
                                series.Points.Clear();

                            lastPtIdx = AddXY(series, xValue, yValue);
                        }
                        else
                        {
                            lastPtIdx = series.Points.AddY(yValue);
                        }

                        xValue = series.Points[lastPtIdx].XValue;
                        chart.ChartAreas[0].AxisX.Maximum = xValue; 

                        if (series.Points.Count == 1)
                        {
                            InitSeries(series, (ulong)yValue);
                            UpdateLegend(series, this.legendView.BackColor, false);
                        }

                        if (autoY0axisCb.Checked)
                            resetY0axisBtn_Click(null, EventArgs.Empty);

                    }
                }

                System.Windows.Forms.DataVisualization.Charting.Axis a;
                a = chart.ChartAreas[0].AxisX;
            }
        }
        #endregion

        #region ==== Chart Mouse and Keyboard
        DataPoint lastDp = null;
        Point mouseDown;

        private void chart_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = e.Location;
        }

        private void chart_KeyUp(object sender, KeyEventArgs e)
        {
            Axis xAxis = chart.ChartAreas[0].AxisX;
            double xInc = (xAxis.Maximum - xAxis.Minimum) / 50.0;

            switch (e.KeyCode)
            {
                case Keys.Left:
                    xAxis.Minimum += xInc;
                    xAxis.Maximum += xInc;
                    break;
                case Keys.Right:
                    xAxis.Minimum -= xInc;
                    xAxis.Maximum -= xInc;
                    break;
            }

        }

        private void chart_MouseMove(object sender, MouseEventArgs e)
        {
            HitTestResult hitResult;
            try
            {
                hitResult = chart.HitTest(e.X, e.Y);
                chart.Focus();
            }
            catch
            {
                return;
            }
            this.status.Text = string.Empty;

            // Limit Axis hit to outsize edge
            Rectangle rect = chart.DisplayRectangle;
            rect.Inflate(-100, -50);
            rect.Y = 0;

            if (hitResult.Axis != null && !rect.Contains(e.Location))
            {
                this.status.Text += " Axis " + hitResult.Axis.Name;
               
                if (e.Button == MouseButtons.Left)
                {
                    hitResult.Axis.LabelStyle.ForeColor = Color.Red;
                    int yChg = e.Y - mouseDown.Y;
                    int xChg = mouseDown.X - e.X;
                    double span = (hitResult.Axis.Maximum - hitResult.Axis.Minimum);

                    if (hitResult.Axis == chart.ChartAreas[0].AxisX)
                    {
                        double  xMove = (span * xChg / 100.0);
                        hitResult.Axis.Minimum = Math.Min(hitResult.Axis.Minimum, hitResult.Axis.Maximum + xMove);
                        hitResult.Axis.Maximum += xMove;
                    }
                    else
                    {
                        // int yMove = (int)(hitResult.Axis.Maximum * yChg / 100);
                        double yMove = (span * yChg / 100);
                        if (yMove > 10)
                            yMove = (int)yMove;
                        hitResult.Axis.Minimum = Math.Min(hitResult.Axis.Minimum, hitResult.Axis.Maximum + yMove);
                        hitResult.Axis.Maximum += yMove;
                    }

                    mouseDown = e.Location;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    hitResult.Axis.LabelStyle.ForeColor = Color.Orange;
                    int yChg = e.Y - mouseDown.Y;
                    int xChg = mouseDown.X - e.X;
                    double span = (hitResult.Axis.Maximum - hitResult.Axis.Minimum);

                    if (hitResult.Axis == chart.ChartAreas[0].AxisX)
                    {
                        double xMove = (span * xChg / 100.0);
                        hitResult.Axis.Maximum += xMove;
                        hitResult.Axis.Minimum += xMove;
                    }
                    else
                    {
                        // int yMove = (int)(hitResult.Axis.Maximum * yChg / 100);
                        double yMove = (span * yChg / 100);
                        if (yMove > 10)
                            yMove = (int)yMove;
                        hitResult.Axis.Maximum += yMove;
                        hitResult.Axis.Minimum += yMove;
                    }

                    mouseDown = e.Location;
                }
                else
                    hitResult.Axis.LabelStyle.ForeColor = Color.White;

            }
            
            if (hitResult.ChartArea != null)
            {
                this.status.Text += " Area";
                if (e.Button == MouseButtons.Right)
                {
                    int xChg = mouseDown.X - e.X;
                    Axis axis = chart.ChartAreas[0].AxisX;
                    double span = (axis.Maximum - axis.Minimum);
                    double xMove = (span * xChg / 100.0);
                    axis.Maximum += xMove;
                    axis.Minimum += xMove;
                    mouseDown = e.Location;
                }
            }
            
            if (hitResult.Series != null)
            {
                Series series = hitResult.Series;
                this.status.Text += " Series " + series.Name;
                this.status.Text += " Index " + hitResult.PointIndex.ToString();

                if (lastDp != null)
                {
                    lastDp.MarkerStyle = MarkerStyle.None;
                    lastDp.Label = "";
                }

                if (hitResult.PointIndex != -1)
                {
                    SeriesAux aux = (SeriesAux)series.Tag;

                    lastDp = series.Points[hitResult.PointIndex];
                    double val = lastDp.YValues[0];
                    lastDp.MarkerStyle = MarkerStyle.Circle;
                    lastDp.MarkerSize = 10;
                    int prevIdx = Math.Max(0, hitResult.PointIndex - 10);
                    string dpStr = string.Format("-\n {0} {1:G} \n [{2:#,##0}] at {3} \n Slope(10pts):\n{4:G}\n-",
                        hitResult.Series.Name, 
                        val,
                        (val - aux.offset) / aux.scale,
                        DateTime.FromOADate(lastDp.XValue).ToString("G"),
                        (val - series.Points[prevIdx].YValues[0])/(hitResult.PointIndex - prevIdx));
                    // lastDp.ToolTip = dpStr;
                    // lastDp.LabelToolTip = dpStr;
                    lastDp.Label = dpStr;
                    lastDp.LabelBackColor = Color.LightGray;

                    this.status.Text += " Value=" + val.ToString();
                    UpdateLegend(hitResult.PointIndex);
                    UpdateDataView(hitResult.PointIndex, e.Button == MouseButtons.Left, series);
                }
            }
        }

        #endregion

        #region ==== User Interface callbacks
        private void refreshBtn_Click(object sender, EventArgs e)
        {
            UpdateAll();
        }

        private void sysView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (ignoreChanges == false)
            {
                Series demoSeries = this.chart.Series.FindByName("Demo");
                if (demoSeries != null)
                {
                    demoSeries.Enabled = false;
                    UpdateLegend(demoSeries, demoColor, false);
                }
                UpdateSeries();             
            }
        }

        private void parmList_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (ignoreChanges == false)
                UpdateSeries();
        }

        private void procList_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (ignoreChanges == false)
                UpdateSeries();
        }

        TimeSpan xTimeSpan = new TimeSpan(0, 0, 5, 0, 0);
        private void ValueChanged(object sender, EventArgs e)
        {
            maxPrimYaxis = (ulong)NumMaxPrimYaxis.Value;
            int xAxisValue = (int)NumxSpan.Value;
            switch (xSpanUnits.Text)
            {
                case "Seconds":
                    xTimeSpan = new TimeSpan(0, 0, xAxisValue);
                    break;
                case "Minutes":
                    xTimeSpan = new TimeSpan(0, xAxisValue, 0);
                    break;
                case "Hours":
                    xTimeSpan = new TimeSpan(xAxisValue, 0, 0);
                    break;
                case "Days":
                    xTimeSpan = new TimeSpan(xAxisValue, 0, 0, 0); 
                    break;
            }

            if (ignoreChanges == false)
            {
                UpdateXAxis();
                UpdateSeriesSettings();
            }
        }

        private void UnitChanged(object sender, EventArgs e)
        {
            double xSpanValue = 0;
            switch (xSpanUnits.Text)
            {
                case "Seconds":
                    xSpanValue = xTimeSpan.TotalSeconds;
                    break;
                case "Minutes":
                    xSpanValue = xTimeSpan.TotalMinutes;
                    break;
                case "Hours":
                    xSpanValue = xTimeSpan.TotalHours;
                    break;
                case "Days":
                    xSpanValue = xTimeSpan.TotalDays;
                    break;
            }

            if (xSpanValue < 1)
                xSpanValue = 1;
            ignoreChanges = true;
            NumxSpan.Value = (int)xSpanValue;
            ignoreChanges = false;
        }


        bool pause = false;
        private void pauseBtn_Click(object sender, EventArgs e)
        {
            pause = !pause;
            pauseBtn.Text = pause ? "Resume" : "Pause";
            labelRight.ForeColor = pause ? Color.Red : labelCenter.ForeColor;
        }

        private void legendView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (ignoreChanges == false)
            {
                Series series = (Series)e.Item.Tag;
                if (series != null)
                    series.Enabled = e.Item.Checked;
            }
        }

        private void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProcList();
            UpdateParmList();

            if (this.tabs.SelectedTab == tabData)
                dataRefreshBtn_Click(sender, e);
        }

        /// <summary>
        /// Column header click fires sort.
        /// </summary>
        private void ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListView listView = sender as ListView;
            ListViewColumnSorter sorter = listView.ListViewItemSorter as ListViewColumnSorter;
            if (sorter != null)
            {
                // Determine if clicked column is already the column that is being sorted.
                if (e.Column == sorter.SortColumn1)
                {
                    // Reverse the current sort direction for this column.
                    if (sorter.Order1 == SortOrder.Ascending)
                        sorter.Order1 = SortOrder.Descending;
                    else
                        sorter.Order1 = SortOrder.Ascending;
                }
                else
                {
                    // Set the column number that is to be sorted; default to ascending.
                    sorter.SortColumn1 = e.Column;
                    sorter.Order1 = SortOrder.Ascending;
                }

                // Single column sort, set both columns to the same value.
                sorter.SortColumn2 = sorter.SortColumn1;

                // Clear old arrows and set new arrow
                foreach (ColumnHeader colHdr in listView.Columns)
                    colHdr.ImageIndex = -1;

                listView.Columns[e.Column].ImageIndex = (sorter.Order1 == SortOrder.Ascending) ? 0 : 1;

                // Assume right justified are numeric and left are alpha
                sorter.SortType = listView.Columns[e.Column].TextAlign == HorizontalAlignment.Right ?
                    ListViewColumnSorter.SortDataType.eNumeric :
                    ListViewColumnSorter.SortDataType.eAlpha;

                // Perform the sort with these new sort options.
                if (listView != null)
                    listView.Sort();
            }
        }
        #endregion

        #region ==== Save screen as Image
        /// <summary>
        /// Capture and return  screen image of this application.
        /// </summary>
        /// <returns></returns>
        private Image ScreenCaptureSelf()
        {
            // Screen.PrimaryScreen.Bounds;
            Rectangle rect = this.Bounds;

            int color = Screen.PrimaryScreen.BitsPerPixel;
            PixelFormat pFormat;

            switch (color)
            {
                case 8:
                case 16:
                    pFormat = PixelFormat.Format16bppRgb565;
                    break;

                case 24:
                    pFormat = PixelFormat.Format24bppRgb;
                    break;

                case 32:
                    pFormat = PixelFormat.Format32bppArgb;
                    break;

                default:
                    pFormat = PixelFormat.Format32bppArgb;
                    break;
            }

            Application.DoEvents();
            Image image = new Bitmap(rect.Width, rect.Height, pFormat);
            Graphics g = Graphics.FromImage(image);
            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, rect.Size);

            return image;
        }

        public void SaveAsImage()
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ScreenCaptureSelf().Save(saveFileDialog.FileName);
                    this.status.Text = "Saved Image:" + saveFileDialog.FileName;
                }
                catch (Exception ee)
                {
                    this.status.Text = "Failed to save:" + saveFileDialog.FileName + ", Error:" + ee.Message;
                }
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            SaveAsImage();
        }
        #endregion

        #region ==== Print screen capture

        public void PrintGraph()
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = this.title;
            printDoc.DefaultPageSettings.Landscape = true;
            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);

            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDoc;
            printDialog.UseEXDialog = true;
            printDialog.AllowSomePages = true;
            printDialog.AllowCurrentPage = true;
            printDialog.AllowPrintToFile = true;
            printDialog.ShowHelp = true;
            printDialog.ShowNetwork = true;
            printDialog.AllowSelection = false;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }

        void PrintPage(object o, PrintPageEventArgs e)
        {
            Image image = ScreenCaptureSelf();
            Point p = new Point(10, 10);
            e.Graphics.DrawImage(image, p);     
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            PrintGraph();
        }

        #endregion

        #region ==== Legend Editor
        int ToInt(string str)
        {
            int n = 0;
            try
            {
                n = int.Parse(str);
            }
            catch { }
            return n;
        }

        double ToDouble(string str)
        {
            double d = 0;
            try
            {
                d = double.Parse(str);
            }
            catch { }
            return d;
        }

        void scale_changed(object sender, EventArgs e)
        {
            NumBox.NumEvent numEvent = (NumBox.NumEvent)e;
            Series series = (Series)numEvent.tag;
            SeriesAux aux = (SeriesAux)series.Tag;
            aux.scale = (numEvent.num == 0) ? 1.0 : numEvent.num;
            series.Tag = aux;
            UpdateAll();
        }
        void offset_changed(object sender, EventArgs e)
        {
            NumBox.NumEvent numEvent = (NumBox.NumEvent)e;
            Series series = (Series)numEvent.tag;
            SeriesAux aux = (SeriesAux)series.Tag;
            aux.offset = (int)numEvent.num;
            series.Tag = aux;
            UpdateAll();
        }

        private string ToString(AxisType axisType)
        {
            return (axisType == AxisType.Primary) ? "Left" : "Right";
        }
        private AxisType ToAxisType(string axisStr)
        {
            return (axisStr == ToString(AxisType.Primary)) ? AxisType.Primary : AxisType.Secondary;
        }

        private void legendView_MouseUp(object sender, MouseEventArgs e)
        {
            Point p = e.Location;
            Control c = sender as Control;
            ListViewHitTestInfo hitInfo = this.legendView.HitTest(p);
            ListViewItem.ListViewSubItem subItem = hitInfo.SubItem;
            int subCol = subItem == null ? -1 : hitInfo.Item.SubItems.IndexOf(subItem);

            if (subCol == (int)LegCol.width)
            {
                NumBox fieldBox = new NumBox();
                fieldBox.FieldText = subItem.Text;
                fieldBox.Location = c.PointToScreen(subItem.Bounds.Location);
                fieldBox.Show();
                fieldBox.Width = subItem.Bounds.Width;  // reset width
                fieldBox.Visible = false;
                Series series = (Series)hitInfo.Item.Tag;
                fieldBox.Tag = series;

                if (fieldBox.ShowDialog() == DialogResult.OK)
                {
                    SeriesAux aux = (SeriesAux)series.Tag;
                    subItem.Text = fieldBox.FieldText;
                    int width = ToInt(subItem.Text);
                    if (width > 0 && width < 10)
                        series.BorderWidth = width;
                }
            }
            else if (subCol == (int)LegCol.offset)
            {
                NumBox fieldBox = new NumBox();
                fieldBox.FieldText = subItem.Text;
                fieldBox.Location = c.PointToScreen(subItem.Bounds.Location);
                fieldBox.Show();
                fieldBox.Width = subItem.Bounds.Width;  // reset width
                fieldBox.Visible = false;
                Series series = (Series)hitInfo.Item.Tag;
                fieldBox.Tag = series;
                fieldBox.changed += new EventHandler(offset_changed);

                if (fieldBox.ShowDialog() == DialogResult.OK)
                {
                    SeriesAux aux = (SeriesAux)series.Tag;
                    subItem.Text = fieldBox.FieldText;
                    int offset = ToInt(subItem.Text);
                    if (offset != aux.offset)
                    {
                        aux.offset = offset;
                        series.Tag = aux;
                        UpdateSeries();
                    }
                }
            }
            else if (subCol == (int)LegCol.scale)
            {
                NumBox fieldBox = new NumBox();
                fieldBox.FieldText = subItem.Text;
                fieldBox.Location = c.PointToScreen(subItem.Bounds.Location);
                fieldBox.Show();
                fieldBox.Width = subItem.Bounds.Width;  // reset width
                fieldBox.Visible = false;
                Series series = (Series)hitInfo.Item.Tag;
                fieldBox.Tag = series;
                fieldBox.changed += new EventHandler(scale_changed);

                if (fieldBox.ShowDialog() == DialogResult.OK)
                {
                    SeriesAux aux = (SeriesAux)series.Tag;
                    subItem.Text = fieldBox.FieldText;
                    double scale = ToDouble(subItem.Text);
                    if (scale != aux.scale && scale != 0)
                    {
                        aux.scale = scale;
                        series.Tag = aux;
                        UpdateSeries();
                    }
                }
            }
            else if (subCol == (int)LegCol.yaxis)
            {
                ComboField comboField = new ComboField();
                comboField.ComboText = subItem.Text;
                // comboField.SetChoices(Enum.GetNames(typeof(AxisType)));
                comboField.SetChoices(new string[] { ToString(AxisType.Primary), ToString(AxisType.Secondary) });
                comboField.Location = c.PointToScreen(subItem.Bounds.Location);
                comboField.Show();
                comboField.Width = subItem.Bounds.Width;   // reset width
                comboField.Visible = false;

                if (comboField.ShowDialog() == DialogResult.OK)
                {
                    foreach (ListViewItem item in legendView.SelectedItems)
                    {
                        Series series = (Series)item.Tag;
                        try
                        {
                            series.YAxisType = ToAxisType(comboField.ComboText);
                            item.SubItems[(int)LegCol.yaxis].Text = ToString(series.YAxisType);
                        }
                        catch { };
                    }
                }
            }
            else if (subCol == (int)LegCol.color)
            {
                colorDialog.Color = subItem.BackColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    subItem.BackColor = colorDialog.Color;
                    Series series = (Series)hitInfo.Item.Tag;
                    series.Color = colorDialog.Color;
                }
            }
            else if (subCol == (int)LegCol.chartType)
            {
                // string[] chartTypeChoices = Enum.GetNames(typeof(SeriesChartType));
                // string[] subSafeChoices = new string[] { "Point", "Line", "FastLine", "Spline", "Column", "Area", "SplineArea" };
                string[] subSafeChoices = new string[] { "Point", "Line", "Spline", "Column", "Area" };
                ComboField comboField = new ComboField();
                comboField.ComboText = subItem.Text;
                comboField.SetChoices(subSafeChoices);
                comboField.Location = c.PointToScreen(subItem.Bounds.Location);
                comboField.Width = subItem.Bounds.Width;

                if (comboField.ShowDialog() == DialogResult.OK)
                {
                    chart.ApplyPaletteColors();

                    foreach (ListViewItem item in legendView.SelectedItems)
                    {
                        Series series = (Series)item.Tag;
                        try
                        {
                            series.ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), comboField.ComboText);
                            item.SubItems[(int)LegCol.chartType].Text = comboField.ComboText;

                            series.Color = Color.FromArgb(
                                comboField.ComboText.Contains("Area") ? 128 : 255, series.Color);

                        }
                        catch { };
                    }
                }
            }
        }

        private void UpdateSeriesAux(Series series, double newScale, int newOffset)
        {
            SeriesAux aux = (SeriesAux)series.Tag;

            foreach (DataPoint dp in series.Points)
            {
                // adjV = rawV * scale + offset
                // rawV = (adjV - offset) / scale

                dp.YValues[0] = (dp.YValues[0] - aux.offset) / aux.scale * newScale + newOffset;
            }

            aux.offset = newOffset;
            aux.scale  = newScale;
            series.Tag = aux;
        }

#if false
        Color[] m_colors = null;
        private Color ColorOf(uint colorIdx, uint colorCnt)
        {
            if (m_colors == null || m_colors.Length != colorCnt)
            {
                int componentParts = (int)Math.Ceiling(Math.Pow(colorCnt, 1.0 / 3));
                int colorCubeCnt = componentParts * componentParts * componentParts;
                Random randomColor = new Random();
                m_colors = new Color[colorCnt];
                for (int cIdx = 0; cIdx != colorCnt; cIdx++)
                {
                    int cubeIdx = cIdx; //  randomColor.Next(colorCubeCnt);
                    int x = cubeIdx / (componentParts * componentParts);
                    int y = cubeIdx % (componentParts * componentParts) / componentParts;
                    int z = cubeIdx % (componentParts * componentParts) % componentParts;

                    int colorScale = 255 / (componentParts - 1);
                    m_colors[cIdx] = Color.FromArgb(255, x * colorScale, y * colorScale, z * colorScale);
                }
            }

            return m_colors[colorIdx];
        }

        uint m_colorIdx = 0;
        private Color NextColor()
        {
            return ColorOf(m_colorIdx++, 16);
        }
#endif

        uint nextNameIdx = 1;
        private string NextLegendName(string name)
        {
            // Increment items name.
            return string.Format("{0}:{1}", nextNameIdx++, name);
        }

        private void legendPopup_Click(object sender, EventArgs e)
        {
            string text = (sender is ToolStripItem) ?
               ((ToolStripItem)sender).Text :
               ((ToolStripDropDownItem)sender).Text;

            switch (text)
            {
                case "Split":
                    if (legendView.SelectedItems.Count != 0)
                    {
                        ListViewItem lvItem = legendView.SelectedItems[0];
                        Series series = (Series)lvItem.Tag;
                        ListViewItem lvItemNew = (ListViewItem)lvItem.Clone();
                        series.Name = lvItem.Text = NextLegendName(lvItem.Text);
                        // lvItem.SubItems[(int)LegCol.color].BackColor = NextColor();

                        legendView.Items.Insert(lvItem.Index+1, lvItemNew);
                        UpdateSeries(lvItemNew);    // This will add a new series to the graph.
                        Series seriesNew = (Series)lvItemNew.Tag;
                        seriesNew.ChartType = series.ChartType;
                        seriesNew.BorderWidth = series.BorderWidth;
                         
                        // UpdateLegend(series, legendView.BackColor, true);
                    }
                    break;

                case "Select All":
                    foreach (ListViewItem item in legendView.Items)
                        item.Selected = true;
                    break;

                case "Clear All":
                    foreach (ListViewItem item in legendView.Items)
                    {
                        Series series = (Series)item.Tag;
                        series.Points.Clear();
                        UpdateLegend(series, legendView.BackColor, true);
                    }
                    break;

                case "Clear Selected":
                    foreach (ListViewItem item in legendView.SelectedItems)
                    {
                        Series series = (Series)item.Tag;
                        series.Points.Clear();
                        UpdateLegend(series, legendView.BackColor, true);
                    } break;

                case "Delete Selected":
                    foreach (ListViewItem item in legendView.SelectedItems)
                    {
                        Series series = (Series)item.Tag;
                        chart.Series.Remove(series);
                        legendView.Items.Remove(item);
                    }
                    break;

                case "Set all offsets to minimum":
                    SetSeriesOffset();
                    break;

                case "Normalize all":
                    NormalizeSeries();
                    break;

                case "Reset All offsets and scales":
                    ResetSeries();
                    break;

            }
        }

        private void legendView_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    foreach (ListViewItem item in legendView.SelectedItems)
                    {
                        Series series = (Series)item.Tag;
                        chart.Series.Remove(series);
                        legendView.Items.Remove(item);
                    }
                    break;

                case Keys.A:
                    if (e.Control)
                    {
                        foreach (ListViewItem item in legendView.Items)
                            item.Selected = true;
                    }
                    break;

                case Keys.C:
                    foreach (ListViewItem item in legendView.SelectedItems)
                    {
                        Series series = (Series)item.Tag;
                        series.Points.Clear();
                        UpdateLegend(series, legendView.BackColor, true);
                    }
                    break;

                case Keys.M:    // set offset to negative min point series
                    SetSeriesOffset();
                    break;

                case Keys.N:    // normalize to Y range of 0..100
                    NormalizeSeries();
                    break;


                case Keys.R:    // Reset offset and scale
                    ResetSeries();
                    break;

                case Keys.Y:
                    foreach (ListViewItem item in legendView.SelectedItems)
                    {
                        Series series = (Series)item.Tag;
                        SeriesAux aux = (SeriesAux)series.Tag;
                        aux.scale *= (e.Shift) ? 1.2 : 0.8;
                        series.Tag = aux;
                        item.SubItems[(int)LegCol.scale].Text = aux.scale.ToString("F2");
                        UpdateSeries();
                    }
                    break;

                default:
                    break;
            }
        }

        public void SetSeriesOffset()
        {
            foreach (ListViewItem item in legendView.Items)
            {
                Series series = (Series)item.Tag;
                SeriesAux aux = (SeriesAux)series.Tag;

                if (series.Points.Count != 0)
                {
                    double min = series.Points.FindMinByValue().YValues[0];
                    // double max = series.Points.FindMaxByValue().YValues[0];

                    min = (min - aux.offset);
                    int newOffset = (int)-min;
                    UpdateSeriesAux(series, aux.scale, newOffset);
                }
            }
            // UpdateSeries();
            resetY0axisBtn_Click(null, EventArgs.Empty);
            status.Text = "Set series offset to minimum";
        }

        public void NormalizeSeries()
        {
#if false
            // Only normalize if more than 1 series using axis.
            int [] axisCnt = new int[2] {0, 0};

            foreach (ListViewItem item in legendView.Items)
            {
                Series series = (Series)item.Tag;
                axisCnt[(int)series.YAxisType]++;
            }
#endif
            foreach (ListViewItem item in legendView.Items)
            {
                Series series = (Series)item.Tag;
                SeriesAux aux = (SeriesAux)series.Tag;

                if (series.Points.Count != 0 /* && axisCnt[(int)series.YAxisType] > 1 */)
                {
                    double min = series.Points.FindMinByValue().YValues[0];
                    double max = series.Points.FindMaxByValue().YValues[0];
                    if (aux.scale == 0)
                        aux.scale = 1.0;

                    min = (min - aux.offset) / aux.scale;
                    max = (max - aux.offset) / aux.scale;
                    double span = max - min;
                    if (span > 0)
                    {
                        double newScale = 100 / span;
                        int newOffset = (int)(-min * newScale);
                        UpdateSeriesAux(series, newScale, newOffset);
                    }
                    else
                    {
                        UpdateSeriesAux(series, 1.0, 0);
                    }
                }
                else
                {
                    UpdateSeriesAux(series, 1.0, 0);
                }
            }

            // UpdateSeries();
            resetY0axisBtn_Click(null, EventArgs.Empty);
            status.Text = "Normalize series";
        }

        public void ResetSeries()
        {
            double maxY1 = 0;
            double maxY2 = 0;
            foreach (ListViewItem item in legendView.Items)
            {
                Series series = (Series)item.Tag;
                SeriesAux aux = (SeriesAux)series.Tag;
                if (series.YAxisType == AxisType.Primary)
                    maxY1 = Math.Max(maxY1, series.Points.FindMaxByValue().YValues[0]);
                else
                    maxY2 = Math.Max(maxY2, series.Points.FindMaxByValue().YValues[0]);
                aux.scale = 1;
                aux.offset = 0;
                series.Tag = aux;
            }

            chart.ChartAreas[0].AxisY.Maximum = maxY1;
            chart.ChartAreas[0].AxisY2.Maximum = maxY2;

            UpdateAll();
        }

        #endregion

        #region ==== System Graph UI callbacks
        private void numSysTimer_ValueChanged(object sender, EventArgs e)
        {
            sysTimer.Interval = (int)numSysTimer.Value * 1000;
        }

        private void sysTimer_Tick(object sender, EventArgs e)
        {
            this.labelRight.Text = DateTime.Now.ToString("G");
            if (pause == false)
            {
                UpdateSysGraph();
                UpdateLegend(LastPointIndex);
                UpdateAxis();
            }
        }

        ProcessEx.SystemInterruptInformation prevInterruptInformation = ProcessEx.GetInterrupt();

        PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time",  "_Total");
        PerformanceCounter memCounter = new PerformanceCounter("Memory", "Available MBytes");


        public void UpdateSysGraph()
        {
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpStatistics tcpStat = properties.GetTcpIPv4Statistics();
            UdpStatistics udpStat = properties.GetUdpIPv4Statistics();
            int perfDriveNum = 0;
            FileSysInfo.DISK_PERFORMANCE diskPerformance = new FileSysInfo.DISK_PERFORMANCE();
            ProcessEx.SystemInterruptInformation  interruptInformation = ProcessEx.GetInterrupt();

            double xValue = DateTime.Now.ToOADate();

            foreach (ListViewItem ckBox in sysList.CheckedItems)
            {
                Series series = chart.Series.FindByName(ckBox.Text);
                if (series == null)
                {
                    series = chart.Series.Add(ckBox.Text);
                    // chart.ApplyPaletteColors();
                    series.Tag = new SeriesAux { offset = 0, scale = 1.0 };
                    series.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
                }
                SeriesAux aux = (SeriesAux)series.Tag;

                string[] fields = ckBox.Text.Split(' ');

                DriveInfo[] diList = DriveInfo.GetDrives();
                int driveNum = 0;
                foreach (DriveInfo di in diList)
                {
                    if (di.DriveType == DriveType.Fixed && di.IsReady)
                    {
                        if (fields[0] == di.Name)
                        {
                            if (perfDriveNum != driveNum)
                            {
                                perfDriveNum = driveNum;
                                try
                                {
                                    diskPerformance = FileSysInfo.GetDiskPerformance(di.Name);
                                    // diskPerformance = FileSysInfo.GetDiskPerformance(perfDriveNum);
                                }
                                catch (Exception ee)
                                {
                                    diskPerformance = new FileSysInfo.DISK_PERFORMANCE();
                                    status.Text = ee.Message;
                                }
                            }
                            break;
                        }
                        driveNum++;
                    }
                }

                double yValue = 1.0;
                switch ((SysGraph)ckBox.Tag)
                {
                    case SysGraph.ProcessLoadPercent:
                        yValue = Math.Max(0.0f, cpuCounter.NextValue());
                        break;
                    case SysGraph.MegMemoryAvailable:
                        yValue = Math.Max(0.0f, memCounter.NextValue());
                        break;

                    case SysGraph.DiskQueueDepth:
                        yValue = diskPerformance.QueueDepth;
                        break;
                    case SysGraph.DiskNumReads:
                        yValue = diskPerformance.ReadCount;
                        break;
                    case SysGraph.DiskNumWrites:
                        yValue = diskPerformance.WriteCount;
                        break;
                    case SysGraph.DiskBytesRead:
                        yValue = diskPerformance.BytesRead;
                        break;
                    case SysGraph.DiskBytesWritten:
                        yValue = diskPerformance.BytesWritten;
                        break;

                    case SysGraph.TcpSegReceived:
                        yValue = tcpStat.SegmentsReceived;
                        break;
                    case SysGraph.TcpSegSent:
                        yValue = tcpStat.SegmentsSent;
                        break;
                    case SysGraph.TcpSeqResent:
                        yValue = tcpStat.SegmentsResent;
                        break;
                        
                    case SysGraph.UdpDgramReceived:
                        yValue = udpStat.DatagramsReceived;
                        break;
                    case SysGraph.UdpDgramSent:
                        yValue = udpStat.DatagramsSent;
                        break;
                    case SysGraph.UdpDgramDiscarded:
                        yValue = udpStat.IncomingDatagramsDiscarded;
                        break;


                    case SysGraph.ContextSwitches:
                        yValue = (interruptInformation.ContextSwitches -
                            prevInterruptInformation.ContextSwitches) & 0xffff;
                        break;
                    case SysGraph.DpcCount:
                        yValue = interruptInformation.DpcCount & 0xffff;
                        break;
                }

                if (series.Points.Count == 0 && yValue > 100 && aux.offset == 0)
                {
                    // Automatically set base line to first value.
                    aux.offset = (int)-yValue + 1;
                    series.Tag = aux;
                }

                yValue = yValue * aux.scale + aux.offset;

                // Force X axis to include new value.
                if (chart.ChartAreas[0].AxisX.Minimum > xValue)
                    chart.ChartAreas[0].AxisX.Minimum = xValue;
                if (chart.ChartAreas[0].AxisX.Maximum < xValue)
                    chart.ChartAreas[0].AxisX.Maximum = xValue; 

                AddXY(series, xValue, yValue);
                if (series.Points.Count == 1)
                {
                    InitSeries(series, (ulong)yValue);
                }

                UpdateYAxis(series, yValue);
                UpdateLegend(series, systemColor, false);
            }

            prevInterruptInformation = interruptInformation;
        }


        private void legendView_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in legendView.Items)
            {
                Series series = (Series)item.Tag;
                series.ShadowOffset = item.Selected ? series.BorderWidth + 1 : 0;
            }

            // Load data view if (visible)
            if (legendView.SelectedItems.Count != 0 && this.tabs.SelectedTab == tabData)
            {
                Series series = (Series)legendView.SelectedItems[0].Tag;
                dataSeriesCB.Text = series.Name;
                LoadDataView(series);
            }
        }

        public void LoadDataView(Series series)
        {
            dataSeriesCB.Text = series.Name;

            ListView.SelectedIndexCollection selected = this.dataView.SelectedIndices;
            int[] selectIdx = new int [selected.Count];
            selected.CopyTo(selectIdx, 0);

            this.dataView.BeginUpdate();
            this.dataView.Items.Clear();
            foreach (DataPoint dp in series.Points)
            {
                DateTime dt = DateTime.FromOADate(dp.XValue);
                ListViewItem lvItem = this.dataView.Items.Add(dt.ToString("G"));
                lvItem.Tag = dt;
                lvItem.SubItems.Add(dp.YValues[0].ToString("#,##0"));
            }

            foreach (int idx in selectIdx)
            {
                if (idx < dataView.Items.Count)
                    this.dataView.Items[idx].Selected = true;
            }
            this.dataView.EndUpdate();
        }

        public void UpdateDataView(int pointIdx, bool select, Series series)
        {
            if (pointIdx >= this.dataView.Items.Count ||
                  dataSeriesCB.Text != series.Name)
                LoadDataView(series);

           // if (!select)
           //     this.dataView.SelectedItems.Clear();
            if (select)
                System.Diagnostics.Debug.WriteLine("Selected=" + pointIdx);
           
            if (pointIdx < this.dataView.Items.Count)
            {
                this.dataView.Items[pointIdx].EnsureVisible();
                this.dataView.Items[pointIdx].Selected = select;
            }
        }

        private void dataRefreshBtn_Click(object sender, EventArgs e)
        {
            foreach (Series series in chart.Series)
            {
                if (series.Name == dataSeriesCB.Text)
                {
                    LoadDataView(series);
                    break;
                }
            }
        }

        private void dataView_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataNumTx.Text = dataView.SelectedItems.Count.ToString("#,##0");
            double sum = 0;
            int cnt = 0;
            DateTime begDt = DateTime.MinValue;
            DateTime endDt = DateTime.MinValue;
            foreach (ListViewItem lvItem in dataView.SelectedItems)
            {
                double num;
                DateTime dt;
                if (DateTime.TryParse(lvItem.SubItems[0].Text, out dt) && 
                    double.TryParse(lvItem.SubItems[1].Text, out num))
                {
                    cnt++;
                    sum += num;
                    if (begDt == DateTime.MinValue)
                        begDt = dt;
                    endDt = dt;
                }
            }

            dataNumTx.Text = cnt.ToString("#,##0");
            dataAvgTx.Text = ((cnt != 0) ? sum / cnt : 0.0).ToString();

            TimeSpan span = endDt - begDt;
            if (span.TotalMinutes < 0)
                span = -span;
            System.Diagnostics.Debug.WriteLine("SpanMin=" +  span.TotalMinutes);

            if (span.TotalDays >= 1)
            {
                slopeLbl.Text = "/Day";
                spanLbl.Text = "Days:";
                dataSlopeTx.Text = (sum / span.TotalDays).ToString("#,##0");
                dataSpanTx.Text = span.TotalDays.ToString("#,##0");
            }
            else if (span.TotalHours >= 1)
            {
                slopeLbl.Text = "/Hr";
                spanLbl.Text = "Hrs:";
                dataSlopeTx.Text = (sum / span.TotalHours).ToString("#,##0");
                dataSpanTx.Text = span.TotalHours.ToString("#,##0");
            }
            else if (span.TotalMinutes >= 1)
            {
                slopeLbl.Text = "/Min";
                spanLbl.Text = "Mins:";
                dataSlopeTx.Text = (sum / span.TotalMinutes).ToString("#,##0");
                dataSpanTx.Text = span.TotalMinutes.ToString("#,##0");
            }
            else
            {
                slopeLbl.Text = "/Sec";
                spanLbl.Text = "Secs:";
                dataSlopeTx.Text = (span.TotalSeconds > 0) ?
                    (sum / span.TotalSeconds).ToString("#,##0") : string.Empty;
                dataSpanTx.Text = span.TotalSeconds.ToString("#,##0");
            }
        }

        private void dataView_MouseEnter(object sender, EventArgs e)
        {
            dataView.Focus();
        }

        private void deleteSelectedToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dataView.SelectedIndices.Count == 0)
                return;

            Series dataSeries = chart.Series.FindByName(dataSeriesCB.Text);

            if (dataSeries != null)
            {
                dataView.BeginUpdate();
                int ptIdx = 0;
                foreach (ListViewItem lvItem in dataView.SelectedItems)
                {
                    DateTime dt = (DateTime)lvItem.Tag;
                    double adate = dt.ToOADate();

                    while (ptIdx < dataSeries.Points.Count && dataSeries.Points[ptIdx].XValue < adate)
                        ptIdx++;

                    if (ptIdx < dataSeries.Points.Count && dataSeries.Points[ptIdx].XValue == adate)
                        dataSeries.Points.RemoveAt(ptIdx);

                    dataView.Items.Remove(lvItem);
                }
                dataView.EndUpdate();
            }
        }

        private void clearAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dataView.Items.Clear();
            Series dataSeries = chart.Series.FindByName(dataSeriesCB.Text);
            if (dataSeries != null)
                dataSeries.Points.Clear();
        }

   
        private void timerCkBox_CheckedChanged(object sender, EventArgs e)
        {
            this.sysTimer.Enabled = timerCkBox.Checked;
            if (this.sysTimer.Enabled)
                this.sysTimer.Start();
        }

        private void resetY0axisBtn_Click(object sender, EventArgs e)
        {
            double minY1 = 1e99;
            double maxY1 = -1e99;
            double minY2 = 1e99;
            double maxY2 = -1e99;

            foreach (Series series in chart.Series)
            {
                if (series.Enabled)
                {
                    double minY = series.Points.FindMinByValue().YValues[0];
                    double maxY = series.Points.FindMaxByValue().YValues[0];

                    if (series.YAxisType == AxisType.Primary)
                    {
                        minY1 = Math.Min(minY1, minY);
                        maxY1 = Math.Max(maxY1, maxY);
                    }
                    else
                    {
                        minY2 = Math.Min(minY2, minY);
                        maxY2 = Math.Max(maxY2, maxY);
                    }
                }
            }

            try
            {
                if (maxY1 - minY1 > 0)
                {
                    minY1 *= 0.99;   // spread a little to add a gap
                    maxY1 *= 1.01;
                    chart.ChartAreas[0].AxisY.Maximum = 1e99;   // prevent crossover exception.
                    chart.ChartAreas[0].AxisY.Minimum = Math.Floor(minY1);
                    chart.ChartAreas[0].AxisY.Maximum = maxY1;
                    this.minY1axis.Text = minY1.ToString("#,##0");
                    this.maxY1axis.Text = maxY1.ToString("#,##0");
                }

                if (maxY2 - minY2 > 0)
                {
                    minY2 *= 0.99;
                    maxY2 *= 1.01;
                    chart.ChartAreas[0].AxisY2.Maximum = 1e99;
                    chart.ChartAreas[0].AxisY2.Minimum = Math.Floor(minY2);
                    chart.ChartAreas[0].AxisY2.Maximum = maxY2;

                    this.minY2axis.Text = minY2.ToString("#,##0");
                    this.maxY2axis.Text = maxY2.ToString("#,##0");
                }
                chart.Update();
            }
            catch { }

        }

        private void resetXaxisBtn_Click(object sender, EventArgs e)
        {
            double now = DateTime.Now.ToOADate();
            double minX = 1e99;
            double maxX = -1e99;

            foreach (Series series in chart.Series)
            {
                int cnt = series.Enabled ? series.Points.Count : 0;
                if (cnt != 0)
                {
                    minX = Math.Min(minX, series.Points[0].XValue);
                    maxX = Math.Max(maxX, series.Points[cnt-1].XValue);
                }
            }

            minX = Math.Min(minX, now);
            // maxX = Math.Max(maxX, now);
            maxX = now;
            this.chart.ChartAreas[0].AxisX.Minimum = minX;
            this.chart.ChartAreas[0].AxisX.Maximum = maxX;
        }

        #endregion

        private void helpBtn_Click(object sender, EventArgs e)
        {
            Assembly a = Assembly.GetExecutingAssembly();
            Stream hlpStream = a.GetManifestResourceStream("MagniFile.HelpGraph.html");

            if (hlpStream != null)
            {
                ViewHelp helpDialog = new ViewHelp(hlpStream, logo, this.title);
                helpDialog.Show(this);
            }
        }

        private void deleteSelectedItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem procItem in procList.SelectedItems)
            {
                // delete graph series and legends associated with process.
                string pname = procItem.Text;

                foreach (ListViewItem parameterItem in parmList.CheckedItems)
                {
                    string parm = parameterItem.Text;

                    string seriesName = pname + " " + parm;
                    Series series = chart.Series.FindByName(seriesName);
                    if (series != null)
                        chart.Series.Remove(series);

                    foreach (ListViewItem legendItem in legendView.Items)
                    {
                        // if (series == (Series)legentItem.Tag)
                        if (seriesName == legendItem.Text)
                        {
                            legendView.Items.Remove(legendItem);
                        }
                    }
                }

                procList.Items.Remove(procItem);
            }
        }

     
    }
}
