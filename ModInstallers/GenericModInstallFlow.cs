using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LethalRed.ModInstallers
{
    public class GenericModInstallFlow
    {

        public static IEnumerable<Type> TypesImplementingInterface(Type desiredType)
        {
            return AppDomain
                   .CurrentDomain
                   .GetAssemblies()
                   .SelectMany(assembly => assembly.GetTypes())
                   .Where(type => desiredType.IsAssignableFrom(type));
        }

        public static bool IsRealClass(Type testType)
        {
            return testType.IsAbstract == false
                 && testType.IsGenericTypeDefinition == false
                 && testType.IsInterface == false;
        }

        public static void AskWhichGame()
        {
            List<IModInstall> gamesMods = new List<IModInstall>();
            var x = TypesImplementingInterface(typeof(IModInstall));
            foreach (var type in x)
            {
                if (IsRealClass(type))
                {
                    IModInstall installer = (IModInstall)Activator.CreateInstance(type);
                    gamesMods.Add(installer);
                }
            }

           
            int index = 0;
            Console.WriteLine("Which game would you like to mod? (If it's not installed, you can't mod it)");
            Console.WriteLine();
            foreach (var game in gamesMods)
            {
                Console.WriteLine(index + ": " + game.GetGameName() + " (" + (game.IsGameInstalled() ? "Installed" : "Not installed") + ")");
                index++;
            }

            int inputnumber = 0;
            while (true)
            {
                string input = Console.ReadLine();
                try
                {
                    inputnumber = Int32.Parse(input);
                    IModInstall yay = gamesMods[inputnumber]; //This will throw exception if it's bad
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("invalid input, type the number only");
                }
            }
            InstallWithPrompts(gamesMods[inputnumber]);

        }

        public static void InstallWithPrompts(IModInstall mod)
        {
            try { 
                if (!mod.IsGameInstalled())
                {
                    Console.WriteLine("Could not find "+mod.GetGameName()+", exiting");
                    return;
                }
                Console.WriteLine("Found " + mod.GetGameName() + " installed");

                if (mod.IsModInstalled())
                {
                    Console.WriteLine("Lethal appears to be modded already.");
                    Console.WriteLine("Press enter to remove existing mods.");
                    Console.Write("> ");
                    Console.ReadLine();
                    LethalModUtil.CleanUpOldMods();
                }
                else
                {
                    Console.WriteLine(mod.GetGameName()+" does not appear modded from a quick look.");
                }

                ModConfig ModConfigInstance = ModConfig.GetInstance();
                mod.CleanTempModFolder();

                int current = 1;
                List<ModInstallRequest> modsToInstall = new List<ModInstallRequest>();
                modsToInstall = ModConfigInstance.GetModsForGame(mod.GetGameName());
                int max = modsToInstall.Count;
                foreach (ModInstallRequest request in modsToInstall)
                {
                    Console.WriteLine("Installing mod " + current + " out of " + max);
                    LethalModUtil.InstallModToTemp(request, true);
                    current++;
                }

                mod.MoveTempModsToReal();

                Console.WriteLine("Done modding "+mod.GetGameName()+"! Thanks for using this modder. Press enter to exit");
                Console.ReadLine();
        } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine();
                Console.WriteLine("Something went wrong, please send the above text to red.");
                Console.ReadLine();
            }
}
    }
}
