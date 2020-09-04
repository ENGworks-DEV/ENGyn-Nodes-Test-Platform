using ENGyn.NodesTestPlatform.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENGyn.NodesTestPlatform
{
    public class Runner : IRunnerService
    {
        private readonly string prompt = "ENGyn >";


        // Main Execution Loop
        public void Run()
        {
            while (true)
            {
                try
                {
                    var consoleInput = ReadFromConsole();
                    WriteToConsole(consoleInput);
                }
                catch (Exception ex)
                {
                    WriteToConsole(ex.Message);
                }
            }
        }

        /// <summary>
        /// Excecutes the command entered by the user
        /// </summary>
        /// <param name="command">Command to execute</param>
        /// <returns></returns>
        public string Execute(string command)
        {
            return string.Format("Executed the {0} command", command);
        }

        /// <summary>
        /// Reads a message from the console.
        /// </summary>
        /// <param name="promptMessage">String that contains a message entered on the terminal</param>
        /// <returns></returns>
        public string ReadFromConsole(string promptMessage = "")
        {
            // Show a prompt, and get input:
            Console.Write($"{prompt} {promptMessage}");
            return Console.ReadLine();
        }

        /// <summary>
        /// Write a message to prompt on the terminal
        /// </summary>
        /// <param name="message">Message to write on terminal</param>
        /// <param name="color">Foreground color of the message</param>
        public void WriteToConsole(string message, ConsoleColor color = ConsoleColor.White)
        {
            if (message.Length > 0)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }
}
