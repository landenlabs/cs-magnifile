using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
// using System.Net;
using Microsoft.Win32;
using System.Media;
using System.Reflection;
using System.Security.Principal;
using System.Diagnostics;

namespace MagniFile
{
    /// <summary>
    /// Display and log information about files.
    /// 
    /// Author: Dennis Lang 2009
    /// https://landenlabs.com/
    /// 
    /// </summary>
    public partial class MainForm : Form
    {
        #region ==== Data members
        // Main view objects
        ListViewProcesses ListViewProcesses = new ListViewProcesses();
        ListViewHandles ListViewHandles = new ListViewHandles();
        PrintListView printListView = new PrintListView();
        const int maxSumCnt = 200;

        // Logging stuff
        string procLogFilename = "MagniFileProc.csv";
        TextWriter procLogWriter;
        UInt32 procLogLine = 0;

        string hndLogFilename = "MagniFileHnd.csv";
        TextWriter hndLogWriter;
        UInt32 hndLogLine = 0;

        // Process and Handle filters.
        FilterDialog filterDialog = new FilterDialog();
        FilterList procFilter = new FilterList();
        FilterList hndFilter = new FilterList();

        // Special helper objects
        ListViewHandles.ViewHndGroup viewHndGroup = ListViewHandles.ViewHndGroup.eNoGroup;
        string logOnStr = "Log";
        Color logColor = Color.LightYellow;
        const int procLogCol = ListViewProcesses.pLogCol;
        // List of pid's marked for logging
        List<int> procPidLogList = new List<int>();
        // List of pid names to restore logging if process restarts
        Dictionary<string, int> procNamePidLogList = new Dictionary<string, int>();
        // List of handle object types to show (all, directory, ... file, ...)
        Dictionary<int, bool> hndShowObjDict = new Dictionary<int, bool>();
      
        System.Media.SoundPlayer sPlayer = new System.Media.SoundPlayer();
        #endregion

        public MainForm(string[] args)
        {
			if (!IsRunningAsAdmin()) {
				if (RestartAsAdmin())
					return; // exit current non-admin instance
			}

			// Your privileged code here
			Console.WriteLine("Running with admin rights!");

			InitializeComponent();

            #region ==== Custom initialization 

            this.Text = ProductNameAndVersion();
            this.title.Text = ProductNameAndVersion() + " DLang 2012";

            this.hndNoGrpBtn.Tag = ListViewHandles.ViewHndGroup.eNoGroup;
            this.hndPidGrpBtn.Tag = ListViewHandles.ViewHndGroup.ePidGroup;
            this.hndObjGrpBtn.Tag = ListViewHandles.ViewHndGroup.eObjectGroup;

            // Populate View Handle object list
            AddHndShowButtons();

            // Add handle type columns to summary view 
            int maxObjNum = OpenHandles.MaxObjTypeNumber;
            for (int i = 1; i < maxObjNum; i++)
            {
                string objStr = OpenHandles.ObjectTypeDescription(i);
                int width = objStr.StartsWith("type") ? 0 : 60;
                hndSumView.Columns.Add(objStr, width, HorizontalAlignment.Right);
            }

            // Adjust handle summary column widths to match active list.
            UpdateHndSumColumns(hndShowObjDict);

            ListViewProcesses.UpdateViewEx(this.procView);
            ListViewProcesses.UpdateViewEx(this.procView);    // update twice to get colors correct.

            this.procClock.Text = NowTimeString();
            this.hndClock.Text = NowTimeString();

            // Default to autoUpdate on, 5 seconds
            this.procAutoUpdBtn.Checked = true;
            procUpdRate_Click(proc5secBtn, EventArgs.Empty);
            // Handle update off, but set to 1 minute
            hndUpdRate_Click(hnd1minBtn, EventArgs.Empty);
            this.hndAutoLogBtn.Checked = false;
            hndFileMenu_Click(hndAutoLogBtn, EventArgs.Empty);

            procTimer.Start();
            handleTimer.Start();

            this.procView.ListViewItemSorter = new ListViewColumnSorter(ListViewColumnSorter.SortDataType.eAuto);
            this.handleView.ListViewItemSorter = new ListViewColumnSorter(ListViewColumnSorter.SortDataType.eAuto);
            this.hndSumView.ListViewItemSorter = new ListViewColumnSorter(ListViewColumnSorter.SortDataType.eAuto);
            this.procSumView.ListViewItemSorter = new ListViewColumnSorter(ListViewColumnSorter.SortDataType.eAuto);

            // Activate correct handle view
            this.hndSummaryBtn.Checked = true;
            hndSummaryBtn_Click(null, EventArgs.Empty); 

            // Activate correct process view
            procViewBtn.Text = "";
            procViewBtn_Click(procViewBtn, EventArgs.Empty);


        

            // *** Enable special debug privilege to allow access to system handles. ***
            OpenHandles.EnableDebugPrivilege();

            #region ==== Command line parsing 
            ///
            /// Parse command line switches
            /// 
            ///     -plog <filename>       ; set process log filename
            ///     -hlog <filename>       ; set handle log filename
            ///     -prate <seconods>      ; set process update rate
            ///     -hrate <seconods>      ; set handle update rate
            ///     -proc <procName>       ; set log flag on process
            /// 
            Arguments cmdArgs = new Arguments(args);
            if (cmdArgs["hide"] != null)
                this.WindowState = FormWindowState.Minimized;

            if (cmdArgs.TryAndGet("plog", ref procLogFilename))
            {
                this.procAutoUpdBtn.Checked = this.procAutoLogBtn.Checked = true;
                procFileMenu_Click(procAutoLogBtn, EventArgs.Empty);
            }

            if (cmdArgs.TryAndGet("hlog", ref hndLogFilename))
            {
                this.hndAutoUpdBtn.Checked = this.hndAutoLogBtn.Checked = true;
                hndFileMenu_Click(hndAutoLogBtn, EventArgs.Empty);
            }

            int seconds = 0;
            if (cmdArgs.TryAndGet("prate", ref seconds))
            {
                this.procAutoUpdBtn.Checked = true;
                this.procTimer.Interval = seconds * 1000;
            }
            if (cmdArgs.TryAndGet("hrate", ref seconds))
            {
                this.hndAutoUpdBtn.Checked = true;
                this.handleTimer.Interval = seconds * 1000;
            }

            string procNm = string.Empty;
            if (cmdArgs.TryAndGet("proc", ref procNm))
            {
                foreach (ListViewItem item in this.procView.Items)
                {
                    if (item.SubItems[3].Text == procNm)
                    {
                        item.SubItems[procLogCol].Text = logOnStr;
                        int pid = (int)item.Tag;
                        procPidLogList.Add(pid);
                        procNamePidLogList[procNm] = pid;
                    }
                }
            }

            if (cmdArgs["exit"] != null)
            {
                this.Close();
                return;
            }
            #endregion

            procLogMenu.ToolTipText = "Save process list to log file:" + procLogFilename;
            hndLogMenu.ToolTipText = "Save handle list to log file:" + hndLogFilename;

            procFileMenu_Click(procAutoUpdBtn, EventArgs.Empty);
            EnableDoubleBuffer();
            #endregion 
        }

        #region ==== General UI controls 
        private void EnableDoubleBuffer()
        {
            ListViewHelper.EnableDoubleBuffer(this.procView);
            ListViewHelper.EnableDoubleBuffer(this.procSumView);
            ListViewHelper.EnableDoubleBuffer(this.handleView);
            ListViewHelper.EnableDoubleBuffer(this.hndSumView);
        }

