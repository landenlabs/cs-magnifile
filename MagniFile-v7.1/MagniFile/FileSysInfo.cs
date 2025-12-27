using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;


namespace MagniFile {
	/// <summary>
	/// General File System Information routines.
	/// A bunch of APIs to system internals 
	/// 
	/// Author: Dennis Lang 2009
	/// https://landenlabs.com/
	/// 
	/// </summary>
	class FileSysInfo {
		//  The following code was derived from information provided by these links.
		//
		//  http://blogs.msdn.com/jeffrey_wall/archive/2004/09/13/229137.aspx
		//  http://www.google.com/search?hl=en&lr=&q=DeviceIoControl+c%23
		//
		// CreateFile constants
		//
		const uint FILE_SHARE_READ = 0x00000001;
		const uint FILE_SHARE_WRITE = 0x00000002;
		const uint FILE_SHARE_DELETE = 0x00000004;
		const uint FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;
		const uint OPEN_EXISTING = 3;

		const uint GENERIC_READ = (0x80000000);
		const uint GENERIC_WRITE = (0x40000000);

		const uint FILE_FLAG_NO_BUFFERING = 0x20000000;
		const uint FILE_READ_ATTRIBUTES = (0x0080);
		const uint FILE_WRITE_ATTRIBUTES = 0x0100;
		const uint FILE_ATTRIBUTE_SYSTEM = 0x00000004;
		const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
		const uint ERROR_INSUFFICIENT_BUFFER = 122;

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern IntPtr CreateFile(
			string lpFileName,
			uint dwDesiredAccess,
			uint dwShareMode,
			IntPtr lpSecurityAttributes,
			uint dwCreationDisposition,
			uint dwFlagsAndAttributes,
			IntPtr hTemplateFile);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern int CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern int DeviceIoControl(
			IntPtr hDevice,
			uint dwIoControlCode,
			IntPtr lpInBuffer,
			uint nInBufferSize,
			[Out] IntPtr lpOutBuffer,
			uint nOutBufferSize,
			ref uint lpBytesReturned,
			IntPtr lpOverlapped);

		// NOTE: Vista/Windows 7 - required Administrator priviledges, else you get "Access Denied".
		// Solution:  Run program as administrator.
		//
		// ex   @"\\.\PhysicalDrive0"
		// ex   @"\\.\c:"
		static private IntPtr OpenVolume(string DeviceName) {
			IntPtr hDevice;
			hDevice = CreateFile(
				@"\\.\" + DeviceName,
				GENERIC_READ | GENERIC_WRITE,
				FILE_SHARE_READ | FILE_SHARE_WRITE,
				IntPtr.Zero,
				OPEN_EXISTING,
				0,
				IntPtr.Zero);
			if ((int)hDevice == -1) {
				int err = Marshal.GetLastWin32Error();
				throw new ErrorMsg.Win32Exception(err, hDevice.ToInt32(), DeviceName);
			}
			return hDevice;
		}

		static private IntPtr OpenFile(string path) {
			IntPtr hFile;
			hFile = CreateFile(
						path,
						FILE_READ_ATTRIBUTES /* | FILE_WRITE_ATTRIBUTES */,
						FILE_SHARE_READ, //  | FILE_SHARE_WRITE | FILE_SHARE_DELETE,
						IntPtr.Zero,
						OPEN_EXISTING,
						FILE_ATTRIBUTE_SYSTEM, // FILE_FLAG_NO_BUFFERING | FILE_FLAG_BACKUP_SEMANTICS,   
						IntPtr.Zero);

			if ((int)hFile == -1) {
				int err = Marshal.GetLastWin32Error();
				throw new ErrorMsg.Win32Exception(err, hFile.ToInt32(), path);
			}
			return hFile;
		}

		#region ==== Volume and Partition


		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		static extern bool GetVolumeInformation(
			string Volume,
			StringBuilder VolumeName,
			uint VolumeNameSize,
			out uint SerialNumber,
			out uint SerialNumberLength,
			out uint flags,
			StringBuilder fs,
			uint fs_size);

		public static void GetVolume(string drive) {
			uint serialNum, serialNumLength, flags;
			StringBuilder volumename = new StringBuilder(256);
			StringBuilder fstype = new StringBuilder(256);

			/*
             
             Take a look at GetLogicalDriveStrings() and QueryDosDevice() to 
            get the volume names. Use IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS 
            to translate from the volume names into physical device device 
            numbers. Insert "\\.\PhysicalDrive" in front of the numbers to 
            read from that physical disk. 

             */

			string[] logDrives = Directory.GetLogicalDrives();

			// Use isReady instead of isFixed  

			DriveInfo[] drives = DriveInfo.GetDrives();
			foreach (DriveInfo d in drives) {
				string label = d.IsReady ? String.Format(" - {0}", d.VolumeLabel) : null;
				Console.WriteLine("{0} - {1}{2}", d.Name, d.DriveType, label);
			}

			bool ok = GetVolumeInformation(
					drive,
					volumename,
					(uint)volumename.Capacity - 1,
					out serialNum,
					out serialNumLength,
					out flags,
					fstype,
					(uint)fstype.Capacity - 1);

			if (ok) {
				//  SerialNumber of is..... " + serialNum.ToString() 
				if (volumename != null) {
					// volumename.ToString()  
				}
				if (fstype != null) {
					//  fstype.ToString()  
				}
			}
		}

