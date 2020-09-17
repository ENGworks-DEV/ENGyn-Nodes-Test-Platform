using ENGyn.NodesTestPlatform.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ENGyn.NodesTestPlatform.Core
{
    public class ConsoleCommand
    {
        private readonly string _regexStringsPreservingQuotes = "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        private readonly string _defaultLibary = "DefaultCommands";
        private readonly Command command;

        /// <summary>
        /// Creates a new console command instance to parse user input and generate a command object well formed
        /// </summary>
        /// <param name="input"></param>
        public ConsoleCommand(string input)
        {
            // Regular expresion that split string by whitespaces, respecting quoted text ie: "quoted text"
            // quoted text will be passed as an entire element
            string[] inputArray = Regex.Split(input, _regexStringsPreservingQuotes);
            List<string> arguments = new List<string>();
            command = new Command();

            for (int index = 0; index < inputArray.Length; index++)
            {
                // First element is the command
                if (index == 0)
                {
                    var commands = ParseDefaultCommand(inputArray[index]);
                    command.Name = commands.Item1;
                    command.LibraryClassName = commands.Item2;
                }
                // Other elements are arguments
                else
                {
                    string argument = ParseArguments(inputArray[index]);
                    arguments.Add(argument);
                }
            }

            command.Arguments = arguments;
        }

        /// <summary>
        /// Obtain a command object with the input info provided by the user
        /// </summary>
        /// <returns>Command object with parsed user input</returns>
        public Command GetCommand()
        {
            return command;
        }

        /// <summary>
        /// Analyze the user input and search defaults commands or third party commands using dot notation "Library.CommandName"
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
