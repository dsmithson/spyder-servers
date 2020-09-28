using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vista.Diagnostics;
using Vista.SystemManager;

namespace SpyderServerShim
{
    class Program
    {
        private static SystemMgr sys;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting server version 1.0");

            TraceQueue.TracePosted += TraceQueue_TracePosted;

            sys = new SystemMgr();
            sys.Post();
            sys.AppInit();

            //Optionally we can run immediate and exit to generate default files
            if(args.Contains("-InitOnly"))
            {
                sys.Save();
                sys.SaveConfigFile();
                sys.Shutdown();
                return;
            }

            //Init remoting interface
            sys.StartupRemotingInterface();

            //Now we sleep/wait forever
            await Task.Delay(Timeout.Infinite);
        }

        private static void TraceQueue_TracePosted(object sender, TracingEvent e)
        {
            Console.WriteLine($"[{e.Time}]\t{e.Level}\t{e.Source}\t{e.Message}");
        }
    }
}
