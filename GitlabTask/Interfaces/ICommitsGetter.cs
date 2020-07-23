using System;
using System.Threading.Tasks;

namespace GitlabTask.Interfaces
{
    public interface ICommitsGetter
    {
        public Task<GitlabCommit[]> GetCommitsOfProject(string projectId, DateTimeOffset since);
    }
}