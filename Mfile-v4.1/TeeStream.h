// ------------------------------------------------------------------------------------------------
//  llmsg - Simple info, debug and error message presentation classes
//
//  Dennis Lang 2013
//  https://landenlabs.com/
// ------------------------------------------------------------------------------------------------


// #include <Windows.h 
#include <iostream>
#include <sstream>
#include <iomanip>

//
// http://www.cs.technion.ac.il/~imaman/programs/teestream.html
//
template<typename Elem, typename Traits = std::char_traits<Elem> >
struct basic_TeeStream : std::basic_ostream<Elem,Traits>
{
   typedef std::basic_ostream<Elem,Traits> SuperType;

   basic_TeeStream(std::ostream& o1, std::ostream& o2)
      :  SuperType(o1.rdbuf()), o1_(o1), o2_(o2) { }

   basic_TeeStream& operator<<(SuperType& (__cdecl *manip)(SuperType& ))
   {
      o1_ << manip;
      o2_ << manip;
      return *this;
   }

   template<typename T>
   basic_TeeStream& operator<<(const T& t)
   {
      o1_ << t;
      o2_ << t;
      return *this;
   }

private:
   std::ostream& o1_;
   std::ostream& o2_;
};