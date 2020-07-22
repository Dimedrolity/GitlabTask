using System.IO;
using System.Threading.Tasks;
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
            _commandsExecutor.RegisterCommand(new HelpCommand(_commandsExecutor.GetRegisteredCommandNames));
        }

        [Test]
        public async Task TestRequestToMyPublicGitlab()
        {
            var projectNamesFromConfig = new[] {"20095396"};

            _commandsExecutor.RegisterCommand(new CommitsCommand(
                new FakeConfig(projectNamesFromConfig),
                new GitlabCommitsGetter(new JsonConverter())));
            
            await _commandsExecutor.Execute(new[] {"commits", "0", "1000"});

            var reader = new StringReader(_writer.ToString());

            const string expected = "20095396:\r\n" +
                                    "- Initial template creation\r\n" +
                                    "- Update README.md\r\n" +
                                    "\r\n";
            
            var actual = await reader.ReadToEndAsync();
            Assert.AreEqual(expected, actual);
        }
    }
}