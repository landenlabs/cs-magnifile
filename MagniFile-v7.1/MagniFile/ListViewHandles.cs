using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

using System.Threading;

namespace MagniFile
{
    /// <summary>
    /// Present OpenHandle list in a ListView
    /// 
    /// Author: Dennis Lang 2009
    /// https://landenlabs.com/
    /// 
    /// </summary>
    class ListViewHandles
    {
        public enum ViewHndGroup { eNoGroup, ePidGroup, eObjectGroup };

        private ListView hndView;
        private bool abort;
        Dictionary<int, bool> onlyObjDict;

        public ListView View
        {
            get { return this.hndView; }
            set { this.hndView = value; }
        }

        // Part of update uses a background thread. Set abort to stop thread.
        public void Abort() { this.abort = true; }
       

        OpenHandles.SYSTEM_HANDLE_INFORMATION[] handles;
        SortedList<int, string> pidList;

        public struct HandleInfo
        {
            public OpenHandles.SYSTEM_HANDLE_INFORMATION handle;
            public string desc;
            public string error;
        }
        SortedList<int, SortedList<int, HandleInfo>> pidHndDescList;

        public  SortedList<int, SortedList<int, HandleInfo>> PidHandleList
        {
            get { return this.pidHndDescList; }
        }

        static AutoResetEvent threadEvent;
        static Dictionary<OpenHandles.SYSTEM_HANDLE_INFORMATION, string> HndNameDict = new Dictionary<OpenHandles.SYSTEM_HANDLE_INFORMATION, string>();

        /// <summary>
        ///  Background thread does the slow and painful work of collecting handle information
        ///  and converting it to descriptions
        /// </summary>
        public void ThreadBgWorker()
        {
            bool any = false;
            onlyObjDict.TryGetValue(0, out any);

            Process ourProc = Process.GetCurrentProcess();
            Process pidProc = ourProc;

            // Convert handles to descriptions (this may be slow)
            foreach (OpenHandles.SYSTEM_HANDLE_INFORMATION handle in handles)
            {
                if (this.abort)
                    break;

                int pid = handle.ProcessId;

                if (pid < 0)
                    System.Diagnostics.Debug.WriteLine("Bad pid=" + pid.ToString());

                if (pidList.ContainsKey(pid))
                {
                    SortedList<int, HandleInfo> hndFileList;
                    if (pidHndDescList.TryGetValue(pid, out hndFileList) == false)
                    {
                        hndFileList = new SortedList<int, HandleInfo>();
                        pidHndDescList.Add(pid, hndFileList);
                    }

                    bool objOkay;
                    if (any || onlyObjDict.TryGetValue(handle.ObjectTypeNumber, out objOkay) && objOkay)
                    {
                        try
                        {
                            HandleInfo hndInfo = new HandleInfo();
                            hndInfo.handle = handle;
                            hndInfo.desc = string.Empty;

                            //  Some objects may hang when dup'd, so lets avoid the hang.
                            const int SYNCHRONIZE = 0x00100000;
                            if (handle.ObjectTypeNumber == 28 &&
                                (handle.GrantedAccess == SYNCHRONIZE
                                || handle.GrantedAccess == 0
                                || handle.GrantedAccess == 0x0012019f
                               /*  || handle.GrantedAccess == 0x001a019f */))
                            {
                                // avoid hang
                                // System.Diagnostics.Debug.WriteLine("pid=" + pid);
                            }
                            else
                            {
                                if (pidProc.Id != handle.ProcessId)
                                {
                                    try
                                    {
                                        pidProc = Process.GetProcessById(handle.ProcessId);
                                    }
                                    catch
                                    {
                                        pidProc = null;
                                    }
                                }

                                if (pidProc != null && HndNameDict.TryGetValue(handle, out hndInfo.desc) == false)
                                {
                                    if (HndNameDict.Count > 30000)
                                        HndNameDict.Clear();

                                    OpenHandles.HandleDescription(ourProc, pidProc, handle.Handle, handle.ObjectTypeNumber, out hndInfo.desc);
                                    HndNameDict.Add(handle, hndInfo.desc);
                                }
                            }
                            hndFileList.Add(handle.Handle, hndInfo);
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine("Handle description error:" + ee.Message);
                        }
                    }
                }
            }

            threadEvent.Set();
        }

