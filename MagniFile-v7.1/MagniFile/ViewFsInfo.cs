using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;


namespace MagniFile {
	/// <summary>
	/// General File System Information Dialog.
	/// Display one or more user selected data structures of disk and file system information.
	/// Auto-update and display changes.
	/// 
	/// Author: Dennis Lang 2009
	/// https://landenlabs.com/
	/// 
	/// </summary>
	public partial class ViewFsInfo : Form {
		PrintListView printLV = new PrintListView();
		bool exitOnClose = false;
		Image logo;

		public ViewFsInfo(Image imageLogo) {
			InitializeComponent();

			logo = imageLogo;

			// Prevent viewer from being larger than working area.
			if (this.Height > Screen.GetWorkingArea(this).Height * 0.9)
				this.Height = (int)(Screen.GetWorkingArea(this).Height * 0.9);

			this.Text = "File System Information";

			this.title.Parent = this.titleGlow;
			this.title.Location = new Point(2, 2);

			FillView();
			ListViewColumnSorter lvSorter = new ListViewColumnSorter(ListViewColumnSorter.SortDataType.eAuto);
			lvSorter.SortColumn1 = 2;
			lvSorter.SortColumn2 = 2;
			this.globalStatView.ListViewItemSorter = lvSorter;

			ListViewHelper.EnableDoubleBuffer(this.globalStatView);

			// Sync UI toggle and print settings.
			fitToPageBtn.Checked = printLV.FitToPage = false;

			// this.globalStatView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			// this.globalStatView.AutoResizeColumn(3, ColumnHeaderAutoResizeStyle.HeaderSize);

			// Update tooltip on auto update button
			this.autoBtn.ToolTipText = string.Format("Enable/Disable Auto Update every {0} seconds",
				timer.Interval / 1000.0);
		}

		/// <summary>
		/// Set/Get value indicates if dialog exits when user close it.
		/// </summary>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public bool ExitOnClose {
			get { return exitOnClose; }
			set { exitOnClose = value; }
		}

		/// <summary>
		/// Shut timer off when not visible (ie closed)
		/// </summary>
		protected override void OnVisibleChanged(EventArgs e) {
			base.OnVisibleChanged(e);

			if (this.Visible)
				timer.Start();
			else
				timer.Stop();
		}

		/// <summary>
		/// Close will hide dialog (not visible) allowing state to be saved until reopened.
		/// </summary>
		private void closeBtn_Click(object sender, EventArgs e) {
			// this.Close();
			this.Visible = false;
			if (exitOnClose)
				this.Close();
		}

		protected override void OnFormClosing(FormClosingEventArgs e) {
			e.Cancel = !exitOnClose;        // cancel close, just hide dialog and stop timer.
			base.OnClosing(e);
			this.Visible = false;
		}

		/// <summary>
		/// Reset stats, change based off of new stat sample.
		/// </summary>
		private void resetBtn_Click(object sender, EventArgs e) {
			FillView();
		}

		/// <summary>
		/// Process timer - update stats
		/// </summary>
		private void timer_Tick(object sender, EventArgs e) {
			currentClock.Text = DateTime.Now.ToLongTimeString();
			currentClock.Tag = DateTime.Now;
			UpdateView();
		}

		/// <summary>
		/// Turn on specific stats
		/// </summary>
		/// <param name="stats">Comma separated list of stat choices</param>
		public void ShowStats(string stats) {
			CheckBox[] cbs = new CheckBox[] { sysCk, diskCk, driveCk, cacheCk, statsCk, perfCk };
			foreach (CheckBox cb in cbs)
				cb.Checked = false;

			foreach (string s in stats.Split(',')) {
				foreach (CheckBox cb in cbs) {
					if (string.Compare(cb.Text, stats, true) == 0)
						cb.Checked = true;
				}
			}
			FillView();
		}

