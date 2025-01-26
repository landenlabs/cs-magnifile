// ------------------------------------------------------------------------------------------------
//
// ListOpenHandles
//
// Author:  DLang   2009  
// https://landenlabs.com
//
// ------------------------------------------------------------------------------------------------

#define _CRT_SECURE_NO_WARNINGS
#include "ListOpenHandles.h"
#include "Hnd.h"
#include "Support.h"

#include <windows.h>
#include <errno.h>
#include <assert.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <time.h>
#include <io.h>
#include <stddef.h>     /* offsetof */

// Internals
#include <psapi.h>
#include <winternl.h>
#include <winioctl.h>

// C++
#include <iostream>
#include <fstream>
#include <sstream>
#include <iomanip>
#include <map>
#include <vector>
using  namespace std;

// Libraries
#pragma comment(lib, "psapi.lib")

#if 0
 #include <Ntstatus.h>
#else
 const unsigned int STATUS_INFO_LENGTH_MISMATCH = 0xc0000004;
 // const unsigned int STATUS_INVALID_HANDLE = 0xC0000008;
 const DWORD STATUS_SUCCESS = 0;
#endif


// ------------------------------------------------------------------------------------------------
// Resources on Open File Handles 
//
//  http://forum.sysinternals.com/forum_posts.asp?TID=5677
//  http://forum.sysinternals.com/forum_posts.asp?TID=18892
//  http://www.daniweb.com/forums/thread114975.html
//  http://www.techreplies.com/development-resources-58/enumerating-handles-processes-c-like-procxp-782021/
//  http://www.codeguru.com/forum/archive/index.php/t-328977.html
//  http://social.msdn.microsoft.com/Forums/en-US/netfx64bit/thread/7bfc32ea-b493-4853-a690-fef7b1170162
//  http://msdn.microsoft.com/en-us/library/ms804359.aspx
//  http://msdn.microsoft.com/en-us/library/bb432383(VS.85).aspx
//  C# http://processhacker.sourceforge.net/hacking/structs.php#systemprocessinformation
//  C# http://forum.sysinternals.com/forum_posts.asp?TID=17533

// The following is required if you compile on XP64
#ifdef _WIN64_

typedef enum _OBJECT_INFORMATION_CLASS {
  ObjectBasicInformation=0,
  ObjectNameInformation=1,       // undocumented
  ObjectTypeInformation=2,
} OBJECT_INFORMATION_CLASS;

typedef struct _PUBLIC_OBJECT_BASIC_INFORMATION {
  ULONG  Attributes;
  ACCESS_MASK  GrantedAccess;
  ULONG  HandleCount;
  ULONG  PointerCount;
  ULONG  Reserved[10];
} PUBLIC_OBJECT_BASIC_INFORMATION;

typedef struct _PUBLIC_OBJECT_TYPE_INFORMATION {
    UNICODE_STRING TypeName;
    ULONG TotalNumberOfObjects;
    ULONG TotalNumberOfHandles;
    ULONG TotalPagedPoolUsage;
    ULONG TotalNonPagedPoolUsage;
    ULONG TotalNamePoolUsage;
    ULONG TotalHandleTableUsage;
    ULONG HighWaterNumberOfObjects;
    ULONG HighWaterNumberOfHandles;
    ULONG HighWaterPagedPoolUsage;
    ULONG HighWaterNonPagedPoolUsage;
    ULONG HighWaterNamePoolUsage;
    ULONG HighWaterHandleTableUsage;
    ULONG InvalidAttributes;
    GENERIC_MAPPING GenericMapping;
    ULONG ValidAccessMask;
    BOOLEAN SecurityRequired;
    BOOLEAN MaintainHandleCount;
    ULONG PoolType;
    ULONG DefaultPagedPoolCharge;
    ULONG DefaultNonPagedPoolCharge;
} PUBLIC_OBJECT_TYPE_INFORMATION; 

#endif

typedef struct _PUBLIC_OBJECT_NAME_INFORMATION 
{
  UNICODE_STRING          Name;
  WCHAR                   NameBuffer[1];
} PUBLIC_OBJECT_NAME_INFORMATION, *PPUBLIC_OBJECT_NAME_INFORMATION;

typedef struct _FILE_NAME_INFORMATION 
{
  ULONG                   FileNameLength;
  WCHAR                   FileName[1];
} FILE_NAME_INFORMATION, *PFILE_NAME_INFORMATION;


typedef enum __FILE_INFORMATION_CLASS 
{
    __FileDirectoryInformation=1,
    FileFullDirectoryInformation,
    FileBothDirectoryInformation,
    FileBasicInformation,
    FileStandardInformation,
    FileInternalInformation,
    FileEaInformation,
    FileAccessInformation,
    FileNameInformation,
    FileRenameInformation,
    FileLinkInformation,
    FileNamesInformation,
    FileDispositionInformation,
    FilePositionInformation,
    FileFullEaInformation,
    FileModeInformation,
    FileAlignmentInformation,
    FileAllInformation,
    FileAllocationInformation,
    FileEndOfFileInformation,
    FileAlternateNameInformation,
    FileStreamInformation,
    FilePipeInformation,
    FilePipeLocalInformation,
    FilePipeRemoteInformation,
    FileMailslotQueryInformation,
    FileMailslotSetInformation,
    FileCompressionInformation,
    FileCopyOnWriteInformation,
    FileCompletionInformation,
    FileMoveClusterInformation,
    FileQuotaInformation,
    FileReparsePointInformation,
    FileNetworkOpenInformation,
    FileObjectIdInformation,
    FileTrackingInformation,
    FileOleDirectoryInformation,
    FileContentIndexInformation,
    FileInheritContentIndexInformation,
    FileOleInformation,
    FileMaximumInformation
} __FILE_INFORMATION_CLASS;


