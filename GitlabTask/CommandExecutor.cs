using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GitlabTask
{
    public class CommandsExecutor : ICommandsExecutor
    {
        private readonly List<Command> _commands = new List<Command>();

        private readonly TextWriter _writer;

        public CommandsExecutor(TextWriter writer)
        {
            _writer = writer;
        }

        public void RegisterCommand(Command command)
        {
            _commands.Add(command);
        }

        public string[] GetRegisteredCommandNames()
        {
            return _commands.Select(c => c.Name).ToArray();
        }

        private Command FindCommandByName(string name)
        {
            return _commands.FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task Execute(string[] args)
        {
            if (args[0].Length == 0)
            {
                Console.WriteLine("Please specify <command> as the first command line argument");
                return;
            }

            var commandName = args[0];

            var cmd = FindCommandByName(commandName);
            if (cmd == null)
                _writer.WriteLine("Sorry. Unknown command {0}", commandName);
            else
            {
                // var restArgs = new Span<string>(args, 1, int.MaxValue);

                await cmd.Execute(args[1..], _writer);
            }
        }
    }
}