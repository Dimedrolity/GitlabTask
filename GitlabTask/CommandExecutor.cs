using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GitlabTask.Commands;
using GitlabTask.Interfaces;

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

        public List<Command> GetRegisteredCommands()
        {
            return _commands;
        }

        private Command FindCommandByName(string name)
        {
            return _commands.FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task Execute(string[] args)
        {
            if (args[0].Length == 0)
            {
                Console.WriteLine("Введите команду <command> первым аргументом командной строки");
                return;
            }

            var commandName = args[0];

            var cmd = FindCommandByName(commandName);
            if (cmd == null)
                _writer.WriteLine("Неизвестная команда {0}", commandName);
            else
            {
               await cmd.Execute(args[1..], _writer);
            }
        }
    }
}