struct SYSTEM_HANDLE_INFORMATION
{                                       // XP64     XP32
	ULONG	ProcessId;                  //  4        4
	UCHAR	ObjectTypeNumber;           //  1        1
	UCHAR	Flags;                      //  1        1
	USHORT	Handle;                     //  2        2  
	PVOID	Object;                     //  8        4 
	ACCESS_MASK	GrantedAccess;          //  4        4
};                                      // 24       16

struct SYSTEM_HANDLE_LIST_INFORMATION
{
    ULONG   NumHandles;
    SYSTEM_HANDLE_INFORMATION   Handles[1];
};


// Clone of SYSTEM_INFORMATION_CLASS, just adds additional hide enums
enum __SYSTEM_INFORMATION_CLASS
{
    __SystemBasicInformation,                   // 0 
    __SystemProcessorInformation,               // 1 
    __SystemPerformanceInformation,             // 2 
    __SystemTimeOfDayInformation,               // 3 
    __SystemNotImplemented1,                    // 4 
    __SystemProcessesAndThreadsInformation,     // 5 
    __SystemCallCounts,                         // 6 
    __SystemConfigurationInformation,           // 7 
    __SystemProcessorTimes,                     // 8 
    __SystemGlobalFlag,                         // 9 
    __SystemNotImplemented2,                    // 10 
    __SystemModuleInformation,                  // 11 
    __SystemLockInformation,                    // 12 
    __SystemNotImplemented3,                    // 13 
    __SystemNotImplemented4,                    // 14 
    __SystemNotImplemented5,                    // 15 
    __SystemHandleInformation                   // 16 
    // there's more but it would make the post too long...
};

const SYSTEM_INFORMATION_CLASS SystemHandleInformation = (SYSTEM_INFORMATION_CLASS)__SystemHandleInformation;

#if 0
typedef struct _UNICODE_STRING
{
    USHORT Length;
    USHORT MaximumLength;
    PWSTR Buffer;
} UNICODE_STRING, *PUNICODE_STRING;
#endif

typedef struct _STRING64
{
    USHORT Length;
    USHORT MaximumLength;
    PWSTR  Buffer;
} UNICODE_STRING64;

typedef struct _CLIENT_ID
{
 int  ProcessId;
 int  ThreadId;
}CLIENT_ID, *PCLIENT_ID;

typedef struct _SYSTEM_THREAD_INFORMATION
{
    LARGE_INTEGER KernelTime;
    LARGE_INTEGER UserTime;
    LARGE_INTEGER CreateTime;
    ULONG WaitTime;
    PVOID StartAddress;
    CLIENT_ID ClientId;
    LONG Priority;
    LONG BasePriority;
    ULONG ContextSwitches;
    ULONG ThreadState;
    ULONG WaitReason;
} SYSTEM_THREAD_INFORMATION, *PSYSTEM_THREAD_INFORMATION;


typedef struct _SYSTEM_PROCESS_INFORMATION_undoc
{
    ULONG NextEntryOffset;
    ULONG NumberOfThreads;
    LARGE_INTEGER WorkingSetPrivateSize;
    ULONG HardFaultCount;
    ULONG NumberOfThreadsHighWatermark;
    ULONGLONG CycleTime;
    LARGE_INTEGER CreateTime;
    LARGE_INTEGER UserTime;
    LARGE_INTEGER KernelTime;
    UNICODE_STRING ImageName; 
    LONG BasePriority;
    HANDLE UniqueProcessId;
    HANDLE InheritedFromUniqueProcessId;
    ULONG HandleCount;
    ULONG SessionId;
    ULONG_PTR UniqueProcessKey;
    SIZE_T PeakVirtualSize;
    SIZE_T VirtualSize;
    ULONG PageFaultCount;
    SIZE_T PeakWorkingSetSize;
    SIZE_T WorkingSetSize;
    SIZE_T QuotaPeakPagedPoolUsage;
    SIZE_T QuotaPagedPoolUsage;
    SIZE_T QuotaPeakNonPagedPoolUsage;
    SIZE_T QuotaNonPagedPoolUsage;
    SIZE_T PagefileUsage;
    SIZE_T PeakPagefileUsage;
    SIZE_T PrivatePageCount;
    LARGE_INTEGER ReadOperationCount;
    LARGE_INTEGER WriteOperationCount;
    LARGE_INTEGER OtherOperationCount;
    LARGE_INTEGER ReadTransferCount;
    LARGE_INTEGER WriteTransferCount;
    LARGE_INTEGER OtherTransferCount;
    SYSTEM_THREAD_INFORMATION Threads[1];
} SYSTEM_PROCESS_INFORMATION_undoc;         // undocumented version 


// How to get IP address from handle
//
// http://forum.sysinternals.com/forum_posts.asp?TID=1193

#pragma comment(lib, "ws2_32.lib")
typedef LONG TDI_STATUS;
typedef PVOID CONNECTION_CONTEXT;       // connection context

typedef struct _TDI_REQUEST {
    union {
        HANDLE AddressHandle;
        CONNECTION_CONTEXT ConnectionContext;
        HANDLE ControlChannel;
    } Handle;

    PVOID RequestNotifyObject;
    PVOID RequestContext;
    TDI_STATUS TdiStatus;
} TDI_REQUEST, *PTDI_REQUEST;

