// ------------------------------------------------------------------------------------------------
// Smart file handle class. Close when it goes out of scope.
//
// Project: NTFSfastFind
// Author:  Dennis Lang   Apr-2011
// https://landenlabs.com/
// ------------------------------------------------------------------------------------------------

#pragma once

#include <Windows.h>

// Smart Windows HANDLE, closes handle when it goes out of scope.
class Hnd
{
public:
    Hnd(HANDLE handle = INVALID_HANDLE_VALUE) : m_handle(handle)
    { }

    ~Hnd()
    {
        Close();
    }

    Hnd& operator=(HANDLE other)
    {
        if (other != m_handle)
            Close();
        m_handle = other;
        return *this;
    }

#if 0
    HANDLE Duplicate() const
    {
        if (IsValid())
        {
            HANDLE handle;
            if (::DuplicateHandle(
                    ::GetCurrentProcess(), 
                    m_handle, 
                    ::GetCurrentProcess(),
                    &handle, 
                    0,
                    FALSE,
                    DUPLICATE_SAME_ACCESS))
            {
                return handle;
            }
        }

        return INVALID_HANDLE_VALUE;
    }
#endif

    bool IsValid() const
    { return m_handle != INVALID_HANDLE_VALUE; }

    operator HANDLE& ()
    {  return m_handle; }

    void Close()
    {
        if (IsValid())
        {
            CloseHandle(m_handle);
            m_handle = INVALID_HANDLE_VALUE;
        }
    }

    HANDLE m_handle;
};

