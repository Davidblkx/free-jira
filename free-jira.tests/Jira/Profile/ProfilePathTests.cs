using System.Text.RegularExpressions;
using FreeJira.Jira.Profile;
using Xunit;
using Moq;
using FreeJira.Infra;
using System.IO;

namespace FreeJira.tests.Jira.Profile
{
    public class ProfilePathTests
    {
        [Theory]
        [InlineData("batatas")]
        [InlineData("batatas.db")]
        public void GetNameWithExtensionTest(string name) {
            const string ext = ProfilePath.PROFILE_FILE_EXTENSION;
            var target = ProfilePath.GetNameWithExtension(name);

            Assert.EndsWith(ext, target);
            
            // checks if don't duplicate extension
            var occurrences = Regex.Matches(target, ext).Count;
            Assert.Equal(1, occurrences);
        }

        [Theory]
        [InlineData("batatas")]
        [InlineData("batatas.db")]
        public void GetProfilePathTest(string name) {
            const string folderName = "data";
            var settingsMock = new Mock<IFreeJiraSettings>();
            settingsMock.Setup(e => e.Folder)
                .Returns(new DirectoryInfo(folderName));
            
            var target = ProfilePath.GetProfilePath(settingsMock.Object, name);
            var targetName = ProfilePath.GetNameWithExtension(name);

            Assert.NotNull(target);
            Assert.IsType<FileInfo>(target);
            Assert.Equal(ProfilePath.PROFILE_DEFAULT_FOLDER_NAME, target.Directory.Name);
            Assert.Equal(targetName, target.Name);

            var targetFolder = target.Directory.Parent.Name;
            Assert.Equal(folderName, targetFolder);
        }
    }
}