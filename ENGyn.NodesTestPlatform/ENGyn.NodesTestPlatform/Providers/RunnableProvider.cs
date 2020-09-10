using ENGyn.NodesTestPlatform.Core;
using ENGyn.NodesTestPlatform.Models;
using ENGyn.NodesTestPlatform.Services;
using ENGyn.NodesTestPlatform.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ENGyn.NodesTestPlatform.Providers
{
    public class RunnableProvider : IRunnableService
    {
        private readonly string _art;
        private readonly ReflectionHandler _reflectionHandler;
        private readonly CommandValidation _commandValidation;
        private readonly CommandHandler _commandHandler;
        private readonly Dictionary<string, Dictionary<string, IList<ParameterInfo>>> _commandLibaries;

        public RunnableProvider()
        {
            _commandValidation = new CommandValidation();
            _reflectionHandler = new ReflectionHandler();
            _commandHandler = new CommandHandler();
            _commandLibaries = _reflectionHandler.LoadAndGetLibraries();
            _art = DesignArt.CreateArt();
        }

        /// <summary>
        /// Main execution loop of the application
        /// </summary>
        public void Run()
        {
            ConsolePrompt.WriteToConsole(_art, ConsoleColor.Green);

            while (true)
            {
                try
                {
                    var consoleInput = ConsolePrompt.ReadFromConsole();
                    ConsoleCommand consoleCommand = new ConsoleCommand(consoleInput);
                    var executionResult = Execute(consoleCommand.GetCommand());
                    ConsolePrompt.WriteToConsole(executionResult);
                }
                catch (Exception ex)
                {
                    ConsolePrompt.WriteToConsole(ex.Message, ConsoleColor.Yellow);
                }
            }
        }

        public string Execute(Command userCommand)
        {
            // Validate command
            bool validCommandLibraryFlag = _commandValidation.ValidateLibraryCommand(_commandLibaries, userCommand.LibraryClassName, userCommand.Name);

            if (!validCommandLibraryFlag)
            {
                throw new ArgumentException("Unknown command. Use command 'help' to see commands info");
            }

            // This list will store the argument values that are going to be used on the command method
            var methodArgumentValueList = new List<object>();

            // This variable store the arguments present on the method that represent the command to be executed
            var commandArgumentList = _commandLibaries[userCommand.LibraryClassName][userCommand.Name];

            // Validate argument count
            var validArgsCountFlag = _commandValidation.ValidateProvidedArgumentsCount(commandArgumentList, userCommand.Arguments.Count(), out string validationMessage);

            // Check for invalid argument count
            if (!validArgsCountFlag)
            {
                throw new ArgumentException(validationMessage);
            }

            // If command contains arguments
            if (commandArgumentList.Count > 0)
            {
                // Adding default values to each item of list (Each item represent an argument of the command method)
                foreach (var argument in commandArgumentList)
                {
                    methodArgumentValueList.Add(argument.DefaultValue);
                }

                // Walk through user command to extract arguments and subsitue previous generated default values on their respective index
                for (int index = 0; index < userCommand.Arguments.Count(); index++)
                {
                    var methodArgument = commandArgumentList.ElementAt(index);
                    var argumentType = methodArgument.ParameterType;
                    var parsedValue = ParserHelper.ParseType(argumentType, userCommand.Arguments.ElementAt(index));
                    methodArgumentValueList.RemoveAt(index);
                    methodArgumentValueList.Insert(index, parsedValue);
                }
            }

            // Invoke Console Command
            return _commandHandler.InvokeConsoleCommand(userCommand, methodArgumentValueList.ToArray());
        }
    }
}
