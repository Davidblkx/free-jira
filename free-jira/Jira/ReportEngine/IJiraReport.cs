using System.Collections.Generic;
using System.Threading.Tasks;
using FreeJira.Jira;
using FreeJira.Jira.Profile.Sprint;

namespace FreeJira.Jira.ReportEngine
{
    /// <summary>
    /// Can convert a JQL result to a report that can be exported to CSV or printed to screen
    /// </summary>
    /// <typeparam name="TRow">Report row result</typeparam>
    public interface IJiraReport<TRow> : IJiraReport
        where TRow : class
    {
        /// <summary>
        /// Build report rows
        /// </summary>
        /// <param name="client">Client to request issues</param>
        /// <param name="reportParams"></param>
        /// <param name="sprint"></param>
        /// <returns>List of rows, and a dictionary with resume</returns>
        Task<(IEnumerable<TRow>, Dictionary<string, string>)> BuildReport(
            JiraClient client, IJiraSprint? sprint, Dictionary<string, string> reportParams);

        /// <summary>
        /// Convert results to a CSV string
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        Task<string> ToCSV((IEnumerable<TRow>, Dictionary<string, string>) result);
    }

    public interface IJiraReport {
        /// <summary>
        /// Name of report
        /// </summary>
        /// <value></value>
        string Name { get; }

        /// <summary>
        /// Description
        /// </summary>
        /// <value></value>
        string Description { get; }

        /// <summary>
        /// Expected params, description
        /// </summary>
        /// <value></value>
        Dictionary<string, string> ParamsMap { get; }

        /// <summary>
        /// Print report into Console
        /// </summary>
        /// <param name="client"></param>
        /// <param name="sprint"></param>
        /// <param name="reportParams"></param>
        /// <returns></returns>
        Task PrintReport(
            JiraClient client, IJiraSprint? sprint, Dictionary<string, string> reportParams);

        /// <summary>
        /// Convert results to a CSV string
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        Task<string> ToCSV(
            JiraClient client, IJiraSprint? sprint, Dictionary<string, string> reportParams);
    }
}