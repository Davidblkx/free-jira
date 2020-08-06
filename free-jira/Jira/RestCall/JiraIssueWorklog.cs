using FreeJira.Jira.Client;
using FreeJira.Jira.Model;
using FreeJira.Jira.RestClient;
using Optional;
using RestSharp;

namespace FreeJira.Jira.RestCall
{
    public class JiraIssueWorklog : IJiraRestCall<INoBody, JiraIssueWorklogResponse>
    {
        public JiraRestVersion Version => JiraRestVersion.RestClientV2;

        public string IssueIdOrKey { get; set; } = "";

        public string Endpoint => $"issue/{IssueIdOrKey}/worklog";

        public Method Method => Method.GET;

        public Option<INoBody> Body => Option.None<INoBody>();

        public static JiraIssueWorklog ForIssue(string keyOrId)
            => new JiraIssueWorklog() { IssueIdOrKey = keyOrId };
    }
}