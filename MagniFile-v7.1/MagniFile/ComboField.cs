using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MagniFile
{
    /// Simple combo box used to update a field in a tabular list box.
    ///   
    /// Author: Dennis Lang - 2009
    /// https://landenlabs.com/
    /// 
    public partial class ComboField : Form
    {
        public ComboField()
        {
            InitializeComponent();
        }

        public void SetChoices(string choiceStr)
        {
            string[] choices = choiceStr.Split(',');
            SetChoices(choices);
        }

        public void SetChoices(string[] choices)
        {
            this.combo.Items.Clear();
            AddChoices(choices);
        }

        public void AddChoices(string[] choices)
        {
            this.combo.SelectedIndexChanged -= new System.EventHandler(this.combo_SelectedIndexChanged);
            this.combo.Items.AddRange(choices);
            this.combo.SelectedIndexChanged += new System.EventHandler(this.combo_SelectedIndexChanged);
        }

        public string ComboText
        {
            get { return this.combo.Text; }
            set { this.combo.Text = value; }
        }

        public Font ComboFont
        {
            get { return this.combo.Font; }
            set { this.combo.Font = value; }
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

        private void combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
