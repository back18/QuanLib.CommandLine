using QuanLib.CommandLine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.ConsoleCommand
{
    public class CommandPanel
    {
        public CommandPanel(CommandObject obj)
        {
            _obj = obj;
        }

        private CommandObject _obj;


    }
}
