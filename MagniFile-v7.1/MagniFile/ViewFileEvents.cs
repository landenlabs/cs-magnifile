using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

using System.Collections;

using System.Reflection;        // help


// Convert owner SId value to user name
// 
// http://netcode.ru/dotnet/?lang=&katID=30&skatID=277&artID=7656

namespace MagniFile
{
    /// <summary>
    /// File System Event Monitor Dialog.
    /// 
    /// Author: Dennis Lang 2009
    /// https://landenlabs.com/
    /// 
    /// </summary>
    public partial class ViewFileEvents : Form
    {
        public ViewFileEvents(Image imageLogo)
        {
            InitializeComponent();

            logo = imageLogo;

            // Prevent viewer from being larger than working area.
            if (this.Height > Screen.GetWorkingArea(this).Height * 0.9)
                this.Height = (int)(Screen.GetWorkingArea(this).Height * 0.9);

            UpdateTitle();

            ListViewHelper.EnableDoubleBuffer(this.monitorView);
            ListViewHelper.EnableDoubleBuffer(this.eventView);

            monitorView.ListViewItemSorter = sorter;
            eventView.ListViewItemSorter = sorter;
            AddMonitorItem();
        }

        #region ==== Local Data

        /// <summary>
        /// Event object info pushed into a queue via background events and pulled out in foreground by timer.
        /// </summary>
        class EventInfo
        {
            public EventInfo(FileSystemWatcher _watcher, WatcherChangeTypes _eventType, string _path)
            {
                watcher = _watcher;
                eventType = _eventType;
                dateTime = DateTime.Now;
                path = _path;
                oldPath = string.Empty;
            }

            public EventInfo(FileSystemWatcher _watcher, WatcherChangeTypes _eventType, string _path, string _oldPath)
            {
                watcher = _watcher;
                eventType = _eventType;
                dateTime = DateTime.Now;
                path = _path;
                oldPath = _oldPath;
            }

            public WatcherChangeTypes eventType;
            public DateTime dateTime;
            public string path;
            public string oldPath;
            public FileSystemWatcher watcher;
        }
        Queue<EventInfo> eventQueue = new Queue<EventInfo>();

        // [HostProtectionAttribute(SecurityAction.LinkDemand, Synchronization = true)]
        // public static Queue<EventInfo> Synchronized(Queue<EventInfo> eventQueue);

        public class WatchInfo
        {
            public WatchInfo()
            {
                watcher = new System.IO.FileSystemWatcher();
            }

            public System.IO.FileSystemWatcher watcher;
        };

        Image logo;
        bool pause = false;
        ListViewItem focusItem = null;
        int focusSubIdx = -1;
        Color markColor = Color.LightGreen;
        const string markText = "x";
        const string onStr = "a";
        const string offStr = "q";
        const int maxEventItems = 100;

        ListViewColumnSorter sorter = new ListViewColumnSorter(ListViewColumnSorter.SortDataType.eAuto);

        #endregion

        private void closeBtn_Click(object sender, EventArgs e)
        {
            pause = true;
            pauseBtn.Checked = pause;
            this.Close();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            // Keep running even when closed !
#if false
            this.pauseBtn.Checked = this.pause = !this.Visible;
            labelRight.ForeColor = pause ? Color.Red : labelCenter.ForeColor;
#endif
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;        // cancel close, just hide dialog
            base.OnClosing(e);
            this.Visible = false;
        }

