using System.Collections.Generic;
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
        public void CommitsCommand_AllBranchesFromConfig()
        {
            var apnsProject = new GitlabProject("APNS", "all_branches");
            var branches = new[]
            {
                new GitlabBranch("qwe"),
                new GitlabBranch("asd"),
            };
            var apnsCommits = new[]
            {
                new GitlabCommit("Добавлен генератор токенов", "2020-07-23T16:04:00Z"),
                new GitlabCommit("Добавлены запросы в APNS с использованием Http2", "2020-07-22T16:04:00Z"),
                new GitlabCommit("Добавлены классы уведомлений", "2020-07-21T16:04:00Z"),
            };

            _commandsExecutor.RegisterCommand(new CommitsCommand(
                new ConfigStub(new[] {apnsProject}),
                new CommitsGetterStub(apnsCommits),
                new BranchesGetterStub(branches)));
            _commandsExecutor.Execute("commits");

            var reader = new StringReader(_writer.ToString());

            var expected = "APNS:\r\n" +
                           $"- branch {branches[0].Name}:\r\n" +
                           "-- Добавлены классы уведомлений\r\n" +
                           "-- Добавлены запросы в APNS с использованием Http2\r\n" +
                           "-- Добавлен генератор токенов\r\n" +
                           $"- branch {branches[1].Name}:\r\n" +
                           "-- Добавлены классы уведомлений\r\n" +
                           "-- Добавлены запросы в APNS с использованием Http2\r\n" +
                           "-- Добавлен генератор токенов\r\n" +
                           "\r\n";
            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CommitsCommand_TwoBranchesFromConfig()
        {
            var apnsProject = new GitlabProject("APNS", "qwe_and_asd");
            var branches = new[]
            {
                new GitlabBranch("qwe"),
                new GitlabBranch("asd"),
                new GitlabBranch("zxc"),
            };
            var apnsCommits = new[]
            {
                new GitlabCommit("Добавлен генератор токенов", "2020-07-23T16:04:00Z"),
                new GitlabCommit("Добавлены запросы в APNS с использованием Http2", "2020-07-22T16:04:00Z"),
                new GitlabCommit("Добавлены классы уведомлений", "2020-07-21T16:04:00Z"),
            };

            _commandsExecutor.RegisterCommand(new CommitsCommand(
                new ConfigStub(new[] {apnsProject}),
                new CommitsGetterStub(apnsCommits),
                new BranchesGetterStub(branches)));
            _commandsExecutor.Execute("commits");

            var reader = new StringReader(_writer.ToString());

            var expected = "APNS:\r\n" +
                           $"- branch {branches[0].Name}:\r\n" +
                           "-- Добавлены классы уведомлений\r\n" +
                           "-- Добавлены запросы в APNS с использованием Http2\r\n" +
                           "-- Добавлен генератор токенов\r\n" +
                           $"- branch {branches[1].Name}:\r\n" +
                           "-- Добавлены классы уведомлений\r\n" +
                           "-- Добавлены запросы в APNS с использованием Http2\r\n" +
                           "-- Добавлен генератор токенов\r\n" +
                           "\r\n";
            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CommitsCommand_AllBranchesFromArgs()
        {
            var apnsProject = new GitlabProject("APNS", "qwe_and_asd");
            var branches = new[]
            {
                new GitlabBranch("qwe"),
                new GitlabBranch("asd"),
                new GitlabBranch("zxc"),
            };
            var apnsCommits = new[]
            {
                new GitlabCommit("Добавлен генератор токенов", "2020-07-23T16:04:00Z"),
                new GitlabCommit("Добавлены запросы в APNS с использованием Http2", "2020-07-22T16:04:00Z"),
                new GitlabCommit("Добавлены классы уведомлений", "2020-07-21T16:04:00Z"),
            };

            _commandsExecutor.RegisterCommand(new CommitsCommand(
                new ConfigStub(new[] {apnsProject}),
                new CommitsGetterStub(apnsCommits),
                new BranchesGetterStub(branches)));
            _commandsExecutor.Execute("commits",
                new Dictionary<string, string>
                {
                    {"branches", "all"}
                });

            var reader = new StringReader(_writer.ToString());

            var expected = "APNS:\r\n" +
                           $"- branch {branches[0].Name}:\r\n" +
                           "-- Добавлены классы уведомлений\r\n" +
                           "-- Добавлены запросы в APNS с использованием Http2\r\n" +
                           "-- Добавлен генератор токенов\r\n" +
                           $"- branch {branches[1].Name}:\r\n" +
                           "-- Добавлены классы уведомлений\r\n" +
                           "-- Добавлены запросы в APNS с использованием Http2\r\n" +
                           "-- Добавлен генератор токенов\r\n" +
                           $"- branch {branches[2].Name}:\r\n" +
                           "-- Добавлены классы уведомлений\r\n" +
                           "-- Добавлены запросы в APNS с использованием Http2\r\n" +
                           "-- Добавлен генератор токенов\r\n" +
                           "\r\n";
            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CommitsCommand_TwoBranchesFromArgs()
        {
            var apnsProject = new GitlabProject("APNS", "allBranches");
            var branches = new[]
            {
                new GitlabBranch("qwe"),
                new GitlabBranch("asd"),
                new GitlabBranch("zxc"),
            };
            var apnsCommits = new[]
            {
                new GitlabCommit("Добавлен генератор токенов", "2020-07-23T16:04:00Z"),
                new GitlabCommit("Добавлены запросы в APNS с использованием Http2", "2020-07-22T16:04:00Z"),
                new GitlabCommit("Добавлены классы уведомлений", "2020-07-21T16:04:00Z"),
            };

            _commandsExecutor.RegisterCommand(new CommitsCommand(
                new ConfigStub(new[] {apnsProject}),
                new CommitsGetterStub(apnsCommits),
                new BranchesGetterStub(branches)));
            _commandsExecutor.Execute("commits",
                new Dictionary<string, string>
                {
                    {"branches", "qwe,asd"}
                });

            var reader = new StringReader(_writer.ToString());

            var expected = "APNS:\r\n" +
                           $"- branch {branches[0].Name}:\r\n" +
                           "-- Добавлены классы уведомлений\r\n" +
                           "-- Добавлены запросы в APNS с использованием Http2\r\n" +
                           "-- Добавлен генератор токенов\r\n" +
                           $"- branch {branches[1].Name}:\r\n" +
                           "-- Добавлены классы уведомлений\r\n" +
                           "-- Добавлены запросы в APNS с использованием Http2\r\n" +
                           "-- Добавлен генератор токенов\r\n" +
                           "\r\n";
            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CommitsCommand_WithHoursArgument()
        {
            var apnsProject = new GitlabProject("APNS", "allBranches");
            var branches = new[]
            {
                new GitlabBranch("qwe"),
                new GitlabBranch("asd"),
            };
            var apnsCommits = new[]
            {
                new GitlabCommit("Добавлен генератор токенов", "2020-07-23T16:04:00Z"),
                new GitlabCommit("Добавлены запросы в APNS с использованием Http2", "2020-07-22T16:04:00Z"),
                new GitlabCommit("Добавлены классы уведомлений", "2020-07-21T16:04:00Z"),
            };

            _commandsExecutor.RegisterCommand(new CommitsCommand(
                new ConfigStub(new[] {apnsProject}),
                new CommitsGetterStub(apnsCommits),
                new BranchesGetterStub(branches)));
            _commandsExecutor.Execute("commits",
                new Dictionary<string, string>
                {
                    {"h", (1000 * 24).ToString()}, //1000 дней
                });

            var reader = new StringReader(_writer.ToString());

            var expected = "APNS:\r\n" +
                           $"- branch {branches[0].Name}:\r\n" +
                           "-- Добавлены классы уведомлений\r\n" +
                           "-- Добавлены запросы в APNS с использованием Http2\r\n" +
                           "-- Добавлен генератор токенов\r\n" +
                           $"- branch {branches[1].Name}:\r\n" +
                           "-- Добавлены классы уведомлений\r\n" +
                           "-- Добавлены запросы в APNS с использованием Http2\r\n" +
                           "-- Добавлен генератор токенов\r\n" +
                           "\r\n";
            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CommitsCommand_WithDaysArgument()
        {
            var apnsProject = new GitlabProject("APNS", "allBranches");
            var branches = new[]
            {
                new GitlabBranch("qwe"),
                new GitlabBranch("asd"),
            };
            var apnsCommits = new[]
            {
                new GitlabCommit("Добавлен генератор токенов", "2020-07-23T16:04:00Z"),
                new GitlabCommit("Добавлены запросы в APNS с использованием Http2", "2020-07-22T16:04:00Z"),
                new GitlabCommit("Добавлены классы уведомлений", "2020-07-21T16:04:00Z"),
            };

            _commandsExecutor.RegisterCommand(new CommitsCommand(
                new ConfigStub(new[] {apnsProject}),
                new CommitsGetterStub(apnsCommits),
                new BranchesGetterStub(branches)));
            _commandsExecutor.Execute("commits",
                new Dictionary<string, string>
                {
                    {"d", (1000).ToString()}, //1000 дней
                });

            var reader = new StringReader(_writer.ToString());

            var expected = "APNS:\r\n" +
                           $"- branch {branches[0].Name}:\r\n" +
                           "-- Добавлены классы уведомлений\r\n" +
                           "-- Добавлены запросы в APNS с использованием Http2\r\n" +
                           "-- Добавлен генератор токенов\r\n" +
                           $"- branch {branches[1].Name}:\r\n" +
                           "-- Добавлены классы уведомлений\r\n" +
                           "-- Добавлены запросы в APNS с использованием Http2\r\n" +
                           "-- Добавлен генератор токенов\r\n" +
                           "\r\n";
            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CommitsCommand_ProjectsWithoutBranches()
        {
            var apnsProject = new GitlabProject("APNS", "allBranches");
            var branches = new GitlabBranch[0];

            _commandsExecutor.RegisterCommand(new CommitsCommand(
                new ConfigStub(new[] {apnsProject}),
                null,
                new BranchesGetterStub(branches)));
            _commandsExecutor.Execute("commits");

            var reader = new StringReader(_writer.ToString());

            var expected = "APNS:\r\n- \r\n\r\n";
            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }
    }
}