using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Objects
{
    public class OverflowItem : Item
    {
        public OverflowItem(string text, int startIndex) : base(text, startIndex) { }

        public override ItemType Type => ItemType.Overflow;
    }
}
