using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GitlabTask.Interfaces;

namespace GitlabTask
{
    public class GitlabCommitsGetter : ICommitsGetter
    {
        private readonly IJsonConverter _jsonConverter;

        public GitlabCommitsGetter(IJsonConverter jsonConverter)
        {
            _jsonConverter = jsonConverter;
        }

        private static readonly HttpClient Client = new HttpClient();

        public async Task<IEnumerable<GitlabCommit>> GetCommitsOfProject(string projectId, DateTimeOffset since)
        {
            //формат даты: YYYY-MM-DDTHH:MM:SSZ, например 2020-07-22T17:40:00Z
            var url = $"https://gitlab.com/api/v4/projects/{projectId}/repository/commits" + $"?since={since:s}Z";

            using var response = await Client.GetAsync(url);

            var json = await response.Content.ReadAsStringAsync();
            return _jsonConverter.ConvertJsonToCommits(json);
        }
    }
}