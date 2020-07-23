using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GitlabTask.Interfaces;

namespace GitlabTask
{
    public class CommitsCommand : Command
    {
        private readonly IConfig _config;
        private readonly ICommitsGetter _commitsGetter;

        public CommitsCommand(IConfig config, ICommitsGetter commitsGetter)
            : base("commits", "commits <h> <d> <detailed>.\n" +
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
            var projectsFromConfig = _config.GetProjects();

            var date = DateTimeOffset.Now;

            if (args.Length == 0)
            {
                var yesterday = DateTimeOffset.Now.AddDays(-1);
                date = yesterday; //default
            }

            if (args.Length > 0)
            {
                var hours = int.Parse(args[0]);
                date = date.AddHours(hours * -1);
            }

            if (args.Length > 1)
            {
                var days = int.Parse(args[1]);
                date = date.AddDays(days * -1);
            }

            foreach (var project in projectsFromConfig)
            {
                var commits = await _commitsGetter.GetCommitsOfProject(project.Id, date);
                var commitsList = commits.ToList();
                commitsList.Sort((c1, c2) => string.CompareOrdinal(c1.CreatedAt, c2.CreatedAt));

                project.Commits = commitsList.ToArray();

                await writer.WriteLineAsync(project.ToString());
                await writer.WriteLineAsync();
            }
        }
    }
}