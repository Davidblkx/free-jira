using System.Threading.Tasks;
using free_jira.Jira.Client;
using free_jira.Jira.Model;
using Optional;

namespace free_jira.Jira
{
    public class JiraIssueClient
    {
        private readonly IJiraRestClient _restClient;

        public JiraIssueClient(IJiraRestClient restClient) {
            _restClient = restClient;
        }
        
        public Task<Option<JiraIssueJqlResponse<T>>> Jql<T>(string jql) where T : class {
            var request = JiraIssueJqlSearch<T>.FromType(jql);
            return _restClient.ExecuteAsync(request);
        }
    }
}