using QuanLib.CommandLine.Attributes;
using QuanLib.CommandLine.Objects;
using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    public interface ICommandSystem : IRunnable
    {
        public CommandSender CommandSender { get; }

        public CommandPool CommandPool { get; }

        public CommandParser CommandParser { get; }

        public ICommandTerminal CommandTerminal { get; }
    }
}
