using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExaCMD
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Tools.DisplayCurrentDirectoryInConsole();
                PerformOneCommand();
            }
        }

        static void PerformOneCommand()
        {
            ConsoleManager ConsoleManager = new ConsoleManager();
            ConsoleKeyInfo keyInfo;
            StringBuilder inputBuilder;
            CommandManager CommandManager = new CommandManager();

            List<string> commandHistory = new List<string>();
            int indexHistory = 0;
            int inputPosition = 0;
            inputBuilder = new StringBuilder();
            do
            {
                keyInfo = Console.ReadKey(true);
                if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0 && keyInfo.Key == ConsoleKey.Spacebar)
                {
                    ConsoleManager.HandleAutoCompletion(CommandManager.DictCommand.Keys.ToList(), inputBuilder);
                    inputPosition = 0;
                    inputBuilder = new StringBuilder();
                }

                if (keyInfo.Key == ConsoleKey.Backspace)
                    ConsoleManager.HandleBackspace(inputBuilder, ref inputPosition);
                else if (keyInfo.Key == ConsoleKey.UpArrow)
                    ConsoleManager.HandleUpArrow(commandHistory, ref indexHistory, ref inputBuilder);
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                    ConsoleManager.HandleDownArrow(commandHistory, ref indexHistory, ref inputBuilder);

                else
                    ConsoleManager.HandleKeyChar(keyInfo.KeyChar, ref inputBuilder, ref inputPosition);


            } while (keyInfo.Key != ConsoleKey.Enter);


            Console.Write(Environment.NewLine);
            if (inputBuilder.Length > 0)
            {
                commandHistory.Add(inputBuilder.ToString());
                indexHistory = commandHistory.Count();
            }

            CommandManager.ReplaceCommand(ref inputBuilder);
            CommandManager.PerformCommand(inputBuilder);
        }

    }
}
