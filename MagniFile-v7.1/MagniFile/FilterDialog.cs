using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MagniFile
{
    public partial class FilterDialog : Form
    {
        public FilterDialog()
        {
            InitializeComponent();
        }

        public class FilterItem
        {
            public bool    enabled;
            public Regex   regex;
        }

        public FilterList List
        {
            set
            {
                this.filterView.Items.Clear();
                foreach (FilterItem filterItem in value.List)
                {
                    filterView.Items.Add(filterItem.regex.ToString()).Checked = filterItem.enabled;
                }
            }

            get
            {
                List<FilterItem> filterList = new List<FilterItem>();
                foreach (ListViewItem lvItem in filterView.Items)
                {
                    FilterItem filterItem = new FilterItem();
                    filterItem.enabled = lvItem.Checked;
                    filterItem.regex = new Regex(lvItem.Text);
                    filterList.Add(filterItem);
                }

                return new FilterList(filterList);
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            this.filterView.Items.Add("").Checked = true;
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in filterView.SelectedItems)
            {
                filterView.Items.Remove(item);
            }
        }

        private void filterView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (filterView.SelectedItems.Count != 0)
            {
                this.textBox.Text = filterView.SelectedItems[0].Text;
                this.status.Text = string.Empty;
            }
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            status.Text = string.Empty;
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    Regex regex = new Regex(textBox.Text);
                    if (filterView.SelectedItems.Count != 0)
                        filterView.SelectedItems[0].Text = textBox.Text;
                    else
                        filterView.Items.Add(textBox.Text).Checked = true;
                }
                catch (Exception ex)
                {
                    status.Text = ex.Message;
                }
            }

        }
    }

    public class FilterList
    {
        List<FilterDialog.FilterItem> m_list;

        public FilterList()
        {
            m_list = new List<FilterDialog.FilterItem>();
        }

        public FilterList(List<FilterDialog.FilterItem> list)
        {
            m_list = list;
        }

        public List<FilterDialog.FilterItem> List
        {
            get { return m_list;  }
            set { m_list = value; }
        }

        public bool IsMatch(string test)
        {
            bool anyEnabled = false;
            foreach (FilterDialog.FilterItem filterItem in m_list)
            {
                anyEnabled |= filterItem.enabled;

                if (filterItem.enabled &&
                    filterItem.regex.IsMatch(test))
                    return true;
            }

            return !anyEnabled;
        }



    }
}
