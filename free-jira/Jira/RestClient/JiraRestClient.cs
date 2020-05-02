using System.IO;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.SystemTextJson;
using System;
using Optional;
using System.Text.Json;

using static Optional.Option;

namespace FreeJira.Jira.Client
{
    public interface IJiraRestClient
    {
        IAuthenticator Authenticator { get; }
        Uri JiraServelUrl { get; }

        Task<Option<TResponse>> ExecuteAsync<TBody, TResponse>(IJiraRestCall<TBody, TResponse> restCall, CancellationToken cancellation = default);
        Task<IRestResponse> ExecuteRawRequestAsync<TBody, TResponse>(IJiraRestCall<TBody, TResponse> restCall, CancellationToken cancellation = default);
    }

    public class JiraRestClient : IJiraRestClient
    {
        public IAuthenticator Authenticator { get; }
        public Uri JiraServelUrl { get; }

        private readonly RestClient _client;

        public JiraRestClient(IAuthenticator authenticator, string url)
        {
            Authenticator = authenticator;
            JiraServelUrl = new Uri(url);
            _client = BuildClient(JiraServelUrl, authenticator);
        }

        /// <summary>
        /// Creates a request client with base user/pass
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <param name="serverUrl"></param>
        /// <returns></returns>
        public static JiraRestClient FromBasicAuth(string user, string pass, string serverUrl)
        {
            var baseAuth = new HttpBasicAuthenticator(user, pass);
            return new JiraRestClient(baseAuth, serverUrl);
        }

        /// <summary>
        /// Execute request and deserialize response
        /// </summary>
        /// <param name="restCall"></param>
        /// <param name="cancellation"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<Option<TResponse>> ExecuteAsync<TBody, TResponse>(
            IJiraRestCall<TBody, TResponse> restCall,
            CancellationToken cancellation = default
        ) {
            var res = await ExecuteRawRequestAsync(restCall, cancellation);
            if (!res.IsSuccessful) return None<TResponse>();
            try
            {
                using var stream = new MemoryStream(res.RawBytes);
                return Some(await JsonSerializer.DeserializeAsync<TResponse>(stream, BuildJsonOptions()));
            }
            catch (Exception e) { 
                Console.WriteLine(e.Message);
                return None<TResponse>(); 
            }
        }

        /// <summary>
        /// Execute request
        /// </summary>
        /// <param name="restCall"></param>
        /// <param name="cancellation"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task<IRestResponse> ExecuteRawRequestAsync<TBody, TResponse>(
            IJiraRestCall<TBody, TResponse> restCall, 
            CancellationToken cancellation = default
        ) {
            var req = FromRestCall(restCall);
            return _client.ExecuteAsync(req, cancellation);
        }

        private RestRequest FromRestCall<TBody, TResponse>(IJiraRestCall<TBody, TResponse> restCall)
        {
            var url = $"/rest/api/{(int)restCall.Version}/{restCall.Endpoint}";
            var req = new RestRequest(url, restCall.Method, DataFormat.Json);
            restCall.Body.MatchSome(e => req.AddJsonBody(e));
            return req;
        }

        private RestClient BuildClient(Uri baseUrl, IAuthenticator authenticator) {
            var cli = new RestClient(baseUrl);
            var jsonOptions = BuildJsonOptions();
            cli.UseSystemTextJson(jsonOptions);
            cli.Authenticator = authenticator;
            return cli;
        }

        private JsonSerializerOptions BuildJsonOptions() {
            return new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                IgnoreNullValues = true,
                WriteIndented = true,
            };
        }
    }
}