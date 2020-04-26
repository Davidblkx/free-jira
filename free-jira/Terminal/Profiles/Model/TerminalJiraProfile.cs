using System;
using FreeJira.Jira.Profile;
using ConsoleInteractive.Form;
using FreeJira.Terminal.Validators;
using ConsoleInteractive.Components;
using ConsoleInteractive.InputValidation;

namespace FreeJira.Terminal.Profiles.Model
{
    public class TerminalJiraProfile : IJiraProfile
    {
        [FormEntry(Priority = 0, ProviderKey = nameof(ProfileName))]
        public string ProfileName { get; set; } = "";
        [FormEntry(Priority = 1, ProviderKey = nameof(ProfilePassword))]
        public string? ProfilePassword { get; set; }
        [FormEntry(Priority = 2, ProviderKey = nameof(User))]
        public string User { get; set; } = "";
        [FormEntry(Priority = 3, ProviderKey = nameof(Pass))]
        public string Pass { get; set; } = "";
        [FormEntry(Priority = 4, ProviderKey = nameof(ServerUrl))]
        public Uri?  ServerUrl { get; set; }

        public string Url {
            get {
                if (ServerUrl is null)
                    throw new NullReferenceException("Server url is invalid");
                return ServerUrl.ToString();
            }
        }

        public TerminalJiraProfile() { RegisterComponents(); }

        private void RegisterComponents() {
            var required = ValidatorProvider.Global.GetCollection<string>(StringValidators.REQUIRED);

            var name = InputText.Create("Profile name:", "", required);
            ComponentsProvider.Global.Register(nameof(ProfileName), name);

            var pass = InputText.Create("Profile password [Empty to ignore]:", "");
            ComponentsProvider.Global.Register(nameof(ProfilePassword), pass);

            var user = InputText.Create("Jira username/email:", "", required);
            ComponentsProvider.Global.Register(nameof(User), user);

            var token = InputText.Create("Jira token:", "", required);
            ComponentsProvider.Global.Register(nameof(Pass), token);

            var url = InputText.Create<Uri?>("Jira server url", null);
            ComponentsProvider.Global.Register(nameof(ServerUrl), url);
        }
    }
}