        protected override void OnResizeBegin(EventArgs e)
        {
            this.procView.BeginUpdate();
            this.handleView.BeginUpdate();

            base.OnResizeBegin(e);
        }
        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            this.procView.EndUpdate();
            this.handleView.EndUpdate();
        }

		bool IsRunningAsAdmin() {
			var identity = WindowsIdentity.GetCurrent();
			var principal = new WindowsPrincipal(identity);
			return principal.IsInRole(WindowsBuiltInRole.Administrator);
		}

		bool RestartAsAdmin() {
			var exeName = Assembly.GetExecutingAssembly().Location;
			var startInfo = new ProcessStartInfo(exeName) {
				UseShellExecute = true,
				Verb = "runas" // triggers UAC prompt
			};

			try {
				Process.Start(startInfo);
				return true;
			} catch {
				Console.WriteLine("User declined elevation.");
				return false;
			}
		}

		private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

		protected override void OnFormClosing(FormClosingEventArgs e) 
		{
			if (this.viewNetInfo != null)
            {
                viewNetInfo.ExitOnClose = true;
                viewNetInfo.Close();
                viewNetInfo.Dispose();
            }
            if (this.viewFileEvents != null)
            {
                viewFileEvents.Close();
                viewFileEvents.Dispose();
            }
            foreach (ViewGraph viewGraph in viewGraphList)
            {
                viewGraph.Close();
                viewGraph.Dispose();
            }
            if (this.viewFileList != null)
            {
                viewFileList.Close();
                viewFileList = null;
            }
            base.OnClosing(e);
        }

        public static string ProductNameAndVersion()
        {
            string appName = Application.ProductName;
            string appVern = Application.ProductVersion;
            return appName + " v" + appVern.Substring(0, 3); //  Get part of versoin string "n.n"
        }

        string PrintTitle()
        {
            return string.Format("{0}     "
                    , this.Text
                    // , DateTime.Now.ToString("G")
                    // , Dns.GetHostName()
                    // , System.Environment.UserName
                    // , System.Environment.OSVersion.ToString()
                    );
        }

        private void About_Click(object sender, EventArgs e)
        {
            About about = new About(this);
            about.Text = "About - " + ProductNameAndVersion();
            about.Show();
        }

        /// <summary>
        /// Return Now Time string as hh:mm:ss
        /// </summary>
        private string NowTimeString()
        {
            return DateTime.Now.ToString("T", DateTimeFormatInfo.InvariantInfo); 
        }

        /// <summary>
        /// Column header click fires sort.
        /// Supports two tier sorting.
        /// </summary>
        private void ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListView listView = sender as ListView;
            ListViewColumnSorter sorter = listView.ListViewItemSorter as ListViewColumnSorter;
            if (sorter == null)
                return;

