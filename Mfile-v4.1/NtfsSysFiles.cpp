// ------------------------------------------------------------------------------------------------
// 
// NtfsSysFiles
//
// Author:  DLang   2009 
// https://landenlabs.com
//
// ------------------------------------------------------------------------------------------------

#define _CRT_SECURE_NO_WARNINGS
#include <iostream>
#include <iomanip>
#include <assert.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <time.h>
#include <io.h>

using  namespace std;

#include <windows.h>
#include <winioctl.h>

#include "PsUtil.h"

// ------------------------------------------------------------------------------------------------
static char* ShowError(void) 
{ 
    static char szReturn[128] = {0};
    char *lpMsgBuf; 
    FormatMessage( 
        FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM, 
        NULL, 
        GetLastError(), 
        MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), 
        (LPTSTR) &lpMsgBuf, 
        0, 
        NULL 
        ); 
    memset(szReturn,0,  sizeof szReturn);
    strcpy(szReturn, lpMsgBuf);
    LocalFree( lpMsgBuf ); 
    return (char *) szReturn;
}


/*
    NTFS special files:

    $AttrDef
    $BadClus
    $BadClus:$Bad
    $BitMap
    $Boot
    $LogFile
    $Mft
    $MftMirr
    $Secure
    $UpCase
    $Volume
    $Extend
    $Extend\$Reparse
    $Extend\$ObjId
    $Extend\$UsnJrnl
    $Extend\$UsnJrnl:$Max
    $Extend\$Quota

*/


/*
 Registry special files:

 user.dat and system.dat

 doc\<user>\Local Settings\Application Data\Microsoft\Windows\UsrClass.dat
 doc\<user>\NtUser.dat
 doc\LocalService\Local Settings\Application Data\Microsoft\Windows\UsrClass.dat
 doc\LocalService\NtUser.dat
 doc\NetworkService\Local Settings\Application Data\Microsoft\Windows\UsrClass.dat
 doc\NetworkService\NtUser.dat

 c:\windows\system32\config\default
 c:\windows\system32\config\SAM
 c:\windows\system32\config\SECUIRTY
 c:\windows\system32\config\software
 c:\windows\system32\config\system

 */

extern int FileAllocation(const char* szFileName, bool showError, const char* prefix, std::ostream& out);

// ------------------------------------------------------------------------------------------------
// Display allocation map for MFT file
bool NtfsSysFiles(bool showError, const char* prefix, std::ostream& out)
{
    const char* sNtfsFiles[] = 
    {
    "$AttrDef",
    "$BadClus",
    "$BadClus:$Bad",
    "$BitMap",
    "$Boot",
    "$LogFile",
    "$Mft",
    "$MftMirr",
    "$Secure",
    "$UpCase",
    "$Volume",
    "$Extend",
    "$Extend\\$Reparse",
    "$Extend\\$ObjId",
    "$Extend\\$UsnJrnl",
    "$Extend\\$UsnJrnl:$Max",
    "$Extend\\$Quota",
    };

    const char filename[] = "c:\\$Mft";

    PsUtil::EnableDebugPrivilege();

    // FileAllocation(filename, true, "\t");

    std::string drive = "c:\\";
    out << "\n---- NTFS File allocation drive " << drive << std::endl;
    for (unsigned idx = 0; idx != ARRAYSIZE(sNtfsFiles); idx++)
    {
        std::string filePath = drive;
        filePath += sNtfsFiles[idx];
        FileAllocation(filePath.c_str(), showError, prefix, out);
    }

    return true;
}