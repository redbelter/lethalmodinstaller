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

        public Dictionary<string, List<ModInstallRequest>> AllMods = new Dictionary<string, List<ModInstallRequest>>();

        string defaultConfig = @"{""Lethal Company"": [
    ""BepInEx-BepInExPack"",
    ""x753-More_Suits"", 
   	""amnsoft-EmployeeAssignments"",
    ""JunLethalCompany-GamblingMachineAtTheCompany"",
    ""matsuura-HealthMetrics"",
    ""RickArg-Helmet_Cameras"",
    ""anormaltwig-LateCompany"",
    ""BatTeam-LethalFashion"",
    ""notnotnotswipez-MoreCompany"",
    ""Jordo-NeedyCats"",
    ""Midge-PushCompany"",
    ""tinyhoot-ShipLoot"",
    ""RugbugRedfern-Skinwalkers"",
    ""FlipMods-TooManyEmotes"",
    ""Verity-TooManySuits"",
 	""EliteMasterEric-Coroner"",
    ""FlipMods-LetMeLookDown"",
    ""TestAccount666-ShipWindows""
]
}";

        public List<ModInstallRequest> GetModsForGame(string game)
        {
            if (AllMods.ContainsKey(game))
            {
                return AllMods[game];
            } 
            else
            {
                throw new Exception("Could not find mod config for: " + game);
            }
        }

        public ModConfig()
        {
            string jsontxt = "";
            if (!File.Exists(DEFAULT_MOD_FILE))
            {
                Console.WriteLine("Could not find lethalmods.txt in current directory. Using red's recommended list.");
               // Console.ReadLine();
                jsontxt = defaultConfig;
            }
            else
            {
                Console.WriteLine("Reading config from lethalmods.txt");
                jsontxt = File.ReadAllText(DEFAULT_MOD_FILE);
            }
            dynamic array = JsonConvert.DeserializeObject(jsontxt);

            foreach(var game in array)
            {
                string nameOfGame = game.Name;
                var mods = game.Value;
                foreach (var item in mods)
                {
                    if (!AllMods.ContainsKey(nameOfGame))
                    {
                        AllMods.Add(nameOfGame, new List<ModInstallRequest>());
                    }

                    AllMods[nameOfGame].Add(new ModInstallRequest(item));
                }
            }
           
        }

        

    }
}
