using QuanLib.CommandLine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    public abstract class CommandReader
    {
        public abstract bool IsRun { get; }

        public abstract CommandItems Start();

        public abstract void Stop();
    }
}
