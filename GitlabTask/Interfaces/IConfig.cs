using System.Collections.Generic;

namespace GitlabTask.Interfaces
{
    public interface IConfig
    {
        public IEnumerable<GitlabProject> GetProjects();
        public IEnumerable<string> GetPatternsOfExcludedTitle();
    }
}