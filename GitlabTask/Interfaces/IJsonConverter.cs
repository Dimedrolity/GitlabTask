using System.Collections.Generic;

namespace GitlabTask.Interfaces
{
    public interface IJsonConverter
    {
        public IEnumerable<GitlabCommit> ConvertJsonToCommits(string json);
        public IEnumerable<GitlabBranch> ConvertJsonToBranches(string json);
    }
}