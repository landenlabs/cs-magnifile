using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;

using System.Reflection;        // Assembly

namespace MagniFile
{
    /// <summary>
    /// Display disk (and file) cluster allocation map.
    /// 
    /// Author: Dennis Lang 2009
    /// https://landenlabs.com/
    /// 
    /// TODO - Add support for non-NTFS drives
    /// 
    /// </summary>
    public partial class ViewDiskAllocation : Form
    {
        System.Media.SoundPlayer sPlayer = new System.Media.SoundPlayer();
        Image logo;
        Color fileColor = Color.LightCoral;

        /// <summary>
        /// View disk cluster allocation (utilization) as an image.
        /// </summary>
        public ViewDiskAllocation(Image imageLogo)
        {
            InitializeComponent();

            logo = imageLogo;

            // Prevent viewer from being larger than working area.
            if (this.Height > Screen.GetWorkingArea(this).Height * 0.9)
                this.Height = (int)(Screen.GetWorkingArea(this).Height * 0.9);

            InitializeUser();
        }

        /// <summary>
        /// View disk allocation (utilization) with file allocation in overlay.
        /// </summary>
        public ViewDiskAllocation(string filepath)
        {
            InitializeComponent();
            InitializeUser();

            ViewFileAllocation(filepath);
        }

        private void InitializeUser()
        {
            this.Text = "Disk Allocation - " + MainForm.ProductNameAndVersion();

            DriveInfo[] diList = DriveInfo.GetDrives();
            foreach (DriveInfo di in diList)
            {
                // if (di.DriveType == DriveType.Fixed || di.DriveType == DriveType.Removable) 
                if (di.IsReady)
                {
                    this.driveCombo.Items.Add(di.Name.Split(Path.DirectorySeparatorChar)[0]);
                }
            }

            this.driveCombo.SelectedItem = this.driveCombo.Items[0];
            MakeLegendImage();
        }

        public void ViewFileAllocation(string filepath)
        {
            string[] filepaths = new string[] { filepath };
            ViewFileAllocation(filepaths);
        }

        public void ViewFileAllocation(string[] filepaths)
        {
            try
            {
                Directory.SetCurrentDirectory(this.driveCombo.SelectedItem.ToString());
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(filepaths[0]);
                string device = fileInfo.DirectoryName.Split(Path.DirectorySeparatorChar)[0];
                if (device.Length != 0)
                {
                    int comboIdx = this.driveCombo.FindString(device);
                    if (comboIdx != -1)
                        this.driveCombo.SelectedItem = this.driveCombo.Items[comboIdx];
                    else
                    {
                        MessageBox.Show("Failed to locate device for:" + filepaths[0]);
                        return;
                    }
                }

                DriveInfo driveInfo = new DriveInfo(device);
                if (driveInfo.IsReady == true && driveInfo.DriveType == DriveType.Fixed)
                {
                    MakeGrid();
                    RefreshDriveAllocationImage(device);
                    HighLightFileAllocation(filepaths, true);
                }
            }
            catch (Exception ee)
            {
                status.Text += " " + ee.Message;
            }
        }

        /// <summary>
        /// Make grid squares in overlay to divide area by quarters.
        /// </summary>
        private void MakeGrid()
        {
            Size size = diskPicture.DisplayRectangle.Size;

            int w = size.Width;
            int h = size.Height;
            Bitmap topImage = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(topImage);
            for (int y = 0; y < h; y += h / 4)
            {
                g.DrawLine(Pens.Black, new Point(0, y), new Point(w, y));
                g.DrawLine(Pens.White, new Point(0, y + 1), new Point(w, y + 1));
            }
            for (int x = 0; x < w; x += w / 4)
            {
                g.DrawLine(Pens.Black, new Point(x, 0), new Point(x, h));
                g.DrawLine(Pens.White, new Point(x + 1, 0), new Point(x + 1, h));
            }
            g.Flush();

            this.diskPicture.Image = topImage;
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            string device = this.driveCombo.SelectedItem.ToString();
            RefreshDriveAllocationImage(device);
        }

        // Keep copy of some stuff so resize/refresh can regenerate material.
        List<string> m_filepaths = new List<string>();

        Point firstP = Point.Empty;
        Point lastP = Point.Empty;
        int dia = 0;
        const int diaStart = 20;


