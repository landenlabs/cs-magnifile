using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;


namespace MagniFile {
	/// <summary>
	/// List Open Files and Directories Dialog.
	/// Filter to prune list, select to view cluster allocation list.
	/// Auto-update and display changes.
	/// 
	/// Author: Dennis Lang 2009
	/// https://landenlabs.com/
	/// 
	/// </summary>
	public partial class ViewFileList : Form {
		PrintListView printLV = new PrintListView();
		bool exitOnClose = false;
		List<ListViewItem> newItems = new List<ListViewItem>();
		ListViewColumnSorter lvSorter = new ListViewColumnSorter(ListViewColumnSorter.SortDataType.eAlpha);
		const int firstIntervalmsec = 1500;
		const int updateIntervalmsec = 10000;
		Image logo;

		public ViewFileList(Image imageLogo) {
			InitializeComponent();

			logo = imageLogo;

			// Prevent viewer from being larger than working area.
			if (this.Height > Screen.GetWorkingArea(this).Height * 0.9)
				this.Height = (int)(Screen.GetWorkingArea(this).Height * 0.9);

			this.Text = "Open Files - " + MainForm.ProductNameAndVersion();

			this.title.Parent = this.titleGlow;
			this.title.Location = new Point(2, 2);

			startClock.Text = DateTime.Now.ToLongTimeString();
			startClock.Tag = DateTime.Now;

			// Use timer to populate open file list after UI is open and stable.
			timer.Interval = firstIntervalmsec;
			this.abortBtn.Visible = this.progressBar.Visible = false;
			this.filterBtn.Visible = this.filterBox.Visible = false;

			lvSorter.SortColumn1 = lvSorter.SortColumn2 = 0;
			this.fileView.ListViewItemSorter = lvSorter;
			this.modView.ListViewItemSorter = lvSorter;

			ListViewHelper.EnableDoubleBuffer(this.fileView);
			ListViewHelper.EnableDoubleBuffer(this.modView);

			SetMode(ListMode.eOpenFiles);

			// Sync UI toggle and print settings.
			fitToPageBtn.Checked = printLV.FitToPage = false;

			// Update tooltip on auto update button
			this.autoBtn.ToolTipText = string.Format("Enable/Disable Auto Update every {0} seconds",
				updateIntervalmsec / 1000.0);

		}

		#region ==== Close/Open

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

			if (this.Visible) {
				timer.Start();
			} else {
				timer.Stop();
				abort = true;
			}

			autoBtn.Checked = timer.Enabled;
			autoUpdateToolStripMenuItem.Checked = timer.Enabled;
		}

		/// <summary>
		/// Close will hide dialog (not visible) allowing state to be saved until reopened.
		/// </summary>
		private void closeBtn_Click(object sender, EventArgs e) {
			this.Close();
		}

		protected override void OnFormClosing(FormClosingEventArgs e) {
			e.Cancel = !exitOnClose;        // cancel close, just hide dialog and stop timer.
			if (exitOnClose && lvHandles != null) {
				lvHandles.AbortThreads();
			}
			base.OnClosing(e);
			this.Visible = false;
		}

		#endregion

		/// <summary>
		/// Reset stats, change based off of new stat sample.
		/// </summary>
		private void resetBtn_Click(object sender, EventArgs e) {
			startClock.Text = DateTime.Now.ToLongTimeString();
			startClock.Tag = DateTime.Now;
			activeView.Items.Clear();
			statView.Items.Clear();
			UpdateView();
		}

		/// <summary>
		/// Process timer - update stats
		/// </summary>
		private void timer_Tick(object sender, EventArgs e) {
			currentClock.Text = DateTime.Now.ToLongTimeString();
			currentClock.Tag = DateTime.Now;

			if (timer.Interval != updateIntervalmsec) {
				// First timer has fired, popuplate file list and set new timer interval.
				timer.Stop();
				timer.Interval = updateIntervalmsec;
				autoBtn.Checked = false;
				autoUpdateToolStripMenuItem.Checked = false;
			}
			UpdateView();
		}

		bool abort = false;
		private void abortBtn_Click(object sender, EventArgs e) {
			abort = true;
		}

