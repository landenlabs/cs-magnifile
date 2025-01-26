// ------------------------------------------------------------------------------------------------
// ListOpenHandles
// Author:  DLang   2009  
// ------------------------------------------------------------------------------------------------

#pragma once

#include <windows.h>

#include <iostream>
#include <string>
#include <map>

#include "PsUtil.h"
#include "Options.h"

struct Value
{
    Value() : minVal(0), curVal(0), maxVal(0) {}
    Value(size_t minv, size_t curV, size_t maxV) : minVal(minv), curVal(curV), maxVal(maxV) {}

    void Set(size_t val) 
    { minVal = curVal = maxVal = val; }

    void Update(size_t val)
    {
        if (minVal == maxVal && minVal == 0)
            minVal = curVal = maxVal = val;
        else if (val < minVal)
            minVal = val;
        else if (val > maxVal)
            maxVal = val;
        curVal = val;
    }

    size_t minVal;
    size_t curVal;
    size_t maxVal;
};
struct UIntPair
{
    UIntPair() : first(0), second(0) {}
    unsigned int first;
    unsigned int second;
};

class ListOpenHandles
{
public:
    ListOpenHandles() : 
        m_showModules(false), 
        m_showAll(false), 
        m_showNet(false), 
        m_showSummary(false),
        m_showFiles(false),
        m_showEvents(false),
        m_showHex(false),
        m_showProcInfo(true)
    {}

    int ReportOpenFiles(std::ostream& out, const Options& options);
    int ReportOpenFiles(std::ostream& out, const Options& options, bool showHeaders, const char* prefix);
    void ColorizeProcInfo(const std::string& str);

   
    typedef std::map<std::string, Value>  ValueMap;
    typedef std::map<std::string, unsigned int> WidthMap;

    typedef std::map<unsigned int, unsigned int> HandleCountByType;
    typedef std::map<unsigned int, UIntPair> HandleCountPairByType;
    typedef std::map<Pid_t, HandleCountPairByType> PidHandleCountByType;

    std::string m_procName;

    // ---- Process Information
    void GetProcInfo(const void* pProcInfoVoid);    // Populate values below.
    char        m_procDataStr[64];
    char        m_procTimeStr[64];
    char        m_elapseTmStr[64];
    ValueMap    m_procInfoMap;              // min, cur, max value per parameter
    WidthMap    m_procInfoWidthMap;         // presentation width per parameter

    // ---- Get Handle count information
    // Populate values below, return access error count
    size_t GetHandleCounts(HANDLE procHnd, Pid_t pid, const void* pProcInfoVoid, std::ostream& out);    
    HandleCountByType       m_handleCountByType;        // current Pid's handle count details
    PidHandleCountByType    m_pidHandleCountByType;     // count per PID

    // ---- Runtime command options
    std::string m_process;          // -p <process>
    std::string m_filename;         // -i <filename>

    bool        m_showModules;      // -y m
    bool        m_showAll;          // -y a
    bool        m_showNet;          // -y n
    bool        m_showSummary;      // -y s
    bool        m_showFiles;        // -y f
    bool        m_showEvents;       // -y e
    bool        m_showHex;          // -y x
    bool        m_showProcInfo;     // -y p

    typedef std::map<unsigned int, const char*> ObjTypeMap;
    static ObjTypeMap sObjTypeMap;
};


