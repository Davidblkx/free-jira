using System.Threading.Tasks;
using FreeJira.Jira.Profile;

namespace FreeJira.Helpers
{
    internal static class ProfileHelpers
    {
        public static async Task<IJiraProfileService?> GetJiraProfileService(string? profileName) {
            var name = await GetProfileNameOrDefault(profileName);
            if (string.IsNullOrEmpty(name)) { return null; }
            
            return await JiraProfileService.Load(name);
        }

        public static async Task<string?> GetProfileNameOrDefault(string? profileName) {
            return string.IsNullOrEmpty(profileName) ? 
                await JiraProfileService.GetDefaultProfileName() :
                profileName;
        }
    }
}