using System.IO;
using System.Threading.Tasks;
using GitlabTask.Commands;
using NUnit.Framework;

namespace GitlabTask.Tests
{
    public class GitlabCommitsGetterTests
    {
        private CommandsExecutor _commandsExecutor;
        private StringWriter _writer;

        [SetUp]
        public void Setup()
        {
            _writer = new StringWriter();
            _commandsExecutor = new CommandsExecutor(_writer);
        }

        [Test]
        public async Task RequestCommitsInLast1000Days(
            [Values(new[] {"commits", "0", "1000"}, new[] {"commits", "24000"})]
            string[] args)
        {
            var projectNamesFromConfig = new[]
            {
                new GitlabProject("MyTestProject", "20095396")
            };

            _commandsExecutor.RegisterCommand(new CommitsCommand(
                new ConfigStub(projectNamesFromConfig),
                new GitlabCommitsGetter(new JsonConverter())));

            await _commandsExecutor.Execute(args);

            var reader = new StringReader(_writer.ToString());

            var expected = "MyTestProject:\r\n" +
                           "- Initial template creation\r\n" +
                           "- Update README.md\r\n" +
                           "\r\n";

            var actual = await reader.ReadToEndAsync();
            Assert.AreEqual(expected, actual);
        }
    }
}