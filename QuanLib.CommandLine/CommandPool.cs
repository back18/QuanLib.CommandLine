using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    public class CommandPool
    {
        public CommandPool()
        {
            _command = new();
            RootKeyNode = new("root");
        }

        private readonly Dictionary<string, Command> _command;

        public KeyNode RootKeyNode { get; }

        public int Count => _command.Count;

        public Command this[string key] => _command[key];

        public void AddCommand(Command command)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));

            if (!_command.TryAdd(command.Key.ToString(), command))
                throw new ArgumentException("命令重复" ,nameof(command));

            RootKeyNode.AddSubNode(command.Key);
        }

        public void RemoveCommand(string key)
        {
            ArgumentNullException.ThrowIfNull(key, nameof(key));

            if (!_command.ContainsKey(key))
                throw new ArgumentException("不存在该命令：" + key, nameof(key));

            _command.Remove(key);
            RootKeyNode.RemoveSubNode(key);
        }

        public bool TryGetCommand(string key, [MaybeNullWhen(false)] out Command command)
            => _command.TryGetValue(key, out command);

        public bool ContainsCommand(string key)
            => _command.ContainsKey(key);
    }
}
