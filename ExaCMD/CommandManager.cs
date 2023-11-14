using Colors.Net.StringColorExtensions;
using Colors.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ExaCMD
{
    public class CommandManager
    {
        public delegate void CustomCommand(string arg);
        public static Dictionary<string, CustomCommand> DictCommand { get; set; } = new Dictionary<string, CustomCommand>()
        {
            { "cls", ClearConsole },
            { "clear", ClearConsole },
            { "clean", ClearConsole },
            { "cd", ChangeDirectory },
        };

        public static Dictionary<string, string> DictAlias { get; set; } = new Dictionary<string, string>()
        {
            { "clear", "cls" },
            { "clean", "cls"},
            { "ls", "dir /b" },
        };


        private bool CustomCommandExist(string input, out CustomCommand command, out string arg)
        {
            foreach (var commandKey in DictCommand.Keys)
            {
                if (input.StartsWith(commandKey, StringComparison.OrdinalIgnoreCase))
                {
                    command = DictCommand[commandKey];
                    arg = input.Substring(commandKey.Length).Trim();
                    return true;
                }
            }

            command = null;
            arg = null;
            return false;
        }

        public static void ReplaceCommand(ref StringBuilder input)
        {
            foreach (var aliasPair in DictAlias)
            {
                string aliasKey = aliasPair.Key;
                string aliasValue = aliasPair.Value;

                if (input.ToString().StartsWith(aliasKey, StringComparison.OrdinalIgnoreCase))
                {
                    input.Replace(aliasKey, aliasValue, 0, aliasKey.Length);
                    return;
                }
            }
        }



        public void PerformCommand(StringBuilder input)
        {
            if (CustomCommandExist(input.ToString(), out CustomCommand command, out string arg))
                command?.Invoke(arg);
            else
                ExecuteCommand(input.ToString());
        }


        private static void ExecuteCommand(string input)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/C {input}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    ColoredConsole.WriteLine($"{e.Data}".DarkGreen());
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    ColoredConsole.WriteLine($"{e.Data}".DarkRed());
                }
            };

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();
        }


        private static void ClearConsole(string arg)
        {
            Console.Clear();
        }
        private static void ChangeDirectory(string arg)
        {
            try
            {
                Directory.SetCurrentDirectory(arg);
            }
            catch (Exception ex)
            {
                ColoredConsole.Write($"Error changing directory: {ex.Message}".DarkRed());
                Console.Write(Environment.NewLine);
            }
        }
    }
}
