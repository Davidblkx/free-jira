using System;
namespace free_jira.Jira.Profile
{
    public class JiraSprint
    {
        public string IdName { get; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public JiraSprint(string name, DateTime start, DateTime end) {
            IdName = GetSprintIdFromName(name);
            Name = name;
            Start = start;
            End = end;
        }

        /// <summary>
        /// Returns sprint ID based on name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetSprintIdFromName(string name) {
            return name.ToLower().Trim().Replace(' ', '_');
        }
    }
}