		bool doUpdate = true;
		public void UpdateView() {
			if (doUpdate) {
				doUpdate = false;
				switch (listMode) {
					case ListMode.eOpenFiles:
						UpdateFileView();
						break;
					case ListMode.eProcessModules:
						UpdateModView();
						break;
				}


				highlightNewFiles = true;
				doUpdate = true;
			}
		}

		#region ===== File View


		/// <summary>
		/// Remove item 'lookfor' from 'removeFrom'
		/// </summary>
		/// <param name="lookFor"></param>
		/// <param name="removeFrom"></param>
		private void Remove(ListViewItem lookFor, List<ListViewItem> removeFrom) {
			foreach (ListViewItem lItem in removeFrom) {
				if (lItem.Tag == lookFor.Tag) {
					removeFrom.Remove(lItem);
					return;
				}
			}
		}

		/// <summary>
		/// Find 'hashCode' match in 'search' list and return 'foundItem' and true.
		/// </summary>
		/// <param name="hashCode"></param>
		/// <param name="search"></param>
		/// <param name="foundItem"></param>
		/// <returns></returns>
		private bool Find(int hashCode, ListView search, out ListViewItem foundItem) {
			foreach (ListViewItem item in search.Items) {
				if ((int)item.Tag == hashCode) {
					foundItem = item;
					return true;
				}
			}

			foundItem = null;
			return false;
		}

		/// <summary>
		/// Find matching hashCode item in activeView list and return.
		/// If no match, add new item and set its hashCode value in tag field.
		/// Highlight new items if highlightNewFiles is true.
		/// </summary>
		bool highlightNewFiles = false;
		private ListViewItem FindorAdd(int hashCode) {
			ListViewItem item;
			if (Find(hashCode, activeView, out item))
				return item;

			ListViewItem newItem = activeView.Items.Add("");
			newItem.Tag = hashCode;
			newItems.Add(newItem);

			if (highlightNewFiles)
				newItem.BackColor = Color.LightGreen;

			return newItem;
		}

		/// <summary>
		/// Update list of open files and directories.
		/// Highlight new items in green and dead in red.
		/// </summary>
		bool updatingFileView = false;
		ListViewHandles lvHandles = null;

