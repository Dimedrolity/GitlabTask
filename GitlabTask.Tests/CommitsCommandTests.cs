using System;
using System.IO;
using FakeItEasy;
using GitlabTask.Interfaces;
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
            var project = new GitlabProject("APNS", "nevermind");
            var configStub = A.Fake<IConfig>();
            A.CallTo(() => configStub.GetProjects()).Returns(new[] {project});
            A.CallTo(() => configStub.GetBranchesOfProjectAsString(A<string>._)).Returns("all");

            var commits = new[]
            {
                new GitlabCommit("Добавлен генератор токенов", "2020-07-23T16:04:00Z"),
                new GitlabCommit("Добавлены запросы в APNS с использованием Http2", "2020-07-22T16:04:00Z"),
                new GitlabCommit("Добавлены классы уведомлений", "2020-07-21T16:04:00Z"),
            };
            var commitsGetterStub = A.Fake<ICommitsGetter>();
            A.CallTo(() => commitsGetterStub.GetCommitsOfProject(A<string>._, A<string>._, A<DateTimeOffset>._))
                .Returns(commits);

            var branches = new[]
            {
                new GitlabBranch("qwe"),
                new GitlabBranch("asd"),
            };
            var branchesGetterStub = A.Fake<IBranchesGetter>();
            A.CallTo(() => branchesGetterStub.GetBranchesOfProject(A<string>._)).Returns(branches);

            var cmd = new CommitsCommand(
                configStub,
                commitsGetterStub,
                branchesGetterStub);
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
            var project = new GitlabProject("APNS", "nevermind");
            var config = A.Fake<IConfig>();
            A.CallTo(() => config.GetProjects()).Returns(new[] {project});
            A.CallTo(() => config.GetBranchesOfProjectAsString(A<string>._)).Returns("qwe,asd");

            var commits = new[]
            {
                new GitlabCommit("Добавлен генератор токенов", "2020-07-23T16:04:00Z"),
                new GitlabCommit("Добавлены запросы в APNS с использованием Http2", "2020-07-22T16:04:00Z"),
                new GitlabCommit("Добавлены классы уведомлений", "2020-07-21T16:04:00Z"),
            };
            var commitsGetterStub = A.Fake<ICommitsGetter>();
            A.CallTo(() => commitsGetterStub.GetCommitsOfProject(A<string>._, A<string>._, A<DateTimeOffset>._))
                .Returns(commits);

            var branches = new[]
            {
                new GitlabBranch("qwe"),
                new GitlabBranch("asd"),
                new GitlabBranch("zxc"),
            };
            var branchesGetterStub = A.Fake<IBranchesGetter>();
            A.CallTo(() => branchesGetterStub.GetBranchesOfProject(A<string>._)).Returns(branches);

            var cmd = new CommitsCommand(
                config,
                commitsGetterStub,
                branchesGetterStub);
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
            var project = new GitlabProject("APNS", "nevermind");
            var config = A.Fake<IConfig>();
            A.CallTo(() => config.GetProjects()).Returns(new[] {project});

            var commits = new[]
            {
                new GitlabCommit("Добавлен генератор токенов", "2020-07-23T16:04:00Z"),
                new GitlabCommit("Добавлены запросы в APNS с использованием Http2", "2020-07-22T16:04:00Z"),
                new GitlabCommit("Добавлены классы уведомлений", "2020-07-21T16:04:00Z"),
            };
            var commitsGetterStub = A.Fake<ICommitsGetter>();
            A.CallTo(() => commitsGetterStub.GetCommitsOfProject(A<string>._, A<string>._, A<DateTimeOffset>._))
                .Returns(commits);

            var branches = new[]
            {
                new GitlabBranch("qwe"),
                new GitlabBranch("asd"),
                new GitlabBranch("zxc"),
            };
            var branchesGetterStub = A.Fake<IBranchesGetter>();
            A.CallTo(() => branchesGetterStub.GetBranchesOfProject(A<string>._)).Returns(branches);

            var cmd = new CommitsCommand(
                config,
                commitsGetterStub,
                branchesGetterStub);
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
            var project = new GitlabProject("APNS", "nevermind");
            var config = A.Fake<IConfig>();
            A.CallTo(() => config.GetProjects()).Returns(new[] {project});

            var commits = new[]
            {
                new GitlabCommit("Добавлен генератор токенов", "2020-07-23T16:04:00Z"),
                new GitlabCommit("Добавлены запросы в APNS с использованием Http2", "2020-07-22T16:04:00Z"),
                new GitlabCommit("Добавлены классы уведомлений", "2020-07-21T16:04:00Z"),
            };
            var commitsGetterStub = A.Fake<ICommitsGetter>();
            A.CallTo(() => commitsGetterStub.GetCommitsOfProject(A<string>._, A<string>._, A<DateTimeOffset>._))
                .Returns(commits);

            var branches = new[]
            {
                new GitlabBranch("qwe"),
                new GitlabBranch("asd"),
                new GitlabBranch("zxc"),
            };
            var branchesGetterStub = A.Fake<IBranchesGetter>();
            A.CallTo(() => branchesGetterStub.GetBranchesOfProject(A<string>._)).Returns(branches);

            var cmd = new CommitsCommand(
                config,
                commitsGetterStub,
                branchesGetterStub);
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
            var project = new GitlabProject("APNS", "nevermind");
            var config = A.Fake<IConfig>();
            A.CallTo(() => config.GetProjects()).Returns(new[] {project});
            A.CallTo(() => config.GetBranchesOfProjectAsString(A<string>._)).Returns("all");

            var commits = new[]
            {
                new GitlabCommit("Добавлен генератор токенов", "2020-07-23T16:04:00Z"),
                new GitlabCommit("Добавлены запросы в APNS с использованием Http2", "2020-07-22T16:04:00Z"),
                new GitlabCommit("Добавлены классы уведомлений", "2020-07-21T16:04:00Z"),
            };
            var commitsGetterStub = A.Fake<ICommitsGetter>();
            A.CallTo(() => commitsGetterStub.GetCommitsOfProject(A<string>._, A<string>._, A<DateTimeOffset>._))
                .Returns(commits);

            var branches = new[]
            {
                new GitlabBranch("qwe"),
                new GitlabBranch("asd"),
            };
            var branchesGetterStub = A.Fake<IBranchesGetter>();
            A.CallTo(() => branchesGetterStub.GetBranchesOfProject(A<string>._)).Returns(branches);

            var cmd = new CommitsCommand(
                config,
                commitsGetterStub,
                branchesGetterStub);
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
            var project = new GitlabProject("APNS", "nevermind");
            var config = A.Fake<IConfig>();
            A.CallTo(() => config.GetProjects()).Returns(new[] {project});
            A.CallTo(() => config.GetBranchesOfProjectAsString(A<string>._)).Returns("all");

            var commits = new[]
            {
                new GitlabCommit("Добавлен генератор токенов", "2020-07-23T16:04:00Z"),
                new GitlabCommit("Добавлены запросы в APNS с использованием Http2", "2020-07-22T16:04:00Z"),
                new GitlabCommit("Добавлены классы уведомлений", "2020-07-21T16:04:00Z"),
            };
            var commitsGetterStub = A.Fake<ICommitsGetter>();
            A.CallTo(() => commitsGetterStub.GetCommitsOfProject(A<string>._, A<string>._, A<DateTimeOffset>._))
                .Returns(commits);

            var branches = new[]
            {
                new GitlabBranch("qwe"),
                new GitlabBranch("asd"),
            };
            var branchesGetterStub = A.Fake<IBranchesGetter>();
            A.CallTo(() => branchesGetterStub.GetBranchesOfProject(A<string>._)).Returns(branches);

            var cmd = new CommitsCommand(
                config,
                commitsGetterStub,
                branchesGetterStub);
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
            var project = new GitlabProject("APNS", "nevermind");
            var config = A.Fake<IConfig>();
            A.CallTo(() => config.GetProjects()).Returns(new[] {project});
            A.CallTo(() => config.GetBranchesOfProjectAsString(A<string>._)).Returns("all");

            var branches = new GitlabBranch[0];
            var branchesGetterStub = A.Fake<IBranchesGetter>();
            A.CallTo(() => branchesGetterStub.GetBranchesOfProject(A<string>._)).Returns(branches);

            var cmd = new CommitsCommand(
                config,
                null,
                branchesGetterStub);
            cmd.Execute(0, 1, null, _writer);

            var reader = new StringReader(_writer.ToString());

            var expected = "APNS:\r\n\r\n\r\n\r\n\r\n";
            var actual = reader.ReadToEnd();
            Assert.AreEqual(expected, actual);
        }
    }
}