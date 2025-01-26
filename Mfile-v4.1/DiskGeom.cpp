// ------------------------------------------------------------------------------------------------
//
// DiskGeom - list disk geometry information
//
// Author:  DLang   2009   
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

// ------------------------------------------------------------------------------------------------
DISK_GEOMETRY_EX* GetDiskGeometry(int nDriveNo, DWORD& geomSize)
{
	BOOL	ret;
	Hnd	    hDevice;
	TCHAR	pszDevice[256];

    // Struture is variable size, so guess at max size.
    const size_t sdiskGeomExSz = sizeof(DISK_GEOMETRY_EX) + 4069;
    geomSize = sdiskGeomExSz;
    char diskGeometryBuf[sdiskGeomExSz];
    DISK_GEOMETRY_EX* psDiskGeometryEx = (DISK_GEOMETRY_EX*)diskGeometryBuf;

	if (psDiskGeometryEx == NULL)
		return	NULL;
	::ZeroMemory(psDiskGeometryEx, geomSize);

	::_stprintf_s(pszDevice, 256, _T("\\\\.\\PhysicalDrive%d"), nDriveNo);
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
        IOCTL_DISK_GET_DRIVE_GEOMETRY_EX,
        NULL,
        0,
        psDiskGeometryEx,
        geomSize,
        &geomSize,
        NULL);

	if (ret == FALSE || geomSize < sizeof(DISK_GEOMETRY_EX))
	{
		ret = ::DeviceIoControl(
            hDevice,
            IOCTL_DISK_GET_DRIVE_GEOMETRY,
            NULL,
            0,
            &psDiskGeometryEx->Geometry,
            sizeof(DISK_GEOMETRY),
            &geomSize,
            NULL);

		if (ret == FALSE || geomSize != sizeof(DISK_GEOMETRY))
		{
			::ZeroMemory(psDiskGeometryEx, sizeof(DISK_GEOMETRY_EX));
			ret = FALSE;
		}
		else
		{
			psDiskGeometryEx->DiskSize.QuadPart = psDiskGeometryEx->Geometry.Cylinders.QuadPart;
			psDiskGeometryEx->DiskSize.QuadPart *= psDiskGeometryEx->Geometry.TracksPerCylinder;
			psDiskGeometryEx->DiskSize.QuadPart *= psDiskGeometryEx->Geometry.SectorsPerTrack;
			psDiskGeometryEx->DiskSize.QuadPart *= psDiskGeometryEx->Geometry.BytesPerSector;
		}
	}

	return	psDiskGeometryEx;
}

// ------------------------------------------------------------------------------------------------
bool DisplayDiskGeometry(const char* prefix, std::ostream& out)
{
    bool status = true;
    DISK_GEOMETRY_EX* pDiskGeom = NULL;
    DWORD size;

    std::string phyDriveList, driveLetters;
    FsUtil::GetPhysicalDrives(phyDriveList, driveLetters);
    // std::cout << "PhyDriveList:" << phyDriveList.c_str() << ", Letters:" << driveLetters.c_str() << std::endl;

    std::sort(phyDriveList.begin(), phyDriveList.end());
    phyDriveList.erase(std::unique(phyDriveList.begin(), phyDriveList.end()), phyDriveList.end());

    for (std::string::const_iterator iter = phyDriveList.begin(); iter != phyDriveList.end(); ++iter)
    {
        unsigned driveN = *iter - '0';
        pDiskGeom = GetDiskGeometry(driveN, size);
        if (pDiskGeom != NULL)
        {
            PDISK_PARTITION_INFO pDiskPart = DiskGeometryGetPartition(pDiskGeom);
            PDISK_DETECTION_INFO pDiskInfo = DiskGeometryGetDetect(pDiskGeom);

            out << prefix 
                << " Cylinders: " 
                << pDiskGeom->Geometry.Cylinders.QuadPart
                << ", MediaType: " 
                << pDiskGeom->Geometry.MediaType
                << ", Tracks/Cyl: " 
                << pDiskGeom->Geometry.TracksPerCylinder
                << ", Sectors/Trk: " 
                << pDiskGeom->Geometry.SectorsPerTrack
                << ", Bytes/Sector: " 
                << pDiskGeom->Geometry.BytesPerSector
                << std::endl;
        }
        else
        {
            status = false;
        }
    }

    return status;
}

