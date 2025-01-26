// ------------------------------------------------------------------------------------------------
//
// DiskSmart - List disk drive's 'S.M.A.R.T.' information
//
// Author:  DLang   2009  
// https://landenlabs.com
//
// ------------------------------------------------------------------------------------------------
    
#define _CRT_SECURE_NO_WARNINGS

/*
 Thanks to...

 http://objectmix.com/c/403686-determining-size-physical-device-2.html

If you wanted to compute the drive size from all that info, you can
theoretically multiply the wBytesPerSector member of that ATA_ID_SECTOR
structure by ulTotalAddressableSectors. However, on my machine,
wBytesPerSector is set to 0. Also I am not sure if bytes per sector is
constant over the entire drive... I may be wrong about that. If it's 0, I
have had some success at using the sector size returned by the Window's
GetDiskFreeSpace function instead.

For example, if the sector size is 512 bytes (determined either by
wBytesPerSector or GetDiskFreeSpace) and ulTotalAddressableSectors is
195371568; then:

512 * 195,371,568 = 100,030,242,816

That's 100030242816 bytes; so in gigabytes:

100030242816 / (1024 * 1024 * 1024) = 93.16 GB

And when I check that against the physical drive size reported by the
Windows Disk Management Utility, I see that it also has the number "93.16
GB", precisely, in the little text box, and that makes me happy.

There is a more full-featured example on CodeProject here:

http://www.codeproject.com/KB/winsdk...formation.aspx

I tried to find the original documentation I was looking at but I could not.
It's really frustrating -- I remember having a really great set of
documentation about ATA commands in front of me, that's where a lot of the
values and structures in my code snippet came from. If I find it I will post
it here (and eventually I will find it, the only reason I was doing this to
begin with was a project that I had to put on the back burner that I will
eventually take up again). Microsoft does have some documentation here,
which could be used as a starting point:

http://msdn2.microsoft.com/en-us/library/ms803645.aspx

Hmm, it looks like some info here, including info about the magic numbers
you have to put in the ATA command structure:

http://www.dewassoc.com/kbase/hard_d...technology.htm

Info on smart attribute codes is here:

http://en.wikipedia.org/wiki/Self-Mo...ing_Technology

I am sorry I could not be more clear. Also I apologize for not knowing where
the documentation I was referring to was located, I really wish I could
remember. In the sample code I posted, the DiskSmart class doesn't actually do
anything, but after the constructor is called the drive is open and smart is
enabled so you could theoretically start issuing other smart commands and
reading drive attributes.

In any case, IOCTL_DISK_GET_DRIVE_GEOMETRY_EX is much more straightforward,
especially if you are not trying to issue any other SMART commands.

Hope that is at least somewhat informative, and sorry about the long delay,
Jason

---

Also relevant windows headers here:

http://www.krugle.org/examples/p-nmz...bcA/ntdddisk.h

And you can see all the things you can play with.

----
Ah, here. This is not what I originally looked at but it does describe a
lot:

http://www.codeguru.com/forum/archiv.../t-250315.html

Ignore everything in that thread except for the very last post. You will
only get confused if you read the rest of the posts -- like most
codeguru.com threads, this one is filled by a bunch of people that have no
idea what they are talking about. Fortunately, here, the original poster
came back and posted a valid solution with a good explanation.

Combine that with the windows header I just linked to (the smart stuff is at
the end), and that should give you all the info you need to figure out what
is going on -- in addition to the sample code I posted.

Jason
*/

// #ifndef __DiskSmart_h__
// #define __DiskSmart_h__

#include <windows.h>
#include <assert.h>
#include <algorithm>
#include <string>
#include <iostream>

#include "FsUtil.h"
#include "Hnd.h"

using std::string;


