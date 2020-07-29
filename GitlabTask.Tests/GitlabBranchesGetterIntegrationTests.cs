using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace GitlabTask.Tests
{
    public class GitlabBranchesGetterIntegrationTests
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
        public async Task RequestAllCommitsInLast1000Days()
        {
            var getter = new GitlabBranchesGetter(new JsonConverter(), new ConfigStub(null), _client);

            var branches = await getter.GetBranchesOfProject("20095396");

            await _writer.WriteAsync(string.Join("\r\n", branches.Select((b => b.Name))));

            var reader = new StringReader(_writer.ToString());

            var expected = "master";

            var actual = await reader.ReadToEndAsync();
            Assert.AreEqual(expected, actual);
        }
    }
}