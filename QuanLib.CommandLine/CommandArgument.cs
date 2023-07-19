using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    public class CommandArgument
    {
        public CommandArgument(int index, Type type, string name, string description, ObjectParser parser, ObjectRange? range = null)
        {
            Index = index;
            Type = type;
            Name = name;
            Description = description;
            Parser = parser;
            Range = range;
        }

        public int Index { get; }

        public Type Type { get; }

        public string Name { get; }

        public string Description { get; }

        public ObjectParser Parser { get; }

        public ObjectRange? Range { get; }

        public override string ToString()
        {
            return $"(Index: {Index} Type: {Type} Name: {Name})";
        }
    }
}
