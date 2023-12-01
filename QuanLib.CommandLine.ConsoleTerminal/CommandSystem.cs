using QuanLib.CommandLine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.ConsoleTerminal
{
    public class CommandSystem : ICommandSystem
    {
        public CommandSystem(CommandSender sender)
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));

            Sender = sender;
            Pool = new();
            Parser = new(Pool);
            Terminal = new CommandTerminal(Parser);

            Pool.AddCommand(new(new("commandsystem stop"), CommandFunc.GetFunc(Stop, this)));

            _runing = false;
        }

        private bool _runing;

        public bool Runing => _runing;

        public CommandSender Sender { get; }

        public CommandPool Pool { get; }

        public CommandParser Parser { get; }

        public ICommandTerminal Terminal { get; }

        public void Start()
        {
            if (_runing)
                return;
            _runing = true;

            while (_runing)
            {
                CommandItems items = Terminal.Reader.ReadCommand();
                string? output = Terminal.Executor.ExecuteCommand(Sender, Parser.ToCommandObject(items), out var result);
                Terminal.Writer.WriteResult(output);
            }
        }

        public void Stop()
        {
            _runing = false;
            Terminal.Reader.StopRead();
        }
    }
}
