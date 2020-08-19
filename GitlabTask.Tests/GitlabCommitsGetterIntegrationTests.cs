using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using GitlabTask.Interfaces;
using NUnit.Framework;

namespace GitlabTask.Tests
{
    public class GitlabCommitsGetterIntegrationTests
    {
        private StringWriter _writer;

        private static HttpClient _client;

        [OneTimeSetUp]
        public void SetUpFixture()
        {
            _client = new HttpClient();
        }

        [SetUp]
        public void Setup()
        {
            _writer = new StringWriter();
        }

        [Test]
        public async Task RequestAllCommitsFrom2018()
        {
            var config = A.Fake<IConfig>();
            A.CallTo(() => config.GetGitlabDomainName()).Returns("gitlab.com");
            var getter = new GitlabCommitsGetter(new JsonConverter(), config, _client);

            var commits = await getter.GetCommitsOfProject("20095396", "master",
                new DateTimeOffset(new DateTime(2018, 1, 1)));

            await _writer.WriteAsync(string.Join("\r\n", commits));

            var reader = new StringReader(_writer.ToString());

            var expected = "Update README.md\r\n" +
                           "Initial template creation";

            var actual = await reader.ReadToEndAsync();
            Assert.AreEqual(expected, actual);
        }
    }
}