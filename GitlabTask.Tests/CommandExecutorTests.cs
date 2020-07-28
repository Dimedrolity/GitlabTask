using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace GitlabTask.Tests
{
    public class CommandExecutorTests
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
        public async Task ExecuteUnknownCommand()
        {
            await _commandsExecutor.Execute("unknown");

            var expected = "Неизвестная команда unknown\r\n";

            var reader = new StringReader(_writer.ToString());
            var actual = await reader.ReadToEndAsync();

            Assert.AreEqual(expected, actual);
        }
    }
}