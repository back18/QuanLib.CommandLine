using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class CommandAttribute : Attribute
    {
        public CommandAttribute(string key, bool allowGetReturnValue)
        {
            Key = key;
            AllowGetReturnValue = allowGetReturnValue;
        }

        public string Key { get; }

        public bool AllowGetReturnValue { get; }
    }
}