// ATA ID command response; structure memory alignment set to 1 so
// we can grab this straight from the response buffer.
#pragma pack (push, 1)
struct ATA_ID_SECTOR {
    USHORT  wGenConfig;
    USHORT  wNumCyls;
    USHORT  wReserved;
    USHORT  wNumHeads;
    USHORT  wBytesPerTrack;
    USHORT  wBytesPerSector;
    USHORT  wSectorsPerTrack;
    USHORT  wVendorUnique[3];
    CHAR    sSerialNumber[20]; // <-- not a c-style string (see source)
    USHORT  wBufferType;
    USHORT  wBufferSize;
    USHORT  wECCSize;
    CHAR    sFirmwareRev[8];   // <-- not a c-style string (see source)
    CHAR    sModelNumber[40];  // <-- not a c-style string (see source)
    USHORT  wMoreVendorUnique;
    USHORT  wDoubleWordIO;
    USHORT  wCapabilities;
    USHORT  wReserved1;
    USHORT  wPIOTiming;
    USHORT  wDMATiming;
    USHORT  wBS;
    USHORT  wNumCurrentCyls;
    USHORT  wNumCurrentHeads;
    USHORT  wNumCurrentSectorsPerTrack;
    ULONG   ulCurrentSectorCapacity;
    USHORT  wMultSectorStuff;
    ULONG   ulTotalAddressableSectors;
    USHORT  wSingleWordDMA;
    USHORT  wMultiWordDMA;
    BYTE    bReserved[128];
};
#pragma pack (pop)

// ------------------------------------------------------------------------------------------------
class DiskSmart 
{
public:
    DiskSmart (unsigned int driveN);  //  throw (string);
    ~DiskSmart (void);

    void DisplayValues(const char* prefix, std::ostream& out) const; 

private:

    string             _filename;
    unsigned           _dindex;
    Hnd                _hdrive;
    GETVERSIONINPARAMS _verinfo;
    ATA_ID_SECTOR      _ataid;
    string             _ataid_serial;
    string             _ataid_model;
    string             _ataid_firmware;

    void _enableSMART (void); //  throw (string);

};

// #endif


#define DRIVE_HEAD_REG 0xA0
#define ECLASS_SYSTEM   0
#define ECLASS_SMART    1

// ------------------------------------------------------------------------------------------------
// returns an error string
//    errclass: ECLASS_SYSTEM for GetLastError(), ECLASS_SMART for smart error code.
//    code: error code, depending on errclass
//    subcode: error subcode, depending on errclass.
static string _winerr (DWORD errclass, DWORD code, DWORD subcode = 0) 
{
    char msg[1024];
    int i;
    string errmsg;

    if (errclass == ECLASS_SYSTEM) 
    {
        if (!FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, NULL, code, 0, msg, sizeof(msg), NULL))
        {
            errmsg = "Unknown system error";
        }
        else 
        {
            // error messages sometimes have trailing newlines.
            for (i = (int)strlen(msg) - 1; i >= 0 && isspace((unsigned char)msg[i]); -- i)
                msg[i] = 0;
            errmsg = msg;
        }
    } 
    else if (errclass == ECLASS_SMART) 
    {
        switch (code) 
        {
            case SMART_NO_ERROR: errmsg += "No error"; break;
            case SMART_IDE_ERROR: 
                _snprintf(msg, sizeof(msg),  "Error from IDE controller: %08X\n", subcode);
                errmsg += msg;
                break;
            case SMART_INVALID_FLAG: errmsg += "Invalid command flag"; break;
            case SMART_INVALID_COMMAND: errmsg += "Invalid command byte"; break;
            case SMART_INVALID_BUFFER: errmsg += "Bad buffer (null, invalid addr..)"; break;
            case SMART_INVALID_DRIVE: errmsg += "Drive number not valid"; break;
            case SMART_INVALID_IOCTL: errmsg += "Invalid IOCTL"; break;
            case SMART_ERROR_NO_MEM: errmsg += "Could not lock user's buffer"; break;
            case SMART_INVALID_REGISTER: errmsg += "Some IDE Register not valid"; break;
            case SMART_NOT_SUPPORTED: errmsg += "Invalid cmd flag set"; break;
            case SMART_NO_IDE_DEVICE: errmsg += "Cmd issued to device not present"; break;
            default: 
                _snprintf(msg, sizeof(msg), "Unknown SMART error: %08X,%08X", code, subcode);
                errmsg += msg;
                break;
        }
    } 
    else 
    {
        errmsg = "Unknown error.";
    }

    return errmsg;
}