        /// <summary>
        /// Draw file allocation in overlay image as small circles.
        /// </summary>
        private void HighLightFileAllocation(string[] filepaths, bool updList)
        {
            bool reset = true;
            int cnt = filepaths.Length;
            for (int idx = 0; idx < cnt; idx++)
            {
                HighLightFileAllocation(filepaths[idx], reset, updList, cnt == 1);
                reset = false;
            }
        }

        private void HighLightFileAllocation(string filepath, bool reset, bool updList, bool animate)
        {
            try
            {
                Array vcn_lcn = FileSysInfo.GetFileMap(filepath);

                int len0 = vcn_lcn.GetLength(0);
                int len1 = vcn_lcn.GetLength(1);

                Graphics g = Graphics.FromImage(diskPicture.Image);
                int bgW = diskPicture.BackgroundImage.Width;
                int bgH = diskPicture.BackgroundImage.Height;
                int dspW = diskPicture.Image.Width;
                int dspH = diskPicture.Image.Height;
                double xScale = (double)dspW / bgW;
                double yScale = (double)dspH / bgH;

                Int64 prevVcn = 0;
                firstP = Point.Empty;
                lastP = Point.Empty;

                if (reset)
                {
                    if (updList)
                        infoList.Items.Clear();
                    m_filepaths.Clear();
                    this.status.Text = string.Empty;
                    this.filepathBox.Text = string.Empty; 
                }

                System.IO.FileInfo fileInfo = new System.IO.FileInfo(filepath);
                ListViewItem item;

                if (updList)
                {
                    item = infoList.Items.Add("");
                    item.Tag = fileInfo.FullName;

                    item.BackColor = this.fileColor;
                    item.SubItems.Add("Name:");
                    item.SubItems.Add(fileInfo.Name);

                    item = infoList.Items.Add("");
                    item.SubItems.Add("Size:");
                    item.SubItems.Add(fileInfo.Length.ToString("#,##0"));

                    item = infoList.Items.Add("");
                    item.SubItems.Add("#Allocations:");
                    item.SubItems.Add(len0.ToString("#,##0"));

                    item = infoList.Items.Add("");
                    item = infoList.Items.Add("");
                    item.SubItems.Add("Length");
                    item.SubItems.Add("Cluster#");

                    item = userFileView.Items.Add("");
                    item.SubItems.Add("");
                    item.SubItems.Add(fileInfo.FullName);
                    item.SubItems.Add(fileInfo.Length.ToString("#,##0"));
                    item.SubItems.Add(len0.ToString("#,##0"));
                    item.SubItems.Add(fileInfo.LastWriteTime.ToString("g"));
                }

                dia = 4;

                for (int fragIdx = 0; fragIdx < len0; fragIdx++)
                {
                    // for (int y = 0; y < len1; y++)
                    Int64 vcn = (Int64)vcn_lcn.GetValue(fragIdx, 0);
                    Int64 lcn = (Int64)vcn_lcn.GetValue(fragIdx, 1);
                    Int64 len = vcn - prevVcn;
                    prevVcn = vcn;

                    if (lcn > 0)
                    {
                        if (updList)
                        {
                            if (fragIdx < 200)
                            {
                                item = infoList.Items.Add("");
                                item.SubItems.Add(len.ToString("#,##0"));
                                item.SubItems.Add(lcn.ToString("#,##0"));
                            }
                            else if (fragIdx == 200)   // limit list length
                            {
                                item = infoList.Items.Add("");
                                item.SubItems.Add("too many");
                                item.SubItems.Add("truncated list");
                            }
                        }

                        if (len > 1024*1024)
                            len = 1;    // truncate length, it is probably corrupt

                        int colorScale = (int)len;
                        Pen[] pens = new Pen[2] {
                                new Pen(Color.FromArgb(0, 255, 255), 1.0f),
                                new Pen(Color.FromArgb(255, 0, 255), 1.0f)};


                        int lastPixel = -1;
                        while (len-- > 0)
                        {
                            int pixel = (int)(lcn / clusterPerPixel);
                            vcn++;
                            lcn++;

                            if (pixel != lastPixel)
                            {
                                lastPixel = pixel;
                                lastP = new Point((int)((pixel % bgW) * xScale), (int)((pixel / bgW) * yScale));
                                if (firstP == Point.Empty)
                                    firstP = lastP;

                                // Pick some colors, alternate color by fragmentation.
                                Pen pen = pens[fragIdx & 1];
                                // g.DrawLine(Pens.Lavender, lastP, lastP);
                                g.DrawEllipse(pen, lastP.X - dia / 2, lastP.Y - dia / 2, dia, dia);
                            }
                        }
                    }
                }

                // Mark start and end of allocation chain
                dia = diaStart;
                g.DrawEllipse(Pens.Orange, firstP.X - dia / 2, firstP.Y - dia / 2, dia, dia);
                g.DrawEllipse(Pens.Orange, lastP.X - dia / 2, lastP.Y - dia / 2, dia, dia);

                g.Flush();
                diskPicture.Refresh();

                // Update title
                this.filepathBox.Text += (this.filepathBox.Text.Length != 0 ? "; " : "") + filepath;

                this.status.Text += string.Format(" FileSize:{0}  {1}", fileInfo.Length, filepath);

                m_filepaths.Add(filepath);
                item = infoList.Items.Add("");

                if (animate)
                {
                    this.timer.Start();
                    this.sPlayer.Stream = Properties.Resources.sonar;
                    this.sPlayer.Play();
                }
            }
            catch (Exception ee)
            {
                this.status.Text = ee.Message + ", Failed to highlight allocation of file:" + filepath;
            }
        }

