namespace MagniFile
{
    partial class ViewNetInfo
    {
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary>
	/// Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
	    if (disposing && (components != null))
	    {
		components.Dispose();
	    }
	    base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewNetInfo));
        this.globalCfgMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.autoUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.clearSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.clearDeadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
        this.processInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.sortArrowImages = new System.Windows.Forms.ImageList(this.components);
        this.closeBtn = new System.Windows.Forms.Button();
        this.title = new System.Windows.Forms.Label();
        this.timer = new System.Windows.Forms.Timer(this.components);
        this.toolTip = new System.Windows.Forms.ToolTip(this.components);
        this.clearFilterBtn = new System.Windows.Forms.Button();
        this.filterText = new System.Windows.Forms.TextBox();
        this.currentClock = new System.Windows.Forms.Button();
        this.startClock = new System.Windows.Forms.Button();
        this.gStatToolContainer = new System.Windows.Forms.ToolStripContainer();
        this.splitter = new System.Windows.Forms.SplitContainer();
        this.ckFilters = new System.Windows.Forms.CheckedListBox();
        this.netView = new System.Windows.Forms.ListView();
        this.col_process = new System.Windows.Forms.ColumnHeader();
        this.col_pid = new System.Windows.Forms.ColumnHeader();
        this.col_lAddr = new System.Windows.Forms.ColumnHeader();
        this.col_lport = new System.Windows.Forms.ColumnHeader();
        this.col_rAddr = new System.Windows.Forms.ColumnHeader();
        this.col_rhost = new System.Windows.Forms.ColumnHeader();
        this.col_rPort = new System.Windows.Forms.ColumnHeader();
        this.col_prot = new System.Windows.Forms.ColumnHeader();
        this.col_status = new System.Windows.Forms.ColumnHeader();
        this.col_ftime = new System.Windows.Forms.ColumnHeader();
        this.col_ltime = new System.Windows.Forms.ColumnHeader();
        this.col_duration = new System.Windows.Forms.ColumnHeader();
        this.statView = new System.Windows.Forms.ListView();
        this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
        this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
        this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
        this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
        this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
        this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
        this.toolStrip = new System.Windows.Forms.ToolStrip();
        this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
        this.viewNoGrpMenu = new System.Windows.Forms.ToolStripMenuItem();
        this.viewRemoteHostGrpMenu = new System.Windows.Forms.ToolStripMenuItem();
        this.viewProtocolGrpMenu = new System.Windows.Forms.ToolStripMenuItem();
        this.viewStatusGrpMenu = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
        this.autoBtn = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
        this.refreshBtn = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
        this.resetBtn = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
        this.exportBtn = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
        this.printSetupBtn = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
        this.fitToPageBtn = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
        this.printPreviewBtn = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
        this.printBtn = new System.Windows.Forms.ToolStripButton();
        this.helpMenuBtn = new System.Windows.Forms.ToolStripButton();
        this.titleGlow = new System.Windows.Forms.Label();
        this.statMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.exportCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.loadSnapshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.saveSnapshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.iconPanel = new System.Windows.Forms.Panel();
        this.helpBtn = new System.Windows.Forms.Button();
        this.titlePanel = new System.Windows.Forms.Panel();
        this.globalCfgMenu.SuspendLayout();
        this.gStatToolContainer.ContentPanel.SuspendLayout();
        this.gStatToolContainer.TopToolStripPanel.SuspendLayout();
        this.gStatToolContainer.SuspendLayout();
        this.splitter.Panel1.SuspendLayout();
        this.splitter.Panel2.SuspendLayout();
        this.splitter.SuspendLayout();
        this.toolStrip.SuspendLayout();
        this.statMenu.SuspendLayout();
        this.titlePanel.SuspendLayout();
        this.SuspendLayout();
        // 
        // globalCfgMenu
        // 
        this.globalCfgMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.autoUpdateToolStripMenuItem,
            this.refreshToolStripMenuItem,
            this.toolStripSeparator1,
            this.clearAllToolStripMenuItem,
            this.clearSelectedToolStripMenuItem,
            this.clearDeadToolStripMenuItem,
            this.toolStripSeparator9,
            this.processInformationToolStripMenuItem});
        this.globalCfgMenu.Name = "E";
        this.globalCfgMenu.Size = new System.Drawing.Size(182, 170);
        // 
        // exportToolStripMenuItem
        // 
        this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
        this.exportToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
        this.exportToolStripMenuItem.Text = "Export(CSV)";
        this.exportToolStripMenuItem.ToolTipText = "Save values to CSV file";
        this.exportToolStripMenuItem.Click += new System.EventHandler(this.globalMitem_Click);
        // 
        // autoUpdateToolStripMenuItem
        // 
        this.autoUpdateToolStripMenuItem.Checked = true;
        this.autoUpdateToolStripMenuItem.CheckOnClick = true;
        this.autoUpdateToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
        this.autoUpdateToolStripMenuItem.Name = "autoUpdateToolStripMenuItem";
        this.autoUpdateToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
        this.autoUpdateToolStripMenuItem.Text = "Auto Update";
        this.autoUpdateToolStripMenuItem.ToolTipText = "Enable or Disable Automatic Update";
        this.autoUpdateToolStripMenuItem.Click += new System.EventHandler(this.globalMitem_Click);
        // 
        // refreshToolStripMenuItem
        // 
        this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
        this.refreshToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
        this.refreshToolStripMenuItem.Text = "Refresh";
        this.refreshToolStripMenuItem.ToolTipText = "Force Update of values";
        this.refreshToolStripMenuItem.Click += new System.EventHandler(this.globalMitem_Click);
        // 
        // toolStripSeparator1
        // 
        this.toolStripSeparator1.Name = "toolStripSeparator1";
        this.toolStripSeparator1.Size = new System.Drawing.Size(178, 6);
        // 
        // clearAllToolStripMenuItem
        // 
        this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
        this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
        this.clearAllToolStripMenuItem.Text = "Clear All";
        this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.globalMitem_Click);
        // 
        // clearSelectedToolStripMenuItem
        // 
        this.clearSelectedToolStripMenuItem.Name = "clearSelectedToolStripMenuItem";
        this.clearSelectedToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
        this.clearSelectedToolStripMenuItem.Text = "Clear Selected";
        this.clearSelectedToolStripMenuItem.Click += new System.EventHandler(this.globalMitem_Click);
        // 
        // clearDeadToolStripMenuItem
        // 
        this.clearDeadToolStripMenuItem.Name = "clearDeadToolStripMenuItem";
        this.clearDeadToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
        this.clearDeadToolStripMenuItem.Text = "Clear Dead";
        this.clearDeadToolStripMenuItem.Click += new System.EventHandler(this.globalMitem_Click);
        // 
        // toolStripSeparator9
        // 
        this.toolStripSeparator9.Name = "toolStripSeparator9";
        this.toolStripSeparator9.Size = new System.Drawing.Size(178, 6);
        // 
        // processInformationToolStripMenuItem
        // 
        this.processInformationToolStripMenuItem.Name = "processInformationToolStripMenuItem";
        this.processInformationToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
        this.processInformationToolStripMenuItem.Text = "Process Information";
        this.processInformationToolStripMenuItem.Click += new System.EventHandler(this.globalMitem_Click);
        // 
        // sortArrowImages
        // 
        this.sortArrowImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("sortArrowImages.ImageStream")));
        this.sortArrowImages.TransparentColor = System.Drawing.Color.Transparent;
        this.sortArrowImages.Images.SetKeyName(0, "upBW.png");
        this.sortArrowImages.Images.SetKeyName(1, "downBW.png");
        // 
        // closeBtn
        // 
        this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.closeBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.closeBtn.Location = new System.Drawing.Point(822, 830);
        this.closeBtn.Name = "closeBtn";
        this.closeBtn.Size = new System.Drawing.Size(79, 24);
        this.closeBtn.TabIndex = 1;
        this.closeBtn.Text = "Close";
        this.closeBtn.UseVisualStyleBackColor = true;
        this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
        // 
        // title
        // 
        this.title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.title.BackColor = System.Drawing.Color.Transparent;
        this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.title.ForeColor = System.Drawing.Color.White;
        this.title.Location = new System.Drawing.Point(0, 0);
        this.title.Margin = new System.Windows.Forms.Padding(0);
        this.title.Name = "title";
        this.title.Size = new System.Drawing.Size(300, 40);
        this.title.TabIndex = 2;
        this.title.Text = "Network Information";
        this.title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // timer
        // 
        this.timer.Interval = 2000;
        this.timer.Tick += new System.EventHandler(this.timer_Tick);
        // 
        // clearFilterBtn
        // 
        this.clearFilterBtn.Location = new System.Drawing.Point(0, 0);
        this.clearFilterBtn.Margin = new System.Windows.Forms.Padding(0);
        this.clearFilterBtn.Name = "clearFilterBtn";
        this.clearFilterBtn.Size = new System.Drawing.Size(75, 23);
        this.clearFilterBtn.TabIndex = 1;
        this.clearFilterBtn.Text = "Clear Filter:";
        this.toolTip.SetToolTip(this.clearFilterBtn, "Clear filter text");
        this.clearFilterBtn.UseVisualStyleBackColor = true;
        this.clearFilterBtn.Click += new System.EventHandler(this.clearFilterBtn_Click);
        // 
        // filterText
        // 
        this.filterText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.filterText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
        this.filterText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.filterText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.filterText.Location = new System.Drawing.Point(75, 0);
        this.filterText.Margin = new System.Windows.Forms.Padding(0);
        this.filterText.Name = "filterText";
        this.filterText.Size = new System.Drawing.Size(632, 22);
        this.filterText.TabIndex = 2;
        this.filterText.Text = "!idle";
        this.toolTip.SetToolTip(this.filterText, "Enter words to match separated by semicolon, press enter to force update.    Ex c" +
                "hrome;svchost");
        this.filterText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.filterText_KeyUp);
        // 
        // currentClock
        // 
        this.currentClock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.currentClock.BackgroundImage = global::MagniFile.Properties.Resources.ovalGrey;
        this.currentClock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.currentClock.FlatAppearance.BorderSize = 0;
        this.currentClock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.currentClock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.currentClock.ForeColor = System.Drawing.Color.White;
        this.currentClock.Location = new System.Drawing.Point(801, 18);
        this.currentClock.Name = "currentClock";
        this.currentClock.Size = new System.Drawing.Size(100, 22);
        this.currentClock.TabIndex = 15;
        this.currentClock.Text = "hh:mm am";
        this.toolTip.SetToolTip(this.currentClock, "Time of last update,  controlled by  AutoUpdate");
        this.currentClock.UseVisualStyleBackColor = true;
        // 
        // startClock
        // 
        this.startClock.BackgroundImage = global::MagniFile.Properties.Resources.ovalGrey;
        this.startClock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.startClock.FlatAppearance.BorderSize = 0;
        this.startClock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.startClock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.startClock.ForeColor = System.Drawing.Color.White;
        this.startClock.Location = new System.Drawing.Point(12, 18);
        this.startClock.Name = "startClock";
        this.startClock.Size = new System.Drawing.Size(100, 22);
        this.startClock.TabIndex = 17;
        this.startClock.Text = "hh:mm am";
        this.toolTip.SetToolTip(this.startClock, "Start Time, press Reset to set new starting values");
        this.startClock.UseVisualStyleBackColor = true;
        // 
        // gStatToolContainer
        // 
        this.gStatToolContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        // 
        // gStatToolContainer.ContentPanel
        // 
        this.gStatToolContainer.ContentPanel.Controls.Add(this.splitter);
        this.gStatToolContainer.ContentPanel.Size = new System.Drawing.Size(889, 751);
        this.gStatToolContainer.LeftToolStripPanelVisible = false;
        this.gStatToolContainer.Location = new System.Drawing.Point(12, 48);
        this.gStatToolContainer.Name = "gStatToolContainer";
        this.gStatToolContainer.RightToolStripPanelVisible = false;
        this.gStatToolContainer.Size = new System.Drawing.Size(889, 776);
        this.gStatToolContainer.TabIndex = 6;
        this.gStatToolContainer.Text = "toolStripContainer1";
        // 
        // gStatToolContainer.TopToolStripPanel
        // 
        this.gStatToolContainer.TopToolStripPanel.Controls.Add(this.toolStrip);
        // 
        // splitter
        // 
        this.splitter.BackColor = System.Drawing.Color.Fuchsia;
        this.splitter.Dock = System.Windows.Forms.DockStyle.Fill;
        this.splitter.Location = new System.Drawing.Point(0, 0);
        this.splitter.Name = "splitter";
        this.splitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
        // 
        // splitter.Panel1
        // 
        this.splitter.Panel1.Controls.Add(this.ckFilters);
        this.splitter.Panel1.Controls.Add(this.filterText);
        this.splitter.Panel1.Controls.Add(this.clearFilterBtn);
        this.splitter.Panel1.Controls.Add(this.netView);
        // 
        // splitter.Panel2
        // 
        this.splitter.Panel2.Controls.Add(this.statView);
        this.splitter.Panel2MinSize = 0;
        this.splitter.Size = new System.Drawing.Size(889, 751);
        this.splitter.SplitterDistance = 568;
        this.splitter.TabIndex = 1;
        // 
        // ckFilters
        // 
        this.ckFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.ckFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.ckFilters.CheckOnClick = true;
        this.ckFilters.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.ckFilters.FormattingEnabled = true;
        this.ckFilters.Items.AddRange(new object[] {
            "Tcp",
            "Udp",
            "Show Dead",
            "Listening",
            "Established",
            "Close*",
            "Fin*",
            "TimeWait"});
        this.ckFilters.Location = new System.Drawing.Point(707, 0);
        this.ckFilters.Margin = new System.Windows.Forms.Padding(0);
        this.ckFilters.Name = "ckFilters";
        this.ckFilters.Size = new System.Drawing.Size(182, 23);
        this.ckFilters.TabIndex = 3;
        this.ckFilters.SelectedIndexChanged += new System.EventHandler(this.checkBoxes_SelectedIndexChanged);
        this.ckFilters.MouseEnter += new System.EventHandler(this.checkBoxes_MouseEnter);
        this.ckFilters.MouseLeave += new System.EventHandler(this.checkBoxes_MouseLeave);
        // 
        // netView
        // 
        this.netView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.netView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_process,
            this.col_pid,
            this.col_lAddr,
            this.col_lport,
            this.col_rAddr,
            this.col_rhost,
            this.col_rPort,
            this.col_prot,
            this.col_status,
            this.col_ftime,
            this.col_ltime,
            this.col_duration});
        this.netView.ContextMenuStrip = this.globalCfgMenu;
        this.netView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.netView.FullRowSelect = true;
        this.netView.GridLines = true;
        this.netView.Location = new System.Drawing.Point(0, 22);
        this.netView.Name = "netView";
        this.netView.Size = new System.Drawing.Size(889, 544);
        this.netView.SmallImageList = this.sortArrowImages;
        this.netView.TabIndex = 0;
        this.netView.UseCompatibleStateImageBehavior = false;
        this.netView.View = System.Windows.Forms.View.Details;
        this.netView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ColumnClick);
        this.netView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.netView_KeyUp);
        // 
        // col_process
        // 
        this.col_process.Text = "Process";
        this.col_process.Width = 100;
        // 
        // col_pid
        // 
        this.col_pid.Text = "Pid";
        this.col_pid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        // 
        // col_lAddr
        // 
        this.col_lAddr.Text = "Local Address";
        this.col_lAddr.Width = 120;
        // 
        // col_lport
        // 
        this.col_lport.Text = "Port";
        this.col_lport.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        // 
        // col_rAddr
        // 
        this.col_rAddr.Text = "Remote Address";
        this.col_rAddr.Width = 120;
        // 
        // col_rhost
        // 
        this.col_rhost.Text = "Remote Host";
        this.col_rhost.Width = 100;
        // 
        // col_rPort
        // 
        this.col_rPort.Text = "Port";
        this.col_rPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        // 
        // col_prot
        // 
        this.col_prot.Text = "Prot";
        // 
        // col_status
        // 
        this.col_status.Text = "Status";
        this.col_status.Width = 100;
        // 
        // col_ftime
        // 
        this.col_ftime.Text = "First Time";
        this.col_ftime.Width = 100;
        // 
        // col_ltime
        // 
        this.col_ltime.Text = "Last Tme";
        this.col_ltime.Width = 100;
        // 
        // col_duration
        // 
        this.col_duration.Text = "Duration(min)";
        this.col_duration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        // 
        // statView
        // 
        this.statView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
        this.statView.ContextMenuStrip = this.statMenu;
        this.statView.Dock = System.Windows.Forms.DockStyle.Fill;
        this.statView.FullRowSelect = true;
        this.statView.GridLines = true;
        this.statView.Location = new System.Drawing.Point(0, 0);
        this.statView.Name = "statView";
        this.statView.Size = new System.Drawing.Size(889, 179);
        this.statView.SmallImageList = this.sortArrowImages;
        this.statView.TabIndex = 0;
        this.statView.UseCompatibleStateImageBehavior = false;
        this.statView.View = System.Windows.Forms.View.Details;
        this.statView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ColumnClick);
        // 
        // columnHeader1
        // 
        this.columnHeader1.Text = "Protocol";
        this.columnHeader1.Width = 100;
        // 
        // columnHeader2
        // 
        this.columnHeader2.Text = "Description";
        this.columnHeader2.Width = 150;
        // 
        // columnHeader3
        // 
        this.columnHeader3.Text = "Start Value";
        this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        this.columnHeader3.Width = 100;
        // 
        // columnHeader4
        // 
        this.columnHeader4.Text = "Change";
        this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        this.columnHeader4.Width = 100;
        // 
        // columnHeader5
        // 
        this.columnHeader5.Text = "Current Value";
        this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        this.columnHeader5.Width = 100;
        // 
        // columnHeader6
        // 
        this.columnHeader6.Text = "Maximum";
        this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        this.columnHeader6.Width = 100;
        // 
        // toolStrip
        // 
        this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
        this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
        this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripSeparator10,
            this.autoBtn,
            this.toolStripSeparator4,
            this.refreshBtn,
            this.toolStripSeparator5,
            this.resetBtn,
            this.toolStripSeparator2,
            this.exportBtn,
            this.toolStripSeparator3,
            this.printSetupBtn,
            this.toolStripSeparator6,
            this.fitToPageBtn,
            this.toolStripSeparator8,
            this.printPreviewBtn,
            this.toolStripSeparator7,
            this.printBtn,
            this.helpMenuBtn});
        this.toolStrip.Location = new System.Drawing.Point(0, 0);
        this.toolStrip.Name = "toolStrip";
        this.toolStrip.Size = new System.Drawing.Size(889, 25);
        this.toolStrip.Stretch = true;
        this.toolStrip.TabIndex = 0;
        // 
        // toolStripDropDownButton1
        // 
        this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewNoGrpMenu,
            this.viewRemoteHostGrpMenu,
            this.viewProtocolGrpMenu,
            this.viewStatusGrpMenu});
        this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
        this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
        this.toolStripDropDownButton1.Size = new System.Drawing.Size(57, 22);
        this.toolStripDropDownButton1.Text = " View...";
        // 
        // viewNoGrpMenu
        // 
        this.viewNoGrpMenu.Checked = true;
        this.viewNoGrpMenu.CheckState = System.Windows.Forms.CheckState.Checked;
        this.viewNoGrpMenu.Name = "viewNoGrpMenu";
        this.viewNoGrpMenu.Size = new System.Drawing.Size(194, 22);
        this.viewNoGrpMenu.Tag = -1;
        this.viewNoGrpMenu.Text = "No Group";
        this.viewNoGrpMenu.Click += new System.EventHandler(this.viewGrpMenu_Click);
        // 
        // viewRemoteHostGrpMenu
        // 
        this.viewRemoteHostGrpMenu.Name = "viewRemoteHostGrpMenu";
        this.viewRemoteHostGrpMenu.Size = new System.Drawing.Size(194, 22);
        this.viewRemoteHostGrpMenu.Tag = 5;
        this.viewRemoteHostGrpMenu.Text = "Group By Remote Host";
        this.viewRemoteHostGrpMenu.Click += new System.EventHandler(this.viewGrpMenu_Click);
        // 
        // viewProtocolGrpMenu
        // 
        this.viewProtocolGrpMenu.Name = "viewProtocolGrpMenu";
        this.viewProtocolGrpMenu.Size = new System.Drawing.Size(194, 22);
        this.viewProtocolGrpMenu.Tag = 7;
        this.viewProtocolGrpMenu.Text = "Group By Protocol";
        this.viewProtocolGrpMenu.Click += new System.EventHandler(this.viewGrpMenu_Click);
        // 
        // viewStatusGrpMenu
        // 
        this.viewStatusGrpMenu.Name = "viewStatusGrpMenu";
        this.viewStatusGrpMenu.Size = new System.Drawing.Size(194, 22);
        this.viewStatusGrpMenu.Tag = 8;
        this.viewStatusGrpMenu.Text = "Group By Status";
        this.viewStatusGrpMenu.Click += new System.EventHandler(this.viewGrpMenu_Click);
        // 
        // toolStripSeparator10
        // 
        this.toolStripSeparator10.Name = "toolStripSeparator10";
        this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
        // 
        // autoBtn
        // 
        this.autoBtn.Checked = true;
        this.autoBtn.CheckOnClick = true;
        this.autoBtn.CheckState = System.Windows.Forms.CheckState.Checked;
        this.autoBtn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.autoBtn.Image = global::MagniFile.Properties.Resources.check_on;
        this.autoBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.autoBtn.Name = "autoBtn";
        this.autoBtn.Size = new System.Drawing.Size(88, 22);
        this.autoBtn.Text = "Auto Update";
        this.autoBtn.ToolTipText = "Enable/Disable Auto Update every few seconds";
        this.autoBtn.Click += new System.EventHandler(this.toolButton_Click);
        // 
        // toolStripSeparator4
        // 
        this.toolStripSeparator4.Name = "toolStripSeparator4";
        this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
        // 
        // refreshBtn
        // 
        this.refreshBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.refreshBtn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.refreshBtn.Image = ((System.Drawing.Image)(resources.GetObject("refreshBtn.Image")));
        this.refreshBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.refreshBtn.Name = "refreshBtn";
        this.refreshBtn.Size = new System.Drawing.Size(49, 22);
        this.refreshBtn.Text = "Refresh";
        this.refreshBtn.ToolTipText = "Refresh display with latest values";
        this.refreshBtn.Click += new System.EventHandler(this.toolButton_Click);
        // 
        // toolStripSeparator5
        // 
        this.toolStripSeparator5.Name = "toolStripSeparator5";
        this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
        // 
        // resetBtn
        // 
        this.resetBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.resetBtn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.resetBtn.Image = ((System.Drawing.Image)(resources.GetObject("resetBtn.Image")));
        this.resetBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.resetBtn.Name = "resetBtn";
        this.resetBtn.Size = new System.Drawing.Size(39, 22);
        this.resetBtn.Text = "Reset";
        this.resetBtn.ToolTipText = "Reset Change by updating Starting values";
        this.resetBtn.Click += new System.EventHandler(this.resetBtn_Click);
        // 
        // toolStripSeparator2
        // 
        this.toolStripSeparator2.Name = "toolStripSeparator2";
        this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
        // 
        // exportBtn
        // 
        this.exportBtn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.exportBtn.Image = global::MagniFile.Properties.Resources.printSetup;
        this.exportBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.exportBtn.Name = "exportBtn";
        this.exportBtn.Size = new System.Drawing.Size(86, 22);
        this.exportBtn.Text = "Export(CSV)";
        this.exportBtn.ToolTipText = "Export table  as CSV file";
        this.exportBtn.Click += new System.EventHandler(this.toolButton_Click);
        // 
        // toolStripSeparator3
        // 
        this.toolStripSeparator3.Name = "toolStripSeparator3";
        this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
        // 
        // printSetupBtn
        // 
        this.printSetupBtn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.printSetupBtn.Image = global::MagniFile.Properties.Resources.printSetup;
        this.printSetupBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.printSetupBtn.Name = "printSetupBtn";
        this.printSetupBtn.Size = new System.Drawing.Size(80, 22);
        this.printSetupBtn.Text = "Print Setup";
        this.printSetupBtn.ToolTipText = "Open Printer setup dialog";
        this.printSetupBtn.Click += new System.EventHandler(this.toolButton_Click);
        // 
        // toolStripSeparator6
        // 
        this.toolStripSeparator6.Name = "toolStripSeparator6";
        this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
        // 
        // fitToPageBtn
        // 
        this.fitToPageBtn.Checked = true;
        this.fitToPageBtn.CheckOnClick = true;
        this.fitToPageBtn.CheckState = System.Windows.Forms.CheckState.Checked;
        this.fitToPageBtn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.fitToPageBtn.Image = global::MagniFile.Properties.Resources.check_on;
        this.fitToPageBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.fitToPageBtn.Name = "fitToPageBtn";
        this.fitToPageBtn.Size = new System.Drawing.Size(75, 22);
        this.fitToPageBtn.Text = "FitToPage";
        this.fitToPageBtn.ToolTipText = "Toggles Fit-to-page print mode";
        this.fitToPageBtn.Click += new System.EventHandler(this.toolButton_Click);
        // 
        // toolStripSeparator8
        // 
        this.toolStripSeparator8.Name = "toolStripSeparator8";
        this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
        // 
        // printPreviewBtn
        // 
        this.printPreviewBtn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.printPreviewBtn.Image = global::MagniFile.Properties.Resources.printPreview;
        this.printPreviewBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.printPreviewBtn.Name = "printPreviewBtn";
        this.printPreviewBtn.Size = new System.Drawing.Size(90, 22);
        this.printPreviewBtn.Text = "Print Preview";
        this.printPreviewBtn.ToolTipText = "Open print preview dialog";
        this.printPreviewBtn.Click += new System.EventHandler(this.toolButton_Click);
        // 
        // toolStripSeparator7
        // 
        this.toolStripSeparator7.Name = "toolStripSeparator7";
        this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
        // 
        // printBtn
        // 
        this.printBtn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.printBtn.Image = global::MagniFile.Properties.Resources.print;
        this.printBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.printBtn.Name = "printBtn";
        this.printBtn.Size = new System.Drawing.Size(49, 22);
        this.printBtn.Text = "Print";
        this.printBtn.ToolTipText = "Open Print dialog";
        this.printBtn.Click += new System.EventHandler(this.toolButton_Click);
        // 
        // helpMenuBtn
        // 
        this.helpMenuBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
        this.helpMenuBtn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.helpMenuBtn.Image = global::MagniFile.Properties.Resources.help24;
        this.helpMenuBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.helpMenuBtn.Name = "helpMenuBtn";
        this.helpMenuBtn.Size = new System.Drawing.Size(48, 22);
        this.helpMenuBtn.Text = "Help";
        this.helpMenuBtn.Click += new System.EventHandler(this.helpMenuBtn_Click);
        // 
        // titleGlow
        // 
        this.titleGlow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.titleGlow.BackColor = System.Drawing.Color.Transparent;
        this.titleGlow.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.titleGlow.ForeColor = System.Drawing.Color.Maroon;
        this.titleGlow.Location = new System.Drawing.Point(0, 0);
        this.titleGlow.Margin = new System.Windows.Forms.Padding(0);
        this.titleGlow.Name = "titleGlow";
        this.titleGlow.Size = new System.Drawing.Size(300, 40);
        this.titleGlow.TabIndex = 7;
        this.titleGlow.Text = "Network Information";
        this.titleGlow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // statMenu
        // 
        this.statMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportCSVToolStripMenuItem,
            this.loadSnapshotToolStripMenuItem,
            this.saveSnapshotToolStripMenuItem});
        this.statMenu.Name = "statMenu";
        this.statMenu.Size = new System.Drawing.Size(158, 70);
        // 
        // exportCSVToolStripMenuItem
        // 
        this.exportCSVToolStripMenuItem.Name = "exportCSVToolStripMenuItem";
        this.exportCSVToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
        this.exportCSVToolStripMenuItem.Text = "Export (CSV)";
        this.exportCSVToolStripMenuItem.Click += new System.EventHandler(this.exportCSVToolStripMenuItem_Click);
        // 
        // loadSnapshotToolStripMenuItem
        // 
        this.loadSnapshotToolStripMenuItem.Name = "loadSnapshotToolStripMenuItem";
        this.loadSnapshotToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
        this.loadSnapshotToolStripMenuItem.Text = "Load Snapshot";
        this.loadSnapshotToolStripMenuItem.Click += new System.EventHandler(this.loadSnapshotToolStripMenuItem_Click);
        // 
        // saveSnapshotToolStripMenuItem
        // 
        this.saveSnapshotToolStripMenuItem.Name = "saveSnapshotToolStripMenuItem";
        this.saveSnapshotToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
        this.saveSnapshotToolStripMenuItem.Text = "Save Snapshot";
        this.saveSnapshotToolStripMenuItem.Click += new System.EventHandler(this.saveSnapshotToolStripMenuItem_Click);
        // 
        // iconPanel
        // 
        this.iconPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("iconPanel.BackgroundImage")));
        this.iconPanel.Location = new System.Drawing.Point(129, 2);
        this.iconPanel.Margin = new System.Windows.Forms.Padding(0);
        this.iconPanel.Name = "iconPanel";
        this.iconPanel.Size = new System.Drawing.Size(42, 42);
        this.iconPanel.TabIndex = 16;
        // 
        // helpBtn
        // 
        this.helpBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.helpBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.helpBtn.Image = ((System.Drawing.Image)(resources.GetObject("helpBtn.Image")));
        this.helpBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
        this.helpBtn.Location = new System.Drawing.Point(730, 830);
        this.helpBtn.Name = "helpBtn";
        this.helpBtn.Size = new System.Drawing.Size(75, 24);
        this.helpBtn.TabIndex = 14;
        this.helpBtn.Text = "Help";
        this.helpBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        this.helpBtn.UseVisualStyleBackColor = true;
        this.helpBtn.Click += new System.EventHandler(this.helpBtn_Click);
        // 
        // titlePanel
        // 
        this.titlePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.titlePanel.BackColor = System.Drawing.Color.Transparent;
        this.titlePanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("titlePanel.BackgroundImage")));
        this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.titlePanel.Controls.Add(this.titleGlow);
        this.titlePanel.Controls.Add(this.title);
        this.titlePanel.Location = new System.Drawing.Point(300, 0);
        this.titlePanel.Margin = new System.Windows.Forms.Padding(0);
        this.titlePanel.Name = "titlePanel";
        this.titlePanel.Size = new System.Drawing.Size(300, 42);
        this.titlePanel.TabIndex = 18;
        // 
        // ViewNetInfo
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.Navy;
        this.ClientSize = new System.Drawing.Size(913, 866);
        this.Controls.Add(this.startClock);
        this.Controls.Add(this.iconPanel);
        this.Controls.Add(this.helpBtn);
        this.Controls.Add(this.gStatToolContainer);
        this.Controls.Add(this.currentClock);
        this.Controls.Add(this.closeBtn);
        this.Controls.Add(this.titlePanel);
        this.DoubleBuffered = true;
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "ViewNetInfo";
        this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
        this.Text = "MagniFile - Network  Information";
        this.globalCfgMenu.ResumeLayout(false);
        this.gStatToolContainer.ContentPanel.ResumeLayout(false);
        this.gStatToolContainer.TopToolStripPanel.ResumeLayout(false);
        this.gStatToolContainer.TopToolStripPanel.PerformLayout();
        this.gStatToolContainer.ResumeLayout(false);
        this.gStatToolContainer.PerformLayout();
        this.splitter.Panel1.ResumeLayout(false);
        this.splitter.Panel1.PerformLayout();
        this.splitter.Panel2.ResumeLayout(false);
        this.splitter.ResumeLayout(false);
        this.toolStrip.ResumeLayout(false);
        this.toolStrip.PerformLayout();
        this.statMenu.ResumeLayout(false);
        this.titlePanel.ResumeLayout(false);
        this.ResumeLayout(false);

	}

	#endregion

    private System.Windows.Forms.Button closeBtn;
    private System.Windows.Forms.Label title;
    private System.Windows.Forms.Timer timer;
	private System.Windows.Forms.ToolTip toolTip;
	private System.Windows.Forms.ContextMenuStrip globalCfgMenu;
	private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
	private System.Windows.Forms.ToolStripMenuItem autoUpdateToolStripMenuItem;
	private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
	private System.Windows.Forms.ImageList sortArrowImages;
	private System.Windows.Forms.ToolStripContainer gStatToolContainer;
	private System.Windows.Forms.ToolStrip toolStrip;
	private System.Windows.Forms.ToolStripButton autoBtn;
	private System.Windows.Forms.ToolStripButton refreshBtn;
	private System.Windows.Forms.ToolStripButton resetBtn;
	private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
	private System.Windows.Forms.ToolStripButton exportBtn;
	private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
	private System.Windows.Forms.ToolStripButton printSetupBtn;
	private System.Windows.Forms.ToolStripButton printPreviewBtn;
	private System.Windows.Forms.ToolStripButton printBtn;
	private System.Windows.Forms.Label titleGlow;
    private System.Windows.Forms.ToolStripButton fitToPageBtn;
	private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
	private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
	private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
	private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
	private System.Windows.Forms.Button helpBtn;
    private System.Windows.Forms.ListView netView;
    private System.Windows.Forms.ColumnHeader col_process;
    private System.Windows.Forms.ColumnHeader col_pid;
    private System.Windows.Forms.ColumnHeader col_lAddr;
    private System.Windows.Forms.ColumnHeader col_lport;
    private System.Windows.Forms.ColumnHeader col_rAddr;
    private System.Windows.Forms.ColumnHeader col_rPort;
    private System.Windows.Forms.ColumnHeader col_prot;
    private System.Windows.Forms.ColumnHeader col_status;
    private System.Windows.Forms.ColumnHeader col_ftime;
    private System.Windows.Forms.ColumnHeader col_ltime;
    private System.Windows.Forms.SplitContainer splitter;
    private System.Windows.Forms.ListView statView;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem clearSelectedToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem clearDeadToolStripMenuItem;
    private System.Windows.Forms.ToolStripButton helpMenuBtn;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
    private System.Windows.Forms.ToolStripMenuItem processInformationToolStripMenuItem;
    private System.Windows.Forms.Button clearFilterBtn;
    private System.Windows.Forms.TextBox filterText;
    private System.Windows.Forms.CheckedListBox ckFilters;
    private System.Windows.Forms.ColumnHeader col_rhost;
    private System.Windows.Forms.ColumnHeader col_duration;
    private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
    private System.Windows.Forms.ToolStripMenuItem viewNoGrpMenu;
    private System.Windows.Forms.ToolStripMenuItem viewRemoteHostGrpMenu;
    private System.Windows.Forms.ToolStripMenuItem viewProtocolGrpMenu;
    private System.Windows.Forms.ToolStripMenuItem viewStatusGrpMenu;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
    private System.Windows.Forms.ColumnHeader columnHeader6;
    private System.Windows.Forms.ContextMenuStrip statMenu;
    private System.Windows.Forms.ToolStripMenuItem exportCSVToolStripMenuItem;
    private System.Windows.Forms.Button currentClock;
    private System.Windows.Forms.Panel iconPanel;
    private System.Windows.Forms.Button startClock;
    private System.Windows.Forms.Panel titlePanel;
    private System.Windows.Forms.ToolStripMenuItem loadSnapshotToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveSnapshotToolStripMenuItem;
    }
}
