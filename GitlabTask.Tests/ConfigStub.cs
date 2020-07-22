namespace GitlabTask.Tests
{
    public class FakeConfig : IConfig
    {
        private readonly string[] _projectNames;

        public FakeConfig(string[] projectNames)
        {
            _projectNames = projectNames;
        }

        public string[] GetProjectNames()
        {
            return _projectNames;
        }
    }
}