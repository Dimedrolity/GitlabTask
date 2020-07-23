using NUnit.Framework;

namespace GitlabTask.Tests
{
    public class ConfigTests
    {
        [Test]
        public void TestConfig()
        {
            var config = new Config();
            var projects = config.GetProjects();
            Assert.IsTrue(projects != null && projects.Length != 0);
        }
    }
}