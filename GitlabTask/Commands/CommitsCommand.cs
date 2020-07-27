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
        private readonly ICommitsGetter _commitsGetter;

        private readonly IEnumerable<GitlabProject> _projectsFromConfig;
        private readonly IEnumerable<string> _patternsOfExcludedTitle;

        public CommitsCommand(IConfig config, ICommitsGetter commitsGetter)
            : base("commits", "Показывает список коммитов в хронологическом порядке.\n" +
                              "По умолчанию выводятся коммиты за последний день.\n" +
                              "Это можно изменить, передав аргументы h:<число> и/или d:<число>, " +
                              "тогда список будет состоять из коммитов за последние h часов и d дней.\n" +
                              "Например, command:commits h:3 d:1, коммиты за последние 1 день и 3 часа")
        {
            _commitsGetter = commitsGetter;

            _projectsFromConfig = config.GetProjects();
            _patternsOfExcludedTitle = config.GetPatternsOfExcludedTitle();
        }

        public override async Task Execute(Dictionary<string, string> args, TextWriter writer)
        {
            var sinceTimestamp = GetTimestampAffectedByArguments(args);
            await WriteProjectsToWriter(writer, sinceTimestamp);
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

        private async Task WriteProjectsToWriter(TextWriter writer, DateTimeOffset sinceTimestamp)
        {
            foreach (var project in _projectsFromConfig)
            {
                var commits = await _commitsGetter.GetCommitsOfProject(project.Id, sinceTimestamp);
                var commitsList = FilterCommitsByPatterns(commits).ToList();
                commitsList.Sort((c1, c2) => string.CompareOrdinal(c1.CreatedAt, c2.CreatedAt));
                project.Commits = commitsList.ToArray();

                await writer.WriteLineAsync(project + "\r\n");
            }
        }

        private IEnumerable<GitlabCommit> FilterCommitsByPatterns(IEnumerable<GitlabCommit> commits)
        {
            return commits.Where(commit =>
                _patternsOfExcludedTitle.All(pattern =>
                    !Regex.IsMatch(commit.Title, pattern, RegexOptions.IgnoreCase)));
        }
    }
}