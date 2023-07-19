using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Objects
{
    public class KeyErrorItem : Item
    {
        public KeyErrorItem(string text, KeyNode baseNode, int startIndex) : base(text, startIndex)
        {
            BaseNode = baseNode;
        }

        public override ItemType Type => ItemType.KeyError;

        public KeyNode BaseNode { get; }
    }
}
