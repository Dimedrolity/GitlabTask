using System.Threading.Tasks;

namespace GitlabTask.Interfaces
{
    public interface ICommandsExecutor
    {
        public Task Execute(string[] args);
    }
}