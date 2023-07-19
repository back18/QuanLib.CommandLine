using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Exceptions
{
    public class CommandArgumentOutOfRangeException : CommandArgumentException
    {
        public CommandArgumentOutOfRangeException() : base(DefaultMessage) { }

        public CommandArgumentOutOfRangeException(string? message) : base(message) { }

        public CommandArgumentOutOfRangeException(CommandArgument argument, object? argumentValue) :
            base(argument, argumentValue, DefaultMessage) { }

        public CommandArgumentOutOfRangeException(CommandArgument argument, object? argumentValue, string? message) :
            base(argument, argumentValue, message) { }

        protected new static string DefaultMessage = "一个或多个命令参数的数值超出了给定的范围";

        public override string Message
        {
            get
            {
                string s = base.Message;
                if (string.IsNullOrEmpty(s))
                {
                    if (Argument is not null)
                        s = "命令参数的数值超出了给定的范围";
                    else s = DefaultMessage;
                }
                if (ArgumentValue is not null)
                    s += ArgumentValue.ToString();
                if (Argument is not null)
                {
                    s += Argument.ToString();
                    if (Argument.Range is not null)
                        s += Argument.Range.ToString();
                }

                return s;
            }
        }
    }
}
