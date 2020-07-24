using System.IO;
using GitlabTask.Commands;
using NUnit.Framework;

namespace GitlabTask.Tests
{
    public class HelpCommandTests
    {
        private CommandsExecutor _commandsExecutor;
        private StringWriter _writer;

        [SetUp]
        public void Setup()
        {
            _writer = new StringWriter();
            _commandsExecutor = new CommandsExecutor(_writer);
            _commandsExecutor.RegisterCommand(new HelpCommand(() => _commandsExecutor.Commands));
        }

        [Test]
        public void HelpCommandWithoutArguments()
        {
            _commandsExecutor.RegisterCommand(new CommitsCommand(new ConfigStub(null), new CommitsGetterStub(null)));
            _commandsExecutor.RegisterCommand(new ProjectsCommand(new ConfigStub(null)));

            _commandsExecutor.Execute(new[] {"help"});

            var reader = new StringReader(_writer.ToString());

            var expected = "Список доступных команд:\r\n" +
                           "- help\r\n" +
                           "- commits\r\n" +
                           "- projects\r\n" +
                           "Более подробная информация -> help <command>\n";

            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void HelpCommandForHelpCommand()
        {
            _commandsExecutor.Execute(new[] {"help", "help"});

            var reader = new StringReader(_writer.ToString());

            var expected = "Описание команды help:\r\n"
                           + "- " + "help <command>.\n" +
                           "По умолчанию показывает список доступных команд.\n" +
                           "При указании аргумента <command> выводит подробное описание команды.\n";

            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void HelpCommandForCommitsCommand()
        {
            _commandsExecutor.RegisterCommand(new CommitsCommand(new ConfigStub(null), new CommitsGetterStub(null)));

            _commandsExecutor.Execute(new[] {"help", "commits"});

            var reader = new StringReader(_writer.ToString());

            var expected = "Описание команды commits:\r\n"
                           + "- " + "commits <h> <d>.\n" +
                           "Показывает список коммитов в хронологическом порядке.\n" +
                           "По умолчанию выводятся коммиты за последний день.\n" +
                           "Это можно изменить, передав аргументы <h> и/или <d>, " +
                           "тогда список будет состоять из коммитов за последние <h> часов и <d> дней.\n";

            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void HelpCommandForProjectsCommand()
        {
            _commandsExecutor.RegisterCommand(new ProjectsCommand(null));

            _commandsExecutor.Execute(new[] {"help", "projects"});

            var reader = new StringReader(_writer.ToString());

            var expected = "Описание команды projects:\r\n"
                           + "- " +
                           "Показывает список отслеживаемых проектов (находится в конфиге appsettings.json)\n";

            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void HelpCommandForUnknownCommand()
        {
            _commandsExecutor.Execute(new[] {"help", "unknown"});

            var reader = new StringReader(_writer.ToString());

            var expected = "Команда unknown не существует\r\n";

            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }
    }
}