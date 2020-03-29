using Optional;
using RestSharp;
namespace free_jira.Jira.Client
{
    public interface IJiraRestCall<TBody, TResponse>
    {
        JiraRestVersion Version { get; }
        string Endpoint { get; }
        Method Method { get; }
        Option<TBody> Body { get; }
    }
}