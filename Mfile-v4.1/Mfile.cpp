// ------------------------------------------------------------------------------------------------
// Mfile - list open handles or file system information
//
// Command line version of Magnifile C# application.
//
// Author:  DLang   2009  
// https://landenlabs.com
// ------------------------------------------------------------------------------------------------

#include <iostream>
#include <fstream>
#include <iomanip>
#include <assert.h>


#include "ListOpenHandles.h"
#include "NetSysInfo.h"
#include "Options.h"
#include "TeeStream.h"

extern int ListFileStats(const char* drivePath, const char* prefix, std::ostream& out);
extern void DisplayFileAllocation(const char* filePathOrPattern, bool showError, const char* prefix, std::ostream& out);
extern int DisplaySmartInfo(const char* prefix, std::ostream& out);
extern bool DisplayDiskGeometry(const char* prefix, std::ostream& out);
extern bool DisplayDiskAttributes(const char* prefix, std::ostream& out);
extern bool NtfsSysFiles(bool showError, const char* prefix, std::ostream& out);

using  namespace std;

const char usage[] =
    "Mfile v4.1 By DLang   " __DATE__ "\n"
    "\n"
    " List Open Handles and File System information\n"
    "\n"
    "File Handles:\n"
    "    -p processName ; Limit output to specified processName \n"
    "    -p pid         ; Limit output to specific process id \n"
    "    -y s|m|f|e|p|a ; Report s=summary, m=modules, f=files, e=events, p=procInfo, a=all \n"
    "    -r <repeat#>   ; Repeat output after waitMinutes \n"
    "    -w <waitMin>   ; WaitMinutes defaults to 0.1 \n"
    "    -C             ; Colorize: red below min, green above max, use with -r and -y s \n"
    "\n"
    "Other commands:\n"
    "    -a <filename>  ; Display file cluster allocation \n"
    "    -g             ; Display disk geometry \n"
    "    -A             ; Display disk Attributes \n"
    "    -c             ; Display network connections \n"
    "    -s             ; Display smart drive info \n"
    "    -z <drivePath> ; Display File System Stats\n"
    "\n"
    "Example:\n"
    "   mfile -y s              ; list handle summary for all processes\n"
    "   mfile -y a              ; list all open handles for all processes\n"
    "   mfile -p foobar -y s    ; list handle summary for process foobar \n"
    "   mfile -p foobar -y s -r 9999 -w 0.1  ; list handle summary for process foobar, every .1 minutes \n"
    "\n"
    "   mfile -z                ; list file system information\n"
    "   mfile -z \\\\.\\d       ; list file system information\n"
    "   mfile -a report.xls     ; list file allocation for file report.xls \n"
    "   mfile -a *.xls          ; \n"
    "\n";

// ------------------------------------------------------------------------------------------------
#ifndef HAVE_GETOPT
const char *optarg = NULL; /* argument associated with option */
int opterr   = 0;       /* if error message should be printed */
int optind   = 1;       /* index into parent argv vector */
int optopt   = 0;       /* character checked for validity */

#define BADCH (int)'?'
#define EMSG ""

// ------------------------------------------------------------------------------------------------
int getopt(int nargc, const char * nargv[], const char *ostr)
{
	static const char* place = EMSG; //  option letter processing 
	const char* oli;                 // option letter list index 
	const char* p;
	
	if (!*place) 
    { 
        // update scanning pointer 
		if (optind >= nargc || *(place = nargv[optind]) != '-') 
        {
			place = EMSG;
			return (EOF);
		}
		if (place[1] && *++place == '-') 
        {
            //  found "--" 
			++optind;
			place = EMSG;
			return (EOF);
		}
	} 
    
	if ((optopt = (int)*place++) == (int)':' || !(oli = strchr(ostr, optopt))) 
    {
		// For backwards compatibility: don't treat '-' as an
		// option letter unless caller explicitly asked for it.
		if (optopt == (int)'-')
			return (EOF);
		if (!*place)
			++optind;
		if (opterr) 
        {
			if (!(p = strrchr(*nargv, '/')))
				p = *nargv;
			else
				++p;
			fprintf(stderr, "%s: illegal option -- %c\n", p, optopt);
		}
		return (BADCH);
	}

	if (*++oli != ':') 
    { 
        // don't need argument 
		optarg = NULL;
		if (!*place)
			++optind;
	}
    else 
    { 
        // need an argument  
		if (*place)
        {
            // no white space 
			optarg = place;
        }
		else if (nargc <= ++optind) 
        { 
            //  no arg 
			place = EMSG;
			if (!(p = strrchr(*nargv, '/')))
				p = *nargv;
			else
				++p;
			if (opterr)
				fprintf(stderr, "%s: option requires an argument -- %c\n",
				p, optopt);
			return (BADCH);
		} else // white space 
			optarg = nargv[optind];
		place = EMSG;
		++optind;
	}

	return (optopt); // dump back option letter 
}
#endif
   
