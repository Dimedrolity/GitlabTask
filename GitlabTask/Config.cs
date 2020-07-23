using System.Linq;
using GitlabTask.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GitlabTask
{
    public class Config : IConfig
    {
        private readonly IConfiguration _configuration;

        public Config()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();
        }

        public GitlabProject[] GetProjects()
        {
            return _configuration.GetSection("projects").GetChildren()
                .Select(projectFromJson => new GitlabProject(projectFromJson["name"], projectFromJson["projectId"]))
                .ToArray();
        }
    }
}