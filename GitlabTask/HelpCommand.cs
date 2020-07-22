using System;
using System.IO;
using System.Threading.Tasks;

namespace GitlabTask
{
    public class HelpCommand : Command
    {
        private readonly Func<string[]> _getRegisteredCommandNames;

        public HelpCommand(Func<string[]> getRegisteredCommandNames)
            : base("help", "shows information")
        {
            _getRegisteredCommandNames = getRegisteredCommandNames;
        }

        public override async Task Execute(string[] args, TextWriter writer)
        {
            await writer.WriteAsync(string.Join(' ', _getRegisteredCommandNames()));
        }
    }
}