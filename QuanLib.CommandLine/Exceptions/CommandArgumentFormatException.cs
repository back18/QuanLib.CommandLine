using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Exceptions
{
    public class CommandArgumentFormatException : CommandArgumentException
    {
        public CommandArgumentFormatException() : base(DefaultMessage) { }

        public CommandArgumentFormatException(string? message) : base(message) { }

        public CommandArgumentFormatException(CommandArgument argument, object? argumentValue, Exception? innerException = null) :
            base(argument, argumentValue, DefaultMessage, innerException) { }

        public CommandArgumentFormatException(CommandArgument argument, object? argumentValue, string? message, Exception? innerException) :
        base(argument, argumentValue, message, innerException)  { }

        protected new static string DefaultMessage = "一个或多个命令参数解析失败";

        public override string Message
        {
            get
            {
                string s = base.Message;
                if (string.IsNullOrEmpty(s))
                {
                    if (Argument is not null)
                        s = "命令参数解析失败";
                    else s = DefaultMessage;
                }
                if (ArgumentValue is not null)
                    s += ArgumentValue.ToString();
                if (Argument is not null)
                {
                    s += Argument.ToString();
                    if (InnerException is not null)
                        s +=  $"(内部异常: {InnerException.Message})";
                }

                return s;
            }
        }
    }
}
