using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GitlabTask.Interfaces;

namespace GitlabTask
{
    public class GitlabBranchesGetter : IBranchesGetter
    {
        private static HttpClient _client;
        private readonly IJsonConverter _jsonConverter;

        private readonly string _domainName;
        private readonly string _personalAccessToken;

        public GitlabBranchesGetter(IJsonConverter jsonConverter, IConfig config, HttpClient client)
        {
            _jsonConverter = jsonConverter;

            _personalAccessToken = config.GetPersonalAccessToken();
            _domainName = config.GetGitlabDomainName();

            _client = client;
        }

        public async Task<IEnumerable<GitlabBranch>> GetBranchesOfProject(string projectId)
        {
            var url = $"https://{_domainName}/api/v4/projects/{projectId}/repository/branches";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("PRIVATE-TOKEN", _personalAccessToken);

            using var response = await _client.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();
            return _jsonConverter.ConvertJsonToBranches(json);
        }
    }
}