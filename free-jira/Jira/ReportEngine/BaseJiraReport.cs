using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreeJira.Jira.Model;
using FreeJira.Jira.Profile.Sprint;

namespace FreeJira.Jira.ReportEngine
{
    public abstract class BaseJiraReport<TRow, TFields> : IJiraReport<TRow>
        where TRow : class
        where TFields : class
    {
        public string Name { get; }
        public string Description { get; }
        public Dictionary<string, string> Params { get; protected set; }
        public Dictionary<string, string> ParamsMap { get; }
        public JiraClient? Client { get; protected set; }
        public IJiraSprint? Sprint { get; protected set; }

        /// <summary>
        /// Init report setrings
        /// </summary>
        /// <param name="name">Name of report</param>
        /// <param name="desc">Description of resport</param>
        /// <param name="paramsMap">param map</param>
        public BaseJiraReport(
            string name,
            string desc, 
            Dictionary<string, string>? paramsMap = null
        ) {
            Name = name;
            Description = desc;
            Params = new Dictionary<string, string>();
            ParamsMap = paramsMap is null ?
                new Dictionary<string, string>()
                : paramsMap;
        }

        public async Task PrintReport(JiraClient client, IJiraSprint? sprint, Dictionary<string, string> reportParams)
        {
            var res = await BuildReport(client, sprint, reportParams);
            foreach(var r in res.Item1)
                Console.WriteLine(r);
            foreach(var k in res.Item2)
                Console.WriteLine($"{k.Key} = {k.Value}");
        }

        public async Task<string> ToCSV(JiraClient client, IJiraSprint? sprint, Dictionary<string, string> reportParams)
        {
            var res = await BuildReport(client, sprint, reportParams);
            return await ToCSV(res);
        }

        public async Task<(IEnumerable<TRow>, Dictionary<string, string>)> BuildReport(
            JiraClient client, IJiraSprint? sprint, Dictionary<string, string> reportParams)
        {
            Client = client;
            Sprint = sprint;
            Params = reportParams;

            var jql = await BuildJQL();
            var res = await client.IssueClient.Jql<TFields>(jql);
            var jqlResults = res.ValueOr(new JiraIssueJqlResponse<TFields>());

            var rows = new List<TRow>();
            foreach (var issue in jqlResults.Issues)
                rows.AddRange(await BuildRow(issue));

            var resume = await BuildResume(jqlResults.Issues, rows);
            return (rows, resume);
        }

        public abstract Task<string> ToCSV((IEnumerable<TRow>, Dictionary<string, string>) result);

        public override string ToString() {
            return $"{Name}: {Description}";
        }

        /// <summary>
        /// Build jql to query JIRA
        /// </summary>
        /// <returns></returns>
        protected abstract Task<string> BuildJQL();

        /// <summary>
        /// Build one or multiple results from an issue
        /// </summary>
        /// <param name="issue"></param>
        /// <returns></returns>
        protected abstract Task<IEnumerable<TRow>> BuildRow(JiraIssue<TFields> issue);

        /// <summary>
        /// Build resume of report
        /// </summary>
        /// <param name="source"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        protected virtual Task<Dictionary<string, string>> BuildResume(
            IEnumerable<JiraIssue<TFields>> source, IEnumerable<TRow> results)
                => Task.Run(() => new Dictionary<string, string>());
    }
}