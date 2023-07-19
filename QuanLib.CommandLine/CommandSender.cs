using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    public class CommandSender
    {
        public CommandSender(Level level)
        {
            Level = level;
        }

        public Level Level { get; }
    }
}
