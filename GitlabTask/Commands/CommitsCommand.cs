using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GitlabTask.Interfaces;

namespace GitlabTask.Commands
{
    public class CommitsCommand : Command
    {
        private readonly IConfig _config;
        private readonly ICommitsGetter _commitsGetter;
        private readonly IBranchesGetter _branchesGetter;

        private readonly IEnumerable<GitlabProject> _projectsFromConfig;
        private readonly IEnumerable<string> _patternsOfExcludedTitle;

        public CommitsCommand(IConfig config, ICommitsGetter commitsGetter, IBranchesGetter branchesGetter)
            : base("commits", "Показывает список коммитов в хронологическом порядке.\n" +
                              "По умолчанию выводятся коммиты за последний день.\n" +
                              "Это можно изменить, передав аргументы h:<число> и/или d:<число>, " +
                              "тогда список будет состоять из коммитов за последние h часов и d дней.\n" +
                              "Например, command:commits h:3 d:1, коммиты за последние 1 день и 3 часа")
        {
            _config = config;
            _commitsGetter = commitsGetter;
            _branchesGetter = branchesGetter;

            _projectsFromConfig = config.GetProjects();
            _patternsOfExcludedTitle = config.GetPatternsOfExcludedTitle();
        }

        public override async Task Execute(Dictionary<string, string> args, TextWriter writer)
        {
            var sinceTimestamp = GetTimestampAffectedByArguments(args);

            if (args != null && args.ContainsKey("branches"))
            {
                var branchesArgument = args["branches"];
                if (branchesArgument == "all")
                {
                    await TakeCommitsFromAllBranchesSince(sinceTimestamp, writer);
                }
                else
                {
                    var branches = branchesArgument.Split(',')
                        .Select(bName => new GitlabBranch(bName)).ToArray();
                    await TakeCommitsOnlyFromBranches(branches, sinceTimestamp, writer);
                }
            }
            else
            {
                var projects = _projectsFromConfig.ToArray();
                foreach (var project in projects)
                {
                    var branchesStringFromConfig = _config.GetBranchesStringOfProject(project.Id);

                    if (branchesStringFromConfig == "all")
                    {
                        project.Branches = await _branchesGetter.GetBranchesOfProject(project.Id);
                    }
                    else
                    {
                        project.Branches = branchesStringFromConfig.Split(',')
                            .Select(bName => new GitlabBranch(bName));
                    }
                }

                await WriteProjectsToWriter(projects, writer, sinceTimestamp);
            }
        }

        private async Task TakeCommitsOnlyFromBranches(IEnumerable<GitlabBranch> branches,
            DateTimeOffset sinceTimestamp, TextWriter writer)
        {
            foreach (var project in _projectsFromConfig)
            {
                project.Branches = branches.ToArray();
                foreach (var branch in project.Branches)
                {
                    var commits = await _commitsGetter.GetCommitsOfProject(project.Id, branch.Name, sinceTimestamp);
                    var commitsList = FilterCommitsByPatterns(commits).ToList();
                    commitsList.Sort((c1, c2) => string.CompareOrdinal(c1.CreatedAt, c2.CreatedAt));
                    branch.Commits = commitsList;
                }

                await writer.WriteLineAsync(project + "\r\n");
            }
        }

        private async Task WriteProjectsToWriter(IEnumerable<GitlabProject> projects, TextWriter writer,
            DateTimeOffset sinceTimestamp)
        {
            foreach (var project in projects)
            {
                var branches = project.Branches.ToArray();
                foreach (var branch in branches)
                {
                    var commits = await _commitsGetter.GetCommitsOfProject(project.Id, branch.Name, sinceTimestamp);
                    var commitsList = FilterCommitsByPatterns(commits).ToList();
                    commitsList.Sort((c1, c2) => string.CompareOrdinal(c1.CreatedAt, c2.CreatedAt));
                    branch.Commits = commitsList;
                }

                project.Branches = branches;

                await writer.WriteLineAsync(project + "\r\n");
            }
        }

        private async Task TakeCommitsFromAllBranchesSince(DateTimeOffset sinceTimestamp, TextWriter writer)
        {
            foreach (var project in _projectsFromConfig)
            {
                var branches = await _branchesGetter.GetBranchesOfProject(project.Id);
                project.Branches = branches;
                foreach (var branch in project.Branches)
                {
                    var commits = await _commitsGetter.GetCommitsOfProject(project.Id, branch.Name, sinceTimestamp);
                    var commitsList = FilterCommitsByPatterns(commits).ToList();
                    commitsList.Sort((c1, c2) => string.CompareOrdinal(c1.CreatedAt, c2.CreatedAt));
                    branch.Commits = commitsList;
                }

                await writer.WriteLineAsync(project + "\r\n");
            }
        }

        private static DateTimeOffset GetTimestampAffectedByArguments(IReadOnlyDictionary<string, string> args)
        {
            var now = DateTimeOffset.Now;
            var date = now;

            if (args != null && (args.ContainsKey("h") || args.ContainsKey("d")))
            {
                if (args.ContainsKey("h"))
                {
                    var hours = int.Parse(args["h"]);
                    date = date.AddHours(hours * -1);
                }

                if (args.ContainsKey("d"))
                {
                    var days = int.Parse(args["d"]);
                    date = date.AddDays(days * -1);
                }

                return date;
            }

            var yesterday = now.AddDays(-1);
            return yesterday;
        }


        private IEnumerable<GitlabCommit> FilterCommitsByPatterns(IEnumerable<GitlabCommit> commits)
        {
            return commits.Where(commit =>
                _patternsOfExcludedTitle.All(pattern =>
                    !Regex.IsMatch(commit.Title, pattern, RegexOptions.IgnoreCase)));
        }
    }
}