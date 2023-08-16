using QuanLib.CommandLine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.ConsoleCommand
{
    public class CommandWriter : ICommandWriter
    {
        public void WriteResult(string? text)
        {
            if (text is not null)
                Console.WriteLine(text);
        }
    }
}
