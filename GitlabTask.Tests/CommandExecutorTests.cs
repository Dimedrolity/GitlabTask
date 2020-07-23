using System.IO;
using GitlabTask.Commands;
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
        public void CommitsCommandWithoutArguments()
        {
            var taxSeeProject = new GitlabProject("TaxSee Workstation", "1");
            var taxSeeCommits = new[]
            {
                new GitlabCommit("Удален отладочный код", "mmm", "aaa", "324"),
                new GitlabCommit("Проверка подозрительных заказов", "mmm", "aaa", "324"),
                new GitlabCommit("Добавлен флажок \"Клиент просит не звонить\"", "mmm", "aaa", "324"),
            };
            taxSeeProject.Commits = taxSeeCommits;

            var priceServiceProject = new GitlabProject("PriceService", "2");
            var priceServiceCommits = new[]
            {
                new GitlabCommit("Исправление ошибок округления", "mmm", "aaa", "324"),
                new GitlabCommit("Рефакторинг обращений к графхоперу", "mmm", "aaa", "324"),
                new GitlabCommit("Фикс ошибки если адресный сервис не возвращает zoneId по координатам", "mmm", "aaa",
                    "324"),
            };
            priceServiceProject.Commits = priceServiceCommits;

            var projectsFromConfig = new[] {taxSeeProject, priceServiceProject};

            _commandsExecutor.RegisterCommand(new CommitsCommand(
                new FakeConfig(projectsFromConfig),
                new FakeCommitsGetter(projectsFromConfig)));
            _commandsExecutor.Execute(new[] {"commits"});

            var reader = new StringReader(_writer.ToString());

            const string expected = "TaxSee Workstation:\r\n" +
                                    "- Удален отладочный код\r\n" +
                                    "- Проверка подозрительных заказов\r\n" +
                                    "- Добавлен флажок \"Клиент просит не звонить\"\r\n" +
                                    "\r\n" +
                                    "PriceService:\r\n" +
                                    "- Исправление ошибок округления\r\n" +
                                    "- Рефакторинг обращений к графхоперу\r\n" +
                                    "- Фикс ошибки если адресный сервис не возвращает zoneId по координатам\r\n" +
                                    "\r\n";
            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }
    }
}