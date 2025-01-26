// ------------------------------------------------------------------------------------------------
// FileSystem utility class used to access file system information.
//
// Project: Mfile (Magnifile)
// Author:  Dennis Lang   Apr-2011
// https://landenlabs.com/
// ------------------------------------------------------------------------------------------------

#pragma once

#include <string>

typedef unsigned long Pid_t;

namespace PsUtil
{
    // Get Process name associated with PID, else return false.
    bool GetProcessName(Pid_t pid, std::string& procName);

    // Enable debug privilege so we can access/open other process' memory.
    void EnableDebugPrivilege();
}