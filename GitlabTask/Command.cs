using System.IO;

namespace GitlabTask
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

        public abstract void Execute(string[] args, TextWriter writer);
    }
}