		/// <summary>
		/// Fill listView with initial global network statistics.
		/// </summary>
		public void FillView() {
			startClock.Text = DateTime.Now.ToLongTimeString();
			startClock.Tag = DateTime.Now;
			currentClock.Text = DateTime.Now.ToLongTimeString();
			currentClock.Tag = DateTime.Now;

			globalStatView.BeginUpdate();
			globalStatView.Items.Clear();

			if (sysCk.Checked)
				AddSysInfo("Sys", Color.WhiteSmoke);

			if (diskCk.Checked)
				SysManagement.ViewSysInfo(globalStatView, "Disk", Color.Wheat);

			if (driveCk.Checked)
				AddDriveInfo("Drive", Color.FromArgb(220, 220, 220));

			if (cacheCk.Checked)
				AddCacheInfo("Cache", Color.FromArgb(225, 255, 220));

			if (statsCk.Checked)
				AddFileStats("Stats", Color.FromArgb(220, 255, 220));

			if (perfCk.Checked)
				AddPerfInfo("Perf", Color.FromArgb(255, 220, 255));

			globalStatView.EndUpdate();
		}

		public void UpdateView() {
			// globalStatView.BeginUpdate();
			this.Cursor = Cursors.WaitCursor;

			if (sysCk.Checked)
				AddSysInfo("Sys", Color.WhiteSmoke);
			else
				ListViewExt.RemoveItems("Sys", globalStatView);

			if (diskCk.Checked) {
				if (ListViewExt.AnyItems("Disk", globalStatView) == false)
					SysManagement.ViewSysInfo(globalStatView, "Disk", Color.Wheat);
			} else
				ListViewExt.RemoveItems("Disk", globalStatView);

			if (driveCk.Checked)
				AddDriveInfo("Drive", Color.FromArgb(220, 220, 220));
			else
				ListViewExt.RemoveItems("Drive", globalStatView);

			if (cacheCk.Checked)
				AddCacheInfo("Cache", Color.FromArgb(225, 255, 220));
			else
				ListViewExt.RemoveItems("Cache", globalStatView);

			if (statsCk.Checked)
				AddFileStats("Stats", Color.FromArgb(220, 255, 220));
			else
				ListViewExt.RemoveItems("Stats", globalStatView);

			if (perfCk.Checked)
				AddPerfInfo("Perf", Color.FromArgb(255, 220, 255));
			else
				ListViewExt.RemoveItems("Perf", globalStatView);

			this.Cursor = Cursors.Default;
			// globalStatView.EndUpdate();
		}

		private void AddCacheInfo(string f0, Color color) {
			bool alreadyExist = ListViewExt.AnyItems(f0, globalStatView);
			if (alreadyExist)
				return;

			DriveInfo[] diList = DriveInfo.GetDrives();
			int driveNum = 0;
			foreach (DriveInfo di in diList) {
				if (di.DriveType == DriveType.Fixed) {
					string f0d = f0 + " " + di.Name;

					try {
						FileSysInfo.DISK_CACHE_INFORMATION diskCacheInfo = FileSysInfo.GetDiskCacheInformation(driveNum);
						ListViewExt.ReflectToList(diskCacheInfo, false, globalStatView, f0d, color);
						driveNum++;
					} catch { }
				}
			}
		}

		private void AddPerfInfo(string f0, Color color) {
			bool upd = ListViewExt.AnyItems(f0, globalStatView);

			DriveInfo[] diList = DriveInfo.GetDrives();
			int driveNum = 0;
			foreach (DriveInfo di in diList) {
				if (di.DriveType == DriveType.Fixed) {
					string f0d = f0 + " " + di.Name;

					try {
						FileSysInfo.DISK_PERFORMANCE diskPerformance = FileSysInfo.GetDiskPerformance(driveNum);
						if (upd)
							ListViewExt.ReflectUpdList(diskPerformance, true, globalStatView, f0d, color);
						else
							ListViewExt.ReflectToList(diskPerformance, false, globalStatView, f0d, color);
						driveNum++;
					} catch { }
				}
			}
		}

