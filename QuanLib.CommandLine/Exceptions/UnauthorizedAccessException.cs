using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Exceptions
{
    public class UnauthorizedAccessException : Exception
    {
        public UnauthorizedAccessException() : base("没有足够的权限执行命令") { }

        public UnauthorizedAccessException(string? message) : base(message) { }
    }
}
