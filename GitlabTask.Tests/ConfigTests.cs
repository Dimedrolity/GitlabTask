using System.Linq;
using NUnit.Framework;

namespace GitlabTask.Tests
{
    public class ConfigTests
    {
        [Test]
        public void ConfigProjectsIsNotEmpty()
        {
            var config = new Config();
            var projects = config.GetProjects();
            Assert.IsTrue(projects != null && projects.Any());
        }
        
        [Test]
        public void ConfigPatternsIsNotEmpty()
        {
            var config = new Config();
            var patterns = config.GetPatternsOfExcludedTitle();
            Assert.IsTrue(patterns != null && patterns.Any());
        }
    }
}