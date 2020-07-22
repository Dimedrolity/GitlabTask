using System.Threading.Tasks;

namespace GitlabTask
{
    public interface ICommandsExecutor
    {
        public Task Execute(string[] args);
    }
}