using QuanLib.CommandLine.Attributes;
using QuanLib.CommandLine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    public class CommandSystem
    {
        public CommandSystem(CommandSender sender, CommandParser parser, CommandTerminal terminal)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            Terminal = terminal ?? throw new ArgumentNullException(nameof(terminal));
            _isrun = false;

            _objectPool = new();
        }

        private bool _isrun;

        private readonly CommandSender _sender;

        private readonly CommandParser _parser;

        private readonly Dictionary<string, object?> _objectPool;

        public CommandTerminal Terminal { get; }

        public bool IsRun => _isrun;

        public void Start()
        {
            _isrun = true;
            Console.WriteLine("命令系统已启动");
            try
            {
                while (_isrun)
                {
                    CommandItems items = Terminal.Reader.Start();
                    string? message = Terminal.Executor.ExecuteCommand(_sender, _parser.ToCommandObject(items), out var result);
                    Terminal.Outputer.OutputMessage(message);
                }
            }
            catch (Exception ex)
            {
                if (_isrun)
                {
                    Console.WriteLine("命令系统遇到意外异常，错误信息：");
                    Console.WriteLine(ex.ToString());
                }
                throw;
            }
        }

        public void Stop()
        {
            _isrun = false;
            Terminal.Reader.Stop();
            Console.WriteLine("命令系统已终止");
        }

        private class SystemCommandSet
        {
            public SystemCommandSet(CommandSystem system)
            {
                _system = system ?? throw new ArgumentNullException(nameof(system));
            }

            private readonly CommandSystem _system;

            [Command("system objectpool add", false)]
            public void AddObject(string key, object? obj)
                => _system._objectPool.Add(key, obj);
        }
    }
}
