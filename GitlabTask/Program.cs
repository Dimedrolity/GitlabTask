﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GitlabTask.Commands;
using GitlabTask.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GitlabTask
{
    class Program
    {
        private static string _keyValueSeparator = ":";

        static async Task Main(string[] args)
        {
            var writer = Console.Out;

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();
            var config = new Config(configurationBuilder);

            var executor = CreateExecutor(writer, config);

            if (args.Length != 0)
            {
                await ConvertArgsAndExecute(executor, args);
            }
            else
            {
                await writer.WriteAsync(
                    $"\nЗапустите приложение, передав команду аргументом например 'command{_keyValueSeparator}help'");
            }
        }

        private static ICommandsExecutor CreateExecutor(TextWriter writer, IConfig config)
        {
            var commandsExecutor = new CommandsExecutor(writer);
            var httpClient = new HttpClient();
            var jsonConverter = new JsonConverter();
            commandsExecutor.RegisterCommand(new CommitsCommand(config,
                new GitlabCommitsGetter(jsonConverter, config, httpClient),
                new GitlabBranchesGetter(jsonConverter, config, httpClient)));
            commandsExecutor.RegisterCommand(new HelpCommand(() => commandsExecutor.Commands));
            commandsExecutor.RegisterCommand(new ProjectsCommand(config));
            return commandsExecutor;
        }

        private static async Task ConvertArgsAndExecute(ICommandsExecutor executor, IEnumerable<string> args)
        {
            Dictionary<string, string> argsAsDictionary = null;
            try
            {
                argsAsDictionary = ConvertArgsToDictionary(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (argsAsDictionary != null)
            {
                var commandName = argsAsDictionary["command"];
                argsAsDictionary.Remove("command");
                await executor.Execute(commandName, argsAsDictionary);
            }
        }

        private static Dictionary<string, string> ConvertArgsToDictionary(IEnumerable<string> args)
        {
            return args
                .Select(keyAndValue =>
                {
                    var split = keyAndValue.Split(_keyValueSeparator);
                    if (split.Length != 2)
                    {
                        throw new ArgumentException($"Неправильный формат ввода - {keyAndValue}\n" +
                                                    $"Правильный формат - command{_keyValueSeparator}<название команды>," +
                                                    " например, command:help");
                    }

                    return new KeyValuePair<string, string>(split[0], split[1]);
                })
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}