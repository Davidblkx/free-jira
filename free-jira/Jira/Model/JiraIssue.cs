namespace free_jira.Jira.Model
{
    public class JiraIssue<T> where T : class
    {
        public string Expand { get; set; } = "";
        public string Self { get; set; } = "";
        public string Id { get; set; } = "";
        public string Key { get; set; } = "";
        public T? Fields { get; set; }
    }
}