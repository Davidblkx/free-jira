using System.Linq;
using System.Threading.Tasks;
using FreeJira.Jira.Client;
using FreeJira.Jira.Model;
using Optional;

namespace FreeJira.Jira 
{
    public class JiraUserClient
    {
        private readonly IJiraRestClient _restClient;

        public JiraUserClient(IJiraRestClient restClient) {
            _restClient = restClient;
        }
        
        public async Task<Option<string>> loadAccountId(string username)
        {
            var request = JiraAccountRestCall.Create(username);
            var res = await _restClient.ExecuteAsync(request);
            return res.Map(e => e.FirstOrDefault()).Map(e => e.AccountId);
        }
    }
}