		public void UpdateFileView() {
			if (updatingFileView)
				return;
			updatingFileView = true;
			currentClock.Text = DateTime.Now.ToLongTimeString();
			currentClock.Tag = DateTime.Now;

			foreach (ListViewItem nItem in newItems)
				nItem.BackColor = Color.White;
			newItems.Clear();

			List<ListViewItem> inactiveItems = new List<ListViewItem>();
			foreach (ListViewItem oItem in fileView.Items)
				inactiveItems.Add(oItem);

			Process[] procList = Process.GetProcesses();
			this.progressBar.Value = 0;
			this.progressBar.Maximum = procList.Length;

			this.fileView.ListViewItemSorter = null;

			lvHandles = new ListViewHandles();
			Dictionary<int, bool> justDirAndFiles = new Dictionary<int, bool>();
			justDirAndFiles.Add(2, true);   // directory
			justDirAndFiles.Add(28, true);  // files

			int procIdx = 0;
			bool firstPass = true;
			this.abort = false;
			this.abortBtn.Visible = this.progressBar.Visible = true;
			this.filterBtn.Visible = this.filterBox.Visible = false;
			this.Cursor = Cursors.WaitCursor;

			foreach (Process process in procList) {
				if (abort)
					break;

				SortedList<int, string> pidList = new SortedList<int, string>();
				pidList.Add(process.Id, process.ProcessName);
				this.status.Text = process.ProcessName;

				int procCnt = lvHandles.GetHandles(pidList, justDirAndFiles, firstPass);
				firstPass = false;
				this.fileView.BeginUpdate();

				foreach (int pid in lvHandles.PidHandleList.Keys) {
					if (abort)
						break;

					SortedList<int, ListViewHandles.HandleInfo> hndList = lvHandles.PidHandleList[pid];

					foreach (int hnd in hndList.Keys) {
						if (abort)
							break;

						ListViewHandles.HandleInfo hndInfo = hndList[hnd];
						if (hndInfo.desc.Length == 0)
							continue;

						int hashCode = hndInfo.GetHashCode();

						ListViewItem item = FindorAdd(hashCode);
						Remove(item, inactiveItems);

						while (item.SubItems.Count < 15)
							item.SubItems.Add("");

						if (hndInfo.error != null)
							continue;

						item.SubItems[2].Text = hndInfo.desc;

						try {
							// any invalid paths may throw an exception.
							item.SubItems[0].Text = Process.GetProcessById(pid).ProcessName;
							item.SubItems[1].Text = pid.ToString();
							item.SubItems[1].Tag = pid;
							item.SubItems[2].Text = Path.GetDirectoryName(hndInfo.desc);
							item.SubItems[2].Tag = hndInfo.desc;
							item.SubItems[3].Text = Path.GetFileName(hndInfo.desc);
							item.SubItems[4].Text = Path.GetExtension(hndInfo.desc).ToLower();
						} catch { }

						try {
							if (hndInfo.handle.ObjectTypeNumber == 28) {
								try {
									if (File.Exists(hndInfo.desc)) {
										FileInfo fileInfo = new FileInfo(hndInfo.desc);
										item.SubItems[5].Text = fileInfo.Length.ToString("#,##0");
										item.SubItems[6].Text = fileInfo.LastWriteTime.ToString("g");
									}
								} catch {
									item.SubItems[5].Text = "";
									item.SubItems[6].Text = "";
								}
							}
							if (hndInfo.handle.ObjectTypeNumber == 2) {
								DirectoryInfo dirInfo = new DirectoryInfo(hndInfo.desc);
								item.SubItems[5].Text = "";
								item.SubItems[6].Text = dirInfo.LastWriteTime.ToString("g");
							}
						} catch {
							item.SubItems[5].Text = "";
							item.SubItems[6].Text = "";
						}

						int c = 7;
						item.SubItems[c++].Text = OpenHandles.ObjectTypeDescription(hndInfo.handle.ObjectTypeNumber);
						item.SubItems[c++].Text = OpenHandles.AccessToString(hndInfo.handle.GrantedAccess);
						item.SubItems[c++].Text = "cluster"; // ToDo - get starting cluster #
						item.SubItems[c++].Text = currentClock.Text;
					}
				}
				this.fileView.EndUpdate();

				procIdx++;
				progressBar.Value = procIdx;
			}

			if (!abort) {
				foreach (ListViewItem dItem in inactiveItems)
					dItem.BackColor = Color.LightPink;
			}

			this.Cursor = Cursors.Default;
			this.progressBar.Value = 0;
			this.abortBtn.Visible = this.progressBar.Visible = false;
			this.filterBtn.Visible = this.filterBox.Visible = true;
			this.fileView.ListViewItemSorter = lvSorter;
			updatingFileView = false;
			this.status.Text = string.Empty;
		}

		bool updatingModView = false;
		public void UpdateModView() {
			if (updatingModView)
				return;

			updatingModView = true;
			currentClock.Text = DateTime.Now.ToLongTimeString();
			currentClock.Tag = DateTime.Now;

			Process[] procList = Process.GetProcesses();
			this.progressBar.Value = 0;
			this.progressBar.Maximum = procList.Length;

			this.modView.ListViewItemSorter = null;

			int procIdx = 0;
			this.abort = false;
			this.abortBtn.Visible = this.progressBar.Visible = true;
			this.filterBtn.Visible = this.filterBox.Visible = false;
			this.modView.Items.Clear();

			foreach (Process process in procList) {
				if (abort)
					break;

				modView.BeginUpdate();

				List<string> modNames = ListViewProcesses.EnumModules(process);
				foreach (string modName in modNames) {
					if (abort)
						break;

					ListViewItem item = modView.Items.Add("");

					while (item.SubItems.Count < 15)
						item.SubItems.Add("");

					item.SubItems[0].Text = process.ProcessName;
					item.SubItems[1].Text = process.Id.ToString();
					item.SubItems[1].Tag = process.Id;
					item.SubItems[2].Text = Path.GetDirectoryName(modName);
					item.SubItems[2].Tag = modName;
					item.SubItems[3].Text = Path.GetFileName(modName);
					item.SubItems[4].Text = Path.GetExtension(modName).ToLower();

					try {
						FileInfo fileInfo = new FileInfo(modName);
						item.SubItems[5].Text = fileInfo.Length.ToString("#,##0");
						item.SubItems[6].Text = fileInfo.LastWriteTime.ToString("g");
						item.SubItems[7].Text = "File";
						item.SubItems[8].Text = fileInfo.Attributes.ToString();
					} catch {
						item.SubItems[5].Text = "";
						item.SubItems[6].Text = "";
						item.SubItems[7].Text = "";
						item.SubItems[8].Text = "";
					}

					item.SubItems[9].Text = "cluster"; // ToDo - get starting cluster #
					item.SubItems[10].Text = currentClock.Text;
				}
				this.modView.EndUpdate();

				procIdx++;
				progressBar.Value = procIdx;
				Application.DoEvents();
			}

			this.progressBar.Value = 0;
			this.abortBtn.Visible = this.progressBar.Visible = false;
			this.filterBtn.Visible = this.filterBox.Visible = true;
			this.modView.ListViewItemSorter = lvSorter;
			updatingModView = false;
		}


