// ------------------------------------------------------------------------------------------------
// Network System Information
//
// Project: Mfile (Magnifile)
// Author:  Dennis Lang   Apr-2011
// https://landenlabs.com/
// ------------------------------------------------------------------------------------------------


#include "NetSysInfo.h"
#include "PsUtil.h"
#include "Support.h"

// c++
#include <map>
#include <vector>
#include <iomanip>

#include <winsock2.h>
#include <Ws2tcpip.h>

// Windows
#include <windows.h>
#include <Assert.h>

// Network
#include <Iphlpapi.h>
#pragma comment(lib, "iphlpapi.lib")

enum TcpConnectionOffloadState
{
    InHost = 0,
    Offloading = 1,
    Offloaded = 2,
    Uploading = 3,
    Max = 4
};

enum TcpTableClass 
{
    BasicListener,
    BasicConnections,
    BasicAll,
    OwnerPidListener,
    OwnerPidConnections,
    OwnerPidAll,
    OwnerModuleListener,
    OwnerModuleConnections,
    OwnerModuleAll
};

enum MibTcpState  
{
    Closed = 1,
    Listening = 2,
    SynSent = 3,
    SynReceived = 4,
    Established = 5,
    FinWait1 = 6,
    FinWait2 = 7,
    CloseWait = 8,
    Closing = 9,
    LastAck = 10,
    TimeWait = 11,
    DeleteTcb = 12
};


// ------------------------------------------------------------------------------------------------
bool GetTcpStats(MIB_TCPSTATS& tcpStats)
{
	DWORD dwRetVal = 0;
	return (GetTcpStatistics(&tcpStats) == NO_ERROR); 
}

//-----------------------------------------------------------------------------
/// Get TCPv4 connection table, return number of entries, 0 if error.
DWORD GetTcp4Pids(ScopePtr<MIB_TCPTABLE_OWNER_PID>& pTable)
{
    DWORD objSize = pTable.Size();
    DWORD status;
    status = GetExtendedTcpTable(pTable, &objSize, false, AF_INET, TCP_TABLE_OWNER_PID_ALL, 0);

    if (objSize > pTable.Size())
    {
        pTable.Resize(objSize);
        status = GetExtendedTcpTable(pTable, &objSize, false, AF_INET, TCP_TABLE_OWNER_PID_ALL, 0);
    }

    return (status == 0) ? pTable->dwNumEntries : 0;
}

// ------------------------------------------------------------------------------------------------
/// Get TCPv6 connection table, return number of entries, 0 if error.
DWORD GetTcp6Pids(ScopePtr<MIB_TCP6TABLE_OWNER_PID>& pTable)
{
    DWORD objSize = pTable.Size();
    DWORD status;
    status = GetExtendedTcpTable(pTable, &objSize, false, AF_INET6, TCP_TABLE_OWNER_PID_ALL, 0);

    if (objSize > pTable.Size())
    {
        pTable.Resize(objSize);
        status = GetExtendedTcpTable(pTable, &objSize, false, AF_INET6, TCP_TABLE_OWNER_PID_ALL, 0);
    }

    return (status == 0) ? pTable->dwNumEntries : 0;
}

// ------------------------------------------------------------------------------------------------
bool GetUdpStats(MIB_UDPSTATS& udpStats)
{
    return (GetUdpStatistics(&udpStats) == NO_ERROR); 
}

// ------------------------------------------------------------------------------------------------
DWORD GetUdp4Pids(ScopePtr<MIB_UDPTABLE_OWNER_PID>& pTable)
{
    DWORD objSize = pTable.Size();
    DWORD status;
    status = GetExtendedUdpTable(pTable, &objSize, false, AF_INET, UDP_TABLE_OWNER_PID, 0);

    if (objSize > pTable.Size())
    {
        pTable.Resize(objSize);
        status = GetExtendedUdpTable(pTable, &objSize, false, AF_INET, UDP_TABLE_OWNER_PID, 0);
    }

    return (status == 0) ? pTable->dwNumEntries : 0;
}

