using System;

namespace FreeJira.Jira.Model
{
    public class JiraWorklog
    {
        public string Self { get; set; } = "";
        public string Comment { get; set; } = "";
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Started { get; set; }
        public string TimeSpent { get; set; } = "";
        public long TimeSpentSeconds { get; set; }
        public string Id { get; set; } = "";
        public string IssueId { get; set; } = "";
        public JiraAuthor? Author { get; set; }
        public JiraAuthor? UpdateAuthor { get; set; }
    }
}