// ------------------------------------------------------------------------------------------------
// IDE device strings are stored with every other character swapped. this will
// swap the characters and also trim leading and trailing whitespace, use to
// convert string from hardware into human readable string.
//
// example:   _idestring("  H  ihtre.e    ", 16) returns "Hi there."

static string _idestring (const char *in, unsigned bytes) 
{
    int i, j;
    char *msg = new char[bytes + 1], *ptr;

    // swap chars then terminate string with 0
    for (i = 0, j = 0; i < (int)bytes; i += 2) 
    {
        if (i < (int)bytes - 1) msg[j ++] = (char)in[i + 1];
        msg[j ++] = (char)in[i];
    }

    msg[bytes] = 0;

    // trim leading and trailing whitespace
    for (ptr = msg; *ptr && isspace((unsigned char)*ptr); ++ ptr)
        ;

    for (i = (int)strlen(ptr) - 1; i >= 0 && isspace((unsigned char)ptr[i]); -- i)
        ptr[i] = 0;

    string ret = ptr;
    delete[] msg;
    return ret;
}

// ------------------------------------------------------------------------------------------------
// constructor opens drive, enables smart, queries for info

DiskSmart::DiskSmart(unsigned int drive_index)
    // throw (string)
{
    char dfile[100];

    _snprintf(dfile, sizeof(dfile), "\\\\.\\PhysicalDrive%u", drive_index);

    _filename = dfile;
    _dindex = drive_index;
    _hdrive = CreateFile(dfile,
        GENERIC_READ | GENERIC_WRITE,
        FILE_SHARE_READ | FILE_SHARE_WRITE | FILE_SHARE_DELETE,
        NULL,
        OPEN_EXISTING,
        FILE_ATTRIBUTE_SYSTEM,
        NULL);

    if (!_hdrive.IsValid()) 
    {
        DWORD e = GetLastError();
        throw "Could not open " + _filename + ": " + _winerr(ECLASS_SYSTEM, e);
    }

    try 
    {
        _enableSMART();
    } 
    catch (...) 
    {
        throw;
    }
}


// ------------------------------------------------------------------------------------------------

DiskSmart::~DiskSmart(void) 
{
}

