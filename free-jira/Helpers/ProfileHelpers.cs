using System.Threading.Tasks;
using FreeJira.Jira.Profile;
using FreeJira.Jira.Profile.Sprint;

namespace FreeJira.Helpers
{
    internal static class ProfileHelpers
    {
        public static async Task<IJiraProfileService?> GetJiraProfileService(string? profile) {
            var name = await GetProfileNameOrDefault(profile);
            if (string.IsNullOrEmpty(name)) { return null; }
            
            return await JiraProfileService.Load(name);
        }

        public static async Task<string?> GetProfileNameOrDefault(string? profileName) {
            return string.IsNullOrEmpty(profileName) ? 
                await JiraProfileService.GetDefaultProfileName() :
                profileName;
        }

        public static async Task<IJiraSprintService?> GetJiraSprintService(string? profile)
            => (await GetJiraProfileService(profile))?.GetSprintService();
    }
}