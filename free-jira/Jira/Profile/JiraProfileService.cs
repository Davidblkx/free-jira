using System.IO;
using System.Threading.Tasks;
using System;
using FreeJira.Infra;
using LiteDB;
using FreeJira.Jira.Profile.Sprint;
using FreeJira.Jira.Profile.Unit;

namespace FreeJira.Jira.Profile
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
            return await FromName(profileName, password);
        }

        /// <summary>
        /// Create and load a new profile database
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task<IJiraProfileService> Create(JiraProfile profile, string password = "") {
            var service = await FromName(profile.ProfileName, password);
            service.SetProfile(profile);
            return service;
        }

        private static async Task<JiraProfileService> FromName(string profileName, string password) {
            var settings = await FreeJiraSettings.GetSettings();
            var profilePath = GetAndCreateProfileFolder(settings, profileName);
            var db = ProfilePath.GetDatabase(settings, profileName, password);
            return new JiraProfileService(db, profilePath);
        }

        private static FileInfo GetAndCreateProfileFolder(IFreeJiraSettings settings, string profileName) {
            var path = ProfilePath.GetProfilePath(settings, profileName);
            if (!path.Directory.Exists) { path.Directory.Create(); }
            return path;
        }

        protected void SetProfile(JiraProfile profile) {
            _db.DropCollection(COLLECTION_PROFILES);
            var profiles = _db.GetCollection<JiraProfile>(COLLECTION_PROFILES);
            profiles.Insert(profile);
        }
    }
}