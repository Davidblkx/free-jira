using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;
using ConsoleInteractive;
using FreeJira.Helpers;
using FreeJira.Jira.Profile.Sprint;

namespace FreeJira.Terminal.Sprints.Commands
{
    internal static class SprintDeleteCommand
    {
        public static Command Build() {
            return new Command("delete", "select and delete profiles") {
                Handler = CommandHandler.Create<string>(DeleteSprint)
            };
        }

        private static async Task DeleteSprint(string profile) {
            var service = await ProfileHelpers.GetJiraSprintService(profile);
            IEnumerable<IJiraSprint> sprintList = service?.GetSprints() ??
                new List<JiraSprint>();

            if (sprintList.Count() == 0) {
                Console.WriteLine("Can't find any sprint to delete");
                return;
            }

            var toDelete = await ConsoleI.Select(sprintList, sprintList.Count());
            var deleted = toDelete.Select(
                s => service?.DeleteSprint(s.Name) ?? false)
                .Select( t => t ? 1 : 0)
                .Sum();

            Console.WriteLine($"Deleted {deleted} entries");
        }
    }
}