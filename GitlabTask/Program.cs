using System;
using System.Threading.Tasks;
using GitlabTask.Commands;
using GitlabTask.Interfaces;

namespace GitlabTask
{
    class Program
    {
        static async Task Main()
        {
            var executor = CreateExecutor();
            await RunInteractiveMode(executor);
        }

        private static ICommandsExecutor CreateExecutor()
        {
            var writer = Console.Out;
            var commandsExecutor = new CommandsExecutor(writer);
            var config = new Config();
            commandsExecutor.RegisterCommand(new CommitsCommand(config, new GitlabCommitsGetter(new JsonConverter())));
            commandsExecutor.RegisterCommand(new HelpCommand(commandsExecutor.GetRegisteredCommands));
            commandsExecutor.RegisterCommand(new TrackedProjectsCommand(config));

            return commandsExecutor;
        }

        public static async Task RunInteractiveMode(ICommandsExecutor executor)
        {
            Console.WriteLine("\nВведите команду, например help");
            while (true)
            {
                var line = Console.ReadLine();
                if (line == null || line == "exit")
                    return;
                await executor.Execute(line.Split(' '));
                Console.WriteLine();
            }
        }
    }
}