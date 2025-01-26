using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Win32;


namespace MagniFile
{
    /// Display information about author & program.
    /// 
    /// Author: Dennis Lang 2009
    /// https://landenlabs.com/
    /// 
   
	/// <summary>
	/// About Author and application.
	/// </summary>
	public class About : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox logoBox;
		private System.Windows.Forms.Label titleBar;
		private System.Windows.Forms.LinkLabel linkLabel1;
        private Label label1;
        private Button closeBtn;
        private Label aboutDetails;
        private Label titleBarbg;
        private CheckBox regAllocationCk;
        private ToolTip abouToolTip;
        private IContainer components;

        public About(MainForm mainform)
		{
			InitializeComponent();
            titleBar.Text = MainForm.ProductNameAndVersion();
            titleBarbg.Text = MainForm.ProductNameAndVersion();

            this.titleBar.Parent = this.titleBarbg;
            this.titleBar.Location = new Point(1, 1);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.logoBox = new System.Windows.Forms.PictureBox();
            this.titleBar = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.closeBtn = new System.Windows.Forms.Button();
            this.aboutDetails = new System.Windows.Forms.Label();
            this.titleBarbg = new System.Windows.Forms.Label();
            this.regAllocationCk = new System.Windows.Forms.CheckBox();
            this.abouToolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.logoBox)).BeginInit();
            this.SuspendLayout();
            // 
            // logoBox
            // 
            this.logoBox.BackColor = System.Drawing.Color.Transparent;
            this.logoBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("logoBox.BackgroundImage")));
            this.logoBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.logoBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logoBox.Location = new System.Drawing.Point(12, 12);
            this.logoBox.Name = "logoBox";
            this.logoBox.Size = new System.Drawing.Size(128, 128);
            this.logoBox.TabIndex = 0;
            this.logoBox.TabStop = false;
            // 
            // titleBar
            // 
            this.titleBar.BackColor = System.Drawing.Color.Transparent;
            this.titleBar.Font = new System.Drawing.Font("Franklin Gothic Medium", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleBar.ForeColor = System.Drawing.Color.Red;
            this.titleBar.Location = new System.Drawing.Point(385, 44);
            this.titleBar.Name = "titleBar";
            this.titleBar.Size = new System.Drawing.Size(206, 23);
            this.titleBar.TabIndex = 3;
            this.titleBar.Text = "MagniFile v3.4";
            this.titleBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel1
            // 
            this.linkLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(308, 12);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(275, 20);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://landenlabs.com";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(387, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 23);
            this.label1.TabIndex = 5;
            this.label1.Text = "By: Dennis Lang";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // closeBtn
            // 
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBtn.BackColor = System.Drawing.Color.Navy;
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeBtn.ForeColor = System.Drawing.Color.White;
            this.closeBtn.Location = new System.Drawing.Point(516, 443);
            this.closeBtn.Margin = new System.Windows.Forms.Padding(0);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(75, 23);
            this.closeBtn.TabIndex = 7;
            this.closeBtn.Text = "Close";
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // aboutDetails
            // 
            this.aboutDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.aboutDetails.BackColor = System.Drawing.Color.Transparent;
            this.aboutDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutDetails.ForeColor = System.Drawing.Color.White;
            this.aboutDetails.Image = ((System.Drawing.Image)(resources.GetObject("aboutDetails.Image")));
            this.aboutDetails.Location = new System.Drawing.Point(12, 163);
            this.aboutDetails.Name = "aboutDetails";
            this.aboutDetails.Size = new System.Drawing.Size(493, 303);
            this.aboutDetails.TabIndex = 8;
            this.aboutDetails.Text = resources.GetString("aboutDetails.Text");
            // 
            // titleBarbg
            // 
            this.titleBarbg.BackColor = System.Drawing.Color.Transparent;
            this.titleBarbg.Font = new System.Drawing.Font("Franklin Gothic Medium", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleBarbg.ForeColor = System.Drawing.Color.White;
            this.titleBarbg.Location = new System.Drawing.Point(385, 44);
            this.titleBarbg.Name = "titleBarbg";
            this.titleBarbg.Size = new System.Drawing.Size(206, 23);
            this.titleBarbg.TabIndex = 9;
            this.titleBarbg.Text = "Magnifile v3.4";
            this.titleBarbg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // regAllocationCk
            // 
            this.regAllocationCk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.regAllocationCk.AutoSize = true;
            this.regAllocationCk.BackColor = System.Drawing.Color.Navy;
            this.regAllocationCk.ForeColor = System.Drawing.Color.White;
            this.regAllocationCk.Location = new System.Drawing.Point(516, 412);
            this.regAllocationCk.Name = "regAllocationCk";
            this.regAllocationCk.Size = new System.Drawing.Size(69, 17);
            this.regAllocationCk.TabIndex = 10;
            this.regAllocationCk.Text = "RegAlloc";
            this.abouToolTip.SetToolTip(this.regAllocationCk, "Register/Unregister shell pulldown menu choice to view file allocation map.");
            this.regAllocationCk.UseVisualStyleBackColor = false;
            this.regAllocationCk.Click += new System.EventHandler(this.RegAllocMenu_Click);
            // 
            // About
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(605, 488);
            this.Controls.Add(this.regAllocationCk);
            this.Controls.Add(this.aboutDetails);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.titleBar);
            this.Controls.Add(this.logoBox);
            this.Controls.Add(this.titleBarbg);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.logoBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel linkLabel = sender as LinkLabel;
            System.Diagnostics.Process.Start(linkLabel.Text);
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RegAllocMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.regAllocationCk.Checked)
                    Register();
                else
                    Unregister();
            }
            catch (Exception e1)
            {
                MessageBox.Show(this, "Unable to perform operation : " + e1.Message);
            }
        }

        private const string RegAnyShell = "*\\shell\\";

        private void DesktopRegistry()
        {
            // 
            //  classes_Root\DesktopBackground\Shell\
            //      Magnfile\   default
            //          Icon="...Magnifile.exe"
            //          SubCommands="Magnfile;Magnfile.Azulon"

            // LocalMachin\softwaer\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\Shell\
            //          Magnifile\ default="Magnifile"
            //          icon = "...Magnifile.exe"
            //          command\  default="..Magnifile.exe"
            //
            //          Magnifile.Azulon\   default="graph Azulon memory"
            //          icon =  
            //          commnad\  default = " ...Magnfile.exe -graphProc Azulon.exe -graphParam PrivateMem -graphXtime 1hour
        }

        private void Register()
        {
            string appName = Application.ProductName;
            string RegShell = RegAnyShell + appName;
            string Command = RegShell + "\\command";

            RegistryKey regmenu = null;
            RegistryKey regcmd = null;
            try
            {
                regmenu = Registry.ClassesRoot.CreateSubKey(RegShell);
                if (regmenu != null)
                    regmenu.SetValue("", "Allocation " + appName);
                regcmd = Registry.ClassesRoot.CreateSubKey(Command);
                if (regcmd != null)
                    regcmd.SetValue("", Environment.CommandLine + 
                        " -exit -hide -filemap " + "\"%1\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem registering application on Shell, " + ex.Message);
            }
            finally
            {
                if (regmenu != null)
                    regmenu.Close();
                if (regcmd != null)
                    regcmd.Close();
            }
        }

        private void Unregister()
        {
            string appName = Application.ProductName;
            string RegShell = RegAnyShell + appName;
            string Command = RegShell + "\\command";

            try
            {
                RegistryKey reg = Registry.ClassesRoot.OpenSubKey(Command);
                if (reg != null)
                {
                    reg.Close();
                    Registry.ClassesRoot.DeleteSubKey(Command);
                }
                reg = Registry.ClassesRoot.OpenSubKey(RegShell);
                if (reg != null)
                {
                    reg.Close();
                    Registry.ClassesRoot.DeleteSubKey(RegShell);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem removing application info from Shell, " + ex.Message);
            }
        }


	}
}