typedef struct _TDI_CONNECTION_INFORMATION {
    LONG UserDataLength;        // length of user data buffer
    PVOID UserData;             // pointer to user data buffer
    LONG OptionsLength;         // length of following buffer
    PVOID Options;              // pointer to buffer containing options
    LONG RemoteAddressLength;   // length of following buffer
    PVOID RemoteAddress;        // buffer containing the remote address
} TDI_CONNECTION_INFORMATION, *PTDI_CONNECTION_INFORMATION;

typedef struct _TDI_REQUEST_QUERY_INFORMATION {
    TDI_REQUEST Request;
    ULONG QueryType;                          // class of information to be queried.
    PTDI_CONNECTION_INFORMATION RequestConnectionInformation;
} TDI_REQUEST_QUERY_INFORMATION, *PTDI_REQUEST_QUERY_INFORMATION;

#define TDI_QUERY_ADDRESS_INFO			0x00000003
#define IOCTL_TDI_QUERY_INFORMATION		CTL_CODE(FILE_DEVICE_TRANSPORT, 4, METHOD_OUT_DIRECT, FILE_ANY_ACCESS)


#include "NtApi.h"
NtApi sNtApi;

// ------------------------------------------------------------------------------------------------
/// Convert Wide character to multibyte
/// Warning - static internal storage pointer returned.
static const char* WideToCptr(wchar_t* wPtr, unsigned wLen)
{
    if (wLen <= 0)
        return "";

    int cLen = WideCharToMultiByte(CP_ACP, 0, wPtr, wLen/2, NULL, 0, NULL, NULL);
    static ScopePtr<char> cPtr(cLen+1);
    
    if ((size_t)cLen > cPtr.Size())
    {
        cPtr.Resize(cLen+1);
    }

    cLen = WideCharToMultiByte(CP_ACP, 0, wPtr, wLen/2, cPtr, cLen, NULL, NULL);
    cPtr[cLen] = '\0';
    return cPtr;
}

// ------------------------------------------------------------------------------------------------
/// Compare full pathed process name too just name
static bool CompareProcessName(std::string justProcName, const char* fullPathProc, Pid_t pid)
{   
    if (justProcName.empty())
        return true;

    Pid_t justPid = (Pid_t)atol(justProcName.c_str());
    if (justPid != 0)
        return justPid == pid;

    const char* pName = strrchr(fullPathProc, '\\');
    pName = (pName != NULL) ? pName+1 : fullPathProc;

    return (_strnicmp(justProcName.c_str(),  pName, justProcName.length()) == 0);
}

// ------------------------------------------------------------------------------------------------
void ListOpenHandles::ColorizeProcInfo(const std::string& str) 
{
    const WORD FOREGROUND_WHITE = FOREGROUND_RED + FOREGROUND_GREEN + FOREGROUND_BLUE;
    HANDLE outHandle = GetStdHandle(STD_OUTPUT_HANDLE);
    WORD color = FOREGROUND_WHITE;   

    const Value& value = m_procInfoMap[str];
    if (value.minVal == value.curVal && value.curVal == value.maxVal)
    {
    }
    else if (value.curVal == value.maxVal)
    {
        color = FOREGROUND_GREEN; 
    }
    else if (value.minVal == value.curVal)
    {
        color = FOREGROUND_RED;
    }

    SetConsoleTextAttribute(outHandle, color);
    std::cerr << setw(m_procInfoWidthMap[str]) << value.curVal;
    SetConsoleTextAttribute(outHandle, FOREGROUND_WHITE);
}

// ------------------------------------------------------------------------------------------------
/// List openHandles and repeat per settings.
int ListOpenHandles::ReportOpenFiles(ostream& out, const Options& options)
{
    bool showHeaders = true;
    int  result = 0;

    for (unsigned repIdx = 0; repIdx < options.m_repeat; repIdx++)
    {
        if (repIdx != 0)
            Sleep((DWORD)(options.m_waitMin * 60 * 1000));

        time_t now;
        time(&now);
        char buf[512];
        strftime(buf, sizeof(buf), "%x, %X, ", localtime(&now));

        showHeaders = ((repIdx % 30) == 0);
        result = ReportOpenFiles(out, options, showHeaders, buf);
    }

    return result;
}

// ------------------------------------------------------------------------------------------------

static string ConnectionDetails(HANDLE hObject)
{
    // http://msdn.microsoft.com/en-us/library/ms800801.aspx

    typedef NTSTATUS (WINAPI * DevIoCtlFile_t)(
            HANDLE FileHandle, HANDLE Event, PIO_APC_ROUTINE ApcRoutine, PVOID ApcContext,
            PIO_STATUS_BLOCK IoStatusBlock, DWORD IoControlCode,
            PVOID InputBuffer, DWORD InputBufferLength,
            PVOID OutputBuffer, DWORD OutputBufferLength);

    static DevIoCtlFile_t DevIoCtlFile = (DevIoCtlFile_t)GetProcAddress(GetModuleHandle("NTDLL.DLL"), "NtDeviceIoControlFile");
    string result;

    if (DevIoCtlFile != NULL)
    {
        IO_STATUS_BLOCK IoStatusBlock;
        TDI_REQUEST_QUERY_INFORMATION tdiRequestAddress = {{0}, TDI_QUERY_ADDRESS_INFO};
        BYTE tdiAddress[128];

        HANDLE hEvent2 = 0; // CreateEvent(NULL, TRUE, FALSE, NULL);
        NTSTATUS ntReturn2 = DevIoCtlFile(hObject, hEvent2, NULL, NULL, &IoStatusBlock, IOCTL_TDI_QUERY_INFORMATION,
            &tdiRequestAddress, sizeof(tdiRequestAddress), &tdiAddress, sizeof(tdiAddress));
        if (hEvent2) 
            CloseHandle(hEvent2);

        if (ntReturn2 == STATUS_SUCCESS)
        {
            struct in_addr *pAddr = (struct in_addr *)&tdiAddress[14];
            std::ostringstream sout;
            sout << inet_ntoa(*pAddr) << ":" <<  ntohs(*(PUSHORT)&tdiAddress[12]);
            result = sout.str();
        }
    }

    return result;
}

