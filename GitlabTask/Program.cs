using System;

namespace GitlabTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var executor = CreateExecutor();
            if (args.Length > 0)
                executor.Execute(args);
            else
                RunInteractiveMode(executor);
        }

        private static ICommandsExecutor CreateExecutor()
        {
            var writer = Console.Out;
            var commandsExecutor = new CommandsExecutor(writer);
            commandsExecutor.RegisterCommand(
                new CommitsCommand(new Config(), new GitlabCommitsGetter(new JsonParser())));
            commandsExecutor.RegisterCommand(new HelpCommand(commandsExecutor.GetRegisteredCommandNames));

            return commandsExecutor;
        }

        public static void RunInteractiveMode(ICommandsExecutor executor)
        {
            while (true)
            {
                var line = Console.ReadLine();
                if (line == null || line == "exit")
                    return;
                executor.Execute(line.Split(' '));
            }
        }
    }
}