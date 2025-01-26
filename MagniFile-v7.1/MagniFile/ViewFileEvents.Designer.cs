namespace MagniFile
{
    partial class ViewFileEvents
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewFileEvents));
            this.closeBtn = new System.Windows.Forms.Button();
            this.toolContainer = new System.Windows.Forms.ToolStripContainer();
            this.splitter = new System.Windows.Forms.SplitContainer();
            this.monitorView = new System.Windows.Forms.ListView();
            this.col_enable = new System.Windows.Forms.ColumnHeader();
            this.col_monFile = new System.Windows.Forms.ColumnHeader();
            this.col_monSub = new System.Windows.Forms.ColumnHeader();
            this.col_pattern = new System.Windows.Forms.ColumnHeader();
            this.col_chg = new System.Windows.Forms.ColumnHeader();
            this.col_create = new System.Windows.Forms.ColumnHeader();
            this.col_delete = new System.Windows.Forms.ColumnHeader();
            this.col_rename = new System.Windows.Forms.ColumnHeader();
            this.col_color = new System.Windows.Forms.ColumnHeader();
            this.monitorPopMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortImageList = new System.Windows.Forms.ImageList(this.components);
            this.ckImageList = new System.Windows.Forms.ImageList(this.components);
            this.eventView = new System.Windows.Forms.ListView();
            this.col_stime = new System.Windows.Forms.ColumnHeader();
            this.col_ltime = new System.Windows.Forms.ColumnHeader();
            this.col_count = new System.Windows.Forms.ColumnHeader();
            this.col_colr = new System.Windows.Forms.ColumnHeader();
            this.col_event = new System.Windows.Forms.ColumnHeader();
            this.col_dir = new System.Windows.Forms.ColumnHeader();
            this.col_name = new System.Windows.Forms.ColumnHeader();
            this.col_ext = new System.Windows.Forms.ColumnHeader();
            this.col_size = new System.Windows.Forms.ColumnHeader();
            this.col_mDate = new System.Windows.Forms.ColumnHeader();
            this.col_attr = new System.Windows.Forms.ColumnHeader();
            this.col_owner = new System.Windows.Forms.ColumnHeader();
            this.eventPopMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.explorerDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.launchCmdexeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.addBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.delBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.pauseBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.exportBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.helpMenuBtn = new System.Windows.Forms.ToolStripButton();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.status = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpBtn = new System.Windows.Forms.Button();
            this.labelRight = new System.Windows.Forms.Button();
            this.labelLeft = new System.Windows.Forms.Button();
            this.labelCenter = new System.Windows.Forms.Button();
            this.toolContainer.ContentPanel.SuspendLayout();
            this.toolContainer.TopToolStripPanel.SuspendLayout();
            this.toolContainer.SuspendLayout();
            this.splitter.Panel1.SuspendLayout();
            this.splitter.Panel2.SuspendLayout();
            this.splitter.SuspendLayout();
            this.monitorPopMenu.SuspendLayout();
            this.eventPopMenu.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeBtn
            // 
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeBtn.Location = new System.Drawing.Point(964, 538);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(75, 23);
            this.closeBtn.TabIndex = 0;
            this.closeBtn.Text = "Close";
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // toolContainer
            // 
            this.toolContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.toolContainer.BottomToolStripPanelVisible = false;
            // 
            // toolContainer.ContentPanel
            // 
            this.toolContainer.ContentPanel.Controls.Add(this.splitter);
            this.toolContainer.ContentPanel.Size = new System.Drawing.Size(1023, 447);
            this.toolContainer.LeftToolStripPanelVisible = false;
            this.toolContainer.Location = new System.Drawing.Point(16, 48);
            this.toolContainer.Name = "toolContainer";
            this.toolContainer.RightToolStripPanelVisible = false;
            this.toolContainer.Size = new System.Drawing.Size(1023, 472);
            this.toolContainer.TabIndex = 2;
            this.toolContainer.Text = "toolStripContainer1";
            // 
            // toolContainer.TopToolStripPanel
            // 
            this.toolContainer.TopToolStripPanel.Controls.Add(this.toolStrip);
            // 
            // splitter
            // 
            this.splitter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.splitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitter.Location = new System.Drawing.Point(0, 0);
            this.splitter.Name = "splitter";
            this.splitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitter.Panel1
            // 
            this.splitter.Panel1.Controls.Add(this.monitorView);
            this.splitter.Panel1MinSize = 0;
            // 
            // splitter.Panel2
            // 
            this.splitter.Panel2.Controls.Add(this.eventView);
            this.splitter.Panel2MinSize = 0;
            this.splitter.Size = new System.Drawing.Size(1023, 447);
            this.splitter.SplitterDistance = 194;
            this.splitter.TabIndex = 0;
            // 
            // monitorView
            // 
            this.monitorView.CheckBoxes = true;
            this.monitorView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_enable,
            this.col_monFile,
            this.col_monSub,
            this.col_pattern,
            this.col_chg,
            this.col_create,
            this.col_delete,
            this.col_rename,
            this.col_color});
            this.monitorView.ContextMenuStrip = this.monitorPopMenu;
            this.monitorView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.monitorView.GridLines = true;
            this.monitorView.HideSelection = false;
            this.monitorView.Location = new System.Drawing.Point(0, 0);
            this.monitorView.Name = "monitorView";
            this.monitorView.Size = new System.Drawing.Size(1023, 194);
            this.monitorView.SmallImageList = this.sortImageList;
            this.monitorView.StateImageList = this.ckImageList;
            this.monitorView.TabIndex = 0;
            this.toolTip.SetToolTip(this.monitorView, resources.GetString("monitorView.ToolTip"));
            this.monitorView.UseCompatibleStateImageBehavior = false;
            this.monitorView.View = System.Windows.Forms.View.Details;
            this.monitorView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.monitorView_ItemChecked);
            this.monitorView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.monitorView_MouseUp);
            this.monitorView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ColumnClick);
            this.monitorView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.monitorView_KeyUp);
            // 
            // col_enable
            // 
            this.col_enable.Text = "Enable";
            this.col_enable.Width = 50;
            // 
            // col_monFile
            // 
            this.col_monFile.Text = "Base Directory";
            this.col_monFile.Width = 200;
            // 
            // col_monSub
            // 
            this.col_monSub.Text = "SubDir";
            this.col_monSub.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // col_pattern
            // 
            this.col_pattern.Text = "Pattern";
            this.col_pattern.Width = 100;
            // 
            // col_chg
            // 
            this.col_chg.Text = "Change";
            this.col_chg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_chg.Width = 55;
            // 
            // col_create
            // 
            this.col_create.Text = "Create";
            this.col_create.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_create.Width = 50;
            // 
            // col_delete
            // 
            this.col_delete.Text = "Delete";
            this.col_delete.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_delete.Width = 50;
            // 
            // col_rename
            // 
            this.col_rename.Text = "Rename";
            this.col_rename.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_rename.Width = 55;
            // 
            // col_color
            // 
            this.col_color.Text = "Color";
            // 
            // monitorPopMenu
            // 
            this.monitorPopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem});
            this.monitorPopMenu.Name = "monitorPopMenu";
            this.monitorPopMenu.Size = new System.Drawing.Size(113, 48);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.monitorPop_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.monitorPop_Click);
            // 
            // sortImageList
            // 
            this.sortImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("sortImageList.ImageStream")));
            this.sortImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.sortImageList.Images.SetKeyName(0, "upBW.png");
            this.sortImageList.Images.SetKeyName(1, "downBW.png");
            // 
            // ckImageList
            // 
            this.ckImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ckImageList.ImageStream")));
            this.ckImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ckImageList.Images.SetKeyName(0, "check-off.png");
            this.ckImageList.Images.SetKeyName(1, "smlCheck-on.png");
            // 
            // eventView
            // 
            this.eventView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_stime,
            this.col_ltime,
            this.col_count,
            this.col_colr,
            this.col_event,
            this.col_dir,
            this.col_name,
            this.col_ext,
            this.col_size,
            this.col_mDate,
            this.col_attr,
            this.col_owner});
            this.eventView.ContextMenuStrip = this.eventPopMenu;
            this.eventView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventView.FullRowSelect = true;
            this.eventView.GridLines = true;
            this.eventView.HideSelection = false;
            this.eventView.Location = new System.Drawing.Point(0, 0);
            this.eventView.Name = "eventView";
            this.eventView.Size = new System.Drawing.Size(1023, 249);
            this.eventView.SmallImageList = this.sortImageList;
            this.eventView.TabIndex = 1;
            this.eventView.UseCompatibleStateImageBehavior = false;
            this.eventView.View = System.Windows.Forms.View.Details;
            this.eventView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ColumnClick);
            // 
            // col_stime
            // 
            this.col_stime.Text = "First";
            this.col_stime.Width = 130;
            // 
            // col_ltime
            // 
            this.col_ltime.Text = "Last";
            this.col_ltime.Width = 130;
            // 
            // col_count
            // 
            this.col_count.Text = "Count";
            this.col_count.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.col_count.Width = 50;
            // 
            // col_colr
            // 
            this.col_colr.Text = "Color";
            this.col_colr.Width = 50;
            // 
            // col_event
            // 
            this.col_event.Text = "Event";
            // 
            // col_dir
            // 
            this.col_dir.Text = "Directory";
            this.col_dir.Width = 150;
            // 
            // col_name
            // 
            this.col_name.Text = "Name";
            this.col_name.Width = 120;
            // 
            // col_ext
            // 
            this.col_ext.Text = "Ext";
            this.col_ext.Width = 50;
            // 
            // col_size
            // 
            this.col_size.Text = "Size";
            this.col_size.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_mDate
            // 
            this.col_mDate.Text = "Modified";
            this.col_mDate.Width = 130;
            // 
            // col_attr
            // 
            this.col_attr.Text = "Attributes";
            // 
            // col_owner
            // 
            this.col_owner.Text = "Owner";
            // 
            // eventPopMenu
            // 
            this.eventPopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearAllToolStripMenuItem,
            this.clearSelectedToolStripMenuItem,
            this.toolStripSeparator5,
            this.explorerDirectoryToolStripMenuItem,
            this.launchCmdexeToolStripMenuItem});
            this.eventPopMenu.Name = "eventPopMenu";
            this.eventPopMenu.Size = new System.Drawing.Size(164, 98);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.clearAllToolStripMenuItem.Text = "Clear All";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.eventPopMenu_Click);
            // 
            // clearSelectedToolStripMenuItem
            // 
            this.clearSelectedToolStripMenuItem.Name = "clearSelectedToolStripMenuItem";
            this.clearSelectedToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.clearSelectedToolStripMenuItem.Text = "Clear Selected";
            this.clearSelectedToolStripMenuItem.Click += new System.EventHandler(this.eventPopMenu_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(160, 6);
            // 
            // explorerDirectoryToolStripMenuItem
            // 
            this.explorerDirectoryToolStripMenuItem.Name = "explorerDirectoryToolStripMenuItem";
            this.explorerDirectoryToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.explorerDirectoryToolStripMenuItem.Text = "Launch Explorer";
            this.explorerDirectoryToolStripMenuItem.Click += new System.EventHandler(this.eventPopMenu_Click);
            // 
            // launchCmdexeToolStripMenuItem
            // 
            this.launchCmdexeToolStripMenuItem.Name = "launchCmdexeToolStripMenuItem";
            this.launchCmdexeToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.launchCmdexeToolStripMenuItem.Text = "Launch cmd.exe";
            this.launchCmdexeToolStripMenuItem.Click += new System.EventHandler(this.eventPopMenu_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addBtn,
            this.toolStripSeparator1,
            this.delBtn,
            this.toolStripSeparator2,
            this.pauseBtn,
            this.toolStripSeparator3,
            this.exportBtn,
            this.toolStripSeparator6,
            this.helpMenuBtn});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1023, 25);
            this.toolStrip.Stretch = true;
            this.toolStrip.TabIndex = 0;
            // 
            // addBtn
            // 
            this.addBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addBtn.Image = ((System.Drawing.Image)(resources.GetObject("addBtn.Image")));
            this.addBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(30, 22);
            this.addBtn.Text = "Add";
            this.addBtn.ToolTipText = "Add new monitor event template";
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // delBtn
            // 
            this.delBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.delBtn.Image = ((System.Drawing.Image)(resources.GetObject("delBtn.Image")));
            this.delBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.delBtn.Name = "delBtn";
            this.delBtn.Size = new System.Drawing.Size(42, 22);
            this.delBtn.Text = "Delete";
            this.delBtn.ToolTipText = "Delete selected monitor event lines.";
            this.delBtn.Click += new System.EventHandler(this.delBtn_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // pauseBtn
            // 
            this.pauseBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.pauseBtn.Image = ((System.Drawing.Image)(resources.GetObject("pauseBtn.Image")));
            this.pauseBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pauseBtn.Name = "pauseBtn";
            this.pauseBtn.Size = new System.Drawing.Size(40, 22);
            this.pauseBtn.Text = "Pause";
            this.pauseBtn.Click += new System.EventHandler(this.pauseBtn_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // exportBtn
            // 
            this.exportBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.exportBtn.Image = ((System.Drawing.Image)(resources.GetObject("exportBtn.Image")));
            this.exportBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportBtn.Name = "exportBtn";
            this.exportBtn.Size = new System.Drawing.Size(43, 22);
            this.exportBtn.Text = "Export";
            this.exportBtn.Click += new System.EventHandler(this.exportBtn_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // helpMenuBtn
            // 
            this.helpMenuBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.helpMenuBtn.Image = global::MagniFile.Properties.Resources.help24;
            this.helpMenuBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpMenuBtn.Name = "helpMenuBtn";
            this.helpMenuBtn.Size = new System.Drawing.Size(48, 22);
            this.helpMenuBtn.Text = "Help";
            this.helpMenuBtn.Click += new System.EventHandler(this.helpMenuBtn_Click);
            // 
            // colorDialog
            // 
            this.colorDialog.AnyColor = true;
            this.colorDialog.FullOpen = true;
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // status
            // 
            this.status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.status.Location = new System.Drawing.Point(16, 540);
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Size = new System.Drawing.Size(864, 20);
            this.status.TabIndex = 5;
            // 
            // helpBtn
            // 
            this.helpBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.helpBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpBtn.Image = global::MagniFile.Properties.Resources.help24;
            this.helpBtn.Location = new System.Drawing.Point(886, 538);
            this.helpBtn.Name = "helpBtn";
            this.helpBtn.Size = new System.Drawing.Size(75, 23);
            this.helpBtn.TabIndex = 6;
            this.helpBtn.Text = "Help";
            this.helpBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.helpBtn.UseVisualStyleBackColor = true;
            this.helpBtn.Click += new System.EventHandler(this.helpBtn_Click);
            // 
            // labelRight
            // 
            this.labelRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelRight.BackgroundImage = global::MagniFile.Properties.Resources.ovalGrey;
            this.labelRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.labelRight.FlatAppearance.BorderSize = 0;
            this.labelRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRight.ForeColor = System.Drawing.Color.White;
            this.labelRight.Location = new System.Drawing.Point(844, 12);
            this.labelRight.Margin = new System.Windows.Forms.Padding(0);
            this.labelRight.Name = "labelRight";
            this.labelRight.Size = new System.Drawing.Size(195, 23);
            this.labelRight.TabIndex = 7;
            this.labelRight.Text = "Right";
            this.labelRight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelRight.UseVisualStyleBackColor = true;
            // 
            // labelLeft
            // 
            this.labelLeft.AutoSize = true;
            this.labelLeft.BackgroundImage = global::MagniFile.Properties.Resources.ovalGrey;
            this.labelLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.labelLeft.FlatAppearance.BorderSize = 0;
            this.labelLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLeft.ForeColor = System.Drawing.Color.White;
            this.labelLeft.Location = new System.Drawing.Point(16, 10);
            this.labelLeft.Margin = new System.Windows.Forms.Padding(0);
            this.labelLeft.Name = "labelLeft";
            this.labelLeft.Size = new System.Drawing.Size(70, 26);
            this.labelLeft.TabIndex = 8;
            this.labelLeft.Text = "Left";
            this.labelLeft.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelLeft.UseVisualStyleBackColor = true;
            // 
            // labelCenter
            // 
            this.labelCenter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCenter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("labelCenter.BackgroundImage")));
            this.labelCenter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.labelCenter.FlatAppearance.BorderSize = 0;
            this.labelCenter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelCenter.Font = new System.Drawing.Font("Footlight MT Light", 14.25F);
            this.labelCenter.ForeColor = System.Drawing.Color.White;
            this.labelCenter.Location = new System.Drawing.Point(369, 9);
            this.labelCenter.Margin = new System.Windows.Forms.Padding(0);
            this.labelCenter.Name = "labelCenter";
            this.labelCenter.Size = new System.Drawing.Size(280, 27);
            this.labelCenter.TabIndex = 9;
            this.labelCenter.Text = "Monitor File Events";
            this.labelCenter.UseVisualStyleBackColor = true;
            // 
            // ViewFileEvents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Navy;
            this.ClientSize = new System.Drawing.Size(1051, 573);
            this.Controls.Add(this.labelCenter);
            this.Controls.Add(this.labelLeft);
            this.Controls.Add(this.helpBtn);
            this.Controls.Add(this.status);
            this.Controls.Add(this.labelRight);
            this.Controls.Add(this.toolContainer);
            this.Controls.Add(this.closeBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ViewFileEvents";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "ViewFileEvents";
            this.toolContainer.ContentPanel.ResumeLayout(false);
            this.toolContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolContainer.TopToolStripPanel.PerformLayout();
            this.toolContainer.ResumeLayout(false);
            this.toolContainer.PerformLayout();
            this.splitter.Panel1.ResumeLayout(false);
            this.splitter.Panel2.ResumeLayout(false);
            this.splitter.ResumeLayout(false);
            this.monitorPopMenu.ResumeLayout(false);
            this.eventPopMenu.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.ToolStripContainer toolContainer;
        private System.Windows.Forms.SplitContainer splitter;
        private System.Windows.Forms.ListView monitorView;
        private System.Windows.Forms.ColumnHeader col_monFile;
        private System.Windows.Forms.ListView eventView;
        private System.Windows.Forms.ColumnHeader col_ltime;
        private System.Windows.Forms.ColumnHeader col_event;
        private System.Windows.Forms.ColumnHeader col_dir;
        private System.Windows.Forms.ColumnHeader col_name;
        private System.Windows.Forms.ColumnHeader col_ext;
        private System.Windows.Forms.ColumnHeader col_size;
        private System.Windows.Forms.ColumnHeader col_mDate;
        private System.Windows.Forms.ColumnHeader col_attr;
        private System.Windows.Forms.ColumnHeader col_owner;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton addBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton delBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ColumnHeader col_monSub;
        private System.Windows.Forms.ColumnHeader col_pattern;
        private System.Windows.Forms.ColumnHeader col_color;
        private System.Windows.Forms.ColumnHeader col_chg;
        private System.Windows.Forms.ToolStripButton pauseBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton exportBtn;
        private System.Windows.Forms.ImageList ckImageList;
        private System.Windows.Forms.ImageList sortImageList;
        private System.Windows.Forms.ColumnHeader col_create;
        private System.Windows.Forms.ColumnHeader col_delete;
        private System.Windows.Forms.ColumnHeader col_rename;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.ColumnHeader col_enable;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ColumnHeader col_colr;
        private System.Windows.Forms.TextBox status;
        private System.Windows.Forms.ColumnHeader col_stime;
        private System.Windows.Forms.ColumnHeader col_count;
        private System.Windows.Forms.ContextMenuStrip eventPopMenu;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem explorerDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem launchCmdexeToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ContextMenuStrip monitorPopMenu;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton helpMenuBtn;
        private System.Windows.Forms.Button helpBtn;
        private System.Windows.Forms.Button labelRight;
        private System.Windows.Forms.Button labelLeft;
        private System.Windows.Forms.Button labelCenter;
    }
}