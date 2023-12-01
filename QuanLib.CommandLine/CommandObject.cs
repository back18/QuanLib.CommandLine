using QuanLib.CommandLine.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    public class CommandObject
    {
        public CommandObject(Command? command, IReadOnlyList<string> args, object? obj = null)
        {
            ArgumentNullException.ThrowIfNull(args, nameof(args));

            if (args.Any(item => item is null))
                throw new ArgumentException("含有null值的子项", nameof(args));

            Command = command;
            Arguments = args;
            Object = obj;
        }

        public IReadOnlyList<string> Arguments { get; }

        public Command? Command { get; }

        public object? Object { get; }
    }
}
