using LethalRed.ModInstallers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LethalRed
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("This program mods games and checks the mods against virustotal.");
                Console.WriteLine();
                Console.WriteLine("The virus check can take 5-10 minutes, do you want to scan the mods for viruses?");
                Console.WriteLine("\t(type 'scan' + enter to scan, or just press enter install as fast as possible)");
                Console.Write("> ");

                string dontcarecheck = Console.ReadLine();
                if (dontcarecheck != null && dontcarecheck.Equals("scan", StringComparison.InvariantCultureIgnoreCase))
                {
                    ModInstallGlobals.WaitForScan = true;
                    Console.WriteLine("Will scan for viruses...");
                    Thread.Sleep(1000);
                }
                else
                {
                    ModInstallGlobals.WaitForScan = false;
                    Console.WriteLine("Skipping virus check confirmed...");
                    Thread.Sleep(1000);
                }

                GenericModInstallFlow.AskWhichGame();
                

                
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine();
                Console.WriteLine("Something went wrong, please send the above text to red.");
                Console.ReadLine();
            }
        }



    }
}
