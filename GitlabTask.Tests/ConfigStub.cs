using GitlabTask.Interfaces;

namespace GitlabTask.Tests
{
    public class FakeConfig : IConfig
    {
        private readonly GitlabProject[] _projects;

        public FakeConfig(GitlabProject[] projects)
        {
            _projects = projects;
        }

        public GitlabProject[] GetProjects()
        {
            return _projects;
        }
    }
}