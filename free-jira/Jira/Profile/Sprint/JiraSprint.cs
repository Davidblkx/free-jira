using System;
namespace FreeJira.Jira.Profile.Sprint
{
    /// <summary>
    /// Allow to group results by a time interval
    /// </summary>
    public interface IJiraSprint
    {
        string IdName { get; }
        string Name { get; }
        DateTime Start { get; }
        DateTime End { get; }
    }

    public class JiraSprint : IJiraSprint
    {
        public string IdName { get; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public JiraSprint(string name, DateTime start, DateTime end)
        {
            IdName = GetSprintIdFromName(name);
            Name = name;
            Start = start;
            End = end;
        }

        public override string ToString() {
            var start = Start.ToString("yyyy-MM-dd");
            var end = End.ToString("yyyy-MM-dd");
            return $"{Name} [{start}] -> [{end}]";
        }

        /// <summary>
        /// Returns sprint ID based on name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetSprintIdFromName(string name)
        {
            return name.ToLower().Trim().Replace(' ', '_');
        }
    }
}