using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace LethalRed
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            if (!SteamUtil.IsLethalInstalled())
            {
                Console.WriteLine("Could not find Lethal company, exiting");
                return;
            }
            Console.WriteLine("Found lethal install path at: " + SteamUtil.GetSteamPath());

            if (LethalModUtil.IsAlreadyModded())
            {
                Console.WriteLine("Lethal appears to be modded already. Press enter to remove existing mods.");
                Console.ReadLine();
                LethalModUtil.CleanUpOldMods();
            } else
            {
                Console.WriteLine("Lethal does not appear modded from a quick look.");
            }

            ModConfig modsToInstall = ModConfig.GetInstance();

            if (Directory.Exists(FAKE_LETHAL))
            {
                Directory.Delete(FAKE_LETHAL, true);
            }

            foreach (ModInstallRequest request in modsToInstall.AllMods)
            {
                InstallMod(request, true);
            }
            List<string> files = new List<string>();

            foreach (string file in GetFiles(FAKE_LETHAL))
            {
                files.Add(file);
            }

            DeleteFileIfExists(Path.Combine(FAKE_LETHAL, MODDEDLIST));
            var serializer = JsonSerializer.Create(_settings);
            var stringBuilder = new StringBuilder();
            using (var writer = new JsonTextWriter(new StringWriter(stringBuilder)))
            {
                serializer.Serialize(writer, files);
            }
            var moddedjson = stringBuilder.ToString();
            File.WriteAllText(Path.Combine(FAKE_LETHAL, MODDEDLIST), moddedjson);

            Console.WriteLine("About to write to lethal folder, if you want to preview it you can go here: " + Environment.NewLine + FAKE_LETHAL);
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
            Console.WriteLine("Starting copy into: " + SteamUtil.GetLethalCompanyPath());
            CopyFilesRecursively(FAKE_LETHAL, SteamUtil.GetLethalCompanyPath());
            Console.WriteLine("Done copying");

            Console.WriteLine("Done, press enter to exit");
            Console.ReadLine();
        }

        private static readonly string TEMP_DIRECTORY = "tempout";
        private static readonly string FAKE_LETHAL = "fakelethal";
        private static readonly string MODDEDLIST = "moddedfiles.txt";

        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
        };

        static void InstallMod(ModInstallRequest req, bool blockOnVirus)
        {

            // CheckForVirus.CheckFileTest();
            var ret = StoreAPI.GetPackages();
            ret.Wait();
            KeyValuePair<string, ModPackage>? yz = ret.Result.FirstOrDefault(x => x.Key.ToLower().Equals(req.FullName.ToLower()));
            if(yz == null) {
                throw new Exception("can't install package, can't find it" + req.FullName);
            }
            Console.WriteLine("Downloading " + req.FullName);
            var latest = yz.Value.Value.GetLatestVersion();
            File.Delete("temp.zip");
            latest.DownloadPackage("temp.zip");

            Console.WriteLine("Scanning " + req.FullName);
            var xx = CheckForVirus.CheckFile("temp.zip");
            xx.Wait();
            if (!xx.Result)
            {
                Console.WriteLine("File might be a virus, not installing");
                File.Delete("temp.zip");
            } else
            {
                //Install
                Console.WriteLine("Installing " + req.FullName);
                if (Directory.Exists(TEMP_DIRECTORY))
                {
                    Directory.Delete(TEMP_DIRECTORY, true);
                }
                Directory.CreateDirectory(TEMP_DIRECTORY);
                ZipFile.ExtractToDirectory("temp.zip", TEMP_DIRECTORY);

                //Remove garbage files
                DeleteFileIfExists(Path.Combine(TEMP_DIRECTORY, "README.md"));
                DeleteFileIfExists(Path.Combine(TEMP_DIRECTORY, "manifest.json"));
                DeleteFileIfExists(Path.Combine(TEMP_DIRECTORY, "icon.png"));
                DeleteFileIfExists(Path.Combine(TEMP_DIRECTORY, "CHANGELOG.md"));
                

                if (req.FullName.Equals("BepInEx-BepInExPack", StringComparison.OrdinalIgnoreCase))
                {
                    CopyFilesRecursively(Path.Combine(TEMP_DIRECTORY, "BepInExPack"), FAKE_LETHAL);
                }
                else
                {
                    CopyFilesRecursively(TEMP_DIRECTORY, FAKE_LETHAL);
                }
                File.Delete("temp.zip");

                
            }
        }

        private static IEnumerable<string> GetFiles(string path)
        {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                try
                {
                    foreach (string subDir in Directory.GetDirectories(path))
                    {
                        queue.Enqueue(subDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
                string[] files = null;
                try
                {
                    files = Directory.GetFiles(path);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        yield return files[i];
                    }
                }
            }
        }

        private static void DeleteFileIfExists(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
    }
}
