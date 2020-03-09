using System;
using System.Threading.Tasks;
using free_jira.Jira.Profile;

namespace free_jira.tests.Jira
{
    public static class JiraProfileLoader
    {
        public static JiraProfile GetProfile() {
            return new JiraProfile {
                Pass = Environment.GetEnvironmentVariable("JIRA_PASS") ?? "",
                User = Environment.GetEnvironmentVariable("JIRA_USER") ?? "",
                Url = Environment.GetEnvironmentVariable("JIRA_URL") ?? ""
            };
        }

        public static async Task<IJiraSprintService> GetSprintService(string profileName) {
            var profile = GetProfile();
            profile.ProfileName = profileName;
            var service = await JiraProfileService.Create(profile);
            return service.GetSprintService();
        }

        public static async Task<bool> CleanProfile(string name) {
            var file = await JiraProfileService.GetProfilePath(name);
            if (file.Exists) {
                file.Delete();
                return true;
            }

            return false;
        }
    }
}