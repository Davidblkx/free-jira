using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using FreeJira.Jira.Profile;

namespace FreeJira.Terminal.Profiles.Commands
{
    public static class ProfileList
    {
        public static Command GetListCommand() {
            var list = new Command("list") { Description = "List all available profiles" };
            list.Handler = CommandHandler.Create(ListProfiles);
            list.AddAlias("ls");
            return list;
        }

        /// <summary>
        /// Print all available profiles
        /// </summary>
        /// <returns></returns>
        private static async Task ListProfiles() {
            var defaultProfile = await JiraProfileService.GetDefaultProfileName();
            foreach (var p in await JiraProfileService.GetAvailableProfiles()) {
                string isDefault() => defaultProfile == p ? " * " : "";
                Console.WriteLine(" -> "  + p + isDefault());
            }
        }
    }
}