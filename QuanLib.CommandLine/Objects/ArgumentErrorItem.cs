using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Objects
{
    public class ArgumentErrorItem : ArgumentItem
    {
        public ArgumentErrorItem(string text, int startIndex, CommandArgument argument, Exception exception) : base(text, startIndex, argument)
        {
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }

        public override ItemType Type => ItemType.ArgumentError;

        public Exception Exception { get; }
    }
}