        Thread m_threadHnd; // background thread to collect open handles.

        public void AbortThreads()
        {
            try
            {
                if (m_threadHnd != null)
                {
                    m_threadHnd.Abort();
                    if (!m_threadHnd.Join(1000))
                    { }
                    m_threadHnd = null;
                }
            }
            catch { }
        }

        /// <summary>
        /// Get List of open handles
        /// </summary>
        /// <param name="showObjDict"></param>
        /// <returns>process count</returns>
        public int GetHandles(
            SortedList<int, string> pidList, 
            Dictionary<int, bool> showObjDict, 
            bool freshenHandleList)
        {
            // Collect handles and handle descriptions in a background thread
            this.pidHndDescList = new SortedList<int, SortedList<int, HandleInfo>>();
            this.pidList = pidList;
            this.onlyObjDict = showObjDict;
            this.abort = false;

            // Grab handles
            if (freshenHandleList || this.handles == null)
                this.handles = OpenHandles.EnumHandles();

            m_threadHnd = new Thread(new ThreadStart(this.ThreadBgWorker));
            threadEvent = new AutoResetEvent(false);
            m_threadHnd.Start();

            DateTime now = DateTime.Now;
            while (!threadEvent.WaitOne(0) && !this.abort)
            {
                Application.DoEvents();

                // Allow a few seconods to collect info.
                if (m_threadHnd.Join(100))
                    break;
                if ((DateTime.Now - now).Seconds > 2)
                    break;
            }

            if (!m_threadHnd.Join(100))
            {
                try
                {
                    m_threadHnd.Abort();    // why does this throws an exception ?
                }
                catch { }
            }
            m_threadHnd = null;

            return this.pidHndDescList.Count;
        }

