﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LethalRed
{
    public class LethalModUtil
    {

        public static bool WaitForScan = true;
        private static readonly string TEMP_DIRECTORY = "tempout";
        private static readonly string FAKE_LETHAL = "fakelethal";
        private static readonly string MODDEDLIST = "moddedfiles.txt";
        public static readonly string BEPINEX_FOLDER = "BepInEx";

        public static bool IsAlreadyModded()
        {
            if(Directory.Exists(Path.Combine(SteamUtil.GetLethalCompanyPath(), BEPINEX_FOLDER)))
            {
                return true;
            }
            return false;
        }

        public static void CleanUpOldMods()
        {

            Console.WriteLine("This is a simple clean up, we just remove BepInexFolder for now");
            string beppath = Path.Combine(SteamUtil.GetLethalCompanyPath(), BEPINEX_FOLDER);
            if (Directory.Exists(beppath))
            {
                Directory.Delete(beppath, true);
            }
        }

        public static void CleanTempModFolder()
        {

            if (Directory.Exists(LethalModUtil.FAKE_LETHAL))
            {
                Directory.Delete(LethalModUtil.FAKE_LETHAL, true);
            }
        }

        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
        };
        public static void GenerateModFileList()
        {
            List<string> files = new List<string>();

            foreach (string file in FileIO.GetFiles(FAKE_LETHAL))
            {
                files.Add(file.Replace(FAKE_LETHAL, "").Substring(1));
            }

            FileIO.DeleteFileIfExists(Path.Combine(FAKE_LETHAL, MODDEDLIST));
            var serializer = JsonSerializer.Create(_settings);
            var stringBuilder = new StringBuilder();
            using (var writer = new JsonTextWriter(new StringWriter(stringBuilder)))
            {
                serializer.Serialize(writer, files);
            }
            var moddedjson = stringBuilder.ToString();
            File.WriteAllText(Path.Combine(FAKE_LETHAL, MODDEDLIST), moddedjson);
        }

        public static void MoveTempModsToReal()
        {
            GenerateModFileList();
            string fakeLethalPath = Path.Combine(FileIO.GetExecutableCurrentDir(), FAKE_LETHAL);
            string tempPath = Path.Combine(FileIO.GetExecutableCurrentDir(), TEMP_DIRECTORY);
            Console.WriteLine("About to write to lethal folder, if you want to preview it you can go here: " + Environment.NewLine + fakeLethalPath);
            if (LethalModUtil.WaitForScan)
            {
                Console.WriteLine("The scan detected " + CheckForVirus.TotalVirusHits + " hits out of " + CheckForVirus.TotalChecks + ". If this is below 5 this is ok.");
            } else
            {
                Console.WriteLine("We did not scan throughly for viruses...");
            }
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
            Console.WriteLine("Starting copy into: " + SteamUtil.GetLethalCompanyPath());
            FileIO.CopyFilesRecursively(fakeLethalPath, SteamUtil.GetLethalCompanyPath());
            Console.WriteLine("Done copying mods to lethal company. Attempting to delete the temp folders.");
            try
            {
                Directory.Delete(fakeLethalPath, true);
                Directory.Delete(tempPath, true);
            } catch { }
        }


        public static void InstallModToTemp(ModInstallRequest req, bool blockOnVirus)
        {

            string fakeLethalPath = Path.Combine(FileIO.GetExecutableCurrentDir(), FAKE_LETHAL);
            // CheckForVirus.CheckFileTest();
            var ret = StoreAPI.GetPackages();
            ret.Wait();
            KeyValuePair<string, ModPackage>? yz = ret.Result.FirstOrDefault(x => x.Key.ToLower().Equals(req.FullName.ToLower()));
            if (yz == null)
            {
                throw new Exception("can't install package, can't find it" + req.FullName);
            }
            Console.WriteLine("\t Downloading " + req.FullName);
            var latest = yz.Value.Value.GetLatestVersion();
            File.Delete("temp.zip");
            latest.DownloadPackage("temp.zip");

            Console.WriteLine("\t Scanning " + req.FullName);
            bool pass = true;
            bool waitforever = true;
            while (waitforever)
            {
                waitforever = LethalModUtil.WaitForScan;
                try
                {
                    var xx = CheckForVirus.CheckFile(req.FullName, "temp.zip");
                    xx.Wait();
                    pass = xx.Result;
                    break; //get out :)
                }
                catch (Exception)
                {
                    if (waitforever)
                    {
                        Console.WriteLine("\t Sleeping for a minute because this is a free virus scan and it's throttled.");
                        Thread.Sleep(60000);
                    }
                    else
                    {
                        Console.WriteLine("\t Scan throttled, not waiting");
                    }
                }
            }
            if (!pass)
            {
                Console.WriteLine("--> File might be a virus, not installing");
                File.Delete("temp.zip");
            }
            else
            {
                Console.WriteLine("\t Mod passed virus check");
                //Install
                Console.WriteLine("\t Installing " + req.FullName);
                while (Directory.Exists(TEMP_DIRECTORY))
                {
                    Directory.Delete(TEMP_DIRECTORY, true);
                    Thread.Sleep(500);
                }
                Directory.CreateDirectory(TEMP_DIRECTORY);
                ZipFile.ExtractToDirectory("temp.zip", TEMP_DIRECTORY);

                //Remove garbage files
                FileIO.DeleteFileIfExists(Path.Combine(TEMP_DIRECTORY, "README.md"));
                FileIO.DeleteFileIfExists(Path.Combine(TEMP_DIRECTORY, "manifest.json"));
                FileIO.DeleteFileIfExists(Path.Combine(TEMP_DIRECTORY, "icon.png"));
                FileIO.DeleteFileIfExists(Path.Combine(TEMP_DIRECTORY, "CHANGELOG.md"));


                if (req.FullName.Equals("BepInEx-BepInExPack", StringComparison.OrdinalIgnoreCase))
                {
                    FileIO.CopyFilesRecursively(Path.Combine(TEMP_DIRECTORY, "BepInExPack"), fakeLethalPath);
                }
                else
                {
                    if(Directory.Exists(Path.Combine(TEMP_DIRECTORY, "plugins")))
                    {
                        //This is a mod where it put stuff in plugins, move it to bepin
                        Directory.CreateDirectory(Path.Combine(fakeLethalPath, "BepInEx"));
                        Directory.CreateDirectory(Path.Combine(fakeLethalPath, "BepInEx", "plugins"));
                        FileIO.CopyFilesRecursively(Path.Combine(TEMP_DIRECTORY, "plugins"), Path.Combine(fakeLethalPath, "BepInEx", "plugins"));
                    } else if (Directory.Exists(Path.Combine(TEMP_DIRECTORY, "BepInEx")))
                    {
                        //This one likely did the right thing
                        FileIO.CopyFilesRecursively(TEMP_DIRECTORY, fakeLethalPath);
                    } else {
                        //This is a mod where it did everything wrong
                        Directory.CreateDirectory(Path.Combine(fakeLethalPath, "BepInEx"));
                        Directory.CreateDirectory(Path.Combine(fakeLethalPath, "BepInEx", "plugins"));
                        FileIO.CopyFilesRecursively(TEMP_DIRECTORY, Path.Combine(fakeLethalPath, "BepInEx", "plugins"));
                    }
                }

                while (File.Exists("temp.zip"))
                {

                    File.Delete("temp.zip");
                    Thread.Sleep(500);
                }


            }
        }

    }
}
