using System;
using System.Linq;
using System.Threading.Tasks;
using GitlabTask.Interfaces;

namespace GitlabTask.Tests
{
    public class FakeCommitsGetter : ICommitsGetter
    {
        private readonly GitlabProject[] _projects;

        public FakeCommitsGetter(GitlabProject[] projects)
        {
            _projects = projects;
        }

        public Task<GitlabCommit[]> GetCommitsOfProject(string projectId, DateTimeOffset since)
        {
            var commits = _projects.FirstOrDefault(project => project.Id == projectId)?.Commits;
            return Task.FromResult(commits);
        }
    }
}