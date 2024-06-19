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
                Console.WriteLine("This program mods lethal company and checks the mods against virustotal.");
                Console.WriteLine();
                Console.WriteLine("The virus check can take 5-10 minutes, do you want to scan the mods for viruses?");
                Console.WriteLine("\t(type 'scan' + enter to scan, or just press enter install as fast as possible)");
                Console.Write("> ");

                string dontcarecheck = Console.ReadLine();
                if (dontcarecheck != null && dontcarecheck.Equals("scan", StringComparison.InvariantCultureIgnoreCase))
                {
                    LethalModUtil.WaitForScan = true;
                    Console.WriteLine("Will scan for viruses...");
                    Thread.Sleep(1000);
                }
                else
                {
                    LethalModUtil.WaitForScan = false;
                    Console.WriteLine("Skipping virus check confirmed...");
                    Thread.Sleep(1000);
                }

                if (!SteamUtil.IsLethalInstalled())
                {
                    Console.WriteLine("Could not find Lethal company, exiting");
                    return;
                }
                Console.WriteLine("Found lethal install path at: " + SteamUtil.GetLethalCompanyPath());

                if (LethalModUtil.IsAlreadyModded())
                {
                    Console.WriteLine("Lethal appears to be modded already.");
                    Console.WriteLine("Press enter to remove existing mods.");
                    Console.Write("> ");
                    Console.ReadLine();
                    LethalModUtil.CleanUpOldMods();
                }
                else
                {
                    Console.WriteLine("Lethal does not appear modded from a quick look.");
                }

                ModConfig modsToInstall = ModConfig.GetInstance();
                LethalModUtil.CleanTempModFolder();

                int current = 1;
                int max = modsToInstall.AllMods.Count;
                foreach (ModInstallRequest request in modsToInstall.AllMods)
                {
                    Console.WriteLine("Installing mod " + current + " out of " + max);
                    LethalModUtil.InstallModToTemp(request, true);
                    current++;
                }

                LethalModUtil.MoveTempModsToReal();

                Console.WriteLine("Done modding lethal company! Thanks for using this. Press enter to exit");
                Console.ReadLine();
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
