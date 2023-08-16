using QuanLib.CommandLine.Attributes;
using QuanLib.CommandLine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    public interface ICommandSystem : ISwitchable
    {
        public CommandSender Sender { get; }

        public CommandPool Pool { get; }

        public CommandParser Parser { get; }

        public ICommandTerminal Terminal { get; }
    }
}
