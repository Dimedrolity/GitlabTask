using System.Collections.Generic;
using GitlabTask.Interfaces;

namespace GitlabTask.Tests
{
    public class ConfigStub : IConfig
    {
        private readonly IEnumerable<GitlabProject> _projects;

        public ConfigStub(IEnumerable<GitlabProject> projects)
        {
            _projects = projects;
        }

        public string GetGitlabDomainName()
        {
            return "gitlab.com";    //официальный гитлаб
        }

        public string GetPersonalAccessToken()
        {
            return null;    //без токена
        }

        public IEnumerable<GitlabProject> GetProjects()
        {
            return _projects;
        }

        public IEnumerable<string> GetPatternsOfExcludedTitle()
        {
            return new[] {"人+"};    //в коммитах точно не будет китайского иероглифа
        }
    }
}