// ------------------------------------------------------------------------------------------------
// Windows (NT) API
//
// Project: Mfile (Magnifile)
// Author:  Dennis Lang   Apr-2011
// https://landenlabs.com/
// ------------------------------------------------------------------------------------------------

#pragma once

#include <Windows.h>

class NtApi
{
public:
    typedef NTSTATUS (NTAPI *NtQuerySystemInformation_t)(UINT, PVOID, ULONG, PULONG); 
    NtQuerySystemInformation_t QuerySystemInformation;

    typedef NTSTATUS (NTAPI *NtQueryInformationFile_t)(HANDLE, PIO_STATUS_BLOCK, PVOID, ULONG, __FILE_INFORMATION_CLASS); 
    NtQueryInformationFile_t QueryInformationFileW;

    typedef NTSTATUS (NTAPI *NtQueryObject_t)(HANDLE, OBJECT_INFORMATION_CLASS, PVOID, ULONG, PULONG); 
    NtQueryObject_t QueryObjectW;

    typedef NTSTATUS (NTAPI *NtQueryInformationProcess_t)(HANDLE ProcessHandle, PROCESSINFOCLASS ProcessInformationClass,
            PVOID ProcessInformation, ULONG ProcessInformationLength, PULONG ReturnLength OPTIONAL);
    NtQueryInformationProcess_t QueryInformationProcessW;

    NtApi()
    { 
        // Map undocumented or special function calls.
        HMODULE  hNtDll = GetModuleHandle("ntdll.dll"); 
        QuerySystemInformation = (NtQuerySystemInformation_t)GetProcAddress(hNtDll, "NtQuerySystemInformation"); 
        QueryInformationFileW = (NtQueryInformationFile_t)GetProcAddress(hNtDll, "NtQueryInformationFile"); 
        QueryObjectW = (NtQueryObject_t)GetProcAddress(hNtDll, "NtQueryObject"); 
        QueryInformationProcessW = (NtQueryInformationProcess_t)GetProcAddress(hNtDll, "NtQueryInformationProcess"); 
    } 

    bool Ready() const
    {
        return (QuerySystemInformation != NULL &&
                QueryInformationFileW != NULL &&
                QueryObjectW != NULL &&
                QueryInformationProcessW != NULL);
    }
};

#if 0
enum SystemInformationClass 
{
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
};

#endif