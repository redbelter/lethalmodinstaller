using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LethalRed
{
    public interface IModInstall
    {

        bool IsGameInstalled();

        bool IsModInstalled();

        bool CleanUpOldMods();

        bool CleanTempModFolder();

        bool InstallModsToTempFolder(ModInstallRequest req, bool blockOnVirus);

        bool MoveTempModsToReal();
    }
}