        public void UpdateView(
            ListView detailView,
            ListView sumView, 
            TextBox status,
            FilterList filterList,
            SortedList<int, string> _pidList, 
            Dictionary<int, bool> showObjDict,
            ViewHndGroup viewGroup,
            bool hideIfNoDesc,
            bool showClosedHnd)     // briefly show closed handles (pink color for one update cycle).
        {
            if (_pidList.Count == 0)
            {
                // Nothing to do, so exit
                detailView.Items.Clear();
                status.Text = "No handles - Please select a running process to examine";
                return;
            }

            detailView.Enabled = false;

            // Collect handles and handle descriptions in a background thread
            this.pidHndDescList = new SortedList<int, SortedList<int, HandleInfo>>();
            this.pidList = _pidList;
            this.onlyObjDict = showObjDict;
            this.abort = false;

            // Grab handles
            this.handles = OpenHandles.EnumHandles();

            Thread threadHnd = new Thread(new ThreadStart(this.ThreadBgWorker));
            threadEvent = new AutoResetEvent(false);
            threadHnd.Start();

            DateTime now = DateTime.Now;
            // Calling DoEvents is dangerous
            while (!threadEvent.WaitOne(0) && !this.abort)
            {
                Application.DoEvents();

                // Allow a few seconods to collect info.
                if ((DateTime.Now - now).Seconds > 2)
                    break;
            }

            status.Text = string.Empty;

            if (!threadHnd.Join(1000))
            {
                try
                {
                    threadHnd.Abort();
                    if (!abort)
                        status.Text += " Handle collection Timed out, only partial list available. ";
                }
                catch { }
            }

            if (abort)
            {
                // User aborted, so clear list and return.
                status.Text += " Aborted by user. ";
                // return;
            }

            // Fill list view with handle info
            this.hndView = detailView;
            this.hndView.BeginUpdate();

            System.Collections.IComparer sorter = this.hndView.ListViewItemSorter;
            this.hndView.ListViewItemSorter = null;

            // Check if previous handle list is unrelated to new process ids.
            if (pidList.Count == 1 && hndView.Items.Count != 0 && 
                pidList.IndexOfKey((int)hndView.Items[0].Tag) != 0)
            {
                hndItemDict.Clear();
                hndView.Items.Clear();
            }

            if (showClosedHnd)
            {
                // Show closed handles in pink - set all current handles to pink and then update 
                // list with normal color, leaving closed in pink.
                foreach (ListViewItem markItem in this.hndView.Items)
                {
                    if (markItem.BackColor == Color.LightPink)
                    {
#if true
                        string lookup = markItem.SubItems[hIdCol].Text +
                            markItem.SubItems[hNameCol].Text +
                            markItem.SubItems[hHndCol].Text +
                            markItem.SubItems[hObjTypeCol].Text +
                            markItem.SubItems[hDescCol].Text;
                        hndItemDict.Remove(lookup);
#endif
                        this.hndView.Items.Remove(markItem);
                    }
                    else
                    {
                        markItem.BackColor = Color.LightPink;
                    }
                }
            }
            else
            {
                hndView.Items.Clear();
                hndItemDict.Clear();
            }
            this.hndView.Groups.Clear();

            // Used to group objects in listView
            SortedList<byte, ListViewGroup> objGroup = new SortedList<byte, ListViewGroup>();
                                                                      
            foreach (int pid in pidList.Keys)
            {
                string procName = pidList[pid];
                ListViewGroup grp = null;
                if (viewGroup == ViewHndGroup.ePidGroup)
                    grp = hndView.Groups.Add(pid.ToString(), procName);

                SortedList<int, HandleInfo> hndDescList;
                if (this.pidHndDescList.TryGetValue(pid, out hndDescList))         
                {
                    // Used to update handle summary entry.
                    int sMaxHndType = OpenHandles.MaxObjTypeNumber;
                    int hndSumTotal = 0;
                    int[] hndSummary = new int[sMaxHndType];
                    Array.Clear(hndSummary, 0, sMaxHndType);
                    
                    foreach (int hnd in hndDescList.Keys)
                    {
                        HandleInfo hndInfo = hndDescList[hnd];
                        hndSumTotal++;
                        if (hndInfo.handle.ObjectTypeNumber < sMaxHndType)
                            hndSummary[hndInfo.handle.ObjectTypeNumber]++;

                        if (showObjDict[0] == true || showObjDict[hndInfo.handle.ObjectTypeNumber] == true)
                        {
                            if (hideIfNoDesc == false || hndInfo.desc.Length != 0)
                            {
                                ListViewItem item = AddProc(hndInfo, procName, showClosedHnd);

                                if (viewGroup == ViewHndGroup.eObjectGroup)
                                {
                                    if (objGroup.TryGetValue(hndInfo.handle.ObjectTypeNumber, out grp) == false)
                                    {
                                        byte objTypeNum = hndInfo.handle.ObjectTypeNumber;
                                        grp = hndView.Groups.Add(objTypeNum.ToString(), OpenHandles.ObjectTypeDescription(objTypeNum));
                                        objGroup.Add(objTypeNum, grp);
                                    }
                                }
                                item.Group = grp;
                            }
                        }
                    }

                    // Update summary
                    ListViewItem sumItem = sumView.Items.Add(now.ToString("G"));
                    sumItem.SubItems.Add(pid.ToString()).Tag = pid;
                    sumItem.SubItems.Add(procName);
                    sumItem.SubItems.Add(hndSumTotal.ToString());
                    for (int hndIdx = 1; hndIdx < hndSummary.Length; hndIdx++)
                        sumItem.SubItems.Add(hndSummary[hndIdx].ToString());
                }
            }

            this.hndView.ListViewItemSorter = sorter;
            this.hndView.EndUpdate();
            this.hndView.Enabled = true;

            status.Text += hndView.Items.Count.ToString() +
                      " handles for process: " + pidList.First().Value;
            if (pidList.Count > 1)
                status.Text += " ...";
        }