            if (listView.Groups.Count != 0)
                MessageBox.Show("Sorting does not work when in 'Group' mode");

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
                sorter.SortColumn2 = sorter.SortColumn1;
                sorter.Order2 = sorter.Order1;
                sorter.SortColumn1 = e.Column;
                sorter.Order1 = SortOrder.Ascending;
            }

            // Clear old arrows and set new arrow
            foreach (ColumnHeader colHdr in listView.Columns)
                colHdr.ImageIndex = -1;

            if (sorter.SortColumn1 != sorter.SortColumn2)
                listView.Columns[sorter.SortColumn2].ImageIndex = (sorter.Order2 == SortOrder.Ascending) ? 2 : 3;

            listView.Columns[sorter.SortColumn1].ImageIndex = (sorter.Order1 == SortOrder.Ascending) ? 0 : 1;

            // Perform the sort with these new sort options.
            if (listView != null)
                listView.Sort();
        }
        #endregion

        #region ==== Process Menu/Buttons

        /// <summary>
        /// Update Process View List drawing new entries in GREEN and dead in RED.
        /// </summary>
        private void UpdateProcessView()
        {
            procRefreshBtn.Image = global::MagniFile.Properties.Resources.green24;
            // this.sPlayer.Stream = Properties.Resources.sonar;
            // this.sPlayer.Play();
            // Application.DoEvents();     // This is dangerous to do !!!

            System.Collections.IComparer sorter = this.procView.ListViewItemSorter;
            // this.procView.ListViewItemSorter = null;
            ListViewProcesses.UpdateViewEx(this.procView);
            // this.procView.ListViewItemSorter = sorter;
            this.procView.Sort();

            // Restore 'log' state if process restarted or remove pid if dead.
            foreach (ListViewItem item in procView.Items)
            {
                if (item.BackColor == ListViewProcesses.StartColor)
                {
                    int pid = (int)item.Tag;
                    string pname = item.SubItems[ListViewProcesses.pNameCol].Text;
                    int logPid;
                    if (procNamePidLogList.TryGetValue(pname, out logPid) == true)
                    {
                        if (procPidLogList.Find(delegate(int t) { return pid == t; }) != pid)
                        {
                            procPidLogList.Add(pid);
                        }
                    }
                }
                else if (item.BackColor == ListViewProcesses.StopColor)
                {
                    int pid = (int)item.Tag;
                    procPidLogList.Remove(pid);
                }
            }

            // Restore 'log' state
            string logNamePidStr = string.Empty;
            foreach (int pid in procPidLogList)
            {
                ListViewItem item = ListViewProcesses.FindViewItem(procView.Items, pid.ToString(), ListViewProcesses.pIdCol);
                if (item  != null)
                {
                    item.SubItems[procLogCol].Text = logOnStr;
                    item.BackColor = logColor;

                    string pname = item.SubItems[ListViewProcesses.pNameCol].Text;
                    logNamePidStr += string.Format("{0}{1},", pname, pid);
                }
            }

            SetStatus(procStatus, this.procView.Items.Count.ToString() + " processes", false);
            this.procClock.Text = NowTimeString();

            procRefreshBtn.Image = procAutoUpdBtn.Checked ?
                      global::MagniFile.Properties.Resources.yellow24 :
                      global::MagniFile.Properties.Resources.red24;

            if (this.procLogWriter != null)
            {
                LogProcess();
                this.procLogWriter.Flush();
            }

            UpdateProcessSummary();

            foreach (ViewGraph viewGraph in viewGraphList)
            {
                if (viewGraph.LogNamePids != logNamePidStr)
                {
                    viewGraph.LogNamePids = logNamePidStr;
                    viewGraph.UpdateAll();
                }
            }
        }

        /// <summary>
        /// Add 'log' tagged process lines to process summary view.
        /// </summary>
        private void UpdateProcessSummary()
        {
            if (procPidLogList.Count != 0)
            {
                foreach (ListViewItem item in procView.Items)
                {
                    int pid = (int)item.Tag;
                    if (pid == 0)
                        continue;   // ignore pid 0 (idle process)

                    if (procPidLogList.Find(delegate(int t) { return pid == t; }) == pid)
                    {
                        ListViewItem sumItem = procSumView.Items.Add("");

                        int subCnt = item.SubItems.Count;
                        for (int subIdx = 1; subIdx < subCnt; subIdx++)
                        {
                            ListViewItem.ListViewSubItem sItem = sumItem.SubItems.Add(item.SubItems[subIdx].Text);
                            sItem.Tag = item.SubItems[subIdx].Tag;
                        }
                        sumItem.SubItems[1].Text = DateTime.Now.ToString("G");
                        sumItem.SubItems[1].Tag = DateTime.Now;

                        // Update graph
                        foreach (ViewGraph viewGraph in this.viewGraphList)
                            viewGraph.UpdateGraph(sumItem);
                    }
                }

                if (procSumView.Items.Count > maxSumCnt)
                {
                    // Remove every other line.
                    int jump = 2;
                    for (int sumIdx = procSumView.Items.Count - jump; sumIdx > 0; sumIdx -= jump)
                    {
                        procSumView.Items.RemoveAt(sumIdx);
                        jump++;
                    }
                }

                if (procSumView.Items.Count != 0)
                    procSumView.Items[procSumView.Items.Count - 1].EnsureVisible();
            }
        }

        /// <summary>
        /// Log all or marked proocesses to disk log file.
        /// </summary>
        private void LogProcess()
        {
            string txtLine = string.Empty;
            if (procLogLine == 0)
            {
                // Output column header ever few lines.
                txtLine += "Date, Time";
                foreach (ColumnHeader ch in procView.Columns)
                {
                    if (txtLine.Length != 0)
                        txtLine += ",";
                    txtLine += ch.Text + " ";
                }
                procLogWriter.WriteLine(txtLine);
            }

            foreach (ListViewItem item in procView.Items)
            {
                int pid = (int)item.Tag;
                if (pid == 0)
                    continue;   // ignore pid 0 (idle process)

                // if (procPidLogList.Count == 0 ||
                //    procPidLogList.Find(delegate(int t) { return pid == t; }) == pid)
                if (item.SubItems[ListViewProcesses.pLogCol].Text == logOnStr)
                {
                    txtLine = string.Empty;

                    txtLine += DateTime.Now.ToShortDateString();
                    txtLine += ", ";
                    txtLine += NowTimeString();

                    foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                    {
                        if (txtLine.Length != 0)
                            txtLine += ",";
                        if (subItem.Tag != null && subItem.Tag is ulong)
                            txtLine += ((ulong)subItem.Tag).ToString("D");  
                        // else if (subItem.Tag != null)
                        //    txtLine += subItem.Tag.ToString();  // use raw value
                        else
                            txtLine += subItem.Text.Replace(",", null) + " ";
                    }

                    procLogWriter.WriteLine(txtLine);
                    procLogLine++;
                }
            }

            SetStatus(procStatus, procLogLine.ToString() + " lines,  Log file " + procLogFilename, false);
        }

        /// <summary>
        /// Button press to force refresh
        /// </summary>
        private void procRefreshBtn_Click(object sender, EventArgs e)
        {
            UpdateProcessView();    
        }


        /// <summary>
        /// Toggle log column value
        /// </summary>
        private void procView_Click(object sender, EventArgs e)
        {
            Point p = this.procView.PointToClient(System.Windows.Forms.Control.MousePosition);

            ListViewHitTestInfo hitInfo = this.procView.HitTest(p);
            ListViewItem itemAt = hitInfo.Item;

            if (itemAt != null)
            {
                string pname = itemAt.SubItems[ListViewProcesses.pNameCol].Text;

                SetStatus(procStatus,
                    itemAt.SubItems[ListViewProcesses.pIdCol].Text
                    + " "
                    + pname, false);

                if (itemAt.SubItems[procLogCol] == hitInfo.SubItem)
                {
                    bool on = (itemAt.SubItems[procLogCol].Text == logOnStr);
                    on = !on;
                    itemAt.SubItems[procLogCol].Text = on ? logOnStr : string.Empty;

                    int pid = (int)itemAt.Tag;
                    if (on)
                    {
                        procPidLogList.Add(pid);
                        itemAt.BackColor = logColor;
                        procNamePidLogList[pname] = pid;
                    }
                    else
                    {
                        procPidLogList.Remove(pid);
                        itemAt.BackColor = this.procView.BackColor;
                        procNamePidLogList.Remove(pname);
                    }

                    this.procStatus.Text += " (" + procPidLogList.Count.ToString() + ")"
                        + itemAt.SubItems[procLogCol].Text;

                    // if (viewGraph != null)
                    //    viewGraph.UpdateAll();
                }
            }
        }

        private void procTimer_Tick(object sender, EventArgs e)
        {
            if (this.procAutoUpdBtn.Checked)
            {
                UpdateProcessView();
            }
        }

        private void procUpdRate_Click(object sender, EventArgs e)
        {
            ToolStripDropDownItem dropItem = (ToolStripDropDownItem)sender;
            ToolStripDropDownMenu dropMenu = dropItem.DropDown.OwnerItem.Owner as ToolStripDropDownMenu;

            foreach (ToolStripDropDownItem item in dropMenu.Items)
                item.Image = Properties.Resources.check_off;

            dropItem.Image = Properties.Resources.check_on;

            if ((int)dropItem.Tag > 0)
            {
                procTimer.Interval = 1000 * (int)dropItem.Tag;
                procAutoUpdBtn.Checked = true;
                procFileMenu_Click(procAutoUpdBtn, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("TODO - prompt for custom log interval");
            }
        }

        private void procView_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateHandleView();
        }
 
        private void procFileMenu_Click(object sender, EventArgs e)
        {
            string text = (sender is ToolStripItem) ?
               ((ToolStripItem)sender).Text :
               ((ToolStripDropDownItem)sender).Text;

            switch (text)
            {
                case "AutoSize Columns":
                    this.procView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    this.procView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.None);
                    break;

                case "Auto Update":
                    procRefreshBtn.Image = procAutoUpdBtn.Checked ? 
                        global::MagniFile.Properties.Resources.yellow24 :
                        global::MagniFile.Properties.Resources.red24;

                    string intervalStr = (procTimer.Interval / 1000).ToString() + " seconds";
                    procAutoUpdBtn.ToolTipText = "Auto refresh every " + intervalStr;
                    procRefreshBtn.ToolTipText = procAutoUpdBtn.Checked ?
                        "Refresh Process Display, Auto refresh every " + intervalStr :
                        "Refresh Process Display";
                    break;

                case "Filter...":
                    filterDialog.List = procFilter;
                    filterDialog.ShowDialog();
                    procFilter = filterDialog.List;
                    UpdateProcessView();
                    break;

                case "Auto Log":
                    if (procAutoLogBtn.Checked == false)
                    {
                        if (procLogWriter != null)
                            procLogWriter.Close();
                        procLogWriter = null;
                        SetStatus(procStatus, "Logging disabled", false);
                        procLogMenu.ForeColor = Color.White;
                    }
                    else
                    {
                        try
                        {
                            procLogWriter = new StreamWriter(procLogFilename, true);
                            SetStatus(procStatus, "Logging to file:" + procLogFilename, false);
                            procLogMenu.ForeColor = Color.Green;
                        }
                        catch 
                        {
                            SetStatus(procStatus, "Unable to start logging to file:" + procLogFilename, false);
                            procLogMenu.ForeColor = Color.White;
                            procAutoLogBtn.Checked = false;
                        }
                    }
                    break;

                case "Log File...":
                    this.logFileDialog.FileName = procLogFilename;
                    SetPath(this.logFileDialog, procLogFilename);
                    if (this.logFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        procLogFilename = this.logFileDialog.FileName;
                        procLogWriter = null;
                        procAutoLogBtn.Checked = true;
                        procFileMenu_Click(procAutoLogBtn, e);
                    }
                    break;

                case "Load Log File...":
                    this.logOpenDialog.FileName = procLogFilename;
                    this.logOpenDialog.InitialDirectory = Path.GetDirectoryName(procLogFilename);
                    if (this.logOpenDialog.ShowDialog() == DialogResult.OK)
                    {
                        int addCnt = ListViewExt.Import(this.procSumView, this.logOpenDialog.FileName, 
                            ListViewProcesses.pLogCol+2, logOnStr);
                        SetStatus(procStatus, "Imported " + addCnt.ToString() + " lines", false);
                    }
                    break;

                case "Log":
                    if (procLogWriter == null)
                    {
                        try
                        {
                            procLogWriter = new StreamWriter(procLogFilename, true);
                            LogProcess();
                            procLogWriter.Flush();
                            procLogWriter = null;
                        }
                        catch (Exception ee)
                        {
                            SetStatus(procStatus, ee.Message + ", Failed to log to file:" + procLogFilename, true);
                        }
                    }
                    else
                    {
                        LogProcess();
                    }
                    break;

                case "Open log file":
                    try
                    {
                        FileInfo fileInfo = new FileInfo(procLogFilename);
                        if (fileInfo.Exists)
                        {
                            System.Diagnostics.Process.Start(procLogFilename);
                            SetStatus(procStatus, procLogFilename, false);
                        }
                        else
                        {
                            SetStatus(procStatus, "No log file:" + procLogFilename, true);
                        }
                    }
                    catch
                    {
                        SetStatus(procStatus, "Failed to open log file:" + procLogFilename, true);
                    }
                    break;

                case "Export...":
                    ListViewExt.Export(this.procView.Visible ? this.procView : this.procSumView);

                    break;
                case "Print Setup":
                case "Print Setup...":
                    printListView.PageSetup();
                    printListView.FitToPage = fitToPageProcDetailMenu.Checked;
                    break;
                case "Preview":
                case "Print Preview...":
                    printListView.FitToPage = fitToPageProcDetailMenu.Checked;
                    printListView.PrintPreview(this, 
                        this.procView.Visible ? this.procView : this.procSumView, PrintTitle() + "Process List");
                    break;
                case "Print":
                case "Print...":
                    printListView.FitToPage = fitToPageProcDetailMenu.Checked;
                    printListView.Print(this, 
                        this.procView.Visible ? this.procView : this.procSumView, PrintTitle() + "Process List");
                    break;
            }
        }

        private void procViewBtn_Click(object sender, EventArgs e)
        {
            if (procViewBtn.Text != "Summary")
            {
                procLabel.Text = "Process Detail";
                procViewBtn.Text = "Summary";   // btn shows what will become active when pressed.
                procView.Visible = true;
                procSumView.Visible = false;
            }
            else
            {
                procLabel.Text = "Process Summary";
                procViewBtn.Text = "Detail";
                procView.Visible = false;
                procSumView.Visible = true;
            }
        }

