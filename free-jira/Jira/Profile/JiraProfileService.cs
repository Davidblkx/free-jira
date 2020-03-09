using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using free_jira.Infra;
using free_jira.Helpers;
using LiteDB;
namespace free_jira.Jira.Profile
{
    public interface IJiraProfileService
    {
        FileInfo DbPath { get; }

        JiraProfile GetProfile();
        IJiraSprintService GetSprintService();

        void Dispose();
    }

    /// <summary>
    /// Service to load profile and related data
    /// Allows user to protect data using a custom password
    /// </summary>
    public class JiraProfileService : IDisposable, IJiraProfileService
    {
        protected static readonly string COLLECTION_PROFILES = "PROFILES";
        
        private readonly LiteDatabase _db;

        public FileInfo DbPath { get; }
        public LiteDatabase Db => _db;

        public JiraProfileService(LiteDatabase db, FileInfo path) { 
            _db = db;
            DbPath = path;
        }

        /// <summary>
        /// Return Profile of current user
        /// </summary>
        /// <returns></returns>
        public JiraProfile GetProfile() {
            var col = _db.GetCollection<JiraProfile>(COLLECTION_PROFILES);
            var count = col.Count();
            return col.FindOne(e => e.User != "");
        }

        /// <summary>
        /// Return Service to handle sprint
        /// </summary>
        /// <returns></returns>
        public IJiraSprintService GetSprintService() {
            return new JiraSprintService(this);
        }

        public void Dispose() { _db.Dispose(); }

        /// <summary>
        /// Load a profile database
        /// </summary>
        /// <param name="profileName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task<IJiraProfileService> Load(string profileName, string password = "") {
            var dbPath = await GetProfilePath(profileName);
            
            if (!dbPath.Directory.Exists) { dbPath.Directory.Create(); }

            return FromPath(dbPath.FullName, password);
        }

        /// <summary>
        /// Create and load a new profile database
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task<IJiraProfileService> Create(JiraProfile profile, string password = "") {
            var dbPath = await GetProfilePath(profile.ProfileName);

            if (!dbPath.Directory.Exists) { dbPath.Directory.Create(); }
            
            var service = FromPath(dbPath.FullName, password);

            service.SetProfile(profile);

            return service;
        }

        /// <summary>
        /// Returns path to profile file
        /// </summary>
        /// <param name="profileName"></param>
        /// <returns></returns>
        public static async Task<FileInfo> GetProfilePath(string profileName) {
            var settings = await FreeJiraSettings.GetSettings();
            var path = profileName.LastIndexOf(".db") == (profileName.Length - 3)
                ? profileName : profileName + ".db";
            return settings.Folder.RelativeFile("profiles", path);
        }

        private static JiraProfileService FromPath(string path, string password) {
            var fullPath = new FileInfo(path).Extension
                == ".db" ? path : path + ".db";
            var passwordQuery = string.IsNullOrEmpty(password) ? "" : "Password={password};";
            var connectionString = $"Filename={fullPath};{passwordQuery}Connection=Shared";
            var db = new LiteDatabase(connectionString);
            return new JiraProfileService(db, new FileInfo(fullPath));
        }

        protected void SetProfile(JiraProfile profile) {
            _db.DropCollection(COLLECTION_PROFILES);
            var profiles = _db.GetCollection<JiraProfile>(COLLECTION_PROFILES);
            profiles.Insert(profile);
        }
    }
}