        private void RefreshDriveAllocationImage(string device)
        {
            this.status.Text = "Building disk cluster allocation image for drive " + device;

            this.Cursor = Cursors.WaitCursor;
          
            MakeDriveAllocationImage(device);
            MakeGrid();

            this.Cursor = Cursors.Default;
        }

        private void splitter_SplitterMoved(object sender, SplitterEventArgs e)
        {
            UpdateOverlay();
        }

        private void splitter_SplitterMoving(object sender, SplitterCancelEventArgs e)
        {
            this.diskPicture.Image = null;
        }

        /// <summary>
        /// Help resize by hiding overlay, regenerate overlay, unhide.
        /// </summary>
        /// <param name="e"></param>
        FormWindowState beginWinState = FormWindowState.Normal;
        
        protected override void OnResizeBegin(EventArgs e)
        {
            base.OnResizeBegin(e);
            this.diskPicture.Image = null;
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            UpdateOverlay();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (beginWinState != this.WindowState)
            {
                beginWinState = this.WindowState;
                UpdateOverlay();
            }
        }

        private void UpdateOverlay()
        {
            // Remake grid and optionally file allocation
            MakeGrid();
            // MakeLegendImage();
            if (m_filepaths != null)
                HighLightFileAllocation(m_filepaths.ToArray(), true);
        }
       
