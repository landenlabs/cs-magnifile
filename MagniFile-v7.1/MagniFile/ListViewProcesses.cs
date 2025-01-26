using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

namespace MagniFile
{
    /// <summary>
    /// Populate ListView with Process information
    /// 
    /// Author: Dennis Lang 2009
    /// https://landenlabs.com/
    /// 
    /// </summary>
    class ListViewProcesses
    {
        ListView procView;
        List<ListViewItem> deadList = new List<ListViewItem>();
        List<ListViewItem> bornList = new List<ListViewItem>();

        public ListView View
        {
            get { return this.procView; }
            set { this.procView = value; }
        }

        public ListViewItem FindViewItem(ListView.ListViewItemCollection items, string want, int subFld)
        {
            foreach (ListViewItem item in items)
            {
                if (item.SubItems[subFld].Text == want)
                    return item;
            }

            return null;
        }

        public Color StartColor
        {
            get { return Color.LightGreen; }
        }
        public Color StopColor
        {
            get { return Color.LightPink; }
        }

        public static List<string> EnumModules(Process process)
        {
            List<string> moduleList = new List<string>();
            try
            {
                ProcessModuleCollection myModules = process.Modules;
                for (int j = 0; j < myModules.Count; j++)
                {
                    moduleList.Add(myModules[j].FileName);
                    // Console.WriteLine(myModules[j].ModuleName + "\t" + 
                    //    myModules[j].EntryPointAddress.ToString("X") + "\t" + 
                    //    myModules[j].FileVersionInfo.FileVersion);
                }
            }
            catch { }

            return moduleList;
        }

