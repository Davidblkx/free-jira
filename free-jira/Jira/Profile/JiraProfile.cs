using System;
namespace FreeJira.Jira.Profile
{
    public interface IJiraProfile
    {
        string ProfileName { get; }
        string User { get; }
        string Pass { get; }
        string Url { get; }
    }

    public class JiraProfile : IJiraProfile
    {
        public string ProfileName { get; set; }
        public string User { get; set; } = "";
        public string Pass { get; set; } = "";
        public string Url { get; set; } = "";

        public JiraProfile()
        {
            ProfileName = Guid.NewGuid().ToString();
        }

        public static JiraProfile Create() => new JiraProfile();

        /// <summary>
        /// Creates from a base profile
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static JiraProfile From(IJiraProfile p) {
            return new JiraProfile() {
                ProfileName = p.ProfileName,
                Pass = p.Pass,
                Url = p.Url,
                User = p.User
            };
        }
    }
}