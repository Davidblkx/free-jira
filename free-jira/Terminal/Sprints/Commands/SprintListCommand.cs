using System.Linq;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using FreeJira.Helpers;
using FreeJira.Jira.Profile.Sprint;

namespace FreeJira.Terminal.Sprints.Commands
{
    internal class SprintListCommand
    {
        public static Command Build() {
            var cmd =  new Command("list", "List all available sprint");
            cmd.AddAlias("ls");
            cmd.Handler = CommandHandler.Create<string>(ListSprints);
            return cmd;
        }

        private static async Task ListSprints(string? profile) {
            var service = await ProfileHelpers.GetJiraSprintService(profile);

            if (service is null) { 
                Console.WriteLine("Can't find default profile"); return; }
                
            var activeSprint = service.GetActiveSprint();
            service.GetSprints()
                .OrderByDescending(e => e.Start).ToList()
                .ForEach(s => PrintSprint(s, activeSprint));
        }

        private static void PrintSprint(IJiraSprint s, IJiraSprint? activeSprint) {
            var isActive = s.IdName == activeSprint?.IdName ? "*" : "-";
            Console.WriteLine($"{isActive} {s}");
        }
    }
}