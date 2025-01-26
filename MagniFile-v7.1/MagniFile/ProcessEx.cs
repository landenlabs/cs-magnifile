using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

namespace MagniFile
{
    /// <summary>
    /// Access to full 'undocumented' Process List parameters, such as i/o.
    /// 
    /// Original code from ProcessHacker project.
    /// http://processhacker.sourceforge.net/
    /// </summary>
    class ProcessEx
    {
        const uint NtStatus_INFO_LENGTH_MISMATCH = 0xc0000004;
        const uint NtStatus_Success = 0;

        #region ===== Interrupt
        [StructLayout(LayoutKind.Sequential)]
        public struct SystemInterruptInformation
        {
            public ulong ContextSwitches;
            public ulong DpcCount;
            public ulong DpcRate;
            public ulong TimeIncrement;
            public ulong DpcBypassCount;
            public ulong ApcBypassCount;
        }
 
        public static SystemInterruptInformation GetInterrupt()
        {
            int retLength;
            SystemInterruptInformation returnInterrupt = new SystemInterruptInformation(); 
            int size = Marshal.SizeOf(returnInterrupt);

            Win32.MemoryAlloc data = new Win32.MemoryAlloc(size);

            uint status;

            status = Win32.NtQuerySystemInformation(
                Win32.SystemInformationClass.SystemInterruptInformation,
                data,
                data.Size,
                out retLength);

            if (status != NtStatus_Success)
            {
                uint err = (uint)Marshal.GetLastWin32Error();
                return returnInterrupt;
            }

            returnInterrupt = data.ReadStruct<SystemInterruptInformation>(0, 0);
            return returnInterrupt;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SystemDpcBehaviorInformation
        {
            public UInt32 Spare1;
            public UInt32 DpcQueueDepth;
            public UInt32 MinimumDpcRate;
            public UInt32 AdjustDpcThreshold;
            public UInt32 IdealDpcRate;
        }

        public static SystemDpcBehaviorInformation GetDpc()
        {
            int retLength;
            SystemDpcBehaviorInformation returnDpc = new SystemDpcBehaviorInformation();
            int size = Marshal.SizeOf(returnDpc);

            Win32.MemoryAlloc data = new Win32.MemoryAlloc(size);

            uint status;

            status = Win32.NtQuerySystemInformation(
                Win32.SystemInformationClass.SystemDpcBehaviorInformation,
                data,
                data.Size,
                out retLength);

            if (status != NtStatus_Success)
            {
                uint err = (uint)Marshal.GetLastWin32Error();
                return returnDpc;
            }
            
            returnDpc = data.ReadStruct<SystemDpcBehaviorInformation>(0, 0);
            return returnDpc;
        }
        #endregion

        [StructLayout(LayoutKind.Sequential)]
        public struct UnicodeString 
        {
            public ushort Length;               // 2
            public ushort MaximumLength;        // 2
            public IntPtr Buffer;               // 4
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct VmCountersEx
        {
            public IntPtr PeakVirtualSize;
            public IntPtr VirtualSize;
            public int PageFaultCount;
            public IntPtr PeakWorkingSetSize;
            public IntPtr WorkingSetSize;
            public IntPtr QuotaPeakPagedPoolUsage;
            public IntPtr QuotaPagedPoolUsage;
            public IntPtr QuotaPeakNonPagedPoolUsage;
            public IntPtr QuotaNonPagedPoolUsage;
            public IntPtr PagefileUsage;
            public IntPtr PeakPagefileUsage;
            public IntPtr PrivatePageCount;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IoCounters
        {
            public ulong ReadOperationCount;
            public ulong WriteOperationCount;
            public ulong OtherOperationCount;
            public ulong ReadTransferCount;
            public ulong WriteTransferCount;
            public ulong OtherTransferCount;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SystemProcessInformation
        {                                       // Ln   St End
            public int NextEntryOffset;         //  4    0   3
            public int NumberOfThreads;         //  4    4   7
            public long SpareLi1;               //  8    8  15 
            public long SpareLi2;               //  8   16  23 
            public long SpareLi3;               //  8   24  31
            public long CreateTime;             //  8   32  39
            public long UserTime;               //  8   40
            public long KernelTime;             //  8   48
            public UnicodeString ImageName;     //  8   56
            public int BasePriority;            //  4   64
            private IntPtr _processId;          //  4
            private IntPtr _parentProcessId;
            public int HandleCount;
            public int SessionId;
            public IntPtr PageDirectoryBase;
            public VmCountersEx VirtualMemoryCounters;
            public IoCounters IoCounters;

            public int Id
            {
                get { return _processId.ToInt32(); }
                set { _processId = new IntPtr(value); }
            }

            public int ParentProcessId
            {
                get { return _parentProcessId.ToInt32(); }
                set { _parentProcessId = new IntPtr(value); }
            }

            public string Name
            {
                get 
                {
                    string procName = string.Empty;
                    if (ImageName.Length == 0)
                        return procName;

                    try
                    {
                        procName = Marshal.PtrToStringUni(ImageName.Buffer, ImageName.Length / 2);
                    }
                    catch { };
                    return procName;
                }
            }

            public DateTime StartTime
            {
                get { return DateTime.FromFileTime(CreateTime); } // 100-ns units 
            }

            public TimeSpan TotalProcessorTime
            {
                get { return new TimeSpan((UserTime + KernelTime)*60);  }
            }

            public TimeSpan UserProcessorTime
            {
                get { return new TimeSpan(UserTime*60); }
            }

            public long PrivateMemorySize64
            {
                get { return VirtualMemoryCounters.PrivatePageCount.ToInt64(); }
            }
            public long WorkingSet64
            {
                get { return VirtualMemoryCounters.WorkingSetSize.ToInt64(); }
            }

            public long PeakWorkingSet64 
            {
                get { return VirtualMemoryCounters.PeakWorkingSetSize.ToInt64(); }
            }
            public long VirtualMemorySize64 
            {
                get { return VirtualMemoryCounters.VirtualSize.ToInt64(); }
            }
            public long PeakVirtualMemorySize64 
            {
                get { return VirtualMemoryCounters.PeakVirtualSize.ToInt64(); }
            }

            public long PagedMemorySize64 
            {
                get { return VirtualMemoryCounters.PagefileUsage.ToInt64(); }
            }
            public long PeakPagedMemorySize64
            {
                get { return VirtualMemoryCounters.PeakPagefileUsage.ToInt64(); }
            }

            public class ThreadList
            {
                public int Count;
                // TODO - add thread info
            }
            public ThreadList Threads
            {
                get
                {
                    // TODO - populate with something.
                    ThreadList threadList = new ThreadList();
                    threadList.Count = NumberOfThreads;
                    return threadList;
                }
            }

            public class ModuleList
            {
                public int Count;
                // TODO - add module info
            }
            public ModuleList Modules
            {
                get
                {
                    // TODO - populate with something.
                    ModuleList moduleList = new ModuleList();
                    moduleList.Count = 0;
                    // To slow 
                    // moduleList.Count = System.Diagnostics.Process.GetProcessById(this.Id).Modules.Count;
                    return moduleList;
                }
            }
        }

        public struct SystemProcess
        {
            public string Name;
            public SystemProcessInformation Process;
            // public Dictionary<int, SystemThreadInformation> Threads;
        }

        /// <summary>
        /// Gets a dictionary containing the currently running processes.
        /// </summary>
        public static Dictionary<int, SystemProcess> GetProcesses()
        {
            int retLength;
            Dictionary<int, SystemProcess> returnProcesses;

            Win32.MemoryAlloc data = new Win32.MemoryAlloc(0x10000);

            uint status;

            int attempts = 0;

            while (true)
            {
                attempts++;

                status = Win32.NtQuerySystemInformation(
                    Win32.SystemInformationClass.SystemProcessInformation,
                    data,
                    data.Size,
                    out retLength);

                if (status != NtStatus_Success)
                {
                    uint err = (uint)Marshal.GetLastWin32Error();

                    if (status != NtStatus_INFO_LENGTH_MISMATCH)
                        return null;

                    if (attempts > 3)
                        return null;

                    data.Resize(retLength);
                }
                else
                {
                    break;
                }
            }

            // 32 is initial guess on #proocesses on a computer 
            returnProcesses = new Dictionary<int, SystemProcess>(32); 

            int i = 0;
            SystemProcess currentProcess = new SystemProcess();

            do
            {
                currentProcess.Process = data.ReadStruct<SystemProcessInformation>(i, 0);
                currentProcess.Name = currentProcess.Process.Name;

#if false
                if (getThreads &&
                    currentProcess.ProcessId != 0)
                {
                    currentProcess.Threads = new Dictionary<int, SystemThreadInformation>();

                    for (int j = 0; j < currentProcess.NumberOfThreads; j++)
                    {
                        var thread = data.ReadStruct<SystemThreadInformation>(i +
                            Marshal.SizeOf(typeof(SystemProcessInformation)), j);

                        currentProcess.Threads.Add(thread.ClientId.ThreadId, thread);
                    }
                }
#endif

                returnProcesses.Add(currentProcess.Process.Id, currentProcess);

                i += currentProcess.Process.NextEntryOffset;
            } while (currentProcess.Process.NextEntryOffset != 0);

            return returnProcesses;
        }
    }
}
