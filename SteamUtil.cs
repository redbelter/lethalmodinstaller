using Microsoft.Win32;
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

        public static string GetLethalCompanyPath()
        {
            string steamPath = GetSteamPath();
            string PathToLethal = Path.Combine(steamPath, PATH_TO_GAMES);
            PathToLethal = Path.Combine(PathToLethal, "Lethal Company");
            return PathToLethal;
        }

        public static bool IsLethalInstalled()
        {
            return Directory.Exists(GetLethalCompanyPath());
        }
    }
}