// ------------------------------------------------------------------------------------------------
void DiskSmart::DisplayValues(const char* prefix, std::ostream& out) const
{
    char str[1024];
    size_t pos = 0;
    pos += _snprintf(str + pos, sizeof(str) - pos, "Device:          %s\n", _filename.c_str());
    pos += _snprintf(str + pos, sizeof(str) - pos, "Driver Version:  %i.%i\n", _verinfo.bVersion, _verinfo.bRevision);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Driver Caps:     0x%02X\n", _verinfo.fCapabilities);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Device Map:      0x%02X\n", _verinfo.bIDEDeviceMap);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Serial Number:   '%s'\n", _ataid_serial.c_str());
    pos += _snprintf(str + pos, sizeof(str) - pos, "Firmware:        '%s'\n", _ataid_firmware.c_str());
    pos += _snprintf(str + pos, sizeof(str) - pos, "Model Number:    '%s'\n", _ataid_model.c_str());
    pos += _snprintf(str + pos, sizeof(str) - pos, "??? wGenConfig:  0x%04X\n", _ataid.wGenConfig);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Cylinders:       %hu\n", _ataid.wNumCyls);
    pos += _snprintf(str + pos, sizeof(str) - pos, "???:             0x%04X\n", _ataid.wReserved);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Heads:           %hu\n", _ataid.wNumHeads);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Bytes/Track:     %hu\n", _ataid.wBytesPerTrack);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Bytes/Sector:    %hu\n", _ataid.wBytesPerSector);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Sectors/Track:   %hu\n", _ataid.wSectorsPerTrack);
    pos += _snprintf(str + pos, sizeof(str) - pos, "??? vendor id?:  0x%04X:0x%04X:0x%04X\n", _ataid.wVendorUnique[0], _ataid.wVendorUnique[1], _ataid.wVendorUnique[2]);
    pos += _snprintf(str + pos, sizeof(str) - pos, "more vendor id?: 0x%04X\n", _ataid.wMoreVendorUnique);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Cur. Cyls:       %hu\n", _ataid.wNumCurrentCyls);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Cur. Sectors:    %hu\n", _ataid.wNumCurrentHeads);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Cur. Sec/Track:  %hu\n", _ataid.wNumCurrentSectorsPerTrack);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Multisec Stuff:  %hu\n", _ataid.wMultSectorStuff);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Cur. Sec Cpcty:  %lu\n", _ataid.ulCurrentSectorCapacity);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Sectors:         %lu\n", _ataid.ulTotalAddressableSectors);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Buffer Type:     0x%04X\n", _ataid.wBufferType);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Buffer Size:     %hu\n", _ataid.wBufferSize);
    pos += _snprintf(str + pos, sizeof(str) - pos, "ECC Size:        %hu\n", _ataid.wECCSize);
    pos += _snprintf(str + pos, sizeof(str) - pos, "??? bs?:         0x%04X\n", _ataid.wBS);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Double word IO:  0x%04X\n", _ataid.wDoubleWordIO);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Caps:            0x%04x\n", _ataid.wCapabilities);
    pos += _snprintf(str + pos, sizeof(str) - pos, "???:             0x%04x\n", _ataid.wReserved1);
    pos += _snprintf(str + pos, sizeof(str) - pos, "PIO timing:      %hu\n", _ataid.wPIOTiming);
    pos += _snprintf(str + pos, sizeof(str) - pos, "DMA timing:      %hu\n", _ataid.wDMATiming);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Single word DMA  %hu\n", _ataid.wSingleWordDMA);
    pos += _snprintf(str + pos, sizeof(str) - pos, "Multi word DMA   %hu", _ataid.wMultiWordDMA);

    assert(pos < sizeof(str));

    std::string sstr(str, str+pos);

    std::string search("\n");
    std::string replace = search + std::string(prefix);
    pos = 0;
    while ((pos = sstr.find(search, pos)) != std::string::npos) {
         sstr.replace(pos, search.length(), replace);
         pos += replace.length();
    }

    out << sstr << std::endl;
}


// ------------------------------------------------------------------------------------------------
// Enable smart and query some info from the drive
//    _dindex must be set to drive index
//    _hdrive must be handle to open device
//    _filename should be device filename, used for constructing error messages.
// throws a string with an error message on error.

