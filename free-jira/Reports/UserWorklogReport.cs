using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreeJira.Jira.Model;
using FreeJira.Jira.ReportEngine;

namespace FreeJira.Reports
{
    public class UserWorklogReportFields {
        public const string PARAM_USER = "user";
        public const string PARAM_START = "start";
        public const string PARAM_END = "end";

        public string? Summary { get; set; }
        public long? Timeoriginalestimate { get; set; } = 0;
        public long? TimeSpent { get; set; } = 0;
        public JiraWorklogSearch? Worklog { get; set; }

        public static Dictionary<string, string> GetParamsMap()
            => new Dictionary<string, string> {
                [PARAM_USER] = "User or email to get worklogs",
                [PARAM_START] = "Worklogs start date",
                [PARAM_END] = "Worklogs end date",
            };
    }

    public class UserWorklogReportRow {

        public UserWorklogReportRow(JiraIssue<UserWorklogReportFields> i, JiraWorklog w) {
            IssueKey = i.Key;
            Date = w.Started;
            Worklog = w.TimeSpentSeconds;
            WorklogSummary = w.Comment;
            IssueSummary = i.Fields?.Summary ?? "";
        }

        public string IssueKey { get; set; }
        public DateTime Date { get; set; }
        public long Worklog { get; set; }
        public string WorklogSummary { get; set; }
        public string IssueSummary { get; set; }

        public override string ToString() {
            var sum = WorklogSummary is null ? "" : $"[{WorklogSummary}]";
            var date = Date.ToString("yyyy-MM-dd");
            var total = TimeSpan.FromSeconds(Worklog).TotalMinutes;
            return $"{IssueKey} {total}m [{date}] {sum}";
        }
    }

    public class UserWorklogReport : BaseJiraReport<UserWorklogReportRow, UserWorklogReportFields>
    {
        public UserWorklogReport() : base(
            "UserWorklogs",
            "Calculate worklogs for a user",
            UserWorklogReportFields.GetParamsMap()
        ) {}

        public string GetParamUser() {
            var key = UserWorklogReportFields.PARAM_USER;
            if (Params.ContainsKey(key))
                return Params[key];
            throw new ArgumentException("param user is required");
        }

        public string GetStartDate() {
            var key = UserWorklogReportFields.PARAM_START;
            if (Params.ContainsKey(key))
                return Params[key];
            if (Sprint is null)
                throw new ArgumentException("Start date is required");
            return Sprint.Start.ToString("yyyy-MM-dd");
        }

        public string GetEndDate() {
            var key = UserWorklogReportFields.PARAM_END;
            if (Params.ContainsKey(key))
                return Params[key];
            if (Sprint is null)
                throw new ArgumentException("End date is required");
            return Sprint.End.ToString("yyyy-MM-dd");
        }

        public override Task<string> ToCSV((IEnumerable<UserWorklogReportRow>, Dictionary<string, string>) result)
        {
            var csv = new List<string> {$"IssueKey;Date;Worklog;Comment;Summary"};

            csv.AddRange(result.Item1.Select(e 
                => $"{e.IssueKey};{e.Date};{e.Worklog};{e.WorklogSummary};{e.IssueSummary}"));
            csv.AddRange(result.Item2.Select(e
                => $"{e.Key};${e.Value}"));

            return Task.Run(() => string.Join('\n', csv));
        }

        protected override Task<string> BuildJQL()
        {
            var user = GetParamUser();
            var start = GetStartDate();
            var end = GetEndDate();
            return Task.Run(() 
                => $"worklogAuthor in ({user}) && worklogDate >= \"{start}\" && worklogDate <= \"{end}\"");
        }

        protected override Task<IEnumerable<UserWorklogReportRow>> BuildRow(JiraIssue<UserWorklogReportFields> issue)
            => Task.Run(() => {
                var list = new List<UserWorklogReportRow>();
                
                var worklogs = issue.Fields?.Worklog?.Worklogs;
                if (worklogs is null) return list;

                foreach(var w in worklogs)
                    list.Add(new UserWorklogReportRow(issue, w));

                return list as IEnumerable<UserWorklogReportRow>;
            });

        protected override Task<Dictionary<string, string>> BuildResume(
            IEnumerable<JiraIssue<UserWorklogReportFields>> source, IEnumerable<UserWorklogReportRow> results)
                => Task.Run(() => new Dictionary<string, string>{
                    ["Name"] = source.FirstOrDefault()?
                        .Fields?.Worklog?.Worklogs?.FirstOrDefault()?
                        .Author?.DisplayName ?? "Unkown",
                    ["Total Hours"] = results
                        .Select(e => TimeSpan.FromSeconds(e.Worklog))
                        .Sum(e => e.TotalHours)
                        .ToString() + "h",
                });
    }
}