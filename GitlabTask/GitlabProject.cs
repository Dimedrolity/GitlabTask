using System.Linq;

namespace GitlabTask
{
    public class GitlabProject
    {
        public string Name { get; }
        public string Id { get; }
        public GitlabBranch[] Branches { get; set; }

        public GitlabProject(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public override string ToString()
        {
            return $"{Name}:\r\n\r\n" +
                   string.Join("\r\n",
                       Branches.Where(b => b.Commits != null && b.Commits.Length != 0)
                           .Select(b => b.ToString())) +
                   "\r\n\r\n";
        }
    }
}