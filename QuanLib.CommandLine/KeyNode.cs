using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    public class KeyNode : IEquatable<KeyNode>, IEnumerable<KeyNode>
    {
        public KeyNode(string key, KeyNode? baseNode = null)
        {
            ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));

            Key = key;
            BaseNode = baseNode;
            Level = baseNode is null ? 0 : baseNode.Level + 1;
            
            _subnodes = new();
        }

        private readonly Dictionary<string, KeyNode> _subnodes;

        public string Key { get; }

        public int Level { get; }

        public KeyNode? BaseNode { get; private set; }

        public IEnumerable<KeyNode> SubNodes => _subnodes.Values;

        public bool IsRootNode => BaseNode is null;

        public KeyNode this[string key] => _subnodes[key];

        internal void AddSubNode(string nodekey)
        {
            ArgumentNullException.ThrowIfNull(nodekey, nameof(nodekey));

            if (!_subnodes.TryAdd(nodekey, new(nodekey, this)))
                throw new ArgumentException("节点重复", nameof(nodekey));
        }

        internal void AddSubNode(CommandKey commandKey)
        {
            ArgumentNullException.ThrowIfNull(commandKey, nameof(commandKey));

            KeyNode node = this;
            foreach (string item in commandKey.KeyItems)
            {
                if (!node.ContainsSubNode(item))
                    node.AddSubNode(item);
                node = node[item];
            }
        }

        internal void RemoveSubNode(string? nodekey)
        {
            ArgumentNullException.ThrowIfNull(nodekey, nameof(nodekey));

            if (!_subnodes.ContainsKey(nodekey))
                throw new ArgumentException("不存在该节点", nameof(nodekey));

            _subnodes[nodekey].BaseNode = null;
            _subnodes.Remove(nodekey);
        }

        public bool ContainsSubNode(string? nodekey)
        {
            return nodekey is not null && _subnodes.ContainsKey(nodekey);
        }

        public KeyNode[] GetBaseNodes(bool ignoreRoot = false)
        {
            List<KeyNode> nodes = new();
            KeyNode node = this;
            while (true)
            {
                if (ignoreRoot && node.IsRootNode)
                    break;
                nodes.Add(node);
                if (node.BaseNode is null)
                    break;
                node = node.BaseNode;
            }
            nodes.Reverse();
            return nodes.ToArray();
        }

        public string ToCommandKey(bool ignoreRoot = false)
        {
            List<string> items = new();
            foreach (var item in GetBaseNodes(ignoreRoot))
                items.Add(item.Key);
            return string.Join(' ', items.ToArray());
        }

        public static KeyNode BulidRootNode(CommandKey[] keys)
        {
            ArgumentNullException.ThrowIfNull(keys, nameof(keys));

            KeyNode rootnode = new("root");
            foreach (CommandKey commandKey in keys)
                rootnode.AddSubNode(commandKey);

            return rootnode;
        }

        public override string ToString()
        {
            return Key;
        }

        public bool Equals(KeyNode? other)
        {
            if (other is null)
                return false;

            return Key == other.Key;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as KeyNode);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public IEnumerator<KeyNode> GetEnumerator()
        {
            return SubNodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)SubNodes).GetEnumerator();
        }
    }
}
