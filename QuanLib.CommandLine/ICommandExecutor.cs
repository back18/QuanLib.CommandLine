using QuanLib.CommandLine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    public interface ICommandExecutor
    {
        public string? ExecuteCommand(CommandSender sender, CommandObject obj, out object? result);
    }
}
