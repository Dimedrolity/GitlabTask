using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitlabTask.Interfaces
{
    public interface ICommitsGetter
    {
        public Task<IEnumerable<GitlabCommit>> GetCommitsOfProject(string projectId, DateTimeOffset since);
    }
}