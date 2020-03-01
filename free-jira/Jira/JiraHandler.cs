namespace free_jira.Jira
{
    public static class JiraHandler
    {
        public static Atlassian.Jira.Jira GetJiraClient(JiraProfile profile) {
            var client = Atlassian.Jira.Jira.CreateRestClient(profile.Url, profile.User, profile.Pass);
            client.Issues.MaxIssuesPerRequest = 250;
            return client;
        }
    }
}