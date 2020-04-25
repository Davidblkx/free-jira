using System;
using System.Threading.Tasks;
using FreeJira.Jira.Profile;
using Xunit;

namespace FreeJira.tests.Jira
{
    public class JiraProfileServiceTests
    {
        [Fact]
        public void ValidateTestEnvVariables() {
            var profile = JiraProfileLoader.GetProfile();

            Assert.NotNull(profile);
            Assert.NotEqual("", profile.Url);
            Assert.NotEqual("", profile.User);
            Assert.NotEqual("", profile.Pass);
        }

        [Fact]
        public async Task JiraProfileCreateUpdate() {
            var profile = JiraProfileLoader.GetProfile();
            var service = await JiraProfileService.Create(profile);

            Assert.NotNull(service);
            Assert.True(service.DbPath.Exists);
            
            var p = service.GetProfile();

            Assert.NotNull(p);
            Assert.Equal(profile.Pass, p.Pass);
            Assert.Equal(profile.User, p.User);
            Assert.Equal(profile.Url, p.Url);

            service.Dispose();

            var isClean = await JiraProfileLoader.CleanProfile(profile.ProfileName);
            Assert.True(isClean);
        }

        [Fact]
        public async Task JiraProfileGetSprintService() {
            var name = "TestSprintService";
            var service = await JiraProfileLoader.GetSprintService(name);

            Assert.NotNull(service);

            service.GetParent().Dispose();
            await JiraProfileLoader.CleanProfile(name);
        }

        [Fact]
        public async Task JiraProfileSprint() {
            var name = "TestSprintService_all";
            var service = await JiraProfileLoader.GetSprintService(name);
            var sprintName = "Test1";
            var fuzzyName = " TEST1  ";
            var start = DateTime.Now.Subtract(TimeSpan.FromDays(2));
            var end = DateTime.Now.AddDays(2);

            Assert.NotNull(service);

            var testCreated = service.CreateSprint(sprintName, start, end);
            var testCreateDuplicate = service.CreateSprint(sprintName, start, end);
            var testGetActive = service.GetActiveSprint();
            var testGetById = service.GetSprintByName(fuzzyName);
            var testDelete = service.DeleteSprint(sprintName);
            var testDeleteDuplicate = service.DeleteSprint(sprintName);

            Assert.True(testCreated);
            Assert.False(testCreateDuplicate);
            Assert.NotNull(testGetActive);
            Assert.Equal(sprintName, testGetActive?.Name);
            Assert.NotNull(testGetById);
            Assert.Equal(sprintName, testGetById?.Name);
            Assert.True(testDelete);
            Assert.False(testDeleteDuplicate);

            service.GetParent().Dispose();
            await JiraProfileLoader.CleanProfile(name);
        }
    }
}