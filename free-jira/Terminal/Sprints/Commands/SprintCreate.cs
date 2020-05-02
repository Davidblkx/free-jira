using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using ConsoleInteractive;
using FreeJira.Helpers;
using FreeJira.Terminal.Sprints.Model;

namespace FreeJira.Terminal.Sprints.Commands
{
    internal static class SprintCreateCommand
    {
        public static Command Build() {
            return new Command("create", "Create a new sprint") { 
                Handler = CommandHandler.Create<string>(CreateSprint)
            };
        }

        public static async Task CreateSprint(string? profile) {
            var service = await ProfileHelpers.GetJiraProfileService(profile);

            if (service is null) { 
                Console.WriteLine("Can't find default profile"); return; }
                
            var sprint = await ConsoleI.RenderForm<TerminalJiraSprint>();
            if (sprint is null)  {
                Console.WriteLine("Sprint is invalid"); return; }

            var success = service
                .GetSprintService()
                .CreateSprint(sprint);

            if (success) {
                Console.WriteLine($"Created sprint {sprint.Name}");
            } else {
                Console.WriteLine($"Sprint with name [{sprint.Name}] already exists");
            }
        }
    }
}