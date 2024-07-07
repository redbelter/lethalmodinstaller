using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LethalRed
{
    public class LethalModInstaller : IModInstall
    {
        public bool CleanTempModFolder()
        {
            LethalModUtil.CleanTempModFolder();
            return true;
        }

        public bool CleanUpOldMods()
        {
            LethalModUtil.CleanUpOldMods();
            return true;
        }

        public bool InstallModsToTempFolder(ModInstallRequest req, bool blockOnVirus)
        {
            LethalModUtil.InstallModToTemp(req, blockOnVirus);
            return true;
        }

        public bool IsGameInstalled()
        {
            return SteamUtil.IsLethalInstalled();
        }

        public bool IsModInstalled()
        {
            return LethalModUtil.IsAlreadyModded();
        }

        public bool MoveTempModsToReal()
        {
            LethalModUtil.MoveTempModsToReal();
            return true;
        }
    }
}
