using System;
using ConsoleInteractive.Components;
using ConsoleInteractive.Form;
using ConsoleInteractive.InputValidation;
using FreeJira.Jira.Profile.Sprint;
using FreeJira.Terminal.Validators;

namespace FreeJira.Terminal.Sprints.Model
{
    public class TerminalJiraSprint : IJiraSprint
    {
        public string IdName { get; } = "";

        [FormEntry(Priority = 0, ProviderKey = nameof(Name))]
        public string Name { get; set; } = "";
        [FormEntry(Priority = 1, ProviderKey = nameof(Start))]
        public DateTime Start { get; set; }
        [FormEntry(Priority = 2, ProviderKey = nameof(End))]
        public DateTime End { get; set; }

        public TerminalJiraSprint() { RegisterComponents(); }

        private void RegisterComponents() {
            var required = ValidatorProvider.Global.GetCollection<string>(StringValidators.REQUIRED);

            var name = InputText.Create("Sprint name:", "", required);
            ComponentsProvider.Global.Register(nameof(Name), name);

            var start = InputText.Create<DateTime>("Start date [YYYY-MM-DD]:");
            ComponentsProvider.Global.Register(nameof(Start), start);

            var end = InputText.Create<DateTime>("End date [YYYY-MM-DD]:");
            ComponentsProvider.Global.Register(nameof(End), end);
        }
    }
}