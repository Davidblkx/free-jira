using System.Collections.Generic;

namespace FreeJira.Jira.Model
{
    public class JiraIssueWorklogResponse
    {
        public int StartAt { get; set; } = 0;
        public int MaxResults { get; set; } = 50;
        public long Total { get; set; } = 0;
        public IEnumerable<JiraWorklog> Worklogs { get; set; } = new List<JiraWorklog>();
    }
}