#if false
        // Disable update while cursor is over process ListView
        bool procEnterUpdState = false;
        private void procView_MouseEnter(object sender, EventArgs e)
        {
            procEnterUpdState = this.procAutoUpdBtn.Checked;
            if (procEnterUpdState)
            {
                this.procAutoUpdBtn.Checked = false;
                procFileMenu_Click(procAutoLogBtn, EventArgs.Empty);
                this.procStatus.Text = "Update Disabled while cursor over Process list";
            }
        }

        private void procView_MouseLeave(object sender, EventArgs e)
        {
            if (procEnterUpdState)
            {
                this.procAutoUpdBtn.Checked = true;
                procFileMenu_Click(procAutoLogBtn, EventArgs.Empty);
                this.procStatus.Text = "Update restored";
            }
        }
#endif

        #endregion

        #region ==== Handle Menu/Buttons

        private void handleTimer_Tick(object sender, EventArgs e)
        {
            if (this.hndAutoUpdBtn.Checked)
            {
                UpdateHandleView();
            }
        }

        bool isUpdatingHandle = false;

        private void UpdateHandleView()
        {
            if (isUpdatingHandle)
            {
                ListViewHandles.Abort();
                return;
            }

            isUpdatingHandle = true;
            this.Cursor = Cursors.WaitCursor;
            hndRefreshBtn.Image = global::MagniFile.Properties.Resources.green24;
            hndRefreshBtn.Text = "Abort";

            SortedList<int, string> pidList = new SortedList<int, string>();

            foreach (ListViewItem procItem in this.procView.SelectedItems)
            {
                int pid = (int)procItem.SubItems[2].Tag;
                pidList.Add(pid, procItem.SubItems[3].Text);
            }

            if (pidList.Count != 0)
            {
                System.Collections.IComparer sorter = this.handleView.ListViewItemSorter;
                this.handleView.ListViewItemSorter = null;
                SetStatus(handleStatus, "Handle inquire in progress...", false);

                // This method will use a background thread and allow events to fire.
                ListViewHandles.UpdateView(this.handleView, this.hndSumView, this.handleStatus,
                        hndFilter, pidList, 
                        this.hndShowObjDict, this.viewHndGroup, 
                        hndHideNoDescBtn.Checked, hndShowClosedCk.Checked);

                // Keep summary list from getting too large.
                if (hndSumView.Items.Count > maxSumCnt)
                {
                    // Remove some lines, leaving more near the front since it gets repeatly pruned.
                    int jump = 2;
                    for (int sumIdx = hndSumView.Items.Count - jump; sumIdx > 0; sumIdx -= jump)
                    {
                        hndSumView.Items.RemoveAt(sumIdx);
                        jump++;
                    }
                }
                if (hndSumView.Items.Count != 0)
                    hndSumView.Items[hndSumView.Items.Count - 1].EnsureVisible();

                this.handleView.ListViewItemSorter = sorter;
                this.handleView.Sort();
                this.hndClock.Text = NowTimeString();
                this.handleLabel.Text = (pidList.Count == 1) ? pidList.First() + " Handles" : "Open Handles";
                this.Text = "(" + this.handleView.Items.Count.ToString() + ")" + ProductNameAndVersion();
            }
            else
            {
                SetStatus(handleStatus, "Please select one or more processes", false);
                this.handleLabel.Text = "Open Handles";
                this.Text = ProductNameAndVersion();
            }

            if (this.hndLogWriter != null)
            {
                LogHandle();
            }

            hndRefreshBtn.Text = "Refresh";
            hndRefreshBtn.Image = hndAutoUpdBtn.Checked ?
                     global::MagniFile.Properties.Resources.yellow24 :
                     global::MagniFile.Properties.Resources.red24;
            this.Cursor = Cursors.Default;

            isUpdatingHandle = false;
        }

        private void LogHandle()
        {
            string txtLine = string.Empty;
            if (hndLogLine == 0)
            {
                txtLine += "Date, Time";
                foreach (ColumnHeader ch in handleView.Columns)
                {
                    if (txtLine.Length != 0)
                        txtLine += ",";
                    txtLine += ch.Text + " ";
                }
                hndLogWriter.WriteLine(txtLine);
            }

            foreach (ListViewItem item in handleView.Items)
            {
                txtLine = string.Empty;
                txtLine += DateTime.Now.ToShortDateString();
                txtLine += ", ";
                txtLine += NowTimeString();

                foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                {
                    if (txtLine.Length != 0)
                        txtLine += ",";
                    txtLine += subItem.Text.Replace(',', ';');
                }

                hndLogWriter.WriteLine(txtLine);
                hndLogLine++;
            }

            SetStatus(handleStatus, hndLogLine.ToString() + " lines, Log file:" + hndLogFilename, false);
        }

        private void hndUpdRate_Click(object sender, EventArgs e)
        {
            ToolStripDropDownItem dropItem = (ToolStripDropDownItem)sender;
            ToolStripDropDownMenu dropMenu = dropItem.DropDown.OwnerItem.Owner as ToolStripDropDownMenu;

            foreach (object obj in dropMenu.Items)
            {
                if (obj is ToolStripDropDownItem)
                {
                    ToolStripDropDownItem item = (ToolStripDropDownItem)obj; 
                    item.Image = Properties.Resources.check_off;
                }
            }

            dropItem.Image = Properties.Resources.check_on;

            if ((int)dropItem.Tag > 0)
            {
                handleTimer.Interval = 1000 * (int)dropItem.Tag;
                hndAutoUpdBtn.Checked = true;
                hndFileMenu_Click(hndAutoUpdBtn, EventArgs.Empty);
            }
            else
            {
                // TODO - prompt for interval.
            }

            // dropMenu.OwnerItem.Text = dropItem.Text;
            // dropMenu.OwnerItem.Image = dropItem.Image;
        }

        private void hndRefreshBtn_Click(object sender, EventArgs e)
        {
            if (handleTimer.Enabled)
            {
                // User pressed refresh button, Reset timer
                handleLabel.Enabled = false;
                handleLabel.Enabled = true;
            }

            UpdateHandleView();
        }

        private void hndFileMenu_Click(object sender, EventArgs e)
        {
            string text = (sender is ToolStripItem) ?
               ((ToolStripItem)sender).Text :
               ((ToolStripDropDownItem)sender).Text;

            switch (text)
            {
                case "Auto Update":
                    // hndRefreshBtn.ForeColor = hndAutoUpdBtn.Checked ? Color.Green : Color.Red;
                    hndRefreshBtn.Image = hndAutoUpdBtn.Checked ?  
                        global::MagniFile.Properties.Resources.yellow24 :
                        global::MagniFile.Properties.Resources.red24;

                    string intervalStr = (handleTimer.Interval / 1000).ToString() + " seconds";
                    hndAutoUpdBtn.ToolTipText = "Auto refresh every " + intervalStr;
                    hndRefreshBtn.ToolTipText = hndAutoUpdBtn.Checked ?
                        "Refresh Handle Display, Auto refresh every " + intervalStr :
                        "Refresh Handle Display";
                    break;

                case "Auto Log":
                    if (hndAutoLogBtn.Checked == false)
                    {
                        hndLogWriter = null;
                        SetStatus(handleStatus, "Logging disabled", false);
                        hndLogMenu.ForeColor = Color.White;
                    }
                    else
                    {
                        try
                        {
                            hndLogWriter = new StreamWriter(hndLogFilename, true);
                            SetStatus(handleStatus, "Logging to file:" + hndLogFilename, false);
                            hndLogMenu.ForeColor = Color.Green;
                        }
                        catch 
                        {
                            SetStatus(handleStatus, "Unable to start logging to file:" + hndLogFilename, true);
                            hndAutoLogBtn.Checked = false;
                            hndLogMenu.ForeColor = Color.White;
                        }
                    }
                    break;

                case "Filter...":
                    filterDialog.List = hndFilter;
                    filterDialog.ShowDialog();
                    hndFilter = filterDialog.List;
                    UpdateHandleView();
                    break;

                case "Log File...":
                    this.logFileDialog.FileName = hndLogFilename;
                    SetPath(this.logFileDialog, hndLogFilename);
                    if (this.logFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        hndLogFilename = logFileDialog.FileName;
                        hndLogWriter = null;
                        hndAutoLogBtn.Checked = true;
                        hndFileMenu_Click(hndAutoLogBtn, e);  
                    }
                    break;
                case "Log":
                    if (hndLogWriter == null)
                    {
                        try
                        {
                            hndLogWriter = new StreamWriter(hndLogFilename, true);
                            LogHandle();
                            hndLogWriter = null;
                        }
                        catch (Exception ee)
                        {
                            SetStatus(handleStatus, ee.Message + ", Failed to log to file:" + hndLogFilename, true);
                        }
                    }
                    else
                    {
                        LogHandle();
                    }
                    break;

                case "Open log file":
                    try
                    {
                        FileInfo fileInfo = new FileInfo(hndLogFilename);
                        if (fileInfo.Exists)
                        {
                            System.Diagnostics.Process.Start(hndLogFilename);
                            SetStatus(handleStatus, hndLogFilename, false);
                        }
                        else
                        {
                            SetStatus(handleStatus, "No log file:" + hndLogFilename, true);
                        }
                    }
                    catch
                    {
                        SetStatus(handleStatus, "Failed to open log file:" + hndLogFilename, true);
                    }
                    break;

                case "Export...":
                    ListViewExt.Export(
                        this.hndSummaryBtn.Checked ? this.hndSumView : this.handleView);
                    break;
                case "Print Setup":
                case "Print Setup...":
                    printListView.FitToPage = hndFitToPageBtn.Checked;
                    printListView.PageSetup();
                    break;
                case "Preview":
                case "Print Preview...":
                    printListView.FitToPage = hndFitToPageBtn.Checked;
                    printListView.PrintPreview(this, 
                        this.hndSummaryBtn.Checked ? this.hndSumView : this.handleView, 
                        PrintTitle() + "Handle List");
                    break;
                case "Print":
                case "Print...":
                    printListView.FitToPage = hndFitToPageBtn.Checked;
                    printListView.Print(this, 
                        this.hndSummaryBtn.Checked ? this.hndSumView : this.handleView, 
                        PrintTitle() + "Handle List");
                    break;
            }
        }

        /// <summary>
        /// Try and set FileDialog's initial directory path to directory part of filePath.
        /// </summary>
        /// <param name="fileDialog"></param>
        /// <param name="filePath"></param>
        private void SetPath(SaveFileDialog fileDialog, string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                fileDialog.InitialDirectory = fileInfo.Exists ? fileInfo.DirectoryName :
                    Path.GetDirectoryName(filePath);
            }
            catch { }
        }
         
        private void hndGrpBtn_Click(object sender, EventArgs e)
        {
            ToolStripDropDownItem dropItem = (ToolStripDropDownItem)sender;
            ToolStripDropDownMenu dropMenu = dropItem.DropDown.OwnerItem.Owner as ToolStripDropDownMenu;

            foreach (ToolStripDropDownItem item in dropMenu.Items)
                item.Image = Properties.Resources.check_off;

            dropItem.Image = Properties.Resources.check_on;
            if (viewHndGroup != (ListViewHandles.ViewHndGroup)dropItem.Tag)
            {
                viewHndGroup = (ListViewHandles.ViewHndGroup)dropItem.Tag;
                UpdateHandleView();
            }
        }

        private void hndShowAllBtn_Click(object sender, EventArgs e)
        {
            ToolStripDropDownItem dropItem = (ToolStripDropDownItem)sender;
            ToolStripDropDownMenu dropMenu = dropItem.DropDown.OwnerItem.Owner as ToolStripDropDownMenu;

            System.Windows.Forms.ToolStripMenuItem button = (System.Windows.Forms.ToolStripMenuItem)dropItem;
            bool isChecked = button.Checked;

            if (dropItem.Text == "All")
            {
                isChecked = true;  
                foreach (ToolStripDropDownItem item in dropMenu.Items)
                {
                    item.Image = Properties.Resources.check_off;
                    System.Windows.Forms.ToolStripMenuItem itemBtn = 
                        (System.Windows.Forms.ToolStripMenuItem)item;
                    itemBtn.Checked = false;
                    hndShowObjDict[(int)itemBtn.Tag] = false;
                }
            }
            else
            {
                this.hndShowObjDict[0] = false;
                this.hndShowAllBtn.Checked = false;
                this.hndShowAllBtn.Image = Properties.Resources.check_off;
            }

            hndShowObjDict[(int)button.Tag] = isChecked;
            dropItem.Image = isChecked ? Properties.Resources.check_on : Properties.Resources.check_off;
            UpdateHandleView();
            UpdateHndSumColumns(hndShowObjDict);
        }

        void UpdateHndSumColumns(Dictionary<int, bool> colState)
        {
            // Summary View
            // columns:
            //  [0] time
            //  [1] pid
            //  [2] procname
            //  [3] total
            //  [4..n] hnd count by type  (types are 1..maxType)
            //
            if (colState[0])    // [0] == all
            {
                int colCnt = hndSumView.Columns.Count;
                for (int objIdx = 1; objIdx < OpenHandles.MaxObjTypeNumber; objIdx++)
                {
                    int width = 60;
                    // Assume typeN columns are not useful and hide.
                    if (OpenHandles.ObjectTypeDescription(objIdx).StartsWith("type"))
                        width = 0;
                    hndSumView.Columns[objIdx + 3].Width = width;
                }
            }
            else
            {
                int colCnt = hndSumView.Columns.Count;
                for (int objIdx = 1; objIdx < OpenHandles.MaxObjTypeNumber; objIdx++)
                {
                    int width = hndShowObjDict[objIdx] ? 60 : 0;
                    // if (OpenHandles.ObjectTypeDescription(objIdx).StartsWith("type"))
                    //     width = 0;
                    hndSumView.Columns[objIdx + 3].Width = width;
                }
            }
        }

        /// <summary>
        ///  Add view show filter buttons
        /// </summary>
        private void AddHndShowButtons()
        {
            hndShowObjDict.Add(0, false);

            for (int objIdx = 1; OpenHandles.ObjectTypeDescription(objIdx) != string.Empty; objIdx++)
            {
                hndShowObjDict.Add(objIdx, false);
            }

#if false
            // default to ALL on
            hndShowObjDict.[0] = true;
#else
            hndShowAllBtn.Checked = false;
            hndShowAllBtn.Image = Properties.Resources.check_off;
            hndShowObjDict[0] = false;

            // 2, 6, 9, 28
            hndShowObjDict[2] = true;        // directory
            // hndShowObjDict[6] = true;        // thread
            // hndShowObjDict[9] = true;        // event
            hndShowObjDict[28] = true;       // file
#endif

            string objNumStr;
            int objNum;
            for (objNum = 1; (objNumStr = OpenHandles.ObjectTypeDescription(objNum)) != string.Empty; objNum++)
            {
                System.Windows.Forms.ToolStripMenuItem showBtn;
                showBtn = new System.Windows.Forms.ToolStripMenuItem();
                showBtn.Text = objNumStr;
                showBtn.Tag = objNum;
                showBtn.Image = hndShowObjDict[objNum] ? Properties.Resources.check_on : Properties.Resources.check_off;
                showBtn.Click += new System.EventHandler(this.hndShowAllBtn_Click);
                showBtn.CheckOnClick = true;
                showBtn.CheckState = System.Windows.Forms.CheckState.Unchecked;

                this.filterToolStripMenuItem.DropDownItems.Add(showBtn);
            }

            // add some extra, just to be safe.
            while (objNum < 50)
                hndShowObjDict.Add(objNum++, false);
        }

        private void hndPopupBtn_Click(object sender, EventArgs e)
        {
            ToolStripDropDownItem dropItem = (ToolStripDropDownItem)sender;
            ToolStripDropDownMenu dropMenu = dropItem.DropDown.OwnerItem.Owner as ToolStripDropDownMenu;

            switch (dropItem.Text)
            {
                case "AutoSize Columns":
                    this.handleView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    this.handleView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.None);
                    return;
            }

            // Point p = this.handleView.PointToClient(System.Windows.Forms.Control.MousePosition);
            // ListViewItem itemAt = this.handleView.GetItemAt(p.X, p.Y);
            if (this.handleView.SelectedItems.Count != 0)
            {
                ListViewItem item = this.handleView.SelectedItems[0];

                string objType = item.SubItems[ListViewHandles.hObjTypeCol].Text;
                string desc = item.SubItems[ListViewHandles.hDescCol].Text;

                switch (dropItem.Text)
                {
                    case "Count Dups":
                        this.handleView.BeginUpdate();
                        ColumnClick(this.handleView, new ColumnClickEventArgs(ListViewHandles.hDescCol));
                        string prev = string.Empty;
                        int cnt = 0;
                        foreach (ListViewItem hItem in this.handleView.Items)
                        {
                            if (hItem.SubItems[ListViewHandles.hDescCol].Text == prev)
                            {
                                cnt++;
                            }
                            else
                            {
                                cnt = 1;
                                prev = hItem.SubItems[ListViewHandles.hDescCol].Text;
                            }
                            hItem.SubItems[ListViewHandles.hDupCntCol].Text = cnt.ToString();
                        }
                        this.handleView.Columns[ListViewHandles.hDupCntCol].Width = 60;
                        this.handleView.EndUpdate();
                        break;

                    case "Clear Dups":
                        this.handleView.Columns[ListViewHandles.hDupCntCol].Width = 0;
                        break;

                    case "File - Explorer":
                        if (objType == "File" || objType == "Directory")
                        {
                            System.Diagnostics.Debug.WriteLine("Explorer " + desc);
                            string dir = Path.GetDirectoryName(desc);
                            if (System.Diagnostics.Process.Start("explorer.exe", dir).HasExited)
                                MessageBox.Show("Failed to start explorer on dir of " + desc);
                            else
                                MessageBox.Show(desc);
                        }
                        break;
                    case "File - Cmd":
                        if (objType == "File" || objType == "Directory")
                        {
                            System.Diagnostics.Debug.WriteLine("cmd " + desc);

                            string dir = Path.GetDirectoryName(desc);
                            System.Diagnostics.Process.Start("cmd.exe", "/k cd \"" + dir + "\"");
                            MessageBox.Show(desc);
                        }
                        break;
                    case "Key - RegEdit":
                        if (objType == "Registry")
                        {
                            string prefix = @"\REGISTRY\";
                            if (desc.StartsWith(prefix))
                                desc = desc.Substring(prefix.Length);
                            if (desc.StartsWith("MACHINE"))
                                desc = "LOCAL_" + desc;

                            // Preset last place RegEdit visited to our registry section
                            RegistryKey regEditKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Applets\Regedit");
                            regEditKey.SetValue("LastKey", @"My Computer\HKEY_" + desc);
                            regEditKey.Close();

                            // Launch RegEdit which will default at the last place it was (our stuff).
                            System.Diagnostics.Process.Start("regedit.exe");
                            MessageBox.Show("RegEdit " + desc);
                        }
                        break;
                }
            }
        }

        // Toggle hide description check box and update view.
        private void hndHideNoDescBtn_Click(object sender, EventArgs e)
        {
            this.hndHideNoDescBtn.Image = this.hndHideNoDescBtn.Checked ?
                    Properties.Resources.check_on :
                    Properties.Resources.check_off;
            UpdateHandleView();
        }


        private void fileMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem item = this.handleView.SelectedItems[0];

            string objType = item.SubItems[4].Text;
            string filePath = item.SubItems[5].Text;

            if (objType == "File")
            {
                // Currently this does not appear to work as expect.
                ViewDiskAllocation viewDiskAllocation = new ViewDiskAllocation(filePath);
                viewDiskAllocation.Show();
            }
        }

        private void hndSummaryBtn_Click(object sender, EventArgs e)
        {
            this.hndSumView.Visible = this.hndSummaryBtn.Checked;
            this.handleView.Visible = !hndSumView.Visible;
        }

        private void hndClearSumBtn_Click(object sender, EventArgs e)
        {
            this.hndSumView.Items.Clear();
        }

        private void hndShowClosedCk_Click(object sender, EventArgs e)
        {
            UpdateHandleView();   
        }

        #endregion

        #region ==== Process or Handle common UI actions
        private void removeSelectedItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripDropDownItem dropItem = (ToolStripDropDownItem)sender;
            ToolStripDropDownMenu dropMenu = dropItem.DropDown.OwnerItem.Owner as ToolStripDropDownMenu;

            if (this.hndSumView.ContextMenuStrip == dropMenu)
            {
                foreach (ListViewItem item in this.hndSumView.SelectedItems)
                    this.hndSumView.Items.Remove(item);
            }

            if (this.procSumView.ContextMenuStrip == dropMenu)
            {
                foreach (ListViewItem item in this.procSumView.SelectedItems)
                    this.procSumView.Items.Remove(item);
            }
        }

        private void removeAllItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripDropDownItem dropItem = (ToolStripDropDownItem)sender;
            ToolStripDropDownMenu dropMenu = dropItem.DropDown.OwnerItem.Owner as ToolStripDropDownMenu;

            if (this.hndSumView.ContextMenuStrip == dropMenu)
            {
                this.hndSumView.Items.Clear();
            }

            if (this.procSumView.ContextMenuStrip == dropMenu)
            {
                foreach (ListViewItem item in this.procSumView.SelectedItems)
                    this.procSumView.Items.Clear();
            }
        }

        private void SetStatus(TextBox status, string msg, bool error)
        {
            if (error)
            {
                this.sPlayer.Stream = Properties.Resources.sonar;
                this.sPlayer.Play();
            }

            status.ForeColor = error ? Color.DarkRed : Color.Black;
            status.Text = msg;
        }

        #endregion

        #region ==== Resize viewer
        private Size startPos = Size.Empty;

        private void resize_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.startPos != Size.Empty)
            {
                Size newPos = new Size(Control.MousePosition.X, Control.MousePosition.Y);
                this.Size = this.Size - (this.startPos - newPos);
                this.startPos = newPos;
            }
        }

        private void resize_MouseDown(object sender, MouseEventArgs e)
        {
            this.startPos = new Size(Control.MousePosition.X, Control.MousePosition.Y);
            this.Cursor = Cursors.SizeNWSE;
        }

        private void resize_MouseUp(object sender, MouseEventArgs e)
        {
            resize_MouseLeave(sender, e);
        }

        private void resize_MouseLeave(object sender, EventArgs e)
        {
            this.startPos = Size.Empty;
            this.Cursor = Cursors.Default;

            int w = 0;
            foreach (ToolStripItem titem in this.mainToolStrip.Items)
                w += titem.Width;

            w -= titleLeft.Width + titleRight.Width;
            int fillerWidth = this.Width - w;
            if (fillerWidth < 0)
                fillerWidth = 0;

            System.Diagnostics.Debug.WriteLine("filler width= " + fillerWidth.ToString());
            titleLeft.Width = titleRight.Width = fillerWidth / 2;
        }

        private void resize_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.SizeNWSE;
        }

        #endregion

        #region ==== Eye Candy on tool strips

        private void procToolStrip_MouseEnter(object sender, EventArgs e)
        {
            this.procToolStrip.BackgroundImage = Properties.Resources.hblueGlow;
        }

        private void procToolStrip_MouseLeave(object sender, EventArgs e)
        {
            this.procToolStrip.BackgroundImage = Properties.Resources.hblue;
        }

        private void handleToolStrip_MouseEnter(object sender, EventArgs e)
        {
            this.handleToolStrip.BackgroundImage = Properties.Resources.hlblueGlow;
        }

        private void handleToolStrip_MouseLeave(object sender, EventArgs e)
        {
            this.handleToolStrip.BackgroundImage = Properties.Resources.hlblue;
        }
        #endregion

        #region ==== Launch main dialogs  (large icon buttons)
        ViewFsInfo fsInfoDialog;
        private void FileSysInfo_Click(object sender, EventArgs e)
        {
            ToolStripItem dropItem = (ToolStripItem)sender;

            if (fsInfoDialog == null)
                fsInfoDialog = new ViewFsInfo(dropItem.Image);

            fsInfoDialog.Show();
        }

        private void diskClusterBtn_Click(object sender, EventArgs e)
        {
            ToolStripItem dropItem = (ToolStripItem)sender;

            ViewDiskAllocation viewDiskAllocation = new ViewDiskAllocation(dropItem.Image);
            viewDiskAllocation.Show();
        }

        List<ViewGraph>  viewGraphList = new List<ViewGraph>();
        private void graphBtn_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Control.MouseButtons == System.Windows.Forms.MouseButtons.Right)
                viewGraphList = new List<ViewGraph>();

            if (viewGraphList.Count == 0)
            {
                graphPlusBtn_Click(sender, e);
            }

            foreach (ViewGraph viewGraph in viewGraphList)
            {
                viewGraph.Show();
                viewGraph.BringToFront();
            }
        }

        private void graphPlusBtn_Click(object sender, EventArgs e)
        {
            ToolStripItem dropItem = (ToolStripItem)sender;

            ViewGraph viewGraph = new ViewGraph(dropItem.Image);
            viewGraphList.Add(viewGraph);

            viewGraph.GraphListView = this.procSumView;
            viewGraph.Show();
            viewGraph.BringToFront();
        }

       
        ViewNetInfo viewNetInfo;
        private void netInfo_Click(object sender, EventArgs e)
        {
            ToolStripItem dropItem = (ToolStripItem)sender;

            if (viewNetInfo == null)
                viewNetInfo = new ViewNetInfo(dropItem.Image);

            viewNetInfo.Show();
            viewNetInfo.BringToFront();
        }

        ViewFileList viewFileList;
        private void fileListBtn_Click(object sender, EventArgs e)
        {
            ToolStripItem dropItem = (ToolStripItem)sender;

            if (viewFileList == null)
                viewFileList = new ViewFileList(dropItem.Image);

            viewFileList.Show();
            viewFileList.BringToFront();
        }

        ViewFileEvents viewFileEvents;
        private void monFileBtn_Click(object sender, EventArgs e)
        {
            ToolStripItem dropItem = (ToolStripItem)sender;

            if (viewFileEvents == null)
                viewFileEvents = new ViewFileEvents(dropItem.Image);
            viewFileEvents.Show();
            viewFileEvents.BringToFront();
        }

        #endregion

        #region ==== Display Process Information dialog
        private void processInformationMenuBtn_Click(object sender, EventArgs e)
        {
            ListView listView = procSumView.Visible ? procSumView : procView;
            ShowProcessInfo(listView, System.Windows.Forms.Control.MousePosition);
        }

        private void hndSumView_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void ShowProcessInfo(object sender, Point mouseLocation)
        {
            ListView listView = (ListView)sender;
            ListViewItem item = (listView.SelectedItems.Count != 0) ?
                listView.SelectedItems[0] : listView.HitTest(mouseLocation).Item;

            if (item != null && item.Tag != null)
            {
                int pid = (int)item.Tag;
                ViewProcessInfo viewProcessInfo = new ViewProcessInfo(pid);
                viewProcessInfo.Show();
            }
        }

        #endregion

        #region ==== Highlight cell under cursor
        private void procView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.None)
                UpdateProcStatus();
        }

        int lastPDSubIdx = -1;
        ListViewItem lastProcDetailItem = null;
        private void UpdateProcStatus()
        {
            if (lastProcDetailItem != null && lastPDSubIdx > -1)
            {
                lastProcDetailItem.UseItemStyleForSubItems = true;
                lastProcDetailItem.SubItems[lastPDSubIdx].BackColor = lastProcDetailItem.BackColor;
            }

            Point p = this.procView.PointToClient(System.Windows.Forms.Control.MousePosition);
            ListViewHitTestInfo hitInfo = this.procView.HitTest(p);

            string colName = "";
            object subValue = 0;

            if (hitInfo.Item != null && hitInfo.SubItem != null)
            {
                int colIdx = hitInfo.Item.SubItems.IndexOf(hitInfo.SubItem);
                colName = procView.Columns[colIdx].Text;
                if (hitInfo.SubItem.Tag != null)
                    subValue = hitInfo.SubItem.Tag;
                else
                    subValue = hitInfo.SubItem.Text;

                SetStatus(procStatus, string.Format("{0} {1} {2} {3:#,##0}",
                    hitInfo.Item.SubItems[ListViewProcesses.pNameCol].Text,
                    hitInfo.Item.SubItems[ListViewProcesses.pIdCol].Text,
                    colName,
                    subValue), false);

                lastProcDetailItem = hitInfo.Item;
                lastPDSubIdx = colIdx;
                lastProcDetailItem.UseItemStyleForSubItems = false;
                lastProcDetailItem.SubItems[lastPDSubIdx].BackColor = Color.LightBlue;

                // Scroll to keep top and bottom items in view.
                if (p.Y < 30 && lastProcDetailItem.Index > 0)
                    procView.Items[lastProcDetailItem.Index - 1].EnsureVisible();
                else if (p.Y + 25 > procView.Height && lastProcDetailItem.Index + 1 < procView.Items.Count)
                    procView.Items[lastProcDetailItem.Index + 1].EnsureVisible();
            }
        }

        private void procSumView_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateProcSumStatus();
        }

        int lastPSSubIdx = -1;
        ListViewItem lastProcSumItem = null;
        private void UpdateProcSumStatus()
        {
            if (lastProcSumItem != null && lastPSSubIdx > -1)
            {
                lastProcSumItem.UseItemStyleForSubItems = true;
                lastProcSumItem.SubItems[lastPSSubIdx].BackColor = lastProcSumItem.BackColor;

#if false
                foreach (ListViewItem scanItem in this.procSumView.Items)
                {
                    if (scanItem.SubItems[lastPSSubIdx].BackColor == Color.LightCyan)
                    {
                        scanItem.SubItems[lastPSSubIdx].BackColor = scanItem.BackColor;
                    }
                }
#endif
            }

            Point p = this.procSumView.PointToClient(System.Windows.Forms.Control.MousePosition);
            ListViewHitTestInfo hitInfo = this.procSumView.HitTest(p);

            string colName = "";
            object subValue = 0;

            if (hitInfo.Item != null && hitInfo.SubItem != null)
            {
                int colIdx = hitInfo.Item.SubItems.IndexOf(hitInfo.SubItem);
                colName = procSumView.Columns[colIdx].Text;
                if (hitInfo.SubItem.Tag != null)
                    subValue = hitInfo.SubItem.Tag;
                else
                    subValue = hitInfo.SubItem.Text;

                // Scan all rows and measure columns:
                //      min
                //      max
                //      (max-min) / elapsed_minutes
                //
                // report min value max  chg/min
                //

                try
                {
                    ulong dVal = (ulong)subValue;
                    ulong dMin = ulong.MaxValue;
                    ulong dMax = ulong.MinValue;

                    foreach (ListViewItem scanItem in this.procSumView.Items)
                    {
                        if (scanItem.SubItems[ListViewProcesses.pIdCol].Text ==
                             hitInfo.Item.SubItems[ListViewProcesses.pIdCol].Text)
                        {
                            // scanItem.SubItems[colIdx].BackColor = Color.LightCyan;
                            ulong d = (ulong)scanItem.SubItems[colIdx].Tag;
                            dMin = Math.Min(dMin, d);
                            dMax = Math.Max(dMax, d);
                        }
                    }

                    SetStatus(procStatus, string.Format("{0} {1} {2} Min:{3:#,##0} Val:{4:#,##0} Max:{5:#,##0}",
                            hitInfo.Item.SubItems[ListViewProcesses.pNameCol].Text,
                            hitInfo.Item.SubItems[ListViewProcesses.pIdCol].Text,
                            colName,
                            dMin, dVal, dMax), false);
                }
                catch
                {
                    SetStatus(procStatus, string.Format("{0} {1} {2} {3:#,##0}",
                            hitInfo.Item.SubItems[ListViewProcesses.pNameCol].Text,
                            hitInfo.Item.SubItems[ListViewProcesses.pIdCol].Text,
                            colName,
                            subValue), false);
                }

                lastProcSumItem = hitInfo.Item;
                lastPSSubIdx = colIdx;
                lastProcSumItem.UseItemStyleForSubItems = false;
                lastProcSumItem.SubItems[lastPSSubIdx].BackColor = Color.LightBlue;

                // Scroll to keep top and bottom items in view.
                if (p.Y < 30 && lastProcSumItem.Index > 0)
                    procSumView.Items[lastProcSumItem.Index - 1].EnsureVisible();
                else if (p.Y + 25 > procSumView.Height && lastProcSumItem.Index + 1 < procSumView.Items.Count)
                    procSumView.Items[lastProcSumItem.Index + 1].EnsureVisible();
            }
        }


        private void HndView_MouseMove(object sender, MouseEventArgs e)
        {
            ListView lv = (ListView)sender;
            UpdateHndStatus(lv);
        }

        int lastHndSubIdx = -1;
        ListViewItem lastHndItem = null;
        private void UpdateHndStatus(ListView listView)
        {
            if (lastHndItem != null && lastHndSubIdx > -1)
            {
                lastHndItem.UseItemStyleForSubItems = true;
                lastHndItem.SubItems[lastHndSubIdx].BackColor = lastHndItem.BackColor;
            }

            Point p = listView.PointToClient(System.Windows.Forms.Control.MousePosition);
            ListViewHitTestInfo hitInfo = listView.HitTest(p);

            string colName = "";
            object subValue = 0;

            if (hitInfo.Item != null && hitInfo.SubItem != null)
            {
                int colIdx = hitInfo.Item.SubItems.IndexOf(hitInfo.SubItem);
                colName = listView.Columns[colIdx].Text;
                if (hitInfo.SubItem.Tag != null)
                    subValue = hitInfo.SubItem.Tag;
                else
                    subValue = hitInfo.SubItem.Text;

                SetStatus(handleStatus, string.Format("{0} {1} {2} {3:#,##0}",
                        hitInfo.Item.SubItems[1].Text,
                        hitInfo.Item.SubItems[2].Text,
                        colName,
                        subValue), false);

                lastHndItem = hitInfo.Item;
                lastHndSubIdx = colIdx;
                lastHndItem.UseItemStyleForSubItems = false;
                lastHndItem.SubItems[lastHndSubIdx].BackColor = Color.LightBlue;

                // Scroll to keep top and bottom items in view.
                if (p.Y < 30 && lastHndItem.Index > 0)
                    listView.Items[lastHndItem.Index - 1].EnsureVisible();
                else if (p.Y + 25 > listView.Height && lastHndItem.Index + 1 < listView.Items.Count)
                    listView.Items[lastHndItem.Index + 1].EnsureVisible();
            }
        }

        #endregion

        #region ==== Eye candy - glow title area
        private void title_MouseEnter(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.title.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("title.BackgroundImage")));
        }

        private void title_MouseLeave(object sender, EventArgs e)
        {
            this.title.BackgroundImage = null;
        }

        #endregion

      

    }
}
