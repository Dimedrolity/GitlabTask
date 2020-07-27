﻿using System;
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
        public readonly List<Command> Commands = new List<Command>();

        private readonly TextWriter _writer;

        public CommandsExecutor(TextWriter writer)
        {
            _writer = writer;
        }

        public void RegisterCommand(Command command)
        {
            Commands.Add(command);
        }

        public async Task Execute(string commandName, Dictionary<string, string> args = null)
        {
            var cmd = FindCommandByName(commandName);
            if (cmd == null)
                _writer.WriteLine("Неизвестная команда {0}", commandName);
            else
            {
                await cmd.Execute(args, _writer);
            }
        }

        private Command FindCommandByName(string name)
        {
            return Commands.FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));
        }
    }
}