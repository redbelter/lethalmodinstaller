using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LethalRed
{

    public class ModInstallRequest
    {
        public ModInstallRequest(dynamic item) 
        {
            //Right now it's just a string
            string txt = item.ToString();
            string[] request = txt.Split('-');
            if(request.Length < 2)
            {
                throw new Exception("invalid package specification, needs package and name");
            }
            FullName = request[0] + "-" + request[1];
            Name = request[1];
            if (request.Length > 3)
            {
                VersionSpecified = true;
                Version = request[2];
                ProperVersion = new Version(request[2]);
            } else {
                VersionSpecified = false;
            }
            if (request.Length >= 4)
            {
                CopyIntoPlugin = true;
                
            } else
            {
                CopyIntoPlugin = false;
            }
        }

        public string FullName { get; set; }
        public string Name { get; set; }

        public bool VersionSpecified { get; set; }
        public bool CopyIntoPlugin { get; set; }
        public Version ProperVersion { get; set; }
        public string Version { get; set; }

    }

    public class ModConfig
    {
        public static readonly string DEFAULT_MOD_FILE = "lethalmods.txt";
        public static ModConfig instance = null;
        public static ModConfig GetInstance()
        {
            if(instance == null)
            {
                instance = new ModConfig();
            }
            return instance;
        }

        public List<ModInstallRequest> AllMods = new List<ModInstallRequest>();

        string defaultConfig = @"
[
    ""BepInEx-BepInExPack-5.4.2100"",
    ""x753-More_Suits-1.4.3"", 
   	""amnsoft-EmployeeAssignments-1.0.0"",
    ""JunLethalCompany-GamblingMachineAtTheCompany-1.3.5"",
    ""matsuura-HealthMetrics-1.0.2"",
    ""RickArg-Helmet_Cameras-2.1.5"",
    ""anormaltwig-LateCompany-1.0.13"",
    ""BatTeam-LethalFashion-1.0.7"",
    ""notnotnotswipez-MoreCompany-1.9.1"",
    ""Jordo-NeedyCats-1.2.1"",
    ""Midge-PushCompany-1.2.0"",
    ""tinyhoot-ShipLoot-1.1.0"",
    ""RugbugRedfern-Skinwalkers-5.0.0"",
    ""FlipMods-TooManyEmotes-2.1.18"",
    ""Verity-TooManySuits-1.0.9""
]";

        public ModConfig()
        {
            string jsontxt = "";
            if (!File.Exists(DEFAULT_MOD_FILE))
            {
                Console.WriteLine("Could not find lethalmods.txt in current directory. Do you want to use red's recommended list? If so press enter");
                Console.ReadLine();
                jsontxt = defaultConfig;
            }
            else
            {
                Console.WriteLine("Reading config from lethalmods.txt");
                jsontxt = File.ReadAllText(DEFAULT_MOD_FILE);
            }
            dynamic array = JsonConvert.DeserializeObject(jsontxt);

            foreach (var item in array)
            {
                AllMods.Add(new ModInstallRequest(item));
            }
        }

        

    }
}
