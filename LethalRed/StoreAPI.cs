using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LethalRed
{

    public class ModPackage
    {
        public ModPackage(dynamic item)
        {
            Name = item.name;
            FullName = item.full_name;
            PackageURL = item.package_url;
            Versions = new List<ModPackageVersion>();
            
            foreach (var item2 in item.versions)
            {
                Versions.Add(new ModPackageVersion(item2, this));
                
            }
        }

        public string Name { get; set; }
        public string FullName { get; set; }
        public string PackageURL { get; set; }

        public List<ModPackageVersion> Versions { get; set; }

        public ModPackageVersion GetLatestVersion()
        {
            return Versions.OrderByDescending(x => x.ProperVersion).First();
        }
    }

    public class ModPackageVersion
    {
        public ModPackageVersion(dynamic item2, ModPackage parent)
        {
            DateCreated = item2.date_created;
            VersionNumber = item2.version_number;
            DownloadURL = item2.download_url;
            ProperVersion = new Version(item2.version_number);
            this.parent = parent;
        }

        public ModPackage parent { get; set; }

        public string DateCreated { get; set; }
        public string VersionNumber { get; set; }

        public Version ProperVersion { get; set; }
        public string DownloadURL { get; set; }

        public void DownloadPackage(string destination)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(this.DownloadURL, destination);
            }
        }
    }

    public class StoreAPI
    {

        
        private static HttpClient sharedClient = new HttpClient()
        {
            BaseAddress = new Uri("https://thunderstore.io"),
        };
        public static async Task<Dictionary<string, ModPackage>> GetPackages()
        {
            //https://thunderstore.io/api/v1/package/

            Dictionary<string, ModPackage> packages = new Dictionary<string, ModPackage>();
            ///c/{community_identifier}/api/v1/package/
            //string ret = await sharedClient.GetStringAsync("/api/v1/package/");
            string ret = await sharedClient.GetStringAsync("/c/lethal-company/api/v1/package/");
            dynamic array = JsonConvert.DeserializeObject(ret);
            packages.Clear();
            foreach (var item in array)
            {
                ModPackage x = new ModPackage(item);
                if(x.FullName == null)
                {
                    throw new Exception("wtf");
                }
                if (packages.ContainsKey(x.FullName))
                {
                    //throw new Exception(":(");
                    //Just skip it
                }
                else
                {
                    packages.Add(x.FullName, x);
                }
            }
            return packages;
        }
    }
}
