// ------------------------------------------------------------------------------------------------
//
// DiskAttr - list disk attribute information
//
// Author:  DLang   2014  
// https://landenlabs.com
//
// ------------------------------------------------------------------------------------------------

#include "Hnd.h"
#include "FsUtil.h"

#include <atlstr.h>
#include <winioctl.h>
#include <iostream>
#include <algorithm>

using namespace std;

typedef struct _GET_DISK_ATTRIBUTES {
  DWORD     Version;
  DWORD     Reserved1;
  DWORDLONG Attributes;
} GET_DISK_ATTRIBUTES, *PGET_DISK_ATTRIBUTES;

const DWORDLONG DISK_ATTRIBUTE_OFFLINE  = 0x0000000000000001;
const DWORDLONG DISK_ATTRIBUTE_READ_ONLY = 0x0000000000000002;
const DWORD IOCTL_DISK_GET_DISK_ATTRIBUTES = 0x700f0;

// ------------------------------------------------------------------------------------------------
_GET_DISK_ATTRIBUTES* GetDiskAttributes(char driveLetter, DWORD& readSize, std::vector<char>& buffer)
{
	BOOL	ret;
	Hnd	    hDevice;
	TCHAR	pszDevice[256];

    size_t recSize = sizeof(_GET_DISK_ATTRIBUTES);
    buffer.resize(recSize);
    _GET_DISK_ATTRIBUTES* psDiskAttributes = (_GET_DISK_ATTRIBUTES*)buffer.data();

	if (psDiskAttributes == NULL)
		return	NULL;
	::ZeroMemory(psDiskAttributes, recSize);

    ::_stprintf_s(pszDevice, 256, _T("\\\\.\\%c:"), driveLetter);
	// ::_stprintf_s(pszDevice, 256, _T("\\\\.\\PhysicalDrive%d"), nDriveNo);
	hDevice = ::CreateFile(pszDevice, 
            0,      
            FILE_SHARE_READ | FILE_SHARE_WRITE, 
            NULL, 
            OPEN_EXISTING, 
            0, 
            NULL);
	
    if (hDevice == INVALID_HANDLE_VALUE)
    {
		return	NULL;
    }

	ret = ::DeviceIoControl(
        hDevice,
        IOCTL_DISK_GET_DISK_ATTRIBUTES,
        NULL,
        0,
        psDiskAttributes,
        recSize,
        &readSize,
        NULL);

	if (ret == FALSE || readSize < recSize)
	{
		return NULL;
	}

	return	psDiskAttributes;
}

// ------------------------------------------------------------------------------------------------
template <typename TT>
bool HasBit(TT value, TT bit)
{
    return (0 != (value & bit));
}

// ------------------------------------------------------------------------------------------------
bool DisplayDiskAttributes(const char* prefix, std::ostream& out)
{
    bool status = true;
    _GET_DISK_ATTRIBUTES* pDiskAttributes = NULL;
    DWORD size;

    std::vector<char> buffer;
    std::string phyDriveList, driveLetters;

    out << "Values do not appear to be accurate, use diskpart tool to get better results\n";
    FsUtil::GetPhysicalDrives(phyDriveList, driveLetters, DRIVE_UNKNOWN);

    // std::sort(phyDriveList.begin(), phyDriveList.end());
    // phyDriveList.erase(std::unique(phyDriveList.begin(), phyDriveList.end()), phyDriveList.end());

    // for (std::string::const_iterator iter = phyDriveList.begin(); iter != phyDriveList.end(); ++iter)
    for (unsigned idx = 0; idx != phyDriveList.size(); idx++)
    {
        unsigned driveN = phyDriveList[idx] - '0';
        pDiskAttributes = GetDiskAttributes(driveLetters[idx], size, buffer);
        if (pDiskAttributes != NULL)
        {
            DWORDLONG a =  pDiskAttributes->Attributes;

            out << prefix 
                << driveLetters[idx]
                << " Drive#:" << driveN
                << " Attributes: " << (HasBit(pDiskAttributes->Attributes, DISK_ATTRIBUTE_OFFLINE) ? "OffLine" : "OnLine")
                << (HasBit(pDiskAttributes->Attributes, DISK_ATTRIBUTE_READ_ONLY) ? ", ReadOnly" : ", ReadWrite")
                <<   pDiskAttributes->Version << pDiskAttributes->Reserved1 << pDiskAttributes->Attributes
                << std::endl;
        }
        else
        {
            out << prefix 
                << driveLetters[idx]
                << " Drive#:" << driveN
                << " Failed to read attributes"
                << std::endl;

            status = false;
        }
    }

    return status;
}

