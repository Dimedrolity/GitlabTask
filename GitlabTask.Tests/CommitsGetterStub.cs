using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitlabTask.Interfaces;

namespace GitlabTask.Tests
{
    public class CommitsGetterStub : ICommitsGetter
    {
        private readonly IEnumerable<GitlabCommit> _commits;

        public CommitsGetterStub(IEnumerable<GitlabCommit> commits)
        {
            _commits = commits;
        }

        public Task<IEnumerable<GitlabCommit>> GetCommitsOfProject(string projectId, string branchName,
            DateTimeOffset since)
        {
            return Task.FromResult(_commits);
        }
    }
}