// ------------------------------------------------------------------------------------------------
DWORD GetUdp6Pids(ScopePtr<MIB_UDP6TABLE_OWNER_PID>& pTable)
{
    DWORD objSize = pTable.Size();
    DWORD status;
    status = GetExtendedUdpTable(pTable, &objSize, false, AF_INET6, UDP_TABLE_OWNER_PID, 0);

    if (objSize > pTable.Size())
    {
        pTable.Resize(objSize);
        status = GetExtendedUdpTable(pTable, &objSize, false, AF_INET6, UDP_TABLE_OWNER_PID, 0);
    }

    return (status == 0) ? pTable->dwNumEntries : 0;
}

// ------------------------------------------------------------------------------------------------
class IpAddr
{
private:
    DWORD m_addr;
public:
    IpAddr() : m_addr(0) {}
    IpAddr(DWORD addr) : m_addr(addr) {}

    operator DWORD() const { return m_addr; }
};

// ------------------------------------------------------------------------------------------------
typedef UCHAR IPv6Address[16];
class IpEndPoint
{
public: 
    IPv6Address   Address6;
    IpAddr        Address;
    WORD Port;

    IpEndPoint() : Address(0), Port(0)
    {}
};

// ------------------------------------------------------------------------------------------------
class NetworkConnection
{
public:
    enum NetProtocol
    {
        eNone,
        eTcp,
        eUdp,
        eTcp6,
        eUdp6
    };

    enum NetState 
    {
        STATE_NONE       = 0,
        STATE_CLOSED     = MIB_TCP_STATE_CLOSED,     
        STATE_LISTEN     = MIB_TCP_STATE_LISTEN,     
        STATE_SYN_SENT   = MIB_TCP_STATE_SYN_SENT,   
        STATE_SYN_RCVD   = MIB_TCP_STATE_SYN_RCVD,   
        STATE_ESTAB      = MIB_TCP_STATE_ESTAB,      
        STATE_FIN_WAIT1  = MIB_TCP_STATE_FIN_WAIT1,  
        STATE_FIN_WAIT2  = MIB_TCP_STATE_FIN_WAIT2,  
        STATE_CLOSE_WAIT = MIB_TCP_STATE_CLOSE_WAIT, 
        STATE_CLOSING    = MIB_TCP_STATE_CLOSING,    
        STATE_LAST_ACK   = MIB_TCP_STATE_LAST_ACK,   
        STATE_TIME_WAIT  = MIB_TCP_STATE_TIME_WAIT,  
        STATE_DELETE_TCB = MIB_TCP_STATE_DELETE_TCB  
    };

    int Pid;
    NetProtocol Protocol;
    
    IpEndPoint Local;
    IpEndPoint Remote;
    NetState   State;

    NetworkConnection() : Pid(-1), Protocol(eNone), Local(), Remote(), State(STATE_NONE)
    {}

    static const char* NetworkConnection::ToString(NetworkConnection::NetState state)
    {
        return "";
    }

    static const char* NetworkConnection::ToString(NetworkConnection::NetProtocol prot)
    {
        static const char* sProtStr[] =
        {
            "None", "Tcp4", "Udp4", "Tcp6", "Udp6"
        };
        return sProtStr[prot];
    }
};

// ------------------------------------------------------------------------------------------------
/// <summary>
///     Gets the network connections currently active.
/// </summary>
/// <returns>
///     A dictionary of network connections
///     Dictionary<int, List<NetworkConnection>> 
/// </returns>
typedef std::vector<NetworkConnection> NetConnectionList;
typedef std::map<DWORD, NetConnectionList> NetPidConnectionListMap;

