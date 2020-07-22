using System;
using System.Threading.Tasks;

namespace GitlabTask
{
    public interface ICommitsGetter
    {
        public Task<Commit[]> GetCommitsOfProject(string projectId, DateTimeOffset since);
    }
}