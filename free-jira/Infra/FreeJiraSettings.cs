using System;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using FreeJira.Helpers;
using System.Threading.Tasks;

namespace FreeJira.Infra
{
    public interface IFreeJiraSettings
    {
        /// <summary>
        /// Folder to store settings
        /// </summary>
        DirectoryInfo Folder { get; }

        /// <summary>
        /// Port to use when starting server
        /// </summary>
        int ServerPort { get; }

        /// <summary>
        /// Define app mode to start by default
        /// </summary>
        StartupMode StartupMode { get; }

        /// <summary>
        /// Profile to load by default
        /// </summary>
        /// <value></value>
        string DefaultProfile { get; }
    }

    /// <summary>
    /// Handles base application settings
    /// </summary>
    public class FreeJiraSettings : IFreeJiraSettings
    {
        public const string SETTINGS_FILE_NAME = "settings.json";
        private static IFreeJiraSettings? _settings;

        public string BaseFolder { get; set; } = "~";

        /// <summary>
        /// Folder to store settings
        /// </summary>
        /// <value></value>
        [JsonIgnore]
        public DirectoryInfo Folder { get { return new DirectoryInfo(BaseFolder); } }

        /// <summary>
        /// Port to use when starting server
        /// </summary>
        /// <value></value>
        public int ServerPort { get; set; } = 8080;

        /// <summary>
        /// Define app mode to start by default
        /// </summary>
        /// <value></value>
        public StartupMode StartupMode { get; set; } = StartupMode.Terminal;

        /// <summary>
        /// Profile to load by default
        /// </summary>
        /// <value></value>
        public string DefaultProfile { get; set; } = "default";

        /// <summary>
        /// Load settings file. If a files doesn't exist, one is created
        /// </summary>
        /// <returns></returns>
        public async static Task<IFreeJiraSettings> GetSettings() {
            if (!(_settings is null)) { return _settings; }

            var settingsFile = GetSettingsFile();
            await InitSettings(settingsFile);

            var fileStream = File.OpenRead(settingsFile.FullName);
            _settings = await JsonSerializer
                .DeserializeAsync<FreeJiraSettings>(fileStream);
            fileStream.Close();

            return _settings;
        }

        public static async Task<IFreeJiraSettings> UpdateSettings(FreeJiraSettings settings) {
            await SaveSettings(settings);
            _settings = null;
            return await GetSettings();
        }

        public static FreeJiraSettings FromInterface(IFreeJiraSettings settings) {
            return new FreeJiraSettings {
                BaseFolder = settings.Folder.FullName,
                DefaultProfile = settings.DefaultProfile,
                ServerPort = settings.ServerPort,
                StartupMode = settings.StartupMode,
            };
        }

        /// <summary>
        /// set settings file with default values
        /// </summary>
        /// <returns></returns>
        private static async Task InitSettings(FileInfo file) {
            if (!file.Directory.Exists)
                file.Directory.Create();

            if (!file.Exists) {
                var settings = new FreeJiraSettings() {
                    BaseFolder = file.Directory.FullName };
                await SaveSettings(settings);
            }
        }

        /// <summary>
        /// Save settings to user folder
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private static async Task<bool> SaveSettings(FreeJiraSettings settings) {
            var file = GetSettingsFile();
            var json = SerializeSettings(settings);
            
            try {
                await File.WriteAllTextAsync(file.FullName, json);
                return true;
            } catch { }
            
            return false;
        }

        /// <summary>
        /// Return the settings file info
        /// </summary>
        /// <returns></returns>
        private static FileInfo GetSettingsFile() {
            var baseFolder = PathHelpers.GetSettingsFolderPath();
            var settingsFilePath = Path.Combine(baseFolder, SETTINGS_FILE_NAME);
            var file = new FileInfo(settingsFilePath);
            return file;
        }

        private static string SerializeSettings(FreeJiraSettings settings) {
            return JsonSerializer.Serialize(
                settings,
                new JsonSerializerOptions() { WriteIndented = true }
            );
        }
    }
}