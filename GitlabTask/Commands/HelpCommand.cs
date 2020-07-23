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

        public HelpCommand(Func<List<Command>> getRegisteredCommands)
            : base("help", "help <command>.\n" +
                           "По умолчанию показывает список доступных команд.\n" +
                           "При указании аргумента <command> выводит подробное описание команды.\n")
        {
            _getRegisteredCommands = getRegisteredCommands;
        }

        public override async Task Execute(string[] args, TextWriter writer)
        {
            var commands = _getRegisteredCommands();
            if (args.Length == 0)
            {
                var commandNames = commands.Select(c => c.Name);
                await writer.WriteAsync("Список доступных команд:\r\n" +
                                        $"- {string.Join("\r\n- ", commandNames)}\r\n" +
                                        "Более подробная информация -> help <command>\n");
                return;
            }

            var commandName = args[0];
            var command = commands
                .FirstOrDefault(c => string.Equals(c.Name, commandName, StringComparison.OrdinalIgnoreCase));
            
            if (command != null)
                await writer.WriteAsync($"Описание команды {commandName}:\r\n" + $"- {command.Description}");
            else
                await writer.WriteAsync($"Команда {commandName} не существует\r\n");
        }
    }
}