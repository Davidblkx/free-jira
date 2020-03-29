using System.IO;
using System.Runtime.InteropServices;
using System;
using static System.Environment;

namespace free_jira.Helpers
{
    public static class PathHelpers
    {
        public static string GetWindowsAppData() {
            return Environment.GetFolderPath(SpecialFolder.ApplicationData);
        }

        public static string GetSettingsFolderPath() {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                return Path.Combine(GetWindowsAppData(), "FreeJira");
            }

            return "~/free-jira";
        }

        public static FileInfo RelativeFile(this DirectoryInfo directory, params string[] paths) {
            return new FileInfo(Path.Combine(directory.FullName, paths.JoinString('/')));
        }

        public static DirectoryInfo RelativeDirectory(this DirectoryInfo directory, params string[] paths) {
            return new DirectoryInfo(Path.Combine(directory.FullName, paths.JoinString('/')));
        }
    }
}