        private void MakeLegendImage()
        {
            Size size = new Size(4, 256);   //  legendBox.DisplayRectangle.Size;

            Bitmap bmImage = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

            ColorPalette colorPalette = bmImage.Palette;
            for (int clrIdx = 0; clrIdx < 64; clrIdx++)
            {
                int clr1 = 64 - clrIdx;
                int clr2 = clrIdx * 2 + 128;
                colorPalette.Entries[clrIdx] = Color.FromArgb(255, clr2, clr2, clr1);         // r+g = yellow
                colorPalette.Entries[clrIdx + 128] = Color.FromArgb(255, clr1, clr1, clr2);   // blue
                colorPalette.Entries[clrIdx + 64] = Color.FromArgb(255, clr1, clr2, clr1);    // green
                colorPalette.Entries[clrIdx + 192] = Color.FromArgb(255, clr2, clr1, clr1);   // red
            }
            colorPalette.Entries[0] = Color.FromArgb(255, 0, 0, 0);
            bmImage.Palette = colorPalette;

            int imageSize = bmImage.Width * bmImage.Height;
            byte[] pixels = new byte[imageSize];

            // Add color bar legend
            int row = 0;
            for (int h = 0; h < bmImage.Height; h++)
            {
                for (int x = 0; x < bmImage.Width; x++)
                {
                    pixels[row + x] = (byte)(h * 255.0 / bmImage.Height);
                }
                row += bmImage.Width;
            }

            BitmapData bmpData = bmImage.LockBits(
                  new Rectangle(0, 0, bmImage.Width, bmImage.Height),
                  ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            Marshal.Copy(pixels, 0, bmpData.Scan0, imageSize);
            bmImage.UnlockBits(bmpData);

            legendBox.BackgroundImage = bmImage;
            legendBox.BackgroundImageLayout = ImageLayout.Stretch;
        }

        /// <summary>
        /// Make an image with each pixel colorized based on its associated cluster utilization.
        /// </summary>
        uint clusterPerPixel = 64;
        private void MakeDriveAllocationImage(string device)
        {
            Size size = diskPicture.DisplayRectangle.Size;

            Bitmap bmImage = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            diskPicture.BackgroundImage = bmImage;
            diskPicture.Refresh();

            bmImage.Palette = legendBox.BackgroundImage.Palette;

            DriveInfo driveInfo = new DriveInfo(device);
            if (driveInfo.IsReady == false)
            {
                status.Text = "Failed - Drive " + device + " is not ready";
                return;
            }

            BitArray bitArray;
            uint freeClusterCnt = 0;
         
            try
            {
                bitArray = FileSysInfo.GetVolumeMap(device);
                int imageSize = bmImage.Width * bmImage.Height;
                clusterPerPixel = (uint)Math.Ceiling((double)bitArray.Length / imageSize);

                if (bitArray.Length > imageSize)
                {
                    byte[] pixels = new byte[imageSize];

                    uint imgIdx = 0;
                    for (uint bitIdx = 0; bitIdx < bitArray.Length; bitIdx += clusterPerPixel)
                    {
                        uint pixelCnt = 0;
                        uint clsIdx = bitIdx;
                        uint clsEnd = Math.Min((uint)bitArray.Length, clsIdx + clusterPerPixel);
                        uint numCls = clsEnd - clsIdx;
                        while (clsIdx < clsEnd)
                        {
                            if (bitArray[(int)clsIdx++])
                                pixelCnt++;
                        }

                        freeClusterCnt += numCls - pixelCnt;
                        if (numCls > 0)
                        {
                            // byte pixelVal = (byte)(Math.Round(pixelCnt * 8.0 / numCls) * 32 - 1);
                            byte pixelVal = (byte)(pixelCnt * 255 / numCls);
                            pixels[imgIdx] = pixelVal;
                        }

                        imgIdx++;
                    }

                    BitmapData bmpData = bmImage.LockBits(
                          new Rectangle(0, 0, bmImage.Width, bmImage.Height),
                          ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

                    Marshal.Copy(pixels, 0, bmpData.Scan0, imageSize);
                    bmImage.UnlockBits(bmpData);
                    long usedPercent = (bitArray.Length - freeClusterCnt) * 100 / bitArray.Length;
                    long freePercent = freeClusterCnt * 100 / bitArray.Length;
                    this.status.Text = string.Format("Disk Clusters:{0:#,##0} Used:{1}% Free:{2}% Cluster/Pixel:{3}",
                            bitArray.Length, usedPercent, freePercent, clusterPerPixel);
                }
                else
                {
                    byte[] pixels = new byte[imageSize];

                    uint imgIdx = 0;
                    for (uint bitIdx = 0; bitIdx < bitArray.Length; bitIdx++)
                    {
                        if (bitArray[(int)bitIdx])
                        {
                            pixels[imgIdx] = 255;
                        }
                        else
                        {
                            pixels[imgIdx] = 1;    // add a little color to zero area
                            freeClusterCnt++;
                        }

                        imgIdx++;
                    }

                    BitmapData bmpData = bmImage.LockBits(
                          new Rectangle(0, 0, bmImage.Width, bmImage.Height),
                          ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

                    Marshal.Copy(pixels, 0, bmpData.Scan0, imageSize);
                    bmImage.UnlockBits(bmpData);
                    long usedPercent = (bitArray.Length - freeClusterCnt) * 100 / bitArray.Length;
                    long freePercent = freeClusterCnt * 100 / bitArray.Length;
                    this.status.Text = string.Format("Disk Clusters:{0} Used:{1}% Free:{2}% Cluster/Pixel:{3}",
                            bitArray.Length, usedPercent, freePercent, clusterPerPixel);
                }
            }
            catch (Exception ee)
            {
                status.Text = "Failed - " + ee.Message;
                if (ee.Message == "Access is denied.")
                    status.Text += ", run Magnifile as Administrator (see help)";
            }

            diskPicture.BackgroundImage = bmImage;
            diskPicture.Refresh();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Capture image of dialog (screen shot)
        /// </summary>
        /// <returns></returns>
        public Image MakeScreenImage()
        {
            Rectangle rect = this.Bounds;

            int color = Screen.PrimaryScreen.BitsPerPixel;
            PixelFormat pFormat;

            switch (color)
            {
                case 8:
                case 16:
                    pFormat = PixelFormat.Format16bppRgb565;
                    break;

                case 24:
                    pFormat = PixelFormat.Format24bppRgb;
                    break;

                case 32:
                    pFormat = PixelFormat.Format32bppArgb;
                    break;

                default:
                    pFormat = PixelFormat.Format32bppArgb;
                    break;
            }

            Application.DoEvents();
            Image image = new Bitmap(rect.Width, rect.Height, pFormat);
            Graphics g = Graphics.FromImage(image);
            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, rect.Size);

            return image;
        }


        /// <summary>
        /// Save screen shot as image, prompt for file and save image.
        /// </summary>
        private void saveImageBtn_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string ext = Path.GetExtension(this.saveFileDialog.FileName);
                if (ext == ".csv" || ext == ".txt")
                {
                    ListViewExt.Export(infoList, this.saveFileDialog.FileName);
                }
                else
                {
                    try
                    {
                        System.Threading.Thread.Sleep(1000);    // wait for file dialog to close
                        this.MakeScreenImage().Save(this.saveFileDialog.FileName);
                        this.status.Text = "Save screen to image:" + this.saveFileDialog.FileName;
                    }
                    catch (Exception ee)
                    {
                        this.status.Text = ee.Message + ", Failed to save image to:" + this.saveFileDialog.FileName;
                    }
                }
            }
        }

        /// <summary>
        /// File browse button - selected file while have its cluter allocation displayed in overlay.
        /// </summary>
        private void browseBtn_Click(object sender, EventArgs e)
        {
            // TODO - set file browser to combo box drive.
            try
            {
                if (File.Exists(openFileDialog.FileName))
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                string initDir = openFileDialog.InitialDirectory;
                if (initDir.Length > 1)
                    openFileDialog.InitialDirectory = this.driveCombo.SelectedItem.ToString()[0] + initDir.Substring(1);
                else
                    openFileDialog.InitialDirectory = this.driveCombo.SelectedItem.ToString();
            }
            catch { }
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ViewFileAllocation(openFileDialog.FileNames);
            }
        }

