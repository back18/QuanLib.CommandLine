using QuanLib.CommandLine.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Objects
{
    public class CommandObject
    {
        public CommandObject(Command? command, IReadOnlyList<string> args, object? obj = null)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            if (args.Any(item => item is null))
                throw new ArgumentException("含有null值的子项", nameof(args));

            Command = command;
            Arguments = args ?? throw new ArgumentNullException(nameof(args));
            Object = obj;
        }

        public IReadOnlyList<string> Arguments { get; }

        public Command? Command { get; }

        public object? Object { get; }
    }
}
