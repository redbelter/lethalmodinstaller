using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LethalRed
{
    public class CWModInstaller : IModInstall
    {
        public string GetGameName()
        {
            return "Content Warning";
        }

        public bool CleanTempModFolder()
        {
            throw new NotImplementedException();
        }

        public bool CleanUpOldMods()
        {
            throw new NotImplementedException();
        }

        public bool InstallModsToTempFolder(ModInstallRequest req, bool blockOnVirus)
        {
            throw new NotImplementedException();
        }

        public bool IsGameInstalled()
        {
            return SteamUtil.IsCWInstalled();
        }

        public bool IsModInstalled()
        {
            throw new NotImplementedException();
        }

        public bool MoveTempModsToReal()
        {
            throw new NotImplementedException();
        }
    }
}
