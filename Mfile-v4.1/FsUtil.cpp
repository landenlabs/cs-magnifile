// ------------------------------------------------------------------------------------------------
// FileSystem utility class used to access file system information.
//
// Project: NTFSfastFind
// Author:  Dennis Lang   Apr-2011
// https://landenlabs.com/
// ------------------------------------------------------------------------------------------------

#include "FsUtil.h"
#include "Hnd.h"

#include <iostream>
#include <sstream>

#define IOCTL_VOLUME_LOGICAL_TO_PHYSICAL \
        CTL_CODE( IOCTL_VOLUME_BASE, 8, METHOD_BUFFERED, FILE_ANY_ACCESS )
#define IOCTL_VOLUME_PHYSICAL_TO_LOGICAL \
        CTL_CODE( IOCTL_VOLUME_BASE, 9, METHOD_BUFFERED, FILE_ANY_ACCESS )

// ------------------------------------------------------------------------------------------------
char FsUtil::GetDriveLetter(const char* path)
{
    if (strcspn(path, ":") == 1)
        return path[0];

    char currentDir[MAX_PATH];
    GetCurrentDirectory(ARRAYSIZE(currentDir), currentDir);
    return currentDir[0];
}

// ------------------------------------------------------------------------------------------------
DWORD FsUtil::GetDriveAndPartitionNumber(const char* volumeName, unsigned& phyDrvNum, unsigned& partitionNum)
{
    Hnd volumeHandle = CreateFile(
        volumeName,                     // "\\\\.\\C:";
        GENERIC_READ | GENERIC_WRITE,
        FILE_SHARE_READ|FILE_SHARE_WRITE,
        NULL,
        OPEN_EXISTING,
        FILE_ATTRIBUTE_NORMAL,
        NULL);

    if (!volumeHandle.IsValid())
        return GetLastError();

    struct STORAGE_DEVICE_NUMBER 
    {
        DEVICE_TYPE DeviceType;
        ULONG       DeviceNumber;
        ULONG       PartitionNumber;
    };

    STORAGE_DEVICE_NUMBER storage_device_number;
    DWORD dwBytesReturned;

    if (!DeviceIoControl(
            volumeHandle,
            IOCTL_STORAGE_GET_DEVICE_NUMBER,
            NULL,
            0,
            &storage_device_number,
            sizeof(storage_device_number),
            &dwBytesReturned,
            NULL))
    {
        return GetLastError();
    }

    phyDrvNum = storage_device_number.DeviceNumber;
    partitionNum = storage_device_number.PartitionNumber - 1;   // appears to one based, so shift down one.

    return ERROR_SUCCESS;
}

// ------------------------------------------------------------------------------------------------
/// Get Physical drive numbers
///
/// Note - see GetLogicalDriveStrings()
/// wmic logicaldisk get caption,description,drivetype,providername,volumename
/// wmic diskdrive list brief /format:list
/*
0   DRIVE_UNKNOWN       The drive type cannot be determined.
1   DRIVE_NO_ROOT_DIR   The root path is invalid; for example, there is no volume mounted at the specified path.
2   DRIVE_REMOVABLE     The drive has removable media; for example, a floppy drive, thumb drive, or flash card reader.
3   DRIVE_FIXED         The drive has fixed media; for example, a hard disk drive or flash drive.
4   DRIVE_REMOTE        The drive is a remote (network) drive.
5   DRIVE_CDROM         The drive is a CD-ROM drive.
6   DRIVE_RAMDISK       The drive is a RAM disk.
*/

