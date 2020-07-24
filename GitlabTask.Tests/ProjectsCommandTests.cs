using System.IO;
using GitlabTask.Commands;
using NUnit.Framework;

namespace GitlabTask.Tests
{
    public class ProjectsCommandTests
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
        public void TestProjectCommand()
        {
            var apnsProject = new GitlabProject("APNS", "1");
            var gitlabProject = new GitlabProject("GitlabTask", "2");

            var projectsFromConfig = new[] {apnsProject, gitlabProject};

            _commandsExecutor.RegisterCommand(new ProjectsCommand(
                new ConfigStub(projectsFromConfig)));
            _commandsExecutor.Execute(new[] {"projects"});

            var reader = new StringReader(_writer.ToString());

            var expected = "Список отслеживаемых проектов (находится в конфиге appsettings.json):\r\n" +
                           "- APNS\r\n" +
                           "- GitlabTask\r\n";
            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }
    }
}