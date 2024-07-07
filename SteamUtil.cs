using Gameloop.Vdf;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LethalRed
{
    public class SteamUtil
    {

        private static string PATH_TO_GAMES = "steamapps\\common\\";
        public static string GetSteamPath()
        {
            string InstallPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam", "InstallPath", null);
            if (InstallPath != null)
            {
                return InstallPath;
            }
            else
            {
                throw new Exception("cant find steam!");
            }
        }

        private static List<string> libscache = new List<string>();

        public static string[] GetLibraryPaths()
        {
            if(libscache.Count != 0)
            {
                return libscache.ToArray();
            }
            string SteamPath = GetSteamPath();
            string libraryvdf = Path.Combine(SteamPath, "config", "libraryfolders.vdf");
            string defaultLibrary = Path.Combine(SteamPath, PATH_TO_GAMES);
            List<string> libs = new List<string>();
           
            if (File.Exists(libraryvdf))
            {
                string vdfjson = File.ReadAllText(libraryvdf);
                dynamic array = VdfConvert.Deserialize(vdfjson);
                for(int i = 0; i < 10; i++)
                {
                    try
                    {
                        string potpath = array.Value[i].Value.path.ToString();
                        potpath = Path.Combine(potpath, PATH_TO_GAMES);

                        if (potpath != null && Directory.Exists(potpath))
                        {
                            Console.WriteLine("Found library path: " + potpath);
                            libs.Add(potpath);
                        }
                    }
                    catch
                    {
                        //oh well, we're likely done
                        break;
                    }
                }
            }
            if (libs.Count == 0)
            {
                Console.WriteLine("We couldn't parse library vdf, let's try default lib path");
                if (Directory.Exists(defaultLibrary))
                {
                    Console.WriteLine("Default library path: " + defaultLibrary);
                    libs.Add(defaultLibrary);
                }
            }

            libscache = libs;
            return libscache.ToArray();
        }

        public static string GetLethalCompanyPath()
        {
            foreach (string lib in GetLibraryPaths())
            {
                string test = Path.Combine(lib, "Lethal Company");
                if (Directory.Exists(test))
                {
                    return test;
                }
            }

            throw new Exception("Could not find lethal company :(");
        }


        public static bool IsLethalInstalled()
        {
            return Directory.Exists(GetLethalCompanyPath());
        }
        public static bool IsCWInstalled()
        {
            return Directory.Exists(GetCWPath());
        }

        public static string GetCWPath()
        {
            foreach (string lib in GetLibraryPaths())
            {
                string test = Path.Combine(lib, "Content Warning");
                if (Directory.Exists(test))
                {
                    return test;
                }
            }

            throw new Exception("Could not find Content Warning :(");
        }

    }
}
