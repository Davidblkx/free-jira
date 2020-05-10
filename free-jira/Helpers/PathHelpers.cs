using System.IO;
using System.Runtime.InteropServices;
using System;
using static System.Environment;

namespace FreeJira.Helpers
{
    public static class PathHelpers
    {
        /// <summary>
        /// return Windows %APPDATA%
        /// </summary>
        /// <returns></returns>
        public static string GetWindowsAppData() {
            return Environment.GetFolderPath(SpecialFolder.ApplicationData);
        }

        public static string? GetUnixHomeDir() {
            return Environment.GetFolderPath(SpecialFolder.UserProfile);
        }

        /// <summary>
        /// Folder to save settings
        /// </summary>
        /// <returns></returns>
        public static string GetSettingsFolderPath() {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                return Path.Combine(GetWindowsAppData(), "FreeJira");
            }

            var unixUserHome = GetUnixHomeDir();
            if (!string.IsNullOrEmpty(unixUserHome)) {
                return Path.Combine(unixUserHome, ".free-jira");
            }

            return ".free-jira";
        }

        /// <summary>
        /// Return <FileInfo> relative to current <DirectoryInfo>
        /// </summary>
        public static FileInfo RelativeFile(this DirectoryInfo directory, params string[] paths) {
            return new FileInfo(Path.Combine(directory.FullName, paths.JoinString('/')));
        }

        /// <summary>
        /// Return <DirectoryInfo> relative to current <DirectoryInfo>
        /// </summary>
        public static DirectoryInfo RelativeDirectory(this DirectoryInfo directory, params string[] paths) {
            return new DirectoryInfo(Path.Combine(directory.FullName, paths.JoinString('/')));
        }

        public static string GetNameWithoutExtension(this FileInfo file) {
            var extLength = file.Extension?.Length ?? 0;
            var nameLength = file.Name.Length - extLength;
            return file.Name.Substring(0, nameLength);
        }
    }
}