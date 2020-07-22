using System.IO;
using System.Linq;

namespace GitlabTask
{
    public class CommitsCommand : Command
    {
        private readonly IConfig _config;
        private readonly ICommitsGetter _commitsGetter;

        public CommitsCommand(IConfig config, ICommitsGetter commitsGetter)
            : base("commits", "show commits")
        {
            _config = config;
            _commitsGetter = commitsGetter;
        }

        public override void Execute(string[] args, TextWriter writer)
        {
            var projectNamesFromConfig = _config.GetProjectNames();

            foreach (var projectName in projectNamesFromConfig)
            {
                var commits = _commitsGetter.GetCommitsOfProjectWithId(projectName);

                var rep = new Repository(projectName, commits);

                writer.WriteLine($"{rep.Name}:" + "\r\n- " +
                                 string.Join("\r\n- ", rep.Commits.Select(c => c.Title)));
                writer.WriteLine();
            }
        }
    }
}