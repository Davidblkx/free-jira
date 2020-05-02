using FreeJira.Jira.Client;

namespace FreeJira.Jira
{
    public class JiraClient
    {
        public IJiraRestClient RestClient { get; }
        public JiraIssueClient IssueClient { get; }

        public JiraClient(IJiraRestClient client) {
            RestClient = client;
            IssueClient = new JiraIssueClient(client);
        }

        public static JiraClient FromBaseAuth(string user, string pass, string url) {
            return new JiraClient(JiraRestClient.FromBasicAuth(user, pass, url));
        }
    }
}