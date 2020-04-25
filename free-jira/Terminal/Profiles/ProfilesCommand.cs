using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using FreeJira.Infra;
using FreeJira.Helpers;
using FreeJira.Jira.Profile.Unit;

namespace FreeJira.Terminal.Profiles
{
    public class ProfilesCommand
    {
        public static Command BuildProfileCommand() {
            var cmd = new Command("profile") { Description = "Profile related actions" };

            var list = new Command("list") { Description = "List all available profiles" };
            list.Handler = CommandHandler.Create(ListProfiles);
            list.AddAlias("ls");
            cmd.AddCommand(list);

            return cmd;
        }

        /// <summary>
        /// Print all available profiles
        /// </summary>
        /// <returns></returns>
        private static async Task ListProfiles() {
            var settings = await FreeJiraSettings.GetSettings();

            foreach (var p in ProfilePath.GetAvailableProfiles(settings)) {
                string isDefault() => settings.DefaultProfile == p ? " * " : "";
                Console.WriteLine(" -> "  + p + isDefault());
            }
        }
    }
}