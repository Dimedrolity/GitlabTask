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
            commandsExecutor.RegisterCommand(new CommitsCommand(config,
                new GitlabCommitsGetter(new JsonConverter(), config)));
            commandsExecutor.RegisterCommand(new HelpCommand(() => commandsExecutor.Commands));
            commandsExecutor.RegisterCommand(new ProjectsCommand(config));

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