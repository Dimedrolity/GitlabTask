using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitlabTask.Tests
{
    public class FakeCommitsGetter : ICommitsGetter
    {
        private readonly Dictionary<string, Commit[]> _projectIdToCommits;

        public FakeCommitsGetter(Dictionary<string, Commit[]> projectIdToCommits)
        {
            _projectIdToCommits = projectIdToCommits;
        }

        public Task<Commit[]> GetCommitsOfProject(string projectId, DateTimeOffset since)
        {
            return Task.FromResult(_projectIdToCommits[projectId]);
        }
    }
}