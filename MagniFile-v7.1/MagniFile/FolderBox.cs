using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MagniFile
{
    /// <summary>
    /// File or directory filename selection dialog.
    /// 
    /// Author: Dennis Lang 2009
    /// https://landenlabs.com/
    /// </summary>
    public partial class FolderBox : Form
    {
        public FolderBox()
        {
            InitializeComponent();
        }

        private void FileBox_Load(object sender, EventArgs e)
        {
        }

        public string FileText
        {
            get { return this.textBox.Text; }
            set { this.textBox.Text = value; }
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void loadBtn_Click(object sender, EventArgs e)
        {
#if true
            this.folderBrowserDialog.SelectedPath = this.textBox.Text;
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox.Text = this.folderBrowserDialog.SelectedPath;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
#else
            this.openFileDialog.FileName = this.textBox.Text;
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox.Text = this.openFileDialog.FileName;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
#endif
        }
    }
}