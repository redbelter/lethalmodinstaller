using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LethalRed
{
    public class ModInstallGlobals
    {
        public static readonly string BEPINEX_FOLDER = "BepInEx";
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

        public abstract bool CleanTempModFolder();

        public abstract bool InstallModsToTempFolder(ModInstallRequest req, bool blockOnVirus);

        public abstract bool MoveTempModsToReal();
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
    }
}
