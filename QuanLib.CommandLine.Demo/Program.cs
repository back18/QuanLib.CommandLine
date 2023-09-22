using QuanLib.CommandLine.ConsoleTerminal;

namespace QuanLib.CommandLine.Demo
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            CommandSystem commandSystem = new(new(Level.Root));
            commandSystem.Pool.AddCommand(new(new("screen lsit"), CommandFunc.GetFunc(Add)));
            commandSystem.Pool.AddCommand(new(new("process lsit"), CommandFunc.GetFunc(Add)));
            commandSystem.Pool.AddCommand(new(new("form lsit"), CommandFunc.GetFunc(Add)));
            commandSystem.Start();
        }

        private static int Add(int a, int b)
        { return a + b; }
    }
}