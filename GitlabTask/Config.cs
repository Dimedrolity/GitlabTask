using System;
using GitlabTask.Interfaces;

namespace GitlabTask
{
    public class Config : IConfig
    {
        public GitlabProject[] GetProjects()
        {
            //TODO брать из настоящего конфига
            throw new NotImplementedException();
        }
    }
}