using Xunit;
using FreeJira.Infra;
using System.Threading.Tasks;
using System.IO;

namespace FreeJira.tests.Infra
{
    public class FreeJiraSettingsTests
    {
        [Fact]
        public async Task TestName()
        {
            var settings = await FreeJiraSettings.GetSettings();

            Assert.NotNull(settings);
            Assert.NotNull(settings.Folder);
            Assert.True(settings.Folder.Exists);
        }
    }
}