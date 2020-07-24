using System.Collections.Generic;
using System.Linq;
using GitlabTask.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GitlabTask
{
    public class Config : IConfig
    {
        private readonly IConfiguration _configuration;

        public Config()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();
        }

        public string GetGitlabDomainName()
        {
            return _configuration.GetSection("gitlabDomainName").Value;
        }

        public string GetPersonalAccessToken()
        {
            return _configuration.GetSection("personalAccessToken").Value;
        }

        public IEnumerable<GitlabProject> GetProjects()
        {
            return _configuration.GetSection("projects").GetChildren()
                .Select(projectFromJson => new GitlabProject(projectFromJson["name"], projectFromJson["projectId"]));
        }

        public IEnumerable<string> GetPatternsOfExcludedTitle()
        {
            return _configuration.GetSection("patternOfExcludedTitle").GetChildren()
                .Select(pattern => pattern.Value);
        }
    }
}