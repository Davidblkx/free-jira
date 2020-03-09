using System;
namespace free_jira.Jira.Profile
{
    public class JiraProfile
    {
        public string ProfileName { get; set; }
        public string User { get; set; } = "";
        public string Pass { get; set; } = "";
        public string Url { get; set; } = "";

        public JiraProfile() {
            ProfileName = Guid.NewGuid().ToString();
        }
    }
}