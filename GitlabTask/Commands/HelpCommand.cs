using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GitlabTask.Commands
{
    public class HelpCommand : Command
    {
        private readonly Func<List<Command>> _getRegisteredCommands;

        private readonly string _argumentNameForCommandDescription = "about";

        public HelpCommand(Func<List<Command>> getRegisteredCommands)
            : base("help", "По умолчанию показывает список доступных команд.\n" +
                           "При указании аргумента 'about:<command>' выводит подробное описание команды.\n")
        {
            _getRegisteredCommands = getRegisteredCommands;
        }

        public override async Task Execute(Dictionary<string, string> args, TextWriter writer)
        {
            var commands = _getRegisteredCommands();
            if (args == null || args.Count == 0)
            {
                var commandNames = commands.Select(c => c.Name);
                await writer.WriteAsync("Список доступных команд:\r\n" +
                                        $"- command:{string.Join("\r\n- command:", commandNames)}\r\n" +
                                        "Более подробная информация -> 'command:help about:<command>'\n");
                return;
            }

            var commandName = args[_argumentNameForCommandDescription];
            var command = commands
                .FirstOrDefault(c => string.Equals(c.Name, commandName, StringComparison.OrdinalIgnoreCase));

            if (command != null)
                await writer.WriteAsync($"Описание команды {commandName}:\r\n" + $"- {command.Description}");
            else
                await writer.WriteAsync($"Команда {commandName} не существует\r\n");
        }
    }
}