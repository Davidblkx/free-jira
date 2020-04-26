using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using FreeJira.Jira.Profile;
using ConsoleInteractive.Components;
using System.Linq;

namespace FreeJira.Terminal.Profiles
{
    /// <summary>
    /// Delete profile
    /// </summary>
    public static class ProfilesDelete
    {
        public static Command GetDeleteProfileCommand() {
            var cmd = new Command("delete") { 
                Description = "Delete a Profile" };
            cmd.Handler = CommandHandler.Create<string>(DeleteProfile);
            
            var profileNameOptions = new string[] { "--name", "-n" };
            cmd.AddOption(new Option<string>(
                profileNameOptions, "name of profile to delete"));

            return cmd;
        }

        private static async Task DeleteProfile(string? name) {
            string profileName = name ?? "";
            if (string.IsNullOrEmpty(profileName)) {
                profileName = await GetProfileName();
            }
            
            try {
                var res = await JiraProfileService.DeleteProfile(profileName);
                if (res) Console.WriteLine("Profile deleted");
                else Console.WriteLine("Profile not found");
            } catch {
                Console.WriteLine($"Can't delete profile: {profileName}");
            }
        }

        private static async Task<string> GetProfileName() {
            var profiles = await JiraProfileService.GetAvailableProfiles();
            var inputSelect = InputSelection
                .From<string>("Select a profile to delete")
                .AddOption(profiles);
            return (await inputSelect.RequestInput()).FirstOrDefault();
        }
    }
}