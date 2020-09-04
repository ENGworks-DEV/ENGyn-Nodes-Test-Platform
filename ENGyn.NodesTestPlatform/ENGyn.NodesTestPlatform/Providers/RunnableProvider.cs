using ENGyn.NodesTestPlatform.Core;
using ENGyn.NodesTestPlatform.Models;
using ENGyn.NodesTestPlatform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENGyn.NodesTestPlatform.Providers
{
    public class RunnableProvider : IRunnableService
    {
        private readonly string promptMark = "eng > ";

        public void Run()
        {
            while (true)
            {
                try
                {
                    var consoleInput = ReadFromConsole();
                    ConsoleCommand consoleCommand = new ConsoleCommand(consoleInput);
                    Execute(consoleCommand.command);
                    WriteToConsole(consoleInput);
                }
                catch (Exception ex)
                {
                    WriteToConsole(ex.Message, ConsoleColor.Yellow);
                }
            }
        }

        public string ReadFromConsole(string promptMessage = "")
        {
            // Show a prompt, and get input:
            Console.Write($"{promptMark} {promptMessage}");
            return Console.ReadLine();
        }

        public void WriteToConsole(string message, ConsoleColor color = ConsoleColor.White)
        {
            if (message.Length > 0)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        public string Execute(Command command)
        {
            return string.Format("Executed the {0} command", command.Name);
        }
    }
}
