using ENGyn.NodesTestPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ENGyn.NodesTestPlatform.Core
{
    public class ConsoleCommand
    {
        private readonly string _regexStringsPreservingQuotes = "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        private readonly string _defaultLibary = "DefaultCommands";
        private Command command;

        public ConsoleCommand(string input)
        {
            string[] inputArray = Regex.Split(input, _regexStringsPreservingQuotes);
            List<string> arguments = new List<string>();
            command = new Command();

            for (int index = 0; index < inputArray.Length; index++)
            {
                if (index == 0)
                {
                    var commands = ParseDefaultCommand(inputArray[index]);
                    command.Name = commands.Item1;
                    command.LibraryClassName = commands.Item2;
                }
                else
                {
                    string argument = ParseArguments(inputArray[index]);
                    arguments.Add(argument);
                }
            }

            command.Arguments = arguments;
        }

        public Command GetCommand()
        {
            return command;
        }

        /// <summary>
        /// Analyze the user input and search defaults commands or third party commands
        /// </summary>
        private (string, string) ParseDefaultCommand(string inputCommand)
        {
            // Default Settings
            string commandName = inputCommand;
            string libraryName = _defaultLibary;

            // Validating if user inputs external command
            string[] extcommand = inputCommand.Split('.');
            if (extcommand.Length > 1)
            {
                commandName = extcommand[0];
                libraryName = extcommand[1];
            }

            return (commandName, libraryName);
        }

        /// <summary>
        /// Check the input string and look for the entered arguments.This includes quoted arguments
        /// </summary>
        /// <param name="inputArgument">text string containing the arguments to execute a command</param>
        /// <returns></returns>
        private string ParseArguments(string inputArgument)
        {
            string argument = inputArgument;

            // Is the argument a quoted text string?
            var regex = new Regex("\"(.*?)\"", RegexOptions.Singleline);
            var match = regex.Match(inputArgument);

            if (match.Captures.Count > 0)
            {
                // Get the unquoted text:
                var captureQuotedText = new Regex("[^\"]*[^\"]");
                var quoted = captureQuotedText.Match(match.Captures[0].Value);
                argument = quoted.Captures[0].Value;
            }
            return argument;
        }
    }
}