        /// View Column layout;
        public const int hIdCol = 1;
        public const int hNameCol = 2;
        public const int hHndCol = 3;
        public const int hObjTypeCol = 4;
        public const int hDescCol = 5;
        public const int hAgeCol = 6;
        public const int hStimeCol = 7;
        public const int hDupCntCol = 8;
        public const int hLastCol = 9;

        Dictionary<string, ListViewItem> hndItemDict = new Dictionary<string, ListViewItem>(1000);

        private ListViewItem AddProc(HandleInfo hndInfo, string procName, bool colorizeNewHandles)
        {
            ListViewItem item = null;   // this.hndView.Items.Add("");

            string handleStr = hndInfo.handle.Handle.ToString();
            string objTypeStr = OpenHandles.ObjectTypeDescription(hndInfo.handle.ObjectTypeNumber);

#if true
            string lookup = hndInfo.handle.ProcessId.ToString() +
                procName + handleStr + objTypeStr + hndInfo.desc;
            if (hndItemDict.TryGetValue(lookup, out item))
            {
                item.BackColor = this.hndView.BackColor;
                int age;
                item.SubItems[hAgeCol].Tag = age = (int)item.SubItems[hAgeCol].Tag + 1;
                item.SubItems[hAgeCol].Text = age.ToString("#,##0");
                return item;
            }
#else
            foreach (ListViewItem scanItem in this.hndView.Items)
            {
                if ((int)scanItem.Tag == hndInfo.handle.ProcessId)
                {
                    if (scanItem.SubItems[hNameCol].Text == procName &&
                        scanItem.SubItems[hHndCol].Text == handleStr &&
                        scanItem.SubItems[hObjTypeCol].Text == objTypeStr &&
                        scanItem.SubItems[hDescCol].Text == hndInfo.desc)
                    {
                        item = scanItem;
                        item.BackColor = this.hndView.BackColor;
                        int age;
                        item.SubItems[hAgeCol].Tag = age = (int)item.SubItems[hAgeCol].Tag + 1;
                        item.SubItems[hAgeCol].Text = age.ToString("#,##0");
                        return item;
                    }
                }
            }
#endif

            if (item == null)
            {
                item = this.hndView.Items.Add("");
                if (colorizeNewHandles)
                    item.BackColor = Color.LightGreen;

                for (int col = 0; col < hLastCol; col++)
                    item.SubItems.Add("");
            }

            item.SubItems[hIdCol].Text = hndInfo.handle.ProcessId.ToString();
            item.SubItems[hNameCol].Text = procName;
            item.SubItems[hHndCol].Text = hndInfo.handle.Handle.ToString();
            item.SubItems[hObjTypeCol].Text = OpenHandles.ObjectTypeDescription(hndInfo.handle.ObjectTypeNumber);
            if (hndInfo.error == null || hndInfo.error.Length == 0)
            {
                item.SubItems[hDescCol].Text = hndInfo.desc;
                item.SubItems[hDescCol].BackColor = item.BackColor;
            }
            else
            {
                item.UseItemStyleForSubItems = false;
                item.SubItems[hDescCol].Text = hndInfo.error;
                item.SubItems[hDescCol].BackColor = Color.FromArgb(255, 220, 220);
            }

            item.SubItems[hAgeCol].Text = "0";
            item.SubItems[hAgeCol].Tag = 0;
            item.SubItems[hStimeCol].Text = DateTime.Now.ToString("G");

            item.Tag = hndInfo.handle.ProcessId;

#if true
            hndItemDict.Add(lookup, item);
#endif
            return item;
        }
    }
}
