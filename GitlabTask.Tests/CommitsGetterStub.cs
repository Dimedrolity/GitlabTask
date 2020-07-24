using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitlabTask.Interfaces;

namespace GitlabTask.Tests
{
    public class CommitsGetterStub : ICommitsGetter
    {
        private readonly IEnumerable<GitlabProject> _projects;

        public CommitsGetterStub(IEnumerable<GitlabProject> projects)
        {
            _projects = projects;
        }

        public Task<IEnumerable<GitlabCommit>> GetCommitsOfProject(string projectId, DateTimeOffset since)
        {
            var commits = _projects.FirstOrDefault(project => project.Id == projectId)?.Commits;
            return Task.FromResult(commits);
        }
    }
}