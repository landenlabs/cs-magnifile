namespace MagniFile
{
    partial class ViewFsInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewFsInfo));
            this.globalStatView = new System.Windows.Forms.ListView();
            this.col_class = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_desc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_val1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_change = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_val2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.globalCfgMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.printSetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortArrowImages = new System.Windows.Forms.ImageList(this.components);
            this.closeBtn = new System.Windows.Forms.Button();
            this.title = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.sysCk = new System.Windows.Forms.CheckBox();
            this.driveCk = new System.Windows.Forms.CheckBox();
            this.statsCk = new System.Windows.Forms.CheckBox();
            this.cacheCk = new System.Windows.Forms.CheckBox();
            this.diskCk = new System.Windows.Forms.CheckBox();
            this.perfCk = new System.Windows.Forms.CheckBox();
            this.currentClock = new System.Windows.Forms.Button();
            this.startClock = new System.Windows.Forms.Button();
            this.gStatToolContainer = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
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
            this.helpBtn = new System.Windows.Forms.Button();
            this.globalCfgMenu.SuspendLayout();
            this.gStatToolContainer.ContentPanel.SuspendLayout();
            this.gStatToolContainer.TopToolStripPanel.SuspendLayout();
            this.gStatToolContainer.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // globalStatView
            // 
            this.globalStatView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_class,
            this.col_desc,
            this.col_val1,
            this.col_change,
            this.col_val2});
            this.globalStatView.ContextMenuStrip = this.globalCfgMenu;
            this.globalStatView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.globalStatView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.globalStatView.FullRowSelect = true;
            this.globalStatView.GridLines = true;
            this.globalStatView.Location = new System.Drawing.Point(0, 0);
            this.globalStatView.Name = "globalStatView";
            this.globalStatView.Size = new System.Drawing.Size(628, 751);
            this.globalStatView.SmallImageList = this.sortArrowImages;
            this.globalStatView.TabIndex = 0;
            this.globalStatView.UseCompatibleStateImageBehavior = false;
            this.globalStatView.View = System.Windows.Forms.View.Details;
            this.globalStatView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ColumnClick);
            this.globalStatView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.globalStatView_KeyUp);
            // 
            // col_class
            // 
            this.col_class.Text = "Class";
            // 
            // col_desc
            // 
            this.col_desc.Text = "Description";
            this.col_desc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.col_desc.Width = 130;
            // 
            // col_val1
            // 
            this.col_val1.Text = "Starting Value";
            this.col_val1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.col_val1.Width = 130;
            // 
            // col_change
            // 
            this.col_change.Text = "Change";
            this.col_change.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.col_change.Width = 100;
            // 
            // col_val2
            // 
            this.col_val2.Text = "Current Value";
            this.col_val2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.col_val2.Width = 130;
            // 
            // globalCfgMenu
            // 
            this.globalCfgMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.autoUpdateToolStripMenuItem,
            this.refreshToolStripMenuItem,
            this.toolStripSeparator1,
            this.printSetupToolStripMenuItem,
            this.printPreviewToolStripMenuItem,
            this.printToolStripMenuItem});
            this.globalCfgMenu.Name = "E";
            this.globalCfgMenu.Size = new System.Drawing.Size(149, 142);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
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
            this.autoUpdateToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.autoUpdateToolStripMenuItem.Text = "Auto Update";
            this.autoUpdateToolStripMenuItem.ToolTipText = "Enable or Disable Automatic Update";
            this.autoUpdateToolStripMenuItem.Click += new System.EventHandler(this.globalMitem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.ToolTipText = "Force Update of values";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.globalMitem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
            // 
            // printSetupToolStripMenuItem
            // 
            this.printSetupToolStripMenuItem.Name = "printSetupToolStripMenuItem";
            this.printSetupToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.printSetupToolStripMenuItem.Text = "Print Setup";
            this.printSetupToolStripMenuItem.Click += new System.EventHandler(this.globalMitem_Click);
            // 
            // printPreviewToolStripMenuItem
            // 
            this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.printPreviewToolStripMenuItem.Text = "Print Preview";
            this.printPreviewToolStripMenuItem.Click += new System.EventHandler(this.globalMitem_Click);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.printToolStripMenuItem.Text = "Print";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.globalMitem_Click);
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
            this.closeBtn.Location = new System.Drawing.Point(561, 830);
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
            this.title.Location = new System.Drawing.Point(34, 9);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(580, 36);
            this.title.TabIndex = 2;
            this.title.Text = "File System Information";
            this.title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer
            // 
            this.timer.Interval = 2000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // sysCk
            // 
            this.sysCk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sysCk.AutoSize = true;
            this.sysCk.Checked = true;
            this.sysCk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sysCk.ForeColor = System.Drawing.Color.Yellow;
            this.sysCk.Location = new System.Drawing.Point(12, 837);
            this.sysCk.Name = "sysCk";
            this.sysCk.Size = new System.Drawing.Size(43, 17);
            this.sysCk.TabIndex = 8;
            this.sysCk.Text = "Sys";
            this.toolTip.SetToolTip(this.sysCk, "General System Information");
            this.sysCk.UseVisualStyleBackColor = true;
            this.sysCk.Click += new System.EventHandler(this.ck_Click);
            // 
            // driveCk
            // 
            this.driveCk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.driveCk.AutoSize = true;
            this.driveCk.ForeColor = System.Drawing.Color.White;
            this.driveCk.Location = new System.Drawing.Point(243, 837);
            this.driveCk.Name = "driveCk";
            this.driveCk.Size = new System.Drawing.Size(51, 17);
            this.driveCk.TabIndex = 9;
            this.driveCk.Text = "Drive";
            this.toolTip.SetToolTip(this.driveCk, "Disk Drive details");
            this.driveCk.UseVisualStyleBackColor = true;
            this.driveCk.Click += new System.EventHandler(this.ck_Click);
            // 
            // statsCk
            // 
            this.statsCk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.statsCk.AutoSize = true;
            this.statsCk.ForeColor = System.Drawing.Color.White;
            this.statsCk.Location = new System.Drawing.Point(187, 837);
            this.statsCk.Name = "statsCk";
            this.statsCk.Size = new System.Drawing.Size(50, 17);
            this.statsCk.TabIndex = 10;
            this.statsCk.Text = "Stats";
            this.toolTip.SetToolTip(this.statsCk, "File system statisticas (NTFS)");
            this.statsCk.UseVisualStyleBackColor = true;
            this.statsCk.Click += new System.EventHandler(this.ck_Click);
            // 
            // cacheCk
            // 
            this.cacheCk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cacheCk.AutoSize = true;
            this.cacheCk.ForeColor = System.Drawing.Color.Yellow;
            this.cacheCk.Location = new System.Drawing.Point(71, 837);
            this.cacheCk.Name = "cacheCk";
            this.cacheCk.Size = new System.Drawing.Size(57, 17);
            this.cacheCk.TabIndex = 11;
            this.cacheCk.Text = "Cache";
            this.toolTip.SetToolTip(this.cacheCk, "File System Cache Info");
            this.cacheCk.UseVisualStyleBackColor = true;
            this.cacheCk.Click += new System.EventHandler(this.ck_Click);
            // 
            // diskCk
            // 
            this.diskCk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.diskCk.AutoSize = true;
            this.diskCk.ForeColor = System.Drawing.Color.Yellow;
            this.diskCk.Location = new System.Drawing.Point(134, 837);
            this.diskCk.Name = "diskCk";
            this.diskCk.Size = new System.Drawing.Size(47, 17);
            this.diskCk.TabIndex = 12;
            this.diskCk.Text = "Disk";
            this.toolTip.SetToolTip(this.diskCk, "Physical Disk Information");
            this.diskCk.UseVisualStyleBackColor = true;
            this.diskCk.Click += new System.EventHandler(this.ck_Click);
            // 
            // perfCk
            // 
            this.perfCk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.perfCk.AutoSize = true;
            this.perfCk.ForeColor = System.Drawing.Color.White;
            this.perfCk.Location = new System.Drawing.Point(300, 837);
            this.perfCk.Name = "perfCk";
            this.perfCk.Size = new System.Drawing.Size(86, 17);
            this.perfCk.TabIndex = 13;
            this.perfCk.Text = "Performance";
            this.toolTip.SetToolTip(this.perfCk, "Disk Performance Stats");
            this.perfCk.UseVisualStyleBackColor = true;
            this.perfCk.Click += new System.EventHandler(this.ck_Click);
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
            this.currentClock.Location = new System.Drawing.Point(536, 18);
            this.currentClock.Name = "currentClock";
            this.currentClock.Size = new System.Drawing.Size(100, 23);
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
            this.startClock.Size = new System.Drawing.Size(100, 23);
            this.startClock.TabIndex = 16;
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
            this.gStatToolContainer.ContentPanel.Controls.Add(this.globalStatView);
            this.gStatToolContainer.ContentPanel.Size = new System.Drawing.Size(628, 751);
            this.gStatToolContainer.LeftToolStripPanelVisible = false;
            this.gStatToolContainer.Location = new System.Drawing.Point(12, 48);
            this.gStatToolContainer.Name = "gStatToolContainer";
            this.gStatToolContainer.RightToolStripPanelVisible = false;
            this.gStatToolContainer.Size = new System.Drawing.Size(628, 776);
            this.gStatToolContainer.TabIndex = 6;
            this.gStatToolContainer.Text = "toolStripContainer1";
            // 
            // gStatToolContainer.TopToolStripPanel
            // 
            this.gStatToolContainer.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(628, 25);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 0;
            // 
            // autoBtn
            // 
            this.autoBtn.Checked = true;
            this.autoBtn.CheckOnClick = true;
            this.autoBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoBtn.Image = global::MagniFile.Properties.Resources.check_on;
            this.autoBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.autoBtn.Name = "autoBtn";
            this.autoBtn.Size = new System.Drawing.Size(94, 22);
            this.autoBtn.Text = "Auto Update";
            this.autoBtn.ToolTipText = "Enable/Disable Auto Update every few seconds.\r\nSee bottom of dialog for checkboxe" +
    "s to enable various metrics.\r\nOnly the last three choices in white will change o" +
    "ver time.";
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
            this.refreshBtn.Image = ((System.Drawing.Image)(resources.GetObject("refreshBtn.Image")));
            this.refreshBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(50, 22);
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
            this.exportBtn.Image = global::MagniFile.Properties.Resources.printSetup;
            this.exportBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportBtn.Name = "exportBtn";
            this.exportBtn.Size = new System.Drawing.Size(89, 22);
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
            this.printSetupBtn.Image = global::MagniFile.Properties.Resources.printSetup;
            this.printSetupBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printSetupBtn.Name = "printSetupBtn";
            this.printSetupBtn.Size = new System.Drawing.Size(85, 22);
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
            this.fitToPageBtn.Image = global::MagniFile.Properties.Resources.check_on;
            this.fitToPageBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fitToPageBtn.Name = "fitToPageBtn";
            this.fitToPageBtn.Size = new System.Drawing.Size(80, 22);
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
            this.printPreviewBtn.Image = global::MagniFile.Properties.Resources.printPreview;
            this.printPreviewBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printPreviewBtn.Name = "printPreviewBtn";
            this.printPreviewBtn.Size = new System.Drawing.Size(96, 22);
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
            this.printBtn.Image = global::MagniFile.Properties.Resources.print;
            this.printBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printBtn.Name = "printBtn";
            this.printBtn.Size = new System.Drawing.Size(52, 20);
            this.printBtn.Text = "Print";
            this.printBtn.ToolTipText = "Open Print dialog";
            this.printBtn.Click += new System.EventHandler(this.toolButton_Click);
            // 
            // helpMenuBtn
            // 
            this.helpMenuBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.helpMenuBtn.Image = global::MagniFile.Properties.Resources.help24;
            this.helpMenuBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpMenuBtn.Name = "helpMenuBtn";
            this.helpMenuBtn.Size = new System.Drawing.Size(52, 20);
            this.helpMenuBtn.Text = "Help";
            this.helpMenuBtn.ToolTipText = "Help";
            this.helpMenuBtn.Click += new System.EventHandler(this.helpMenuBtn_Click);
            // 
            // titleGlow
            // 
            this.titleGlow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleGlow.BackColor = System.Drawing.Color.Transparent;
            this.titleGlow.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleGlow.ForeColor = System.Drawing.Color.Maroon;
            this.titleGlow.Location = new System.Drawing.Point(34, 9);
            this.titleGlow.Name = "titleGlow";
            this.titleGlow.Size = new System.Drawing.Size(580, 36);
            this.titleGlow.TabIndex = 7;
            this.titleGlow.Text = "File System Information";
            this.titleGlow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // helpBtn
            // 
            this.helpBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.helpBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpBtn.Image = ((System.Drawing.Image)(resources.GetObject("helpBtn.Image")));
            this.helpBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.helpBtn.Location = new System.Drawing.Point(469, 830);
            this.helpBtn.Name = "helpBtn";
            this.helpBtn.Size = new System.Drawing.Size(75, 24);
            this.helpBtn.TabIndex = 14;
            this.helpBtn.Text = "Help";
            this.helpBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.helpBtn.UseVisualStyleBackColor = true;
            this.helpBtn.Click += new System.EventHandler(this.helpBtn_Click);
            // 
            // ViewFsInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Navy;
            this.ClientSize = new System.Drawing.Size(652, 866);
            this.Controls.Add(this.startClock);
            this.Controls.Add(this.currentClock);
            this.Controls.Add(this.helpBtn);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.perfCk);
            this.Controls.Add(this.diskCk);
            this.Controls.Add(this.driveCk);
            this.Controls.Add(this.cacheCk);
            this.Controls.Add(this.statsCk);
            this.Controls.Add(this.sysCk);
            this.Controls.Add(this.gStatToolContainer);
            this.Controls.Add(this.titleGlow);
            this.Controls.Add(this.title);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ViewFsInfo";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "MagniFile - File System Information";
            this.globalCfgMenu.ResumeLayout(false);
            this.gStatToolContainer.ContentPanel.ResumeLayout(false);
            this.gStatToolContainer.TopToolStripPanel.ResumeLayout(false);
            this.gStatToolContainer.TopToolStripPanel.PerformLayout();
            this.gStatToolContainer.ResumeLayout(false);
            this.gStatToolContainer.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView globalStatView;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.ColumnHeader col_class;
        private System.Windows.Forms.ColumnHeader col_desc;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ColumnHeader col_val1;
        private System.Windows.Forms.ColumnHeader col_change;
        private System.Windows.Forms.ColumnHeader col_val2;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip globalCfgMenu;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem printSetupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ImageList sortArrowImages;
        private System.Windows.Forms.ToolStripContainer gStatToolContainer;
        private System.Windows.Forms.ToolStrip toolStrip1;
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
        private System.Windows.Forms.CheckBox sysCk;
        private System.Windows.Forms.CheckBox driveCk;
        private System.Windows.Forms.CheckBox statsCk;
        private System.Windows.Forms.CheckBox cacheCk;
        private System.Windows.Forms.CheckBox diskCk;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.CheckBox perfCk;
        private System.Windows.Forms.Button helpBtn;
        private System.Windows.Forms.ToolStripButton helpMenuBtn;
        private System.Windows.Forms.Button currentClock;
        private System.Windows.Forms.Button startClock;
    }
}