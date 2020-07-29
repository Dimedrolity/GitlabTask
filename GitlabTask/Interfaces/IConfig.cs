using System.Collections.Generic;

namespace GitlabTask.Interfaces
{
    public interface IConfig
    {
        public string GetGitlabDomainName();
        public string GetPersonalAccessToken();
        public IEnumerable<GitlabProject> GetProjects();
        public IEnumerable<string> GetPatternsOfExcludedTitle();
        public string GetBranchesOfProjectAsString(string projectId);
    }
}