		#endregion

		#region ==== DiskFreeSpace
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
		   out ulong lpFreeBytesAvailable,
		   out ulong lpTotalNumberOfBytes,
		   out ulong lpTotalNumberOfFreeBytes);

		// string device = driveLetter + ":\\";
		// return GetDiskFreeSpaceEx(device, out freeBytesAvail, out totalNumOfBytes, out totalNumOfFreeBytes);
		#endregion


		#region ==== File System Statistics
		[StructLayout(LayoutKind.Sequential)]
		public struct FILESYSTEM_STATISTICS {
			public UInt16 FileSystemType;
			public UInt16 Version;
			UInt32 SizeOfCompleteStructure;
			public UInt32 UserFileReads;
			public UInt32 UserFileReadBytes;
			public UInt32 UserDiskReads;
			public UInt32 UserFileWrites;
			public UInt32 UserFileWriteBytes;
			public UInt32 UserDiskWrites;
			public UInt32 MetaDataReads;
			public UInt32 MetaDataReadBytes;
			public UInt32 MetaDataDiskReads;
			public UInt32 MetaDataWrites;
			public UInt32 MetaDataWriteBytes;
			public UInt32 MetaDataDiskWrites;
		};

		public static void Merge(ref FILESYSTEM_STATISTICS inOut1, FILESYSTEM_STATISTICS in2) {
			inOut1.UserFileReads += in2.UserFileReads;
			inOut1.UserFileReadBytes += in2.UserFileReadBytes;
			inOut1.UserDiskReads += in2.UserDiskReads;
			inOut1.UserFileWrites += in2.UserFileWrites;
			inOut1.UserFileWriteBytes += in2.UserFileWriteBytes;
			inOut1.UserDiskWrites += in2.UserDiskWrites;
			inOut1.MetaDataReads += in2.MetaDataReads;
			inOut1.MetaDataReadBytes += in2.MetaDataReadBytes;
			inOut1.MetaDataDiskReads += in2.MetaDataDiskReads;
			inOut1.MetaDataWrites += in2.MetaDataWrites;
			inOut1.MetaDataWriteBytes += in2.MetaDataWriteBytes;
			inOut1.MetaDataDiskWrites += in2.MetaDataDiskWrites;
		}

		/// <summary>
		/// Reflect list into a list of names and values stored in a dictionary.
		/// </summary>
		/// <param name="in1"></param>
		/// <returns></returns>
		public static Dictionary<string, object> GetDictionary(/* NTFS_STATISTICS */ object inObj) {
			Dictionary<string, object> dict = new Dictionary<string, object>();

			Type objectType = inObj.GetType();
			FieldInfo[] fldList = objectType.GetFields();
			int cnt = fldList.Count();

			for (int idx = 0; idx < cnt; idx++) {
				FieldInfo fld = fldList[idx];
				object fldObj = fld.GetValue(inObj);

				if (fldObj != null) {
					Type t1 = fldObj.GetType();

					if (t1.IsNestedPublic) {
						Dictionary<string, object> dict2 = GetDictionary(fldObj);
						// dict = dict.Concat(dict2).ToDictionary(e => fld.Name + "." + e.Key, e => e.Value);
						foreach (string skey in dict2.Keys)
							dict.Add(fld.Name + "." + skey, dict2[skey]);
						// System.Diagnostics.Debug.WriteLine("dict Size = " + dict.Count.ToString());
					} else {
						dict.Add(fld.Name, fldObj);
					}
				}
			}

			return dict;
		}

