using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace MagniFile {
	/// Utility functions to access network information.
	/// 
	/// Original code lifted from ProcessHacker
	/// http://processhacker.sourceforge.net/
	/// 
	/// Modified by: Dennis Lang - 2009
	/// https://landenlabs.com/
	/// 
	public static class Utils {
		/// <summary>
		/// Swaps the order of the bytes.
		/// </summary>
		/// <param name="v">The number to change.</param>
		/// <returns>A number.</returns>
		public static ushort Reverse(this ushort v) {
			byte b1 = (byte)v;
			byte b2 = (byte)(v >> 8);

			return (ushort)(b2 | (b1 << 8));
		}
	}

	class Win32 {
		/// <summary>
		/// Memory unmanaged memory management.
		/// </summary>
		public class MemoryAlloc : IDisposable {
			IntPtr _memory;
			int _length;

			/// <summary>
			/// Whether the object has been freed.
			/// </summary>
			private volatile bool _disposed = false;

			public MemoryAlloc(int length) {
				// _memory = Marshal.AllocHGlobal(length);
				_memory = Marshal.AllocCoTaskMem(length);
				_length = length;
			}

			~MemoryAlloc() {
				Free();
				Dispose();
			}

			/// <summary>
			/// Ensures that the GC does not own the object.
			/// </summary>
			public void Dispose() {
				_disposed = true;
				// this.Dispose();
			}

			protected void Free() {
				if (_disposed == false && _length != 0) {
					// Marshal.FreeHGlobal(_memory);
					Marshal.FreeCoTaskMem(_memory);
					_memory = (IntPtr)null;
					_length = 0;
				}
			}

			public void Resize(int length) {
				_memory = Marshal.ReAllocHGlobal(_memory, new IntPtr(length));
				_length = length;
			}

			public int Size {
				get { return _length; }
			}

			public static implicit operator IntPtr(MemoryAlloc mem) {
				return mem._memory;
			}

			public unsafe static implicit operator void*(MemoryAlloc mem) {
				return mem._memory.ToPointer();
			}

			public IntPtr Increment(int offset) {
				return new IntPtr(_memory.ToInt64() + offset);
			}

			public int ReadInt32(int offset) {
				return this.ReadInt32(offset, 0);
			}

			public int ReadInt32(int offset, int index) {
				return Marshal.ReadInt32(_memory, offset + index * sizeof(int));
			}

			public IntPtr ReadIntPtr(int offset) {
				return this.ReadIntPtr(offset, 0);
			}

			public IntPtr ReadIntPtr(int offset, int index) {
				return Marshal.ReadIntPtr(_memory, offset + index * IntPtr.Size);
			}

			public uint ReadUInt32(int offset) {
				return this.ReadUInt32(offset, 0);
			}

			public uint ReadUInt32(int offset, int index) {
				return (uint)this.ReadInt32(offset, index);
			}

			public T ReadStruct<T>()
				where T : struct {
				return this.ReadStruct<T>(0);
			}

			public T ReadStruct<T>(int index)
				where T : struct {
				return this.ReadStruct<T>(0, index);
			}

			public T ReadStruct<T>(int offset, int index)
				where T : struct {
				return (T)Marshal.PtrToStructure(
					Increment(offset + Marshal.SizeOf(typeof(T)) * index), typeof(T));
			}

			public string ReadUnicodeString(int offset) {
				return Marshal.PtrToStringUni(Increment(offset));
			}

			public string ReadUnicodeString(int offset, int length) {
				return Marshal.PtrToStringUni(Increment(offset), length);
			}
		}

		public enum SystemInformationClass : int {
			SystemBasicInformation,
			SystemProcessorInformation,
			SystemPerformanceInformation,
			SystemTimeOfDayInformation,
			SystemPathInformation,
			SystemProcessInformation,
			SystemCallCountInformation,
			SystemDeviceInformation,
			SystemProcessorPerformanceInformation,
			SystemFlagsInformation,
			SystemCallTimeInformation, // 10
			SystemModuleInformation,
			SystemLocksInformation,
			SystemStackTraceInformation,
			SystemPagedPoolInformation,
			SystemNonPagedPoolInformation,
			SystemHandleInformation,
			SystemObjectInformation,
			SystemPageFileInformation,
			SystemVdmInstemulInformation,
			SystemVdmBopInformation, // 20
			SystemFileCacheInformation,
			SystemPoolTagInformation,
			SystemInterruptInformation,
			SystemDpcBehaviorInformation,
			SystemFullMemoryInformation,
			SystemLoadGdiDriverInformation,
			SystemUnloadGdiDriverInformation,
			SystemTimeAdjustmentInformation,
			SystemSummaryMemoryInformation,
			SystemMirrorMemoryInformation, // 30
			SystemPerformanceTraceInformation,
			SystemCrashDumpInformation,
			SystemExceptionInformation,
			SystemCrashDumpStateInformation,
			SystemKernelDebuggerInformation,
			SystemContextSwitchInformation,
			SystemRegistryQuotaInformation,
			SystemExtendServiceTableInformation, // used to be SystemLoadAndCallImage
			SystemPrioritySeparation,
			SystemVerifierAddDriverInformation, // 40
			SystemVerifierRemoveDriverInformation,
			SystemProcessorIdleInformation,
			SystemLegacyDriverInformation,
			SystemCurrentTimeZoneInformation,
			SystemLookasideInformation,
			SystemTimeSlipNotification,
			SystemSessionCreate,
			SystemSessionDetach,
			SystemSessionInformation,
			SystemRangeStartInformation, // 50
			SystemVerifierInformation,
			SystemVerifierThunkExtend,
			SystemSessionProcessInformation,
			SystemLoadGdiDriverInSystemSpace,
			SystemNumaProcessorMap,
			SystemPrefetcherInformation,
			SystemExtendedProcessInformation,
			SystemRecommendedSharedDataAlignment,
			SystemComPlusPackage,
			SystemNumaAvailableMemory, // 60
			SystemProcessorPowerInformation,
			SystemEmulationBasicInformation,
			SystemEmulationProcessorInformation,
			SystemExtendedHandleInformation,
			SystemLostDelayedWriteInformation,
			SystemBigPoolInformation,
			SystemSessionPoolTagInformation,
			SystemSessionMappedViewInformation,
			SystemHotpatchInformation,
			SystemObjectSecurityMode, // 70
			SystemWatchdogTimerHandler, // doesn't seem to be implemented
			SystemWatchdogTimerInformation,
			SystemLogicalProcessorInformation,
			SystemWow64SharedInformation,
			SystemRegisterFirmwareTableInformationHandler,
			SystemFirmwareTableInformation,
			SystemModuleInformationEx,
			SystemVerifierTriageInformation,
			SystemSuperfetchInformation,
			SystemMemoryListInformation, // 80
			SystemFileCacheInformationEx,
			SystemNotImplemented19,
			SystemProcessorDebugInformation,
			SystemVerifierInformation2,
			SystemNotImplemented20,
			SystemRefTraceInformation,
			SystemSpecialPoolTag, // MmSpecialPoolTag, then MmSpecialPoolCatchOverruns != 0
			SystemProcessImageName,
			SystemNotImplemented21,
			SystemBootEnvironmentInformation, // 90
			SystemEnlightenmentInformation,
			SystemVerifierInformationEx,
			SystemNotImplemented22,
			SystemNotImplemented23,
			SystemCovInformation,
			SystemNotImplemented24,
			SystemNotImplemented25,
			SystemPartitionInformation,
			SystemSystemDiskInformation, // this and SystemPartitionInformation both call IoQuerySystemDeviceName
			SystemPerformanceDistributionInformation, // 100
			SystemNumaProximityNodeInformation,
			SystemTimeZoneInformation2,
			SystemCodeIntegrityInformation,
			SystemNotImplemented26,
			SystemUnknownInformation, // No symbols for this case, very strange...
			SystemVaInformation // 106, calls MmQuerySystemVaInformation
		}

		[DllImport("ntdll.dll")]
		public static extern uint NtQuerySystemInformation(
			[In] SystemInformationClass SystemInformationClass,
			IntPtr SystemInformation,
			[In] int SystemInformationLength,
			[Out][Optional] out int ReturnLength
			);

		[StructLayout(LayoutKind.Explicit)]
		public struct INet6Address {
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			[FieldOffset(0)]
			public byte[] Bytes;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			[FieldOffset(0)]
			public ushort[] Words;
		}

		#region TCP DLLimport

		[DllImport("iphlpapi.dll", SetLastError = true)]
		public extern static int SetTcpEntry(
			[In] ref MibTcpRow TcpRow
			);

		[DllImport("iphlpapi.dll", SetLastError = true)]
		public extern static int GetExtendedTcpTable(
			[Out] IntPtr Table,
			ref int Size,
			[In] bool Order,
			[In] AiFamily IpVersion,
			[In] TcpTableClass TableClass,
			[In] int Reserved
			);

		[DllImport("iphlpapi.dll", SetLastError = true)]
		public extern static int GetTcpStatistics(
			[Out] out MibTcpStats pStats
			);

		[DllImport("iphlpapi.dll", SetLastError = true)]
		public static extern int GetTcpTable(
			[Out] byte[] tcpTable,
			ref int pdwSize,
			[In] bool bOrder
			);

		[DllImport("iphlpapi.dll", SetLastError = true)]
		public static extern int GetTcp6Table(
			[Out] byte[] tcpTable,
			ref int pdwSize,
			[In] bool bOrder);

		[DllImport("iphlpapi.dll", SetLastError = true)]
		public extern static int AllocateAndGetTcpExTableFromStack(
			[Out] out IntPtr pTable,
			[In] bool bOrder,
			[In] IntPtr heap,
			[In] int flags,
			[In] int family
			);

		#endregion

		#region TCP Struct

		[StructLayout(LayoutKind.Sequential)]
		public struct MibTcpRow {
			public MibTcpState State;
			public uint LocalAddress;
			public int LocalPort;
			public uint RemoteAddress;
			public int RemotePort;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MibTcp6Row {
			public MibTcpState State;
			public uint LocalAddress;
			public uint LocalScopeId;
			public int LocalPort;
			public uint RemoteAddr;
			public int RemoteScopeId;
			public int RemotePort;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MibTcp6Row2 {
			public INet6Address LocalAddr;
			public uint LocalScopeId;
			public int LocalPort;
			public INet6Address RemoteAddr;
			public uint RemoteScopeId;
			public int RemotePort;
			public MibTcpState State;
			public int OwningPid;
			public TcpConnectionOffloadState OffloadState;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MibTcpRowOwnerPid {
			public MibTcpState State;
			public uint LocalAddress;
			public int LocalPort;
			public uint RemoteAddress;
			public int RemotePort;
			public int OwningProcessId;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct MibTcp6RowOwnerPid {
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public byte[] LocalAddress;
			public uint LocalScopeId;
			public int LocalPort;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public byte[] RemoteAddress;
			public uint RemoteScopeId;
			public int RemotePort;
			public MibTcpState State;
			public int OwningProcessId;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MibTcpStats {
			public uint RtoAlgorithm;
			public uint RtoMin;
			public uint RtoMax;
			public uint MaxConn;
			public uint ActiveOpens;
			public uint PassiveOpens;
			public uint AttemptFails;
			public uint EstabResets;
			public uint CurrEstab;
			public uint InSegs;
			public uint OutSegs;
			public uint RetransSegs;
			public uint InErrs;
			public uint OutRsts;
			public uint NumConns;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MibTcpTable {
			public int NumEntries;
			public MibTcpRow[] Table;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MibTcp6Table {
			public int NumEntries;
			public MibTcp6Row[] Table;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MibTcpTableOwnerPid {
			public int NumEntries;
			public MibTcpRowOwnerPid[] Table;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MibTcp6TableOwnerPid {
			public int NumEntries;
			public MibTcp6RowOwnerPid[] Table;
		}

		#endregion

		#region TCP enum

		public enum AiFamily : int {
			/// <summary>
			/// The address family is unspecified.
			/// </summary>
			Unspecified = 0,
			/// <summary>
			/// The Internet Protocol version 4 (IPv4) address family.
			/// </summary>
			INet = 2,
			/// <summary>
			/// The NetBIOS address family. This address family is only supported 
			/// if a Windows Sockets provider for NetBIOS is installed.
			/// </summary>
			NetBios = 17,
			/// <summary>
			/// The Internet Protocol version 6 (IPv6) address family.
			/// </summary>
			INet6 = 23,
			/// <summary>
			/// The Infrared Data Association (IrDA) address family. This address 
			/// family is only supported if the computer has an infrared port and 
			/// driver installed.
			/// </summary>
			IrDA = 26,
			/// <summary>
			/// The Bluetooth address family. This address family is only supported 
			/// if a Bluetooth adapter is installed on Windows Server 2003 or later.
			/// </summary>
			Bth = 32
		}

		public enum TcpConnectionOffloadState {
			InHost = 0,
			Offloading = 1,
			Offloaded = 2,
			Uploading = 3,
			Max = 4
		}

		public enum TcpTableClass : int {
			BasicListener,
			BasicConnections,
			BasicAll,
			OwnerPidListener,
			OwnerPidConnections,
			OwnerPidAll,
			OwnerModuleListener,
			OwnerModuleConnections,
			OwnerModuleAll
		}

		public enum MibTcpState : int {
			Closed = 1,
			Listening = 2,
			SynSent = 3,
			SynReceived = 4,
			Established = 5,
			FinWait1 = 6,
			FinWait2 = 7,
			CloseWait = 8,
			Closing = 9,
			LastAck = 10,
			TimeWait = 11,
			DeleteTcb = 12
		}

		#endregion

		#region TCP functions

		public static MibTcpStats GetTcpStats() {
			MibTcpStats tcpStats;
			GetTcpStatistics(out tcpStats);
			return tcpStats;
		}

		public static MibTcpTableOwnerPid GetTcpTable() {
			MibTcpTableOwnerPid table = new MibTcpTableOwnerPid();
			int length = 0;

			GetExtendedTcpTable(IntPtr.Zero, ref length, false, AiFamily.INet, TcpTableClass.OwnerPidAll, 0);

			using (MemoryAlloc mem = new MemoryAlloc(length)) {
				GetExtendedTcpTable(mem, ref length, false, AiFamily.INet, TcpTableClass.OwnerPidAll, 0);

				int count = mem.ReadInt32(0);

				table.NumEntries = count;
				table.Table = new MibTcpRowOwnerPid[count];

				for (int i = 0; i < count; i++)
					table.Table[i] = mem.ReadStruct<MibTcpRowOwnerPid>(sizeof(int), i);
			}

			return table;
		}

		#endregion




		#region UDP DLLimport

		[DllImport("iphlpapi.dll", SetLastError = true)]
		public extern static int GetExtendedUdpTable(
			[Out] IntPtr Table,
			ref int Size,
			[In] bool Order,
			[In] AiFamily IpVersion,
			[In] UdpTableClass TableClass,
			[In] int Reserved
			);

		[DllImport("iphlpapi.dll", SetLastError = true)]
		public static extern int GetUdpStatistics(
			[Out] out MibUdpStats pStats
			);

		[DllImport("iphlpapi.dll", SetLastError = true)]
		public static extern int GetUdpTable(
			[Out] byte[] udpTable,
			ref int pdwSize,
			[In] bool bOrder
			);

		[DllImport("iphlpapi.dll", SetLastError = true)]
		public extern static int AllocateAndGetUdpExTableFromStack(
			[Out] out IntPtr pTable,
			[In] bool bOrder,
			[In] IntPtr heap,
			[In] int flags,
			[In] int family
			);

		#endregion

		#region UDP Struct

		[StructLayout(LayoutKind.Sequential)]
		public struct MibUdpRow {
			public uint LocalAddress;
			public int LocalPort;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MibUdp6Row {
			public INet6Address LocalAddress;
			public uint LocalScopeId;
			public int LocalPort;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MibUdpTable {
			public uint NumEntries;
			public MibUdpRow[] Table;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MibUdp6Table {
			public int NumEntries;
			public MibUdp6Row[] Table;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MibUdpRowOwnerPid {
			public uint LocalAddress;
			public int LocalPort;
			public int OwningProcessId;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct MibUdp6RowOwnerPid {
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public byte[] LocalAddress;
			public uint LocalScopeId;
			public int LocalPort;
			public int OwningProcessId;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct MibUdp6RowOwnerModule {
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public byte[] LocalAddress;
			public uint LocalScopeId;
			public int LocalPort;
			public int OwningProcessId;
			public long CreateTimestamp;
			public int Flags;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public long[] OwningModuleInfo;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MibUdpStats {
			public int InDatagrams;
			public int NoPorts;
			public int InErrors;
			public int OutDatagrams;
			public int NumAddrs;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MibUdpTableOwnerPid {
			public int NumEntries;
			public MibUdpRowOwnerPid[] Table;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MibUdp6TableOwnerPid {
			public int NumEntries;
			public MibUdp6RowOwnerPid[] Table;
		}

		#endregion

		#region UDP enum

		public enum UdpTableClass : int {
			Basic,
			OwnerPid,
			OwnerModule
		}

		#endregion

		#region UDP functions

		public static MibUdpStats GetUdpStats() {
			MibUdpStats udpStats;
			GetUdpStatistics(out udpStats);
			return udpStats;
		}

		public static MibUdpTableOwnerPid GetUdpTable() {
			MibUdpTableOwnerPid table = new MibUdpTableOwnerPid();
			int length = 0;

			GetExtendedUdpTable(IntPtr.Zero, ref length, false, AiFamily.INet, UdpTableClass.OwnerPid, 0);

			using (MemoryAlloc mem = new MemoryAlloc(length)) {
				GetExtendedUdpTable(mem, ref length, false, AiFamily.INet, UdpTableClass.OwnerPid, 0);

				int count = mem.ReadInt32(0);

				table.NumEntries = count;
				table.Table = new MibUdpRowOwnerPid[count];

				for (int i = 0; i < count; i++)
					table.Table[i] = mem.ReadStruct<MibUdpRowOwnerPid>(sizeof(int), i);
			}

			return table;
		}


		#region Errors

		public static int GetLastErrorCode() {
			return (int)Marshal.GetLastWin32Error();
		}

		/// <summary>
		/// Gets the error message associated with the last error that occured.
		/// </summary>
		/// <returns>An error message.</returns>
		public static string GetLastErrorMessage() {
			return GetLastErrorCode().ToString();
		}

		/// <summary>
		/// Throws a WindowsException with the last error that occurred.
		/// </summary>
		public static void ThrowLastError() {
			// ThrowLastError(GetLastErrorCode());
		}


		#endregion


		public enum NetworkProtocol {
			Tcp,
			Udp,
			Tcp6,
			Udp6
		}

		public struct NetworkConnection {
			public int Pid;
			public NetworkProtocol Protocol;
			public IPEndPoint Local;
			public IPEndPoint Remote;
			public MibTcpState State;
			public object Tag;

			public override int GetHashCode() {
				int hashcode = Pid;
				hashcode += (int)Protocol + hashcode * 11;
				hashcode += Local.GetHashCode() + hashcode * 11;
				if (Remote != null)
					hashcode += Remote.Address.GetHashCode() + hashcode * 11;

				//return base.GetHashCode();
				return hashcode;
			}

			public void CloseTcpConnection() {
				MibTcpRow row = new MibTcpRow() {
					State = MibTcpState.DeleteTcb,
					LocalAddress = (uint)this.Local.Address.Address,
					LocalPort = ((ushort)this.Local.Port).Reverse(),
					RemoteAddress = this.Remote != null ? (uint)this.Remote.Address.Address : 0,
					RemotePort = this.Remote != null ? ((ushort)this.Remote.Port).Reverse() : 0
				};
				int result = Win32.SetTcpEntry(ref row);

				// if (result != 0)
				//     Win32.ThrowLastError(result);
			}
		}


		/// <summary>
		/// Gets the network connections currently active.
		/// </summary>
		/// <returns>A dictionary of network connections.</returns>
		public static Dictionary<int, List<NetworkConnection>> GetNetworkConnections() {
			var retDict = new Dictionary<int, List<NetworkConnection>>();
			int length;

			// TCP IPv4

			length = 0;
			Win32.GetExtendedTcpTable(IntPtr.Zero, ref length, false, AiFamily.INet, TcpTableClass.OwnerPidAll, 0);

			using (var mem = new MemoryAlloc(length)) {
				if (Win32.GetExtendedTcpTable(mem, ref length, false, AiFamily.INet, TcpTableClass.OwnerPidAll, 0) != 0)
					Win32.ThrowLastError();

				int count = mem.ReadInt32(0);

				for (int i = 0; i < count; i++) {
					var struc = mem.ReadStruct<MibTcpRowOwnerPid>(sizeof(int), i);

					if (!retDict.ContainsKey(struc.OwningProcessId))
						retDict.Add(struc.OwningProcessId, new List<NetworkConnection>());

					retDict[struc.OwningProcessId].Add(new NetworkConnection() {
						Protocol = NetworkProtocol.Tcp,
						Local = new IPEndPoint(struc.LocalAddress, ((ushort)struc.LocalPort).Reverse()),
						Remote = new IPEndPoint(struc.RemoteAddress, ((ushort)struc.RemotePort).Reverse()),
						State = struc.State,
						Pid = struc.OwningProcessId
					});
				}
			}

			// UDP IPv4

			length = 0;
			Win32.GetExtendedUdpTable(IntPtr.Zero, ref length, false, AiFamily.INet, UdpTableClass.OwnerPid, 0);

			using (var mem = new MemoryAlloc(length)) {
				if (Win32.GetExtendedUdpTable(mem, ref length, false, AiFamily.INet, UdpTableClass.OwnerPid, 0) != 0)
					Win32.ThrowLastError();

				int count = mem.ReadInt32(0);

				for (int i = 0; i < count; i++) {
					var struc = mem.ReadStruct<MibUdpRowOwnerPid>(sizeof(int), i);

					if (!retDict.ContainsKey(struc.OwningProcessId))
						retDict.Add(struc.OwningProcessId, new List<NetworkConnection>());

					retDict[struc.OwningProcessId].Add(
						new NetworkConnection() {
							Protocol = NetworkProtocol.Udp,
							Local = new IPEndPoint(struc.LocalAddress, ((ushort)struc.LocalPort).Reverse()),
							Pid = struc.OwningProcessId
						});
				}
			}

			// TCP IPv6

			length = 0;
			Win32.GetExtendedTcpTable(IntPtr.Zero, ref length, false, AiFamily.INet6, TcpTableClass.OwnerPidAll, 0);

			using (var mem = new MemoryAlloc(length)) {
				if (Win32.GetExtendedTcpTable(mem, ref length, false, AiFamily.INet6, TcpTableClass.OwnerPidAll, 0) == 0) {
					int count = mem.ReadInt32(0);

					for (int i = 0; i < count; i++) {
						var struc = mem.ReadStruct<MibTcp6RowOwnerPid>(sizeof(int), i);

						if (!retDict.ContainsKey(struc.OwningProcessId))
							retDict.Add(struc.OwningProcessId, new List<NetworkConnection>());

						retDict[struc.OwningProcessId].Add(new NetworkConnection() {
							Protocol = NetworkProtocol.Tcp6,
							Local = new IPEndPoint(new IPAddress(struc.LocalAddress, struc.LocalScopeId), ((ushort)struc.LocalPort).Reverse()),
							Remote = new IPEndPoint(new IPAddress(struc.RemoteAddress, struc.RemoteScopeId), ((ushort)struc.RemotePort).Reverse()),
							State = struc.State,
							Pid = struc.OwningProcessId
						});
					}
				}
			}

			// UDP IPv6

			length = 0;
			Win32.GetExtendedUdpTable(IntPtr.Zero, ref length, false, AiFamily.INet6, UdpTableClass.OwnerPid, 0);

			using (var mem = new MemoryAlloc(length)) {
				if (Win32.GetExtendedUdpTable(mem, ref length, false, AiFamily.INet6, UdpTableClass.OwnerPid, 0) == 0) {
					int count = mem.ReadInt32(0);

					for (int i = 0; i < count; i++) {
						var struc = mem.ReadStruct<MibUdp6RowOwnerPid>(sizeof(int), i);

						if (!retDict.ContainsKey(struc.OwningProcessId))
							retDict.Add(struc.OwningProcessId, new List<NetworkConnection>());

						retDict[struc.OwningProcessId].Add(
							new NetworkConnection() {
								Protocol = NetworkProtocol.Udp6,
								Local = new IPEndPoint(new IPAddress(struc.LocalAddress, struc.LocalScopeId), ((ushort)struc.LocalPort).Reverse()),
								Pid = struc.OwningProcessId
							});
					}
				}
			}

			return retDict;
		}

		#endregion
	}

	/// <summary>
	/// Represents a network adapter installed on the machine.
	/// Properties of this class can be used to obtain current network speed.
	/// </summary>
	public class NetworkAdapter {
		//http://www.dotnet247.com/247reference/System/Net/NetworkInformation/System.Net.NetworkInformation.aspx
		//MibTcpStats plus others are locatated in NetworkInfomation class

		/// <summary>
		/// Instances of this class are supposed to be created only in an NetworkMonitor.
		/// </summary>
		internal NetworkAdapter(string name) {
			this.name = name;
		}

		private long dlSpeed, ulSpeed;              // Download\Upload speed in bytes per second.
		private long dlValue, ulValue;              // Download\Upload counter value in bytes.
		private long dlValueOld, ulValueOld;        // Download\Upload counter value one second earlier, in bytes.

		internal string name;                               // The name of the adapter.
		internal PerformanceCounter dlCounter, ulCounter;   // Performance counters to monitor download and upload speed.

		/// <summary>
		/// Preparations for monitoring.
		/// </summary>
		internal void init() {
			// Since dlValueOld and ulValueOld are used in method refresh() to calculate network speed, they must have be initialized.
			this.dlValueOld = this.dlCounter.NextSample().RawValue;
			this.ulValueOld = this.ulCounter.NextSample().RawValue;
		}

		/// <summary>
		/// Obtain new sample from performance counters, and refresh the values saved in dlSpeed, ulSpeed, etc.
		/// This method is supposed to be called only in NetworkMonitor, one time every second.
		/// </summary>
		internal void refresh() {
			this.dlValue = this.dlCounter.NextSample().RawValue;
			this.ulValue = this.ulCounter.NextSample().RawValue;

			// Calculates download and upload speed.
			this.dlSpeed = this.dlValue - this.dlValueOld;
			this.ulSpeed = this.ulValue - this.ulValueOld;

			this.dlValueOld = this.dlValue;
			this.ulValueOld = this.ulValue;
		}

		/// <summary>
		/// Overrides method to return the name of the adapter.
		/// </summary>
		/// <returns>The name of the adapter.</returns>
		public override string ToString() {
			return this.name;
		}

		/// <summary>
		/// The name of the network adapter.
		/// </summary>
		public string Name {
			get {
				return this.name;
			}
		}

		/// <summary>
		/// Current download speed in bytes per second.
		/// </summary>
		public long DownloadSpeed {
			get {
				return this.dlSpeed;
			}
		}

		/// <summary>
		/// Current upload speed in bytes per second.
		/// </summary>
		public long UploadSpeed {
			get {
				return this.ulSpeed;
			}
		}

		/// <summary>
		/// Current download speed in kbytes per second.
		/// </summary>
		public double DownloadSpeedKbps {
			get {
				return this.dlSpeed / 1024.0;
			}
		}

		/// <summary>
		/// Current upload speed in kbytes per second.
		/// </summary>
		public double UploadSpeedKbps {
			get {
				return this.ulSpeed / 1024.0;
			}
		}
	}

	public class NetworkInformation {
		private static NetworkInterface[] NIC;

		public NetworkInformation() {
			NIC = NetworkInterface.GetAllNetworkInterfaces();
		}

		public string BytesReceived(int index) {
			return NIC[index].GetIPv4Statistics().BytesReceived.ToString();
		}

		public string BytesSent(int index) {
			return NIC[index].GetIPv4Statistics().BytesSent.ToString();
		}

		public string IncomingPacketsDiscarded(int index) {
			return NIC[index].GetIPv4Statistics().IncomingPacketsDiscarded.ToString();
		}

		public string IncomingPacketsWithErrors(int index) {
			return NIC[index].GetIPv4Statistics().IncomingPacketsWithErrors.ToString();
		}

		public string Description(int index) {
			return NIC[index].Description;
		}

		public string Speed(int index) {
			return NIC[index].Speed.ToString();
		}


	}


	/// <summary>
	/// The NetworkMonitor class monitors network speed for each network adapter on the computer, using classes for Performance counter in .NET library.
	/// </summary>
	public class NetworkMonitor {
		private System.Timers.Timer timer;      // The timer event executes every second to refresh the values in adapters.
		private ArrayList adapters;             // The list of adapters on the computer.
		private ArrayList monitoredAdapters;    // The list of currently monitored adapters.

		/// <summary>
		/// NetworkMonitor
		/// </summary>
		public NetworkMonitor() {
			this.adapters = new ArrayList();
			this.monitoredAdapters = new ArrayList();
			this.EnumerateNetworkAdapters();

			this.timer = new System.Timers.Timer(1000);
			//            this.timer.Elapsed += new ElapsedEventHandler(this.timer_Elapsed);
		}

		/// <summary>
		/// Enumerates network adapters installed on the computer.
		/// </summary>
		private void EnumerateNetworkAdapters() {
			PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");

			foreach (string name in category.GetInstanceNames()) {
				// This one exists on every computer.
				if (name == "MS TCP Loopback interface") { continue; }
				// Create an instance of NetworkAdapter class, and create performance counters for it.
				NetworkAdapter adapter = new NetworkAdapter(name);
				adapter.dlCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", name);
				adapter.ulCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", name);
				this.adapters.Add(adapter);         // Add it to ArrayList adapter
			}
		}

#if false
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (NetworkAdapter adapter in this.monitoredAdapters)
            { adapter.refresh(); }
        }
#endif

		/// <summary>
		/// Get instances of NetworkAdapter for installed adapters on this computer.
		/// </summary>
		public NetworkAdapter[] Adapters {
			get {
				return (NetworkAdapter[])this.adapters.ToArray(typeof(NetworkAdapter));
			}
		}

		/// <summary>
		/// Enable the timer and add all adapters to the monitoredAdapters list, unless the adapters list is empty.
		/// </summary>
		public void StartMonitoring() {
			if (this.adapters.Count > 0) {
				foreach (NetworkAdapter adapter in this.adapters)
					if (!this.monitoredAdapters.Contains(adapter)) {
						this.monitoredAdapters.Add(adapter);
						adapter.init();
					}

				this.timer.Enabled = true;
			}
		}

		/// <summary>
		/// Enable the timer, and add the specified adapter to the monitoredAdapters list
		/// </summary>
		/// <param name="adapter"></param>
		public void StartMonitoring(NetworkAdapter adapter) {
			if (!this.monitoredAdapters.Contains(adapter)) {
				this.monitoredAdapters.Add(adapter);
				adapter.init();
			}
			this.timer.Enabled = true;
		}

		/// <summary>
		/// Disable the timer, and clear the monitoredAdapters list.
		/// </summary>
		public void StopMonitoring() {
			this.monitoredAdapters.Clear();
			this.timer.Enabled = false;
		}

		/// <summary>
		/// Remove the specified adapter from the monitoredAdapters list, and disable the timer if the monitoredAdapters list is empty.
		/// </summary>
		/// <param name="adapter"></param>
		public void StopMonitoring(NetworkAdapter adapter) {
			if (this.monitoredAdapters.Contains(adapter)) { this.monitoredAdapters.Remove(adapter); }
			if (this.monitoredAdapters.Count == 0) { this.timer.Enabled = false; }
		}
	}
}
