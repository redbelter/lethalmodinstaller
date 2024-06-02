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
           // CheckForVirus.CheckFileTest();
            var ret = StoreAPI.GetPackages();
            ret.Wait();
            foreach (var package in ret.Result)
            {
                if (package.Key.ToLower().Contains("BepInExPack".ToLower()))
                {
                    Console.WriteLine(package.Key);
                }
            }
            var z = ret.Result["BepInEx-BepInExPack"];
            var y = ret.Result["BepInEx-BepInExPack"].GetLatestVersion();
            y.DownloadPackage("temp.zip");
          
            var xx = CheckForVirus.CheckFile("temp.zip");
            xx.Wait();

            Console.ReadLine();
        }
    }
}