// ------------------------------------------------------------------------------------------------
const WORD FOREGROUND_WHITE = FOREGROUND_RED + FOREGROUND_GREEN + FOREGROUND_BLUE;


void ColorizeOutput(unsigned int value, UIntPair& prevValues, unsigned width)
{
    HANDLE outHandle = GetStdHandle(STD_OUTPUT_HANDLE);
    WORD color = FOREGROUND_WHITE;   

    if (prevValues.first == prevValues.second && prevValues.first == 0)
    {
        prevValues.first = prevValues.second = value;
    }
    else if (value > prevValues.second)
    {
        color = FOREGROUND_GREEN; 
        prevValues.second = value;
    }
    else if (value < prevValues.first)
    {
        color = FOREGROUND_RED;
        prevValues.first = value;
    }

    SetConsoleTextAttribute(outHandle, color);
    std::cerr << setw(width) << value;
    SetConsoleTextAttribute(outHandle, FOREGROUND_WHITE);
}

#define OffsetSPI(x) offsetof(struct _SYSTEM_PROCESS_INFORMATION_undoc, x)
struct SysProcInfoPair
{
    SYSTEM_PROCESS_INFORMATION_undoc minValue;
    SYSTEM_PROCESS_INFORMATION_undoc maxValue;


    template <typename TT>
    void ColorizeOutput(TT value, size_t offset, int width)
    {
        HANDLE outHandle = GetStdHandle(STD_OUTPUT_HANDLE);
        WORD color = FOREGROUND_WHITE;

        TT* pMinValue = (TT*)((char*)&minValue + offset);
        TT* pMaxValue = (TT*)((char*)&maxValue + offset);

        if (*pMinValue == *pMaxValue &&  *pMinValue == 0)
        {
            *pMinValue = value;
            *pMaxValue = value;
        }
        if (value > *pMaxValue)
        {
            color = FOREGROUND_GREEN; 
            *pMaxValue = value;
        }
        else if (value < *pMinValue)
        {
            color = FOREGROUND_RED;
            *pMinValue = value;
        }

        SetConsoleTextAttribute(outHandle, color);
        std::cerr << setw(width) << value;
        SetConsoleTextAttribute(outHandle, FOREGROUND_WHITE);
    }


    template <typename TT>
    Value Update(TT value, size_t offset)
    {
        TT* pMinValue = (TT*)((char*)&minValue + offset);
        TT* pMaxValue = (TT*)((char*)&maxValue + offset);

        if (*pMinValue == *pMaxValue &&  *pMinValue == 0)
        {
            *pMinValue = value;
            *pMaxValue = value;
        }
        else if (value > *pMaxValue)
        {
            *pMaxValue = value;
        }
        else if (value < *pMinValue)
        {
            *pMinValue = value;
        }

        Value outValue((size_t)*pMinValue, (size_t)value, (size_t)*pMaxValue);
        return outValue;
    }
};

// ------------------------------------------------------------------------------------------------
template <typename TT>
ostream& FmtColumn(ostream& out, const TT value, unsigned int width)
{
    out << setw(width) << value << ", ";
    return out;
}

// ------------------------------------------------------------------------------------------------
// Return error < 0
//        hndCnt > 0
enum LOH_Status { eLOH_none = 0, eLOH_FAILED_BIND = -1, eLOH_FAILED_ALLOC = -2 };
const std::string sPidStr("PID");
const std::string sHndStr("Handles");
const std::string sThrStr("Threads");
const std::string sMPUStr("MaxPageUsage");
const std::string sPPCStr("PrivPageCnt");
const std::string sETMStr("ElapsedTm");
const std::string sIOStr("IOCnt");

ListOpenHandles::ObjTypeMap ListOpenHandles::sObjTypeMap;

