using System.Linq;

namespace GitlabTask
{
    public class GitlabProject
    {
        public string Name { get; }
        public string Id { get; }
        public GitlabCommit[] Commits { get; set; }

        public GitlabProject(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public override string ToString()
        {
            return $"{Name}:" +
                   "\r\n- " + string.Join("\r\n- ", Commits.Select(c => c.Title));
        }
    }
}