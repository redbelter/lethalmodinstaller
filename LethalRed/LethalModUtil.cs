using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LethalRed
{
    public class LethalModUtil
    {

        public static bool IsAlreadyModded()
        {
            if(Directory.Exists(Path.Combine(SteamUtil.GetLethalCompanyPath(), "BepInEx")))
            {
                return true;
            }
            return false;
        }

        public static void CleanUpOldMods()
        {

        }
    }
}
