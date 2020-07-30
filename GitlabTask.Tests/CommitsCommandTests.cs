using System.IO;
using NUnit.Framework;

namespace GitlabTask.Tests
{
    public class CommitsCommandTests
    {
        private StringWriter _writer;

        [SetUp]
        public void Setup()
        {
            _writer = new StringWriter();
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

            var cmd = new CommitsCommand(
                new ConfigStub(new[] {apnsProject}),
                new CommitsGetterStub(apnsCommits),
                new BranchesGetterStub(branches));
            cmd.Execute(0, 1, null, _writer);

            var reader = new StringReader(_writer.ToString());

            var expected = "APNS:\r\n\r\n" +
                           $" {branches[0].Name}:\r\n" +
                           " - Добавлены классы уведомлений\r\n" +
                           " - Добавлены запросы в APNS с использованием Http2\r\n" +
                           " - Добавлен генератор токенов\r\n" +
                           $" {branches[1].Name}:\r\n" +
                           " - Добавлены классы уведомлений\r\n" +
                           " - Добавлены запросы в APNS с использованием Http2\r\n" +
                           " - Добавлен генератор токенов\r\n" +
                           "\r\n\r\n";
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

            var cmd = new CommitsCommand(
                new ConfigStub(new[] {apnsProject}),
                new CommitsGetterStub(apnsCommits),
                new BranchesGetterStub(branches));
            cmd.Execute(0, 1, null, _writer);

            var reader = new StringReader(_writer.ToString());

            var expected = "APNS:\r\n\r\n" +
                           $" {branches[0].Name}:\r\n" +
                           " - Добавлены классы уведомлений\r\n" +
                           " - Добавлены запросы в APNS с использованием Http2\r\n" +
                           " - Добавлен генератор токенов\r\n" +
                           $" {branches[1].Name}:\r\n" +
                           " - Добавлены классы уведомлений\r\n" +
                           " - Добавлены запросы в APNS с использованием Http2\r\n" +
                           " - Добавлен генератор токенов\r\n" +
                           "\r\n\r\n";
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

            var cmd = new CommitsCommand(
                new ConfigStub(new[] {apnsProject}),
                new CommitsGetterStub(apnsCommits),
                new BranchesGetterStub(branches));
            cmd.Execute(0, 1, "all", _writer);

            var reader = new StringReader(_writer.ToString());

            var expected = "APNS:\r\n\r\n" +
                           $" {branches[0].Name}:\r\n" +
                           " - Добавлены классы уведомлений\r\n" +
                           " - Добавлены запросы в APNS с использованием Http2\r\n" +
                           " - Добавлен генератор токенов\r\n" +
                           $" {branches[1].Name}:\r\n" +
                           " - Добавлены классы уведомлений\r\n" +
                           " - Добавлены запросы в APNS с использованием Http2\r\n" +
                           " - Добавлен генератор токенов\r\n" +
                           $" {branches[2].Name}:\r\n" +
                           " - Добавлены классы уведомлений\r\n" +
                           " - Добавлены запросы в APNS с использованием Http2\r\n" +
                           " - Добавлен генератор токенов\r\n" +
                           "\r\n\r\n";
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

            var cmd = new CommitsCommand(
                new ConfigStub(new[] {apnsProject}),
                new CommitsGetterStub(apnsCommits),
                new BranchesGetterStub(branches));
            cmd.Execute(0, 1, "qwe,asd", _writer);

            var reader = new StringReader(_writer.ToString());

            var expected = "APNS:\r\n\r\n" +
                           $" {branches[0].Name}:\r\n" +
                           " - Добавлены классы уведомлений\r\n" +
                           " - Добавлены запросы в APNS с использованием Http2\r\n" +
                           " - Добавлен генератор токенов\r\n" +
                           $" {branches[1].Name}:\r\n" +
                           " - Добавлены классы уведомлений\r\n" +
                           " - Добавлены запросы в APNS с использованием Http2\r\n" +
                           " - Добавлен генератор токенов\r\n" +
                           "\r\n\r\n";
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

            var cmd = new CommitsCommand(
                new ConfigStub(new[] {apnsProject}),
                new CommitsGetterStub(apnsCommits),
                new BranchesGetterStub(branches));
            cmd.Execute(1000 * 24, 0, null, _writer);

            var reader = new StringReader(_writer.ToString());

            var expected = "APNS:\r\n\r\n" +
                           $" {branches[0].Name}:\r\n" +
                           " - Добавлены классы уведомлений\r\n" +
                           " - Добавлены запросы в APNS с использованием Http2\r\n" +
                           " - Добавлен генератор токенов\r\n" +
                           $" {branches[1].Name}:\r\n" +
                           " - Добавлены классы уведомлений\r\n" +
                           " - Добавлены запросы в APNS с использованием Http2\r\n" +
                           " - Добавлен генератор токенов\r\n" +
                           "\r\n\r\n";
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


            var cmd = new CommitsCommand(
                new ConfigStub(new[] {apnsProject}),
                new CommitsGetterStub(apnsCommits),
                new BranchesGetterStub(branches));
            cmd.Execute(0, 1000, null, _writer);

            var reader = new StringReader(_writer.ToString());

            var expected = "APNS:\r\n\r\n" +
                           $" {branches[0].Name}:\r\n" +
                           " - Добавлены классы уведомлений\r\n" +
                           " - Добавлены запросы в APNS с использованием Http2\r\n" +
                           " - Добавлен генератор токенов\r\n" +
                           $" {branches[1].Name}:\r\n" +
                           " - Добавлены классы уведомлений\r\n" +
                           " - Добавлены запросы в APNS с использованием Http2\r\n" +
                           " - Добавлен генератор токенов\r\n" +
                           "\r\n\r\n";
            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CommitsCommand_ProjectsWithoutBranches()
        {
            var apnsProject = new GitlabProject("APNS", "allBranches");
            var branches = new GitlabBranch[0];

            var cmd = new CommitsCommand(
                new ConfigStub(new[] {apnsProject}),
                null,
                new BranchesGetterStub(branches));
            cmd.Execute(0, 1, null, _writer);

            var reader = new StringReader(_writer.ToString());

            var expected = "APNS:\r\n\r\n\r\n\r\n\r\n";
            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }
    }
}