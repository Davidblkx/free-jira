using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using free_jira.Infra;
using free_jira.Helpers;

namespace free_jira.Terminal.Profiles
{
    public class ProfilesCommand
    {
        public static Command BuildProfileCommand() {
            var cmd = new Command("profile") { Description = "Profile related actions" };

            var list = new Command("list") { Description = "List all avaible profiles" };
            list.Handler = CommandHandler.Create(ListProfiles);
            cmd.AddCommand(list);

            return cmd;
        }

        /// <summary>
        /// Print all avaible profiles
        /// </summary>
        /// <returns></returns>
        private static async Task ListProfiles() {
            var settings = await FreeJiraSettings.GetSettings();
            var folder = settings.Folder.RelativeDirectory("profiles");

            if (!folder.Exists) {
                Console.WriteLine("Can't find profiles fodler!");
                return;
            }

            Console.WriteLine("");
            foreach (var d in folder.GetFiles("*.db")) {
                var name = d.Name.Substring(0, d.Name.Length - d.Extension.Length);
                string isDefault() => settings.DefaultProfile == name ? " * " : "";
                Console.WriteLine(" -> "  + name + isDefault());
            }
            Console.WriteLine("");
        }
    }
}