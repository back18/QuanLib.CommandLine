using QuanLib.CommandLine.Exceptions;
using QuanLib.CommandLine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    public class CommandParser
    {
        public CommandParser(CommandPool pool)
        {
            _pool = pool;
        }

        private readonly CommandPool _pool;

        public CommandItems ToItems(string s)
        {
            ArgumentNullException.ThrowIfNull(s, nameof(s));

            List<Item> result = new();

            string[] items = Split(s);
            KeyNode node = _pool.RootKeyNode;
            Command? command = null;
            int itemIndex = default;
            int argIndex = default;
            ParseStage stage = ParseStage.Key;
            for (int i = 0; i < items.Length; i++)
            {
                string item = items[i];
                string value = Item.ToValue(item);

                switch (stage)
                {
                    case ParseStage.Key:
                        if (!string.IsNullOrEmpty(value) && node.ContainsSubNode(value))
                        {
                            node = node[value];
                            result.Add(new KeyItem(item, itemIndex, node));
                            _pool.TryGetCommand(node.ToCommandKey(true), out command);
                        }
                        else if (command is not null)
                        {
                            stage = ParseStage.Argument;
                            argIndex = i;
                            goto case ParseStage.Argument;
                        }
                        else
                        {
                            result.Add(new KeyErrorItem(item, node, itemIndex));
                            stage = ParseStage.Overflow;
                        }
                        break;
                    case ParseStage.Argument:
                        if (command is null)
                            throw new InvalidOperationException();
                        if (i - argIndex < command.Func.Arguments.Count)
                        {
                            CommandArgument argument = command.Func.Arguments[i - argIndex];
                            if (argument.Parser.TryParse(value, out var obj))
                            {
                                bool? range = argument.Range?.IsRange(obj);
                                if (range is null || range.Value)
                                    result.Add(new ArgumentItem(item, itemIndex, argument));
                                else
                                    result.Add(new ArgumentErrorItem(item, itemIndex, argument, new CommandArgumentOutOfRangeException(argument, range.Value)));
                            }
                            else
                                result.Add(new ArgumentErrorItem(item, itemIndex, argument, new CommandArgumentFormatException(argument, item)));
                        }
                        else
                        {
                            stage = ParseStage.Overflow;
                            goto case ParseStage.Overflow;
                        }
                        break;
                    case ParseStage.Overflow:
                        result.Add(new OverflowItem(item, itemIndex));
                        break;
                    default:
                        throw new InvalidOperationException();
                }
                itemIndex += item.Length + 1;
            }

            for (int i = result.Count - 1; i >= 1; i--)
            {
                switch (result[i].Type)
                {
                    case ItemType.Key:
                    case ItemType.Argument:
                        goto ok;
                    case ItemType.ArgumentError:
                    case ItemType.Overflow:
                        if (result[i - 1] is KeyItem keyItem)
                        {
                            result[i] = new KeyErrorItem(result[i].Text, keyItem.Node, result[i].StartIndex);
                            goto ok;
                        }
                        break;
                }
            }

            ok:
            return new(result);
        }

        public Command? ToCommand(CommandItems items)
        {
            if (items is null)
                return null;

            List<string> keys = new();
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Type == ItemType.Key)
                    keys.Add(items[i].Value);
                else
                    break;
            }

            _pool.TryGetCommand(string.Join(' ', keys), out var command);
            return command;
        }

        public CommandObject ToCommandObject(CommandItems items)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            List<string> keys = new();
            List<string> args = new();
            ParseStage stage = ParseStage.Key;
            for (int i = 0; i < items.Count; i++)
            {
                switch (items[i].Type)
                {
                    case ItemType.Key:
                        if (stage != ParseStage.Key)
                            goto default;
                        keys.Add(items[i].Value);
                        break;
                    default:
                        if (stage == ParseStage.Key)
                            stage = ParseStage.Argument;
                        args.Add(items[i].Value);
                        break;
                }
            }

            _pool.TryGetCommand(string.Join(' ', keys), out var command);
            return new(command, args.AsReadOnly());
        }

        public static string[] Split(string text)
        {
            string[] items = text.Split(' ');
            Queue<(int start, int end)> ranges = new();

            int index = 0;
            while (true)
            {
                int start = GetStart(index);
                if (start == -1)
                    break;

                int end = GetEnd(start);
                if (end == -1)
                    break;

                index = end + 1;
                ranges.Enqueue((start, end));
            }

            if (ranges.Count == 0)
                return items;

            List<string> result = new();
            for (int i = 0; i < items.Length; i++)
            {
                if (ranges.Count > 0)
                {
                    (int start, int end) = ranges.Peek();
                    if (i == start)
                    {
                        result.Add(string.Join(' ', items[start..(end + 1)]));
                        ranges.Dequeue();
                        i = end;
                        continue;
                    }
                }

                result.Add(items[i]);
            }
            return result.ToArray();

            int GetStart(int index)
            {
                for (int i = index; i < items.Length; i++)
                {
                    if (items[i].StartsWith('"'))
                        return i;
                }
                return -1;
            }

            int GetEnd(int index)
            {
                for (int i = index; i < items.Length; i++)
                {
                    if (items[i].EndsWith('"') && (items[i].Length <= 1 || items[i][^2] != '\\'))
                        return i;
                }
                return -1;
            }
        }

        private enum ParseStage
        {
            Key,

            Argument,

            Overflow
        }
    }
}
