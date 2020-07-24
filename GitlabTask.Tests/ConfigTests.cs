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

        [Test]
        public void ConfigGitlabDomainNameExists()
        {
            var config = new Config();
            var domainName = config.GetGitlabDomainName();
            Assert.IsTrue(domainName != null);
        }

        [Test]
        public void ConfigPersonalAccessTokenExists()
        {
            var config = new Config();
            var personalAccessToken = config.GetPersonalAccessToken();
            Assert.IsTrue(personalAccessToken != null);
        }
    }
}