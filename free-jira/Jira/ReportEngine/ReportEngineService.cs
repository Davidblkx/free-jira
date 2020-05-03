using System;
using System.Collections.Generic;
using System.Linq;

namespace FreeJira.Jira.ReportEngine
{
    public static class ReportEngineService
    {
        public static IEnumerable<IJiraReport?> GetReports() {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IJiraReport).IsAssignableFrom(p) && !p.IsAbstract)
                .Select(p => Activator.CreateInstance(p) as IJiraReport);
        }

        public static IJiraReport? GetReportByName(string name) {
            return GetReports()
                .FirstOrDefault(
                    e => e?.Name == name);
        }
    }
}