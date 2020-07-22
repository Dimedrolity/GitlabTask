using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            var projectNamesFromConfig = _config.GetProjectNames();

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

            var detailed = args.Length > 2;

            foreach (var projectName in projectNamesFromConfig)
            {
                var commits = await _commitsGetter.GetCommitsOfProject(projectName, date);
                var rep = new Repository(projectName, commits);

                var commitsList = commits.ToList();
                commitsList.Sort((c1, c2) => string.CompareOrdinal(c1.CreatedAt, c2.CreatedAt));

                //TODO if detailed выводить - время коммитов, автора, полный текст коммитов.
                await writer.WriteLineAsync($"{rep.Name}:" +
                                            "\r\n- " + string.Join("\r\n- ", commitsList.Select(c => c.Title)));
                await writer.WriteLineAsync();
            }
        }
    }
}