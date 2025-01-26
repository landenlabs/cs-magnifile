namespace MagniFile
{
    partial class ViewDiskAllocation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewDiskAllocation));
            this.diskPicture = new System.Windows.Forms.PictureBox();
            this.closeBtn = new System.Windows.Forms.Button();
            this.driveCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.TextBox();
            this.refreshBtn = new System.Windows.Forms.Button();
            this.title = new System.Windows.Forms.Button();
            this.saveImageBtn = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.legendBox = new System.Windows.Forms.PictureBox();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.browseBtn = new System.Windows.Forms.Button();
            this.filepathBox = new System.Windows.Forms.TextBox();
            this.infoList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.splitter = new System.Windows.Forms.SplitContainer();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabDetails = new System.Windows.Forms.TabPage();
            this.tabSysFiles = new System.Windows.Forms.TabPage();
            this.sysFileView = new System.Windows.Forms.ListView();
            this.col0 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_sFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_sSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_sFrag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_sDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabUserFiles = new System.Windows.Forms.TabPage();
            this.userFileView = new System.Windows.Forms.ListView();
            this.col_u0 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_u1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_uFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_uSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_uFrag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_uDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.helpBtn = new System.Windows.Forms.Button();
            this.clusterLbl = new System.Windows.Forms.Label();
            this.clusterVal = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.diskPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.legendBox)).BeginInit();
            this.splitter.Panel1.SuspendLayout();
            this.splitter.Panel2.SuspendLayout();
            this.splitter.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabDetails.SuspendLayout();
            this.tabSysFiles.SuspendLayout();
            this.tabUserFiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // diskPicture
            // 
            this.diskPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.diskPicture.BackColor = System.Drawing.Color.Black;
            this.diskPicture.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("diskPicture.BackgroundImage")));
            this.diskPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.diskPicture.Location = new System.Drawing.Point(0, 0);
            this.diskPicture.Margin = new System.Windows.Forms.Padding(0);
            this.diskPicture.Name = "diskPicture";
            this.diskPicture.Size = new System.Drawing.Size(1024, 512);
            this.diskPicture.TabIndex = 0;
            this.diskPicture.TabStop = false;
            this.diskPicture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.diskPicture_MouseMove);
            // 
            // closeBtn
            // 
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBtn.Location = new System.Drawing.Point(1218, 606);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(75, 23);
            this.closeBtn.TabIndex = 1;
            this.closeBtn.Text = "Close";
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // driveCombo
            // 
            this.driveCombo.BackColor = System.Drawing.Color.Navy;
            this.driveCombo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.driveCombo.ForeColor = System.Drawing.Color.White;
            this.driveCombo.FormattingEnabled = true;
            this.driveCombo.Location = new System.Drawing.Point(119, 15);
            this.driveCombo.Name = "driveCombo";
            this.driveCombo.Size = new System.Drawing.Size(76, 21);
            this.driveCombo.TabIndex = 2;
            this.toolTip.SetToolTip(this.driveCombo, "Select Drive to Display");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(29, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Disk Drive:";
            // 
            // status
            // 
            this.status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.status.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.status.Location = new System.Drawing.Point(178, 606);
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Size = new System.Drawing.Size(948, 22);
            this.status.TabIndex = 5;
            // 
            // refreshBtn
            // 
            this.refreshBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("refreshBtn.BackgroundImage")));
            this.refreshBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.refreshBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshBtn.ForeColor = System.Drawing.Color.White;
            this.refreshBtn.Location = new System.Drawing.Point(201, 6);
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(75, 37);
            this.refreshBtn.TabIndex = 6;
            this.refreshBtn.Text = "Refresh";
            this.toolTip.SetToolTip(this.refreshBtn, "Refresh Cluster Allocation Image ");
            this.refreshBtn.UseVisualStyleBackColor = true;
            this.refreshBtn.Click += new System.EventHandler(this.refreshBtn_Click);
            this.refreshBtn.MouseEnter += new System.EventHandler(this.button_MouseEnter);
            this.refreshBtn.MouseLeave += new System.EventHandler(this.button_MouseLeave);
            // 
            // title
            // 
            this.title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.title.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("title.BackgroundImage")));
            this.title.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.ForeColor = System.Drawing.Color.White;
            this.title.Location = new System.Drawing.Point(363, 3);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(930, 40);
            this.title.TabIndex = 7;
            this.title.Text = "Disk Cluster Allocation";
            this.title.UseVisualStyleBackColor = true;
            this.title.MouseEnter += new System.EventHandler(this.button_MouseEnter);
            this.title.MouseLeave += new System.EventHandler(this.button_MouseLeave);
            // 
            // saveImageBtn
            // 
            this.saveImageBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("saveImageBtn.BackgroundImage")));
            this.saveImageBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.saveImageBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveImageBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveImageBtn.ForeColor = System.Drawing.Color.White;
            this.saveImageBtn.Location = new System.Drawing.Point(282, 6);
            this.saveImageBtn.Name = "saveImageBtn";
            this.saveImageBtn.Size = new System.Drawing.Size(75, 37);
            this.saveImageBtn.TabIndex = 8;
            this.saveImageBtn.Text = "SaveAs";
            this.toolTip.SetToolTip(this.saveImageBtn, "Save Screen As Image");
            this.saveImageBtn.UseVisualStyleBackColor = true;
            this.saveImageBtn.Click += new System.EventHandler(this.saveImageBtn_Click);
            this.saveImageBtn.MouseEnter += new System.EventHandler(this.button_MouseEnter);
            this.saveImageBtn.MouseLeave += new System.EventHandler(this.button_MouseLeave);
            // 
            // legendBox
            // 
            this.legendBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.legendBox.BackColor = System.Drawing.Color.Black;
            this.legendBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.legendBox.Location = new System.Drawing.Point(1024, 0);
            this.legendBox.Margin = new System.Windows.Forms.Padding(0);
            this.legendBox.Name = "legendBox";
            this.legendBox.Size = new System.Drawing.Size(19, 512);
            this.legendBox.TabIndex = 11;
            this.legendBox.TabStop = false;
            this.toolTip.SetToolTip(this.legendBox, resources.GetString("legendBox.ToolTip"));
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Png|*.png|Jpeg|*.jpg|Csv|*.csv|Txt|*.txt";
            this.saveFileDialog.Title = "Save Screen Image";
            // 
            // browseBtn
            // 
            this.browseBtn.Location = new System.Drawing.Point(32, 47);
            this.browseBtn.Name = "browseBtn";
            this.browseBtn.Size = new System.Drawing.Size(75, 23);
            this.browseBtn.TabIndex = 9;
            this.browseBtn.Text = "Browse...";
            this.browseBtn.UseVisualStyleBackColor = true;
            this.browseBtn.Click += new System.EventHandler(this.browseBtn_Click);
            // 
            // filepathBox
            // 
            this.filepathBox.AcceptsTab = true;
            this.filepathBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filepathBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.filepathBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.filepathBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.filepathBox.Location = new System.Drawing.Point(119, 49);
            this.filepathBox.Name = "filepathBox";
            this.filepathBox.Size = new System.Drawing.Size(1174, 20);
            this.filepathBox.TabIndex = 10;
            this.filepathBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.filepathBox_KeyUp);
            // 
            // infoList
            // 
            this.infoList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.infoList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoList.FullRowSelect = true;
            this.infoList.GridLines = true;
            this.infoList.HoverSelection = true;
            this.infoList.Location = new System.Drawing.Point(3, 3);
            this.infoList.Name = "infoList";
            this.infoList.Size = new System.Drawing.Size(200, 480);
            this.infoList.TabIndex = 12;
            this.infoList.UseCompatibleStateImageBehavior = false;
            this.infoList.View = System.Windows.Forms.View.Details;
            this.infoList.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.infoList_ItemMouseHover);
            this.infoList.DoubleClick += new System.EventHandler(this.infoList_DoubleClick);
            this.infoList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.infoList_MouseUp);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = ".";
            this.columnHeader1.Width = 0;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = ".";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = ".";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "";
            // 
            // openFileDialog
            // 
            this.openFileDialog.AddExtension = false;
            this.openFileDialog.Filter = "Any|*.*|Exe|*.exe";
            this.openFileDialog.Multiselect = true;
            this.openFileDialog.SupportMultiDottedExtensions = true;
            this.openFileDialog.Title = "Select File to View Allocation";
            this.openFileDialog.ValidateNames = false;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // splitter
            // 
            this.splitter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitter.BackColor = System.Drawing.Color.Purple;
            this.splitter.Location = new System.Drawing.Point(32, 80);
            this.splitter.Name = "splitter";
            // 
            // splitter.Panel1
            // 
            this.splitter.Panel1.Controls.Add(this.diskPicture);
            this.splitter.Panel1.Controls.Add(this.legendBox);
            // 
            // splitter.Panel2
            // 
            this.splitter.Panel2.Controls.Add(this.tabs);
            this.splitter.Panel2MinSize = 0;
            this.splitter.Size = new System.Drawing.Size(1261, 512);
            this.splitter.SplitterDistance = 1043;
            this.splitter.TabIndex = 14;
            this.splitter.SplitterMoving += new System.Windows.Forms.SplitterCancelEventHandler(this.splitter_SplitterMoving);
            this.splitter.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitter_SplitterMoved);
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabDetails);
            this.tabs.Controls.Add(this.tabSysFiles);
            this.tabs.Controls.Add(this.tabUserFiles);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(214, 512);
            this.tabs.TabIndex = 13;
            this.tabs.SelectedIndexChanged += new System.EventHandler(this.tabs_SelectedIndexChanged);
            this.tabs.MouseUp += new System.Windows.Forms.MouseEventHandler(this.infoList_MouseUp);
            // 
            // tabDetails
            // 
            this.tabDetails.Controls.Add(this.infoList);
            this.tabDetails.Location = new System.Drawing.Point(4, 22);
            this.tabDetails.Name = "tabDetails";
            this.tabDetails.Padding = new System.Windows.Forms.Padding(3);
            this.tabDetails.Size = new System.Drawing.Size(206, 486);
            this.tabDetails.TabIndex = 0;
            this.tabDetails.Text = "Details";
            this.tabDetails.UseVisualStyleBackColor = true;
            // 
            // tabSysFiles
            // 
            this.tabSysFiles.Controls.Add(this.sysFileView);
            this.tabSysFiles.Location = new System.Drawing.Point(4, 22);
            this.tabSysFiles.Name = "tabSysFiles";
            this.tabSysFiles.Padding = new System.Windows.Forms.Padding(3);
            this.tabSysFiles.Size = new System.Drawing.Size(206, 486);
            this.tabSysFiles.TabIndex = 1;
            this.tabSysFiles.Text = "SysFiles";
            this.tabSysFiles.UseVisualStyleBackColor = true;
            // 
            // sysFileView
            // 
            this.sysFileView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col0,
            this.col1,
            this.col_sFile,
            this.col_sSize,
            this.col_sFrag,
            this.col_sDate});
            this.sysFileView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysFileView.FullRowSelect = true;
            this.sysFileView.GridLines = true;
            this.sysFileView.HoverSelection = true;
            this.sysFileView.Location = new System.Drawing.Point(3, 3);
            this.sysFileView.Name = "sysFileView";
            this.sysFileView.Size = new System.Drawing.Size(200, 480);
            this.sysFileView.TabIndex = 13;
            this.sysFileView.UseCompatibleStateImageBehavior = false;
            this.sysFileView.View = System.Windows.Forms.View.Details;
            this.sysFileView.DoubleClick += new System.EventHandler(this.infoList_DoubleClick);
            // 
            // col0
            // 
            this.col0.Text = "";
            this.col0.Width = 0;
            // 
            // col1
            // 
            this.col1.Width = 0;
            // 
            // col_sFile
            // 
            this.col_sFile.Text = "File";
            // 
            // col_sSize
            // 
            this.col_sSize.Text = "Size";
            this.col_sSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_sFrag
            // 
            this.col_sFrag.Text = "#Frag";
            this.col_sFrag.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_sDate
            // 
            this.col_sDate.Text = "Date";
            // 
            // tabUserFiles
            // 
            this.tabUserFiles.Controls.Add(this.userFileView);
            this.tabUserFiles.Location = new System.Drawing.Point(4, 22);
            this.tabUserFiles.Name = "tabUserFiles";
            this.tabUserFiles.Size = new System.Drawing.Size(206, 486);
            this.tabUserFiles.TabIndex = 2;
            this.tabUserFiles.Text = "UserFiles";
            this.tabUserFiles.UseVisualStyleBackColor = true;
            // 
            // userFileView
            // 
            this.userFileView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_u0,
            this.col_u1,
            this.col_uFile,
            this.col_uSize,
            this.col_uFrag,
            this.col_uDate});
            this.userFileView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userFileView.FullRowSelect = true;
            this.userFileView.GridLines = true;
            this.userFileView.HoverSelection = true;
            this.userFileView.Location = new System.Drawing.Point(0, 0);
            this.userFileView.Name = "userFileView";
            this.userFileView.Size = new System.Drawing.Size(206, 486);
            this.userFileView.TabIndex = 13;
            this.userFileView.UseCompatibleStateImageBehavior = false;
            this.userFileView.View = System.Windows.Forms.View.Details;
            // 
            // col_u0
            // 
            this.col_u0.Text = "";
            this.col_u0.Width = 0;
            // 
            // col_u1
            // 
            this.col_u1.Text = "";
            this.col_u1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.col_u1.Width = 0;
            // 
            // col_uFile
            // 
            this.col_uFile.Text = "File";
            // 
            // col_uSize
            // 
            this.col_uSize.Text = "Size";
            this.col_uSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_uFrag
            // 
            this.col_uFrag.Text = "#Frag";
            this.col_uFrag.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_uDate
            // 
            this.col_uDate.Text = "Date";
            // 
            // helpBtn
            // 
            this.helpBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.helpBtn.Image = global::MagniFile.Properties.Resources.help24;
            this.helpBtn.Location = new System.Drawing.Point(1137, 606);
            this.helpBtn.Name = "helpBtn";
            this.helpBtn.Size = new System.Drawing.Size(75, 23);
            this.helpBtn.TabIndex = 15;
            this.helpBtn.Text = "Help";
            this.helpBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.helpBtn.UseVisualStyleBackColor = true;
            this.helpBtn.Click += new System.EventHandler(this.helpBtn_Click);
            // 
            // clusterLbl
            // 
            this.clusterLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clusterLbl.AutoSize = true;
            this.clusterLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clusterLbl.ForeColor = System.Drawing.Color.White;
            this.clusterLbl.Location = new System.Drawing.Point(35, 608);
            this.clusterLbl.Name = "clusterLbl";
            this.clusterLbl.Size = new System.Drawing.Size(50, 13);
            this.clusterLbl.TabIndex = 16;
            this.clusterLbl.Text = "Cluster:";
            // 
            // clusterVal
            // 
            this.clusterVal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clusterVal.AutoSize = true;
            this.clusterVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clusterVal.ForeColor = System.Drawing.Color.White;
            this.clusterVal.Location = new System.Drawing.Point(91, 611);
            this.clusterVal.Name = "clusterVal";
            this.clusterVal.Size = new System.Drawing.Size(0, 13);
            this.clusterVal.TabIndex = 17;
            // 
            // ViewDiskAllocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(1319, 641);
            this.Controls.Add(this.clusterVal);
            this.Controls.Add(this.clusterLbl);
            this.Controls.Add(this.helpBtn);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.filepathBox);
            this.Controls.Add(this.browseBtn);
            this.Controls.Add(this.saveImageBtn);
            this.Controls.Add(this.title);
            this.Controls.Add(this.refreshBtn);
            this.Controls.Add(this.status);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.driveCombo);
            this.Controls.Add(this.closeBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ViewDiskAllocation";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "MagniFile - ViewDiskAllocation";
            ((System.ComponentModel.ISupportInitialize)(this.diskPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.legendBox)).EndInit();
            this.splitter.Panel1.ResumeLayout(false);
            this.splitter.Panel2.ResumeLayout(false);
            this.splitter.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.tabDetails.ResumeLayout(false);
            this.tabSysFiles.ResumeLayout(false);
            this.tabUserFiles.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox diskPicture;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.ComboBox driveCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox status;
        private System.Windows.Forms.Button refreshBtn;
        private System.Windows.Forms.Button title;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button saveImageBtn;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button browseBtn;
        private System.Windows.Forms.TextBox filepathBox;
        private System.Windows.Forms.PictureBox legendBox;
        private System.Windows.Forms.ListView infoList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.SplitContainer splitter;
        private System.Windows.Forms.Button helpBtn;
        private System.Windows.Forms.Label clusterLbl;
        private System.Windows.Forms.Label clusterVal;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabDetails;
        private System.Windows.Forms.TabPage tabSysFiles;
        private System.Windows.Forms.ListView sysFileView;
        private System.Windows.Forms.ColumnHeader col0;
        private System.Windows.Forms.ColumnHeader col_sFile;
        private System.Windows.Forms.ColumnHeader col_sSize;
        private System.Windows.Forms.ColumnHeader col_sFrag;
        private System.Windows.Forms.ColumnHeader col_sDate;
        private System.Windows.Forms.TabPage tabUserFiles;
        private System.Windows.Forms.ListView userFileView;
        private System.Windows.Forms.ColumnHeader col_u0;
        private System.Windows.Forms.ColumnHeader col_u1;
        private System.Windows.Forms.ColumnHeader col_uFile;
        private System.Windows.Forms.ColumnHeader col_uSize;
        private System.Windows.Forms.ColumnHeader col_uFrag;
        private System.Windows.Forms.ColumnHeader col1;
        private System.Windows.Forms.ColumnHeader col_uDate;
    }
}