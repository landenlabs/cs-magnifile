namespace MagniFile
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.procStatus = new System.Windows.Forms.TextBox();
            this.procToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.procAutoUpdBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.updateRateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prooc1secBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.proc5secBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.proc1minBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.proc5minBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.proc1hourBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.procCustomBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.procFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.procAutoLogBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.procLogFileBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.loToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.procLogOpenBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.procExpBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.printSetupProcDetailMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.printPreviewProcDetailMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.printProcDetailMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.fitToPageProcDetailMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.procViewBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.procRefreshBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.procClock = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.procLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.procExportMenu = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.procLogMenu = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.procPrintMenu = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.procPreviewMenu = new System.Windows.Forms.ToolStripButton();
            this.procSumView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.cols_pid = new System.Windows.Forms.ColumnHeader();
            this.cols_name = new System.Windows.Forms.ColumnHeader();
            this.cols_ioRead = new System.Windows.Forms.ColumnHeader();
            this.cols_ioWrite = new System.Windows.Forms.ColumnHeader();
            this.cols_ioOther = new System.Windows.Forms.ColumnHeader();
            this.cols_iooChg = new System.Windows.Forms.ColumnHeader();
            this.cols_nHnd = new System.Windows.Forms.ColumnHeader();
            this.cols_start = new System.Windows.Forms.ColumnHeader();
            this.cols_totTime = new System.Windows.Forms.ColumnHeader();
            this.cols_userTime = new System.Windows.Forms.ColumnHeader();
            this.cols_privMem = new System.Windows.Forms.ColumnHeader();
            this.cols_wkSet = new System.Windows.Forms.ColumnHeader();
            this.cols_wkSetP = new System.Windows.Forms.ColumnHeader();
            this.cols_virt = new System.Windows.Forms.ColumnHeader();
            this.cols_virtP = new System.Windows.Forms.ColumnHeader();
            this.cols_pageSize = new System.Windows.Forms.ColumnHeader();
            this.cols_pgSizeP = new System.Windows.Forms.ColumnHeader();
            this.cols_nThreads = new System.Windows.Forms.ColumnHeader();
            this.cols_nMod = new System.Windows.Forms.ColumnHeader();
            this.procSumPopupMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeSelectedItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processInformationMenuBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.sortImages = new System.Windows.Forms.ImageList(this.components);
            this.procView = new System.Windows.Forms.ListView();
            this.col0 = new System.Windows.Forms.ColumnHeader();
            this.col_log = new System.Windows.Forms.ColumnHeader();
            this.col_id = new System.Windows.Forms.ColumnHeader();
            this.col_name = new System.Windows.Forms.ColumnHeader();
            this.col_IOread = new System.Windows.Forms.ColumnHeader();
            this.col_IOwrite = new System.Windows.Forms.ColumnHeader();
            this.col_IOother = new System.Windows.Forms.ColumnHeader();
            this.col_ioChg = new System.Windows.Forms.ColumnHeader();
            this.col_handles = new System.Windows.Forms.ColumnHeader();
            this.col_start = new System.Windows.Forms.ColumnHeader();
            this.col_totTime = new System.Windows.Forms.ColumnHeader();
            this.col_userTime = new System.Windows.Forms.ColumnHeader();
            this.col_privMem = new System.Windows.Forms.ColumnHeader();
            this.col_work = new System.Windows.Forms.ColumnHeader();
            this.col_wkPeak = new System.Windows.Forms.ColumnHeader();
            this.col_virt = new System.Windows.Forms.ColumnHeader();
            this.col_virtPeakSize = new System.Windows.Forms.ColumnHeader();
            this.col_paged = new System.Windows.Forms.ColumnHeader();
            this.col_pagedPeak = new System.Windows.Forms.ColumnHeader();
            this.col_Threads = new System.Windows.Forms.ColumnHeader();
            this.col_Modules = new System.Windows.Forms.ColumnHeader();
            this.procDetailPopupMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.procInfoDetailMenuBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hndSumView = new System.Windows.Forms.ListView();
            this.col_tm = new System.Windows.Forms.ColumnHeader();
            this.col_pid = new System.Windows.Forms.ColumnHeader();
            this.col_nm = new System.Windows.Forms.ColumnHeader();
            this.col_tot = new System.Windows.Forms.ColumnHeader();
            this.resizePanel = new System.Windows.Forms.Panel();
            this.handleStatus = new System.Windows.Forms.TextBox();
            this.closeBtn = new System.Windows.Forms.Button();
            this.handleView = new System.Windows.Forms.ListView();
            this.colH0 = new System.Windows.Forms.ColumnHeader();
            this.colH_pid = new System.Windows.Forms.ColumnHeader();
            this.col_procName = new System.Windows.Forms.ColumnHeader();
            this.colH_handle = new System.Windows.Forms.ColumnHeader();
            this.colH_object = new System.Windows.Forms.ColumnHeader();
            this.col_desc = new System.Windows.Forms.ColumnHeader();
            this.col_age = new System.Windows.Forms.ColumnHeader();
            this.col_stime = new System.Windows.Forms.ColumnHeader();
            this.col_dup = new System.Windows.Forms.ColumnHeader();
            this.hndPopupMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.hndPopupAutoSizeBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hndPopupExplorerBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hndPopupCmdBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hndPopupRegEditBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.fileMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cntDupFilesBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.cleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.handleToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSplitButton2 = new System.Windows.Forms.ToolStripSplitButton();
            this.hndAutoUpdBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hndRateBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hnd1secBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hnd5secBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hnd1minBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hnd5minBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hnd1hourBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hndCustomBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.descFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.hndAutoLogBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hndLogFileBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.openLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hndExportBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
            this.hndPrSetBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hndPrPreviewBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hndPrBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hndFitToPageBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.hndViewBtn = new System.Windows.Forms.ToolStripSplitButton();
            this.hndHideNoDescBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hndShowAllBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.groupByToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hndNoGrpBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hndPidGrpBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hndObjGrpBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hndSummaryBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hndClearSumBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.hndShowClosedCk = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.hndRefreshBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.hndClock = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.handleLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.hndExportMenu = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.hndLogMenu = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.hndPrintMenu = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator20 = new System.Windows.Forms.ToolStripSeparator();
            this.hndPreviewMenu = new System.Windows.Forms.ToolStripLabel();
            this.procTimer = new System.Windows.Forms.Timer(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.exportFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.logFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.handleTimer = new System.Windows.Forms.Timer(this.components);
            this.logOpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.diskDetailBtn = new System.Windows.Forms.ToolStripButton();
            this.fsInfoBtn = new System.Windows.Forms.ToolStripButton();
            this.graphBtn = new System.Windows.Forms.ToolStripButton();
            this.graphPlusBtn = new System.Windows.Forms.ToolStripLabel();
            this.diskClusterBtn = new System.Windows.Forms.ToolStripButton();
            this.titleLeft = new System.Windows.Forms.ToolStripLabel();
            this.title = new System.Windows.Forms.ToolStripLabel();
            this.netInfoBtn = new System.Windows.Forms.ToolStripButton();
            this.fileListBtn = new System.Windows.Forms.ToolStripButton();
            this.monFileBtn = new System.Windows.Forms.ToolStripButton();
            this.titleRight = new System.Windows.Forms.ToolStripLabel();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.procToolStrip.SuspendLayout();
            this.procSumPopupMenu.SuspendLayout();
            this.procDetailPopupMenu.SuspendLayout();
            this.hndPopupMenu.SuspendLayout();
            this.handleToolStrip.SuspendLayout();
            this.mainToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Blue;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 71);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.splitContainer1.Panel1MinSize = 0;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1180, 558);
            this.splitContainer1.SplitterDistance = 0;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.Color.Black;
            this.splitContainer2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("splitContainer2.BackgroundImage")));
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainer2.Panel1.Controls.Add(this.procStatus);
            this.splitContainer2.Panel1.Controls.Add(this.procToolStrip);
            this.splitContainer2.Panel1.Controls.Add(this.procSumView);
            this.splitContainer2.Panel1.Controls.Add(this.procView);
            this.splitContainer2.Panel1MinSize = 0;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.splitContainer2.Panel2.Controls.Add(this.hndSumView);
            this.splitContainer2.Panel2.Controls.Add(this.resizePanel);
            this.splitContainer2.Panel2.Controls.Add(this.handleStatus);
            this.splitContainer2.Panel2.Controls.Add(this.closeBtn);
            this.splitContainer2.Panel2.Controls.Add(this.handleView);
            this.splitContainer2.Panel2.Controls.Add(this.handleToolStrip);
            this.splitContainer2.Panel2MinSize = 0;
            this.splitContainer2.Size = new System.Drawing.Size(1176, 558);
            this.splitContainer2.SplitterDistance = 335;
            this.splitContainer2.SplitterWidth = 10;
            this.splitContainer2.TabIndex = 0;
            // 
            // procStatus
            // 
            this.procStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.procStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.procStatus.Location = new System.Drawing.Point(1, 315);
            this.procStatus.Margin = new System.Windows.Forms.Padding(0);
            this.procStatus.Name = "procStatus";
            this.procStatus.ReadOnly = true;
            this.procStatus.Size = new System.Drawing.Size(1175, 20);
            this.procStatus.TabIndex = 2;
            // 
            // procToolStrip
            // 
            this.procToolStrip.BackColor = System.Drawing.Color.SteelBlue;
            this.procToolStrip.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("procToolStrip.BackgroundImage")));
            this.procToolStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.procToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.procToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton1,
            this.toolStripSeparator10,
            this.procViewBtn,
            this.toolStripSeparator8,
            this.procRefreshBtn,
            this.toolStripSeparator5,
            this.procClock,
            this.toolStripSeparator6,
            this.procLabel,
            this.toolStripSeparator2,
            this.procExportMenu,
            this.toolStripSeparator7,
            this.procLogMenu,
            this.toolStripSeparator9,
            this.procPrintMenu,
            this.toolStripSeparator17,
            this.procPreviewMenu});
            this.procToolStrip.Location = new System.Drawing.Point(0, 0);
            this.procToolStrip.Name = "procToolStrip";
            this.procToolStrip.Size = new System.Drawing.Size(1176, 25);
            this.procToolStrip.Stretch = true;
            this.procToolStrip.TabIndex = 0;
            this.procToolStrip.Text = "toolStrip2";
            this.procToolStrip.MouseEnter += new System.EventHandler(this.procToolStrip_MouseEnter);
            this.procToolStrip.MouseLeave += new System.EventHandler(this.procToolStrip_MouseLeave);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.procAutoUpdBtn,
            this.updateRateToolStripMenuItem,
            this.procFilterToolStripMenuItem,
            this.toolStripSeparator15,
            this.procAutoLogBtn,
            this.procLogFileBtn,
            this.loToolStripMenuItem,
            this.procLogOpenBtn,
            this.procExpBtn,
            this.toolStripSeparator16,
            this.printSetupProcDetailMenu,
            this.printPreviewProcDetailMenu,
            this.printProcDetailMenu,
            this.fitToPageProcDetailMenu});
            this.toolStripSplitButton1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripSplitButton1.ForeColor = System.Drawing.Color.White;
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(44, 22);
            this.toolStripSplitButton1.Text = "File";
            this.toolStripSplitButton1.ToolTipText = "Update, log, export and print";
            // 
            // procAutoUpdBtn
            // 
            this.procAutoUpdBtn.CheckOnClick = true;
            this.procAutoUpdBtn.Name = "procAutoUpdBtn";
            this.procAutoUpdBtn.Size = new System.Drawing.Size(163, 22);
            this.procAutoUpdBtn.Text = "Auto Update";
            this.procAutoUpdBtn.ToolTipText = "Toggle automatic update of display (use Update Rate to set update frequency).";
            this.procAutoUpdBtn.Click += new System.EventHandler(this.procFileMenu_Click);
            // 
            // updateRateToolStripMenuItem
            // 
            this.updateRateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.prooc1secBtn,
            this.proc5secBtn,
            this.proc1minBtn,
            this.proc5minBtn,
            this.proc1hourBtn,
            this.procCustomBtn});
            this.updateRateToolStripMenuItem.Name = "updateRateToolStripMenuItem";
            this.updateRateToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.updateRateToolStripMenuItem.Text = "Update  Rate";
            // 
            // prooc1secBtn
            // 
            this.prooc1secBtn.Name = "prooc1secBtn";
            this.prooc1secBtn.Size = new System.Drawing.Size(129, 22);
            this.prooc1secBtn.Tag = 1;
            this.prooc1secBtn.Text = "1 Second";
            this.prooc1secBtn.Click += new System.EventHandler(this.procUpdRate_Click);
            // 
            // proc5secBtn
            // 
            this.proc5secBtn.Name = "proc5secBtn";
            this.proc5secBtn.Size = new System.Drawing.Size(129, 22);
            this.proc5secBtn.Tag = 5;
            this.proc5secBtn.Text = "5 Second";
            this.proc5secBtn.Click += new System.EventHandler(this.procUpdRate_Click);
            // 
            // proc1minBtn
            // 
            this.proc1minBtn.Name = "proc1minBtn";
            this.proc1minBtn.Size = new System.Drawing.Size(129, 22);
            this.proc1minBtn.Tag = 60;
            this.proc1minBtn.Text = "1 Minute";
            this.proc1minBtn.Click += new System.EventHandler(this.procUpdRate_Click);
            // 
            // proc5minBtn
            // 
            this.proc5minBtn.Name = "proc5minBtn";
            this.proc5minBtn.Size = new System.Drawing.Size(129, 22);
            this.proc5minBtn.Tag = 300;
            this.proc5minBtn.Text = "5 Minute";
            this.proc5minBtn.Click += new System.EventHandler(this.procUpdRate_Click);
            // 
            // proc1hourBtn
            // 
            this.proc1hourBtn.Name = "proc1hourBtn";
            this.proc1hourBtn.Size = new System.Drawing.Size(129, 22);
            this.proc1hourBtn.Tag = 3600;
            this.proc1hourBtn.Text = "Hourly";
            this.proc1hourBtn.Click += new System.EventHandler(this.procUpdRate_Click);
            // 
            // procCustomBtn
            // 
            this.procCustomBtn.Enabled = false;
            this.procCustomBtn.Name = "procCustomBtn";
            this.procCustomBtn.Size = new System.Drawing.Size(129, 22);
            this.procCustomBtn.Tag = -1;
            this.procCustomBtn.Text = "Custom";
            this.procCustomBtn.Click += new System.EventHandler(this.procUpdRate_Click);
            // 
            // procFilterToolStripMenuItem
            // 
            this.procFilterToolStripMenuItem.Enabled = false;
            this.procFilterToolStripMenuItem.Name = "procFilterToolStripMenuItem";
            this.procFilterToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.procFilterToolStripMenuItem.Text = "Filter...";
            this.procFilterToolStripMenuItem.ToolTipText = "Reduce process list by defining filters.";
            this.procFilterToolStripMenuItem.Visible = false;
            this.procFilterToolStripMenuItem.Click += new System.EventHandler(this.procFileMenu_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(160, 6);
            // 
            // procAutoLogBtn
            // 
            this.procAutoLogBtn.CheckOnClick = true;
            this.procAutoLogBtn.Name = "procAutoLogBtn";
            this.procAutoLogBtn.Size = new System.Drawing.Size(163, 22);
            this.procAutoLogBtn.Text = "Auto Log";
            this.procAutoLogBtn.ToolTipText = "Toggle whether screen updates are also exported to log file.";
            this.procAutoLogBtn.Click += new System.EventHandler(this.procFileMenu_Click);
            // 
            // procLogFileBtn
            // 
            this.procLogFileBtn.Name = "procLogFileBtn";
            this.procLogFileBtn.Size = new System.Drawing.Size(163, 22);
            this.procLogFileBtn.Text = "Log File...";
            this.procLogFileBtn.ToolTipText = "Define file to store process list information.";
            this.procLogFileBtn.Click += new System.EventHandler(this.procFileMenu_Click);
            // 
            // loToolStripMenuItem
            // 
            this.loToolStripMenuItem.Name = "loToolStripMenuItem";
            this.loToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.loToolStripMenuItem.Text = "Load Log File...";
            this.loToolStripMenuItem.Visible = false;
            this.loToolStripMenuItem.Click += new System.EventHandler(this.procFileMenu_Click);
            // 
            // procLogOpenBtn
            // 
            this.procLogOpenBtn.Name = "procLogOpenBtn";
            this.procLogOpenBtn.Size = new System.Drawing.Size(163, 22);
            this.procLogOpenBtn.Text = "Open log file";
            this.procLogOpenBtn.ToolTipText = "Open current Log file with default  external viewer (ex notepad or excel)";
            this.procLogOpenBtn.Click += new System.EventHandler(this.procFileMenu_Click);
            // 
            // procExpBtn
            // 
            this.procExpBtn.Name = "procExpBtn";
            this.procExpBtn.Size = new System.Drawing.Size(163, 22);
            this.procExpBtn.Text = "Export...";
            this.procExpBtn.ToolTipText = "Export current process information to a text file.";
            this.procExpBtn.Click += new System.EventHandler(this.procFileMenu_Click);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(160, 6);
            // 
            // printSetupProcDetailMenu
            // 
            this.printSetupProcDetailMenu.Name = "printSetupProcDetailMenu";
            this.printSetupProcDetailMenu.Size = new System.Drawing.Size(163, 22);
            this.printSetupProcDetailMenu.Text = "Print Setup...";
            this.printSetupProcDetailMenu.ToolTipText = "Select printer";
            this.printSetupProcDetailMenu.Click += new System.EventHandler(this.procFileMenu_Click);
            // 
            // printPreviewProcDetailMenu
            // 
            this.printPreviewProcDetailMenu.Name = "printPreviewProcDetailMenu";
            this.printPreviewProcDetailMenu.Size = new System.Drawing.Size(163, 22);
            this.printPreviewProcDetailMenu.Text = "Print Preview...";
            this.printPreviewProcDetailMenu.ToolTipText = "Print to preview";
            this.printPreviewProcDetailMenu.Click += new System.EventHandler(this.procFileMenu_Click);
            // 
            // printProcDetailMenu
            // 
            this.printProcDetailMenu.Name = "printProcDetailMenu";
            this.printProcDetailMenu.Size = new System.Drawing.Size(163, 22);
            this.printProcDetailMenu.Text = "Print...";
            this.printProcDetailMenu.Click += new System.EventHandler(this.procFileMenu_Click);
            // 
            // fitToPageProcDetailMenu
            // 
            this.fitToPageProcDetailMenu.Checked = true;
            this.fitToPageProcDetailMenu.CheckOnClick = true;
            this.fitToPageProcDetailMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fitToPageProcDetailMenu.Name = "fitToPageProcDetailMenu";
            this.fitToPageProcDetailMenu.Size = new System.Drawing.Size(163, 22);
            this.fitToPageProcDetailMenu.Text = "Fit To Page";
            this.fitToPageProcDetailMenu.ToolTipText = "Auto shrink up to 20% to fit on printer page";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
            // 
            // procViewBtn
            // 
            this.procViewBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.procViewBtn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.procViewBtn.ForeColor = System.Drawing.Color.White;
            this.procViewBtn.Image = ((System.Drawing.Image)(resources.GetObject("procViewBtn.Image")));
            this.procViewBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.procViewBtn.Name = "procViewBtn";
            this.procViewBtn.Size = new System.Drawing.Size(67, 22);
            this.procViewBtn.Text = "Summary";
            this.procViewBtn.Click += new System.EventHandler(this.procViewBtn_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // procRefreshBtn
            // 
            this.procRefreshBtn.AutoToolTip = false;
            this.procRefreshBtn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.procRefreshBtn.ForeColor = System.Drawing.Color.White;
            this.procRefreshBtn.Image = global::MagniFile.Properties.Resources.red24;
            this.procRefreshBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.procRefreshBtn.Name = "procRefreshBtn";
            this.procRefreshBtn.Size = new System.Drawing.Size(72, 22);
            this.procRefreshBtn.Text = "Refresh";
            this.procRefreshBtn.ToolTipText = "Refresh  Process Display";
            this.procRefreshBtn.Click += new System.EventHandler(this.procRefreshBtn_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // procClock
            // 
            this.procClock.AutoSize = false;
            this.procClock.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.procClock.ForeColor = System.Drawing.Color.White;
            this.procClock.Name = "procClock";
            this.procClock.Size = new System.Drawing.Size(70, 22);
            this.procClock.Text = "88:88:88";
            this.procClock.ToolTipText = "Time of last Process update";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // procLabel
            // 
            this.procLabel.AutoSize = false;
            this.procLabel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.procLabel.ForeColor = System.Drawing.Color.White;
            this.procLabel.Name = "procLabel";
            this.procLabel.Size = new System.Drawing.Size(500, 22);
            this.procLabel.Text = "Process List";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // procExportMenu
            // 
            this.procExportMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.procExportMenu.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.procExportMenu.ForeColor = System.Drawing.Color.White;
            this.procExportMenu.Image = global::MagniFile.Properties.Resources.expoort;
            this.procExportMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.procExportMenu.Margin = new System.Windows.Forms.Padding(0);
            this.procExportMenu.Name = "procExportMenu";
            this.procExportMenu.Size = new System.Drawing.Size(76, 25);
            this.procExportMenu.Text = "Export...";
            this.procExportMenu.ToolTipText = "Export entire process list to disk file as csv\r\n";
            this.procExportMenu.Click += new System.EventHandler(this.procFileMenu_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // procLogMenu
            // 
            this.procLogMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.procLogMenu.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.procLogMenu.ForeColor = System.Drawing.Color.White;
            this.procLogMenu.Image = ((System.Drawing.Image)(resources.GetObject("procLogMenu.Image")));
            this.procLogMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.procLogMenu.Name = "procLogMenu";
            this.procLogMenu.Size = new System.Drawing.Size(32, 22);
            this.procLogMenu.Text = "Log";
            this.procLogMenu.ToolTipText = "Export process lines marked with  \'log\' tag  to log file.\r\nUse File  button to co" +
                "nfigure automatic logging.";
            this.procLogMenu.Click += new System.EventHandler(this.procFileMenu_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // procPrintMenu
            // 
            this.procPrintMenu.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.procPrintMenu.ForeColor = System.Drawing.Color.White;
            this.procPrintMenu.Image = global::MagniFile.Properties.Resources.print;
            this.procPrintMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.procPrintMenu.Name = "procPrintMenu";
            this.procPrintMenu.Size = new System.Drawing.Size(54, 22);
            this.procPrintMenu.Text = "Print";
            this.procPrintMenu.ToolTipText = "Print Process List";
            this.procPrintMenu.Click += new System.EventHandler(this.procFileMenu_Click);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(6, 25);
            // 
            // procPreviewMenu
            // 
            this.procPreviewMenu.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.procPreviewMenu.ForeColor = System.Drawing.Color.White;
            this.procPreviewMenu.Image = global::MagniFile.Properties.Resources.printPreview;
            this.procPreviewMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.procPreviewMenu.Name = "procPreviewMenu";
            this.procPreviewMenu.Size = new System.Drawing.Size(73, 22);
            this.procPreviewMenu.Text = "Preview";
            this.procPreviewMenu.ToolTipText = "Preview printing process list";
            this.procPreviewMenu.Click += new System.EventHandler(this.procFileMenu_Click);
            // 
            // procSumView
            // 
            this.procSumView.AllowColumnReorder = true;
            this.procSumView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.procSumView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.cols_pid,
            this.cols_name,
            this.cols_ioRead,
            this.cols_ioWrite,
            this.cols_ioOther,
            this.cols_iooChg,
            this.cols_nHnd,
            this.cols_start,
            this.cols_totTime,
            this.cols_userTime,
            this.cols_privMem,
            this.cols_wkSet,
            this.cols_wkSetP,
            this.cols_virt,
            this.cols_virtP,
            this.cols_pageSize,
            this.cols_pgSizeP,
            this.cols_nThreads,
            this.cols_nMod});
            this.procSumView.ContextMenuStrip = this.procSumPopupMenu;
            this.procSumView.FullRowSelect = true;
            this.procSumView.GridLines = true;
            this.procSumView.Location = new System.Drawing.Point(0, 22);
            this.procSumView.Margin = new System.Windows.Forms.Padding(0);
            this.procSumView.Name = "procSumView";
            this.procSumView.Size = new System.Drawing.Size(1176, 290);
            this.procSumView.SmallImageList = this.sortImages;
            this.procSumView.TabIndex = 3;
            this.toolTip.SetToolTip(this.procSumView, "Historical list of \'log\' tagged process information.  Click \'log\' column in detai" +
                    "led process view to mark items for inclusion. Table updated when process view is" +
                    " refreshed.");
            this.procSumView.UseCompatibleStateImageBehavior = false;
            this.procSumView.View = System.Windows.Forms.View.Details;
            this.procSumView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ColumnClick);
            this.procSumView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.procSumView_MouseMove);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "-";
            this.columnHeader1.Width = 0;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Time";
            this.columnHeader2.Width = 140;
            // 
            // cols_pid
            // 
            this.cols_pid.Text = "ProcId";
            this.cols_pid.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.cols_pid.Width = 47;
            // 
            // cols_name
            // 
            this.cols_name.Text = "Name";
            this.cols_name.Width = 130;
            // 
            // cols_ioRead
            // 
            this.cols_ioRead.Text = "#Read I/O";
            this.cols_ioRead.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cols_ioWrite
            // 
            this.cols_ioWrite.Text = "#Write I/O";
            this.cols_ioWrite.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cols_ioOther
            // 
            this.cols_ioOther.Text = "#Other I/O";
            this.cols_ioOther.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cols_iooChg
            // 
            this.cols_iooChg.Text = "I/O Chg";
            this.cols_iooChg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cols_nHnd
            // 
            this.cols_nHnd.Text = "#Handles";
            this.cols_nHnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cols_start
            // 
            this.cols_start.Text = "Start";
            this.cols_start.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.cols_start.Width = 150;
            // 
            // cols_totTime
            // 
            this.cols_totTime.Text = "TotalTm";
            this.cols_totTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cols_userTime
            // 
            this.cols_userTime.Text = "UserTm";
            this.cols_userTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cols_privMem
            // 
            this.cols_privMem.Text = "PrivateMem";
            this.cols_privMem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cols_wkSet
            // 
            this.cols_wkSet.Text = "WorkSet";
            this.cols_wkSet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cols_wkSetP
            // 
            this.cols_wkSetP.Text = "WorkSet (Peak)";
            this.cols_wkSetP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cols_virt
            // 
            this.cols_virt.Text = "Virtual Size";
            this.cols_virt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cols_virtP
            // 
            this.cols_virtP.Text = "Virtual (Peak) Size";
            this.cols_virtP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cols_pageSize
            // 
            this.cols_pageSize.Text = "Paged Size";
            this.cols_pageSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cols_pgSizeP
            // 
            this.cols_pgSizeP.Text = "Paged (Peak) Size";
            this.cols_pgSizeP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cols_nThreads
            // 
            this.cols_nThreads.Text = "#Threads";
            this.cols_nThreads.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cols_nMod
            // 
            this.cols_nMod.Text = "#Modules";
            this.cols_nMod.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cols_nMod.Width = 0;
            // 
            // procSumPopupMenu
            // 
            this.procSumPopupMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeSelectedItemsToolStripMenuItem,
            this.removeAllToolStripMenuItem,
            this.processInformationMenuBtn});
            this.procSumPopupMenu.Name = "sumPopupMenu";
            this.procSumPopupMenu.Size = new System.Drawing.Size(181, 70);
            // 
            // removeSelectedItemsToolStripMenuItem
            // 
            this.removeSelectedItemsToolStripMenuItem.Name = "removeSelectedItemsToolStripMenuItem";
            this.removeSelectedItemsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removeSelectedItemsToolStripMenuItem.Text = "Clear Selected Items";
            this.removeSelectedItemsToolStripMenuItem.Click += new System.EventHandler(this.removeSelectedItemsToolStripMenuItem_Click);
            // 
            // removeAllToolStripMenuItem
            // 
            this.removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
            this.removeAllToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removeAllToolStripMenuItem.Text = "Clear All";
            this.removeAllToolStripMenuItem.Click += new System.EventHandler(this.removeAllItemsToolStripMenuItem_Click);
            // 
            // processInformationMenuBtn
            // 
            this.processInformationMenuBtn.Name = "processInformationMenuBtn";
            this.processInformationMenuBtn.Size = new System.Drawing.Size(180, 22);
            this.processInformationMenuBtn.Text = "Process Information";
            this.processInformationMenuBtn.Click += new System.EventHandler(this.processInformationMenuBtn_Click);
            // 
            // sortImages
            // 
            this.sortImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("sortImages.ImageStream")));
            this.sortImages.TransparentColor = System.Drawing.Color.Transparent;
            this.sortImages.Images.SetKeyName(0, "upBW.png");
            this.sortImages.Images.SetKeyName(1, "downBW.png");
            this.sortImages.Images.SetKeyName(2, "up2BW.png");
            this.sortImages.Images.SetKeyName(3, "down2BW.png");
            // 
            // procView
            // 
            this.procView.AllowColumnReorder = true;
            this.procView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.procView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col0,
            this.col_log,
            this.col_id,
            this.col_name,
            this.col_IOread,
            this.col_IOwrite,
            this.col_IOother,
            this.col_ioChg,
            this.col_handles,
            this.col_start,
            this.col_totTime,
            this.col_userTime,
            this.col_privMem,
            this.col_work,
            this.col_wkPeak,
            this.col_virt,
            this.col_virtPeakSize,
            this.col_paged,
            this.col_pagedPeak,
            this.col_Threads,
            this.col_Modules});
            this.procView.ContextMenuStrip = this.procDetailPopupMenu;
            this.procView.FullRowSelect = true;
            this.procView.GridLines = true;
            this.procView.HideSelection = false;
            this.procView.Location = new System.Drawing.Point(0, 25);
            this.procView.Margin = new System.Windows.Forms.Padding(0);
            this.procView.Name = "procView";
            this.procView.Size = new System.Drawing.Size(1176, 290);
            this.procView.SmallImageList = this.sortImages;
            this.procView.TabIndex = 1;
            this.procView.UseCompatibleStateImageBehavior = false;
            this.procView.View = System.Windows.Forms.View.Details;
            this.procView.SelectedIndexChanged += new System.EventHandler(this.procView_SelectedIndexChanged);
            this.procView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ColumnClick);
            this.procView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.procView_MouseMove);
            this.procView.Click += new System.EventHandler(this.procView_Click);
            // 
            // col0
            // 
            this.col0.Text = "-";
            this.col0.Width = 0;
            // 
            // col_log
            // 
            this.col_log.Text = "Log";
            this.col_log.Width = 40;
            // 
            // col_id
            // 
            this.col_id.Text = "ProcId";
            this.col_id.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.col_id.Width = 47;
            // 
            // col_name
            // 
            this.col_name.Text = "Name";
            this.col_name.Width = 130;
            // 
            // col_IOread
            // 
            this.col_IOread.Text = "#Reads";
            this.col_IOread.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_IOwrite
            // 
            this.col_IOwrite.Text = "#Writes";
            this.col_IOwrite.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_IOother
            // 
            this.col_IOother.Text = "#Other I/O";
            this.col_IOother.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_ioChg
            // 
            this.col_ioChg.Text = "I/O Chg";
            this.col_ioChg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_handles
            // 
            this.col_handles.Text = "#Handles";
            this.col_handles.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_start
            // 
            this.col_start.Text = "Start Time";
            this.col_start.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_start.Width = 150;
            // 
            // col_totTime
            // 
            this.col_totTime.Text = "TotalTm";
            this.col_totTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_userTime
            // 
            this.col_userTime.Text = "UserTm";
            this.col_userTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_privMem
            // 
            this.col_privMem.Text = "PrivateMem";
            this.col_privMem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_work
            // 
            this.col_work.Text = "WorkSet";
            this.col_work.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_wkPeak
            // 
            this.col_wkPeak.Text = "WorkSet (Peak)";
            this.col_wkPeak.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_virt
            // 
            this.col_virt.Text = "Virtual Size";
            this.col_virt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_virtPeakSize
            // 
            this.col_virtPeakSize.Text = "Virtual (Peak) Size";
            this.col_virtPeakSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_paged
            // 
            this.col_paged.Text = "Paged Size";
            this.col_paged.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_pagedPeak
            // 
            this.col_pagedPeak.Text = "Paged (Peak) Size";
            this.col_pagedPeak.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_Threads
            // 
            this.col_Threads.Text = "#Threads";
            this.col_Threads.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_Modules
            // 
            this.col_Modules.Text = "#Modules";
            this.col_Modules.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.col_Modules.Width = 0;
            // 
            // procDetailPopupMenu
            // 
            this.procDetailPopupMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.procInfoDetailMenuBtn});
            this.procDetailPopupMenu.Name = "procDetailPopupMenu";
            this.procDetailPopupMenu.Size = new System.Drawing.Size(181, 26);
            // 
            // procInfoDetailMenuBtn
            // 
            this.procInfoDetailMenuBtn.Name = "procInfoDetailMenuBtn";
            this.procInfoDetailMenuBtn.Size = new System.Drawing.Size(180, 22);
            this.procInfoDetailMenuBtn.Text = "Process Information";
            this.procInfoDetailMenuBtn.Click += new System.EventHandler(this.processInformationMenuBtn_Click);
            // 
            // hndSumView
            // 
            this.hndSumView.AllowColumnReorder = true;
            this.hndSumView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hndSumView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_tm,
            this.col_pid,
            this.col_nm,
            this.col_tot});
            this.hndSumView.ContextMenuStrip = this.procSumPopupMenu;
            this.hndSumView.FullRowSelect = true;
            this.hndSumView.GridLines = true;
            this.hndSumView.Location = new System.Drawing.Point(0, 25);
            this.hndSumView.Name = "hndSumView";
            this.hndSumView.Size = new System.Drawing.Size(1176, 163);
            this.hndSumView.SmallImageList = this.sortImages;
            this.hndSumView.TabIndex = 5;
            this.hndSumView.UseCompatibleStateImageBehavior = false;
            this.hndSumView.View = System.Windows.Forms.View.Details;
            this.hndSumView.SelectedIndexChanged += new System.EventHandler(this.hndSumView_SelectedIndexChanged);
            this.hndSumView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ColumnClick);
            this.hndSumView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HndView_MouseMove);
            // 
            // col_tm
            // 
            this.col_tm.Text = "Time";
            this.col_tm.Width = 140;
            // 
            // col_pid
            // 
            this.col_pid.Text = "ProcId";
            this.col_pid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_nm
            // 
            this.col_nm.Text = "Name";
            this.col_nm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.col_nm.Width = 120;
            // 
            // col_tot
            // 
            this.col_tot.Text = "Total";
            this.col_tot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // resizePanel
            // 
            this.resizePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.resizePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(165)))), ((int)(((byte)(175)))));
            this.resizePanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("resizePanel.BackgroundImage")));
            this.resizePanel.Location = new System.Drawing.Point(1153, 191);
            this.resizePanel.Margin = new System.Windows.Forms.Padding(0);
            this.resizePanel.Name = "resizePanel";
            this.resizePanel.Size = new System.Drawing.Size(22, 22);
            this.resizePanel.TabIndex = 4;
            this.resizePanel.MouseLeave += new System.EventHandler(this.resize_MouseLeave);
            this.resizePanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.resize_MouseMove);
            this.resizePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.resize_MouseDown);
            this.resizePanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.resize_MouseUp);
            this.resizePanel.MouseEnter += new System.EventHandler(this.resize_MouseEnter);
            // 
            // handleStatus
            // 
            this.handleStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.handleStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(165)))), ((int)(((byte)(176)))));
            this.handleStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.handleStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.handleStatus.Location = new System.Drawing.Point(3, 194);
            this.handleStatus.Margin = new System.Windows.Forms.Padding(0);
            this.handleStatus.Name = "handleStatus";
            this.handleStatus.ReadOnly = true;
            this.handleStatus.Size = new System.Drawing.Size(1068, 15);
            this.handleStatus.TabIndex = 2;
            // 
            // closeBtn
            // 
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(165)))), ((int)(((byte)(175)))));
            this.closeBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeBtn.Location = new System.Drawing.Point(1076, 189);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(75, 23);
            this.closeBtn.TabIndex = 3;
            this.closeBtn.Text = "Close";
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // handleView
            // 
            this.handleView.AllowColumnReorder = true;
            this.handleView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.handleView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colH0,
            this.colH_pid,
            this.col_procName,
            this.colH_handle,
            this.colH_object,
            this.col_desc,
            this.col_age,
            this.col_stime,
            this.col_dup});
            this.handleView.ContextMenuStrip = this.hndPopupMenu;
            this.handleView.FullRowSelect = true;
            this.handleView.GridLines = true;
            this.handleView.Location = new System.Drawing.Point(0, 25);
            this.handleView.Margin = new System.Windows.Forms.Padding(0);
            this.handleView.Name = "handleView";
            this.handleView.Size = new System.Drawing.Size(1176, 165);
            this.handleView.SmallImageList = this.sortImages;
            this.handleView.TabIndex = 1;
            this.handleView.UseCompatibleStateImageBehavior = false;
            this.handleView.View = System.Windows.Forms.View.Details;
            this.handleView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ColumnClick);
            this.handleView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HndView_MouseMove);
            // 
            // colH0
            // 
            this.colH0.Text = "-";
            this.colH0.Width = 0;
            // 
            // colH_pid
            // 
            this.colH_pid.Text = "ProcId";
            this.colH_pid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_procName
            // 
            this.col_procName.Text = "ProcName";
            this.col_procName.Width = 111;
            // 
            // colH_handle
            // 
            this.colH_handle.Text = "Handle";
            this.colH_handle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // colH_object
            // 
            this.colH_object.Text = "Object";
            this.colH_object.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colH_object.Width = 73;
            // 
            // col_desc
            // 
            this.col_desc.Text = "Description";
            this.col_desc.Width = 724;
            // 
            // col_age
            // 
            this.col_age.Text = "Age";
            this.col_age.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // col_stime
            // 
            this.col_stime.Text = "Start Time";
            this.col_stime.Width = 150;
            // 
            // col_dup
            // 
            this.col_dup.Text = "Dup Cnt";
            this.col_dup.Width = 0;
            // 
            // hndPopupMenu
            // 
            this.hndPopupMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hndPopupAutoSizeBtn,
            this.hndPopupExplorerBtn,
            this.hndPopupCmdBtn,
            this.hndPopupRegEditBtn,
            this.fileMapToolStripMenuItem,
            this.cntDupFilesBtn,
            this.cleToolStripMenuItem});
            this.hndPopupMenu.Name = "hndPopupMenu";
            this.hndPopupMenu.Size = new System.Drawing.Size(172, 158);
            // 
            // hndPopupAutoSizeBtn
            // 
            this.hndPopupAutoSizeBtn.Name = "hndPopupAutoSizeBtn";
            this.hndPopupAutoSizeBtn.Size = new System.Drawing.Size(171, 22);
            this.hndPopupAutoSizeBtn.Text = "AutoSize Columns";
            this.hndPopupAutoSizeBtn.Click += new System.EventHandler(this.hndPopupBtn_Click);
            // 
            // hndPopupExplorerBtn
            // 
            this.hndPopupExplorerBtn.Name = "hndPopupExplorerBtn";
            this.hndPopupExplorerBtn.Size = new System.Drawing.Size(171, 22);
            this.hndPopupExplorerBtn.Text = "File - Explorer";
            this.hndPopupExplorerBtn.Click += new System.EventHandler(this.hndPopupBtn_Click);
            // 
            // hndPopupCmdBtn
            // 
            this.hndPopupCmdBtn.Name = "hndPopupCmdBtn";
            this.hndPopupCmdBtn.Size = new System.Drawing.Size(171, 22);
            this.hndPopupCmdBtn.Text = "File - Cmd";
            this.hndPopupCmdBtn.Click += new System.EventHandler(this.hndPopupBtn_Click);
            // 
            // hndPopupRegEditBtn
            // 
            this.hndPopupRegEditBtn.Name = "hndPopupRegEditBtn";
            this.hndPopupRegEditBtn.Size = new System.Drawing.Size(171, 22);
            this.hndPopupRegEditBtn.Text = "Key - RegEdit";
            this.hndPopupRegEditBtn.Click += new System.EventHandler(this.hndPopupBtn_Click);
            // 
            // fileMapToolStripMenuItem
            // 
            this.fileMapToolStripMenuItem.Name = "fileMapToolStripMenuItem";
            this.fileMapToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.fileMapToolStripMenuItem.Text = "File - Map";
            this.fileMapToolStripMenuItem.Click += new System.EventHandler(this.fileMapToolStripMenuItem_Click);
            // 
            // cntDupFilesBtn
            // 
            this.cntDupFilesBtn.Name = "cntDupFilesBtn";
            this.cntDupFilesBtn.Size = new System.Drawing.Size(171, 22);
            this.cntDupFilesBtn.Text = "Count Dups";
            this.cntDupFilesBtn.Click += new System.EventHandler(this.hndPopupBtn_Click);
            // 
            // cleToolStripMenuItem
            // 
            this.cleToolStripMenuItem.Name = "cleToolStripMenuItem";
            this.cleToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.cleToolStripMenuItem.Text = "Clear Dups";
            this.cleToolStripMenuItem.Click += new System.EventHandler(this.hndPopupBtn_Click);
            // 
            // handleToolStrip
            // 
            this.handleToolStrip.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("handleToolStrip.BackgroundImage")));
            this.handleToolStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.handleToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.handleToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton2,
            this.toolStripSeparator14,
            this.hndViewBtn,
            this.toolStripSeparator3,
            this.hndRefreshBtn,
            this.toolStripSeparator1,
            this.hndClock,
            this.toolStripSeparator4,
            this.handleLabel,
            this.toolStripSeparator11,
            this.hndExportMenu,
            this.toolStripSeparator12,
            this.hndLogMenu,
            this.toolStripSeparator13,
            this.hndPrintMenu,
            this.toolStripSeparator20,
            this.hndPreviewMenu});
            this.handleToolStrip.Location = new System.Drawing.Point(0, 0);
            this.handleToolStrip.Name = "handleToolStrip";
            this.handleToolStrip.Size = new System.Drawing.Size(1176, 25);
            this.handleToolStrip.TabIndex = 0;
            this.handleToolStrip.Text = "toolStrip3";
            this.handleToolStrip.MouseEnter += new System.EventHandler(this.handleToolStrip_MouseEnter);
            this.handleToolStrip.MouseLeave += new System.EventHandler(this.handleToolStrip_MouseLeave);
            // 
            // toolStripSplitButton2
            // 
            this.toolStripSplitButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hndAutoUpdBtn,
            this.hndRateBtn,
            this.descFilterToolStripMenuItem,
            this.toolStripSeparator18,
            this.hndAutoLogBtn,
            this.hndLogFileBtn,
            this.openLogFileToolStripMenuItem,
            this.hndExportBtn,
            this.toolStripSeparator19,
            this.hndPrSetBtn,
            this.hndPrPreviewBtn,
            this.hndPrBtn,
            this.hndFitToPageBtn});
            this.toolStripSplitButton2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripSplitButton2.ForeColor = System.Drawing.Color.White;
            this.toolStripSplitButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton2.Image")));
            this.toolStripSplitButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton2.Name = "toolStripSplitButton2";
            this.toolStripSplitButton2.Size = new System.Drawing.Size(44, 22);
            this.toolStripSplitButton2.Text = "File";
            this.toolStripSplitButton2.ToolTipText = "Update, log, export and print";
            // 
            // hndAutoUpdBtn
            // 
            this.hndAutoUpdBtn.CheckOnClick = true;
            this.hndAutoUpdBtn.Name = "hndAutoUpdBtn";
            this.hndAutoUpdBtn.Size = new System.Drawing.Size(163, 22);
            this.hndAutoUpdBtn.Text = "Auto Update";
            this.hndAutoUpdBtn.ToolTipText = "Toggle automatic update of handle display (see Update Rate for frequency).";
            this.hndAutoUpdBtn.Click += new System.EventHandler(this.hndFileMenu_Click);
            // 
            // hndRateBtn
            // 
            this.hndRateBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hnd1secBtn,
            this.hnd5secBtn,
            this.hnd1minBtn,
            this.hnd5minBtn,
            this.hnd1hourBtn,
            this.hndCustomBtn});
            this.hndRateBtn.Name = "hndRateBtn";
            this.hndRateBtn.Size = new System.Drawing.Size(163, 22);
            this.hndRateBtn.Text = "Update  Rate";
            this.hndRateBtn.ToolTipText = "Rate screen updates when Auto Update is enabled.";
            // 
            // hnd1secBtn
            // 
            this.hnd1secBtn.Name = "hnd1secBtn";
            this.hnd1secBtn.Size = new System.Drawing.Size(129, 22);
            this.hnd1secBtn.Tag = 1;
            this.hnd1secBtn.Text = "1 Second";
            this.hnd1secBtn.Click += new System.EventHandler(this.hndUpdRate_Click);
            // 
            // hnd5secBtn
            // 
            this.hnd5secBtn.Name = "hnd5secBtn";
            this.hnd5secBtn.Size = new System.Drawing.Size(129, 22);
            this.hnd5secBtn.Tag = 5;
            this.hnd5secBtn.Text = "5 Second";
            this.hnd5secBtn.Click += new System.EventHandler(this.hndUpdRate_Click);
            // 
            // hnd1minBtn
            // 
            this.hnd1minBtn.Name = "hnd1minBtn";
            this.hnd1minBtn.Size = new System.Drawing.Size(129, 22);
            this.hnd1minBtn.Tag = 60;
            this.hnd1minBtn.Text = "1 Minute";
            this.hnd1minBtn.Click += new System.EventHandler(this.hndUpdRate_Click);
            // 
            // hnd5minBtn
            // 
            this.hnd5minBtn.Name = "hnd5minBtn";
            this.hnd5minBtn.Size = new System.Drawing.Size(129, 22);
            this.hnd5minBtn.Tag = 500;
            this.hnd5minBtn.Text = "5 Minute";
            this.hnd5minBtn.Click += new System.EventHandler(this.hndUpdRate_Click);
            // 
            // hnd1hourBtn
            // 
            this.hnd1hourBtn.Name = "hnd1hourBtn";
            this.hnd1hourBtn.Size = new System.Drawing.Size(129, 22);
            this.hnd1hourBtn.Tag = 3600;
            this.hnd1hourBtn.Text = "Hourly";
            this.hnd1hourBtn.Click += new System.EventHandler(this.hndUpdRate_Click);
            // 
            // hndCustomBtn
            // 
            this.hndCustomBtn.Enabled = false;
            this.hndCustomBtn.Name = "hndCustomBtn";
            this.hndCustomBtn.Size = new System.Drawing.Size(129, 22);
            this.hndCustomBtn.Tag = -1;
            this.hndCustomBtn.Text = "Custom";
            this.hndCustomBtn.Click += new System.EventHandler(this.hndUpdRate_Click);
            // 
            // descFilterToolStripMenuItem
            // 
            this.descFilterToolStripMenuItem.Enabled = false;
            this.descFilterToolStripMenuItem.Name = "descFilterToolStripMenuItem";
            this.descFilterToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.descFilterToolStripMenuItem.Text = "Filter...";
            this.descFilterToolStripMenuItem.ToolTipText = "Reduce handle list by defining filters.";
            this.descFilterToolStripMenuItem.Visible = false;
            this.descFilterToolStripMenuItem.Click += new System.EventHandler(this.hndFileMenu_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(160, 6);
            // 
            // hndAutoLogBtn
            // 
            this.hndAutoLogBtn.CheckOnClick = true;
            this.hndAutoLogBtn.Name = "hndAutoLogBtn";
            this.hndAutoLogBtn.Size = new System.Drawing.Size(163, 22);
            this.hndAutoLogBtn.Text = "Auto Log";
            this.hndAutoLogBtn.ToolTipText = "Toggle whether screen updates are also exported to log file.";
            this.hndAutoLogBtn.Click += new System.EventHandler(this.hndFileMenu_Click);
            // 
            // hndLogFileBtn
            // 
            this.hndLogFileBtn.Name = "hndLogFileBtn";
            this.hndLogFileBtn.Size = new System.Drawing.Size(163, 22);
            this.hndLogFileBtn.Text = "Log File...";
            this.hndLogFileBtn.ToolTipText = "Define file to store handle list information.";
            this.hndLogFileBtn.Click += new System.EventHandler(this.hndFileMenu_Click);
            // 
            // openLogFileToolStripMenuItem
            // 
            this.openLogFileToolStripMenuItem.Name = "openLogFileToolStripMenuItem";
            this.openLogFileToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.openLogFileToolStripMenuItem.Text = "Open log file";
            this.openLogFileToolStripMenuItem.ToolTipText = "Open current handle log file with external viewer (ex: notepad or excel)";
            this.openLogFileToolStripMenuItem.Click += new System.EventHandler(this.hndFileMenu_Click);
            // 
            // hndExportBtn
            // 
            this.hndExportBtn.Name = "hndExportBtn";
            this.hndExportBtn.Size = new System.Drawing.Size(163, 22);
            this.hndExportBtn.Text = "Export...";
            this.hndExportBtn.ToolTipText = "Export handle information to file.";
            this.hndExportBtn.Click += new System.EventHandler(this.hndFileMenu_Click);
            // 
            // toolStripSeparator19
            // 
            this.toolStripSeparator19.Name = "toolStripSeparator19";
            this.toolStripSeparator19.Size = new System.Drawing.Size(160, 6);
            // 
            // hndPrSetBtn
            // 
            this.hndPrSetBtn.Name = "hndPrSetBtn";
            this.hndPrSetBtn.Size = new System.Drawing.Size(163, 22);
            this.hndPrSetBtn.Text = "Print Setup...";
            this.hndPrSetBtn.ToolTipText = "Select printer";
            this.hndPrSetBtn.Click += new System.EventHandler(this.hndFileMenu_Click);
            // 
            // hndPrPreviewBtn
            // 
            this.hndPrPreviewBtn.Name = "hndPrPreviewBtn";
            this.hndPrPreviewBtn.Size = new System.Drawing.Size(163, 22);
            this.hndPrPreviewBtn.Text = "Print Preview...";
            this.hndPrPreviewBtn.ToolTipText = "Print to preview";
            this.hndPrPreviewBtn.Click += new System.EventHandler(this.hndFileMenu_Click);
            // 
            // hndPrBtn
            // 
            this.hndPrBtn.Name = "hndPrBtn";
            this.hndPrBtn.Size = new System.Drawing.Size(163, 22);
            this.hndPrBtn.Text = "Print...";
            this.hndPrBtn.Click += new System.EventHandler(this.hndFileMenu_Click);
            // 
            // hndFitToPageBtn
            // 
            this.hndFitToPageBtn.Checked = true;
            this.hndFitToPageBtn.CheckOnClick = true;
            this.hndFitToPageBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hndFitToPageBtn.Name = "hndFitToPageBtn";
            this.hndFitToPageBtn.Size = new System.Drawing.Size(163, 22);
            this.hndFitToPageBtn.Text = "Fit To Page";
            this.hndFitToPageBtn.ToolTipText = "Auto shrink up by 20% to fit on printer page";
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(6, 25);
            // 
            // hndViewBtn
            // 
            this.hndViewBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.hndViewBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hndHideNoDescBtn,
            this.filterToolStripMenuItem,
            this.groupByToolStripMenuItem,
            this.hndSummaryBtn,
            this.hndClearSumBtn,
            this.hndShowClosedCk});
            this.hndViewBtn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hndViewBtn.ForeColor = System.Drawing.Color.White;
            this.hndViewBtn.Image = ((System.Drawing.Image)(resources.GetObject("hndViewBtn.Image")));
            this.hndViewBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.hndViewBtn.Name = "hndViewBtn";
            this.hndViewBtn.Size = new System.Drawing.Size(64, 22);
            this.hndViewBtn.Text = "View...";
            // 
            // hndHideNoDescBtn
            // 
            this.hndHideNoDescBtn.CheckOnClick = true;
            this.hndHideNoDescBtn.Name = "hndHideNoDescBtn";
            this.hndHideNoDescBtn.Size = new System.Drawing.Size(238, 22);
            this.hndHideNoDescBtn.Text = "Hide if no description";
            this.hndHideNoDescBtn.Click += new System.EventHandler(this.hndHideNoDescBtn_Click);
            // 
            // filterToolStripMenuItem
            // 
            this.filterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hndShowAllBtn});
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            this.filterToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.filterToolStripMenuItem.Text = "Limit objects to";
            // 
            // hndShowAllBtn
            // 
            this.hndShowAllBtn.Checked = true;
            this.hndShowAllBtn.CheckOnClick = true;
            this.hndShowAllBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hndShowAllBtn.Name = "hndShowAllBtn";
            this.hndShowAllBtn.Size = new System.Drawing.Size(90, 22);
            this.hndShowAllBtn.Tag = 0;
            this.hndShowAllBtn.Text = "All";
            this.hndShowAllBtn.Click += new System.EventHandler(this.hndShowAllBtn_Click);
            // 
            // groupByToolStripMenuItem
            // 
            this.groupByToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hndNoGrpBtn,
            this.hndPidGrpBtn,
            this.hndObjGrpBtn});
            this.groupByToolStripMenuItem.Name = "groupByToolStripMenuItem";
            this.groupByToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.groupByToolStripMenuItem.Text = "Group By";
            // 
            // hndNoGrpBtn
            // 
            this.hndNoGrpBtn.Checked = true;
            this.hndNoGrpBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hndNoGrpBtn.Name = "hndNoGrpBtn";
            this.hndNoGrpBtn.Size = new System.Drawing.Size(151, 22);
            this.hndNoGrpBtn.Text = "No Group";
            this.hndNoGrpBtn.Click += new System.EventHandler(this.hndGrpBtn_Click);
            // 
            // hndPidGrpBtn
            // 
            this.hndPidGrpBtn.Name = "hndPidGrpBtn";
            this.hndPidGrpBtn.Size = new System.Drawing.Size(151, 22);
            this.hndPidGrpBtn.Text = "Pid Group";
            this.hndPidGrpBtn.Click += new System.EventHandler(this.hndGrpBtn_Click);
            // 
            // hndObjGrpBtn
            // 
            this.hndObjGrpBtn.Name = "hndObjGrpBtn";
            this.hndObjGrpBtn.Size = new System.Drawing.Size(151, 22);
            this.hndObjGrpBtn.Text = "Object Group";
            this.hndObjGrpBtn.Click += new System.EventHandler(this.hndGrpBtn_Click);
            // 
            // hndSummaryBtn
            // 
            this.hndSummaryBtn.CheckOnClick = true;
            this.hndSummaryBtn.Name = "hndSummaryBtn";
            this.hndSummaryBtn.Size = new System.Drawing.Size(238, 22);
            this.hndSummaryBtn.Text = "Summary";
            this.hndSummaryBtn.ToolTipText = "Display handle summary and history";
            this.hndSummaryBtn.Click += new System.EventHandler(this.hndSummaryBtn_Click);
            // 
            // hndClearSumBtn
            // 
            this.hndClearSumBtn.Name = "hndClearSumBtn";
            this.hndClearSumBtn.Size = new System.Drawing.Size(238, 22);
            this.hndClearSumBtn.Text = "Clear Summary";
            this.hndClearSumBtn.Click += new System.EventHandler(this.hndClearSumBtn_Click);
            // 
            // hndShowClosedCk
            // 
            this.hndShowClosedCk.Checked = true;
            this.hndShowClosedCk.CheckOnClick = true;
            this.hndShowClosedCk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hndShowClosedCk.Name = "hndShowClosedCk";
            this.hndShowClosedCk.Size = new System.Drawing.Size(238, 22);
            this.hndShowClosedCk.Text = "Briefly Show Closed Handles";
            this.hndShowClosedCk.Click += new System.EventHandler(this.hndShowClosedCk_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // hndRefreshBtn
            // 
            this.hndRefreshBtn.AutoSize = false;
            this.hndRefreshBtn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hndRefreshBtn.ForeColor = System.Drawing.Color.White;
            this.hndRefreshBtn.Image = global::MagniFile.Properties.Resources.red24;
            this.hndRefreshBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.hndRefreshBtn.Name = "hndRefreshBtn";
            this.hndRefreshBtn.Size = new System.Drawing.Size(75, 22);
            this.hndRefreshBtn.Text = "Refresh";
            this.hndRefreshBtn.ToolTipText = "Refresh Open Handles List";
            this.hndRefreshBtn.Click += new System.EventHandler(this.hndRefreshBtn_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // hndClock
            // 
            this.hndClock.AutoSize = false;
            this.hndClock.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hndClock.ForeColor = System.Drawing.Color.White;
            this.hndClock.Name = "hndClock";
            this.hndClock.Size = new System.Drawing.Size(70, 22);
            this.hndClock.Text = "88:88:88";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // handleLabel
            // 
            this.handleLabel.AutoSize = false;
            this.handleLabel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.handleLabel.ForeColor = System.Drawing.Color.White;
            this.handleLabel.Name = "handleLabel";
            this.handleLabel.Size = new System.Drawing.Size(500, 22);
            this.handleLabel.Text = "Open Handles";
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
            // 
            // hndExportMenu
            // 
            this.hndExportMenu.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hndExportMenu.ForeColor = System.Drawing.Color.White;
            this.hndExportMenu.Image = global::MagniFile.Properties.Resources.expoort;
            this.hndExportMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.hndExportMenu.Name = "hndExportMenu";
            this.hndExportMenu.Size = new System.Drawing.Size(76, 22);
            this.hndExportMenu.Text = "Export...";
            this.hndExportMenu.ToolTipText = "Export handle list to disk file as csv";
            this.hndExportMenu.Click += new System.EventHandler(this.hndFileMenu_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(6, 25);
            // 
            // hndLogMenu
            // 
            this.hndLogMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.hndLogMenu.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hndLogMenu.ForeColor = System.Drawing.Color.White;
            this.hndLogMenu.Image = ((System.Drawing.Image)(resources.GetObject("hndLogMenu.Image")));
            this.hndLogMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.hndLogMenu.Name = "hndLogMenu";
            this.hndLogMenu.Size = new System.Drawing.Size(32, 22);
            this.hndLogMenu.Text = "Log";
            this.hndLogMenu.ToolTipText = "Export process lines marked with  \'log\' tag  to log file.\r\nUse File  button to co" +
                "nfigure automatic logging.";
            this.hndLogMenu.Click += new System.EventHandler(this.hndFileMenu_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(6, 25);
            // 
            // hndPrintMenu
            // 
            this.hndPrintMenu.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hndPrintMenu.ForeColor = System.Drawing.Color.White;
            this.hndPrintMenu.Image = global::MagniFile.Properties.Resources.print;
            this.hndPrintMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.hndPrintMenu.Name = "hndPrintMenu";
            this.hndPrintMenu.Size = new System.Drawing.Size(54, 22);
            this.hndPrintMenu.Text = "Print";
            this.hndPrintMenu.ToolTipText = "Print Handle List";
            this.hndPrintMenu.Click += new System.EventHandler(this.hndFileMenu_Click);
            // 
            // toolStripSeparator20
            // 
            this.toolStripSeparator20.Name = "toolStripSeparator20";
            this.toolStripSeparator20.Size = new System.Drawing.Size(6, 25);
            // 
            // hndPreviewMenu
            // 
            this.hndPreviewMenu.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hndPreviewMenu.ForeColor = System.Drawing.Color.White;
            this.hndPreviewMenu.Image = global::MagniFile.Properties.Resources.printPreview;
            this.hndPreviewMenu.Name = "hndPreviewMenu";
            this.hndPreviewMenu.Size = new System.Drawing.Size(69, 22);
            this.hndPreviewMenu.Text = "Preview";
            this.hndPreviewMenu.ToolTipText = "Preview printing handle list";
            this.hndPreviewMenu.Click += new System.EventHandler(this.hndFileMenu_Click);
            // 
            // procTimer
            // 
            this.procTimer.Interval = 1000;
            this.procTimer.Tick += new System.EventHandler(this.procTimer_Tick);
            // 
            // exportFileDialog
            // 
            this.exportFileDialog.Filter = "csv|*.csv|txt|*.txt|Any|*.*";
            this.exportFileDialog.Title = "Export MagniFile List";
            // 
            // logFileDialog
            // 
            this.logFileDialog.Filter = "csv|*.csv|text|*.txt|Any|*.*";
            this.logFileDialog.Title = "Log MagniFile ";
            // 
            // handleTimer
            // 
            this.handleTimer.Interval = 1000;
            this.handleTimer.Tick += new System.EventHandler(this.handleTimer_Tick);
            // 
            // logOpenDialog
            // 
            this.logOpenDialog.Filter = "csv|*.csv|text|*.txt|Any|*.*";
            this.logOpenDialog.Title = "Log MagniFile ";
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.BackColor = System.Drawing.Color.Transparent;
            this.mainToolStrip.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mainToolStrip.BackgroundImage")));
            this.mainToolStrip.CanOverflow = false;
            this.mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.diskDetailBtn,
            this.fsInfoBtn,
            this.graphBtn,
            this.graphPlusBtn,
            this.diskClusterBtn,
            this.titleLeft,
            this.title,
            this.netInfoBtn,
            this.fileListBtn,
            this.monFileBtn,
            this.titleRight});
            this.mainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Size = new System.Drawing.Size(1180, 71);
            this.mainToolStrip.Stretch = true;
            this.mainToolStrip.TabIndex = 0;
            this.mainToolStrip.Text = "toolStrip1";
            // 
            // diskDetailBtn
            // 
            this.diskDetailBtn.AutoSize = false;
            this.diskDetailBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.diskDetailBtn.Image = ((System.Drawing.Image)(resources.GetObject("diskDetailBtn.Image")));
            this.diskDetailBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.diskDetailBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.diskDetailBtn.Name = "diskDetailBtn";
            this.diskDetailBtn.Size = new System.Drawing.Size(64, 64);
            this.diskDetailBtn.ToolTipText = "About Author and Program information.";
            this.diskDetailBtn.Visible = false;
            this.diskDetailBtn.Click += new System.EventHandler(this.About_Click);
            // 
            // fsInfoBtn
            // 
            this.fsInfoBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.fsInfoBtn.Image = ((System.Drawing.Image)(resources.GetObject("fsInfoBtn.Image")));
            this.fsInfoBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.fsInfoBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fsInfoBtn.Name = "fsInfoBtn";
            this.fsInfoBtn.Size = new System.Drawing.Size(68, 68);
            this.fsInfoBtn.ToolTipText = "Display File System Details";
            this.fsInfoBtn.Click += new System.EventHandler(this.FileSysInfo_Click);
            // 
            // graphBtn
            // 
            this.graphBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.graphBtn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.graphBtn.ForeColor = System.Drawing.Color.White;
            this.graphBtn.Image = ((System.Drawing.Image)(resources.GetObject("graphBtn.Image")));
            this.graphBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.graphBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.graphBtn.Name = "graphBtn";
            this.graphBtn.Size = new System.Drawing.Size(68, 68);
            this.graphBtn.Text = "graph";
            this.graphBtn.ToolTipText = "Graph Process  Summary Log";
            this.graphBtn.Click += new System.EventHandler(this.graphBtn_Click);
            // 
            // graphPlusBtn
            // 
            this.graphPlusBtn.AutoSize = false;
            this.graphPlusBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("graphPlusBtn.BackgroundImage")));
            this.graphPlusBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.graphPlusBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.graphPlusBtn.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.graphPlusBtn.ForeColor = System.Drawing.Color.Transparent;
            this.graphPlusBtn.Name = "graphPlusBtn";
            this.graphPlusBtn.Size = new System.Drawing.Size(20, 68);
            this.graphPlusBtn.Text = "+";
            this.graphPlusBtn.ToolTipText = "Additional Graph of Process  Summary Log";
            this.graphPlusBtn.Click += new System.EventHandler(this.graphPlusBtn_Click);
            // 
            // diskClusterBtn
            // 
            this.diskClusterBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.diskClusterBtn.Image = ((System.Drawing.Image)(resources.GetObject("diskClusterBtn.Image")));
            this.diskClusterBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.diskClusterBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.diskClusterBtn.Name = "diskClusterBtn";
            this.diskClusterBtn.Size = new System.Drawing.Size(68, 68);
            this.diskClusterBtn.Text = "Disk Cluster";
            this.diskClusterBtn.ToolTipText = "Display Disk Cluster Allocation Map";
            this.diskClusterBtn.Click += new System.EventHandler(this.diskClusterBtn_Click);
            // 
            // titleLeft
            // 
            this.titleLeft.AutoSize = false;
            this.titleLeft.BackColor = System.Drawing.Color.Transparent;
            this.titleLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.titleLeft.ForeColor = System.Drawing.Color.Transparent;
            this.titleLeft.Name = "titleLeft";
            this.titleLeft.Size = new System.Drawing.Size(100, 68);
            this.titleLeft.Text = ".";
            // 
            // title
            // 
            this.title.AutoSize = false;
            this.title.BackColor = System.Drawing.Color.Transparent;
            this.title.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("title.BackgroundImage")));
            this.title.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.title.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.title.Font = new System.Drawing.Font("Footlight MT Light", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.ForeColor = System.Drawing.Color.Red;
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(480, 64);
            this.title.Text = "MagniFile v7.1 - DLang 2012";
            this.title.MouseEnter += new System.EventHandler(this.title_MouseEnter);
            this.title.MouseLeave += new System.EventHandler(this.title_MouseLeave);
            this.title.Click += new System.EventHandler(this.About_Click);
            // 
            // netInfoBtn
            // 
            this.netInfoBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.netInfoBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.netInfoBtn.ForeColor = System.Drawing.Color.White;
            this.netInfoBtn.Image = ((System.Drawing.Image)(resources.GetObject("netInfoBtn.Image")));
            this.netInfoBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.netInfoBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.netInfoBtn.Name = "netInfoBtn";
            this.netInfoBtn.Size = new System.Drawing.Size(68, 68);
            this.netInfoBtn.Text = "Network";
            this.netInfoBtn.ToolTipText = "Network Information";
            this.netInfoBtn.Click += new System.EventHandler(this.netInfo_Click);
            // 
            // fileListBtn
            // 
            this.fileListBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.fileListBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.fileListBtn.Image = ((System.Drawing.Image)(resources.GetObject("fileListBtn.Image")));
            this.fileListBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.fileListBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fileListBtn.Name = "fileListBtn";
            this.fileListBtn.Size = new System.Drawing.Size(68, 68);
            this.fileListBtn.Text = "Open File and Process  Module List";
            this.fileListBtn.Click += new System.EventHandler(this.fileListBtn_Click);
            // 
            // monFileBtn
            // 
            this.monFileBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.monFileBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.monFileBtn.ForeColor = System.Drawing.Color.White;
            this.monFileBtn.Image = ((System.Drawing.Image)(resources.GetObject("monFileBtn.Image")));
            this.monFileBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.monFileBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.monFileBtn.Name = "monFileBtn";
            this.monFileBtn.Size = new System.Drawing.Size(68, 68);
            this.monFileBtn.Text = "Monitor Files";
            this.monFileBtn.Click += new System.EventHandler(this.monFileBtn_Click);
            // 
            // titleRight
            // 
            this.titleRight.AutoSize = false;
            this.titleRight.BackColor = System.Drawing.Color.Transparent;
            this.titleRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.titleRight.ForeColor = System.Drawing.Color.Transparent;
            this.titleRight.Name = "titleRight";
            this.titleRight.Size = new System.Drawing.Size(40, 68);
            this.titleRight.Text = ".";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1180, 629);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mainToolStrip);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "MagniFile - DLang - v1.0";
            this.toolTip.SetToolTip(this, "Display Open Handles");
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            this.procToolStrip.ResumeLayout(false);
            this.procToolStrip.PerformLayout();
            this.procSumPopupMenu.ResumeLayout(false);
            this.procDetailPopupMenu.ResumeLayout(false);
            this.hndPopupMenu.ResumeLayout(false);
            this.handleToolStrip.ResumeLayout(false);
            this.handleToolStrip.PerformLayout();
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView procView;
        private System.Windows.Forms.ColumnHeader col0;
        private System.Windows.Forms.ColumnHeader col_log;
        private System.Windows.Forms.ColumnHeader col_id;
        private System.Windows.Forms.ColumnHeader col_name;
        private System.Windows.Forms.ColumnHeader col_start;
        private System.Windows.Forms.ColumnHeader col_totTime;
        private System.Windows.Forms.ColumnHeader col_userTime;
        private System.Windows.Forms.ToolStrip procToolStrip;
        private System.Windows.Forms.TextBox handleStatus;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.ColumnHeader col_privMem;
        private System.Windows.Forms.ColumnHeader col_work;
        private System.Windows.Forms.ColumnHeader col_wkPeak;
        private System.Windows.Forms.ColumnHeader col_virt;
        private System.Windows.Forms.ColumnHeader col_virtPeakSize;
        private System.Windows.Forms.ColumnHeader col_paged;
        private System.Windows.Forms.ColumnHeader col_pagedPeak;
        private System.Windows.Forms.ColumnHeader col_handles;
        private System.Windows.Forms.ColumnHeader col_Threads;
        private System.Windows.Forms.ColumnHeader col_Modules;
        private System.Windows.Forms.ToolStripButton procRefreshBtn;
        private System.Windows.Forms.Timer procTimer;
        private System.Windows.Forms.ToolStripLabel procClock;
        private System.Windows.Forms.ToolStripLabel procLabel;
        private System.Windows.Forms.ListView handleView;
        private System.Windows.Forms.ColumnHeader colH0;
        private System.Windows.Forms.ColumnHeader colH_pid;
        private System.Windows.Forms.ColumnHeader colH_handle;
        private System.Windows.Forms.ColumnHeader colH_object;
        private System.Windows.Forms.ToolStrip handleToolStrip;
        private System.Windows.Forms.ToolStripLabel handleLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton hndRefreshBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel hndClock;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ColumnHeader col_procName;
        private System.Windows.Forms.ColumnHeader col_desc;
        private System.Windows.Forms.ToolStripButton diskDetailBtn;
        private System.Windows.Forms.ToolStripLabel title;
        private System.Windows.Forms.TextBox procStatus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton procExportMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton procPreviewMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton procPrintMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripButton hndExportMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripLabel hndPreviewMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripButton hndPrintMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ColumnHeader col_ioChg;
        private System.Windows.Forms.ImageList sortImages;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripMenuItem procLogFileBtn;
        private System.Windows.Forms.ToolStripMenuItem procAutoLogBtn;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem updateRateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem procAutoUpdBtn;
        private System.Windows.Forms.ToolStripMenuItem prooc1secBtn;
        private System.Windows.Forms.ToolStripMenuItem proc5secBtn;
        private System.Windows.Forms.ToolStripMenuItem proc1minBtn;
        private System.Windows.Forms.ToolStripMenuItem proc5minBtn;
        private System.Windows.Forms.ToolStripMenuItem proc1hourBtn;
        private System.Windows.Forms.ToolStripMenuItem procCustomBtn;
        private System.Windows.Forms.ToolStripMenuItem procExpBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripMenuItem printSetupProcDetailMenu;
        private System.Windows.Forms.ToolStripMenuItem printPreviewProcDetailMenu;
        private System.Windows.Forms.ToolStripMenuItem printProcDetailMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton2;
        private System.Windows.Forms.ToolStripMenuItem hndAutoUpdBtn;
        private System.Windows.Forms.ToolStripMenuItem hndRateBtn;
        private System.Windows.Forms.ToolStripMenuItem hnd1secBtn;
        private System.Windows.Forms.ToolStripMenuItem hnd5secBtn;
        private System.Windows.Forms.ToolStripMenuItem hnd1minBtn;
        private System.Windows.Forms.ToolStripMenuItem hnd5minBtn;
        private System.Windows.Forms.ToolStripMenuItem hnd1hourBtn;
        private System.Windows.Forms.ToolStripMenuItem hndCustomBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem hndAutoLogBtn;
        private System.Windows.Forms.ToolStripMenuItem hndLogFileBtn;
        private System.Windows.Forms.ToolStripMenuItem hndExportBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator19;
        private System.Windows.Forms.ToolStripMenuItem hndPrSetBtn;
        private System.Windows.Forms.ToolStripMenuItem hndPrPreviewBtn;
        private System.Windows.Forms.ToolStripMenuItem hndPrBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator20;
        private System.Windows.Forms.SaveFileDialog exportFileDialog;
        private System.Windows.Forms.SaveFileDialog logFileDialog;
        private System.Windows.Forms.Timer handleTimer;
        private System.Windows.Forms.ToolStripSplitButton hndViewBtn;
        private System.Windows.Forms.ToolStripMenuItem hndNoGrpBtn;
        private System.Windows.Forms.ToolStripMenuItem hndPidGrpBtn;
        private System.Windows.Forms.ToolStripMenuItem hndObjGrpBtn;
        private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hndShowAllBtn;
        private System.Windows.Forms.ToolStripButton fsInfoBtn;
        private System.Windows.Forms.ContextMenuStrip hndPopupMenu;
        private System.Windows.Forms.ToolStripMenuItem hndPopupAutoSizeBtn;
        private System.Windows.Forms.ToolStripMenuItem hndPopupExplorerBtn;
        private System.Windows.Forms.ToolStripMenuItem hndPopupCmdBtn;
        private System.Windows.Forms.ToolStripMenuItem hndPopupRegEditBtn;
        private System.Windows.Forms.ToolStripButton diskClusterBtn;
        private System.Windows.Forms.ToolStripMenuItem hndHideNoDescBtn;
        private System.Windows.Forms.Panel resizePanel;
        private System.Windows.Forms.ToolStripMenuItem fileMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem groupByToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton procLogMenu;
        private System.Windows.Forms.ToolStripButton hndLogMenu;
        private System.Windows.Forms.ToolStripMenuItem procLogOpenBtn;
        private System.Windows.Forms.ToolStripMenuItem openLogFileToolStripMenuItem;
        private System.Windows.Forms.ListView hndSumView;
        private System.Windows.Forms.ColumnHeader col_tm;
        private System.Windows.Forms.ColumnHeader col_pid;
        private System.Windows.Forms.ColumnHeader col_nm;
        private System.Windows.Forms.ColumnHeader col_tot;
        private System.Windows.Forms.ToolStripMenuItem hndSummaryBtn;
        private System.Windows.Forms.ToolStripMenuItem hndClearSumBtn;
        private System.Windows.Forms.ListView procSumView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader cols_pid;
        private System.Windows.Forms.ColumnHeader cols_name;
        private System.Windows.Forms.ColumnHeader cols_start;
        private System.Windows.Forms.ColumnHeader cols_totTime;
        private System.Windows.Forms.ColumnHeader cols_userTime;
        private System.Windows.Forms.ColumnHeader cols_privMem;
        private System.Windows.Forms.ColumnHeader cols_wkSet;
        private System.Windows.Forms.ColumnHeader cols_wkSetP;
        private System.Windows.Forms.ColumnHeader cols_virt;
        private System.Windows.Forms.ColumnHeader cols_virtP;
        private System.Windows.Forms.ColumnHeader cols_pageSize;
        private System.Windows.Forms.ColumnHeader cols_pgSizeP;
        private System.Windows.Forms.ColumnHeader cols_nHnd;
        private System.Windows.Forms.ColumnHeader cols_nThreads;
        private System.Windows.Forms.ColumnHeader cols_nMod;
        private System.Windows.Forms.ToolStripButton procViewBtn;
        private System.Windows.Forms.ToolStripMenuItem removeSelectedItemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeAllToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader cols_ioRead;
        private System.Windows.Forms.ColumnHeader cols_ioWrite;
        private System.Windows.Forms.ColumnHeader cols_ioOther;
        private System.Windows.Forms.ColumnHeader col_IOread;
        private System.Windows.Forms.ColumnHeader col_IOwrite;
        private System.Windows.Forms.ColumnHeader col_IOother;
        private System.Windows.Forms.ToolStripButton graphBtn;
        private System.Windows.Forms.ToolStripButton netInfoBtn;
        private System.Windows.Forms.ToolStripButton fileListBtn;
        private System.Windows.Forms.ToolStripButton monFileBtn;
        private System.Windows.Forms.ColumnHeader cols_iooChg;
        private System.Windows.Forms.ColumnHeader col_age;
        private System.Windows.Forms.ColumnHeader col_stime;
        private System.Windows.Forms.ColumnHeader col_dup;
        private System.Windows.Forms.ToolStripMenuItem cntDupFilesBtn;
        private System.Windows.Forms.ToolStripMenuItem cleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog logOpenDialog;
        private System.Windows.Forms.ToolStripLabel titleRight;
        private System.Windows.Forms.ToolStripLabel titleLeft;
        private System.Windows.Forms.ToolStripMenuItem processInformationMenuBtn;
        private System.Windows.Forms.ContextMenuStrip procDetailPopupMenu;
        private System.Windows.Forms.ToolStripMenuItem procInfoDetailMenuBtn;
        public System.Windows.Forms.ContextMenuStrip procSumPopupMenu;
        private System.Windows.Forms.ToolStripMenuItem fitToPageProcDetailMenu;
        private System.Windows.Forms.ToolStripMenuItem hndFitToPageBtn;
        private System.Windows.Forms.ToolStripMenuItem hndShowClosedCk;
        private System.Windows.Forms.ToolStripMenuItem procFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem descFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripLabel graphPlusBtn;
    }
}

