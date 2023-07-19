using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Objects
{
    public class ArgumentItem : Item
    {
        public ArgumentItem(string text, int startIndex, CommandArgument argument) : base(text, startIndex)
        {
            Argument = argument ?? throw new ArgumentNullException(nameof(argument));
        }

        public CommandArgument Argument { get; }

        public override ItemType Type => ItemType.Argument;
    }
}
