using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class ArgumentDescriptionAttribute : Attribute
    {
        public ArgumentDescriptionAttribute(string description)
        {
            Description = description;
        }

        public string Description { get; }
    }
}
