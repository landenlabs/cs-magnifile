// ------------------------------------------------------------------------------------------------
// Support classes
// Author:  DLang   2009  
// ------------------------------------------------------------------------------------------------

#pragma once



/// Simple smart pointer 
template < typename T > 
class ScopePtr
{
private:
    T*      m_pData; // Generic pointer to be stored
    size_t  m_size;

public:
    ScopePtr() : m_pData((T*)malloc(sizeof(T))), m_size(sizeof(T))
    { }

    ScopePtr(size_t size) : m_pData((T*)malloc(size)), m_size(size)
    { }

    ~ScopePtr()
    {
        free(m_pData);
    }

    ScopePtr& operator=(ScopePtr& rhs)
    {
        if (this != &rhs)
        {
            free(m_pData);
            m_pData = rhs.m_pData;
            rhs.m_pData = NULL;
        }
    }

    void Resize(size_t newSize)
    {
        m_pData = (T*)realloc(m_pData, newSize);
        m_size = newSize;
    }

    unsigned long Size() const { return (unsigned long)m_size;}
    operator T*()  { return m_pData; }
    T* operator-> () { return m_pData;}
};
