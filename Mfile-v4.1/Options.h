// ------------------------------------------------------------------------------------------------
// Runtime options class
//
// Project: Mfile (Magnifile)
// Author:  Dennis Lang   Apr-2011
// https://landenlabs.com/
// ------------------------------------------------------------------------------------------------

#pragma once

class Options
{
public:
    Options() :
        m_waitMin(0),
        m_repeat(1),
        m_colorize(false),
        m_showNetConn(false)
    { }

    double      m_waitMin;          // -w <minutes>
    unsigned    m_repeat;           // -r <num>
    bool        m_colorize;         // -C

    bool        m_showNetConn;      // -c
};