using System.Linq;
using Newtonsoft.Json;

namespace GitlabTask
{
    public class GitlabBranch
    {
        [JsonProperty(PropertyName = "name")] public string Name { get; }

        public GitlabCommit[] Commits { get; set; }

        public GitlabBranch(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $" {Name}:\r\n" +
                   " - " + string.Join("\r\n - ", Commits.Select(c => c.ToString()));
        }
    }
}