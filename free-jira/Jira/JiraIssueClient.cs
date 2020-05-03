using System.Threading.Tasks;
using FreeJira.Jira.Client;
using FreeJira.Jira.Model;
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
    }
}