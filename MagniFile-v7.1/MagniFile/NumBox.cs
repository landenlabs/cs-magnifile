using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MagniFile
{
    /// Used to prompt for input from a tabular list field.
    /// Place box directly over tabular list field and monitor change event.
    /// 
    /// Author: Dennis Lang - 2009
    /// https://landenlabs.com/
    /// 
    public partial class NumBox : Form
    {
        public NumBox()
        {
            InitializeComponent();
        }

        public double mouseNumChange = 1;
        public int mouseScale = 10;
        public event EventHandler changed;

        public class NumEvent : EventArgs
        {
            public NumEvent(double n, object t)
            {
                num = n;
                tag = t;
            }
            public double num;
            public object tag;
        }

        public string FieldText
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

        Point mouseStart = Point.Empty;
        double orgValue = 0;
        private void textBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseStart != Point.Empty && this.DisplayRectangle.Contains(this.PointToClient(e.Location)) == false)
            {
                int dX = e.X - mouseStart.X;
                int dY = mouseStart.Y - e.Y;
                double d = orgValue + (((Math.Abs(dX) > Math.Abs(dY)) ? dX : dY) / mouseScale) * mouseNumChange;

                this.textBox.Text = d.ToString();
                if (changed != null)
                    changed.Invoke(this, new NumEvent(d, this.Tag));
            }
        }

        private void textBox_MouseDown(object sender, MouseEventArgs e)
        {
            mouseStart = e.Location;
            orgValue = 0;
            try
            {
                orgValue = double.Parse(this.textBox.Text);
            }
            catch { }
        }

        private void textBox_MouseUp(object sender, MouseEventArgs e)
        {
            mouseStart = Point.Empty;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}