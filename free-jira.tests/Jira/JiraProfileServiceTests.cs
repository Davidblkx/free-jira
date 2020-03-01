using System;
using System.Threading.Tasks;
using free_jira.Jira;
using Xunit;

namespace free_jira.tests.Jira
{
    public class JiraProfileServiceTests
    {
        [Fact]
        public void ValidateTestEnvVariables() {
            var profile = GetProfile();

            Assert.NotNull(profile);
            Assert.NotEqual("", profile.Url);
            Assert.NotEqual("", profile.User);
            Assert.NotEqual("", profile.Pass);
        }

        [Fact]
        public async Task JiraProfileCreateUpdate() {
            var profile = GetProfile();
            var service = await JiraProfileService.Create(profile);

            Assert.NotNull(service);
            Assert.True(service.DbPath.Exists);
            
            var p = service.GetProfile();

            Assert.NotNull(p);
            Assert.Equal(profile.Pass, p.Pass);
            Assert.Equal(profile.User, p.User);
            Assert.Equal(profile.Url, p.Url);
        }

        private JiraProfile GetProfile() {
            return new JiraProfile {
                Pass = Environment.GetEnvironmentVariable("JIRA_PASS") ?? "",
                User = Environment.GetEnvironmentVariable("JIRA_USER") ?? "",
                Url = Environment.GetEnvironmentVariable("JIRA_URL") ?? ""
            };
        }
    }
}