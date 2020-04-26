using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;
using ConsoleInteractive.Components;
using FreeJira.Jira.Profile;

namespace FreeJira.Terminal.Profiles.Commands
{
    public static class ProfileDefault
    {
        public static Command GetGetDefaultCommand() {
            var cmd = new Command("get-default") { Description = "Get default profile" };
            cmd.Handler = CommandHandler.Create(PrintDefaultProfile);
            return cmd;
        }

        public static Command GetSetDefaultCommand() {
            var cmd = new Command("set-default") { Description = "Set default profile" };
            cmd.Handler = CommandHandler.Create<string>(SetDefaultProfile);
            var profileNameOptions = new string[] { "--name", "-n" };
            cmd.AddOption(new Option<string>(
                profileNameOptions, "name of profile to set as default"));
            return cmd;
        }

        private static async Task PrintDefaultProfile() {
            var profileName = await JiraProfileService.GetDefaultProfileName();
            Console.WriteLine(profileName);
        }

        private static async Task SetDefaultProfile(string? name) {
            string profileName = name ?? "";
            if (string.IsNullOrEmpty(profileName)) {
                profileName = await GetProfileName();
            }
            
            try {
                var res = await JiraProfileService.SetDefaultProfile(profileName);
                if (res) Console.WriteLine($"Profile {profileName} set as default");
                else Console.WriteLine("Profile not found");
            } catch {
                Console.WriteLine($"Can't find profile: {profileName}");
            }
        }

        private static async Task<string> GetProfileName() {
            var profiles = await JiraProfileService.GetAvailableProfiles();
            var inputSelect = InputSelection
                .From<string>("Select default profile")
                .AddOption(profiles);
            return (await inputSelect.RequestInput()).FirstOrDefault();
        }
    }
}