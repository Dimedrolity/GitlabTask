using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using GitlabTask.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GitlabTask
{
    class Program
    {
        /// <summary>
        /// Консольное приложения для просмотра Gitlab коммитов.
        /// Показывает список коммитов в хронологическом порядке.
        /// Временной интервал изменяется с помощью аргументов hours и/или days и расчитывается относительно текущего времени.
        /// Например, "--hours 3 --days 1", коммиты за последние 1 день и 3 часа от текущего времени
        /// </summary>
        /// <param name="hours">Показать коммиты за последние hours часов</param>
        /// <param name="days">Показать коммиты за последние days дней</param>
        /// <param name="branches">Переопределяет список бранчей, из которых показывать коммиты.
        /// Это применится ко всем проектам, независимо от того, что для них указано в конфиге!</param>
        /// <param name="output">При использовании аргумента будет создан/перезаписан файл.
        /// Файл будет иметь название значения аргумента. В этот файл будет произведен вывод.
        /// Пример валидного значения – out.txt</param>
        private static async Task Main(int hours, int days, string branches, string output)
        {
            var config = GetConfig();
            var httpClient = new HttpClient();
            var jsonConverter = new JsonConverter();
            var commitsCommand = new CommitsCommand(config,
                new GitlabCommitsGetter(jsonConverter, config, httpClient),
                new GitlabBranchesGetter(jsonConverter, config, httpClient));

            var writer = output == null ? Console.Out : new StreamWriter(output);
            await WriteStartDate(writer, hours, days);
            await commitsCommand.Execute(writer, hours, days, branches);
        }

        private static IConfig GetConfig()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();
            
            return new Config(configurationBuilder);
        }

        private static async Task WriteStartDate(TextWriter writer, int hours, int days)
        {
            var startDate = DateTime.Now.AddDays(-days).AddHours(-hours);
            var startDateToString = startDate.ToString("dddd h:mm, d MMMM", CultureInfo.InvariantCulture);

            await writer.WriteLineAsync(
                $"Коммиты, начиная с {startDateToString} (за последние {days} дней и {hours} часов)\n");
        }
    }
}