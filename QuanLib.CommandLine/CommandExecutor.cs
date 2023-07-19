using QuanLib.CommandLine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    public class CommandExecutor
    {
        public virtual string? ExecuteCommand(CommandSender sender, CommandObject obj, out object? result)
        {
            if (sender is null)
                throw new ArgumentNullException(nameof(sender));
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (obj.Command is null)
            {
                result = null;
                return "【错误】未知或不完整命令。";
            }

            if (sender.Level < obj.Command.Level)
            {
                result = null;
                return "【错误】没有足够的权限执行该命令。";
            }

            return obj.Command.Func.Run(obj.Object, out result, obj.Arguments.ToArray());
        }
    }
}
