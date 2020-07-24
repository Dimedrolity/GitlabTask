using System.IO;
using GitlabTask.Commands;
using NUnit.Framework;

namespace GitlabTask.Tests
{
    public class CommitsCommandTests
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
        public void CommitsCommandReturnsCommitsInChronologicalOrder()
        {
            var apnsProject = new GitlabProject("APNS", "1");
            var apnsCommits = new[]
            {
                new GitlabCommit("Добавлен генератор токенов", "2020-07-23T16:04:00Z"),
                new GitlabCommit("Добавлены запросы в APNS с использованием Http2", "2020-07-22T16:04:00Z"),
                new GitlabCommit("Добавлены классы уведомлений", "2020-07-21T16:04:00Z"),
            };
            apnsProject.Commits = apnsCommits;

            var projectsFromConfig = new[] {apnsProject};

            _commandsExecutor.RegisterCommand(new CommitsCommand(
                new ConfigStub(projectsFromConfig),
                new CommitsGetterStub(projectsFromConfig)));
            _commandsExecutor.Execute(new[] {"commits"});

            var reader = new StringReader(_writer.ToString());

            var expected = "APNS:\r\n" +
                           "- Добавлены классы уведомлений\r\n" +
                           "- Добавлены запросы в APNS с использованием Http2\r\n" +
                           "- Добавлен генератор токенов\r\n" +
                           "\r\n";
            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }
    }
}