        private void UpdateTitle()
        {
            this.labelLeft.Text = Environment.MachineName.ToString();
            // this.labelCenter.Text = MainForm.ProductNameAndVersion();
            this.labelRight.Text = DateTime.Now.ToString("G");
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

        private void exportBtn_Click(object sender, EventArgs e)
        {
            ListViewExt.Export(eventView);
        }

        #region ==== Add/Delete Event
        private Color[] colors = 
        { 
            Color.Brown,
            Color.Yellow,
            Color.Red,
            Color.Tan,
            Color.Blue,
            Color.Pink,
            Color.PaleGoldenrod,
            Color.Green,
            Color.DarkGoldenrod,
            Color.Maroon,
            Color.Orange,
            Color.Gray,
            Color.YellowGreen,
        };
        int colorIdx = 0;

        const int monCol_on = 0;
        const int monCol_file = 1;
        const int monCol_sub = 2;
        const int monCol_pat = 3;
        const int monCol_chg = 4;
        const int monCol_cre = 5;
        const int monCol_del = 6;
        const int monCol_ren = 7;
        const int monCol_col = 8;

        private void addBtn_Click(object sender, EventArgs e)
        {
            AddMonitorItem();
        }

        public void AddMonitorItem()
        {
            ListViewItem item = this.monitorView.Items.Add("");
            while (item.SubItems.Count < 10)
                item.SubItems.Add("");

            item.SubItems[monCol_file].Text = @"C:\";

            // Marlett a=check, q=-, r=X space=box
            item.UseItemStyleForSubItems = false;
            float fontSize = this.Font.Size * 1.5f;
            item.SubItems[monCol_sub].Font = new Font("Marlett", fontSize);
            item.SubItems[monCol_sub].Text = onStr;

            item.SubItems[monCol_pat].Text = "*";

            item.SubItems[monCol_chg].Font = new Font("Marlett", fontSize);
            item.SubItems[monCol_chg].Text = onStr;

            item.SubItems[monCol_cre].Font = new Font("Marlett", fontSize);
            item.SubItems[monCol_cre].Text = onStr;

            item.SubItems[monCol_del].Font = new Font("Marlett", fontSize);
            item.SubItems[monCol_del].Text = onStr;

            item.SubItems[monCol_ren].Font = new Font("Marlett", fontSize);
            item.SubItems[monCol_ren].Text = onStr;

            item.SubItems[monCol_col].BackColor = colors[colorIdx];
            colorIdx = (colorIdx + 1) % colors.Length;
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in monitorView.SelectedItems)
            {
                monitorView.Items.Remove(item);
            }
        }

        private void pauseBtn_Click(object sender, EventArgs e)
        {
            pause = !pause;
            pauseBtn.Checked = pause;
            labelRight.ForeColor = pause ? Color.Red : labelCenter.ForeColor;
        }

        #endregion


        #region ==== Edit Events

        private void ClearFocus()
        {
            if (focusSubIdx != -1 && focusItem != null)
            {
                if (focusItem.SubItems[focusSubIdx].BackColor == markColor)
                    focusItem.SubItems[focusSubIdx].BackColor = this.monitorView.BackColor;
                else if (focusItem.SubItems[focusSubIdx].Text == markText)
                    focusItem.SubItems[focusSubIdx].Text = "";
            }
        }

        private void SetFocus()
        {
            if (focusSubIdx != -1 && focusItem != null)
            {
                if (focusItem.SubItems[focusSubIdx].BackColor == this.monitorView.BackColor)
                    focusItem.SubItems[focusSubIdx].BackColor = markColor;
                else if (focusItem.SubItems[focusSubIdx].Text == "")
                    focusItem.SubItems[focusSubIdx].Text = markText;
            }
        }

        private void monitorView_MouseUp(object sender, MouseEventArgs e)
        {
            ClearFocus();

            Point p = e.Location;
            Control c = sender as Control;
            ListViewHitTestInfo hitInfo = this.monitorView.HitTest(p);
            ListViewItem.ListViewSubItem subItem = hitInfo.SubItem;
            int subCol = subItem == null ? -1 : hitInfo.Item.SubItems.IndexOf(subItem);

            focusItem = hitInfo.Item;
            focusSubIdx = subCol;
            FocusEdit(Keys.None);
        }

