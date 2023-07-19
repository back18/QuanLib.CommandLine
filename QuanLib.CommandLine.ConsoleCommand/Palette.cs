using QuanLib.ConsoleUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.ConsoleCommand
{
    public class Palette
    {
        public Palette()
        {
            KeyColor = TextColor.FormatForegroundColor(ConsoleColor.Blue);
            ArgumentColor = TextColor.FormatForegroundColor(ConsoleColor.Green);
            PromptColor = TextColor.FormatForegroundColor(ConsoleColor.DarkGray);
            WarnColor = TextColor.FormatForegroundColor(ConsoleColor.Yellow);
            ErrorColor = TextColor.FormatForegroundColor(ConsoleColor.Red);
            OverflowColor = TextColor.FormatForegroundColor(ConsoleColor.Red);
            ItemColor = TextColor.FormatForegroundColor(ConsoleColor.Magenta);
            SelectBackgroundColor = ConsoleColor.White;
        }

        public TextColor KeyColor { get; set; }

        public TextColor ArgumentColor { get; set; }

        public TextColor PromptColor { get; set; }

        public TextColor WarnColor { get; set; }

        public TextColor ErrorColor { get; set; }

        public TextColor OverflowColor { get; set; }

        public TextColor ItemColor { get; set; }

        public ConsoleColor SelectBackgroundColor { get; set; }
    }
}
