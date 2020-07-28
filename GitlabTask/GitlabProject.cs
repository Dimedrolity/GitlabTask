using System.Collections.Generic;

namespace GitlabTask
{
    public class GitlabProject
    {
        public string Name { get; }
        public string Id { get; }
        public IEnumerable<GitlabBranch> Branches { get; set; }

        public GitlabProject(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public override string ToString()
        {
            return $"{Name}:" +
                   "\r\n- " + string.Join("\r\n- ", Branches);
        }
    }
}