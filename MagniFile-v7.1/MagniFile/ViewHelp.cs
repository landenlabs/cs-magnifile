using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MagniFile
{
    /// <summary>
    /// General purpose html help viewer
    /// 
    /// Author: Dennis Lang 2009
    /// https://landenlabs.com/
    /// </summary>
    public partial class ViewHelp : Form
    {
        public ViewHelp(Stream htmlStream, Image logoImage, string title)
        {
            InitializeComponent();

            // Prevent viewer from being larger than working area.
            if (this.Height > Screen.GetWorkingArea(this).Height * 0.9)
                this.Height = (int)(Screen.GetWorkingArea(this).Height * 0.9);

            this.Text = "Help - " + MainForm.ProductNameAndVersion();

            if (logoImage != null)
                this.logo.BackgroundImage = logoImage;

            this.title.Text = this.titleBg.Text = title;
            this.titleBg.Parent = this.titleBar;
            this.title.Parent = this.titleBg;
            this.title.Location = new Point(1, 1);
            OnResizeEnd(EventArgs.Empty);

            // get a reference to the current assembly
            this.webBrowser.DocumentStream = htmlStream;
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            this.titleBg.Location = new Point(
                Math.Max(0, (this.titleBar.Width - this.titleBg.Width) / 2),
                this.titleBg.Location.Y);

            this.logo.Visible = !this.logo.DisplayRectangle.Contains(this.titleBg.Location);

            base.OnResizeEnd(e);
        }                  

        private void printBtn_Click(object sender, EventArgs e)
        {
            this.webBrowser.Print();
            MessageBox.Show("Document sent to printer");
        }
    }
}
