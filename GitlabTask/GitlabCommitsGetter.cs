using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GitlabTask.Interfaces;

namespace GitlabTask
{
    public class GitlabCommitsGetter : ICommitsGetter
    {
        private static readonly HttpClient Client = new HttpClient();
        private readonly IJsonConverter _jsonConverter;

        private readonly string _domainName;
        private readonly string _personalAccessToken;

        public GitlabCommitsGetter(IJsonConverter jsonConverter, IConfig config)
        {
            _jsonConverter = jsonConverter;

            _personalAccessToken = config.GetPersonalAccessToken();
            _domainName = config.GetGitlabDomainName();
        }

        public async Task<IEnumerable<GitlabCommit>> GetCommitsOfProject(string projectId, DateTimeOffset since)
        {
            //формат даты: YYYY-MM-DDTHH:MM:SSZ, например 2020-07-22T17:40:00Z
            var url = $"https://{_domainName}/api/v4/projects/{projectId}/repository/commits" + $"?since={since:s}Z";
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            req.Headers.Add("PRIVATE-TOKEN", _personalAccessToken);

            using var response = await Client.SendAsync(req);

            var json = await response.Content.ReadAsStringAsync();
            return _jsonConverter.ConvertJsonToCommits(json);
        }
    }
}