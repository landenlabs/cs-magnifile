
#define THREAD_DNS


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace MagniFile {
	/// <summary>
	/// General Network Information Dialog.
	/// Display one or more user selected data structures of network information.
	/// Auto-update and display changes.
	/// 
	/// Author: Dennis Lang 2009
	/// https://landenlabs.com/
	/// 
	/// </summary>
	public partial class ViewNetInfo : Form {
		#region ==== Data Members
		PrintListView printLV = new PrintListView();
		bool exitOnClose = false;
		List<ListViewItem> newItems = new List<ListViewItem>();
		ListViewColumnSorter lvSorter = new ListViewColumnSorter(ListViewColumnSorter.SortDataType.eAuto);
		Image logo = null;
		#endregion

		public ViewNetInfo(Image imageLogo) {
			InitializeComponent();

			logo = imageLogo;

			// Prevent viewer from being larger than working area.
			if (this.Height > Screen.GetWorkingArea(this).Height * 0.9)
				this.Height = (int)(Screen.GetWorkingArea(this).Height * 0.9);

			this.Text = "Network Information - " + MainForm.ProductNameAndVersion();

			this.title.Parent = this.titleGlow;
			this.title.Location = new Point(2, 2);

			startClock.Text = DateTime.Now.ToLongTimeString();
			startClock.Tag = DateTime.Now;

			UpdateView();

			lvSorter.SortColumn1 = 2;
			lvSorter.SortColumn2 = 2;
			this.netView.ListViewItemSorter = lvSorter;
			this.statView.ListViewItemSorter = lvSorter;

			// Default all filters on.
			for (int ckIdx = 0; ckIdx < ckFilters.Items.Count; ckIdx++)
				ckFilters.SetItemChecked(ckIdx, true);

			ListViewHelper.EnableDoubleBuffer(this.netView);

			// Sync UI toggle and print settings.
			fitToPageBtn.Checked = printLV.FitToPage = false;

			// this.netView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			// this.netView.AutoResizeColumn(3, ColumnHeaderAutoResizeStyle.HeaderSize);

			// Update tooltip on auto update button
			this.autoBtn.ToolTipText = string.Format("Enable/Disable Auto Update every {0} seconds",
				timer.Interval / 1000.0);

			timer.Start();
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

#if false
            // Stop when minimized or closed.
            if (this.Visible)
                timer.Start();
            else
                timer.Stop();
#endif
		}

		/// <summary>
		/// Close will hide dialog (not visible) allowing state to be saved until reopened.
		/// </summary>
		private void closeBtn_Click(object sender, EventArgs e) {
			// this.Visible = false;
			// if (exitOnClose)
			this.Close();
		}

		protected override void OnFormClosing(FormClosingEventArgs e) {
			if (exitOnClose && dnsResolver != null)
				dnsResolver.Stop();
			e.Cancel = !exitOnClose;        // cancel close, just hide dialog and stop timer.
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
			netView.Items.Clear();
			statView.Items.Clear();
			UpdateView();
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
		/// Update all panels (network connections and network stats)
		/// </summary>
		public void UpdateView() {
			UpdateConnView(false);
			UpdateStatView();
			UpdateCountView();
		}

		/// <summary>
		/// Update Network Statistics row.  Find match and update 'change'.
		/// </summary>
		public ListViewItem UpdateStat(string id, string desc, long val) {
			string tag = id + desc;

			ListViewItem item = null;
			foreach (ListViewItem scanItem in statView.Items) {
				if ((string)scanItem.Tag == tag) {
					item = scanItem;
					break;
				}
			}

			if (item == null) {
				item = statView.Items.Add(id);
				while (item.SubItems.Count < 6)
					item.SubItems.Add("");
				item.Tag = tag;
				item.SubItems[1].Text = desc;
				item.SubItems[2].Text = val.ToString("#,##0");
				item.SubItems[2].Tag = val;
			}

			long n = (long)item.SubItems[2].Tag;
			item.SubItems[3].Text = (val - n).ToString("#,##0");
			item.SubItems[4].Text = val.ToString("#,##0");
			long maxVal = item.SubItems[5].Tag != null ? (long)item.SubItems[5].Tag : 0;
			item.SubItems[5].Tag = maxVal = Math.Max(maxVal, val);
			item.SubItems[5].Text = maxVal.ToString("#,##0");

			return item;
		}

		/// <summary>
		/// Update Network Statistics view.
		/// </summary>
		public void UpdateStatView() {
			IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
			IPGlobalStatistics ipStat = properties.GetIPv4GlobalStatistics();
			IcmpV4Statistics icmpStat = properties.GetIcmpV4Statistics();
			TcpConnectionInformation[] tcpConnInfoList = properties.GetActiveTcpConnections();

			TcpStatistics tcpStat = properties.GetTcpIPv4Statistics();
			string id = "Tcp";
			UpdateStat(id, "Segments Received", tcpStat.SegmentsReceived);
			UpdateStat(id, "Segments Sent", tcpStat.SegmentsSent);
			UpdateStat(id, "Segments Resent", tcpStat.SegmentsResent);
			UpdateStat(id, "Receive Errors", tcpStat.ErrorsReceived);

			UdpStatistics udpStat = properties.GetUdpIPv4Statistics();
			id = "Udp";
			UpdateStat(id, "Datagrams Received", udpStat.DatagramsReceived);
			UpdateStat(id, "Datagrams Sent", udpStat.DatagramsSent);
			UpdateStat(id, "Datagrams Discarded", udpStat.IncomingDatagramsDiscarded);
			UpdateStat(id, "Incoming Errors", udpStat.IncomingDatagramsWithErrors);
		}


		/// </summary>
		public void UpdateCountView() {
			string id = "StatDsp";
			UpdateStat(id, "#Tcp",
					cntDspProt[(int)Win32.NetworkProtocol.Tcp] +
					cntDspProt[(int)Win32.NetworkProtocol.Tcp6]);
			UpdateStat(id, "#Udp",
				   cntDspProt[(int)Win32.NetworkProtocol.Udp] +
				   cntDspProt[(int)Win32.NetworkProtocol.Udp6]);
			UpdateStat(id, "#Established",
					cntDspState[(int)Win32.MibTcpState.Established]);
			UpdateStat(id, "#Listening",
					cntDspState[(int)Win32.MibTcpState.Listening]);
			UpdateStat(id, "#Close*",
					cntDspState[(int)Win32.MibTcpState.Closed] +
					cntDspState[(int)Win32.MibTcpState.CloseWait] +
					cntDspState[(int)Win32.MibTcpState.Closing]);
			UpdateStat(id, "#Fin*",
					cntDspState[(int)Win32.MibTcpState.FinWait1] +
					cntDspState[(int)Win32.MibTcpState.FinWait2]);

			id = "StatAll";
			UpdateStat(id, "#Tcp",
					cntAllProt[(int)Win32.NetworkProtocol.Tcp] +
					cntAllProt[(int)Win32.NetworkProtocol.Tcp6]);
			UpdateStat(id, "#Udp",
				   cntAllProt[(int)Win32.NetworkProtocol.Udp] +
				   cntAllProt[(int)Win32.NetworkProtocol.Udp6]);
			UpdateStat(id, "#Established",
					cntAllState[(int)Win32.MibTcpState.Established]);
			UpdateStat(id, "#Listening",
					cntAllState[(int)Win32.MibTcpState.Listening]);
			UpdateStat(id, "#Close*",
					cntAllState[(int)Win32.MibTcpState.Closed] +
					cntAllState[(int)Win32.MibTcpState.CloseWait] +
					cntAllState[(int)Win32.MibTcpState.Closing]);
			UpdateStat(id, "#Fin*",
					cntAllState[(int)Win32.MibTcpState.FinWait1] +
					cntAllState[(int)Win32.MibTcpState.FinWait2]);
		}

		#region ===== Connections

		private void Remove(ListViewItem item, List<ListViewItem> list) {
			foreach (ListViewItem lItem in list) {
				if (lItem.Tag == item.Tag) {
					list.Remove(lItem);
					return;
				}
			}
		}

		private bool Find(int hashCode, ListView lv, out ListViewItem foundItem) {
			foreach (ListViewItem item in lv.Items) {
				if ((int)item.Tag == hashCode) {
					foundItem = item;
					return true;
				}
			}

			foundItem = null;
			return false;
		}

		private ListViewItem FindorAdd(int hashCode) {
			ListViewItem item;
			if (Find(hashCode, netView, out item)) {
				item.BackColor = netView.BackColor;
				return item;
			}

			ListViewItem newItem = netView.Items.Add("");
			newItem.Tag = hashCode;
#if false
            newItems.Add(newItem);
#endif
			newItem.BackColor = Color.LightGreen;

			return newItem;
		}

		public class DnsResolver {
			Dictionary<long, string> IpToHostDictionary = new Dictionary<long, string>();
			Queue<IPAddress> resolveIpQueue = new Queue<IPAddress>();
			Semaphore more = new Semaphore(1, 1);
			bool abort;
#if THREAD_DNS
			Thread dnsThread;
			public DnsResolver() {
				dnsThread = new Thread(new ThreadStart(this.Run));
				dnsThread.Start();
			}

			~DnsResolver() {
				Stop();
			}

			public void Stop() {
				if (dnsThread.IsAlive) {
					abort = true;
					try {
						more.Release();
						dnsThread.Abort();
						dnsThread.Join();
					} catch { }
				}
			}
#endif

			public void Run() {
				abort = false;
				more.WaitOne();

				while (false == abort) {
					IPAddress ipAddress = IPAddress.None;

					lock (resolveIpQueue) {
						if (resolveIpQueue.Count != 0)
							ipAddress = resolveIpQueue.Dequeue();
					}

					if (ipAddress != IPAddress.None)
						IpToHostName(ipAddress);

					more.WaitOne();
				}
			}

			private string IpToHostName(IPAddress ipAddress) {
				string hostname;
				if (IpToHostDictionary.TryGetValue(ipAddress.Address, out hostname))
					return hostname;

				hostname = ipAddress.ToString();
				try {
					IPHostEntry ipentry = Dns.Resolve(ipAddress.ToString());
					hostname = ipentry.HostName;
				} catch { }

				lock (IpToHostDictionary) {
					IpToHostDictionary[ipAddress.Address] = hostname;
				}

				return hostname;
			}

			public string HostName(IPAddress ipAddress) {
				string hostname = string.Empty;
				try {
					if (IpToHostDictionary.TryGetValue(ipAddress.Address, out hostname))
						return hostname;
				} catch {
					return hostname;
				}

				hostname = ipAddress.ToString();
#if THREAD_DNS
				lock (resolveIpQueue) {
					resolveIpQueue.Enqueue(ipAddress);
					try {
						more.Release();
					} catch { }
				}
#else
                hostname = IpToHostName(ipAddress);
#endif
				return hostname;
			}
		}

		DnsResolver dnsResolver = new DnsResolver();
		int[] cntAllProt = new int[10];
		int[] cntAllState = new int[15];
		int[] cntDspProt = new int[10];
		int[] cntDspState = new int[15];

		// enum ConnGroup { Process=connColProc, RemoteHost=connColRh, Protocol=connColProt, State=connColSta };
		// ConnGroup connGroup = ConnGroup.Process;

		const int connColProc = 0;
		const int connColPid = 1;
		const int connColLa = 2;
		const int connColLp = 3;
		const int connColRa = 4;
		const int connColRh = 5;
		const int connColRp = 6;
		const int connColProt = 7;
		const int connColSta = 8;
		const int connColStm = 9;
		const int connColCtm = 10;
		const int connColDur = 11;
		const int connColLast = 12;

		/// <summary>
		/// Get hostname for ip, use local cache to improve performance and avoid side traffic.
		/// </summary>
		public void UpdateConnView(bool filtering) {
			bool[] ifState = new bool[15].Select(b => true).ToArray();
			bool[] ifProt = new bool[10].Select(b => true).ToArray();
			ArrayUtil.SetAllValues(cntAllProt, 0);
			ArrayUtil.SetAllValues(cntAllState, 0);
			ArrayUtil.SetAllValues(cntDspProt, 0);
			ArrayUtil.SetAllValues(cntDspState, 0);

			SortedList<string, ListViewGroup> objGroup = new SortedList<string, ListViewGroup>();

			// bool showHost = ckFilters.GetItemCheckState((int)ckFilter.Remote_Host) == CheckState.Checked;
			bool showHost = true;
			bool showDead = !filtering &&
				ckFilters.GetItemCheckState((int)ckFilter.Show_Dead) == CheckState.Checked;
			ifProt[(int)Win32.NetworkProtocol.Tcp] =
				ifProt[(int)Win32.NetworkProtocol.Tcp6] =
				ckFilters.GetItemCheckState((int)ckFilter.Tcp) == CheckState.Checked;
			ifProt[(int)Win32.NetworkProtocol.Udp] =
				ifProt[(int)Win32.NetworkProtocol.Udp6] =
				ckFilters.GetItemCheckState((int)ckFilter.Udp) == CheckState.Checked;

			ifState[(int)Win32.MibTcpState.Listening] =
				ckFilters.GetItemCheckState((int)ckFilter.Listening) == CheckState.Checked;
			ifState[(int)Win32.MibTcpState.Established] =
				ckFilters.GetItemCheckState((int)ckFilter.Established) == CheckState.Checked;
			ifState[(int)Win32.MibTcpState.Closed] =
			ifState[(int)Win32.MibTcpState.CloseWait] =
			ifState[(int)Win32.MibTcpState.Closing] =
				ckFilters.GetItemCheckState((int)ckFilter.ClosedAny) == CheckState.Checked;
			ifState[(int)Win32.MibTcpState.FinWait1] =
			ifState[(int)Win32.MibTcpState.FinWait2] =
				ckFilters.GetItemCheckState((int)ckFilter.FinAny) == CheckState.Checked;
			ifState[(int)Win32.MibTcpState.TimeWait] =
				ckFilters.GetItemCheckState((int)ckFilter.TimeWait) == CheckState.Checked;

			//  Parse file words
			//      word[;word]...
			//  Where word can start with ! to mark exclusion
			//
			string[] filterWords = filterText.Text.ToLower().Split(';');
			List<string> filterNotWords = new List<string>();

			for (int widx = 0; widx < filterWords.Length; widx++) {
				string word = filterWords[widx];
				if (word.Length > 0 && word[0] == '!') {
					filterNotWords.Add(word.Substring(1));
					filterWords[widx] = string.Empty;
				}
			}

			DateTime now = DateTime.Now;
			currentClock.Text = now.ToLongTimeString();
			currentClock.Tag = now;

			this.netView.ListViewItemSorter = null;
			this.netView.BeginUpdate();

			this.netView.Groups.Clear();

#if false
            foreach (ListViewItem nItem in newItems)
                nItem.BackColor = Color.White;
            newItems.Clear();
            List<ListViewItem> inactiveItems = new List<ListViewItem>();

            foreach (ListViewItem oItem in netView.Items)
            {
                oItem.Group = null;
                inactiveItems.Add(oItem);
            }
#endif

			Dictionary<int, List<Win32.NetworkConnection>> netConnections = Win32.GetNetworkConnections();

			foreach (int k in netConnections.Keys) {
				List<Win32.NetworkConnection> netList = netConnections[k];

				foreach (Win32.NetworkConnection netconn in netList) {
					cntAllProt[(int)netconn.Protocol]++;
					cntAllState[(int)netconn.State]++;

					if (ifProt[(int)netconn.Protocol] == false)
						continue;

					if (ifState[(int)netconn.State] == false)
						continue;

					int hashCode = netconn.GetHashCode();
					ListViewItem item = FindorAdd(hashCode);
					item.Group = null;
#if false
                    Remove(item, inactiveItems);
#endif

					while (item.SubItems.Count < connColLast)
						item.SubItems.Add("");

					try {
						Process proc = Process.GetProcessById(netconn.Pid);
						item.SubItems[connColProc].Text = proc.ProcessName;
					} catch {
						item.SubItems[connColProc].Text = "<unknown>";
					}
					item.SubItems[connColPid].Text = netconn.Pid.ToString();
					item.SubItems[connColPid].Tag = netconn.Pid;

					item.SubItems[connColLa].Text = netconn.Local.Address.ToString();
					item.SubItems[connColLp].Text = netconn.Local.Port.ToString();

					if (netconn.Remote != null) {
						item.SubItems[connColRa].Text = netconn.Remote.Address.ToString();
						item.SubItems[connColRh].Text = showHost ?
							dnsResolver.HostName(netconn.Remote.Address) : string.Empty;
						item.SubItems[connColRp].Text = netconn.Remote.Port.ToString();
					}

					item.SubItems[connColProt].Text = netconn.Protocol.ToString();
					item.SubItems[connColSta].Text = netconn.State.ToString();

					// Start Time
					if (item.SubItems[connColStm].Text.Length == 0) {
						item.SubItems[connColStm].Text = currentClock.Text;
						item.SubItems[connColStm].Tag = now;
					}

					// Close time
					item.SubItems[connColCtm].Text = currentClock.Text;
					item.SubItems[connColCtm].Tag = now;

					// Duration
					DateTime startTime = (DateTime)item.SubItems[connColStm].Tag;
					TimeSpan duration = (now - startTime);
					item.SubItems[connColDur].Text = (duration.TotalSeconds / 60.0).ToString("#,##0.#");

					#region ----- Filter text 
					string itemText = string.Empty;
					foreach (ListViewItem.ListViewSubItem sItem in item.SubItems)
						itemText += sItem.Text.ToLower() + " ";
					bool filterMatch = filterWords[0].Length == 0;
					if (!filterMatch) {
						foreach (string word in filterWords) {
							filterMatch = (word.Length > 0) && itemText.Contains(word);
							if (filterMatch)
								break;
						}
					}
					if (filterMatch) {
						foreach (string notWord in filterNotWords) {
							if ((filterMatch = !itemText.Contains(notWord)) == false)
								break;
						}
					}
					if (filterMatch) {
						cntDspProt[(int)netconn.Protocol]++;
						cntDspState[(int)netconn.State]++;

						// Deal with any GroupBy settings.
						if (connGroupCol != -1) {
							ListViewGroup grp = null;
							string grpBy = item.SubItems[connGroupCol].Text;
							if (objGroup.TryGetValue(grpBy, out grp) == false) {
								grp = netView.Groups.Add(grpBy,
									string.Format("{0}:{1}", netView.Columns[connGroupCol].Text, grpBy));
								objGroup.Add(grpBy, grp);
							}
							item.Group = grp;
						}
					} else {
						netView.Items.Remove(item);
					}
					#endregion

				}
			}

			// Deal with Dead connections.
			// ToDo - Dead connections are not filtered!
			int activeCnt = 0;
			foreach (ListViewItem dItem in netView.Items) {
				if ((DateTime)dItem.SubItems[connColCtm].Tag == now) {
					activeCnt++;
				} else {
					if (showDead)
						dItem.BackColor = Color.LightPink;
					else
						this.netView.Items.Remove(dItem);
				}
			}

			this.Text = string.Format("{0} Network Information - {1}",
				activeCnt, MainForm.ProductNameAndVersion());

			this.netView.EndUpdate();
			this.netView.ListViewItemSorter = lvSorter;
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
				this.title.Text,
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
					ListViewExt.Export(netView);
					break;

				case "Print":
					printLV.Print(this, netView, PrintTitle());
					break;

				case "Print Preview":
					printLV.PrintPreview(this, netView, PrintTitle());
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
						this.currentClock.ForeColor = Color.Red;
					} else {
						timer.Start();
						this.currentClock.ForeColor = Color.DarkGreen;
					}
					break;
				case "Refresh":
					UpdateView();
					break;

				case "Clear All":
					this.netView.Items.Clear();
					break;
				case "Clear Selected":
					this.netView.BeginUpdate();
					foreach (ListViewItem item in netView.SelectedItems) {
						this.netView.Items.Remove(item);
					}
					this.netView.EndUpdate();
					break;
				case "Clear Dead":
					this.netView.BeginUpdate();
					this.netView.SelectedItems.Clear();
					foreach (ListViewItem item in netView.Items) {
						if (item.BackColor == Color.LightPink) {
							item.Selected = true;
						}
					}
					this.netView.EndUpdate();
					Action("Clear Selected", null, EventArgs.Empty);
					break;

				case "Process Information":
					ShowProcessInformation();
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
			return ListViewExt.Export(netView, csvfilename);
		}

		/// <summary>
		/// Process keyboard (+/- change font size)
		/// </summary>
		private void netView_KeyUp(object sender, KeyEventArgs e) {
			switch (e.KeyCode) {
				case Keys.Oemplus:
					this.netView.Font = new Font(this.netView.Font.FontFamily,
					this.netView.Font.Size * 1.2f, FontStyle.Regular);
					break;
				case Keys.OemMinus:
					this.netView.Font = new Font(this.netView.Font.FontFamily,
				   this.netView.Font.Size * 0.8f, FontStyle.Regular);
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
		#endregion

		#region ==== Help Dialog
		private void helpBtn_Click(object sender, EventArgs e) {
			ShowHelp();
		}

		private void helpMenuBtn_Click(object sender, EventArgs e) {
			ShowHelp();
		}

		private void ShowHelp() {
			Assembly a = Assembly.GetExecutingAssembly();
			// string[] resNames = a.GetManifestResourceNames();
			Stream hlpStream = a.GetManifestResourceStream("MagniFile.HelpNet.html");

			if (hlpStream != null) {
				ViewHelp helpDialog = new ViewHelp(hlpStream, logo, this.title.Text);
				helpDialog.Show(this);
			}
		}
		#endregion

		/// <summary>
		/// Show detail information about selected process.
		/// </summary>
		private void ShowProcessInformation() {
			if (netView.SelectedItems.Count != 0) {
				int pid = -1;
				pid = (int)netView.SelectedItems[0].SubItems[1].Tag;
				ViewProcessInfo viewProcessInfo = new ViewProcessInfo(pid);
				viewProcessInfo.Show();
			} else {
				Point p = this.netView.PointToClient(System.Windows.Forms.Control.MousePosition);
				ListViewHitTestInfo hitInfo = netView.HitTest(p);
				if (hitInfo.Item != null && hitInfo.Item.SubItems[1].Tag != null) {
					int pid = (int)hitInfo.Item.SubItems[1].Tag;
					ViewProcessInfo viewProcessInfo = new ViewProcessInfo(pid);
					viewProcessInfo.Show();
				}
			}
		}

		private void checkBoxes_MouseEnter(object sender, EventArgs e) {
			CheckedListBox ckbox = (CheckedListBox)sender;
			ckbox.Height = clearFilterBtn.Height * ckbox.Items.Count;
		}

		private void checkBoxes_MouseLeave(object sender, EventArgs e) {
			CheckedListBox ckbox = (CheckedListBox)sender;
			ckbox.Height = clearFilterBtn.Height;
		}

		enum ckFilter { Tcp = 0, Udp, Show_Dead, Listening, Established, ClosedAny, FinAny, TimeWait };
		private void checkBoxes_SelectedIndexChanged(object sender, EventArgs e) {
			UpdateConnView(true);
		}

		private void filterText_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter)
				UpdateConnView(true);
		}

		private void clearFilterBtn_Click(object sender, EventArgs e) {
			this.filterText.Text = string.Empty;
			UpdateConnView(false);
		}

		int connGroupCol = -1;
		private void viewGrpMenu_Click(object sender, EventArgs e) {
			ToolStripDropDownItem dropItem = (ToolStripDropDownItem)sender;
			ToolStripDropDownMenu dropMenu = dropItem.DropDown.OwnerItem.Owner as ToolStripDropDownMenu;

			foreach (ToolStripDropDownItem item in dropMenu.Items)
				item.Image = Properties.Resources.check_off;

			dropItem.Image = Properties.Resources.check_on;
			if (connGroupCol != (int)dropItem.Tag) {
				connGroupCol = (int)dropItem.Tag;
				UpdateConnView(false);
			}
		}

		private void exportCSVToolStripMenuItem_Click(object sender, EventArgs e) {
			ListViewExt.Export(statView);
		}

		private void loadSnapshotToolStripMenuItem_Click(object sender, EventArgs e) {
			ListViewExt.Load(statView);
		}

		private void saveSnapshotToolStripMenuItem_Click(object sender, EventArgs e) {
			ListViewExt.Save(statView);
		}
	}

	public static class ArrayUtil {
		public static T[] SetAllValues<T>(this T[] array, T value) where T : struct {
			for (int i = 0; i < array.Length; i++)
				array[i] = value;

			return array;
		}
	}

}