		/// <summary>
		/// Filter list by selecting and sorting items which contain the filter text.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void filterBtn_Click(object sender, EventArgs e) {
			if (doUpdate) {
				doUpdate = false;
				string filter = filterBox.Text.ToLower();
				bool timerState = timer.Enabled;
				timer.Enabled = false;

				this.activeView.ListViewItemSorter = null;
				this.activeView.SelectedItems.Clear();

				foreach (ListViewItem item in activeView.Items) {
					bool match = false;
					foreach (ListViewItem.ListViewSubItem sItem in item.SubItems) {
						match = sItem.Text.ToLower().Contains(filter);
						if (match) {
							item.Selected = true;
							break;
						}
					}

					item.SubItems[11].Text = match.ToString();
				}

				this.activeView.ListViewItemSorter = lvSorter;
				lvSorter.Order1 = SortOrder.Descending;
				ColumnClick(activeView, new ColumnClickEventArgs(11));
				ColumnClick(activeView, new ColumnClickEventArgs(11));
				this.activeView.EnsureVisible(0);

				timer.Enabled = timerState;
				doUpdate = true;
			}
		}

		#endregion

		#region ===== Toolbar

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

		public enum ListMode { eNoList, eOpenFiles, eProcessModules };
		private ListMode listMode = ListMode.eNoList;
		private ListView activeView;

		public void SetMode(ListMode newMode) {
			if (newMode == listMode)
				return;

			abort = true;
			listMode = newMode;
			switch (listMode) {
				case ListMode.eOpenFiles:
					this.modeBtn.Text = "Modules";
					this.title.Text = this.titleGlow.Text = "Open File List";
					this.fileView.Visible = true;
					this.modView.Visible = false;
					this.activeView = this.fileView;
					break;
				case ListMode.eProcessModules:
					this.modeBtn.Text = "Open Files";
					this.title.Text = this.titleGlow.Text = "Process Module List";
					this.fileView.Visible = false;
					this.modView.Visible = true;
					this.activeView = this.modView;
					break;
			}
		}

		/// <summary>
		/// Common button/menu action execution
		/// </summary>
		private void Action(string cmd, object sender, EventArgs e) {
			switch (cmd) {
				case "Modules":
					SetMode(ListMode.eProcessModules);
					timer.Interval = firstIntervalmsec;
					timer.Start();
					break;
				case "Open Files":
					SetMode(ListMode.eOpenFiles);
					if (fileView.Items.Count < 100) {
						timer.Interval = firstIntervalmsec;
						timer.Start();
					}
					break;

				case "Clear Selected":
					doUpdate = false;
					activeView.BeginUpdate();
					foreach (ListViewItem item in activeView.SelectedItems)
						activeView.Items.Remove(item);
					activeView.EndUpdate();
					doUpdate = true;
					break;

				case "Invert Selection":
					doUpdate = false;
					activeView.BeginUpdate();
					foreach (ListViewItem item in activeView.Items)
						item.Selected = !item.Selected;
					activeView.EndUpdate();
					doUpdate = true;
					break;

				case "Reset Color":
					foreach (ListViewItem item in activeView.Items)
						item.BackColor = Color.White;
					break;

				case "Export(CSV)":
					ListViewExt.Export(activeView);
					break;

				case "Print":
					printLV.Print(this, activeView, PrintTitle());
					break;

				case "Print Preview":
					printLV.PrintPreview(this, activeView, PrintTitle());
					break;

				case "Print Setup":
					printLV.PageSetup();
					break;

				case "FitToPage":
					printLV.FitToPage = fitToPageBtn.Checked;
					fitToPageBtn.Image = fitToPageBtn.Checked ? global::MagniFile.Properties.Resources.check_on : global::MagniFile.Properties.Resources.check_off;
					break;

				case "Auto Update":
					if (timer.Enabled) {
						timer.Stop();
						this.currentClock.ForeColor = Color.Red;
					} else {
						timer.Start();
						this.currentClock.ForeColor = Color.DarkGreen;
					}
					autoUpdateToolStripMenuItem.Checked = timer.Enabled;
					autoBtn.Checked = timer.Enabled;
					autoBtn.Image = autoBtn.Checked ? global::MagniFile.Properties.Resources.check_on : global::MagniFile.Properties.Resources.check_off;
					break;
				case "Refresh":
					UpdateView();
					break;

				case "Process Information":
					ShowProcessInformation();
					break;

				case "Open Directory":
					ExploreSelectedFile();
					break;

				case "Open File":
					OpenSelectedFile();
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
			return ListViewExt.Export(activeView, csvfilename);
		}

		/// <summary>
		/// Process keyboard (+/- change font size)
		/// </summary>
		private void netView_KeyUp(object sender, KeyEventArgs e) {
			switch (e.KeyCode) {
				case Keys.Oemplus:
					this.activeView.Font = new Font(this.activeView.Font.FontFamily,
					this.activeView.Font.Size * 1.2f, FontStyle.Regular);
					break;
				case Keys.OemMinus:
					this.activeView.Font = new Font(this.activeView.Font.FontFamily,
				   this.activeView.Font.Size * 0.8f, FontStyle.Regular);
					break;
			}
		}

		/// <summary>
		/// Column header click fires sort.
		/// </summary>
		private void ColumnClick(object sender, ColumnClickEventArgs e) {
			ListView listView = sender as ListView;
			ListViewColumnSorter sorter = listView.ListViewItemSorter as ListViewColumnSorter;
			if (sorter != null) {
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

		#region ==== Help 
		/// <summary>
		///  Currently not used
		/// </summary>
		private void helpBtn_Click(object sender, EventArgs e) {
			ShowHelp();
		}

		private void helpMenuBtn_Click(object sender, EventArgs e) {
			ShowHelp();
		}

		private void ShowHelp() {
			Assembly a = Assembly.GetExecutingAssembly();
			Stream hlpStream = a.GetManifestResourceStream("MagniFile.HelpFile.html");

			if (hlpStream != null) {
				ViewHelp helpDialog = new ViewHelp(hlpStream, logo, this.title.Text);
				helpDialog.Show(this);
			}
		}
		#endregion

		/// <summary>
		/// Update lower panel with information for items selected in upper panel.
		/// Display file information, size, dates and allocated clusters.
		/// </summary>
		private void fileView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			// Only display information if selection list is small
			if (doUpdate && activeView.SelectedItems.Count < 10) {
				doUpdate = false;
				statView.Items.Clear();

				foreach (ListViewItem fItem in activeView.SelectedItems) {
					if (statView.Items.Count > 200)
						break;

					if (fItem.SubItems[2].Tag != null) {
						string filePath = (string)fItem.SubItems[2].Tag;
						ListViewItem sItem = null;
						foreach (ListViewItem scanItem in statView.Items) {
							if (scanItem.SubItems[0].Text == filePath) {
								sItem = scanItem;
								break;
							}
						}

						if (sItem == null)
							sItem = statView.Items.Add(filePath);

						try {
							FileInfo fileInfo = new FileInfo(filePath);
							Int64 firstCluster = 0;

							sItem.SubItems.Add(fileInfo.Length.ToString("#,##0"));
							sItem.SubItems.Add(fileInfo.CreationTime.ToString("G"));
							sItem.SubItems.Add(fileInfo.LastAccessTime.ToString("G"));
							sItem.SubItems.Add(fileInfo.LastWriteTime.ToString("G"));
							sItem.SubItems.Add(fileInfo.Attributes.ToString());
							sItem.SubItems.Add(firstCluster.ToString("#,##0"));

							// Add file allocation info.
							FileAllocation(fileInfo);
						} catch { }

					}
				}

				doUpdate = true;
			} else {
				statView.Items.Clear();
				statView.Items.Add(activeView.SelectedItems.Count.ToString() + " selected");
				return;
			}
		}

		/// <summary>
		/// Get file cluster allocation list and add to lower panel.
		/// </summary>
		private void FileAllocation(FileInfo fileInfo) {
			try {
				Array vcn_lcn = FileSysInfo.GetFileMap(fileInfo.FullName);

				int len0 = vcn_lcn.GetLength(0);
				int len1 = vcn_lcn.GetLength(1);

				Int64 prevVcn = 0;
				Point firstP = Point.Empty;
				Point lastP = Point.Empty;

				ListViewItem item;
				item = statView.Items.Add("#Allocations:" + len0.ToString());

				for (int fragIdx = 0; fragIdx < len0; fragIdx++) {
					// for (int y = 0; y < len1; y++)
					Int64 vcn = (Int64)vcn_lcn.GetValue(fragIdx, 0);
					Int64 lcn = (Int64)vcn_lcn.GetValue(fragIdx, 1);
					Int64 len = vcn - prevVcn;
					prevVcn = vcn;

					if (lcn > 0) {
						if (statView.Items.Count < 200) {
							item = statView.Items.Add("");
							while (item.SubItems.Count < 8)
								item.SubItems.Add("");
							item.SubItems[0].Text = (fragIdx + 1).ToString();
							item.SubItems[6].Text = lcn.ToString("#,##0");
							item.SubItems[7].Text = len.ToString("#,##0");
						} else if (statView.Items.Count == 200)   // limit list length
						  {
							item = statView.Items.Add("Too many clusters");
						}
					}
				}
			} catch (Exception ee) {
				if (ee.Message == "38" || fileInfo.Length < 4096)
					statView.Items.Add("No clusters");
			}
		}

		/// <summary>
		/// Show detail information about selected process.
		/// </summary>
		private void ShowProcessInformation() {
			try {
				if (activeView.SelectedItems.Count != 0) {
					int pid = -1;
					pid = (int)activeView.SelectedItems[0].SubItems[1].Tag;
					ViewProcessInfo viewProcessInfo = new ViewProcessInfo(pid);
					viewProcessInfo.Show();
				} else {
					Point p = this.activeView.PointToClient(System.Windows.Forms.Control.MousePosition);
					ListViewHitTestInfo hitInfo = activeView.HitTest(p);
					if (hitInfo.Item != null && hitInfo.Item.SubItems[1].Tag != null) {
						int pid = (int)hitInfo.Item.SubItems[1].Tag;
						ViewProcessInfo viewProcessInfo = new ViewProcessInfo(pid);
						viewProcessInfo.Show();
					}
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void ExploreSelectedFile() {
			try {
				if (activeView.SelectedItems.Count != 0) {
					string path = (string)activeView.SelectedItems[0].SubItems[2].Tag;
					LaunchExplorer(path);
				} else {
					Point p = this.activeView.PointToClient(System.Windows.Forms.Control.MousePosition);
					ListViewHitTestInfo hitInfo = activeView.HitTest(p);
					if (hitInfo.Item != null && hitInfo.Item.SubItems[2].Tag != null) {
						string path = (string)hitInfo.Item.SubItems[2].Tag;
						LaunchExplorer(path);
					}
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void OpenSelectedFile() {
			try {
				if (activeView.SelectedItems.Count != 0) {
					string path = (string)activeView.SelectedItems[0].SubItems[2].Tag;
					System.Diagnostics.Process.Start(path);
				} else {
					Point p = this.activeView.PointToClient(System.Windows.Forms.Control.MousePosition);
					ListViewHitTestInfo hitInfo = activeView.HitTest(p);
					if (hitInfo.Item != null && hitInfo.Item.SubItems[2].Tag != null) {
						string path = (string)hitInfo.Item.SubItems[2].Tag;
						System.Diagnostics.Process.Start(path);
					}
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void LaunchExplorer(string path) {
			System.Diagnostics.Process.Start("explorer.exe", "/select,\"" + path + "\"");
		}

		private void LaunchCmd(string path) {
			string dir = Path.GetDirectoryName(path);
			System.Diagnostics.Process.Start("cmd.exe", "/k cd \"" + dir + "\"");
		}

	}
}
