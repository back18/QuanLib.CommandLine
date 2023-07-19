using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Exceptions
{
    public class CommandArgumentCountException : CommandArgumentException
    {
        public CommandArgumentCountException() : base(DefaultMessage) { }

        public CommandArgumentCountException(string? message) : base(message) { }

        public CommandArgumentCountException(int presetCount, int actualCount) : base(DefaultMessage)
        {
            PresetCount = presetCount;
            ActualCount = actualCount;
        }

        protected new const string DefaultMessage = "命令参数的数量不是预期的";

        public int? PresetCount { get; }

        public int? ActualCount { get; }

        public override string Message
        {
            get
            {
                string s = base.Message;
                if (string.IsNullOrEmpty(s))
                    s = DefaultMessage;

                if (PresetCount is not null && ActualCount is not null)
                    s += $"（应为 {PresetCount} 个，实际为 {ActualCount} 个）";

                return s;
            }
        }
    }
}