static bool GetNetworkConnections(NetPidConnectionListMap& connections)
{
    {
        // TCP IPv4
        ScopePtr<MIB_TCPTABLE_OWNER_PID> pTable4;
        DWORD tcp4Cnt = GetTcp4Pids(pTable4);

        for (DWORD idx = 0; idx < tcp4Cnt; idx++)
        {
            MIB_TCPROW_OWNER_PID& tcpOwnerPid = pTable4->table[idx];
            DWORD pid = tcpOwnerPid.dwOwningPid;

            if (connections.find(pid) == connections.end())
            {
                NetConnectionList netList;
                connections.insert(std::make_pair<DWORD,NetConnectionList> (pid, netList));
            }

            NetworkConnection netConn;
            netConn.Protocol =  NetworkConnection::eTcp;
            netConn.Local.Address = tcpOwnerPid.dwLocalAddr;
            netConn.Local.Port = htons((WORD)tcpOwnerPid.dwLocalPort);
            netConn.Remote.Address = tcpOwnerPid.dwRemoteAddr;
            netConn.Remote.Port = htons((WORD)tcpOwnerPid.dwRemotePort);
            netConn.State = (NetworkConnection::NetState)tcpOwnerPid.dwState;
            netConn.Pid = pid;

            connections[pid].push_back(netConn);
        }
    }

    {
        // TCP IPv6
        ScopePtr<MIB_TCP6TABLE_OWNER_PID> pTable6;
        DWORD tcp6Cnt = GetTcp6Pids(pTable6);

        for (DWORD idx = 0; idx < tcp6Cnt; idx++)
        {
            MIB_TCP6ROW_OWNER_PID& tcpOwnerPid = pTable6->table[idx];
            DWORD pid = tcpOwnerPid.dwOwningPid;

            if (connections.find(pid) == connections.end())
            {
                NetConnectionList netList;
                connections.insert(std::make_pair<DWORD,NetConnectionList> (pid, netList));
            }

            NetworkConnection netConn;
            netConn.Protocol =  NetworkConnection::eTcp6;
            memcpy(netConn.Local.Address6, tcpOwnerPid.ucLocalAddr, sizeof(netConn.Local.Address6));
            netConn.Local.Port = htons((WORD)tcpOwnerPid.dwLocalPort);
            memcpy(netConn.Remote.Address6, tcpOwnerPid.ucRemoteAddr, sizeof(netConn.Remote.Address6));
            netConn.Remote.Port = htons((WORD)tcpOwnerPid.dwRemotePort);
            netConn.State = (NetworkConnection::NetState)tcpOwnerPid.dwState;
            netConn.Pid = pid;

            connections[pid].push_back(netConn);
        }
    }

    {
        // UDP IPv4
        ScopePtr<MIB_UDPTABLE_OWNER_PID> pTable;
        DWORD cnt = GetUdp4Pids(pTable);

        for (DWORD idx = 0; idx < cnt; idx++)
        {
            MIB_UDPROW_OWNER_PID& udpOwnerPid = pTable->table[idx];
            DWORD pid = udpOwnerPid.dwOwningPid;

            if (connections.find(pid) == connections.end())
            {
                NetConnectionList netList;
                connections.insert(std::make_pair<DWORD,NetConnectionList> (pid, netList));
            }

            NetworkConnection netConn;
            netConn.Protocol =  NetworkConnection::eUdp;
            netConn.Local.Address = udpOwnerPid.dwLocalAddr;
            netConn.Local.Port = htons((WORD)udpOwnerPid.dwLocalPort);
            netConn.Pid = pid;

            connections[pid].push_back(netConn);
        }
    }

    {
        // UDP IPv6
        ScopePtr<MIB_UDP6TABLE_OWNER_PID> pTable;
        DWORD cnt = GetUdp6Pids(pTable);

        for (DWORD idx = 0; idx < cnt; idx++)
        {
            MIB_UDP6ROW_OWNER_PID& udpOwnerPid = pTable->table[idx];
            DWORD pid = udpOwnerPid.dwOwningPid;

            if (connections.find(pid) == connections.end())
            {
                NetConnectionList netList;
                connections.insert(std::make_pair<DWORD,NetConnectionList> (pid, netList));
            }

            NetworkConnection netConn;
            netConn.Protocol =  NetworkConnection::eUdp6;
            memcpy(netConn.Local.Address6,  udpOwnerPid.ucLocalAddr, sizeof(netConn.Local.Address6));
            netConn.Local.Port = htons((WORD)udpOwnerPid.dwLocalPort);
            netConn.Pid = pid;

            connections[pid].push_back(netConn);
        }
    }

    return true;
}

// ------------------------------------------------------------------------------------------------
static std::ostream& operator << (std::ostream& out, const IpAddr& addr)
{
    struct in_addr *pAddr = (struct in_addr *)&addr;
    out << inet_ntoa(*pAddr);
    return out;
}

