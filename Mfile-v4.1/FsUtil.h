// ------------------------------------------------------------------------------------------------
// FileSystem utility class used to access file system information.
//
// Project: Mfile (Magnifile)
// Author:  Dennis Lang   Apr-2011
// https://landenlabs.com/
// ------------------------------------------------------------------------------------------------

#pragma once

#include <Windows.h>
#include <vector>

#pragma pack(push, curAlignment)
#pragma pack(1)

struct DiskInfo
{
    WORD	wCylinder;
    WORD	wHead;
    WORD	wSector;
    DWORD	dwNumSectors;
    WORD	wType;
    DWORD	dwRelativeSector;
    DWORD	dwNTRelativeSector;
    DWORD	dwBytesPerSector;
};

struct Partition
{
    BYTE	chBootInd;
    BYTE	chHead;
    BYTE	chSector;
    BYTE	chCylinder;
    BYTE	chType;
    BYTE	chLastHead;
    BYTE	chLastSector;
    BYTE	chLastCylinder;
    DWORD	dwRelativeSector;
    DWORD	dwNumberSectors;
};
#pragma pack(pop, curAlignment)

const WORD PART_TABLE = 0;
const WORD BOOT_RECORD = 1;
const WORD EXTENDED_PART = 2;

const BYTE PART_UNKNOWN = 0x00;     // Unknown.  
const BYTE PART_DOS2_FAT = 0x01;	// 12-bit FAT.  
const BYTE PART_DOS3_FAT = 0x04;	// 16-bit FAT. Partition smaller than 32MB.  
const BYTE PART_EXTENDED = 0x05;	// Extended MS-DOS Partition.  
const BYTE PART_DOS4_FAT = 0x06;	// 16-bit FAT. Partition larger than or equal to 32MB.  
const BYTE PART_NTFS = 0x07;	    // NTFS
const BYTE PART_DOS32 = 0x0B;		// 32-bit FAT. Partition up to 2047GB.  
const BYTE PART_DOS32X = 0x0C;		// Same as PART_DOS32(0Bh), but uses Logical Block Address Int 13h extensions.  
const BYTE PART_DOSX13 = 0x0E;		// Same as PART_DOS4_FAT(06h), but uses Logical Block Address Int 13h extensions.  
const BYTE PART_DOSX13X = 0x0F;		// Same as PART_EXTENDED(05h), but uses Logical Block Address Int 13h extensions.  


namespace FsUtil
{
    // Return drive letter of file path or letter of current path.
    char GetDriveLetter(const char* path);

    // Get disk informatino list of available partitions.
    enum FsBits { eFsNone = 0, eFsDOS12 = 1, eFsDOS16 = 2, eFsDOS32 = 4, eFsNTFS = 8, eFsALL=15 };
    typedef std::vector<DiskInfo> DiskInfoList;
    DWORD GetLogicalDrives(const char* phyDrv, DiskInfoList& diskInfoList, FsBits whichFs);

    // Get list of physical driver numbers
    void GetPhysicalDrives(std::string& driveNumbers, std::string& driveLetters, DWORD driveType = DRIVE_FIXED);

    // Get physical disk number and partition number for volume.
    // Pass volume name  "\\\\.\\C:"  which is \\.\C:
    DWORD GetDriveAndPartitionNumber(const char* volumeName, unsigned& phyDrvNum, unsigned& partitionNum);

    // Convert file or drive path to a physical drive number.
    bool GetDriveNumber(const char* path, unsigned& phyDrvNum);
   
    // Convert error number to error message.
    std::string ErrorMsg(DWORD error);
};

