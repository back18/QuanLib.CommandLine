using QuanLib.CommandLine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    public abstract class MessageOutputer
    {
        public abstract void OutputMessage(string? message);
    }
}