// ------------------------------------------------------------------------------------------------
// Toggle state of boolean if token exists in input argument

inline void ToggleBool(bool& outBool, const char* inArg, char inToken)
{
    outBool  = (strchr(inArg, inToken) != NULL) ? !outBool : outBool;
}

// ------------------------------------------------------------------------------------------------
// Redirect cout to file and optionally Tee it with cout

bool RedirectOutput(const char* outFile, bool tee)
{
    static std::ofstream fout;

    bool okay = true;
    fout.open(outFile, ios::app);
    if (fout)
    {
        if (tee)
            static basic_TeeStream<char> teeStream(std::cout, fout);
        else
            std::cout.rdbuf(fout.rdbuf());
    }
    else
        okay = false;

    return okay;
}

// ------------------------------------------------------------------------------------------------
int main(int argc, const char* argv[])
{
    if (argc == 1)
    {
        cout << usage << endl;
        return -1;
    }

    Options options;
    ListOpenHandles listOpenHandles;
    std::ostream& out = cout;
    
    // TODO - Add -o to specify output file

    int c;
    while ((c = getopt(argc, argv, "Aa:Ccgno:p:r:sw:y:z?h")) != -1)
    {
        switch (c)
        {
          case 'A':     // Report file allocation
            DisplayDiskAttributes("\t", out);
            return 0;

          case 'a':     // Report file allocation
            DisplayFileAllocation(optarg, true, "\t", out);
            return 0;

          case 'C':     // Colorize, use with -r, Note: not all displays support colorization
            options.m_colorize = true;
            break;

          case 'c':     // Display network connections
            options.m_showNetConn = true;
            break;
           
          case 'g':
            DisplayDiskGeometry("\t", out);
            return 0;

          case 'o':     // Output file, tee to file and console.
            if (!RedirectOutput(optarg, true))
                perror(optarg);
            break;

          case 'r':     // Repeat count (File Performance command)
            options.m_repeat = atoi(optarg);
            break;

          case 'w':     // WaitMinutes (File Performance command)
            options.m_waitMin = atof(optarg);
            break;

          case 'n':
            NtfsSysFiles(false, "\t", out);
            return 0;

          case 'p':
            listOpenHandles.m_process = optarg;
            break;

          case 's': // -s <drive#>, ex:  -s 0
            try
            {
                DisplaySmartInfo("\t", std::cout);
            } 
            catch (string& exMsg) 
            {
                std::cerr << exMsg << std::endl;
                return -1;
            }
            return 0;

          case 'y':
            ToggleBool(listOpenHandles.m_showSummary, optarg, 's');
            ToggleBool(listOpenHandles.m_showModules, optarg, 'm');
            ToggleBool(listOpenHandles.m_showFiles, optarg, 'f');
            ToggleBool(listOpenHandles.m_showEvents, optarg, 'e');
            ToggleBool(listOpenHandles.m_showAll, optarg, 'a');
            ToggleBool(listOpenHandles.m_showNet, optarg, 'n');
            ToggleBool(listOpenHandles.m_showHex, optarg, 'x');
            ToggleBool(listOpenHandles.m_showProcInfo, optarg, 'p');
            break;

          case 'z':
            return ListFileStats(argv[optind], "\t", out);

          default:
            cout << usage << endl;
            return -1;
        }
    }

    if (options.m_showNetConn)
    {
        PsUtil::EnableDebugPrivilege(); // Required to get process name for pid.
        NetworkSysInfo netSysInfo;
        size_t result = 0;

        for (unsigned repIdx = 0; repIdx < options.m_repeat; repIdx++)
        {
            if (repIdx != 0)
                Sleep((DWORD)(options.m_waitMin * 60 * 1000));

            bool showHeaders = ((repIdx % 30) == 0);
            result = netSysInfo.ListConnections(std::cout, options);
        }

        return (int)result;
    }

    int resultCode = listOpenHandles.ReportOpenFiles(cout, options);
	return resultCode;
}

