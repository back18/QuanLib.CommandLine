using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Objects
{
    public abstract class Item
    {
        protected Item(string text, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(text, nameof(text));

            Text = text;
            Value = ToValue(text);
            StartIndex = startIndex;
        }

        public abstract ItemType Type { get; }

        public string Text { get; }

        public string Value { get; }

        public int StartIndex { get; }

        public int EndIndex => StartIndex + Text.Length;

        public int Length => Text.Length;

        public static string ToValue(string text)
        {
            if (text.Length >= 2 && text.StartsWith("\"") && text.EndsWith("\""))
                text = text[1..^1];
            return Regex.Unescape(text);
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
