using QuanLib.CommandLine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.ConsoleTerminal
{
    public class CommandTerminal : ICommandTerminal
    {
        public CommandTerminal(CommandParser parser)
        {
            Reader = new CommandReader(parser);
            Executor = new CommandExecutor();
            Writer = new CommandWriter();
        }

        public ICommandReader Reader { get; }

        public ICommandExecutor Executor { get; }

        public ICommandWriter Writer { get; }
    }
}
