using QuanLib.CommandLine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.ConsoleCommand
{
    public class ConsoleMessageOutputer : MessageOutputer
    {
        public override void OutputMessage(string? message)
        {
            if (message is not null)
                Console.WriteLine(message);
        }
    }
}
