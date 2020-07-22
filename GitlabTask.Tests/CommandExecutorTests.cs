using System;
using System.Collections.Generic;
using System.IO;
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
            _commandsExecutor.RegisterCommand(new HelpCommand(_commandsExecutor.GetRegisteredCommandNames));
        }

        [Test]
        public void CommitsCommandWithoutArguments()
        {
            var projectNamesFromConfig = new[] {"TaxSee Workstation", "PriceService"};
            var taxSeeCommits = new[]
            {
                new Commit("Удален отладочный код", "mmm", "aaa", "324"),
                new Commit("Проверка подозрительных заказов", "mmm", "aaa", "324"),
                new Commit("Добавлен флажок \"Клиент просит не звонить\"", "mmm", "aaa", "324"),
            };
            var priceServiceCommits = new[]
            {
                new Commit("Исправление ошибок округления", "mmm", "aaa", "324"),
                new Commit("Рефакторинг обращений к графхоперу, реализация недостающих параметров", "mmm", "aaa",
                    "324"),
                new Commit("Фикс ошибки если адресный сервис не возвращает zoneId по координатам", "mmm", "aaa", "324"),
            };
            var projectIdToCommits = new Dictionary<string, Commit[]>
            {
                {projectNamesFromConfig[0], taxSeeCommits},
                {projectNamesFromConfig[1], priceServiceCommits}
            };
            _commandsExecutor.RegisterCommand(new CommitsCommand(
                new FakeConfig(projectNamesFromConfig),
                new FakeCommitsGetter(projectIdToCommits)));
            _commandsExecutor.Execute(new[] {"commits"});

            var reader = new StringReader(_writer.ToString());

            const string expected = "TaxSee Workstation:\r\n" +
                                    "- Удален отладочный код\r\n" +
                                    "- Проверка подозрительных заказов\r\n" +
                                    "- Добавлен флажок \"Клиент просит не звонить\"\r\n" +
                                    "\r\n" +
                                    "PriceService:\r\n" +
                                    "- Исправление ошибок округления\r\n" +
                                    "- Рефакторинг обращений к графхоперу, реализация недостающих параметров\r\n" +
                                    "- Фикс ошибки если адресный сервис не возвращает zoneId по координатам\r\n" +
                                    "\r\n";
            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
            Console.WriteLine(actual);
        }
    }
}