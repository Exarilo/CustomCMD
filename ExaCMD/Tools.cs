using Colors.Net.StringColorExtensions;
using Colors.Net;
using System.IO;
using System;

namespace ExaCMD
{
    public static class Tools
    {
        public static void DisplayCurrentDirectoryInConsole()
        {
            string currentDirectory = Directory.GetCurrentDirectory()+"> ";
            ColoredConsole.Write($"{currentDirectory}".DarkCyan());
            Console.SetCursorPosition(currentDirectory.Length, Console.CursorTop);
        }
    }
}