        private void FocusEdit(Keys keys)
        {
            ListViewItem.ListViewSubItem subItem = focusSubIdx == -1 ? null : focusItem.SubItems[focusSubIdx];
            Control c = this.monitorView;

            // Magic characters to use with Marlett font
            string[] checks = new string[] { offStr, onStr };  // off, on


            switch (focusSubIdx)
            {
                case monCol_file:
                    {
                        subItem.BackColor = markColor;
                        FolderBox fieldBox = new FolderBox();
                        fieldBox.FileText = subItem.Text;
                        fieldBox.Location = c.PointToScreen(subItem.Bounds.Location);
                        // fieldBox.Show();
                        fieldBox.Width = subItem.Bounds.Width;  // reset width
                        // fieldBox.Visible = false;

                        if (fieldBox.ShowDialog() == DialogResult.OK)
                            subItem.Text = fieldBox.FileText;
                    }
                    break;

                case monCol_pat:
                    {
                        subItem.BackColor = markColor;
                        FolderBox fieldBox = new FolderBox();
                        fieldBox.FileText = subItem.Text;
                        fieldBox.Location = c.PointToScreen(subItem.Bounds.Location);
                        // fieldBox.Show();
                        fieldBox.Width = subItem.Bounds.Width;  // reset width
                        // fieldBox.Visible = false;

                        if (fieldBox.ShowDialog() == DialogResult.OK)
                            subItem.Text = fieldBox.FileText;
                    }
                    break;

                case monCol_sub:
                case monCol_chg:
                case monCol_cre:
                case monCol_del:
                case monCol_ren:
                    {
                        subItem.BackColor = markColor;

                        switch (keys)
                        {
                            case Keys.None:
                                {
                                    ComboField comboField = new ComboField();
                                    comboField.ComboFont = subItem.Font;
                                    comboField.ComboText = subItem.Text;  // must set before choices
                                    comboField.SetChoices(checks);
                                    comboField.Location = c.PointToScreen(subItem.Bounds.Location);
                                    comboField.Show();
                                    comboField.Width = subItem.Bounds.Width;   // reset width
                                    comboField.Visible = false;

                                    if (comboField.ShowDialog() == DialogResult.OK)
                                    {
                                        subItem.Text = comboField.ComboText;
                                    }
                                }
                                break;
                            case Keys.Space:    // Toggle
                                subItem.Text = (subItem.Text == checks[0]) ? checks[1] : checks[0];
                                break;
                            case Keys.Y:        // yes/on/true
                                subItem.Text = checks[1];
                                break;
                            case Keys.N:        // no/off/false
                                subItem.Text = checks[0];
                                break;
                        }
                    }
                    break;

                case monCol_col:
                    {
                        colorDialog.Color = subItem.BackColor;
                        subItem.Text = markText;
                        if (colorDialog.ShowDialog() == DialogResult.OK)
                            subItem.BackColor = colorDialog.Color;
                    }
                    break;
            }

            Monitor(focusItem); 
        }

        ListViewItem monCopyItem = null;

        private void monitorPop_Click(object sender, EventArgs e)
        {
            string text = (sender is ToolStripItem) ?
                ((ToolStripItem)sender).Text :
                ((ToolStripDropDownItem)sender).Text;

            switch (text)
            {
                case "Copy":
                    if (focusItem != null)
                    {
                        monCopyItem = focusItem;
                        monCopyItem.Tag = null;
                        status.Text = "Copied:" + monCopyItem.SubItems[1].Text;
                    }
                    break;
                case "Paste":
                    if (monCopyItem != null)
                    {
                        monitorView.Items.Insert(focusItem.Index, (ListViewItem)monCopyItem.Clone());
                        status.Text = "Pasted:" + monCopyItem.SubItems[1].Text;
                    }
                    break;
            }
        }

        private void monitorView_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    {
                        ClearFocus();
                        switch (e.KeyCode)
                        {
                            case Keys.Left:
                                if (focusSubIdx != -1)
                                    focusSubIdx--;
                                break;
                            case Keys.Right:
                                if (focusSubIdx < monitorView.Columns.Count - 1)
                                {
                                    focusSubIdx++;
                                    if (focusItem == null && monitorView.Items.Count != 0)
                                        focusItem = monitorView.Items[0];
                                }
                                break;
                            case Keys.Up:
                                if (focusItem != null && focusItem.Index > 0)
                                    focusItem = monitorView.Items[focusItem.Index - 1];
                                break;
                            case Keys.Down:
                                if (focusItem != null && focusItem.Index < monitorView.Items.Count-1)
                                    focusItem = monitorView.Items[focusItem.Index + 1];
                                break;
                        }
                        SetFocus();
                    }
                    break;

                case Keys.C:        // copy
                    if (e.Control && focusItem != null)
                    {
                        monCopyItem = focusItem;
                        monCopyItem.Tag = null;
                        status.Text = "Copied:" + monCopyItem.SubItems[1].Text;
                    }
                    break;
                case Keys.V:        // paste
                    if (e.Control && monCopyItem != null)
                    {
                        monitorView.Items.Insert(focusItem.Index, (ListViewItem)monCopyItem.Clone());
                        status.Text = "Pasted:" + monCopyItem.SubItems[1].Text;
                    }
                    break;

                case Keys.Space:    // toggle
                case Keys.Y:        // yes/on/true
                case Keys.N:        // no/off/false
                    FocusEdit(e.KeyCode);   
                    break;

