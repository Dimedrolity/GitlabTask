using System.Collections.Generic;
using System.Linq;
using GitlabTask.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GitlabTask
{
    public class Config : IConfig
    {
        private readonly IConfiguration _configuration;

        public Config(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetGitlabDomainName()
        {
            return _configuration["gitlabDomainName"];
        }

        public string GetPersonalAccessToken()
        {
            return _configuration["personalAccessToken"];
        }

        public IEnumerable<GitlabProject> GetProjects()
        {
            return _configuration.GetSection("projects").GetChildren()
                .Select(projectFromJson => new GitlabProject(projectFromJson["name"], projectFromJson["Id"]));
        }

        public IEnumerable<string> GetPatternsOfExcludedTitle()
        {
            return _configuration.GetSection("patternOfExcludedTitle").GetChildren()
                .Select(pattern => pattern.Value);
        }

        public string GetBranchesStringOfProject(string projectId)
        {
            var project = _configuration.GetSection("projects").GetChildren()
                .FirstOrDefault(proj => proj["Id"] == projectId);

            return  project?["branches"];
        }
    }
}