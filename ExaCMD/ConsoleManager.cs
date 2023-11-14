using Colors.Net;
using Colors.Net.StringColorExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExaCMD
{
    public class ConsoleManager
    {
        public ConsoleManager() { }
        public void HandleKeyPress(ConsoleKeyInfo keyInfo)
        {
            if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0 && keyInfo.Key == ConsoleKey.Spacebar)
                HandleAutoCompletion(null, null);
            else if (keyInfo.Key == ConsoleKey.Enter)
                HandleEnter();
            /*
            else if (keyInfo.Key == ConsoleKey.Backspace)
                HandleBackspace(null);
            else
                HandleKeyChar();*/
        }

        public void HandleAutoCompletion(List<string> availableCommands, StringBuilder inputBuilder)
        {
            string partialInput = inputBuilder.ToString();
            List<string> suggestions = availableCommands
                .Where(command => command.StartsWith(partialInput, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (suggestions.Count > 0)
            {
                Console.WriteLine(); // Nouvelle ligne pour afficher les suggestions
                foreach (string suggestion in suggestions)
                {
                    Console.WriteLine(suggestion);
                }
                Tools.DisplayCurrentDirectoryInConsole();
            }
            else
            {
                Console.WriteLine();
                Tools.DisplayCurrentDirectoryInConsole();
            }
        }

        public void HandleEnter()
        {
            Console.Write(Environment.NewLine);
        }

        public void HandleUpArrow(List<string> history, ref int index, ref StringBuilder input)
        {
            var currentCusorPosition = Console.CursorLeft;
            if (history.Count <= 0)
                return;

            if (index > 0)
                index--;

            Console.SetCursorPosition(0, Console.CursorTop);

            Tools.DisplayCurrentDirectoryInConsole();
            Console.Write(history[index]);

            input.Clear();
            input.Append(history[index]);

            Console.SetCursorPosition(currentCusorPosition, Console.CursorTop);
        }

        public void HandleDownArrow(List<string> history, ref int index, ref StringBuilder input)
        {
            var currentCusorPosition = Console.CursorLeft;
            if (history.Count <= 0)
                return;

            if (index < history.Count - 1)
                index++;
            else
                index = history.Count - 1;

            Console.SetCursorPosition(0, Console.CursorTop);

            Tools.DisplayCurrentDirectoryInConsole();
            Console.Write(history[index]);

            input.Clear();
            input.Append(history[index]);

            Console.SetCursorPosition(currentCusorPosition, Console.CursorTop);
        }

        public void HandleBackspace(StringBuilder inputBuilder, ref int inputPosition)
        {
            if (inputPosition > 0)
            {
                inputBuilder.Remove(inputPosition - 1, 1);
                inputPosition--;
                Console.Write("\b \b"); // Effacer le caractère précédent
            }
        }

        public void HandleKeyChar(char keyChar, ref StringBuilder inputBuilder, ref int inputPosition)
        {
            try
            {
                inputBuilder.Insert(inputPosition, keyChar);
                inputPosition++;
                Console.Write(keyChar);
            }
            catch (Exception e) { }
        }
    }
}
