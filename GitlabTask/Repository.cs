namespace GitlabTask
{
    public class Repository
    {
        public string Name { get; }
        public Commit[] Commits { get; }

        public Repository(string name, Commit[] commits)
        {
            Name = name;
            Commits = commits;
        }
    }
}