using System.Threading.Tasks;
using FreeJira.Jira.Client;
using FreeJira.Jira.Model;
using FreeJira.Jira.RestCall;
using Optional;

namespace FreeJira.Jira
{
    public class JiraIssueClient
    {
        private readonly IJiraRestClient _restClient;

        public JiraIssueClient(IJiraRestClient restClient) {
            _restClient = restClient;
        }
        
        public Task<Option<JiraIssueJqlResponse<T>>> Jql<T>(string jql, T? _ = null) where T : class {
            var request = JiraIssueJqlSearch<T>.FromType(jql);
            return _restClient.ExecuteAsync(request);
        }

        public Task<Option<JiraIssueWorklogResponse>> IssueWorklogs(string keyOrId)
        {
            var request = JiraIssueWorklog.ForIssue(keyOrId);
            return _restClient.ExecuteAsync(request);
        }
    }
}