// ------------------------------------------------------------------------------------------------
/// List network connections
/// Return number of connections
size_t NetworkSysInfo::ListConnections(std::ostream& out, const Options& options)
{
    NetPidConnectionListMap connections;
    bool okay = GetNetworkConnections(connections);

    out << "  Pid Prot   Local Address  Port  Remote Address  Port ProcName\n"
           "----- ---- --------------- ----- --------------- ----- --------\n";

    NetPidConnectionListMap::const_iterator iter;
    for(iter = connections.begin();  iter != connections.end(); ++iter)
    {
        Pid_t pid = iter->first;
        const NetConnectionList& netConnList = iter->second;
        std::string procName;
        PsUtil::GetProcessName(pid, procName);

        size_t lstCnt = netConnList.size();
        for (size_t lstIdx = 0; lstIdx < lstCnt; ++lstIdx)
        {
            const NetworkConnection netConn = netConnList[lstIdx];
            out << std::setw(5) << pid
                << " "
                << NetworkConnection::ToString(netConn.Protocol)
                << " ";

            if (netConn.Protocol == NetworkConnection::eTcp ||
                netConn.Protocol == NetworkConnection::eTcp6)
            {
                out 
                    << std::setw(15) << netConn.Local.Address
                    << std::setw(6) << netConn.Local.Port
                    << " "
                    << std::setw(15) << netConn.Remote.Address
                    << std::setw(6) << netConn.Remote.Port;
            }
            else
            {
                out 
                    << std::setw(15) << "ipv6??"                // netConn.Local.Address6
                    << std::setw(6) << netConn.Local.Port
                    << " "
                    << std::setw(15) << "ipv6??"                // netConn.Remote.Address6
                    << std::setw(6) << netConn.Remote.Port;
            }

            out << " "
                << procName
                << std::endl;
        }
    }


    out << "\nInterfaces:\n";
    const bool sLocalNet = true;
    const bool sRemoteNet = false;
    if (ListNetInterfaces(out, "\t", sLocalNet))
    {  }
    if (ListNetInterfaces(out, "\t", sRemoteNet))
    { }

    out << "\nMAC Address\n";
    ListMACaddress(out, "\t");

    out << "\nIP Mask\n";
    ShowIPMask(out, "\t");

    return connections.size();
}

// ------------------------------------------------------------------------------------------------
// Prints the MAC address stored in a 6 byte array to stdout
static void PrintMACaddress(std::ostream& out, const char* prefix, unsigned char MACData[])
{
    char macStr[60];
	_snprintf(macStr, ARRAYSIZE(macStr), "%02X-%02X-%02X-%02X-%02X-%02X\n", 
		MACData[0], MACData[1], MACData[2], MACData[3], MACData[4], MACData[5]);
    out << prefix << macStr;
}

// ------------------------------------------------------------------------------------------------
// Fetches the MAC address and prints it
void NetworkSysInfo::ListMACaddress(std::ostream& out, const char* prefix)
{
    IP_ADAPTER_INFO AdapterInfo[16];       // Allocate information for up to 16 NICs
    DWORD dwBufLen = sizeof(AdapterInfo);  // Size of buffer

    DWORD dwStatus = GetAdaptersInfo(       // Call GetAdapterInfo
            AdapterInfo,                    // [out] buffer to receive data
            &dwBufLen);                     // [in] size of receive data buffer
    assert(dwStatus == ERROR_SUCCESS);      // Verify return value is
                                            // valid, no buffer overflow

    PIP_ADAPTER_INFO pAdapterInfo = AdapterInfo; // Contains pointer to current adapter info
    while (pAdapterInfo)                        // Terminate if last adapter
    {
        PrintMACaddress(out, prefix, pAdapterInfo->Address); // Print MAC address
        pAdapterInfo = pAdapterInfo->Next;      // Progress through linked list
    }
}

