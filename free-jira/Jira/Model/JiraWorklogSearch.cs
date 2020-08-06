using System.Collections.Generic;
namespace FreeJira.Jira.Model
{
    public class JiraWorklogSearch
    {
        public int StartAt { get; set; }
        public int MaxResults { get; set; } = 500;
        public int Total { get; set; }
        public IEnumerable<JiraWorklog> Worklogs { get; set; }
            = new List<JiraWorklog>();
    }
}