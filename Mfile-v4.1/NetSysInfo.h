// ------------------------------------------------------------------------------------------------
// Network system information.
//
// Project: Mfile (Magnifile)
// Author:  Dennis Lang   Apr-2011
// https://landenlabs.com/
// ------------------------------------------------------------------------------------------------

#pragma once

#include <iostream>
#include "Options.h"

class NetworkSysInfo
{
public:
    // List open network connections.
    // Return number of connections.
    size_t ListConnections(std::ostream& out, const Options& options);

    // List network interfaces
    bool ListNetInterfaces(std::ostream& out, const char* prefix, bool localnet);

    // Fetches the MAC address and prints it
    void ListMACaddress(std::ostream& out, const char* prefix);

    bool ShowIPMask(std::ostream& out, const char* prefix);
};

