using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;


namespace MagniFile {
	/// <summary>
	/// Display all available process information (use reflection)
	/// 
	/// Author: Dennis Lang 2009
	/// https://landenlabs.com/
	/// </summary>
	public partial class ViewProcessInfo : Form {
		public ViewProcessInfo(int _pid) {
			InitializeComponent();

			// Prevent viewer from being larger than working area.
			if (this.Height > Screen.GetWorkingArea(this).Height * 0.9)
				this.Height = (int)(Screen.GetWorkingArea(this).Height * 0.9);

			this.pid = _pid;
			FillView();
			this.labelLeft.Text = DateTime.Now.ToString("G");
		}

		int pid;

		public void FillView() {
			this.procInfoView.BeginUpdate();
			this.procInfoView.Items.Clear();
			Process process = null;
			try {
				process = Process.GetProcessById(pid);
			} catch { }
			;

			if (process != null) {
				this.labelCenter.Text = process.ProcessName + " Information";
				ListViewExt.ReflectToList(process, true, procInfoView, process.ProcessName, procInfoView.BackColor);

				Dictionary<int, ProcessEx.SystemProcess> processDict = ProcessEx.GetProcesses();
				ProcessEx.SystemProcess sysProcess;
				if (processDict.TryGetValue(pid, out sysProcess)) {
					ListViewExt.ReflectToList(sysProcess.Process.IoCounters, true, procInfoView, process.ProcessName, Color.FromArgb(255, 220, 220));
				}

				this.procInfoView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
				this.procInfoView.Columns[3].Width = this.procInfoView.Columns[4].Width = 0;
				this.procInfoView.Columns[2].Width = this.procInfoView.Width
					- this.procInfoView.Columns[0].Width
					- this.procInfoView.Columns[1].Width
					- 30;
			}
			this.procInfoView.EndUpdate();
		}

		public void UpdateView() {
			this.procInfoView.BeginUpdate();
			Process process = null;
			try {
				process = Process.GetProcessById(pid);
			} catch { }

			if (process != null) {
				this.labelRight.Text = DateTime.Now.ToString("G");
				this.labelCenter.Text = process.ProcessName + " Information";
				ListViewExt.ReflectUpdList(process, true, procInfoView, process.ProcessName, procInfoView.BackColor);

				Dictionary<int, ProcessEx.SystemProcess> processDict = ProcessEx.GetProcesses();
				ProcessEx.SystemProcess sysProcess;
				if (processDict.TryGetValue(pid, out sysProcess)) {
					ListViewExt.ReflectUpdList(sysProcess.Process.IoCounters, true, procInfoView, process.ProcessName,
						Color.FromArgb(255, 220, 220));
				}

				this.procInfoView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
			this.procInfoView.EndUpdate();
		}

		private void refreshBtn_Click(object sender, EventArgs e) {
			UpdateView();
		}


		private void exportBtn_Click(object sender, EventArgs e) {
			ListViewExt.Export(this.procInfoView);
		}

		private void printBtn_Click(object sender, EventArgs e) {
			PrintListView printListView = new PrintListView();
			printListView.Print(this, this.procInfoView, this.labelCenter.Text);
		}

		private void closeBtn_Click(object sender, EventArgs e) {
			this.Close();
		}

		#region ==== Help Dialog
		private void helpBtn_Click(object sender, EventArgs e) {
			ShowHelp();
		}

		private void helpMenuBtn_Click(object sender, EventArgs e) {
			ShowHelp();
		}

		private void ShowHelp() {
			Assembly a = Assembly.GetExecutingAssembly();
			Stream hlpStream = a.GetManifestResourceStream("MagniFile.HelpProcess.html");

			if (hlpStream != null) {
				ViewHelp helpDialog = new ViewHelp(hlpStream, null, this.Text);
				helpDialog.Show(this);
			}
		}
		#endregion


	}
}