void DiskSmart::_enableSMART(void)   //  throw (string)
{
    SENDCMDINPARAMS ci;
    SENDCMDOUTPARAMS co, *pco;
    DWORD ret;
    unsigned char idsec[IDENTIFY_BUFFER_SIZE + 16 /* 16 bytes for SENDCMDOUTPARAMS header */];

    // SMART version and capabilities check

    if (!DeviceIoControl(_hdrive, SMART_GET_VERSION, 
        NULL, 0, &_verinfo, sizeof(_verinfo),
        &ret, NULL))
    {
        DWORD e = GetLastError();
        throw "Could not get SMART version for " + _filename + ": " + _winerr(ECLASS_SYSTEM, e);
    }

    if ((_verinfo.fCapabilities & CAP_ATA_ID_CMD) != CAP_ATA_ID_CMD)
        throw "Device " + _filename + " does not support ATA ID command";

    if ((_verinfo.fCapabilities & CAP_SMART_CMD) != CAP_SMART_CMD)
        throw "Device " + _filename + " does not support SMART";

    // SMART supported, now enable monitoring and commands on device

    _ASSERTE(_dindex < 256); // it fits in a BYTE

    memset(&ci, 0, sizeof(ci));
    ci.bDriveNumber                 = (BYTE)_dindex;
    ci.irDriveRegs.bFeaturesReg     = ENABLE_SMART;   // <-- bFeaturesReg gets smart commmand.
    ci.irDriveRegs.bSectorCountReg  = 1;
    ci.irDriveRegs.bSectorNumberReg = 1;
    ci.irDriveRegs.bCylLowReg       = SMART_CYL_LOW;  // <-- magic number identifies as smart command
    ci.irDriveRegs.bCylHighReg      = SMART_CYL_HI;   // <-- magic number identifies as smart command
    ci.irDriveRegs.bDriveHeadReg    = DRIVE_HEAD_REG; // <-- magic number identifies as smart command
    ci.irDriveRegs.bCommandReg      = SMART_CMD;      // <-- smart command

    if (!DeviceIoControl(_hdrive, SMART_SEND_DRIVE_COMMAND,
        &ci, sizeof(ci), &co, sizeof(co),
        &ret, NULL))
    {
        DWORD e = GetLastError();
        throw "Could not enable SMART on " + _filename + ": " + _winerr(ECLASS_SYSTEM, e);
    }

    /* // it looks like this isn't valid here? data is just copied from ci...
    if (co.DriverStatus.bDriverError)
    throw "Could not enable SMART on " + _filename + ": " + 
    _winerr(ECLASS_SMART, co.DriverStatus.bDriverError, co.DriverStatus.bIDEError);
    */

    // Get some device info with ATA ID command

    memset(&ci, 0, sizeof(ci));
    //ci.cBufferSize                  = IDENTIFY_BUFFER_SIZE;
    ci.bDriveNumber                 = (BYTE)_dindex;
    ci.irDriveRegs.bFeaturesReg     = 0;
    ci.irDriveRegs.bSectorCountReg  = 1;
    ci.irDriveRegs.bSectorNumberReg = 1;
    ci.irDriveRegs.bCylLowReg       = 0;
    ci.irDriveRegs.bCylHighReg      = 0;
    ci.irDriveRegs.bDriveHeadReg    = DRIVE_HEAD_REG;
    ci.irDriveRegs.bCommandReg      = ID_CMD;

    if (!DeviceIoControl(_hdrive, SMART_RCV_DRIVE_DATA,
        &ci, sizeof(ci), idsec, sizeof(idsec),
        &ret, NULL))
    {
        DWORD e = GetLastError();
        throw "Could not get ATA drive info for " + _filename + ": " + _winerr(ECLASS_SYSTEM, e);
    }

    // that last command has received the ata id data block from the
    // hard drive, and placed the result in our 'idsec' buffer. after
    // making sure the buffer is the size we expect, copy the data
    // into the _ataid structure.

    if (ret < sizeof(_ataid) + 16)
        throw "Could not get ATA drive info for " + _filename + ": Returned data size too small";

    _ASSERTE(sizeof(_ataid) <= IDENTIFY_BUFFER_SIZE);
    pco = (SENDCMDOUTPARAMS *)idsec;
    memcpy(&_ataid, pco->bBuffer, sizeof(_ataid));

    if (pco->DriverStatus.bDriverError)
        throw "Could not get ATA drive info for " + _filename + ": " + 
        _winerr(ECLASS_SMART, pco->DriverStatus.bDriverError, pco->DriverStatus.bIDEError);

    // strings from the device are not quite human-readable, decode and
    // store readable versions:

    _ataid_serial   = _idestring(_ataid.sSerialNumber, sizeof(_ataid.sSerialNumber));
    _ataid_model    = _idestring(_ataid.sModelNumber, sizeof(_ataid.sModelNumber));
    _ataid_firmware = _idestring(_ataid.sFirmwareRev, sizeof(_ataid.sFirmwareRev));
}

// ------------------------------------------------------------------------------------------------
// Display Smart drive information.
// Can throw exception.

extern 
int DisplaySmartInfo(const char* prefix, std::ostream& out)
{
    std::string phyDriveList, driveLetters;
    FsUtil::GetPhysicalDrives(phyDriveList, driveLetters);

    std::sort(phyDriveList.begin(), phyDriveList.end());
    phyDriveList.erase(std::unique(phyDriveList.begin(), phyDriveList.end()), phyDriveList.end());

    for (std::string::const_iterator iter = phyDriveList.begin(); iter != phyDriveList.end(); ++iter)
    {
        unsigned driveN = *iter - '0';

        DiskSmart diskSmart(driveN);
        diskSmart.DisplayValues(prefix, out);
    }

    return 0;
} 
