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

            _patternsOfExcludedTitle = config.GetPatternsOfExcludedTitle();
        }

        public override async Task Execute(Dictionary<string, string> args, TextWriter writer)
        {
            var projects = _config.GetProjects().ToArray();
            var sinceTimestamp = GetTimestampAffectedByArguments(args);

            if (args != null && args.ContainsKey("branches"))
            {
                var branchesFromCommandLine = args["branches"];
                await GetBranchesViaCommandLineAndSetToProjectBranches(projects, branchesFromCommandLine);
            }
            else
            {
                await GetBranchesViaConfigAndSetToProjectBranches(projects);
            }

            await GetCommitsOfProjectBranchesAndSetToBranchCommits(projects, sinceTimestamp);
            await WriteProjectsToWriter(projects, writer);
        }

        private async Task GetBranchesViaCommandLineAndSetToProjectBranches(IEnumerable<GitlabProject> projects, string branchesArgument)
        {
            if (branchesArgument == "all")
            {
                foreach (var project in projects)
                {
                    var branches = await _branchesGetter.GetBranchesOfProject(project.Id);
                    project.Branches = branches.ToArray();
                }
            }
            else
            {
                var branches = ConvertStringToBranches(branchesArgument).ToArray();

                foreach (var project in projects)
                {
                    project.Branches = branches;
                }
            }
        }

        private static IEnumerable<GitlabBranch> ConvertStringToBranches(string branchesArgument)
        {
            return branchesArgument.Split(',')
                .Select(branchName => new GitlabBranch(branchName));
        }

        private async Task GetBranchesViaConfigAndSetToProjectBranches(IEnumerable<GitlabProject> projects)
        {
            foreach (var project in projects)
            {
                var branchesStringFromConfig = _config.GetBranchesOfProjectAsString(project.Id);
            
                project.Branches = (branchesStringFromConfig == "all"
                        ? await _branchesGetter.GetBranchesOfProject(project.Id)
                        : ConvertStringToBranches(branchesStringFromConfig))
                    .ToArray();
            }
        }

        private async Task GetCommitsOfProjectBranchesAndSetToBranchCommits(IEnumerable<GitlabProject> projects,
            DateTimeOffset sinceTimestamp)
        {
            foreach (var project in projects)
            foreach (var branch in project.Branches)
            {
                var commits = await _commitsGetter.GetCommitsOfProject(project.Id, branch.Name, sinceTimestamp);
                var filteredCommits = FilterCommitsByPatterns(commits).ToArray();
                Array.Sort(filteredCommits);
                branch.Commits = filteredCommits;
            }
        }

        private async Task WriteProjectsToWriter(IEnumerable<GitlabProject> projects, TextWriter writer)
        {
            foreach (var project in projects)
            {
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