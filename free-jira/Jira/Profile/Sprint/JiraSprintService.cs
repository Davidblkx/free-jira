using System;
using System.Collections.Generic;
using LiteDB;

namespace FreeJira.Jira.Profile.Sprint
{
    public interface IJiraSprintService
    {
        bool CreateSprint(string name, DateTime start, DateTime end);
        bool CreateSprint(IJiraSprint sprint);
        bool DeleteSprint(string name);
        JiraSprint? GetActiveSprint();
        JiraSprint? GetSprintByName(string name);
        IEnumerable<JiraSprint> GetSprints();
        IJiraProfileService GetParent();
    }

    public class JiraSprintService : IJiraSprintService
    {
        protected static readonly string COLLECTION_SPRINTS = "SPRINTS";
        private readonly LiteDatabase _db;
        private readonly IJiraProfileService _profile;

        public JiraSprintService(JiraProfileService profile)
        {
            _db = profile.Db;
            _profile = profile;
            BsonMapper.Global
                .Entity<JiraSprint>()
                .Id(e => e.IdName);
        }

        /// <summary>
        /// Return parent service
        /// </summary>
        /// <returns></returns>
        public IJiraProfileService GetParent() {
            return _profile;
        }

        /// <summary>
        /// Return available sprints
        /// </summary>
        /// <returns></returns>
        public IEnumerable<JiraSprint> GetSprints()
        {
            return GetCollection().FindAll();
        }

        /// <summary>
        /// Create a new sprint, if name already exists returns false
        /// </summary>
        /// <param name="name">Sprint name</param>
        /// <param name="start">start date</param>
        /// <param name="end">end date</param>
        /// <returns>false, if sprint already exists</returns>
        public bool CreateSprint(string name, DateTime start, DateTime end)
        {
            var sprint = new JiraSprint(name, start, end);
            var col = GetCollection();
            if (col.FindById(sprint.IdName) is null)
            {
                col.Insert(sprint);
                return true;
            }

            return false;
        }

        public bool CreateSprint(IJiraSprint sprint) {
            return CreateSprint(sprint.Name, sprint.Start, sprint.End);
        }

        /// <summary>
        /// Return first sprint that is active today
        /// </summary>
        /// <returns></returns>
        public JiraSprint? GetActiveSprint()
        {
            var today = DateTime.Now;
            return GetCollection()
                .FindOne(s => s.Start <= today && s.End >= today);
        }

        /// <summary>
        /// Search for sprint by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JiraSprint? GetSprintByName(string name)
        {
            var id = JiraSprint.GetSprintIdFromName(name);
            return GetCollection().FindById(id);
        }

        /// <summary>
        /// Delete a sprint
        /// </summary>
        /// <param name="name"></param>
        /// <returns>false, if not found</returns>
        public bool DeleteSprint(string name)
        {
            var id = JiraSprint.GetSprintIdFromName(name);
            return GetCollection().Delete(id);
        }

        private ILiteCollection<JiraSprint> GetCollection()
        {
            return _db.GetCollection<JiraSprint>(COLLECTION_SPRINTS);
        }
    }
}