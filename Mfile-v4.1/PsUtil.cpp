// ------------------------------------------------------------------------------------------------
// FileSystem utility class used to access file system information.
//
// Project: Mfile (Magnifile)
// Author:  Dennis Lang   Apr-2011
// https://landenlabs.com/
// ------------------------------------------------------------------------------------------------

#include "PsUtil.h"
#include "Hnd.h"
#include <iostream>

// Internals
#include <psapi.h>

// Libraries
#pragma comment(lib, "psapi.lib")

// ------------------------------------------------------------------------------------------------
bool PsUtil::GetProcessName(Pid_t pid, std::string& procNameStr)
{
    if (pid == 0 || pid == 4)
    {
        procNameStr = "System";
        return true;
    }

    // Get a handle to the process.
    // Need DebugPrivilege to open most processes.
    Hnd hProcess = OpenProcess(PROCESS_QUERY_INFORMATION|PROCESS_VM_READ, FALSE, pid);
    DWORD err = GetLastError();

    // Get the process name 
    if (hProcess.IsValid())
    {
        char procName[MAX_PATH];
        int fnLen = GetModuleFileNameEx(hProcess, 0, procName, sizeof(procName));
        if (fnLen > 0)
        {
            procNameStr = procName;
            return true;
        }
        err = GetLastError();

        HMODULE hMod;
        DWORD cbNeeded;
        if (EnumProcessModules(hProcess, &hMod, sizeof(hMod), &cbNeeded) )
        {
            GetModuleBaseName(hProcess, hMod, procName, ARRAYSIZE(procName));
            procNameStr = procName;
            return true;                 
        }
        err = GetLastError();

        // QueryFullProcessImageName()
    }

    return false;
}

// ------------------------------------------------------------------------------------------------

void PsUtil::EnableDebugPrivilege()
{
    HANDLE hToken;
    TOKEN_PRIVILEGES tokenPriv;
    LUID luidDebug;

    if (OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES, &hToken) != FALSE) 
    {
        if (LookupPrivilegeValue(NULL, SE_DEBUG_NAME, &luidDebug) != FALSE)
        {
            tokenPriv.PrivilegeCount           = 1;
            tokenPriv.Privileges[0].Luid       = luidDebug;
            tokenPriv.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
            AdjustTokenPrivileges(hToken, FALSE, &tokenPriv, sizeof(tokenPriv), NULL, NULL);
        }
    }
}
