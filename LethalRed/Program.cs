using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LethalRed
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(SteamUtil.IsLethalInstalled());
            Console.WriteLine(LethalModUtil.IsAlreadyModded());
            ModConfig modsToInstall = ModConfig.GetInstance();

            foreach(ModInstallRequest request in modsToInstall.AllMods)
            {
                InstallMod(request, true);
            }


            Console.ReadLine();
        }

        static void InstallMod(ModInstallRequest req, bool blockOnVirus)
        {

            // CheckForVirus.CheckFileTest();
            var ret = StoreAPI.GetPackages();
            ret.Wait();
            KeyValuePair<string, ModPackage>? yz = ret.Result.FirstOrDefault(x => x.Key.ToLower().Equals(req.FullName.ToLower()));
            if(yz == null) {
                throw new Exception("can't install package, can't find it" + req.FullName);
            }
            Console.WriteLine("Downloading " + req.FullName);
            var latest = yz.Value.Value.GetLatestVersion();
            latest.DownloadPackage("temp.zip");

            /*
            foreach (var package in ret.Result)
            {
                if (package.Key.ToLower().Contains(req.FullName.ToLower()))
                {
                    Console.WriteLine(package.Key);
                }
            }
            var z = ret.Result["BepInEx-BepInExPack"];
            var y = ret.Result["BepInEx-BepInExPack"].GetLatestVersion();*/
            //y.DownloadPackage("temp.zip");
            Console.WriteLine("Scanning " + req.FullName);
            var xx = CheckForVirus.CheckFile("temp.zip");
            xx.Wait();
            if (!xx.Result)
            {
                Console.WriteLine("File might be a virus, not installing");
            } else
            {
                //Install
                Console.WriteLine("Installing " + req.FullName);
            }
        }
    }
}
