using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using System.Diagnostics;

using System.Security;
using System.Security.Principal;

using System.Net;       // map tcpdevice to ip:port

namespace MagniFile
{
  
    /// <summary>
    /// Collect open handles.  Code found on sysInternals website.
    /// http://forum.sysinternals.com/forum_posts.asp?TID=17533
    /// 
    /// Modified by Dennis Lang 2009
    /// https://landenlabs.com/
    /// 
    /// </summary>
    class OpenHandles
    {
        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern uint ZwQuerySystemInformation(SYSTEM_INFORMATION_CLASS SystemInformationClass, 
            IntPtr SystemInformation, int SystemInformationLength, out int ReturnLength);

        public enum SYSTEM_INFORMATION_CLASS : int
        {
            SystemBasicInformation,
            SystemProcessorInformation,
            SystemPerformanceInformation,
            SystemTimeOfDayInformation,
            SystemNotImplemented1,
            SystemProcessesAndThreadsInformation,
            SystemCallCounts,
            SystemConfigurationInformation,
            SystemProcessorTimes,
            SystemGlobalFlag,
            SystemNotImplemented2,
            SystemModuleInformation,
            SystemLockInformation,
            SystemNotImplemented3,
            SystemNotImplemented4,
            SystemNotImplemented5,
            SystemHandleInformation
            // there's more but it would make the post too long...
        }

        //  http://msdn.microsoft.com/en-us/library/ms973190.aspx
        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_HANDLE_INFORMATION
        {   // sizeof 16 for XP32 and 24 bytes for XP64
            public Int32 ProcessId;
            public byte ObjectTypeNumber;
            public byte Flags;              // 1 = PROTECT_FROM_CLOSE, 2 = INHERIT
            public short Handle;
            public IntPtr Object;           // 4 or 8 byte field.
            public Int32 GrantedAccess;
            // pad with 4 bytes for xp64
        }
        static  int hndSize = IntPtr.Size;    // 8 for xp64 and 4 for xp32

        const uint STATUS_INFO_LENGTH_MISMATCH = 0xc0000004;

