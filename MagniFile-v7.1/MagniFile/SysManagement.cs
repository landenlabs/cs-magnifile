using System.Drawing;
using System.Management;
using System.Windows.Forms;

namespace MagniFile {
	/// <summary>
	/// Display and log information about files.
	/// 
	/// Author: Dennis Lang 2009
	/// https://landenlabs.com/
	/// 
	/// The following module extracts disk drive information using the management subsystem.
	/// Code was derived from the following example:
	/// 
	/// http://www.geekpedia.com/tutorial233_Getting-Disk-Drive-Information-using-WMI-and-Csharp.html
	/// 
	/// </summary>
	class SysManagement {
		public static void ViewSysInfo(ListView listView, string f0, Color color) {
			// Get all the disk drives
			ManagementObjectSearcher mosDisks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

			// Loop through each object (disk) retrieved by WMI
			foreach (ManagementObject moDisk in mosDisks.Get()) {
				// Add the HDD to the list (use the Model field as the item's caption)
				string model = moDisk["Model"].ToString();
				AddSysInfo(listView, model, f0, color);
			}
		}

		private static void AddSysInfo(ListView listView, string model, string f0, Color color) {
			// Get all the disk drives from WMI that match the Model name selected in the ComboBox
			ManagementObjectSearcher mosDisks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive WHERE Model = '" + model + "'");

			// Loop through the drives retrieved, although it should normally be only one loop going on here
			foreach (ManagementObject moDisk in mosDisks.Get()) {
				// ListViewExt.ReflectToList(moDisk, listView, f0, color);
				string f0d = f0 + moDisk["Name"].ToString();

				//  additional info in moDisk.SystemProperties
				foreach (PropertyData propData in moDisk.Properties) {
					string name = propData.Name;
					ListViewItem item = listView.Items.Add(f0d);
					item.SubItems.Add(name);

					try {
						string val = moDisk[name].ToString();
						item.SubItems.Add(val);
					} catch {
						listView.Items.Remove(item);
					}
				}
			}
		}

	}
}
