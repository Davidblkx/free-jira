using System.Collections.Generic;
namespace FreeJira.Jira.Model
{
    public class JiraIssueJqlResponse<T> where T : class
    {
        public string Expand { get; set; } = "";
        public int StartAt { get; set; } = 0;
        public int MaxResults { get; set; } = 50;
        public long Total { get; set; } = 0;
        public IEnumerable<JiraIssue<T>> Issues { get; set; } = new List<JiraIssue<T>>();
    }
}