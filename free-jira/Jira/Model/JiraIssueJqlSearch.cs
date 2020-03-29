using System.Collections.Generic;
using free_jira.Jira.Client;
using Optional;
using RestSharp;

using static Optional.Option;

namespace free_jira.Jira.Model
{
    public class JiraIssueJqlSearch<T> : 
        IJiraRestCall<JiraIssueJqlSearch<T>.Params, JiraIssueJqlResponse<T>> 
        where T : class
    {
        public JiraRestVersion Version => JiraRestVersion.RestClientV2;

        public string Endpoint => "/search";

        public Method Method => Method.POST;

        public Option<Params> Body { get; private set; }

        /// <summary>
        /// Create a JQL request from a Type [T]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static JiraIssueJqlSearch<T> FromType(string jql) {
            var req = new JiraIssueJqlSearch<T>();
            var data = new Params { Jql = jql };

            foreach (var p in typeof(T).GetProperties()) {
                data.Fields.Add(p.Name);
            }
            
            req.Body = Some(data);
            return req;
        }

        public class Params {
            public string Jql { get; set; } = "";
            public List<string> Fields { get; set; } = new List<string>();
        }
    }
}