int ListOpenHandles::ReportOpenFiles(
    ostream& out, 
    const Options& options, 
    bool showHeaders, 
    const char* preFix)
{
    int goodHndCnt = 0;

    // Enable special mode to allow access to system handles  (way cool).
    PsUtil::EnableDebugPrivilege();

#if 0
    HMODULE  hPsDll = GetModuleHandle("Psapi.dll"); 
    typedef DWORD (NTAPI *GetModuleFileNameEx_t)(HANDLE hProcess, HMODULE hModule,LPTSTR lpFilename, DWORD nSize);
    GetModuleFileNameEx_t GetModuleFileNameExA = (ZGetModuleFileNameEx_t)GetProcAddress(hPsDll, "GetModuleFileNameEx");
#endif
    
    if (!sNtApi.Ready())
    {
        out << "Sorry - ListOpenHandles failed to bind one or more functions. Unable to proceed.\n";
        return eLOH_FAILED_BIND;
    }

    NTSTATUS status = 0;
    ULONG retSize;

    ScopePtr<SYSTEM_HANDLE_LIST_INFORMATION> pSysHandleListInfo(sizeof(SYSTEM_HANDLE_INFORMATION) * 500);

    if (pSysHandleListInfo == NULL)
    {
        out << "Failed to allocate memory for required buffers.\n";
        return eLOH_FAILED_ALLOC;
    }

    while ((status = sNtApi.QuerySystemInformation(SystemHandleInformation, pSysHandleListInfo, pSysHandleListInfo.Size(), &retSize)) == 
        (NTSTATUS)STATUS_INFO_LENGTH_MISMATCH)
    {
        pSysHandleListInfo.Resize(pSysHandleListInfo.Size() + sizeof(SYSTEM_HANDLE_INFORMATION) * 500);
    }
    DWORD numSysHandles = pSysHandleListInfo->NumHandles;

    // Fallback guesses at object type mapping. Normally this list is not required and is provided
    // by quering the object type.  
  
    if (sObjTypeMap.size() < 20)
    {
        sObjTypeMap[1] = "type1";
        sObjTypeMap[2] = "Dir";
        sObjTypeMap[3] = "SymLnk";
        sObjTypeMap[4] = "Token";
        sObjTypeMap[5] = "Proc";
        sObjTypeMap[6] = "Thread";
        sObjTypeMap[7] = "Job";
        sObjTypeMap[8] = "type8";
        sObjTypeMap[9] = "Event";
        sObjTypeMap[10] = "type10";
        sObjTypeMap[11] = "Mutant";
        sObjTypeMap[12] = "Event";
        sObjTypeMap[13] = "Sema4";
        sObjTypeMap[14] = "Timer";
        sObjTypeMap[15] = "type15";
        sObjTypeMap[16] = "Sema16";
        sObjTypeMap[17] = "WinStn";
        sObjTypeMap[18] = "Desktop";
        sObjTypeMap[19] = "Section";
        sObjTypeMap[20] = "Regtry";
        sObjTypeMap[21] = "Port";
        sObjTypeMap[22] = "type22";
        sObjTypeMap[23] = "type23";
        sObjTypeMap[24] = "type24";
        sObjTypeMap[25] = "type25";
        sObjTypeMap[26] = "type26";
        sObjTypeMap[27] = "IoComp";
        sObjTypeMap[28] = "File";
        sObjTypeMap[29] = "WmiGuid";
    }

    static std::vector<bool> sNeedReadAccess;
    if (sNeedReadAccess.size() == 0)
    {
        sNeedReadAccess.resize(40, false);
        sNeedReadAccess[2] = sNeedReadAccess[20] = sNeedReadAccess[28] = true;
    }

    // EnumProcesses(...)

    ScopePtr<SYSTEM_PROCESS_INFORMATION_undoc> pSysProcList(sizeof(SYSTEM_PROCESS_INFORMATION_undoc) * 100);
    while ((status = sNtApi.QuerySystemInformation(SystemProcessInformation, pSysProcList, pSysProcList.Size(), &retSize)) 
        == STATUS_INFO_LENGTH_MISMATCH)
    {
        pSysProcList.Resize(retSize);
    }

    // Allocate initial space for several special buffers.
    ScopePtr<UNICODE_STRING>  pProcInfo(512);


    // Loop over process list, if any
    SYSTEM_PROCESS_INFORMATION_undoc* pSysProcInfo = pSysProcList;
    SYSTEM_PROCESS_INFORMATION_undoc* pNextSysProcInfo = pSysProcList;
    if (retSize >= sizeof(SYSTEM_PROCESS_INFORMATION_undoc))
    {
        Hnd processHandle;
        int szSysProcInfo = sizeof(_SYSTEM_PROCESS_INFORMATION_undoc);
        int x = offsetof(SYSTEM_PROCESS_INFORMATION_undoc, UniqueProcessId);
        int y = offsetof(SYSTEM_PROCESS_INFORMATION, UniqueProcessId);
        int z = sizeof(pSysProcInfo->ImageName);

        // Loop over process table.
        bool more = true;
        while (more)
        {
            pSysProcInfo = pNextSysProcInfo;
            more = (pSysProcInfo->NextEntryOffset != 0);
            pNextSysProcInfo = (SYSTEM_PROCESS_INFORMATION_undoc*)((char*)pSysProcInfo + pSysProcInfo->NextEntryOffset);

            if (pSysProcInfo->UniqueProcessId == 0)
                continue;       // skip processId zero

            // Close previously opened process handle.
            if (processHandle.IsValid())
                processHandle.Close();
            
            Pid_t pid = (Pid_t)pSysProcInfo->UniqueProcessId;

            // Try and open process
            processHandle = OpenProcess(PROCESS_QUERY_INFORMATION |PROCESS_VM_READ|PROCESS_DUP_HANDLE, FALSE, pid);
            if (processHandle == 0)
                continue;       // cannot open system process

            // Get Process name, QueryInformationoProcess enumerators:
            //      ProcessBasicInformation = 0,
            //      ProcessWow64Information = 26
            //      ProcessImageFileName    = 27;
            const PROCESSINFOCLASS ProcessImageFileName = (PROCESSINFOCLASS)27;
            ULONG procRetSize;
            status = sNtApi.QueryInformationProcessW(processHandle, ProcessImageFileName, pProcInfo, pProcInfo.Size(), &procRetSize);
            if (status == 0 || status == STATUS_INFO_LENGTH_MISMATCH )
            {
                const char* procName = WideToCptr(pProcInfo->Buffer, pProcInfo->Length);
                if (CompareProcessName(m_process, procName, pid) == false)
                    continue;   // if specific process requested, ignore others.

                
                char fullPath[MAX_PATH];
                int fnLen = GetModuleFileNameEx(processHandle, 0, fullPath, sizeof(fullPath));
                if (fnLen > 0)
                    m_procName = fullPath;
                else
                    m_procName = procName;
            }

            // Get process handle and thread counts, plus memory and creation time.
            GetProcInfo(pSysProcInfo);
            
            if (m_showModules)
            {
                out << sPidStr << ":" << pid << " Process:" << m_procName << std::endl;
                char szName[MAX_PATH];

                HMODULE hMods[1024];
                if (EnumProcessModules(processHandle, hMods, sizeof(hMods), &retSize))
                {
                    for (int i = 0; i != (retSize / sizeof(HMODULE)); i++ )
                    {
                        // TODO - remove sizeof(TCHAR)
                        if (GetModuleFileNameEx(processHandle, hMods[i], szName, sizeof(szName) / sizeof(TCHAR)))
                        {
                            out << "\tAddress," << hex << hMods[i] << dec << ",ModName," << szName << std::endl;
                        }
                    }
                }
            }

            /// Get Process specific handle counts by handle type
            /// Populates m_handleCountByType
            size_t accessErrorCount = GetHandleCounts(processHandle, pid, pSysHandleListInfo, out);

            if (m_showSummary)
            {
                HandleCountByType::const_iterator iter;

                if (showHeaders)
                {
                    std::ostringstream sout;
                    sout << preFix;
                    FmtColumn(sout, sPidStr, m_procInfoWidthMap[sPidStr]);

                    // Handle count headings
                    std::string objTypeStr;
                    for (iter = m_handleCountByType.begin() ; iter != m_handleCountByType.end(); ++iter)
                    {
                        objTypeStr = sObjTypeMap[iter->first]; 
                        if (objTypeStr.empty())
                            objTypeStr ="<unknown>"; 
                        FmtColumn(sout, objTypeStr, 7);
                    }
                   
                    FmtColumn(sout, "Total", 7);

                    // Process Info headings
                    FmtColumn(sout, sThrStr, m_procInfoWidthMap[sThrStr]);
                    FmtColumn(sout, sMPUStr, m_procInfoWidthMap[sMPUStr]);
                    FmtColumn(sout, sPPCStr, m_procInfoWidthMap[sPPCStr]);
                    FmtColumn(sout, sIOStr, m_procInfoWidthMap[sIOStr]);
                    FmtColumn(sout, sETMStr, m_procInfoWidthMap[sETMStr]);

                    sout << ", Name";
                    sout << std::endl;

                    std::string sline = sout.str();
                    out << sline;
                    showHeaders = false;
                }

                HandleCountPairByType* pPrevHandleCountByType = &m_pidHandleCountByType[pid];

                unsigned int totHnd = 0;
                if (options.m_colorize)
                {
                    UIntPair prevTotHnd;
                    std::cerr << preFix;
                    FmtColumn(std::cerr, pid, m_procInfoWidthMap[sPidStr]);
                    for (iter = m_handleCountByType.begin() ; iter != m_handleCountByType.end(); ++iter)
                    {
                        totHnd += iter->second;
                        UIntPair& prevPair = (*pPrevHandleCountByType)[iter->first];
                        prevTotHnd.first += prevPair.first;
                        prevTotHnd.second += prevPair.second;
                        ColorizeOutput((unsigned int)iter->second, prevPair, 7);
                        std::cerr << ", ";
                    }
                    ColorizeOutput(totHnd, prevTotHnd, 7);

                    ColorizeProcInfo(sThrStr);
                    ColorizeProcInfo(sMPUStr);
                    ColorizeProcInfo(sPPCStr);
                    ColorizeProcInfo(sIOStr);
                    FmtColumn(std::cerr, m_elapseTmStr, m_procInfoWidthMap[sETMStr]);
                    std::cerr << " " << m_procName << std::endl;
                }
                else
                {
                    std::ostringstream sout;
                    sout << preFix;
                    FmtColumn(sout, pid, m_procInfoWidthMap[sPidStr]);

                    for(iter = m_handleCountByType.begin() ; iter != m_handleCountByType.end(); ++iter)
                    {
                        totHnd += iter->second;
                        FmtColumn(sout, iter->second, 7);
                    }

                    FmtColumn(sout, totHnd, 7);

                    FmtColumn(sout, m_procInfoMap[sThrStr].curVal, m_procInfoWidthMap[sThrStr]);
                    FmtColumn(sout, m_procInfoMap[sMPUStr].curVal, m_procInfoWidthMap[sMPUStr]);
                    FmtColumn(sout, m_procInfoMap[sPPCStr].curVal, m_procInfoWidthMap[sPPCStr]);
                    FmtColumn(sout, m_procInfoMap[sIOStr].curVal, m_procInfoWidthMap[sIOStr]);
                    FmtColumn(sout, m_elapseTmStr, m_procInfoWidthMap[sETMStr]);
                    sout << m_procName;

                    std::string sline = sout.str();
                    out << sline << std::endl;
                }
            }

            if (m_showAll && accessErrorCount != 0)
            {
                std::ostringstream sout;
                sout << " access Error count= " << accessErrorCount << std::endl;
                std::string sline = sout.str();
                out << sline;
            }
        } 

        if (processHandle > 0)
            processHandle.Close();
    }

    return goodHndCnt;
}

