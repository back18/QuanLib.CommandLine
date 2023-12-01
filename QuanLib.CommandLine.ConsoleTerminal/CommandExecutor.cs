using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.ConsoleTerminal
{
    public class CommandExecutor : ICommandExecutor
    {
        public string? ExecuteCommand(CommandSender sender, CommandObject obj, out object? result)
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));

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

            try
            {
                return obj.Command.Func.Run(obj.Object, out result, obj.Arguments.ToArray());
            }
            catch (Exception ex)
            {
                result = default;
                return $"【错误】无法执行命令，错误信息: {ex.GetType()}: {ex.Message}";
            }
        }
    }
}
