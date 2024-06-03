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
            if (request.Length == 3)
            {
                VersionSpecified = true;
                Version = request[2];
                ProperVersion = new Version(request[2]);
            } else {
                VersionSpecified = false;
            }
        }

        public string FullName { get; set; }
        public string Name { get; set; }

        public bool VersionSpecified { get; set; }
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

        public ModConfig()
        {
            if (!File.Exists(DEFAULT_MOD_FILE))
            {
                throw new Exception("Mod description file doesn't exist");
            }
            string jsontxt = File.ReadAllText(DEFAULT_MOD_FILE);
            dynamic array = JsonConvert.DeserializeObject(jsontxt);

            foreach (var item in array)
            {
                AllMods.Add(new ModInstallRequest(item));
            }
        }

        

    }
}
