using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GitlabTask.Interfaces;

namespace GitlabTask.Commands
{
    public class CommitsCommand : Command
    {
        private readonly IConfig _config;
        private readonly ICommitsGetter _commitsGetter;

        public CommitsCommand(IConfig config, ICommitsGetter commitsGetter)
            : base("commits", "commits <h> <d>.\n" +
                              "Показывает список коммитов в хронологическом порядке.\n" +
                              "По умолчанию выводятся коммиты за последний день.\n" +
                              "Это можно изменить, передав аргументы <h> и/или <d>, " +
                              "тогда список будет состоять из коммитов за последние <h> часов и <d> дней.\n")
        {
            _config = config;
            _commitsGetter = commitsGetter;
        }

        public override async Task Execute(string[] args, TextWriter writer)
        {
            var sinceTimestamp = GetTimestampAffectedByArguments(args);
            await WriteProjectsToWriter(writer, sinceTimestamp);
        }
        
        private static DateTimeOffset GetTimestampAffectedByArguments(string[] args)
        {
            var now = DateTimeOffset.Now;

            switch (args.Length)
            {
                case 1:
                {
                    var hours = int.Parse(args[0]);
                    return now.AddHours(hours * -1);
                }
                case 2:
                {
                    var days = int.Parse(args[1]);
                    return now.AddDays(days * -1);
                }
                default:
                {
                    var yesterday = now.AddDays(-1);
                    return yesterday;
                }
            }
        }
        
        private async Task WriteProjectsToWriter(TextWriter writer, DateTimeOffset sinceTimestamp)
        {
            var projectsFromConfig = _config.GetProjects();

            foreach (var project in projectsFromConfig)
            {
                var commits = await _commitsGetter.GetCommitsOfProject(project.Id, sinceTimestamp);
                var commitsList = commits.ToList();
                commitsList.Sort((c1, c2) => string.CompareOrdinal(c1.CreatedAt, c2.CreatedAt));

                project.Commits = commitsList.ToArray();

                await writer.WriteLineAsync(project + "\r\n");
            }
        }
    }
}