		/// <summary>
		/// return merge (sum) of  values from identical keys.
		/// </summary>
		/// <param name="in1"></param>
		/// <param name="in2"></param>
		/// <returns></returns>
		public static Dictionary<string, object> Merge(Dictionary<string, object> in1, Dictionary<string, object> in2) {
			Dictionary<string, object> merged = new Dictionary<string, object>();

			foreach (string key in in1.Keys) {
				object obj1 = in1[key];
				object obj2 = in2[key];
				Type objectType1 = obj1.GetType();
				Type objectType2 = obj2.GetType();

				if (obj1 is UInt16) {
					UInt32 sum = (UInt16)obj1 + (UInt32)(UInt16)obj2;
					merged.Add(key, (UInt16)sum);
				} else if (obj1 is UInt32) {
					UInt32 sum = (UInt32)obj1 + (UInt32)obj2;
					merged.Add(key, sum);
				} else {
					// just pass obj1 to output list.
					merged.Add(key, obj1);
				}
			}

			return merged;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct NTFS_STATISTICS {
			UInt32 LogFileFullExceptions;
			UInt32 OtherExceptions;

			//
			// Other meta data io's
			//

			public UInt32 MftReads;
			public UInt32 MftReadBytes;
			public UInt32 MftWrites;
			public UInt32 MftWriteBytes;

			[StructLayout(LayoutKind.Sequential)]
			public struct MftWritesUserLevel {
				public UInt16 Write;
				public UInt16 Create;
				public UInt16 SetInfo;
				public UInt16 Flush;
			}
			public MftWritesUserLevel mftWritesUserLevel;

			public UInt16 MftWritesFlushForLogFileFull;
			public UInt16 MftWritesLazyWriter;
			public UInt16 MftWritesUserRequest;

			public UInt32 Mft2Writes;
			public UInt32 Mft2WriteBytes;

			public MftWritesUserLevel mft2WritesUserLevel;

			public UInt16 Mft2WritesFlushForLogFileFull;
			public UInt16 Mft2WritesLazyWriter;
			public UInt16 Mft2WritesUserRequest;

			public UInt32 RootIndexReads;
			public UInt32 RootIndexReadBytes;
			public UInt32 RootIndexWrites;
			public UInt32 RootIndexWriteBytes;

			public UInt32 BitmapReads;
			public UInt32 BitmapReadBytes;
			public UInt32 BitmapWrites;
			public UInt32 BitmapWriteBytes;

			public UInt16 BitmapWritesFlushForLogFileFull;
			public UInt16 BitmapWritesLazyWriter;
			public UInt16 BitmapWritesUserRequest;

			[StructLayout(LayoutKind.Sequential)]
			public struct BitmapWritesUserLevel {
				public UInt16 Write;
				public UInt16 Create;
				public UInt16 SetInfo;
			}
			public BitmapWritesUserLevel bitmapWritesUserLevel;

			public UInt32 MftBitmapReads;
			public UInt32 MftBitmapReadBytes;
			public UInt32 MftBitmapWrites;
			public UInt32 MftBitmapWriteBytes;

			public UInt16 MftBitmapWritesFlushForLogFileFull;
			public UInt16 MftBitmapWritesLazyWriter;
			public UInt16 MftBitmapWritesUserRequest;

			public MftWritesUserLevel mftBitmapWritesUserLevel;

			public UInt32 UserIndexReads;
			public UInt32 UserIndexReadBytes;
			public UInt32 UserIndexWrites;
			public UInt32 UserIndexWriteBytes;

			//
			// Additions for NT 5.0
			//

			public UInt32 LogFileReads;
			public UInt32 LogFileReadBytes;
			public UInt32 LogFileWrites;
			public UInt32 LogFileWriteBytes;

			[StructLayout(LayoutKind.Sequential)]
			public struct Allocate {
				public UInt32 Calls;                // number of individual calls to allocate clusters
				public UInt32 Clusters;             // number of clusters allocated
				public UInt32 Hints;                // number of times a hint was specified

				public UInt32 RunsReturned;         // number of runs used to satisify all the requests

				public UInt32 HintsHonored;         // number of times the hint was useful
				public UInt32 HintsClusters;        // number of clusters allocated via the hint
				public UInt32 Cache;                // number of times the cache was useful other than the hint
				public UInt32 CacheClusters;        // number of clusters allocated via the cache other than the hint
				public UInt32 CacheMiss;            // number of times the cache wasn't useful
				public UInt32 CacheMissClusters;    // number of clusters allocated without the cache
			}
			public Allocate allocate;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct FS_stats {
			public FILESYSTEM_STATISTICS fs;
			public NTFS_STATISTICS ntfs;
			// UInt32[12] filler;
			// DWORDLONG ForceSizeAndAlignment[32];  // pad to a multiple of 64 bytes
		};

		/// <summary>
		/// Return array of FS_stats  statistics for drive.
		/// </summary>
		/// <param name="drive"></param>
		/// <returns>FS_stats[]</returns>
		static public FS_stats[] GetFileSystemStatistics(string drive) {
			//   \\.\c:   or c:\
			string device = drive;
			IntPtr hDevice = IntPtr.Zero;
			IntPtr pAlloc = IntPtr.Zero;

			try {
				hDevice = CreateFile(device, FILE_READ_ATTRIBUTES, 7, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_BACKUP_SEMANTICS, IntPtr.Zero);

				FS_stats fsStats = new FS_stats();
				int fsStatsSize = Marshal.SizeOf(fsStats);
				int fsStatsSize64 = (fsStatsSize + 63) / 64 * 64;
				int fsStatsTotalSize = fsStatsSize64 * 12;

				pAlloc = Marshal.AllocHGlobal(fsStatsTotalSize);
				IntPtr pDest = pAlloc;
				uint returnSize = 0;

				int status = DeviceIoControl(
					hDevice,
					FSConstants.FSCTL_FILESYSTEM_GET_STATISTICS,
					IntPtr.Zero,
					0,
					pDest,
					(uint)fsStatsTotalSize,
					ref returnSize,
					IntPtr.Zero);

				int err = Marshal.GetLastWin32Error();

				if (status == 0) {
					throw new ErrorMsg.Win32Exception(err, status, device);
				}

				int numStats = (int)returnSize / fsStatsSize64;
				FS_stats[] fsStatList = new FS_stats[numStats];
				IntPtr fsPtr = new IntPtr(pAlloc.ToInt64());

				for (int fsIdx = 0; fsIdx < numStats; fsIdx++) {
					fsStatList[fsIdx] = (FS_stats)Marshal.PtrToStructure(fsPtr, typeof(FS_stats));
					fsPtr = new IntPtr(fsPtr.ToInt64() + fsStatsSize64);
				}

				return fsStatList;
			}
			finally {
				CloseHandle(hDevice);
				hDevice = IntPtr.Zero;

				Marshal.FreeHGlobal(pAlloc);
				pAlloc = IntPtr.Zero;
			}
		}

		#endregion


		#region ==== System Info
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern void GetSystemInfo(SYSTEM_INFO Info);

		[StructLayout(LayoutKind.Sequential)]
		public class SYSTEM_INFO {
			public Int16 ProcessorArchitecture;
			public Int16 Reserved;
			public Int32 PageSize;

			public IntPtr MinAppAddress;
			public IntPtr MaxAppAddress;
			public IntPtr ActiveProcMask;

			public Int32 NumberOfProcessor;
			public Int32 ProcessorType;
			public Int32 AllocGranulirity;

			public Int16 ProcessorLevel;
			public Int16 ProcessorRevision;
		}

		#endregion


		#region ==== Disk Cache Info
		public enum DISK_CACHE_RETENTION_PRIORITY {
			EqualPriority,
			KeepPrefetchedData,
			KeepReadData
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DISK_CACHE_INFORMATION {
			public byte ParametersSavable;
			public byte ReadCacheEnabled;
			public byte WriteCacheEnabled;
			// public byte filler1;
			public UInt32 /* DISK_CACHE_RETENTION_PRIORITY */ ReadRetentionPriority;
			public UInt32 /* DISK_CACHE_RETENTION_PRIORITY */ WriteRetentionPriority;
			public UInt16 DisablePrefetchTransferLength;
			public byte PrefetchScalar;
			// public byte filler2;

			[StructLayout(LayoutKind.Sequential)]
			public struct ScalarPrefetch {
				public UInt16 Minimum;
				public UInt16 Maximum;
				public UInt16 MaximumBlocks;
			}
			public ScalarPrefetch scalarPrefetch;
		}

		static public DISK_CACHE_INFORMATION GetDiskCacheInformation(int physicalDrive) {
			// \\.\PhysicalDrive0
			//   \\.\c:   or c:\
			string device = string.Format(@"\\.\PhysicalDrive{0}", physicalDrive);
			IntPtr hDevice = IntPtr.Zero;
			IntPtr pAlloc = IntPtr.Zero;

			try {
				hDevice = CreateFile(device, GENERIC_READ, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_BACKUP_SEMANTICS, IntPtr.Zero);

				DISK_CACHE_INFORMATION diskCacheInfo = new DISK_CACHE_INFORMATION();
				int cacheInfoSize = Marshal.SizeOf(diskCacheInfo);
				// int cacheInfoSize64 = (cacheInfoSize + 63) / 64 * 64;

				pAlloc = Marshal.AllocHGlobal(cacheInfoSize);
				IntPtr pDest = pAlloc;
				uint returnSize = 0;
				uint ioctl = FSConstants.IOCTL_DISK_GET_CACHE_INFORMATION;

				int status = DeviceIoControl(
					hDevice,
					FSConstants.IOCTL_DISK_GET_CACHE_INFORMATION,
					IntPtr.Zero,
					0,
					pDest,
					(uint)cacheInfoSize,
					ref returnSize,
					IntPtr.Zero);

				if (status == 0) {
					int err = Marshal.GetLastWin32Error();
					throw new ErrorMsg.Win32Exception(err, status, device);
				}

				diskCacheInfo = (DISK_CACHE_INFORMATION)Marshal.PtrToStructure(pAlloc, typeof(DISK_CACHE_INFORMATION));

				return diskCacheInfo;
			}
			finally {
				CloseHandle(hDevice);
				hDevice = IntPtr.Zero;

				Marshal.FreeHGlobal(pAlloc);
				pAlloc = IntPtr.Zero;
			}
		}
		#endregion

		#region ==== Disk Performance

		[StructLayout(LayoutKind.Sequential)]
		public unsafe struct DISK_PERFORMANCE {
			public Int64 BytesRead;
			public Int64 BytesWritten;
			public Int64 ReadTime;
			public Int64 WriteTime;
			public Int64 IdleTime;
			public UInt32 ReadCount;
			public UInt32 WriteCount;
			public UInt32 QueueDepth;
			public UInt32 SplitCount;
			public Int64 QueryTime;
			public UInt32 StorageDeviceNumber;
			public fixed char StorageManagerName[8];
		}

		static public DISK_PERFORMANCE GetDiskPerformance(int physicalDisk) {
			// \\.\PhysicalDrive0
			string device = string.Format(@"\\.\PhysicalDrive{0}", physicalDisk);
			return GetDevicePerformance(device);
		}

		static public DISK_PERFORMANCE GetDiskPerformance(string diskLetter) {
			//   \\.\c:    
			string device = string.Format(@"\\.\{0}", diskLetter.Split('\\')[0]);
			return GetDevicePerformance(device);
		}

		static private DISK_PERFORMANCE GetDevicePerformance(string device) {
			// Example Device:
			//   \\.\PhysicalDrive0
			//   \\.\c:    

			IntPtr hDevice = IntPtr.Zero;
			IntPtr pAlloc = IntPtr.Zero;

			try {
				hDevice = CreateFile(device, GENERIC_READ, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_BACKUP_SEMANTICS, IntPtr.Zero);

				DISK_PERFORMANCE diskPerformance = new DISK_PERFORMANCE();

				int diskPerfSize = Marshal.SizeOf(diskPerformance);
				// int cacheInfoSize64 = (cacheInfoSize + 63) / 64 * 64;

				pAlloc = Marshal.AllocHGlobal(diskPerfSize);
				IntPtr pDest = pAlloc;
				uint returnSize = 0;
				uint ioctl = FSConstants.IOCTL_DISK_GET_CACHE_INFORMATION;

				int status = DeviceIoControl(
					hDevice,
					FSConstants.IOCTL_DISK_PERFORMANCE,
					IntPtr.Zero,
					0,
					pDest,
					(uint)diskPerfSize,
					ref returnSize,
					IntPtr.Zero);

				int err = Marshal.GetLastWin32Error();

				if (status == 0) {
					throw new ErrorMsg.Win32Exception(err, status, device);
				}

				diskPerformance = (DISK_PERFORMANCE)Marshal.PtrToStructure(pAlloc, typeof(DISK_PERFORMANCE));

				return diskPerformance;
			}
			finally {
				CloseHandle(hDevice);
				hDevice = IntPtr.Zero;

				Marshal.FreeHGlobal(pAlloc);
				pAlloc = IntPtr.Zero;
			}
		}

		#endregion


		#region ==== Cluster Allocation Maps 

		/// <summary>
		/// Get cluster usage for a device
		/// </summary>
		/// <param name="DeviceName">use "c:"</param>
		/// <returns>a bitarray for each cluster</returns>
		static public BitArray GetVolumeMap(string DeviceName) {
			IntPtr pBitmap = IntPtr.Zero;
			IntPtr hDevice = IntPtr.Zero;

			try {
				hDevice = OpenVolume(DeviceName);

				Int64 startingLCN = 0;          // starting logical cluster number.
				uint SLCN_size = (uint)Marshal.SizeOf(startingLCN);

				GCHandle handle = GCHandle.Alloc(startingLCN, GCHandleType.Pinned);
				IntPtr pSLCN = handle.AddrOfPinnedObject();

				// Assumes max drive size is 2 TB.

				// alloc off more than enough for my machine
				// 64 megs == 67,108,864 bytes == 536,870,912 bits == cluster count
				// NTFS 4k clusters == 2,147,483,648 k of storage == 2,097,152 megs == 2048 gig disk storage
				uint bitmapSize = 1024 * 1024 * 64; // 1024 bytes == 1k * 1024 == 1 meg * 64 == 64 megs

				uint returnSize = 0;
				pBitmap = Marshal.AllocHGlobal((int)bitmapSize);
				IntPtr pDstBitmap = pBitmap;

				int status = DeviceIoControl(
					hDevice,
					FSConstants.FSCTL_GET_VOLUME_BITMAP,
					pSLCN,
					SLCN_size,
					pDstBitmap,
					bitmapSize,
					ref returnSize,
					IntPtr.Zero);

				if (status == 0) {
					int err = Marshal.GetLastWin32Error();
					throw new ErrorMsg.Win32Exception(err, status, DeviceName);
				}
				handle.Free();

				/*
                object returned was...
                  typedef struct 
                  {
                   LARGE_INTEGER StartingLcn;
                   LARGE_INTEGER BitmapSize;
                   BYTE Buffer[1];
                  } VOLUME_BITMAP_BUFFER, *PVOLUME_BITMAP_BUFFER;
                */
				Int64 StartingLcn = (Int64)Marshal.PtrToStructure(pDstBitmap, typeof(Int64));

				Debug.Assert(StartingLcn == 0);

				pDstBitmap = (IntPtr)((Int64)pDstBitmap + 8);
				Int64 BitmapSize = (Int64)Marshal.PtrToStructure(pDstBitmap, typeof(Int64));

				Int32 byteSize = (int)(BitmapSize / 8);
				byteSize++; // round up - even with no remainder

				IntPtr BitmapBegin = (IntPtr)((Int64)pDstBitmap + 8);

				byte[] byteArr = new byte[byteSize];

				Marshal.Copy(BitmapBegin, byteArr, 0, (Int32)byteSize);

				BitArray retVal = new BitArray(byteArr);
				retVal.Length = (int)BitmapSize; // truncate to exact cluster count
				return retVal;
			}
			finally {
				CloseHandle(hDevice);
				hDevice = IntPtr.Zero;

				Marshal.FreeHGlobal(pBitmap);
				pBitmap = IntPtr.Zero;
			}
		}

		/// <summary>
		/// Returns a 2*number of extents array, the vcn and the lcn as pairs.
		/// </summary>
		/// <param name="path">file to get the map for ex: "c:\windows\explorer.exe" </param>
		/// <returns>An array of [virtual cluster, physical cluster (-1=emtpy)]</returns>
		static public Array GetFileMap(string path) {
			IntPtr hFile = IntPtr.Zero;
			IntPtr pAlloc = IntPtr.Zero;

			try {
				hFile = OpenFile(path);

				Int64 startingLCN = 0;          // starting logical cluster number.
				uint SLCN_size = (uint)Marshal.SizeOf(startingLCN);

				GCHandle handle = GCHandle.Alloc(startingLCN, GCHandleType.Pinned);
				IntPtr pSLCN = handle.AddrOfPinnedObject();

				uint dstSize = 1024 * 1024 * 64; // 1024 bytes == 1k * 1024 == 1 meg * 64 == 64 megs

				uint returnSize = 0;
				pAlloc = Marshal.AllocHGlobal((int)dstSize);
				IntPtr pDest = pAlloc;
				int status = DeviceIoControl(
					hFile,
					FSConstants.FSCTL_GET_RETRIEVAL_POINTERS,
					pSLCN,
					SLCN_size,
					pDest,
					dstSize,
					ref returnSize,
					IntPtr.Zero);

				int err = Marshal.GetLastWin32Error();
				System.Diagnostics.Debug.WriteLine("File map err=" + err.ToString());

				if (status == 0) {
					throw new ErrorMsg.Win32Exception(err, status, path);
				}

				handle.Free();

				/*
                 returned back one of...
                 
                 typedef struct RETRIEVAL_POINTERS_BUFFER 
                 {  
                 UInt32 ExtentCount;  
                 Int64 StartingVcn;  
                    struct 
                    {
                       Int64 NextVcn;
                       Int64 Lcn;
                    } Extents[1];
                 }
                 
                */

				Int32 ExtentCount = (Int32)Marshal.PtrToStructure(pDest, typeof(Int32));

				pDest = (IntPtr)((Int64)pDest + 4);
				pDest = (IntPtr)((Int64)pDest + 4);   // pad to 8 byte boundary
				Int64 StartingVcn = (Int64)Marshal.PtrToStructure(pDest, typeof(Int64));
				Debug.Assert(StartingVcn == 0);

				pDest = (IntPtr)((Int64)pDest + 8);
				// now pDest points at an array of pairs of Int64s.
				Array retVal = Array.CreateInstance(typeof(Int64), new int[2] { ExtentCount, 2 });

				for (int i = 0; i < ExtentCount; i++) {
					for (int j = 0; j < 2; j++) {
						Int64 v = (Int64)Marshal.PtrToStructure(pDest, typeof(Int64));
						retVal.SetValue(v, new int[2] { i, j });
						pDest = (IntPtr)((Int64)pDest + 8);
					}
				}

				return retVal;
			}
			finally {
				CloseHandle(hFile);
				hFile = IntPtr.Zero;

				Marshal.FreeHGlobal(pAlloc);
				pAlloc = IntPtr.Zero;
			}
		}
		#endregion


		#region ===== Get Disk Geometry (not completed yet)
		[StructLayout(LayoutKind.Sequential)]
		internal class DISK_GEOMETRY {
			public long cylinders;
			public uint tracksPerCylinder;
			public uint sectorsPerTrack;
			public uint bytesPerSector;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal class DISK_GEOMETRY_EX {
			public DISK_GEOMETRY geometry = new DISK_GEOMETRY();
			public long DiskSize;
		}

		static public bool GetDriveGeometry(DISK_GEOMETRY_EX pdg) {
#if false
            uint junk = 51200; 
            byte[] buffer = new byte[512];

            bool result = DeviceIoControl(handle, IOCTL_GET_DRIVE_GEOMETRY_EX, IntPtr.Zero, (uint)0,
                pdg, (uint)buffer.Length, ref junk, IntPtr.Zero);

            return result;
#else
			return false;
#endif
		}

		static public string GetDriveGeom() {
			DISK_GEOMETRY_EX pdg2 = new DISK_GEOMETRY_EX();
			GetDriveGeometry(pdg2);

			string info = string.Empty;
			info += string.Format("  Cylinders = {0}", pdg2.geometry.cylinders);
			info += string.Format("  Tracks per cylinder = {0}", pdg2.geometry.tracksPerCylinder);
			info += string.Format("  Sectors per track = {0}", pdg2.geometry.sectorsPerTrack);
			info += string.Format("  Bytes per sector = {0}", pdg2.geometry.bytesPerSector);
			info += string.Format("  Disk size = {0}\n", pdg2.DiskSize);
			return info;
		}
		#endregion


		/// <summary>
		/// Constants lifted from winioctl.h from platform sdk
		/// </summary>
		internal class FSConstants {
			const uint FILE_DEVICE_FILE_SYSTEM = 0x00000009;

			const uint METHOD_NEITHER = 3;
			const uint METHOD_BUFFERED = 0;

			const uint FILE_ANY_ACCESS = 0;
			const uint FILE_SPECIAL_ACCESS = FILE_ANY_ACCESS;

			public static uint FSCTL_GET_VOLUME_BITMAP = CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 27, METHOD_NEITHER, FILE_ANY_ACCESS);
			public static uint FSCTL_GET_RETRIEVAL_POINTERS = CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 28, METHOD_NEITHER, FILE_ANY_ACCESS);
			public static uint FSCTL_MOVE_FILE = CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 29, METHOD_BUFFERED, FILE_SPECIAL_ACCESS);

			public const int FSCTL_FILESYSTEM_GET_STATISTICS = 0x00090060;
			public const int FSCTL_GET_NTFS_VOLUME_DATA = 0x00090064;
			public const int FSCTL_GET_NTFS_FILE_RECORD = 0x00090068;
			// public const int FSCTL_GET_VOLUME_BITMAP         = 0x0009006f;
			// public const int FSCTL_GET_RETRIEVAL_POINTERS    = 0x00090073;


			const uint FILE_DEVICE_MASS_STORAGE = 45;
			const uint IOCTL_STORAGE_BASE = FILE_DEVICE_MASS_STORAGE;

			const uint FILE_DEVICE_DISK = 7;
			const uint FILE_DEVICE_DISK_FILE_SYSTEM = 8;
			const uint IOCTL_DISK_BASE = FILE_DEVICE_DISK;

			const uint FILE_READ_ACCESS = 0x00000001;
			const uint FILE_WRITE_ACCESS = 0x00000002;

			public static uint IOCTL_DISK_GET_DRIVE_GEOMETRY = CTL_CODE(IOCTL_DISK_BASE, 0, METHOD_BUFFERED, FILE_ANY_ACCESS);
			public static uint IOCTL_DISK_GET_PARTITION_INFO = CTL_CODE(IOCTL_DISK_BASE, 1, METHOD_BUFFERED, FILE_READ_ACCESS);
			public static uint IOCTL_DISK_SET_PARTITION_INFO = CTL_CODE(IOCTL_DISK_BASE, 2, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
			public static uint IOCTL_DISK_GET_DRIVE_LAYOUT = CTL_CODE(IOCTL_DISK_BASE, 3, METHOD_BUFFERED, FILE_READ_ACCESS);
			public static uint IOCTL_DISK_SET_DRIVE_LAYOUT = CTL_CODE(IOCTL_DISK_BASE, 4, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
			public static uint IOCTL_DISK_VERIFY = CTL_CODE(IOCTL_DISK_BASE, 5, METHOD_BUFFERED, FILE_ANY_ACCESS);
			public static uint IOCTL_DISK_FORMAT_TRACKS = CTL_CODE(IOCTL_DISK_BASE, 6, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
			public static uint IOCTL_DISK_REASSIGN_BLOCKS = CTL_CODE(IOCTL_DISK_BASE, 7, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
			public static uint IOCTL_DISK_PERFORMANCE = CTL_CODE(IOCTL_DISK_BASE, 8, METHOD_BUFFERED, FILE_ANY_ACCESS);
			public static uint IOCTL_DISK_IS_WRITABLE = CTL_CODE(IOCTL_DISK_BASE, 9, METHOD_BUFFERED, FILE_ANY_ACCESS);
			public static uint IOCTL_DISK_LOGGING = CTL_CODE(IOCTL_DISK_BASE, 10, METHOD_BUFFERED, FILE_ANY_ACCESS);
			public static uint IOCTL_DISK_GET_PARTITION_INFO_EX = CTL_CODE(IOCTL_DISK_BASE, 0x12, METHOD_BUFFERED, FILE_ANY_ACCESS);
			public static uint IOCTL_DISK_SET_PARTITION_INFO_EX = CTL_CODE(IOCTL_DISK_BASE, 0x13, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
			public static uint IOCTL_DISK_GET_DRIVE_LAYOUT_EX = CTL_CODE(IOCTL_DISK_BASE, 0x14, METHOD_BUFFERED, FILE_ANY_ACCESS);
			public static uint IOCTL_DISK_SET_DRIVE_LAYOUT_EX = CTL_CODE(IOCTL_DISK_BASE, 0x15, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
			public static uint IOCTL_DISK_PERFORMANCE_OFF = CTL_CODE(IOCTL_DISK_BASE, 0x18, METHOD_BUFFERED, FILE_ANY_ACCESS);
			public static uint IOCTL_DISK_GET_DRIVE_GEOMETRY_EX = CTL_CODE(IOCTL_DISK_BASE, 0x28, METHOD_BUFFERED, FILE_ANY_ACCESS);
			public static uint IOCTL_DISK_GROW_PARTITION = CTL_CODE(IOCTL_DISK_BASE, 0x34, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);
			public static uint IOCTL_DISK_GET_CACHE_INFORMATION = CTL_CODE(IOCTL_DISK_BASE, 0x35, METHOD_BUFFERED, FILE_READ_ACCESS);
			public static uint IOCTL_DISK_SET_CACHE_INFORMATION = CTL_CODE(IOCTL_DISK_BASE, 0x36, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS);


			static uint CTL_CODE(uint DeviceType, uint Function, uint Method, uint Access) {
				return ((DeviceType) << 16) | ((Access) << 14) | ((Function) << 2) | (Method);
			}
		}


		#region ==== Get Application executable version string

		// Give this assembly a version number:
		// [assembly:AssemblyVersion("4.3.2.1")]

		[DllImport("version.dll")]
		public static extern bool GetFileVersionInfo(string sFileName,
			int handle, int size, byte[] infoBuffer);
		[DllImport("version.dll")]
		public static extern int GetFileVersionInfoSize(string sFileName,
			out int handle);

		// The third parameter - "out string pValue" - is automatically
		// marshaled from ANSI to Unicode:
		[DllImport("version.dll")]
		unsafe public static extern bool VerQueryValue(byte[] pBlock,
			string pSubBlock, out string pValue, out uint len);
		// This VerQueryValue overload is marked with 'unsafe' because 
		// it uses a short*:
		[DllImport("version.dll")]
		unsafe public static extern bool VerQueryValue(byte[] pBlock,
			string pSubBlock, out short* pValue, out uint len);

		// Main is marked with 'unsafe' because it uses pointers:
		unsafe public static bool GetExeVersion(string exePath, out string desc) {
			desc = string.Empty;
			try {
				int handle = 0;
				// Figure out how much version info there is:
				int size = GetFileVersionInfoSize(exePath, out handle);

				if (size == 0)
					return false;

				byte[] buffer = new byte[size];

				if (!GetFileVersionInfo(exePath, handle, size, buffer)) {
					desc = "Failed to query file version information.";
					return false;
				}

				short* subBlock = null;
				uint len = 0;
				// Get the locale info from the version info:
				if (!VerQueryValue(buffer, @"\VarFileInfo\Translation", out subBlock, out len)) {
					desc = "Failed to query version information.";
					return false;
				}

				string spv = @"\StringFileInfo\" + subBlock[0].ToString("X4") + subBlock[1].ToString("X4") + @"\ProductVersion";

				byte* pVersion = null;
				// Get the ProductVersion value for this program:
				string versionInfo;

				if (!VerQueryValue(buffer, spv, out versionInfo, out len)) {
					desc = "Failed to query version information.";
					return false;
				}

				desc = versionInfo;
				return true;
			} catch (Exception e) {
				desc = "Caught unexpected exception " + e.Message;
			}

			return false;
		}
		#endregion


		#region ===== System file list

		static public string[] NtfsSystemFiles {
			get {
				return new string[]
				{
					@"\$AttrDef",
					@"\$BadClus",
                    // @"\$BadClus:$Bad",
                    @"\$BitMap",
					@"\$Boot",
					@"\$LogFile",
					@"\$Mft",
					@"\$MftMirr",
					@"\$Secure",
					@"\$UpCase",
					@"\$Volume",
					@"\$Extend",
					@"\$Extend\$Reparse",
					@"\$Extend\$ObjId",
					@"\$Extend\$UsnJrnl",
                    // @"\$Extend\$UsnJrnl:$Max",
                    @"\$Extend\$Quota"
				};
			}
		}

		static public string[] ConfigSystemFiles {
			get {
				return new string[]
				{
					 @"c:\windows\system32\config\default",
					 @"c:\windows\system32\config\SAM",
					 @"c:\windows\system32\config\security",
					 @"c:\windows\system32\config\software",
					 @"c:\windows\system32\config\system"
				};
			}
		}

		static public string[] RegistryFiles {
			/*     
            doc\<user>\Local Settings\Application Data\Microsoft\Windows\UsrClass.dat
            doc\<user>\NtUser.dat
            doc\LocalService\Local Settings\Application Data\Microsoft\Windows\UsrClass.dat
            doc\LocalService\NtUser.dat
            doc\NetworkService\Local Settings\Application Data\Microsoft\Windows\UsrClass.dat
            doc\NetworkService\NtUser.dat
            */
			get {
				string appData = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				string cappData = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

				string lappData = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				string myDoc = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				string docUser = myDoc.Remove(myDoc.LastIndexOf('\\'));
				string doc = myDoc.Remove(docUser.LastIndexOf('\\'));

				string[] regFiles = new string[6];
				regFiles[0] = lappData + @"\Microsoft\Windows\UsrClass.dat";
				regFiles[1] = docUser + @"\NtUser.dat";

				regFiles[2] =
				   doc +
				   @"\LocalService" +
				   @"\Local Settings\Application Data\Microsoft\Windows\UsrClass.dat";
				regFiles[3] =
					doc +
					@"\LocalService" +
					@"\NtUser.dat";

				regFiles[4] =
					doc +
					@"\NetworkService" +
					@"\Local Settings\Application Data\Microsoft\Windows\UsrClass.dat";
				regFiles[5] =
					doc +
					@"\NetworkService" +
					@"\NtUser.dat";

				return regFiles;

			}
		}

		#endregion

	}
}
