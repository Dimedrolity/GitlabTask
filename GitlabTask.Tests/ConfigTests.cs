using System.Linq;
using GitlabTask.Interfaces;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace GitlabTask.Tests
{
    public class ConfigTests
    {
        private IConfig _config;

        [SetUp]
        public void Setup()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();
            _config = new Config(configurationBuilder);
        }

        [Test]
        public void ConfigProjectsIsNotEmpty()
        {
            var projects = _config.GetProjects();
            Assert.IsTrue(projects != null && projects.Any());
        }

        [Test]
        public void ConfigPatternsIsNotEmpty()
        {
            var patterns = _config.GetPatternsOfExcludedTitle();
            Assert.IsTrue(patterns != null && patterns.Any());
        }

        [Test]
        public void ConfigGitlabDomainNameExists()
        {
            var domainName = _config.GetGitlabDomainName();
            Assert.IsTrue(domainName != null);
        }

        [Test]
        public void ConfigPersonalAccessTokenExists()
        {
            var personalAccessToken = _config.GetPersonalAccessToken();
            Assert.IsTrue(personalAccessToken != null);
        }
    }
}