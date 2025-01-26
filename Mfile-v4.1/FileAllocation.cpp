// ------------------------------------------------------------------------------------------------
//
// FileAllocation - List file cluster allocation information.
//
// Author:  DLang   2009  
// https://landenlabs.com
//
// ------------------------------------------------------------------------------------------------
                  
#define _CRT_SECURE_NO_WARNINGS

#include <iostream>
#include <iomanip>
#include <string>

// #define _WIN32_WINNT 0x0500 
#include <windows.h>
#include <winioctl.h>
#include <stdio.h>

#include "PsUtil.h"

// #include <ntddvol.h>
#define IOCTL_VOLUME_LOGICAL_TO_PHYSICAL \
        CTL_CODE( IOCTL_VOLUME_BASE, 8, METHOD_BUFFERED, FILE_ANY_ACCESS )
#define IOCTL_VOLUME_PHYSICAL_TO_LOGICAL \
        CTL_CODE( IOCTL_VOLUME_BASE, 9, METHOD_BUFFERED, FILE_ANY_ACCESS )

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

// ------------------------------------------------------------------------------------------------
int FileAllocation(const char* szFileName, bool showError, const char* prefix, std::ostream& out)
{
    char szDrive[] = "C:\\";
    char szVolumeHandleName[] = "\\\\.\\C:";
    char szVolumeName[128] = {0};
    char szFileSystemName[32] = {0};

    int iExtentsBufferSize = 1024;
    DWORD dwBytesReturned;
    DWORD dwSectorsPerCluster;
    DWORD dwBytesPerSector;
    DWORD dwNumberFreeClusters;
    DWORD dwTotalNumberClusters;
    DWORD dwVolumeSerialNumber;
    DWORD dwMaxFileNameLength;
    DWORD dwFileSystemFlags;
    DWORD dwClusterSizeInBytes;

    STARTING_VCN_INPUT_BUFFER StartingPointInputBuffer;
    // LARGE_INTEGER FileOffstFromBegDisk;
    LARGE_INTEGER  PhysicalOffsetReturnValue;

    // Get drive letter    
    if (szFileName[1] == ':')
    {
        szDrive[0] = szFileName[0];
        szVolumeHandleName[4] = szFileName[0];
    }

    HANDLE hFile;
    hFile = CreateFile(
        szFileName,
        0,      // GENERIC_READ | GENERIC_WRITE,
        7,      // FILE_SHARE_READ|FILE_SHARE_WRITE,
        NULL,
        OPEN_EXISTING,
        FILE_READ_ATTRIBUTES, // FILE_ATTRIBUTE_NORMAL,
        NULL);

    if (INVALID_HANDLE_VALUE == hFile)
    {
        if (showError)
            std::cerr << "Failed to open:" << szFileName << " Error:" << ShowError() << std::endl;
        return -1;
    }

    // Buffer to hold the extents info
    PRETRIEVAL_POINTERS_BUFFER lpRetrievalPointersBuffer =
        (PRETRIEVAL_POINTERS_BUFFER) malloc(iExtentsBufferSize);

    //StartingVcn field is the virtual cluster number in file
    // It is not a file offset

    // FSCTL_GET_RETRIEVAL_POINTERS acquires a list of virtual clusters from the
    // file system driver that make up the file and the related set of
    // Logical Clusters that represent the volume storage for these Virtual
    // Clusters.  On a heavliy fragmented volume, the file may not necessarily
    // be stored in contiguous storage locations.  Thus, it would be advisable
    // to follow the mapping of virtual to logical clusters "chain" to find
    // the complete physical layout of the file.

    // We want to start at the first virtual cluster ZERO   
    StartingPointInputBuffer.StartingVcn.QuadPart = 0;  
    if (!DeviceIoControl(
        hFile,
        FSCTL_GET_RETRIEVAL_POINTERS,
        &StartingPointInputBuffer,
        sizeof(STARTING_VCN_INPUT_BUFFER),
        lpRetrievalPointersBuffer,
        iExtentsBufferSize,
        &dwBytesReturned,
        NULL))
    {
        if (showError)
            std::cerr << "Failed to access file allocation pointers, Error:" << ShowError()  << std::endl;
        CloseHandle(hFile);
        return -1;
    }
    CloseHandle(hFile);

    if (!GetDiskFreeSpace(
        szDrive,
        &dwSectorsPerCluster,
        &dwBytesPerSector,
        &dwNumberFreeClusters,
        &dwTotalNumberClusters))
    {
        if (showError)
            std::cerr << "Failed to get disk space for:" << szDrive << ", Error:" << ShowError()  << std::endl;
        return -1;
    }
    dwClusterSizeInBytes = dwSectorsPerCluster * dwBytesPerSector;

    if (!GetVolumeInformation(
        szDrive,
        szVolumeName,
        128,
        &dwVolumeSerialNumber,
        &dwMaxFileNameLength,
        &dwFileSystemFlags,
        szFileSystemName,
        32))
    {
        if (showError)
            std::cerr << "Failed to get volume info for:" << szVolumeName << ", Error:" << ShowError()  << std::endl;
        return -1;
    }

    HANDLE volumeHandle = CreateFile(
        szVolumeHandleName,
        GENERIC_READ | GENERIC_WRITE,
        FILE_SHARE_READ|FILE_SHARE_WRITE,
        NULL,
        OPEN_EXISTING,
        FILE_ATTRIBUTE_NORMAL,
        NULL);

    if (volumeHandle == INVALID_HANDLE_VALUE)
        return -1;

    if (strcmp(szFileSystemName, "NTFS") == 0)
    {
        /* Volume logical offset */
        typedef struct _VOLUME_LOGICAL_OFFSET {
            LONGLONG    LogicalOffset;
        } VOLUME_LOGICAL_OFFSET;
        typedef VOLUME_LOGICAL_OFFSET   *PVOLUME_LOGICAL_OFFSET;

        /* Volume physical offset */
        typedef struct _VOLUME_PHYSICAL_OFFSET {
            ULONG       DiskNumber;
            LONGLONG    Offset;
        } VOLUME_PHYSICAL_OFFSET;
        typedef VOLUME_PHYSICAL_OFFSET  *PVOLUME_PHYSICAL_OFFSET;

        /* Volume physical offsets */
        typedef struct _VOLUME_PHYSICAL_OFFSETS {
            ULONG                   NumberOfPhysicalOffsets;
            VOLUME_PHYSICAL_OFFSET  PhysicalOffset[ANYSIZE_ARRAY];
        } VOLUME_PHYSICAL_OFFSETS;
        typedef VOLUME_PHYSICAL_OFFSETS *PVOLUME_PHYSICAL_OFFSETS;

        VOLUME_LOGICAL_OFFSET VolumeLogicalOffset;
        VOLUME_PHYSICAL_OFFSETS VolumePhysicalOffsets;
        LONGLONG logicalOffset = lpRetrievalPointersBuffer->Extents [0].Lcn.QuadPart * dwClusterSizeInBytes;

        VolumeLogicalOffset.LogicalOffset = logicalOffset;
        if (!DeviceIoControl(
            volumeHandle,
            IOCTL_VOLUME_LOGICAL_TO_PHYSICAL,
            &VolumeLogicalOffset,
            sizeof(VOLUME_LOGICAL_OFFSET),
            &VolumePhysicalOffsets,
            sizeof(VOLUME_PHYSICAL_OFFSETS),
            &dwBytesReturned,
            NULL))
        {
            CloseHandle(volumeHandle);
            if (showError)
                std::cerr << "Failed to get logical to physical mapping for:" 
                << szVolumeName << ", Error:" << ShowError()  << std::endl;
            return -1;
        }

        CloseHandle(volumeHandle);
        PhysicalOffsetReturnValue.QuadPart = 0;
        PhysicalOffsetReturnValue.QuadPart += VolumePhysicalOffsets.PhysicalOffset [0].Offset;
        if (PhysicalOffsetReturnValue.QuadPart != -1)
        {
            // std::cout << szFileName << " starts at " <<  PhysicalOffsetReturnValue.QuadPart << std::endl;
            // return 0;
        }

        out 
            << "#" <<  std::setw(3) << lpRetrievalPointersBuffer->ExtentCount 
            << " StartAt Phy:" << PhysicalOffsetReturnValue.QuadPart
            << " LogicalOff:" << logicalOffset
            << " Vcn:" <<  std::setw(3) << lpRetrievalPointersBuffer->StartingVcn.QuadPart 
            << " " << szFileName << std::endl;

        LARGE_INTEGER vcn = lpRetrievalPointersBuffer->StartingVcn;
        for (unsigned i = 0; i < lpRetrievalPointersBuffer->ExtentCount; i++)
        {
            out << prefix << "Len:" << lpRetrievalPointersBuffer->Extents[i].NextVcn.QuadPart - vcn.QuadPart;
            // out << " NextVcn:" << lpRetrievalPointersBuffer->Extents[i].NextVcn.QuadPart;
            out << " Lcn:" << lpRetrievalPointersBuffer->Extents[i].Lcn.QuadPart << std::endl;

            vcn.QuadPart =  lpRetrievalPointersBuffer->Extents[i].NextVcn.QuadPart;
        }
    }
    else 
    {
        if (showError)
            std::cerr << szFileSystemName << " File System NOT supported\n";
        return  -1;
    }
    return 0;
}

// ------------------------------------------------------------------------------------------------
void DisplayFileAllocation(
    const char* filePathOrPattern, 
    bool showError, 
    const char* prefix, 
    std::ostream& out)
{
    PsUtil::EnableDebugPrivilege();

    WIN32_FIND_DATA FileData;    // Data structure describes the file found
    HANDLE  hSearch;             // Search handle returned by FindFirstFile

    char path[MAX_PATH];
    strcpy(path, filePathOrPattern);

    hSearch = FindFirstFile(path, &FileData);
    bool isMore = (hSearch != INVALID_HANDLE_VALUE);
    while (isMore)
    {
        if (FileData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
        {
        }
        else
        {
            std::string fullPath = filePathOrPattern;
            size_t pos = fullPath.rfind('\\');
            if (pos != std::string::npos)
                fullPath.resize(pos+1);
            fullPath += FileData.cFileName;

            FileAllocation(fullPath.c_str(), showError, prefix, out);
        }
        isMore = (FindNextFile(hSearch, &FileData) != 0) ? true : false;
    }

    FindClose(hSearch);
}

