using System;
using System.IO;

namespace GitlabTask
{
    public class HelpCommand : Command
    {
        public HelpCommand(Func<string[]> getRegisteredCommandNames)
            : base("help", "shows information")
        {
        }

        public override void Execute(string[] args, TextWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}