using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;
using ConsoleInteractive;
using FreeJira.Jira.Profile;
using FreeJira.Terminal.Profiles.Model;

namespace FreeJira.Terminal.Profiles.Commands
{
    public static class ProfilesCreate
    {
        public static Command GetCreateProfileCommand() {
            var cmd = new Command("create") { Description = "Create a new Jira Profile" };
            cmd.Handler = CommandHandler.Create(CreateNewProfile);
            return cmd;
        }

        private static async Task CreateNewProfile() {
            var count = (await JiraProfileService.GetAvailableProfiles()).Count();
            var profile = await ConsoleI.RenderForm<TerminalJiraProfile>();
            var isDefault = ConsoleI.AskConfirmation("Set as default profile");

            var service = await JiraProfileService
                .Create(profile, profile.ProfilePassword ?? "");

            Console.WriteLine("Created profile " + profile.ProfileName);
            
            if (count == 0 || isDefault) {
                await service.SetAsDefault();
                Console.WriteLine("Profile set as default");
            }
        }
    }
}