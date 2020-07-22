using System.Collections.Generic;

namespace GitlabTask.Tests
{
    public class FakeCommitsGetter : ICommitsGetter
    {
        private readonly Dictionary<string, Commit[]> _projectIdToCommits;

        public FakeCommitsGetter(Dictionary<string, Commit[]> projectIdToCommits)
        {
            _projectIdToCommits = projectIdToCommits;
        }

        public Commit[] GetCommitsOfProjectWithId(string id)
        {
            return _projectIdToCommits[id];
        }
    }
}