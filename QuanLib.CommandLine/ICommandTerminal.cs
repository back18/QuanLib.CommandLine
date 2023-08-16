using QuanLib.CommandLine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    public interface ICommandTerminal
    {
        public abstract ICommandReader Reader { get; }

        public abstract ICommandExecutor Executor { get; }

        public abstract ICommandWriter Writer { get; }
    }
}
