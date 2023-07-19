using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Objects
{
    public class KeyItem : Item
    {
        public KeyItem(string text, int startIndex, KeyNode node) : base(text, startIndex)
        {
            Node = node ?? throw new ArgumentNullException(nameof(node));
        }

        public KeyNode Node { get; }

        public override ItemType Type => ItemType.Key;
    }
}
