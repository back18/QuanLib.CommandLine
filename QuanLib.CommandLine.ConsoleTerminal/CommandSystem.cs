using QuanLib.CommandLine.Objects;
using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.ConsoleTerminal
{
    public class CommandSystem : RunnableBase, ICommandSystem
    {
        public CommandSystem(CommandSender sender)
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));

            CommandSender = sender;
            CommandPool = new();
            CommandParser = new(CommandPool);
            CommandTerminal = new CommandTerminal(CommandParser);

            CommandPool.AddCommand(new(new("commandsystem stop"), CommandFunc.GetFunc(Stop, this)));
        }

        public CommandSender CommandSender { get; }

        public CommandPool CommandPool { get; }

        public CommandParser CommandParser { get; }

        public ICommandTerminal CommandTerminal { get; }

        protected override void Run()
        {
            while (IsRunning)
            {
                CommandItems items = CommandTerminal.Reader.ReadCommand();
                string? output = CommandTerminal.Executor.ExecuteCommand(CommandSender, CommandParser.ToCommandObject(items), out var result);
                CommandTerminal.Writer.WriteResult(output);
            }
        }
    }
}
