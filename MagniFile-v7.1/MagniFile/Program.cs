using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using System.Runtime.InteropServices;

namespace MagniFile
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        /// <summary>
        /// Display and log information about files.
        /// 
        /// Author: Dennis Lang 2009
        /// https://landenlabs.com/
        /// 
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // redirect console output to parent process;
            // must be before any calls to Console.WriteLine()
            AttachConsole(ATTACH_PARENT_PROCESS);

            System.Console.WriteLine(MainForm.ProductNameAndVersion());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ///
            /// Parse command line switches
            /// 
            /// Magnifile.exe  -graph Azulon.exe  -graphParam PrivateMem -graphXtime 1hour
            /// 
            Arguments cmdArgs = new Arguments(args);
            if (cmdArgs["help"] != null)
            {
                System.Console.WriteLine(
                      "-graphProc <process>    ; Start logging and graphic process\n"
                    + "-graphParam <parameter> ; select parameter to graph, default PrivateMem\n"
                    + "-graphXtime <time>      ; time is number [min|hour|day] \n"
                    + "-plog <filename>        ; set process log filename\n"
                    + "-hlog <filename>        ; set handle log filename\n"
                    + "-prate <seconods>       ; set process update rate\n"
                    + "-hrate <seconods>       ; set handle update rate\n"
                    + "-proc <procName>        ; set log flag on process\n"
                    + "-diskmap <drive>        ; export disk allocation map\n"
                    + "-savemap <filename>     ;   save to filename\n"
                    + "-filemap <filename>     ; export file alloocation map\n"
                    + "-stats <whichStats>     ; export disk stats\n"
                    + "-savestats <filename>   ;   save to filename\n"
                    + "-hide                   ; hide main dialog, use with -diskmap, ...\n"
                    + "-exit                   ; exit\n"
                    );
                return;
            }

            string drive = string.Empty;
            if (cmdArgs.TryAndGet("diskmap", ref drive))
            {
                ViewDiskAllocation viewDiskAllocation = new ViewDiskAllocation(drive);
                viewDiskAllocation.Show();
                string saveAs = string.Empty;
                if (cmdArgs.TryAndGet("savemap", ref saveAs))
                {
                    viewDiskAllocation.MakeScreenImage().Save(saveAs);
                    return;
                }

                Application.Run(viewDiskAllocation);
                return;
            }

            string filemap = string.Empty;
            if (cmdArgs.TryAndGet("filemap", ref filemap))
            {
                ViewDiskAllocation viewDiskAllocation = new ViewDiskAllocation(filemap);
                viewDiskAllocation.Show();
                string saveAs = string.Empty;
                if (cmdArgs.TryAndGet("savemap", ref saveAs))
                {
                    viewDiskAllocation.MakeScreenImage().Save(saveAs);
                    return;
                }

                Application.Run(viewDiskAllocation);
                return;
            }

            string stats = string.Empty;
            if (cmdArgs.TryAndGet("stats", ref stats))
            {
                ViewFsInfo fsInfoDialog = new ViewFsInfo(null);
                fsInfoDialog.ShowStats(stats);
                fsInfoDialog.ExitOnClose = true;
                fsInfoDialog.Show();
                string saveAs = string.Empty;
                if (cmdArgs.TryAndGet("savestats", ref saveAs))
                {
                    fsInfoDialog.Export(saveAs);
                    return;
                }

                Application.Run(fsInfoDialog);
                return;
            }

            if (cmdArgs["exit"] != null)
            {
                return;
            }

            MainForm mainForm = new MainForm(args);
            if (mainForm.IsDisposed == false)
                Application.Run(mainForm);

            System.Console.WriteLine("Done");
            System.Environment.Exit(0);
           
        }
    }
}
