using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GitlabTask.Interfaces;

namespace GitlabTask
{
    public class CommitsCommand
    {
        private readonly IConfig _config;
        private readonly ICommitsGetter _commitsGetter;
        private readonly IBranchesGetter _branchesGetter;
        private readonly IEnumerable<string> _patternsOfExcludedTitle;

        public CommitsCommand(IConfig config, ICommitsGetter commitsGetter, IBranchesGetter branchesGetter)
        {
            _config = config;
            _commitsGetter = commitsGetter;
            _branchesGetter = branchesGetter;

            _patternsOfExcludedTitle = config.GetPatternsOfExcludedTitle();
        }

        public async Task Execute(int hours, int days, string branches, TextWriter writer)
        {
            var projects = _config.GetProjects().ToArray();
            var sinceTimestamp = GetTimestampAffectedByArguments(hours, days);

            if (branches != null)
            {
                await GetBranchesViaCommandLineAndSetToProjectBranches(projects, branches);
            }
            else
            {
                await GetBranchesViaConfigAndSetToProjectBranches(projects);
            }

            await GetCommitsOfProjectBranchesAndSetToBranchCommits(projects, sinceTimestamp);
            await WriteProjectsToWriter(projects, writer);
        }

        private async Task GetBranchesViaCommandLineAndSetToProjectBranches(IEnumerable<GitlabProject> projects,
            string branchesArgument)
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
                await writer.WriteLineAsync(project.ToString());
            }
        }

        private static DateTimeOffset GetTimestampAffectedByArguments(int hours, int days)
        {
            return DateTimeOffset.Now.AddHours(hours * -1).AddDays(days * -1);
        }

        private IEnumerable<GitlabCommit> FilterCommitsByPatterns(IEnumerable<GitlabCommit> commits)
        {
            return commits.Where(commit =>
                _patternsOfExcludedTitle.All(pattern =>
                    !Regex.IsMatch(commit.Title, pattern, RegexOptions.IgnoreCase)));
        }
    }
}