		private void AddFileStats(string f0, Color color) {
			bool upd = ListViewExt.AnyItems(f0, globalStatView);

			DriveInfo[] diList = DriveInfo.GetDrives();
			foreach (DriveInfo di in diList) {
				if (di.DriveType == DriveType.Fixed && di.IsReady) {
					// ListViewItem item = globalStatView.Items.Add(f0);
					string f0d = f0 + " " + di.Name;
					string f0Ntfds = f0 + "NTFS " + di.Name;

					try {
						FileSysInfo.FS_stats[] fsStatsList = FileSysInfo.GetFileSystemStatistics(di.Name);
						Dictionary<string, object> mergedNtfs = FileSysInfo.GetDictionary(fsStatsList[0].ntfs);

						// Merge 'n' lists into one list.
						int fsCnt = fsStatsList.Length;
						for (int fsIdx = 1; fsIdx < fsCnt; fsIdx++) {
							FileSysInfo.Merge(ref fsStatsList[0].fs, fsStatsList[fsIdx].fs);
							if (fsStatsList[0].fs.FileSystemType == 1)
								mergedNtfs = FileSysInfo.Merge(mergedNtfs, FileSysInfo.GetDictionary(fsStatsList[fsIdx].ntfs));
						}

						if (upd) {
							ListViewExt.ReflectUpdList(fsStatsList[0].fs, true, globalStatView, f0d, color);
							if (fsStatsList[0].fs.FileSystemType == 1)
								ListViewExt.UpdList(mergedNtfs, globalStatView, f0Ntfds, color);
						} else {
							ListViewExt.ReflectToList(fsStatsList[0].fs, true, globalStatView, f0d, color);
							if (fsStatsList[0].fs.FileSystemType == 1)
								ListViewExt.AddToList(mergedNtfs, globalStatView, f0Ntfds, color);
						}
					} catch (Exception ee) {
						System.Diagnostics.Debug.WriteLine(ee.Message);
					}
				}
			}
		}

		private void AddSysInfo(string f0, Color color) {
			// Put user code to initialize the page here
			FileSysInfo.SYSTEM_INFO CurrentSystem = new FileSysInfo.SYSTEM_INFO();
			FileSysInfo.GetSystemInfo(CurrentSystem);

			bool alreadyExist = ListViewExt.AnyItems(f0, globalStatView);
			if (!alreadyExist)
				ListViewExt.ReflectToList(CurrentSystem, false, globalStatView, f0, color);
		}

		public void AddDriveInfo(string f0, Color color) {
			bool upd = ListViewExt.AnyItems(f0, globalStatView);

			DriveInfo[] diList = DriveInfo.GetDrives();
			foreach (DriveInfo di in diList) {
				if (di.IsReady) {
					// ListViewItem item = globalStatView.Items.Add(f0);
					string f0d = f0 + " " + di.Name;
					if (upd)
						ListViewExt.ReflectUpdList(di, true, globalStatView, f0d, color);
					else
						ListViewExt.ReflectToList(di, true, globalStatView, f0d, color);
				}
			}
		}

		/// <summary>
		/// Process ToolStrip buttons
		/// </summary>
		private void toolButton_Click(object sender, EventArgs e) {
			ToolStripButton dropItem = sender as ToolStripButton;
			Action(dropItem.Text, sender, e);
		}

		/// <summary>
		/// Process context menu
		/// </summary>
		private void globalMitem_Click(object sender, EventArgs e) {
			ToolStripDropDownItem dropItem = sender as ToolStripDropDownItem;
			// ToolStripDropDownMenu dropMenu = dropItem.DropDown.OwnerItem.Owner as ToolStripDropDownMenu;
			Action(dropItem.Text, sender, e);
		}

		string PrintTitle() {
			return string.Format("{0}\nBegin:{1}   [{2}]   End:{3}\n",
					this.Text,
					((DateTime)this.startClock.Tag).ToString("G"),
					Dns.GetHostName(),
					((DateTime)this.currentClock.Tag).ToString("G"));
		}

