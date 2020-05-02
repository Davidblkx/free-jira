using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using FreeJira.Infra;
using LiteDB;
using FreeJira.Jira.Profile.Sprint;
using System.Linq;

namespace FreeJira.Jira.Profile
{
    public interface IJiraProfileService
    {
        FileInfo DbPath { get; }

        JiraProfile GetProfile();
        IJiraSprintService GetSprintService();
        Task SetAsDefault();
        JiraClient GetJiraClient();

        void Dispose();
    }

    /// <summary>
    /// Service to load profile and related data
    /// Allows user to protect data using a custom password
    /// </summary>
    public class JiraProfileService : IDisposable, IJiraProfileService
    {
        protected const string COLLECTION_PROFILES = "PROFILES";
        
        private readonly LiteDatabase _db;
        private readonly ILiteCollection<JiraProfile> _collection;

        public LiteDatabase Db { get { return _db; }}

        public FileInfo DbPath { get; }

        public JiraProfileService(LiteDatabase db, FileInfo path) { 
            _db = db;
            DbPath = path;
            _collection = _db.GetCollection<JiraProfile>(COLLECTION_PROFILES);
        }

        /// <summary>
        /// Return Profile of current user
        /// </summary>
        /// <returns></returns>
        public JiraProfile GetProfile() {
            return _collection.FindOne(e => e.User != "");
        }

        /// <summary>
        /// Return Service to handle sprint
        /// </summary>
        /// <returns></returns>
        public IJiraSprintService GetSprintService() {
            return new JiraSprintService(this);
        }

        /// <summary>
        /// Return JiraClient for current profile
        /// </summary>
        /// <returns></returns>
        public JiraClient GetJiraClient() {
            var p = GetProfile();
            return JiraClient.FromBaseAuth(p.User, p.Pass, p.Url);
        }

        /// <summary>
        /// Set current profile as default
        /// </summary>
        /// <returns></returns>
        public async Task SetAsDefault() {
            var settings = FreeJiraSettings.FromInterface(await GetSettings());
            settings.DefaultProfile = GetProfile().ProfileName;
            await FreeJiraSettings.UpdateSettings(settings);
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
        public static async Task<IJiraProfileService> Create(IJiraProfile profile, string password = "") {
            var service = await FromName(profile.ProfileName, password);
            service.SetProfile(JiraProfile.From(profile));
            return service;
        }

        /// <summary>
        /// Return list of available profiles
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<string>> GetAvailableProfiles() {
            var settings = await GetSettings();
            return ProfilePath.GetAvailableProfiles(settings);
        }
        
        /// <summary>
        /// Delete profile by name
        /// </summary>
        /// <param name="profileName"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteProfile(string profileName) {
            var settings = await GetSettings();
            var profilePath = GetAndCreateProfileFolder(settings, profileName);
            if (profilePath?.Exists ?? false){
                profilePath.Delete();
                return true;
            }
            return false;
        }

        public static async Task<bool> SetDefaultProfile(string profileName) {
            var exist = (await GetAvailableProfiles())
                .Any(p => p == profileName);

            if (!exist) return false;

            var settings = FreeJiraSettings.FromInterface(await GetSettings());
            settings.DefaultProfile = profileName;
            await FreeJiraSettings.UpdateSettings(settings);
            return true;
        }

        public static async Task<string> GetDefaultProfileName() {
            return (await GetSettings()).DefaultProfile;
        }

        private static async Task<JiraProfileService> FromName(string profileName, string password) {
            var settings = await GetSettings();
            var profilePath = GetAndCreateProfileFolder(settings, profileName);
            var db = ProfilePath.GetDatabase(settings, profileName, password);
            return new JiraProfileService(db, profilePath);
        }

        private static FileInfo GetAndCreateProfileFolder(IFreeJiraSettings settings, string profileName) {
            var path = ProfilePath.GetProfilePath(settings, profileName);
            if (!path.Directory.Exists) { path.Directory.Create(); }
            return path;
        }

        private static Task<IFreeJiraSettings> GetSettings()
            => FreeJiraSettings.GetSettings();

        protected void SetProfile(JiraProfile profile) {
            _collection.DeleteMany(e => e.User != "");
            _collection.Insert(profile);
            _db.Commit();
        }
    }
}