
using System.Collections.Generic;
using System.Linq;
using FreeJira.Jira.Client;
using Optional;
using RestSharp;

namespace FreeJira.Jira.Model
{
    public class JiraAccount
    {
        public string Username { get; set; } = "";
        public string AccountId { get; set; } = "";
    }

    public class JiraAccountRestCall :
        IJiraRestCall<IEnumerable<string>, List<JiraAccount>>
    {
        public JiraRestVersion Version => JiraRestVersion.RestClientV2;
        public string Endpoint {
            get {
                var baseUrl = "/user/bulk/migration";
                var param = "?" + Body
                    .Map(e => e.Select(p => $"username={p}"))
                    .Map(e => string.Join('&', e))
                    .ValueOr("");
                return baseUrl + param;
            }
        }
        public Method Method => Method.GET;
        public Option<IEnumerable<string>> Body { get; private set; }

        public static JiraAccountRestCall Create(IEnumerable<string> users)
            => new JiraAccountRestCall { Body = Option.Some(users) };
        public static JiraAccountRestCall Create(params string[] users)
            => new JiraAccountRestCall { Body = Option.Some<IEnumerable<string>>(users.ToList()) };
    }
}