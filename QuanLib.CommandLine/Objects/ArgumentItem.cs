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
            ArgumentNullException.ThrowIfNull(argument, nameof(argument));

            Argument = argument;
        }

        public CommandArgument Argument { get; }

        public override ItemType Type => ItemType.Argument;
    }
}
