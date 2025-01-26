using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace MagniFile
{
    /// <summary>
    /// ListView column sorter (uses  ordinal ignore case compare) for alpha comparison.
    /// Can cascade and sort two columns.
    /// Can sort alpha, numeric or dates.
    /// 
    /// Author: Dennis Lang - 2009
    /// https://landenlabs.com/
    ///   /// </summary>
    public class ListViewColumnSorter : System.Collections.IComparer
    {
        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int columnToSort1;
        private int columnToSort2;

        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder orderOfSort1;
        private SortOrder orderOfSort2;

        public enum SortDataType { eAuto, eAlpha, eNumeric, eDateTime, eTagUlong, eTagDateTime };
        private SortDataType sortDataType;

        // Debug diagnostic 
        public ulong callCount;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter(SortDataType inSortDataType)
        {
            this.sortDataType = inSortDataType;

            // Initialize the column to '0'
            this.columnToSort1 = 0;
            this.columnToSort2 = 0;

            // Initialize the sort order to 'none'
            this.orderOfSort1 = SortOrder.None;
            this.orderOfSort2 = SortOrder.None;
        }

        public SortDataType SortType
        {
            get { return this.sortDataType; }
            set { this.sortDataType = value; }
        }

        private bool IsNumber(ref string str)
        {
            if (str.Length == 0)
                return false;

            bool num = false;
            int dots = 0;
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];

                if (c == '.')       // only remove 1st dot
                {
                    if (dots++ > 0)
                        str = str.Remove(i);
                }
                else if (c == ',')  // remove all commas
                {
                    str = str.Remove(i,1);
                }
                else if (char.IsNumber(c) == false && c != '-' && c != ' ')
                {
                    if (num)
                        str = str.Remove(i);   // remove to end-of-string
                    return num;
                }
                
                num |= (c != ' ');
            }
            return true;
        }

        private int NumCompare(string strX, string strY)
        {
            if (IsNumber(ref strX) && IsNumber(ref strY))
            {
                double numX = strX.Length == 0 ? 0 : double.Parse(strX);
                double numY = strY.Length == 0 ? 0 : double.Parse(strY);
                return numX == numY ? 0 : (numX < numY ? -1 : 1);
            }
            return  String.Compare(strX, strY, StringComparison.OrdinalIgnoreCase);
        }

        private int DateTimeCompare(string strX, string strY)
        {
            DateTime dtX, dtY;
            int result = 0;

            if (DateTime.TryParse(strX, out dtX) &&
                DateTime.TryParse(strY, out dtY))
            {
                result = (int)dtX.Subtract(dtY).TotalSeconds;
            } 

            // 2nd try
            else if (
                DateTime.TryParseExact(strX, "G", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtX) &&
                DateTime.TryParseExact(strX, "G", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtY))
            {
                result = (int)dtX.Subtract(dtY).TotalSeconds;
            }


            return result;
        }


        /// <summary>
        /// This method is inherited from the IComparer interface.  
        /// It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, 
        /// negative if 'x' is less than 'y' and positive if 'x' 
        /// is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            callCount++;

            ListViewItem listviewX = (ListViewItem)x;
            ListViewItem listviewY = (ListViewItem)y;

            int compareResult = Compare(this.columnToSort1, this.orderOfSort1, listviewX, listviewY);
            if (compareResult == 0 && this.columnToSort2 != this.columnToSort1)
                compareResult = Compare(this.columnToSort2, this.orderOfSort2, listviewX, listviewY);

            return compareResult;
        }

        private int Compare(int column, SortOrder orderOfSort, ListViewItem listviewX, ListViewItem listviewY)
        {
            int compareResult = 0;
            if (column >= listviewX.SubItems.Count ||
                column >= listviewY.SubItems.Count)
                return listviewX.SubItems.Count - listviewY.SubItems.Count;

            // Cast the objects to be compared to ListViewItem objects
            string strX = listviewX.SubItems[column].Text;
            string strY = listviewY.SubItems[column].Text;

            if (this.sortDataType == SortDataType.eAuto)
            {
                DateTime dtX, dtY;

                if (DateTime.TryParse(strX, out dtX) && DateTime.TryParse(strY, out dtY))
                {
                    // Date
                    compareResult = (int)dtX.Subtract(dtY).TotalSeconds;
                }
                else
                {
                    // String
                    compareResult = String.Compare(strX, strY, StringComparison.OrdinalIgnoreCase);
                    if (IsNumber(ref strX) && IsNumber(ref strY))
                    {
                        // Number
                        try
                        {
                            double numX = double.Parse(strX);
                            double numY = double.Parse(strY);
                            compareResult = (numX == numY) ? 0 : ((numX < numY) ? -1 : 1);
                        }
                        catch { }
                    }
                }
            }

            switch (this.sortDataType)
            {
                case SortDataType.eAlpha:
                    // Compare the two items
                    compareResult = String.Compare(strX, strY, StringComparison.OrdinalIgnoreCase);
                    break;
                case SortDataType.eNumeric:
                    compareResult = NumCompare(strX, strY);
                    break;
                case SortDataType.eDateTime:
                    compareResult = DateTimeCompare(strX, strY);
                    break;
                case SortDataType.eTagDateTime:
                    DateTime xDt = (DateTime)listviewX.SubItems[column].Tag;
                    DateTime yDt = (DateTime)listviewY.SubItems[column].Tag;
                    compareResult = xDt.CompareTo(yDt);
                    break;
                case SortDataType.eTagUlong:
                    ulong xNum = (ulong)listviewX.SubItems[column].Tag;
                    ulong yNum = (ulong)listviewY.SubItems[column].Tag;
                    compareResult = (xNum == yNum) ? 0 : ((xNum < yNum) ? -1 : 1);
                    break;
            }

            // Calculate correct return value based on object comparison
            if (orderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (orderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn1
        {
            set
            {
                this.columnToSort1 = value;
                // this.columnToSort2 = value;  // assume single sort, set both the same
            }
            get
            {
                return this.columnToSort1;
            }
        }

        public int SortColumn2
        {
            set
            {
                this.columnToSort2 = value;
            }
            get
            {
                return this.columnToSort2;
            }
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order1
        {
            set
            {
                this.orderOfSort1 = value;
            }
            get
            {
                return this.orderOfSort1;
            }
        }

        public SortOrder Order2
        {
            set
            {
                this.orderOfSort2 = value;
            }
            get
            {
                return this.orderOfSort2;
            }
        }
    }
}