void FsUtil::GetPhysicalDrives(std::string& driveNumbers, std::string& driveLetters, DWORD driveType)
{
    // driveNumbers.clear();
    // driveLetters.clear();

#if 1
    char logicalDrvLetters[64];
    DWORD logDrvLen = GetLogicalDriveStrings(ARRAYSIZE(logicalDrvLetters), logicalDrvLetters);
    char logicalDrvPath[] = "\\\\.\\C:";
    for (unsigned idx = 0; idx < logDrvLen; idx++)
    {
        logicalDrvPath[4] = logicalDrvLetters[idx];
        unsigned phyDrvNum;
        unsigned partitionNum;
        if (ERROR_SUCCESS == GetDriveAndPartitionNumber(logicalDrvPath, phyDrvNum, partitionNum) 
            && partitionNum < -2
            && (driveType == 0 || GetDriveType(logicalDrvPath+4) == driveType))
        {
            driveNumbers += '0' + phyDrvNum;
            driveLetters += logicalDrvLetters[idx];
        }
    }

#else
    char volumeName[MAX_PATH] = "";
    HANDLE volumeHnd = FindFirstVolume(volumeName, ARRAYSIZE(volumeName));
    bool more = volumeHnd != INVALID_HANDLE_VALUE;
    std::vector<char> logicalDrv;

    while (more)
    {
        unsigned volLen = strlen(volumeName);
        if (volLen != 0 && volumeName[volLen - 1] == '\\')
        {
            DWORD nameLen = MAX_PATH+1;
            bool moreVolPath = true;
            char drvLetter = '?';
            while (moreVolPath)
            {
                logicalDrv.resize(nameLen);
                BOOL ok = GetVolumePathNamesForVolumeName(volumeName, &logicalDrv[0], logicalDrv.size(), &nameLen);
                moreVolPath = !ok && (GetLastError() == ERROR_MORE_DATA);
                if (ok && nameLen >= 3 && GetDriveType(&logicalDrv[0]) == DRIVE_FIXED)    // nameLen = length + 2 (nulls)
                {
                    drvLetter = logicalDrv[0];   // Save leading drive letter, ex: "c:\"
                }
            }
            volumeName[volLen-1] = '\0';
            char deviceList[MAX_PATH] = "";
            DWORD devCnt = QueryDosDevice(volumeName+4, deviceList, ARRAYSIZE(deviceList));
            if (devCnt > 24) // devCnt includes length + 2 (nulls)
            {
                driveNumbers += deviceList[22]; // Get number after "\Device\HarddiskVolume"
                driveLetters += drvLetter;
            }
            volumeName[volLen-1] = '\\';
        }
        more = FindNextVolume(volumeHnd, volumeName, ARRAYSIZE(volumeName));
    }

    FindVolumeClose(volumeHnd);
#endif
}

// ------------------------------------------------------------------------------------------------
/// This function is from vinoj kumar's article forensic in codeguru

