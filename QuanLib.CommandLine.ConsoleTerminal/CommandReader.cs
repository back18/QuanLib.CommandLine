using QuanLib.CommandLine.Exceptions;
using QuanLib.CommandLine.Objects;
using QuanLib.ConsoleUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.ConsoleTerminal
{
    public class CommandReader : ICommandReader
    {
        public CommandReader(CommandParser parser)
        {
            ArgumentNullException.ThrowIfNull(parser, nameof(parser));

            _parser = parser;
            _buffer = new();
            _eraser = new(Console.CursorTop);
            _comboBox = null;

            EnablePrompt = true;
            Palette = new();
        }

        private bool _reading;

        private readonly CommandParser _parser;

        private readonly ConsoleBuffer _buffer;

        private readonly ConsoleEraser _eraser;

        private ConsoleComboBox? _comboBox;

        public bool Reading => _reading;

        /// <summary>
        /// 是否启用提示词功能
        /// </summary>
        public bool EnablePrompt { get; set; }

        /// <summary>
        /// 调色板
        /// </summary>
        public Palette Palette { get; }

        public CommandItems ReadCommand()
        {
            _reading = true;

            if (Console.CursorLeft > 0)
                Console.WriteLine();

            _buffer.StartCursor = new(1, Console.CursorTop);
            _buffer.StartCursor.SetToConsole();
            _buffer.Clear();
            _eraser.Reset(Console.CursorTop);
            CommandItems items;

            while (true)
            {
                _buffer.RefreshCursors();
                items = _parser.ToItems(_buffer.ToString());
                Command? command = _parser.ToCommand(items);
                Update(items, command, EnablePrompt);

                ConsoleKeyInfo keyinfo;
                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        keyinfo = Console.ReadKey(true);
                        break;
                    }
                    else if (!_reading)
                    {
                        throw new CommandReaderException("命令读取器已停止");
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }

                if (keyinfo.Modifiers == ConsoleModifiers.Control)
                {
                    int userItemIndex = items.ItemIndexOf(_buffer.UserIndex);
                    switch (keyinfo.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            if (userItemIndex > 0)
                                _buffer.MoveCursor(items[userItemIndex - 1].StartIndex);
                            break;
                        case ConsoleKey.RightArrow:
                            if (userItemIndex < items.Count - 1)
                                _buffer.MoveCursor(items[userItemIndex + 1].StartIndex);
                            break;
                    }
                }
                else
                {
                    switch (keyinfo.Key)
                    {
                        case ConsoleKey.Enter:
                            Update(items, command, false);
                            Console.WriteLine();
                            goto ok;
                        case ConsoleKey.Escape:
                            break;
                        case ConsoleKey.Tab:
                            if (_comboBox is KeyPromptComboBox keyPrompt)
                            {
                                Item item = items[keyPrompt.Node.Level];
                                _buffer.MoveCursor(item.StartIndex + item.Text.Length);
                                _buffer.Append(keyPrompt.Prompt);
                            }
                            break;
                        case ConsoleKey.Backspace:
                            _buffer.Remove();
                            break;
                        case ConsoleKey.UpArrow:
                            _comboBox?.Prev();
                            break;
                        case ConsoleKey.DownArrow:
                            _comboBox?.Next();
                            break;
                        case ConsoleKey.LeftArrow:
                            _buffer.OffsetCursor(-1);
                            break;
                        case ConsoleKey.RightArrow:
                            _buffer.OffsetCursor(1);
                            break;
                        default:
                            if (keyinfo.KeyChar == ' ' || !char.IsWhiteSpace(keyinfo.KeyChar))
                                _buffer.Append(keyinfo.KeyChar);
                            break;
                    }
                }
            }
            ok:
            _reading = false;
            return items;
        }

        public void StopRead()
        {
            _reading = false;
        }

        private void Update(CommandItems items, Command? command, bool enablePrompt)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            CursorPosition cursor = CursorPosition.Now;
            CursorPosition start = new(0, _eraser.StartLine);
            start.SetToConsole();

            _eraser.Erase();
            _eraser.Reset();
            start.SetToConsole();

            List<ConsoleText> commandTexts = new();
            List<ConsoleText> candidateListTexts = new();
            List<ConsoleText> argumentTexts = new();
            int errorKeyTextIndex = -1;

            foreach (Item item in items)
            {
                switch (item.Type)
                {
                    case ItemType.Key:
                        commandTexts.Add(new(item.Text, Palette.KeyColor));
                        break;
                    case ItemType.Argument:
                        commandTexts.Add(new(item.Text, Palette.ArgumentColor));
                        break;
                    case ItemType.KeyError:
                        commandTexts.Add(new(item.Text, Palette.ErrorColor));
                        errorKeyTextIndex = commandTexts.Count - 1;
                        break;
                    case ItemType.ArgumentError:
                        commandTexts.Add(new(item.Text, Palette.ErrorColor));
                        break;
                    case ItemType.Overflow:
                        commandTexts.Add(new(item.Text, Palette.OverflowColor));
                        break;
                    default:
                        throw new NotImplementedException();
                }
                commandTexts.Add(ConsoleText.Space);
            }

            if (!enablePrompt)
                goto print;

            int userItemIndex = items.ItemIndexOf(_buffer.UserIndex);
            Item? userItem = userItemIndex == -1 ? null : items[userItemIndex];

            if (userItem is null)
                goto print;

            switch (userItem.Type)
            {
                case ItemType.KeyError:
                    KeyNode node = (items[userItemIndex] as KeyErrorItem)?.BaseNode ?? throw new InvalidOperationException();
                    if (_comboBox is not KeyPromptComboBox keyPromptComboBox || keyPromptComboBox.Node != node)
                        _comboBox = new KeyPromptComboBox(node);
                    _comboBox.Update(userItem.Value);
                    if (_comboBox.SelectItem is not null)
                    {
                        commandTexts.Insert(errorKeyTextIndex + 1, new(_comboBox.Prompt, Palette.PromptColor));
                        CursorPosition userItemPos = _buffer.GetCursor(userItem.StartIndex);
                        string empty = new(' ', userItemPos.CursorLeft);
                        string[] showItems = _comboBox.GetShowItems();
                        for (int i = 0; i < showItems.Length; i++)
                        {
                            candidateListTexts.Add(new(empty));
                            candidateListTexts.Add(new(_comboBox.Text, Palette.ItemColor));
                            candidateListTexts.Add(new(showItems[i][_comboBox.Text.Length..] + "\n", new(Palette.ItemColor.BackgroundColor, Palette.PromptColor.ForegroundColor)));
                            if (i == _comboBox.DisplaySelectIndex)
                            {
                                candidateListTexts[^1].Color = candidateListTexts[^1].Color.SetBackgroundColor(Palette.SelectBackgroundColor);
                                candidateListTexts[^2].Color = candidateListTexts[^2].Color.SetBackgroundColor(Palette.SelectBackgroundColor);
                            }
                        }
                        _eraser.AddLineNumber(_comboBox.ActualShowCount);
                    }
                    break;
                case ItemType.Argument:
                case ItemType.ArgumentError:
                    if (command is null)
                        throw new InvalidOperationException();
                    int argIndex = items.ArgumentIndexOf();
                    if (command.Func.Arguments.Count > 0)
                    {
                        argumentTexts.Add(new("["));
                        for (int i = 0; i < command.Func.Arguments.Count; i++)
                        {
                            string text = command.Func.Arguments[i].Type.ToString();
                            if (argIndex + i < items.Count)
                            {
                                switch (items[argIndex + i].Type)
                                {
                                    case ItemType.Argument:
                                        argumentTexts.Add(new(text, Palette.ArgumentColor));
                                        break;
                                    case ItemType.ArgumentError:
                                        ArgumentErrorItem errItem = (ArgumentErrorItem)items[argIndex + i];
                                        if (errItem.Exception is CommandArgumentOutOfRangeException)
                                            argumentTexts.Add(new(text, Palette.WarnColor));
                                        else
                                            argumentTexts.Add(new(text, Palette.ErrorColor));
                                        break;
                                    case ItemType.Overflow:
                                        argumentTexts.Add(new(text, Palette.OverflowColor));
                                        break;
                                }
                                if (argIndex + i == userItemIndex)
                                    argumentTexts[^1].Color = argumentTexts[^1].Color.SetBackgroundColor(Palette.SelectBackgroundColor);
                            }
                            else
                            {
                                argumentTexts.Add(new(text, Palette.ErrorColor));
                            }
                            argumentTexts.Add(new(", "));
                        }
                        argumentTexts.Add(new("\b\b]"));
                        CommandArgument argument = command.Func.Arguments[userItemIndex - argIndex];
                        argumentTexts.Add(new("\n[索引] " + argument.Index));
                        argumentTexts.Add(new("\n[类型] " + argument.Type));
                        argumentTexts.Add(new("\n[名称] " + argument.Name));
                        argumentTexts.Add(new("\n[描述] " + argument.Description));
                        argumentTexts.Add(new("\n[最小值] " + argument.Range?.MinValue));
                        argumentTexts.Add(new("\n[最大值] " + argument.Range?.MaxValue));
                        _eraser.AddLineNumber(7);
                    }
                    break;
            }

            _eraser.AddLineNumber(_buffer.LineCount);

            print:
            Console.Write('>');
            ConsoleText.WriteAll(commandTexts.ToArray());
            Console.WriteLine();
            ConsoleText.WriteAll(candidateListTexts.ToArray());
            ConsoleText.WriteAll(argumentTexts.ToArray());

            cursor.SetToConsole();
        }
    }
}