// ------------------------------------------------------------------------------------------------
bool NetworkSysInfo::ShowIPMask(std::ostream& out, const char* prefix)
{
    bool okay = false;
    PMIB_IPADDRTABLE  pIpTab = NULL;
    DWORD dwSizeReq = 0;

    DWORD dwRet = GetIpAddrTable(pIpTab, &dwSizeReq, TRUE);
    if (ERROR_INSUFFICIENT_BUFFER == dwRet) 
    {
        pIpTab = (PMIB_IPADDRTABLE) new BYTE[dwSizeReq];
        assert(pIpTab);
        dwRet = GetIpAddrTable(pIpTab, &dwSizeReq, TRUE);
        if (NO_ERROR == dwRet) 
        {
            // in_addr addr;

            for ( DWORD i = 0; i < pIpTab->dwNumEntries; i++) 
            {
                out << prefix 
                    << " Address:"   << std::setw(15) << IpAddr(pIpTab->table[i].dwAddr)
                    << " Mask:"      << std::setw(15)<< IpAddr(pIpTab->table[i].dwMask) 
                    << " Broadcast:" << std::setw(15)<< IpAddr(pIpTab->table[i].dwBCastAddr)
                    << std::endl;
            }

            okay = true;
        }
    }

    delete[] pIpTab; 
    return okay;
}

// ------------------------------------------------------------------------------------------------
#include <winsock2.h>

const int WSA_OFF = -999;
static int sWSA_status = WSA_OFF;
static WSADATA sWinsockData;
static bool InitWinSock(int mVer=2, int sVer=2)
{
    if (sWSA_status == WSA_OFF)
    {
        sWSA_status = WSAStartup(MAKEWORD(mVer, sVer), &sWinsockData);
    }

    return (sWSA_status == 0);
}

// ------------------------------------------------------------------------------------------------
static void ExitWinSock()
{
    if (sWSA_status == 0)
    {
        WSACleanup();
        sWSA_status = WSA_OFF;
    }
}

// ------------------------------------------------------------------------------------------------
bool inet_inrange(struct in_addr *subject, struct in_addr *IPOfRange, struct in_addr * MaskOfRange)
{
  struct in_addr tmp;

  tmp.S_un.S_un_b.s_b1 = subject->S_un.S_un_b.s_b1 & MaskOfRange->S_un.S_un_b.s_b1;
  tmp.S_un.S_un_b.s_b2 = subject->S_un.S_un_b.s_b2 & MaskOfRange->S_un.S_un_b.s_b2;
  tmp.S_un.S_un_b.s_b3 = subject->S_un.S_un_b.s_b3 & MaskOfRange->S_un.S_un_b.s_b3;
  tmp.S_un.S_un_b.s_b4 = subject->S_un.S_un_b.s_b4 & MaskOfRange->S_un.S_un_b.s_b4;

  return (memcmp(&tmp, IPOfRange, sizeof(tmp)) == 0);
}

// ------------------------------------------------------------------------------------------------
bool NetworkSysInfo::ListNetInterfaces(std::ostream& out, const char* prefix, bool localnet) 
{
    InitWinSock();

    SOCKET sd = WSASocket(AF_INET, SOCK_DGRAM, 0, 0, 0, 0);

    if (sd == SOCKET_ERROR) 
    {
        out << "Failed to get a socket.  Error " << WSAGetLastError() << std::endl;
        return false;
    }

    INTERFACE_INFO InterfaceList[20];
    unsigned long nBytesReturned;

    if (WSAIoctl(sd, SIO_GET_INTERFACE_LIST, 0, 0, &InterfaceList,
        sizeof(InterfaceList), &nBytesReturned, 0, 0) == SOCKET_ERROR) 
    {
        out << "Failed calling WSAIoctl: error " << WSAGetLastError() << std::endl;
        return false;
    }

    unsigned nNumInterfaces = nBytesReturned / sizeof(INTERFACE_INFO);
    struct in_addr IPA; IPA.S_un.S_addr = inet_addr("10.0.0.0");
    struct in_addr MaskA; MaskA.S_un.S_addr = inet_addr("255.0.0.0"); // (10/8 prefix) 
    struct in_addr IPB; IPB.S_un.S_addr = inet_addr("172.16.0.0");
    struct in_addr MaskB; MaskB.S_un.S_addr = inet_addr("255.240.0.0"); // (172.16/12 prefix) 
    struct in_addr IPC; IPC.S_un.S_addr = inet_addr("192.168.0.0");
    struct in_addr MaskC; MaskC.S_un.S_addr = inet_addr("255.255.0.0"); // (192.168/16 prefix)

    for (unsigned i = 0; i < nNumInterfaces; ++i) 
    {
        sockaddr_in* pAddress;
        sockaddr_gen sockGen = InterfaceList[i].iiAddress;
        pAddress = (sockaddr_in*)&sockGen.Address;
        // pAddress = (sockaddr_in*)&(InterfaceList[i].iiAddress);

        // is IP within Private Address Space???
        bool match = inet_inrange(&pAddress->sin_addr, &IPA, &MaskA);
        match = match || inet_inrange(&pAddress->sin_addr, &IPB, &MaskB);
        match = match || inet_inrange(&pAddress->sin_addr, &IPC, &MaskC);
        // match = true;

        u_long nFlags = InterfaceList[i].iiFlags;
        // Skip interfaces that are 'down' 
        if (!(nFlags & IFF_UP))
            continue;

        if (nFlags & IFF_LOOPBACK)
            match = TRUE;

        // if localnet == FALSE, we're looking for PPP connections! 
        if (localnet == false)
        {
            // if ((nFlags & IFF_POINTTOPOINT) 
            if (!match)
            {
                out << prefix << inet_ntoa(pAddress->sin_addr);
                break;
            }
        }
        else
        {
            // don't list LOOPBACK address! 
            if (nFlags & IFF_LOOPBACK)
                continue;

            // if (!(nFlags & IFF_POINTTOPOINT) 
            if (match)
            {
                out  << prefix << inet_ntoa(pAddress->sin_addr);
                break;
            }
        }
    }

    return 0;
}