		/// <summary>
		/// Common button/menu action execution
		/// </summary>
		private void Action(string cmd, object sender, EventArgs e) {
			switch (cmd) {
				case "Export(CSV)":
					ListViewExt.Export(globalStatView);
					break;

				case "Print":
					printLV.Print(this, globalStatView, PrintTitle());
					break;

				case "Print Preview":
					printLV.PrintPreview(this, globalStatView, PrintTitle());
					break;

				case "Print Setup":
					printLV.PageSetup();
					break;

				case "FitToPage":
					printLV.FitToPage = fitToPageBtn.Checked;
					fitToPageBtn.Image = fitToPageBtn.Checked ? global::MagniFile.Properties.Resources.check_on : global::MagniFile.Properties.Resources.check_off;
					break;

				case "Auto Update":
					autoUpdateToolStripMenuItem.Checked = !timer.Enabled;
					autoBtn.Checked = !timer.Enabled;
					autoBtn.Image = autoBtn.Checked ? global::MagniFile.Properties.Resources.check_on : global::MagniFile.Properties.Resources.check_off;

					if (timer.Enabled) {
						timer.Stop();
						this.currentClock.ForeColor = Color.Pink;
					} else {
						timer.Start();
						this.currentClock.ForeColor = Color.LightGreen;
					}
					break;
				case "Refresh":
					UpdateView();
					break;

				default:
					MessageBox.Show(cmd);
					break;
			}
		}

		/// <summary>
		/// Export current display as csv.
		/// </summary>
		/// <param name="csvfilename"></param>
		/// <returns>true if successful</returns>
		public bool Export(string csvfilename) {
			return ListViewExt.Export(globalStatView, csvfilename);
		}

		/// <summary>
		/// Process keyboard (+/- change font size)
		/// </summary>
		private void globalStatView_KeyUp(object sender, KeyEventArgs e) {
			switch (e.KeyCode) {
				case Keys.Oemplus:
					this.globalStatView.Font = new Font(this.globalStatView.Font.FontFamily,
						this.globalStatView.Font.Size * 1.2f, FontStyle.Regular);
					break;
				case Keys.OemMinus:
					this.globalStatView.Font = new Font(this.globalStatView.Font.FontFamily,
				   this.globalStatView.Font.Size * 0.8f, FontStyle.Regular);
					break;
			}
		}

		/// <summary>
		/// Column header click fires sort.
		/// </summary>
		private void ColumnClick(object sender, ColumnClickEventArgs e) {
			ListView listView = sender as ListView;
			ListViewColumnSorter sorter = listView.ListViewItemSorter as ListViewColumnSorter;

			// Determine if clicked column is already the column that is being sorted.
			if (e.Column == sorter.SortColumn1) {
				// Reverse the current sort direction for this column.
				if (sorter.Order1 == SortOrder.Ascending)
					sorter.Order1 = SortOrder.Descending;
				else
					sorter.Order1 = SortOrder.Ascending;
			} else {
				// Set the column number that is to be sorted; default to ascending.
				sorter.SortColumn1 = e.Column;
				sorter.Order1 = SortOrder.Ascending;
			}

			// Clear old arrows and set new arrow
			foreach (ColumnHeader colHdr in listView.Columns)
				colHdr.ImageIndex = -1;

			listView.Columns[e.Column].ImageIndex = (sorter.Order1 == SortOrder.Ascending) ? 0 : 1;

			// Perform the sort with these new sort options.
			if (listView != null)
				listView.Sort();
		}

		private void ck_Click(object sender, EventArgs e) {
			UpdateView();
		}

		private void helpBtn_Click(object sender, EventArgs e) {
			ShowHelp();
		}

		private void helpMenuBtn_Click(object sender, EventArgs e) {
			ShowHelp();
		}

		private void ShowHelp() {
			Assembly a = Assembly.GetExecutingAssembly();
			// string[] resNames = a.GetManifestResourceNames();
			Stream hlpStream = a.GetManifestResourceStream("MagniFile.HelpDisk.html");

			if (hlpStream != null) {
				ViewHelp helpDialog = new ViewHelp(hlpStream, logo, this.Text);
				helpDialog.Show(this);
			}
		}
	}
}
