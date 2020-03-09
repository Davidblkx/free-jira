using System;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using free_jira.Helpers;
using System.Threading.Tasks;

namespace free_jira.Infra
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
    /// Handles all application settings
    /// </summary>
    public class FreeJiraSettings : IFreeJiraSettings
    {
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

            var settingsFile = InitSettings();
            var fileStream = File.OpenRead(settingsFile);
            _settings = await JsonSerializer.DeserializeAsync<FreeJiraSettings>(fileStream);
            fileStream.Close();
            return _settings;
        }

        public static Task<IFreeJiraSettings> UpdateSettings(FreeJiraSettings settings) {
            SaveSettings(settings);
            _settings = null;
            return GetSettings();
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
        /// Load setting file Path, if no file exists, one is created
        /// </summary>
        /// <returns></returns>
        private static string InitSettings() {
            var baseFolder = PathHelpers.GetSettingsFolderPath();
            var settings = new FreeJiraSettings() { BaseFolder = baseFolder };

            return SaveSettings(settings);
        }

        /// <summary>
        /// Save settings to user folder
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private static string SaveSettings(FreeJiraSettings settings) {
            var baseFolder = PathHelpers.GetSettingsFolderPath();
            var settingsFile = Path.Combine(baseFolder, "settings.json");
            if (!Directory.Exists(baseFolder))
            { Directory.CreateDirectory(baseFolder); }

            if (!File.Exists(settingsFile)) {
                var jsonBody = JsonSerializer.Serialize(
                    settings,
                    new JsonSerializerOptions() { WriteIndented = true }
                );
                File.WriteAllText(settingsFile, jsonBody);
            }

            return settingsFile;
        }
    }
}