using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitlabTask.Interfaces
{
    public interface ICommandsExecutor
    {
        public Task Execute(string commandName, Dictionary<string, string> args);
    }
}