        public static SYSTEM_HANDLE_INFORMATION[] EnumHandles()
        {
            int retLength = 0;
            int allocLength = 0x1000;
            IntPtr data = Marshal.AllocHGlobal(allocLength);
            
            // This is needed because ZwQuerySystemInformation with SystemHandleInformation doesn't
            // actually give a real return length when called with an insufficient buffer. This code
            // tries repeatedly to call the function, doubling the buffer size each time it fails.
            while (ZwQuerySystemInformation(SYSTEM_INFORMATION_CLASS.SystemHandleInformation, data,
                allocLength, out retLength) == STATUS_INFO_LENGTH_MISMATCH)
                data = Marshal.ReAllocHGlobal(data, new IntPtr(allocLength *= 2));
            
            // The structure of the buffer is the handle count plus an array of SYSTEM_HANDLE_INFORMATION
            // structures.
            // int handleCount = Marshal.ReadInt32(data);
            int handleCount = Marshal.ReadInt32(data);
            SYSTEM_HANDLE_INFORMATION[] returnHandles = new SYSTEM_HANDLE_INFORMATION[handleCount];
            
            for (int i = 0; i < handleCount; i++)
            {
                returnHandles[i] = (SYSTEM_HANDLE_INFORMATION)Marshal.PtrToStructure(
                    new IntPtr(data.ToInt32() + hndSize + i * Marshal.SizeOf(typeof(SYSTEM_HANDLE_INFORMATION))),
                    typeof(SYSTEM_HANDLE_INFORMATION));
            }
            
            Marshal.FreeHGlobal(data);
            
            return returnHandles;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int DuplicateHandle(int hSourceProcessHandle, int hSourceHandle,
                int hTargetProcessHandle, ref int lpTargetHandle,
                int dwDesiredAccess, int bInheritHandle, int dwOptions);

        [Flags]
        enum DuplicateOptions : uint
        {
            DUPLICATE_CLOSE_SOURCE = (0x00000001),// Closes the source handle. This occurs regardless of any error status returned.
            DUPLICATE_SAME_ACCESS = (0x00000002), //Ignores the dwDesiredAccess parameter. The duplicate handle has the same access as the source handle.
        }

        [StructLayout(LayoutKind.Sequential)]
        struct StatusBlock
        {
            public UInt64 v1;
            public UInt64 v2;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct FILE_NAME_INFORMATION 
        {
          public UInt64     FileNameLength;
          public Char       FileName;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct UNICODE_STRING
        {
            public UInt16 Length;
            public UInt16 MaximumLength;
            public IntPtr pBuffer;
        }

        [DllImport("ntdll.dll", SetLastError = true)]
        static extern uint ZwQueryInformationFile(int hnd, IntPtr StatusBlock, IntPtr FILE_NAME_INFORMATION, 
            int FileInfoLength, int code);


        [DllImport("ntdll.dll", SetLastError = true)]
        static extern uint ZwQueryObject(int hnd, int code, IntPtr FILE_NAME_INFORMATION,
            int FileInfoLength, ref int ReturnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(int handle);


        [DllImport("advapi32", SetLastError = true),
        SuppressUnmanagedCodeSecurityAttribute]
        static extern int OpenProcessToken(
            System.IntPtr ProcessHandle,    // handle to process
            int DesiredAccess,              // desired access to process
            ref IntPtr TokenHandle);        // handle to open access token

  
        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall,
                ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

#if false
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateToken(
            IntPtr ExistingTokenHandle,
            int SECURITY_IMPERSONATION_LEVEL,
            ref IntPtr DuplicateTokenHandle);

        static IntPtr DupeToken(IntPtr token, int Level)
        {
            IntPtr dupeTokenHandle = IntPtr.Zero;
            bool retVal = DuplicateToken(token, Level, ref dupeTokenHandle);
            return dupeTokenHandle;
        }
#endif

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TokPriv1Luid
        {
            public int Count;
            public long Luid;
            public int Attr;
        }

        /// <summary>
        /// Enable special Debug privilege to peak at file handles owned by other processes.
        /// </summary>
        public static void EnableDebugPrivilege()
        {
            // const int TOKEN_QUERY = 0X00000008;
            const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;

            const int SE_PRIVILEGE_ENABLED = 0x00000002;
            const string SE_DEBUG_NAME = "SeDebugPrivilege";

            IntPtr hToken = IntPtr.Zero;
            Process proc = Process.GetCurrentProcess();
            if (OpenProcessToken(proc.Handle, TOKEN_ADJUST_PRIVILEGES , ref hToken) != 0)
            {
                TokPriv1Luid tp = new TokPriv1Luid();
              
                if (LookupPrivilegeValue(string.Empty, SE_DEBUG_NAME, ref tp.Luid))
                {
                    tp.Count = 1;
                    tp.Attr = SE_PRIVILEGE_ENABLED;
                    int tpSz = Marshal.SizeOf(tp);

                    if (AdjustTokenPrivileges(hToken, false, ref tp, tpSz, IntPtr.Zero, IntPtr.Zero) == false)
                    {
                        int err = Marshal.GetLastWin32Error();
                        System.Diagnostics.Debug.WriteLine("Debug Privilege failed, Error =" + err);
                    }
                }
            }
        }

        /// <summary>
        /// Return Description of handle.
        /// Note: only some handle object types are supported (Directory, Key, File, ...)
        /// ToDo: This list should not be hard coded. Instead the OS should be queried for each type.
        /// </summary>
        /// <param name="pid">Process Id of handle owner</param>
        /// <param name="handle">Handle number</param>
        /// <param name="objectTypeNumber">Object number of handle (See ObjectTypeDescription()) </param>
        /// <returns>true string has description, false string has error</returns>
        public static bool HandleDescription(
            Process ourProc,
            Process pidProc,
            int handle, int objectTypeNumber, out string description)
        {
            description = string.Empty;
            bool result = false;

            // In order to query a handle, we need to convert it from the owners number scheme to 
            // our number scheme by duplicating the handle. Some handles cannot be duplicated.
            if (objectTypeNumber > 30 || objectTypeNumber < 0)
                objectTypeNumber = 0;

            switch (objectTypeNumber)
            {
                case 1: //type1 
                // case 2: //Directory 
                // case 3: //SymbolicLink 
                case 4: //Token 
                case 5: //Process 
                case 6: //Thread 
                case 7: //Job 
                case 8: //type8 
                case 9: //Event 
                case 10: //type10 
                // case 11: //Mutant 
                case 12: //type12 
                case 13: //Semaphore 
                case 14: //Timer 
                case 15: //type15 
                // case 16: //KeyedEvent 
                // case 17: //WindowStn 
                // case 18: //Desktop 
                // case 19: //Section 
                // case 20: //Registry 
                case 21: //Port 
                case 22: //type22 
                case 23: //type23 
                case 24: //type24 
                case 25: //type25 
                case 26: //type26 
                case 27: //IoComplete       
                // case 28: //File 
                case 29: //WmiGuid 
                case 30: // ????
                    return false;   // cannot dup, return 
            }

            // Process pidProc = Process.GetProcessById(pid);
            // Process ourProc = Process.GetCurrentProcess();


            int ourHandle = 0;
            IntPtr dataPtr = IntPtr.Zero;
            IntPtr statusBlock = IntPtr.Zero;
            const int READ_CONTROL = 0x00020000;
            const int STANDARD_RIGHTS_READ = READ_CONTROL;
            // const int DUPLICATE_CLOSE_SOURCE = 0x00000001;
            // const int DUPLICATE_SAME_ACCESS = 0x00000002;  


            try
            {
                // socket should use WSADuplicateSocket(SOCKET s, DWORD processId, WSAPROTOCOL_INFO* pProtocolInfo)

                int status1 = DuplicateHandle(pidProc.Handle.ToInt32(), handle, ourProc.Handle.ToInt32(),
                    ref ourHandle, 0 /* STANDARD_RIGHTS_READ */, 0, 0);
                if (status1 == 1 && ourHandle > 0)
                {
                    const int fileInfoSize = 512;             // guess at max query response size.
                    dataPtr = Marshal.AllocHGlobal(fileInfoSize);
                    uint status2;
                    int returnSize = 0;


                    if (objectTypeNumber == 28)
                    {
                        // Use specialized QueryInformationFile for file handles.
                        statusBlock = Marshal.AllocHGlobal(16);
                        int FileNameInformation = 9;        // enum to query filename

                        status2 = ZwQueryInformationFile(ourHandle, statusBlock, dataPtr,
                                fileInfoSize, FileNameInformation);
                        if (status2 == 0)
                        {
                            int filenameLength = Marshal.ReadInt32(dataPtr);
                            if (filenameLength > 0 && filenameLength < 256)
                            {
                                string fn = Marshal.PtrToStringUni(new IntPtr(dataPtr.ToInt32() + 4));
                                string tmpStr = fn.Substring(0, filenameLength / 2);
                                description = (string)tmpStr.Clone();

                                if (description == @"\Device\Tcp" ||
                                    description == @"\Device\Udp")
                                {
                                    description += NetworkInfo.ConnectionDetails(ourHandle);
                                }

                                result = true;
                            }
                        }
                    }

                    if (objectTypeNumber != 28 || description.Length == 0)     
                    {
                        // use QueryObject for non-files handles or files which failed to return a desc.
                        status2 = ZwQueryObject(ourHandle, 1, dataPtr, fileInfoSize, ref returnSize);
                        if (status2 == 0)
                        {
                            int filenameLength = Marshal.ReadInt16(dataPtr);
                            if (filenameLength > 0)
                            {
                                int offset = IntPtr.Size * 2;
                                string fn = Marshal.PtrToStringUni(new IntPtr(dataPtr.ToInt32() + offset));
                                string tmpStr = fn.Substring(0, filenameLength / 2);
                                description = (string)tmpStr.Clone();

                                if (description == @"\Device\Tcp" ||
                                 description == @"\Device\Udp")
                                {
                                    description += NetworkInfo.ConnectionDetails(ourHandle);
                                }

                                result = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                description = ee.Message;
            }
            finally
            {
                Marshal.FreeHGlobal(dataPtr);
                Marshal.FreeHGlobal(statusBlock);
                CloseHandle(ourHandle);
            }

            return result;
        }


        public static int MaxObjTypeNumber
        {
            get { return 29; }
        }

        /// <summary>
        /// Convert object type number to a string.
        /// </summary>
        /// <param name="objTypeNumber"></param>
        /// <returns></returns>
        public static string ObjectTypeDescription(int objTypeNumber)
        {
            string resultName = string.Empty;

            switch (objTypeNumber)
            {
                case 1:  return  "type1";
                case 2:  return  "Directory";
                case 3:  return  "SymbolicLink";
                case 4:  return  "Token";
                case 5:  return  "Process";
                case 6:  return  "Thread";
                case 7:  return  "Job";
                case 8:  return  "type8";
                case 9:  return  "Event";
                case 10:  return  "type10";
                case 11:  return  "Mutant";
                case 12:  return  "type12";
                case 13:  return  "Semaphore";
                case 14:  return  "Timer";
                case 15:  return  "type15";
                case 16:  return  "KeyedEvent";
                case 17:  return  "WindowStn";
                case 18:  return  "Desktop";
                case 19:  return  "Section";
                case 20:  return  "Registry";
                case 21:  return  "Port";
                case 22:  return  "type22";
                case 23:  return  "type23";
                case 24:  return  "type24";
                case 25:  return  "type25";
                case 26:  return  "type26";
                case 27:  return  "IoComplete";
                case 28:  return  "File";
                case 29:  return  "WmiGuid";
            }

            return resultName;
        }


        /// <summary>
        /// Convert access mode to string.
        /// </summary>
        /// <param name="access"></param>
        /// <returns>string</returns>
        public static string AccessToString(int access)
        {
            const UInt32  DELETE                         = 0x00010000;
            const UInt32  READ_CONTROL                   = 0x00020000;
            const UInt32  WRITE_DAC                      = 0x00040000;
            const UInt32  WRITE_OWNER                    = 0x00080000;
            const UInt32  SYNCHRONIZE                    = 0x00100000;
            const UInt32  STANDARD_RIGHTS_REQUIRED       = 0x000F0000;
            const UInt32  STANDARD_RIGHTS_ALL            = 0x001F0000;
            const UInt32  SPECIFIC_RIGHTS_ALL            = 0x0000FFFF;
            const UInt32  ACCESS_SYSTEM_SECURITY         = 0x01000000;
            const UInt32  MAXIMUM_ALLOWED                = 0x02000000;
            const UInt32  GENERIC_READ                   = 0x80000000;
            const UInt32  GENERIC_WRITE                  = 0x40000000;
            const UInt32  GENERIC_EXECUTE                = 0x20000000;
            const UInt32  GENERIC_ALL                    = 0x10000000;

            // Specific access
            const UInt32  FILE_READ_DATA            = 0x0001;    // file & pipe
            const UInt32  FILE_LIST_DIRECTORY       = 0x0001;    // directory
            const UInt32  FILE_WRITE_DATA           = 0x0002;    // file & pipe
            const UInt32  FILE_ADD_FILE             = 0x0002;    // directory
            const UInt32  FILE_APPEND_DATA          = 0x0004;    // file
            const UInt32  FILE_ADD_SUBDIRECTORY     = 0x0004;    // directory
            const UInt32  FILE_CREATE_PIPE_INSTANCE = 0x0004;    // named pipe
            const UInt32  FILE_READ_EA              = 0x0008;    // file & directory
            const UInt32  FILE_WRITE_EA             = 0x0010;    // file & directory
            const UInt32  FILE_EXECUTE              = 0x0020;    // file
            const UInt32  FILE_TRAVERSE             = 0x0020;    // directory
            const UInt32  FILE_DELETE_CHILD         = 0x0040;    // directory
            const UInt32  FILE_READ_ATTRIBUTES      = 0x0080;    // all
            const UInt32  FILE_WRITE_ATTRIBUTES     = 0x0100;    // all

            const UInt32  FILE_ALL_ACCESS  = (STANDARD_RIGHTS_REQUIRED|SYNCHRONIZE| 0x1FF); 
            const UInt32  FILE_GENERIC_READ = (READ_CONTROL|FILE_READ_DATA|FILE_READ_ATTRIBUTES|FILE_READ_EA|SYNCHRONIZE);
            const UInt32  FILE_GENERIC_WRITE = (READ_CONTROL|FILE_WRITE_DATA|FILE_WRITE_ATTRIBUTES|FILE_WRITE_EA|FILE_APPEND_DATA|SYNCHRONIZE);
            const UInt32  FILE_GENERIC_EXECUTE = (READ_CONTROL | FILE_READ_ATTRIBUTES | FILE_EXECUTE | SYNCHRONIZE);


            UInt32 uAccess = (UInt32)access;

            string sAccess = string.Empty;
            if ((uAccess & DELETE) != 0) sAccess += "D";
            if ((uAccess & SPECIFIC_RIGHTS_ALL) != 0)
            {
                if ((uAccess & FILE_READ_DATA) != 0) sAccess += "r";
                if ((uAccess & FILE_WRITE_DATA) != 0) sAccess += "w";
                if ((uAccess & FILE_APPEND_DATA) != 0) sAccess += "a";
                if ((uAccess & FILE_READ_ATTRIBUTES) != 0) sAccess += "t";
                if ((uAccess & FILE_WRITE_ATTRIBUTES) != 0) sAccess += "T";
            }

            if ((uAccess & GENERIC_ALL) != 0)
                sAccess += "All";
            else
            {
                if ((uAccess & GENERIC_READ) != 0) sAccess += "R";
                if ((uAccess & GENERIC_WRITE) != 0) sAccess += "W";
                if ((uAccess & GENERIC_EXECUTE) != 0) sAccess += "E";
            }

            return sAccess;
        }
    }


    /// <summary>
    /// Methods to get Network information.
    /// </summary>
    class NetworkInfo
    {
        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern uint NtDeviceIoControlFile(
            int fileHandle,
            IntPtr eventHandle,
            IntPtr ptrApcRoutine,
            IntPtr ptrApcContext,
            IntPtr StatusBlock,
            UInt32 ioControlCode,
            IntPtr ptrInputBuffer,
            int inputLength,
            IntPtr ptrOutputBuffer,
            int outputLength);

        [StructLayout(LayoutKind.Sequential)]
        public struct TDI_REQUEST
        {
            public IntPtr Handle;
            public IntPtr RequestNotifyObject;
            public IntPtr RequestContext;
            public UInt32 TdiStatus;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TDI_REQUEST_QUERY_INFORMATION
        {
            public TDI_REQUEST Request;
            public UInt32 QueryType;                          // class of information to be queried.
            public IntPtr /* PTDI_CONNECTION_INFORMATION */ RequestConnectionInformation;
        }

        public static UInt16 Reverse(UInt16 v)
        {
            byte b1 = (byte)v;
            byte b2 = (byte)(v >> 8);

            return (UInt16)(b2 | (b1 << 8));
        }

        const UInt32 TDI_QUERY_ADDRESS_INFO = 0x00000003;
        const UInt32 FILE_DEVICE_TRANSPORT = 0x00000021;
        const UInt32 METHOD_OUT_DIRECT = 2;
        const UInt32 FILE_ANY_ACCESS = 0;

        // 	CTL_CODE(FILE_DEVICE_TRANSPORT, 4, METHOD_OUT_DIRECT, FILE_ANY_ACCESS)
        const UInt32 IOCTL_TDI_QUERY_INFORMATION =
              (FILE_DEVICE_TRANSPORT << 16)
            | (FILE_ANY_ACCESS << 14)
            | (4 << 2)                              // function
            | (METHOD_OUT_DIRECT);


        public static string ConnectionDetails(int handle)
        {
            IntPtr statusBlock = Marshal.AllocHGlobal(8);
            TDI_REQUEST_QUERY_INFORMATION tdiRequestAddress =
                new TDI_REQUEST_QUERY_INFORMATION()
                {
                    Request = new TDI_REQUEST() { Handle = IntPtr.Zero },
                    QueryType = TDI_QUERY_ADDRESS_INFO
                };

            IntPtr tdiPtr = Marshal.AllocHGlobal(Marshal.SizeOf(tdiRequestAddress));
            Marshal.StructureToPtr(tdiRequestAddress, tdiPtr, false);

            const int outSize = 128;
            IntPtr outBuffer = Marshal.AllocHGlobal(outSize);

            int err = 0;
            uint ntStatus = 0;

            try
            {
                ntStatus = NtDeviceIoControlFile(
                    handle,
                    IntPtr.Zero,        // event handle
                    IntPtr.Zero,
                    IntPtr.Zero,
                    statusBlock,
                    IOCTL_TDI_QUERY_INFORMATION,
                    tdiPtr,
                    Marshal.SizeOf(tdiRequestAddress),
                    outBuffer,
                    outSize);

            }
            catch (Exception ee)
            {
                err = Marshal.GetLastWin32Error();
            }
            string connectionStr = string.Empty;

            if (ntStatus == 0)
            {
                int addr = Marshal.ReadInt32(outBuffer, 16);
                UInt16 port = (UInt16)Marshal.ReadInt16(outBuffer, 12);
                IPAddress ipAddr = new IPAddress(addr);
                // struct in_addr *pAddr = (struct in_addr *)&tdiAddress[14];
                // sout << inet_ntoa(*pAddr) << ":" <<  ntohs(*(PUSHORT)&tdiAddress[12]);
                connectionStr = string.Format(" Ip:{0} Port:{1}", ipAddr.ToString(), Reverse(port));
            }

            Marshal.FreeHGlobal(statusBlock);
            Marshal.FreeHGlobal(tdiPtr);
            Marshal.FreeHGlobal(outBuffer);

            return connectionStr;
        }
    }

}