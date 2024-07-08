using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LethalRed
{
    public class CWModInstaller : IModInstallBasic
    {
        public override string GetSteamGameName()
        {
            return "Content Warning";
        }

        public override string GetGameName()
        {
            return "Content Warning";
        }

      
       

        public override bool InstallModsToTempFolder(ModInstallRequest req, bool blockOnVirus)
        {
            throw new NotImplementedException();
        }

     
        public override bool MoveTempModsToReal()
        {
            throw new NotImplementedException();
        }
    }
}