#if 0

// ------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a network adapter installed on the machine.
/// Properties of this class can be used to obtain current network speed.
/// </summary>
class NetworkAdapter
{
    std::string name;
    long dlSpeed, ulSpeed;				// Download\Upload speed in bytes per second.
    long dlValue, ulValue;				// Download\Upload counter value in bytes.
    long dlValueOld, ulValueOld;		// Download\Upload counter value one second earlier, in bytes.

    // PerformanceCounter dlCounter, ulCounter;	// Performance counters to monitor download and upload speed.

public:

    NetworkAdapter(std::string _name)
    {
        name = _name;
    }

    void init()
    {
        // Since dlValueOld and ulValueOld are used in method refresh() to calculate network speed, they must have be initialized.
        dlValueOld = dlCounter.NextSample().RawValue;
        ulValueOld = ulCounter.NextSample().RawValue;
    }

    /// <summary>
    /// Obtain new sample from performance counters, and refresh the values saved in dlSpeed, ulSpeed, etc.
    /// This method is supposed to be called only in NetworkMonitor, one time every second.
    /// </summary>
    void refresh()
    {
        dlValue = dlCounter.NextSample().RawValue;
        ulValue = ulCounter.NextSample().RawValue;

        // Calculates download and upload speed.
        dlSpeed = dlValue - dlValueOld;
        ulSpeed = ulValue - ulValueOld;

        dlValueOld = dlValue;
        ulValueOld = ulValue;
    }

    /// <summary>
    /// Overrides method to return the name of the adapter.
    /// </summary>
    /// <returns>The name of the adapter.</returns>
    std::string ToString()
    {
        return name;
    }

    /// <summary>
    /// The name of the network adapter.
    /// </summary>
    std::string Name()
    {
        return name;
    }

    /// <summary>
    /// Current download speed in bytes per second.
    /// </summary>
    long DownloadSpeed()
    {
        return dlSpeed;
    }

    /// <summary>
    /// Current upload speed in bytes per second.
    /// </summary>
    long UploadSpeed()
    {
        return ulSpeed;
    }

    /// <summary>
    /// Current download speed in kbytes per second.
    /// </summary>
    double DownloadSpeedKbps()
    {
        return dlSpeed / 1024.0;
    }

    /// <summary>
    /// Current upload speed in kbytes per second.
    /// </summary>
    double UploadSpeedKbps()
    {
        return ulSpeed / 1024.0;
    }
}

// ------------------------------------------------------------------------------------------------
class NetworkInformation
{
private:
    static NetworkInterface* pNIC = NULL;

public:
    NetworkInformation()
    {
        pNIC = NetworkInterface.GetAllNetworkInterfaces();
    }

    std::string BytesReceived(int index)
    {
        return pNIC[index].GetIPv4Statistics().BytesReceived.ToString();
    }

    string BytesSent(int index)
    {
        return pNIC[index].GetIPv4Statistics().BytesSent.ToString();
    }

