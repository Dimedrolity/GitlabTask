using System.Collections.Generic;
using Newtonsoft.Json;

namespace GitlabTask
{
    public class GitlabBranch
    {
        [JsonProperty(PropertyName = "name")] 
        public string Name { get; }

        public IEnumerable<GitlabCommit> Commits { get; set; }

        public GitlabBranch(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"branch {Name}:" +
                   "\r\n-- " + string.Join("\r\n-- ", Commits);
        }
    }
}