DWORD FsUtil::GetLogicalDrives(const char* phyDrv, DiskInfoList& diskInfoList, FsBits whichFs)
{
    int patIdx, nRet;

    Hnd hDrive = CreateFile(phyDrv, GENERIC_READ, FILE_SHARE_READ|FILE_SHARE_WRITE, 0, OPEN_EXISTING, 0, 0);
    if (hDrive == INVALID_HANDLE_VALUE)
        return GetLastError();

    DWORD dwBytes;
    const unsigned sSectorSize = 512;
    BYTE szSector[sSectorSize];
    nRet = ReadFile(hDrive, szSector, sSectorSize, &dwBytes, 0);
    if (!nRet)
    {
        return GetLastError();
    }

    DWORD dwMainPrevRelSector = 0;
    DWORD dwPrevRelSector     = 0;

    Partition*  pPartition = (Partition*)(szSector + 0x1BE);
    // int partSize = sizeof(Partition);
    DiskInfo diskInfo;

    for (patIdx = 0; patIdx < 4; patIdx++) /// scanning partitions in the physical disk
    {
        diskInfo.wCylinder    = pPartition->chCylinder;
        diskInfo.wHead        = pPartition->chHead;
        diskInfo.wSector      = pPartition->chSector;
        diskInfo.dwNumSectors = pPartition->dwNumberSectors;
        diskInfo.wType        = 
                ((pPartition->chType == PART_EXTENDED) || (pPartition->chType == PART_DOSX13X)) ? 
                EXTENDED_PART:BOOT_RECORD;

        if ((pPartition->chType == PART_EXTENDED) || (pPartition->chType == PART_DOSX13X))
        {
            dwMainPrevRelSector			    = pPartition->dwRelativeSector;
            diskInfo.dwNTRelativeSector	= dwMainPrevRelSector;
        }
        else
        {
            diskInfo.dwNTRelativeSector = dwMainPrevRelSector + pPartition->dwRelativeSector;
        }

        if (diskInfo.wType == EXTENDED_PART)
            break;

        if (pPartition->chType == 0)
            break;

        switch (pPartition->chType)
        {
        case PART_DOS2_FAT: // FAT12
            if ((whichFs & eFsDOS12) != 0)
                diskInfoList.push_back(diskInfo);
            break;
        case PART_DOSX13:
        case PART_DOS4_FAT:
        case PART_DOS3_FAT:
            if ((whichFs & eFsDOS16) != 0)
                diskInfoList.push_back(diskInfo);
            break;
        case PART_DOS32X:
        case PART_DOS32:
            if ((whichFs & eFsDOS32) != 0)
                diskInfoList.push_back(diskInfo);
            break;
        case PART_NTFS:  
            if ((whichFs & eFsNTFS) != 0)
                diskInfoList.push_back(diskInfo);
            break;
        default: // Unknown
            if (whichFs == eFsALL)
                diskInfoList.push_back(diskInfo);
            break;
        }

        pPartition++;
    }

    if (patIdx == 4)
        return ERROR_SUCCESS;

    for (int LogiHard = 0; LogiHard < 50; LogiHard++) // scanning extended partitions
    {
        if (diskInfo.wType == EXTENDED_PART)
        {
            LARGE_INTEGER n64Pos;

            n64Pos.QuadPart = ((LONGLONG) diskInfo.dwNTRelativeSector) * 512;

            nRet = SetFilePointer(hDrive, n64Pos.LowPart, &n64Pos.HighPart, FILE_BEGIN);
            if (nRet == 0xffffffff)
                return GetLastError();;

            dwBytes = 0;

            nRet = ReadFile(hDrive, szSector, 512, (DWORD *) &dwBytes, NULL);
            if (!nRet)
                return GetLastError();

            if (dwBytes != 512)
                return ERROR_READ_FAULT;

            pPartition = (Partition*) (szSector+0x1BE);

            for (patIdx = 0; patIdx < 4; patIdx++)
            {
                diskInfo.wCylinder = pPartition->chCylinder;
                diskInfo.wHead = pPartition->chHead;
                diskInfo.dwNumSectors = pPartition->dwNumberSectors;
                diskInfo.wSector = pPartition->chSector;
                diskInfo.dwRelativeSector = 0;
                diskInfo.wType = ((pPartition->chType == PART_EXTENDED) || (pPartition->chType == PART_DOSX13X)) ? EXTENDED_PART:BOOT_RECORD;

                if ((pPartition->chType == PART_EXTENDED) || (pPartition->chType == PART_DOSX13X))
                {
                    dwPrevRelSector = pPartition->dwRelativeSector;
                    diskInfo.dwNTRelativeSector = dwPrevRelSector + dwMainPrevRelSector;
                }
                else
                {
                    diskInfo.dwNTRelativeSector = dwMainPrevRelSector + dwPrevRelSector + pPartition->dwRelativeSector;
                }

                if (diskInfo.wType == EXTENDED_PART)
                    break;

                if (pPartition->chType == 0)
                    break;

                switch(pPartition->chType)
                {
                case PART_DOS2_FAT: // FAT12
                    if ((whichFs & eFsDOS12) != 0)
                        diskInfoList.push_back(diskInfo);
                    break;
                case PART_DOSX13:
                case PART_DOS4_FAT:
                case PART_DOS3_FAT:
                    if ((whichFs & eFsDOS16) != 0)
                        diskInfoList.push_back(diskInfo);
                    break;
                case PART_DOS32X:
                case PART_DOS32:
                    if ((whichFs & eFsDOS32) != 0)
                        diskInfoList.push_back(diskInfo);
                    break;
                case 7: // NTFS
                    if ((whichFs & eFsNTFS) != 0)
                        diskInfoList.push_back(diskInfo);
                    break;
                default: // Unknown
                    break;
                }

                pPartition++;
            }

            if (patIdx == 4)
                break;
        }
    }

    return ERROR_SUCCESS;
}

// ------------------------------------------------------------------------------------------------
// Convert file or drive path to a physical drive number.

bool FsUtil::GetDriveNumber(const char* path, unsigned& phyDrvNum)
{
    char driveLetter = GetDriveLetter(path);
    unsigned partitionNum = 0;

    char volumePath[] = "\\\\.\\C:";
    volumePath[4] = (char)towupper(driveLetter);

    DWORD error = GetDriveAndPartitionNumber(volumePath, phyDrvNum, partitionNum);
    // if (error != ERROR_SUCCESS) std::cerr << "Error " << ErrorMsg(error).c_str() << std::endl;
   
    return (error == ERROR_SUCCESS);
}

// ------------------------------------------------------------------------------------------------
// Convert error number to error message.

std::string FsUtil::ErrorMsg(DWORD error)
{
	char *lpMsgBuf;

	FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER|FORMAT_MESSAGE_FROM_SYSTEM,
        NULL,
        error,
        MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
        (LPTSTR) &lpMsgBuf,
        0, 
        NULL);

    std::string msg(lpMsgBuf);
	LocalFree(lpMsgBuf);
    return msg;
}