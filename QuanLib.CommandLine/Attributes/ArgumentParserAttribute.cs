using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class ArgumentParserAttribute : Attribute
    {
        public ArgumentParserAttribute(ObjectParser.ParseDelegate parse, ObjectParser.TryParseDelegate? tryParse = null)
        {
            Parser = new(parse, tryParse);
        }

        public ObjectParser Parser { get; }
    }
}