// ------------------------------------------------------------------------------------------------
/// Get Process handle and thread count, memory usage and creation time.
 void ListOpenHandles::GetProcInfo(const void* pProcInfoVoid)
 {
    SYSTEM_PROCESS_INFORMATION_undoc* pSysProcInfo = (SYSTEM_PROCESS_INFORMATION_undoc*)pProcInfoVoid;

    typedef std::map<Pid_t, SysProcInfoPair> PidSystemInformation;
    static PidSystemInformation sPidSystemInformation;

    if (m_procInfoWidthMap.empty())
    {
        m_procInfoWidthMap[sPidStr] = 5;
        m_procInfoWidthMap[sHndStr] = 9;
        m_procInfoWidthMap[sThrStr] = 9;
        m_procInfoWidthMap[sMPUStr] = 13;
        m_procInfoWidthMap[sPPCStr] = 13;
        m_procInfoWidthMap[sIOStr]  = 13;
        m_procInfoWidthMap[sETMStr] = 10;
    }

    // Get and Format Process Creation Time.
    FILETIME    utcCreationFTm;
    FILETIME    locCreationFTm;
    SYSTEMTIME  creationSTm;

    utcCreationFTm.dwHighDateTime = pSysProcInfo->CreateTime.HighPart;
    utcCreationFTm.dwLowDateTime = pSysProcInfo->CreateTime.LowPart;
    FileTimeToLocalFileTime(&utcCreationFTm, &locCreationFTm);
    FileTimeToSystemTime(&locCreationFTm, &creationSTm);

    m_procDataStr[0] = m_procTimeStr[0] = '\0';

    // GetDateFormat(LOCALE_SYSTEM_DEFAULT, DATE_SHORTDATE, &sysTime, NULL, szLocalDate, sizeof(szLocalDate) );
    GetDateFormat(LOCALE_SYSTEM_DEFAULT, 0, &creationSTm, "MM'/'dd'/'yyyy", m_procDataStr, ARRAYSIZE(m_procDataStr) );
    GetTimeFormat(LOCALE_SYSTEM_DEFAULT, TIME_NOSECONDS, &creationSTm, "hh':'mm tt", m_procTimeStr, ARRAYSIZE(m_procTimeStr) );

    // Get System time.
    FILETIME utcSystemFTm;
    GetSystemTimeAsFileTime(&utcSystemFTm);

    LARGE_INTEGER* pCreationFtm = (LARGE_INTEGER*)&utcCreationFTm;
    LARGE_INTEGER* pSystemFtm = (LARGE_INTEGER*)&utcSystemFTm;
    LARGE_INTEGER elapsedTm; 
    elapsedTm.QuadPart = (pSystemFtm->QuadPart - pCreationFtm->QuadPart);
    SYSTEMTIME elapsesSTm;
    FileTimeToSystemTime((FILETIME*)&elapsedTm, &elapsesSTm);
    _snprintf(m_elapseTmStr, ARRAYSIZE(m_elapseTmStr), "%2u:%02u:%02u", 
            (elapsesSTm.wDay-1)*24 + elapsesSTm.wHour, elapsesSTm.wMinute, elapsesSTm.wSecond);

    Pid_t pid = (Pid_t)pSysProcInfo->UniqueProcessId;
    SysProcInfoPair* pPrevSysInfo = &sPidSystemInformation[pid];

    m_procInfoMap[sPidStr].Set(pid);
    m_procInfoMap[sHndStr] = pPrevSysInfo->Update(pSysProcInfo->HandleCount, OffsetSPI(HandleCount));
    m_procInfoMap[sThrStr] = pPrevSysInfo->Update(pSysProcInfo->NumberOfThreads, OffsetSPI(NumberOfThreads));
    m_procInfoMap[sMPUStr] = pPrevSysInfo->Update(pSysProcInfo->PeakPagefileUsage, OffsetSPI(PeakPagefileUsage));
    m_procInfoMap[sPPCStr] = pPrevSysInfo->Update(pSysProcInfo->PrivatePageCount, OffsetSPI(PrivatePageCount));

    ULONGLONG totalIoCount = 0;
    totalIoCount += pSysProcInfo->ReadOperationCount.QuadPart;
    totalIoCount += pSysProcInfo->WriteOperationCount.QuadPart;
    totalIoCount += pSysProcInfo->OtherOperationCount.QuadPart;
    totalIoCount += pSysProcInfo->ReadTransferCount.QuadPart;
    totalIoCount += pSysProcInfo->WriteTransferCount.QuadPart;
    totalIoCount += pSysProcInfo->OtherTransferCount.QuadPart;
    m_procInfoMap[sIOStr].Update(totalIoCount);
 }

 // ------------------------------------------------------------------------------------------------
 /// Get Handle count per handle type for a specific process
 /// Populate:  m_handleCountByType

 size_t ListOpenHandles::GetHandleCounts(
     HANDLE processHandle, 
     Pid_t pid, 
     const void* pProcInfoVoid,     
     ostream& out)
 {
    const SYSTEM_HANDLE_LIST_INFORMATION* pSysHandleListInfo = 
            (const SYSTEM_HANDLE_LIST_INFORMATION*)pProcInfoVoid;
   
    DWORD numSysHandles = pSysHandleListInfo->NumHandles;

    // Temporary work memory pointers. 
    ScopePtr<PUBLIC_OBJECT_BASIC_INFORMATION>  pObjBasic;
    ScopePtr<PUBLIC_OBJECT_TYPE_INFORMATION> pObjType;
    ScopePtr<PUBLIC_OBJECT_NAME_INFORMATION> pObjName;
    ScopePtr<FILE_NAME_INFORMATION> pFileInformation(512);
 
    // m_handleCountByType.clear();
    HandleCountByType::iterator iter;
    for (iter = m_handleCountByType.begin() ; iter != m_handleCountByType.end(); ++iter)
    {
        iter->second = 0;
    }


    size_t accessErrorCount = 0;
    size_t goodHndCount = 0;

    for (DWORD hndIdx = 0; hndIdx != numSysHandles; hndIdx++)
    {
        const SYSTEM_HANDLE_INFORMATION* pSysHandleInfo = &pSysHandleListInfo->Handles[hndIdx];
        Pid_t	processId = pSysHandleListInfo->Handles[hndIdx].ProcessId;
        USHORT  handle    = pSysHandleListInfo->Handles[hndIdx].Handle;

        if (processId != pid)
        {
            // Ignore file handles for other processes.
            continue;
        }

        // Get default object type string
        unsigned objType  = (unsigned)pSysHandleListInfo->Handles[hndIdx].ObjectTypeNumber;
        ObjTypeMap::iterator objTypeIter = sObjTypeMap.find(objType);
        if (objTypeIter == sObjTypeMap.end())
        {
            char* pStr = new char[20];
            sprintf(pStr, "%u", (int)objType);
            sObjTypeMap[objType] = pStr;
        }
        std::string objTypeStr = sObjTypeMap[objType]; 
        std::string objDesc;
        m_handleCountByType[objType]++;

        if (m_showSummary == false)
        {
            //  Some objects may hang when dup'd, so lets avoid the hang.
            // SYNCHRONIZE                      (0x00100000L)
            if (pSysHandleListInfo->Handles[hndIdx].ObjectTypeNumber == 28 &&
                (
                    pSysHandleListInfo->Handles[hndIdx].GrantedAccess == SYNCHRONIZE ||
                    pSysHandleListInfo->Handles[hndIdx].GrantedAccess == 0
                ))
            {
                continue;
            }

            HANDLE dupHnd = INVALID_HANDLE_VALUE;
            BOOL status = DuplicateHandle(processHandle, (HANDLE)handle, GetCurrentProcess(), &dupHnd,  0, FALSE, 0); 
            DWORD err = GetLastError();

            if (status == FALSE || dupHnd == 0)
            {
                if (err == EIO)
                    accessErrorCount++;

                continue;       // ERROR - failed to dup handle
            }

            // Some objects hang if you query them, so lets avoid the hang.
            if (pSysHandleListInfo->Handles[hndIdx].GrantedAccess != 0x0012019f)
            {
                // ----- Get Basic information
                DWORD retSize;
                status = sNtApi.QueryObjectW(dupHnd, ObjectBasicInformation, pObjBasic, pObjBasic.Size(), &retSize);
                if (status == 0 && m_showAll)
                {
                    if (pObjBasic->Attributes != 0)
                        out << " Attr:" << pObjBasic->Attributes;
                }

                // ----- Get Type information.
                status = sNtApi.QueryObjectW(dupHnd, ObjectTypeInformation, pObjType, pObjType.Size(), &retSize);
                if (status == STATUS_INFO_LENGTH_MISMATCH) 
                {
                    pObjType.Resize(retSize);
                    status = sNtApi.QueryObjectW(dupHnd, ObjectTypeInformation, pObjType, pObjType.Size(), &retSize);
                }
                if (status == 0)
                    objTypeStr = WideToCptr(pObjType->TypeName.Buffer, pObjType->TypeName.Length);

                // ----- Get handle description details
                status = sNtApi.QueryObjectW(dupHnd, (OBJECT_INFORMATION_CLASS)1, pObjName, pObjName.Size(), &retSize);
                if (status == STATUS_INFO_LENGTH_MISMATCH)
                {
                    pObjName.Resize(retSize);
                    status = sNtApi.QueryObjectW(dupHnd, (OBJECT_INFORMATION_CLASS)1, pObjName, pObjName.Size(), &retSize);
                }

                if (status == 0 && (m_showAll || m_showNet))
                {
                    objDesc = WideToCptr(pObjName->Name.Buffer, pObjName->Name.Length);
                }
                else
                {
                    IO_STATUS_BLOCK statusBlock;
                    // ZeroMemory(&statusBlock, sizeof(statusBlock));

                    NTSTATUS status = sNtApi.QueryInformationFileW(dupHnd, &statusBlock, pFileInformation, pFileInformation.Size(), FileNameInformation);
                    if (status == STATUS_SUCCESS && m_showFiles)
                    {
                        objDesc = WideToCptr(pFileInformation->FileName, pFileInformation->FileNameLength);
                    }
                }
            }

            // Display results
            if (/* objDesc.empty() == false || */ m_showAll || m_showNet)
            {
                bool isNet = false;
                std::ostringstream sout;

                sout << "\t" << setw(5) << (m_showHex? hex : dec) << handle << dec << ", " << setw(2) << objTypeStr;
                if (objDesc.empty() == false)
                    sout << ",\t " << objDesc.c_str();

                if (objDesc == "\\Device\\Tcp" || objDesc == "\\Device\\Udp")
                {
                    isNet = true;
                    sout << " [" << ConnectionDetails(dupHnd) << "]";
                }

                sout << std::endl;

                if (isNet || m_showAll)
                    out << sout.str();
            }

            CloseHandle(dupHnd);
        }

        goodHndCount++;
    }

    return accessErrorCount;
}

