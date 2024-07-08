using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LethalRed
{
    public class ModInstallGlobals
    {
        public static readonly string BEPINEX_FOLDER = "BepInEx";
        public static bool WaitForScan = true;
    }


    public abstract class IModInstallBasic : IModInstall
    {
        public bool IsGameInstalled()
        {
            return SteamUtil.IsGenericGameInstalled(this);
        }

        public bool IsModInstalled()
        {
            if (Directory.Exists(Path.Combine(SteamUtil.GetGenericGamePath(this), ModInstallGlobals.BEPINEX_FOLDER)))
            {
                return true;
            }
            return false;
        }

        public abstract string GetSteamGameName();

        public abstract string GetGameName();

        public bool CleanUpOldMods()
        {
            Console.WriteLine("This is a simple clean up, we just remove BepInexFolder for now");
            string beppath = Path.Combine(SteamUtil.GetGenericGamePath(this), ModInstallGlobals.BEPINEX_FOLDER);
            if (Directory.Exists(beppath))
            {
                Directory.Delete(beppath, true);
            }
            return true;
        }

        public string GetTempFolderName()
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            string str = rgx.Replace(GetGameName(), "");
            return "FAKE_" + str.Replace(" ", "");
        }

        public string GetTempFolderPath()
        {
            return Path.Combine(FileIO.GetExecutableCurrentDir(), GetTempFolderName());
        }

        public bool CleanTempModFolder()
        {
            if (Directory.Exists(GetTempFolderPath()))
            {
                Directory.Delete(GetTempFolderPath(), true);
            }
            return true;
        }

        public abstract bool InstallModsToTempFolder(ModInstallRequest req, bool blockOnVirus);

        public abstract bool MoveTempModsToReal();

        public string GetBepInExConfigFolderInTemp(bool createIfNotExist = true)
        {
            string bepinPath = Path.Combine(GetTempFolderPath(), "BepInEx");
            string bepinPathPlugin = Path.Combine(GetTempFolderPath(), "BepInEx", "config");
            if (!Directory.Exists(bepinPath))
            {
                if (createIfNotExist)
                {
                    Directory.CreateDirectory(bepinPath);
                }
            }

            if (!Directory.Exists(bepinPathPlugin))
            {
                if (createIfNotExist)
                {
                    Directory.CreateDirectory(bepinPathPlugin);
                }
            }
            return bepinPathPlugin;
        }

        public string GetBepInExPluginFolderInTemp(bool createIfNotExist = true)
        {
            string bepinPath = Path.Combine(GetTempFolderPath(), "BepInEx");
            string bepinPathPlugin = Path.Combine(GetTempFolderPath(), "BepInEx", "plugins");
            if (!Directory.Exists(bepinPath))
            {
                if (createIfNotExist)
                {
                    Directory.CreateDirectory(bepinPath);
                }
            }

            if (!Directory.Exists(bepinPathPlugin))
            {
                if (createIfNotExist)
                {
                    Directory.CreateDirectory(bepinPathPlugin);
                }
            }
            return bepinPathPlugin;
        }

        public void ModifyConfigFiles()
        {
            foreach (var file in FileIO.GetFiles(GetTempFolderPath()))
            {
                //TestAccount666.ShipWindows.cfg
                if (file.Contains("TestAccount666.ShipWindows.cfg"))
                {
                    string allText = File.ReadAllText(file);
                    allText = allText.Replace("Enable Wesley shutter voice lines = true", "Enable Wesley shutter voice lines = false");
                    allText = allText.Replace("Play Wesley shutter voice lines on transitions = true", "Play Wesley shutter voice lines on transitions = false");
                    File.WriteAllText(file, allText);
                } else
                {

                }

            }
        }

    }

    public interface IModInstall
    {
        string GetSteamGameName();

        string GetGameName();

        bool IsGameInstalled();

        bool IsModInstalled();

        bool CleanUpOldMods();

        bool CleanTempModFolder();

        bool InstallModsToTempFolder(ModInstallRequest req, bool blockOnVirus);

        bool MoveTempModsToReal();

        string GetTempFolderName();
        string GetTempFolderPath();

        void ModifyConfigFiles();

        string GetBepInExConfigFolderInTemp(bool createIfNotExist);

        string GetBepInExPluginFolderInTemp(bool createIfNotExist);

    }
}