        private void filepathBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ViewFileAllocation(filepathBox.Text);
            }
            else if (e.KeyCode == Keys.Tab)
            {
                // ToDo: How to get tab to select first autocomplete choice.
            }
        }

        /// <summary>
        /// Eye candy - pulse rings around start and end of file alloocation sequence.
        /// </summary>
        Image overlay;
        private void timer_Tick(object sender, EventArgs e)
        {
            if (diskPicture.Image != null)
            {
                if (dia == diaStart)
                    overlay = (Image)diskPicture.Image.Clone();
                else
                    diskPicture.Image = (Image)overlay.Clone();

                dia += 10;
                if (dia == diaStart + 400)
                    timer.Stop();
                else
                {
                    Graphics g = Graphics.FromImage(diskPicture.Image);

                    // Mark start and end of allocation chain
                    g.DrawEllipse(Pens.Orange, firstP.X - dia / 2, firstP.Y - dia / 2, dia, dia);
                    g.DrawEllipse(Pens.Orange, lastP.X - dia / 2, lastP.Y - dia / 2, dia, dia);
                }

            }
        }

        private void ResetAnimation()
        {
            if (dia > diaStart)
            {
                diskPicture.Image = (Image)overlay.Clone();
                dia = diaStart;
            }
        }

        /// <summary>
        /// Load listView with system filenames, double click on row to file allocation.
        /// </summary>
        private void UpdateSysFileTab()
        {
            try
            {
                Directory.SetCurrentDirectory(this.driveCombo.SelectedItem.ToString());
            }
            catch { }
            string rootDir = Path.GetPathRoot(Directory.GetCurrentDirectory());

            if (sysFileView.Items.Count > 2 && sysFileView.Items[1].SubItems[2].Text == rootDir)
                return;

            Cursor.Current = Cursors.WaitCursor;

            sysFileView.Items.Clear();
            ListViewItem item;

            item = sysFileView.Items.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("Base");
            item.BackColor = Color.LightBlue;

            item = sysFileView.Items.Add("");
            item.SubItems.Add("");
            item.SubItems.Add(rootDir);

            item = sysFileView.Items.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("NTFS");
            item.BackColor = Color.LightBlue;
            AddFilesToView(sysFileView, FileSysInfo.NtfsSystemFiles);

            item = sysFileView.Items.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("Config");
            item.BackColor = Color.LightBlue;
            AddFilesToView(sysFileView, FileSysInfo.ConfigSystemFiles);

            item = sysFileView.Items.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("Registry");
            item.BackColor = Color.LightBlue;
            AddFilesToView(sysFileView, FileSysInfo.RegistryFiles);

            Cursor.Current = Cursors.Default;
        }

        private void AddFilesToView(ListView listView, string[] files)
        {
            foreach (string file in files)
            {
                AddFilesToView(listView, file);
            }
        }

        private void AddFilesToView(ListView listView, string file)
        {
            // View column layout:
            //  0   blank   
            //  1   blank
            //  2   filename
            //  3   size
            //  4   #fragments
            //  5   date (or error message)
            
            ListViewItem item = listView.Items.Add("");
            item.SubItems.Add("");
            item.SubItems.Add(file);
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("");

            try
            {
                Array vcn_lcn = FileSysInfo.GetFileMap(file);
                item.SubItems[4].Text = (vcn_lcn.Length/2).ToString("#,##0");

                try
                {
                    FileInfo finfo = new FileInfo(file);
                    item.SubItems[3].Text = finfo.Length.ToString("#,##0");
                    item.SubItems[5].Text = finfo.LastWriteTime.ToString("G");
                }
                catch { }
            }
            catch (Exception ee)
            {
                item.SubItems[5].Text = ee.Message;
            }
        }


        private void infoList_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                infoList_DoubleClick(sender, EventArgs.Empty);
        }

        /// <summary>
        ///  Double click on system file - view its file allocation in overlay.
        /// </summary>
        private void infoList_DoubleClick(object sender, EventArgs e)
        {
            ListView listView = (ListView)sender;

            if (listView.SelectedItems.Count != 0)
            {
                string filePath = string.Empty;
                foreach (ListViewItem item in listView.SelectedItems)
                {
                    if (item.SubItems.Count > 2 &&
                        item.SubItems[2] != null)
                    {
                        if (filePath.Length > 0)
                            filePath += ";";
                        filePath += item.SubItems[2].Text;
                    }
                }

                this.status.Text = filePath;
                this.ViewFileAllocation(filePath.Split(';'));
            }
        }

        private void infoList_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            if (infoList.SelectedItems.Count != 0)
            {
                ListViewItem item = infoList.SelectedItems[0];
                if (item.BackColor == fileColor && item.Tag != null)
                {
                    ResetAnimation();
                    string fullPath = (string)item.Tag;
                    this.HighLightFileAllocation(fullPath, false, false, true);
                }
                else if (item.SubItems.Count > 2)
                {
                    this.status.Text = item.SubItems[2].Text;
                    if (item.SubItems.Count > 3)
                        this.status.Text += ", " + item.SubItems[3].Text;
                    if (item.SubItems.Count > 4)
                        this.status.Text += ", " + item.SubItems[4].Text;
                }
            }
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }

        private void ShowHelp()
        {
            Assembly a = Assembly.GetExecutingAssembly();
            // string[] resNames = a.GetManifestResourceNames();
            Stream hlpStream = a.GetManifestResourceStream("MagniFile.HelpDiskAllocation.html");

            if (hlpStream != null)
            {
                ViewHelp helpDialog = new ViewHelp(hlpStream, logo, this.title.Text);
                helpDialog.Show(this);
            }
        }

        private void diskPicture_MouseMove(object sender, MouseEventArgs e)
        {
            uint cluster = (uint)e.Y * (uint)diskPicture.BackgroundImage.Width * this.clusterPerPixel;

            clusterVal.Text = cluster.ToString("#,##0");
        }

      
        private void button_MouseEnter(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.BackgroundImage = Properties.Resources.hblueGlow;
        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.BackgroundImage = Properties.Resources.hblue;
        }

        private void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabs.SelectedTab == tabSysFiles)
                UpdateSysFileTab();
        }

     
    }
}
