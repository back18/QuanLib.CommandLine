using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Exceptions
{
    public class CommandUnknownOrIncompleteException : CommandException
    {
        public CommandUnknownOrIncompleteException() : base("未知或不完整命令") { }

        public CommandUnknownOrIncompleteException(string? message) : base(message) { }
    }
}
