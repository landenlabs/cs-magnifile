using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//  http://www.dotnetmonster.com/Uwe/Forum.aspx/dotnet-clr/654/FormatMessage-in-C-only-looks-for-system-messages

using System.Runtime.InteropServices;
using System.Security.Principal;

namespace MagniFile
{
    /// 
    /// Get Windows expanded error message.
    /// 
    /// Author: Dennis Lang 2009
    /// https://landenlabs.com/
    /// 
    class ErrorMsg
    {
       public const uint LOAD_LIBRARY_AS_DATAFILE =       0x00000002; 
       public const uint FORMAT_MESSAGE_FROM_HMODULE =    0x00000800; 
       public const uint FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100; 
       public const uint FORMAT_MESSAGE_IGNORE_INSERTS =  0x00000200; 
       public const uint FORMAT_MESSAGE_FROM_SYSTEM =     0x00001000; 

      
       [DllImport( "kernel32.dll", CharSet=CharSet.Auto )]
       private static extern int FormatMessageW(
           uint dwFormatFlags, IntPtr lpSource, int dwMessageId,
           int dwLanguageId, out IntPtr MsgBuffer, int nSize, IntPtr Arguments );

#if false
       [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
       private static extern IntPtr LoadLibraryEx(
           [MarshalAs(UnmanagedType.LPTStr)] string lpFileName, IntPtr hFile, uint dwFlags);

       public static string GetMessage(int id, string dllFile)
       {
           IntPtr hModule = IntPtr.Zero;
           IntPtr pMessageBuffer;
           int dwBufferLength;
           string sMsg = "";
           uint  dwFormatFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM
                                   | FORMAT_MESSAGE_IGNORE_INSERTS;

           hModule = LoadLibraryEx( dllFile, // dll or exe file
                                   IntPtr.Zero, // null for future use
                                   LOAD_LIBRARY_AS_DATAFILE); // only to extract messages 

           if(IntPtr.Zero != hModule)
           {
                       dwFormatFlags |= FORMAT_MESSAGE_FROM_HMODULE;
                       Console.WriteLine("\n > Found hmodule for: " + dllFile );
           }

           dwBufferLength = FormatMessageW( 
                dwFormatFlags,  // formatting options
                hModule,        // dll file message
                id,             // Message identifier
                0,              // Language identifier
                out pMessageBuffer, // Pointer to a buffer
                0,              // Minimum number of chars to write in pMessageBuffer
                IntPtr.Zero );  // Pointer to an array of insertion strings
                                 
           if (0 != dwBufferLength) 
           {
               sMsg = Marshal.PtrToStringUni(pMessageBuffer);
               Marshal.FreeHGlobal(pMessageBuffer);
           }  
 
           return sMsg;
       }
#endif

        public class Win32Exception : Exception
        {
            int winErr;
            int status;
            string path;

            public Win32Exception(int _winErr, int _status, string _fileName)
            {
               winErr = _winErr;
               status = _status;
               path = _fileName;
            }

            public int Error
            {
                get { return winErr; }
            }
            public int Status
            {
                get { return status; }
            }
            public string Path
            {
                get { return path; }
            }

            public override string Message
            {
                get
                {
                    return GetMessage(winErr);
                }   
            }
       }

       public static string GetMessage(int winErr)
       {
           IntPtr pMessageBuffer;
           uint dwFormatFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM;

           int dwBufferLength = FormatMessageW(
                dwFormatFlags,      // formatting options
                IntPtr.Zero,        // dll file message
                winErr,             // Message identifier
                0,                  // Language identifier
                out pMessageBuffer, // Pointer to a buffer
                0,                  // Minimum number of chars to write in pMessageBuffer
                IntPtr.Zero);       // Pointer to an array of insertion strings

           string sMsg = "";
           if (0 != dwBufferLength)
           {
               sMsg = Marshal.PtrToStringUni(pMessageBuffer);
               char[] crlf = new char[]{'\r', '\n'};
               sMsg = sMsg.TrimEnd(crlf);
               Marshal.FreeHGlobal(pMessageBuffer);
           }

           return sMsg;
       }
   }
}