        /// <summary>
        /// Update view with latest process list. 
        /// Draw new entries in GREEN and died entries in RED. 
        /// </summary>
        public void UpdateViewStd(ListView view)
        {
            procView = view;
            procView.BeginUpdate();

            try
            {
                Process[] myProcesses = Process.GetProcesses();

                List<int> viewPid = new List<int>();

                bornList.Clear();
                foreach (ListViewItem item in deadList)
                {
                    procView.Items.Remove(item);
                }
                deadList.Clear();

                for (int i = 0; i < myProcesses.Length; i++)
                {
                    ListViewItem item = FindViewItem(procView.Items, myProcesses[i].Id.ToString(), pIdCol);
                    if (item == null)
                    {
                        item = AddProc(i, myProcesses[i]);
                        item.BackColor = StartColor;
                        bornList.Add(item);
                    }
                    else
                    {
                        UpdateProcView(item, i, myProcesses[i]);
                        item.BackColor = Color.White;
                    }

                    viewPid.Add((int)item.Tag);
                }

                foreach (ListViewItem item in procView.Items)
                {
                    int ix = (int)item.Tag;
                    if (viewPid.Find(delegate(int t) { return ix == t; }) != ix)
                    {
                        item.BackColor = StopColor;
                        deadList.Add(item);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            this.procView.EndUpdate();
        }

        public void UpdateViewEx(ListView view)
        {
            procView = view;
            procView.BeginUpdate();

            try
            {
                Dictionary<int, ProcessEx.SystemProcess> processDict = ProcessEx.GetProcesses();

                List<int> viewPid = new List<int>();

                bornList.Clear();
                foreach (ListViewItem item in deadList)
                {
                    procView.Items.Remove(item);
                }
                deadList.Clear();

                int i = 0;
                foreach(int pid in processDict.Keys)
                {
                    ListViewItem item = FindViewItem(procView.Items, pid.ToString(), pIdCol);
                    if (item == null)
                    {
                        item = AddProc(i++, processDict[pid]);
                        item.BackColor = StartColor;
                        bornList.Add(item);
                    }
                    else
                    {
                        UpdateProcView(item, i++, processDict[pid]);
                        item.BackColor = Color.White;
                    }

                    viewPid.Add((int)item.Tag);
                }

                foreach (ListViewItem item in procView.Items)
                {
                    int ix = (int)item.Tag;
                    if (viewPid.Find(delegate(int t) { return ix == t; }) != ix)
                    {
                        item.BackColor = StopColor;
                        deadList.Add(item);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            this.procView.EndUpdate();
        }

        /// View Column layout;
        public const int pDTCol = 1;    // summary DateTime column
        public const int pLogCol = 1;   // detail Log column
        public const int pIdCol = 2;
        public const int pNameCol = 3;
        public const int pIoReadCol = 4;
        public const int pIoWriteCol = 5;
        public const int pIoOtherCol = 6;
        public const int pIoChgCol = 7;
        public const int pHandlesCol = 8;
        public const int pStartTmCol = 9;
        public const int pTotalTmCol = 10;
        public const int pUserTmCol = 11;
        public const int pPrivCol = 12;
        public const int pWorkCol = 13;
        public const int pWkPeakCol = 14;
        public const int pVirtCol = 15;
        public const int pVrPeakCol = 16;
        public const int pPagedCol = 17;
        public const int pPgPeakCol = 18;
        public const int pThreadsCol = 19;
        public const int pModulesCol = 20;
        public const int pLastCol = 21;

        List<int> sysPidList = new List<int>();

        string ToKBString(ulong memSize)
        {
            const ulong KB = 1024;
            return ((double)memSize / KB).ToString("F2") + " KB";
        }

        string ToMBString(ulong memSize)
        {
            const ulong KB = 1024;
            const ulong MB = KB * KB;

            return ((double)memSize / MB).ToString("F2") + " MB";
        }

        string ToNString(ulong num)
        {
            // return num.ToString("#,##0", CultureInfo.InvariantCulture);
            return num.ToString("#,##0");
        }

        enum FrN { eNum, eKB, eMB };
        void SetColumn(ListViewItem.ListViewSubItem subItem, ulong val, FrN frm)
        {
            switch (frm)
            {
                case FrN.eNum:
                    subItem.Text = ToNString(val);
                    break;
                case FrN.eKB:
                    subItem.Text = ToKBString(val);
                    break;
                case FrN.eMB:
                    subItem.Text = ToMBString(val);
                    break;
            }
            subItem.Tag = val;
        }

        void SetColumn(ListViewItem.ListViewSubItem subItem, string textVal, ulong tagVal)
        {
            subItem.Text = textVal;
            subItem.Tag = tagVal;
        }

        private ListViewItem AddProc(int i, ProcessEx.SystemProcess process)
        {
            ListViewItem item = this.procView.Items.Add("");
            // item.UseItemStyleForSubItems = false;

            for (int col = 0; col < pLastCol; col++)
                item.SubItems.Add("");

            UpdateProcView(item, i, process);
            return item;
        }

        private ListViewItem AddProc(int i, Process process)
        {
            ListViewItem item = this.procView.Items.Add("");
            for (int col = 0; col < pLastCol; col++)
                item.SubItems.Add("");

            UpdateProcView(item, i, process);
            return item;
        }

   
        private void UpdateProcView(ListViewItem item, int i, Process process)
        {
            item.SubItems[pIdCol].Text = process.Id.ToString();     // 2
            item.SubItems[pIdCol].Tag = process.Id;
            item.Tag = process.Id;
            item.SubItems[pNameCol].Text = process.ProcessName;

            int pid = process.Id;
            if (sysPidList.Find(delegate(int t) { return pid == t; }) == pid)
            {
                return;
            }

            try
            {
                int p = process.BasePriority;

                try
                {
                    SetColumn(item.SubItems[pStartTmCol], 
                        process.StartTime.ToString("G"), 
                        (ulong)process.StartTime.ToBinary());
                    SetColumn(item.SubItems[pTotalTmCol], 
                        process.TotalProcessorTime.TotalMinutes.ToString("F3"), 
                        (ulong)process.TotalProcessorTime.TotalMinutes);
                    SetColumn(item.SubItems[pUserTmCol], 
                        process.UserProcessorTime.TotalMinutes.ToString("F3"),
                        (ulong)process.UserProcessorTime.TotalMinutes);
                }
                catch { }

                //  item.SubItems.Add(process.NonpagedSystemMemorySize64.ToString());
                //  item.SubItems.Add(process.PagedSystemMemorySize64.ToString());

                FrN fm = FrN.eMB;
                SetColumn(item.SubItems[pPrivCol], (ulong)process.PrivateMemorySize64, fm);

                SetColumn(item.SubItems[pWorkCol], (ulong)process.WorkingSet64, fm);
                SetColumn(item.SubItems[pWkPeakCol], (ulong)process.PeakWorkingSet64, fm);

                SetColumn(item.SubItems[pVirtCol], (ulong)process.VirtualMemorySize64, fm);
                SetColumn(item.SubItems[pVrPeakCol], (ulong)process.PeakVirtualMemorySize64, fm);

                SetColumn(item.SubItems[pPagedCol], (ulong)process.PagedMemorySize64, fm);
                SetColumn(item.SubItems[pPgPeakCol], (ulong)process.PeakPagedMemorySize64, fm);

                item.SubItems[pHandlesCol].Text = (process.HandleCount.ToString());
                item.SubItems[pThreadsCol].Text = (
                    process.Threads != null ?
                    process.Threads.Count.ToString() : "0");

                if (process.ProcessName != "System")
                {
                    item.SubItems[pModulesCol].Text = (
                       process.Modules != null ?
                       process.Modules.Count.ToString() : "0");
                }
            }
            catch
            {
                // clear fields.
                sysPidList.Add(process.Id);
            }
        }

        private void UpdateProcView(ListViewItem item, int i, ProcessEx.SystemProcess sysProcess)
        {
            ProcessEx.SystemProcessInformation process = sysProcess.Process;
            item.SubItems[pIdCol].Text = process.Id.ToString();     // 2
            item.SubItems[pIdCol].Tag = process.Id;
            item.Tag = process.Id;
            item.SubItems[pNameCol].Text = sysProcess.Name;

            int pid = process.Id;
            if (sysPidList.Find(delegate(int t) { return pid == t; }) == pid)
            {
                return;
            }

            try
            {
                int p = process.BasePriority;

                try
                {
                    // item.SubItems[pStartTmCol].Text = (process.StartTime.ToString("G"));
                    // item.SubItems[pTotalTmCol].Text = (process.TotalProcessorTime.TotalMinutes.ToString("F3"));
                    // item.SubItems[pUserTmCol].Text = (process.UserProcessorTime.TotalMinutes.ToString("F3"));

                    SetColumn(item.SubItems[pStartTmCol],
                        process.StartTime.ToString("G"),
                        (ulong)process.StartTime.ToBinary());
                    SetColumn(item.SubItems[pTotalTmCol],
                        process.TotalProcessorTime.TotalMinutes.ToString("F3"),
                        (ulong)process.TotalProcessorTime.TotalMinutes);
                    SetColumn(item.SubItems[pUserTmCol],
                        process.UserProcessorTime.TotalMinutes.ToString("F3"),
                        (ulong)process.UserProcessorTime.TotalMinutes);
                }
                catch { }

                //  item.SubItems.Add(process.NonpagedSystemMemorySize64.ToString());
                //  item.SubItems.Add(process.PagedSystemMemorySize64.ToString());

                FrN fm = FrN.eMB;
                FrN fn = FrN.eNum; 
                SetColumn(item.SubItems[pPrivCol], (ulong)process.PrivateMemorySize64, fm);

                SetColumn(item.SubItems[pWorkCol], (ulong)process.WorkingSet64, fm);
                SetColumn(item.SubItems[pWkPeakCol], (ulong)process.PeakWorkingSet64, fm);

                SetColumn(item.SubItems[pVirtCol], (ulong)process.VirtualMemorySize64, fm);
                SetColumn(item.SubItems[pVrPeakCol], (ulong)process.PeakVirtualMemorySize64, fm);

                SetColumn(item.SubItems[pPagedCol], (ulong)process.PagedMemorySize64, fm);
                SetColumn(item.SubItems[pPgPeakCol], (ulong)process.PeakPagedMemorySize64, fm);

                SetColumn(item.SubItems[pHandlesCol], (ulong)process.HandleCount, fn);
                SetColumn(item.SubItems[pThreadsCol], (ulong)(process.Threads != null ? process.Threads.Count : 0), fn);

#if true


                // Operations
                ulong totIo = process.IoCounters.ReadOperationCount +
                    process.IoCounters.WriteOperationCount + process.IoCounters.OtherOperationCount;
                ulong orgTotIo = totIo;
                try
                {
                    // Extract previous total I/O from tags
                    if (item.SubItems[pIoReadCol].Tag != null)
                        orgTotIo = (ulong)item.SubItems[pIoReadCol].Tag + (ulong)item.SubItems[pIoWriteCol].Tag +
                        (ulong)item.SubItems[pIoOtherCol].Tag;
                }
                catch { }

                SetColumn(item.SubItems[pIoReadCol], process.IoCounters.ReadOperationCount, fn);
                SetColumn(item.SubItems[pIoWriteCol], process.IoCounters.WriteOperationCount, fn);
                SetColumn(item.SubItems[pIoOtherCol], process.IoCounters.OtherOperationCount, fn);
                SetColumn(item.SubItems[pIoChgCol], totIo - orgTotIo, fn);
#else
                // Bytes
                SetColumn(item.SubItems[pIoReadCol], process.IoCounters.ReadTransferCount, fn);
                SetColumn(item.SubItems[pIoWriteCol], process.IoCounters.WriteTransferCount, fn);
                SetColumn(item.SubItems[pIoOtherCol], process.IoCounters.OtherTransferCount, fn);
#endif
                if (sysProcess.Name != "System")
                    SetColumn(item.SubItems[pModulesCol], (ulong)(process.Modules != null ? process.Modules.Count : 0), fn);

            }
            catch (Exception ee)
            {
                // clear fields.
                sysPidList.Add(process.Id);
            }
        }
    }    
}
