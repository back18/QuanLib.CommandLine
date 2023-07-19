using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    /// <summary>
    /// 表示一个命令
    /// </summary>
    public class Command
    {
        public Command(CommandKey key, CommandFunc func, Level level = Level.User)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Func = func ?? throw new ArgumentNullException(nameof(func));
            Level = level;
        }

        public CommandKey Key { get; }

        public CommandFunc Func { get; }

        public Level Level { get; }
    }
}
