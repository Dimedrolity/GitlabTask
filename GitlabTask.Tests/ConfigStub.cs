using System.Collections.Generic;
using GitlabTask.Interfaces;

namespace GitlabTask.Tests
{
    public class ConfigStub : IConfig
    {
        private readonly GitlabProject[] _projects;

        public ConfigStub(GitlabProject[] projects)
        {
            _projects = projects;
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