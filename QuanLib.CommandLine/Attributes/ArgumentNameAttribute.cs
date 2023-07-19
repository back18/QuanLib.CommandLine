using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class ArgumentNameAttribute : Attribute
    {
        public ArgumentNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
