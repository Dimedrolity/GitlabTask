using System.Collections.Generic;
using System.Threading.Tasks;
using GitlabTask.Interfaces;

namespace GitlabTask.Tests
{
    public class BranchesGetterStub : IBranchesGetter
    {
        private readonly IEnumerable<GitlabBranch> _branches;

        public BranchesGetterStub(IEnumerable<GitlabBranch> branches)
        {
            _branches = branches;
        }

        public Task<IEnumerable<GitlabBranch>> GetBranchesOfProject(string projectId)
        {
            return Task.FromResult(_branches);
        }
    }
}