using System.IO;
using System.Threading.Tasks;

namespace GitlabTask.Commands
{
    public abstract class Command
    {
        protected Command(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; }
        public string Description { get; }

        public abstract Task Execute(string[] args, TextWriter writer);
    }
}