using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Exceptions
{
    public class CommandArgumentException : CommandException
    {
        public CommandArgumentException() : base(DefaultMessage) { }

        public CommandArgumentException(string? message) : base(message) { }

        public CommandArgumentException(CommandArgument argument, object? argumentValue = null, string? message = null, Exception? innerException = null) : base(message, innerException)
        {
            Argument = argument;
            ArgumentValue = argumentValue;
        }

        protected const string DefaultMessage = "命令参数不是预期的";

        public CommandArgument? Argument { get; }

        public object? ArgumentValue { get; }
    }
}
