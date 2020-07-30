using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitlabTask.Interfaces
{
    public interface IBranchesGetter
    {
        public Task<IEnumerable<GitlabBranch>> GetBranchesOfProject(string projectId);
    }
}