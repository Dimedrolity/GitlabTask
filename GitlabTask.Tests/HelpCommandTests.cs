using System.Collections.Generic;
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
            _commandsExecutor.RegisterCommand(new CommitsCommand(new ConfigStub(null), new CommitsGetterStub(null),
                new BranchesGetterStub(null)));
            _commandsExecutor.RegisterCommand(new ProjectsCommand(new ConfigStub(null)));

            _commandsExecutor.Execute("help");

            var reader = new StringReader(_writer.ToString());

            var expected = "Список доступных команд:\r\n" +
                           "- command:help\r\n" +
                           "- command:commits\r\n" +
                           "- command:projects\r\n" +
                           "Более подробная информация -> 'command:help about:<command>'\n";

            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void HelpCommandForHelpCommand()
        {
            _commandsExecutor.Execute("help",
                new Dictionary<string, string> {{"about", "help"}});

            var reader = new StringReader(_writer.ToString());

            var expected = "Описание команды help:\r\n" +
                           "- " + "По умолчанию показывает список доступных команд.\n" +
                           "При указании аргумента 'about:<command>' выводит подробное описание команды.\n";

            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void HelpCommandForCommitsCommand()
        {
            _commandsExecutor.RegisterCommand(new CommitsCommand(new ConfigStub(null), new CommitsGetterStub(null),
                new BranchesGetterStub(null)));

            _commandsExecutor.Execute("help",
                new Dictionary<string, string> {{"about", "commits"}});

            var reader = new StringReader(_writer.ToString());

            var expected = "Описание команды commits:\r\n" +
                           "- " + "Показывает список коммитов в хронологическом порядке.\n" +
                           "По умолчанию выводятся коммиты за последний день.\n" +
                           "Это можно изменить, передав аргументы h:<число> и/или d:<число>, " +
                           "тогда список будет состоять из коммитов за последние h часов и d дней.\n" +
                           "Например, command:commits h:3 d:1, коммиты за последние 1 день и 3 часа";

            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void HelpCommandForProjectsCommand()
        {
            _commandsExecutor.RegisterCommand(new ProjectsCommand(null));

            _commandsExecutor.Execute("help",
                new Dictionary<string, string> {{"about", "projects"}});

            var reader = new StringReader(_writer.ToString());

            var expected = "Описание команды projects:\r\n" +
                           "- " + "Показывает список отслеживаемых проектов (находится в конфиге appsettings.json)\n";

            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void HelpCommandForUnknownCommand()
        {
            _commandsExecutor.Execute("help",
                new Dictionary<string, string> {{"about", "unknown"}});

            var reader = new StringReader(_writer.ToString());

            var expected = "Команда unknown не существует\r\n";

            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }
    }
}