using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GitlabTask.Interfaces;

namespace GitlabTask.Commands
{
    public class ProjectsCommand : Command
    {
        private readonly IConfig _config;

        public ProjectsCommand(IConfig config)
            : base("projects", "Показывает список отслеживаемых проектов (находится в конфиге appsettings.json)\n")
        {
            _config = config;
        }

        public override async Task Execute(string[] args, TextWriter writer)
        {
            var projectNames = _config.GetProjects().Select(project => project.Name);
            await writer.WriteAsync("Список отслеживаемых проектов (находится в конфиге appsettings.json):\r\n" +
                                    $"- {string.Join("\r\n- ", projectNames)}\r\n");
        }
    }
}