                case Keys.Delete:
                    if (focusItem != null)
                    {
                        ListViewItem dItem = focusItem;

                        if (focusItem.Index > 0)
                            focusItem = monitorView.Items[focusItem.Index - 1];
                        else if (focusItem.Index < monitorView.Items.Count - 1)
                            focusItem = monitorView.Items[focusItem.Index + 1];
                        else
                        {
                            focusItem = null;
                            focusSubIdx = -1;
                        }

                        monitorView.Items.Remove(dItem);
                    }
                    break;
            }
        }

        #endregion


        #region ==== Monitor events

        private void monitorView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            Monitor(e.Item);
        }

        /// <summary>
        /// File system event handlers
        /// </summary>
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            FileSystemWatcher watcher = (FileSystemWatcher)sender;
            // Queue.Synchronized(eventQueue).Enqueue(...);
            lock (eventQueue)
            {
                if (eventQueue.Count < 100)
                {
                    eventQueue.Enqueue(new EventInfo(watcher, e.ChangeType, e.FullPath));
                }
            }
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            FileSystemWatcher watcher = (FileSystemWatcher)sender;
            lock (eventQueue)
            {
                if (eventQueue.Count < 100)
                {
                    eventQueue.Enqueue(new EventInfo(watcher, e.ChangeType, e.FullPath, e.OldFullPath));
                }
            }
        }
   
        private void Monitor(ListViewItem item)
        {
            if (item == null)
                return;

            WatchInfo watchInfo = (item.Tag != null) ? (WatchInfo)item.Tag : new WatchInfo();
            item.Tag = watchInfo;

            watchInfo.watcher.EnableRaisingEvents = false;
            watchInfo.watcher.Changed -= new FileSystemEventHandler(OnChanged);
            watchInfo.watcher.Created -= new FileSystemEventHandler(OnChanged);
            watchInfo.watcher.Deleted -= new FileSystemEventHandler(OnChanged);
            watchInfo.watcher.Renamed -= new RenamedEventHandler(OnRenamed);

            if (item.Checked)
            {
                bool monSub = item.SubItems[monCol_sub].Text == onStr;
                string filter = item.SubItems[monCol_pat].Text;
                string path = item.SubItems[monCol_file].Text;

                watchInfo.watcher.Filter = filter;            //  "*.*";
                string rpath = Path.GetPathRoot(path);
                string dpath = Path.GetDirectoryName(path);
                watchInfo.watcher.Path = (dpath == null) ? rpath : dpath;    // +@"\";         // end with slash

                watchInfo.watcher.IncludeSubdirectories = monSub;
                watchInfo.watcher.NotifyFilter =
                        NotifyFilters.LastAccess
                        | NotifyFilters.LastWrite
                        | NotifyFilters.FileName
                        | NotifyFilters.DirectoryName;

                if (item.SubItems[monCol_chg].Text == onStr)
                    watchInfo.watcher.Changed += new FileSystemEventHandler(OnChanged);

                if (item.SubItems[monCol_cre].Text == onStr)
                    watchInfo.watcher.Created += new FileSystemEventHandler(OnChanged);

                if (item.SubItems[monCol_del].Text == onStr)
                    watchInfo.watcher.Deleted += new FileSystemEventHandler(OnChanged);

                if (item.SubItems[monCol_del].Text == onStr)
                    watchInfo.watcher.Renamed += new RenamedEventHandler(OnRenamed);

                watchInfo.watcher.EnableRaisingEvents = true;
            }
        }

      
        private void eventPopMenu_Click(object sender, EventArgs e)
        {
            string text = (sender is ToolStripItem) ?
             ((ToolStripItem)sender).Text :
             ((ToolStripDropDownItem)sender).Text;

            switch (text)
            {
                case "Launch cmd.exe":
                    if (this.eventView.SelectedItems.Count != 0)
                    {
                        string dir = Path.GetDirectoryName((string)this.eventView.SelectedItems[0].Tag);
                        System.Diagnostics.Process.Start("cmd.exe", "/k cd \"" + dir + "\"");
                        status.Text = "Launched cmd.exe " + dir;
                    }
                    break;
                case "Launch Explorer":
                    if (this.eventView.SelectedItems.Count != 0)
                    {
                        string dir = Path.GetDirectoryName((string)this.eventView.SelectedItems[0].Tag);
                        if (System.Diagnostics.Process.Start("explorer.exe", dir).HasExited)
                            status.Text = "Failed to start explorer on dir of " + dir;
                        else
                            status.Text = "Launched explorer on " + dir;
                    }
                    break;

                case "Clear All":
                    this.eventView.Items.Clear();
                    break;

                case "Clear Selected":
                    this.eventView.BeginUpdate();
                    foreach (ListViewItem item in this.eventView.SelectedItems)
                    {
                        this.eventView.Items.Remove(item);
                    }
                    this.eventView.EndUpdate();
                    break;
            }
        }


        #endregion

        /// <summary>
        /// Pull events from queue and popoulate event List
        /// </summary>
        private void timer_Tick(object sender, EventArgs e)
        {
            this.labelRight.Text = DateTime.Now.ToString("G");

            Type owenerType = Type.GetType("System.Security.Principal.NTAccount");
            //Type owenerType = Type.GetType("System.Security.Principal.SecurityIdentifier");

            if (pause == false)
            {
                // Copy event queue to avoid deadlock.
                Queue<EventInfo> copyEventQueue = null;
                lock (eventQueue)
                {
                    copyEventQueue = new Queue<EventInfo>(eventQueue);
                    eventQueue.Clear();
                }

                if (copyEventQueue.Count == 0)
                    return;

                eventView.BeginUpdate();
              
                while (copyEventQueue.Count != 0)
                {
                    EventInfo eventInfo = copyEventQueue.Dequeue();

                    // Search eventView's tag's for a match on the watcher to get the associated color
                    Color idColor = Color.White;
                    foreach (ListViewItem monItem in monitorView.Items)
                    {
                        if (monItem.Tag != null)
                        {
                            WatchInfo watchInfo = (WatchInfo)monItem.Tag;
                            if (watchInfo.watcher == eventInfo.watcher)
                            {
                                idColor = monItem.SubItems[monCol_col].BackColor;
                                break;
                            }
                        }
                    }

                    ListViewItem item = null;
                    foreach (ListViewItem sItem in this.eventView.Items)
                    {
                        if ((string)sItem.Tag == eventInfo.path && idColor == sItem.SubItems[3].BackColor)
                        {
                            item = sItem;
                            ulong cnt;
                            item.SubItems[1].Tag = eventInfo.dateTime;
                            item.SubItems[2].Tag = cnt = (ulong)item.SubItems[2].Tag + 1;
                            item.SubItems[2].Text = cnt.ToString("#,##0");
                            break;
                        }
                    }

                    if (item == null)
                    {
                        item = this.eventView.Items.Add("");
                        item.Tag = eventInfo.path;

                        while (item.SubItems.Count < 12)
                            item.SubItems.Add("");

                        item.SubItems[0].Text = eventInfo.dateTime.ToString("G");
                        item.SubItems[1].Tag = eventInfo.dateTime;
                        item.SubItems[2].Tag = (ulong)1;
                        item.SubItems[2].Text = "1";

                        // Everytime we add, check if list is overflowing and remove oldest LTime
                        DateTime dt = eventInfo.dateTime;
                        ListViewItem dItem = null;
                        if (eventView.Items.Count > maxEventItems)
                        {
                            foreach (ListViewItem sItem in this.eventView.Items)
                            {
                                if ((DateTime)sItem.SubItems[1].Tag < dt)
                                {
                                    dItem = sItem;
                                }
                            }

                            if (dItem != null)
                                eventView.Items.Remove(dItem);
                        }
                    }

                    item.UseItemStyleForSubItems = false;

                    item.SubItems[1].Text = eventInfo.dateTime.ToString("G");
                    item.SubItems[3].BackColor = idColor;
                    item.SubItems[4].Text = Enum.GetName(typeof(WatcherChangeTypes), eventInfo.eventType);

                    item.SubItems[5].Text = Path.GetDirectoryName(eventInfo.path);
                    item.SubItems[6].Text = Path.GetFileName(eventInfo.path);
                    item.SubItems[7].Text = Path.GetExtension(eventInfo.path);

                    try
                    {
                        FileInfo fileInfo = new FileInfo(eventInfo.path);
                        item.SubItems[8].Text = fileInfo.Length.ToString("#,##0");
                        item.SubItems[9].Text = fileInfo.LastWriteTime.ToString("G");
                        item.SubItems[10].Text = fileInfo.Attributes.ToString();

                        try
                        {
                            // item.SubItems[11].Text = GetOwner(eventInfo.path);
                            // if (item.SubItems[11].Text.Length == 0)
                            {
                                item.SubItems[11].Text = fileInfo.GetAccessControl().GetOwner(owenerType).Value;
                            }
                        }
                        catch (Exception ee)
                        {
                            // DEBUG
                            System.Diagnostics.Debug.WriteLine(ee.Message);
                        }
                    }
                    catch (Exception ee)
                    {
                        // DEBUG
                        System.Diagnostics.Debug.WriteLine(ee.Message);

                        try
                        {
                            DirectoryInfo dirInfo = new DirectoryInfo(eventInfo.path);
                            item.SubItems[8].Text = "";
                            item.SubItems[9].Text = dirInfo.LastWriteTime.ToString("G");
                            item.SubItems[10].Text = dirInfo.Attributes.ToString();
                            item.SubItems[11].Text = dirInfo.GetAccessControl().GetOwner(owenerType).Value;
                        }
                        catch (Exception ee2)
                        {
                            // DEBUG
                            System.Diagnostics.Debug.WriteLine(ee2.Message);
                        }
                    }
                }
                eventView.EndUpdate();

                status.Text = string.Format("{0} Events", eventView.Items.Count);
                this.Text = string.Format("{0} ViewFileEvents {1}",
                        eventView.Items.Count, MainForm.ProductNameAndVersion());
            }
            else
            {
                status.Text = string.Format("[ Paused ]{0} Events", eventView.Items.Count);
            }
        }

        #region ===== Get Owner

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern int GetSecurityInfo(
            IntPtr handle,
            SE_OBJECT_TYPE objectType,
            SECURITY_INFORMATION securityInfo,
            out IntPtr sidOwner,
            out IntPtr sidGroup,
            out IntPtr dacl,
            out IntPtr sacl,
            out IntPtr securityDescriptor);

        [DllImport("advapi32", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool ConvertSidToStringSid(
            IntPtr sid,
            out IntPtr sidString);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr LocalFree(
            IntPtr handle
        );

        enum SE_OBJECT_TYPE
        {
            SE_UNKNOWN_OBJECT_TYPE,
            SE_FILE_OBJECT,
            SE_SERVICE,
            SE_PRINTER,
            SE_REGISTRY_KEY,
            SE_LMSHARE,
            SE_KERNEL_OBJECT,
            SE_WINDOW_OBJECT,
            SE_DS_OBJECT,
            SE_DS_OBJECT_ALL,
            SE_PROVIDER_DEFINED_OBJECT,
            SE_WMIGUID_OBJECT,
            SE_REGISTRY_WOW64_32KEY
        }
        enum SECURITY_INFORMATION
        {
            OWNER_SECURITY_INFORMATION = 1,
            GROUP_SECURITY_INFORMATION = 2,
            DACL_SECURITY_INFORMATION = 4,
            SACL_SECURITY_INFORMATION = 8,
        }

        public string GetOwner(string path)
        {
            FileStream fileStream = null;        
            IntPtr ownerSid;
            IntPtr groupSid;
            IntPtr dacl;
            IntPtr sacl;
            IntPtr securityDescriptor = IntPtr.Zero;

            int returnValue = 0;
            bool success = false;
            string owner = string.Empty;

            try
            {
                fileStream = File.Open(path, FileMode.Open);

                returnValue = GetSecurityInfo(fileStream.Handle, 
                    SE_OBJECT_TYPE.SE_FILE_OBJECT, 
                    SECURITY_INFORMATION.OWNER_SECURITY_INFORMATION 
                    | SECURITY_INFORMATION.DACL_SECURITY_INFORMATION, 
                    out ownerSid, out groupSid, out dacl, out sacl, out securityDescriptor);          

                IntPtr sidString = IntPtr.Zero;
                success = ConvertSidToStringSid(ownerSid, out sidString);
                owner = Marshal.PtrToStringAuto(sidString);
                Marshal.FreeHGlobal(sidString);
            }
            finally
            {
                LocalFree(securityDescriptor);
                fileStream.Close();
            }

            return owner;
        }
        #endregion

        private void helpBtn_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }

        private void helpMenuBtn_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }
 
        private void ShowHelp()
        {
            Assembly a = Assembly.GetExecutingAssembly();
            // string[] resNames = a.GetManifestResourceNames();
            Stream hlpStream = a.GetManifestResourceStream("MagniFile.HelpEvent.html");

            if (hlpStream != null)
            {
                ViewHelp helpDialog = new ViewHelp(hlpStream, logo, this.Text);
                helpDialog.Show(this);
            }
        }
      
    }
}
