using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using FreeJira.Helpers;
using FreeJira.Infra;
using LiteDB;

[assembly:InternalsVisibleTo("free-jira.tests")]
namespace FreeJira.Jira.Profile
{
    /// <summary>
    /// Pure function to handle path related logic for JiraProfile
    /// </summary>
    internal static class ProfilePath
    {
        public const string PROFILE_DEFAULT_FOLDER_NAME = "profiles";
        public const string PROFILE_FILE_EXTENSION = ".db";

        /// <summary>
        /// Add extension name to string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetNameWithExtension(string name) {
            var extLength = PROFILE_FILE_EXTENSION.Length;
            var lengthWithout = name.Length - extLength;
            return name.LastIndexOf(PROFILE_FILE_EXTENSION) == lengthWithout ?
                name : name + PROFILE_FILE_EXTENSION;
        }

        /// <summary>
        /// Get profile file
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="profileName"></param>
        /// <returns></returns>
        public static FileInfo GetProfilePath(IFreeJiraSettings settings, string profileName) {
            var name = GetNameWithExtension(profileName);
            return settings.Folder.RelativeFile(PROFILE_DEFAULT_FOLDER_NAME, name);
        }

        /// <summary>
        /// Create LiteDB connection string
        /// </summary>
        /// <param name="profileName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string GetProfileDBConnectionString(IFreeJiraSettings settings, string profileName, string? password = null) {
            var profilePath = GetProfilePath(settings, profileName).FullName;
            var passwordQuery = string.IsNullOrEmpty(password) ? "" : $"Password={password};";
            return $"Filename={profilePath};{passwordQuery}Connection=Shared";
        }

        /// <summary>
        /// Load database
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="profileName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static LiteDatabase GetDatabase(IFreeJiraSettings settings, string profileName, string? password = null) {
            var connectionString = GetProfileDBConnectionString(settings, profileName, password);
            return new LiteDatabase(connectionString);
        }

        /// <summary>
        /// Load list of profile names
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetAvailableProfiles(IFreeJiraSettings settings) {
            var folder = settings.Folder.RelativeDirectory(PROFILE_DEFAULT_FOLDER_NAME);
            if (!folder.Exists) { yield break; }
            var searchGlob = "*" + PROFILE_FILE_EXTENSION;
            foreach (var d in folder.GetFiles(searchGlob)) {
                yield return d.GetNameWithoutExtension();
            }
        }
    }
}