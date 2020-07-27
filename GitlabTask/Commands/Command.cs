using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GitlabTask.Commands
{
    public abstract class Command
    {
        public string Name { get; }
        public string Description { get; }

        protected Command(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public abstract Task Execute(Dictionary<string, string> args, TextWriter writer);
    }
}