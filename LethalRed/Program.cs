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
using System.Threading.Tasks;

namespace LethalRed
{
    internal class Program
    {
        static void Main(string[] args)
        {

            if(args.Length > 3)
            {
                LethalModUtil.WaitForScan = false;
            }
            
            if (!SteamUtil.IsLethalInstalled())
            {
                Console.WriteLine("Could not find Lethal company, exiting");
                return;
            }
            Console.WriteLine("Found lethal install path at: " + SteamUtil.GetSteamPath());

            if (LethalModUtil.IsAlreadyModded())
            {
                Console.WriteLine("Lethal appears to be modded already. Press enter to remove existing mods.");
                Console.ReadLine();
                LethalModUtil.CleanUpOldMods();
            } else {
                Console.WriteLine("Lethal does not appear modded from a quick look.");
            }

            ModConfig modsToInstall = ModConfig.GetInstance();
            LethalModUtil.CleanTempModFolder();


            foreach (ModInstallRequest request in modsToInstall.AllMods)
            {
                LethalModUtil.InstallModToTemp(request, true);
            }

            LethalModUtil.MoveTempModsToReal();

            Console.WriteLine("Done, press enter to exit");
            Console.ReadLine();
        }



    }
}
