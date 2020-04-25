using System;
using System.Threading.Tasks;
using FreeJira.Jira.Profile;
using FreeJira.Jira.Profile.Sprint;
using FreeJira.Jira.Profile.Unit;

namespace FreeJira.tests.Jira
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
            var sett = await FreeJira.Infra.FreeJiraSettings.GetSettings();
            var file = ProfilePath.GetProfilePath(sett, name);
            if (file.Exists) {
                file.Delete();
                return true;
            }

            return false;
        }
    }
}