using QuanLib.CommandLine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.ConsoleCommand
{
    public class ConsoleTerminal : CommandTerminal
    {
        public ConsoleTerminal(ConsoleCommandReader reader, CommandExecutor executor, ConsoleMessageOutputer outputer)
        {
            Reader = reader;
            Executor = executor;
            Outputer = outputer;
        }

        public override CommandReader Reader { get; }

        public override CommandExecutor Executor { get; }

        public override MessageOutputer Outputer { get; }
    }
}
