using QuanLib.ConsoleUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.ConsoleCommand
{
    public class Prompt
    {
        public Prompt(string value, int bufferIndex)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
            BufferIndex = bufferIndex;
        }

        public string Value { get; }

        public int BufferIndex { get; }
    }
}
