using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Exceptions
{
    public class CommandReaderException : Exception
    {
        public CommandReaderException() : base("命令读取器异常") { }

        public CommandReaderException(string? message) : base(message) { }
    }
}
