using QuanLib.CommandLine.Objects;
using QuanLib.ConsoleUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.ConsoleCommand
{
    public class KeyPromptComboBox : ConsoleComboBox
    {
        public KeyPromptComboBox(KeyNode node) : base(node?.SubNodes.Select(s => s.Key) ?? throw new ArgumentNullException(nameof(node)))
        {
            Node = node;
        }

        public KeyNode Node { get; }
    }
}