    string IncomingPacketsDiscarded(int index)
    {
        return pNIC[index].GetIPv4Statistics().IncomingPacketsDiscarded.ToString();
    }

    string IncomingPacketsWithErrors(int index)
    {
        return pNIC[index].GetIPv4Statistics().IncomingPacketsWithErrors.ToString();
    }

    string Description(int index)
    {
        return NIC[index].Description;
    }

    string Speed(int index)
    {
        return NIC[index].Speed.ToString();
    }
};

// ------------------------------------------------------------------------------------------------
/// <summary>
/// The NetworkMonitor class monitors network speed for each network adapter on the computer, using classes for Performance counter in .NET library.
/// </summary>
class NetworkMonitor
{
System.Timers.Timer timer;						// The timer event executes every second to refresh the values in adapters.
ArrayList adapters;					// The list of adapters on the computer.
ArrayList monitoredAdapters;		// The list of currently monitored adapters.

/// <summary>
/// NetworkMonitor
/// </summary>
NetworkMonitor()
{
    this.adapters = new ArrayList();
    this.monitoredAdapters = new ArrayList();
    this.EnumerateNetworkAdapters();

    this.timer = new System.Timers.Timer(1000);
//            this.timer.Elapsed += new ElapsedEventHandler(this.timer_Elapsed);
}

// ------------------------------------------------------------------------------------------------
/// <summary>
/// Enumerates network adapters installed on the computer.
/// </summary>
void EnumerateNetworkAdapters()
{
    PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");

    foreach (string name in category.GetInstanceNames())
    {
        // This one exists on every computer.
        if (name == "MS TCP Loopback interface")
        { continue; }
        // Create an instance of NetworkAdapter class, and create performance counters for it.
        NetworkAdapter adapter = new NetworkAdapter(name);
        adapter.dlCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", name);
        adapter.ulCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", name);
        this.adapters.Add(adapter);			// Add it to ArrayList adapter
    }
}

#if false
// ------------------------------------------------------------------------------------------------
private void timer_Elapsed(object sender, ElapsedEventArgs e)
{
    foreach (NetworkAdapter adapter in this.monitoredAdapters)
    { adapter.refresh(); }
}
#endif

// ------------------------------------------------------------------------------------------------
/// <summary>
/// Get instances of NetworkAdapter for installed adapters on this computer.
/// </summary>
NetworkAdapter* Adapters
{
    return (NetworkAdapter[])this.adapters.ToArray(typeof(NetworkAdapter));
}

// ------------------------------------------------------------------------------------------------
/// <summary>
/// Enable the timer and add all adapters to the monitoredAdapters list, unless the adapters list is empty.
/// </summary>
public void StartMonitoring()
{
    if (this.adapters.Count > 0)
    {
        foreach (NetworkAdapter adapter in this.adapters)
            if (!this.monitoredAdapters.Contains(adapter))
            {
                this.monitoredAdapters.Add(adapter);
                adapter.init();
            }

        this.timer.Enabled = true;
    }
}

// ------------------------------------------------------------------------------------------------
/// <summary>
/// Enable the timer, and add the specified adapter to the monitoredAdapters list
/// </summary>
/// <param name="adapter"></param>
public void StartMonitoring(NetworkAdapter adapter)
{
    if (!this.monitoredAdapters.Contains(adapter))
    {
        this.monitoredAdapters.Add(adapter);
        adapter.init();
    }
    this.timer.Enabled = true;
}

// ------------------------------------------------------------------------------------------------
/// <summary>
/// Disable the timer, and clear the monitoredAdapters list.
/// </summary>
public void StopMonitoring()
{
    this.monitoredAdapters.Clear();
    this.timer.Enabled = false;
}

// ------------------------------------------------------------------------------------------------
/// <summary>
/// Remove the specified adapter from the monitoredAdapters list, and disable the timer if the monitoredAdapters list is empty.
/// </summary>
/// <param name="adapter"></param>
public void StopMonitoring(NetworkAdapter adapter)
{
    if (this.monitoredAdapters.Contains(adapter))
    { this.monitoredAdapters.Remove(adapter); }
    if (this.monitoredAdapters.Count == 0)
    { this.timer.Enabled = false; }
}
}
}
#endif
