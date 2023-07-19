using QuanLib.CommandLine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    public abstract class CommandTerminal
    {
        public abstract CommandReader Reader { get; }

        public abstract CommandExecutor Executor { get; }

        public abstract MessageOutputer Outputer { get; }
    }
}
