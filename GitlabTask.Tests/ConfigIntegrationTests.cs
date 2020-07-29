using System.Linq;
using GitlabTask.Interfaces;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace GitlabTask.Tests
{
    public class ConfigIntegrationTests
    {
        private IConfig _config;

        [OneTimeSetUp]
        public void Setup()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();
            _config = new Config(configurationBuilder);
        }

        [Test]
        public void Config_ProjectsExist()
        {
            var projects = _config.GetProjects();
            Assert.IsTrue(projects != null && projects.Any());
        }

        [Test]
        public void Config_PatternsExist()
        {
            var patterns = _config.GetPatternsOfExcludedTitle();
            Assert.IsTrue(patterns != null && patterns.Any());
        }

        [Test]
        public void Config_DomainNameExists()
        {
            var domainName = _config.GetGitlabDomainName();
            Assert.IsTrue(!string.IsNullOrEmpty(domainName));
        }

        [Test]
        public void Config_PersonalAccessTokenExists()
        {
            var personalAccessToken = _config.GetPersonalAccessToken();
            Assert.IsTrue(!string.IsNullOrEmpty(personalAccessToken));
        }

        [Test]
        public void Config_BranchesExists()
        {
            var branchesOfProjectAsString = _config.GetBranchesOfProjectAsString("000");
            Assert.IsTrue(string.IsNullOrEmpty(branchesOfProjectAsString));
        }
    }
}