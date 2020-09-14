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

        /// <summary>
        /// It performs the execution and validation of a console command provided by the user
        /// </summary>
        /// <param name="userCommand">Command object provided by the user</param>
        /// <returns>Execution result in form of string</returns>
        /// <exception cref="ArgumentException">Thrown when provided command doesn't exists or when arguments doesn't match the command signature</exception>
        public string Execute(Command userCommand)
        {
            bool validCommandLibraryFlag = _commandValidation.ValidateLibraryCommand(_commandLibaries, userCommand.LibraryClassName, userCommand.Name);

            if (!validCommandLibraryFlag)
            {
                throw new ArgumentException("Unknown command. Use command 'help' to see commands info");
            }

            var methodArgumentValueList = new List<object>();
            var commandArgumentList = _commandLibaries[userCommand.LibraryClassName][userCommand.Name];
            var validArgsCountFlag = _commandValidation.ValidateProvidedArgumentsCount(commandArgumentList, userCommand.Arguments.Count(), out string validationMessage);

            if (!validArgsCountFlag)
            {
                throw new ArgumentException(validationMessage);
            }

            if (commandArgumentList.Count > 0)
            {
                foreach (var argument in commandArgumentList)
                {
                    methodArgumentValueList.Add(argument.DefaultValue);
                }

                for (int index = 0; index < userCommand.Arguments.Count(); index++)
                {
                    var methodArgument = commandArgumentList.ElementAt(index);
                    var argumentType = methodArgument.ParameterType;
                    var parsedValue = ParserHelper.ParseType(argumentType, userCommand.Arguments.ElementAt(index));
                    methodArgumentValueList.RemoveAt(index);
                    methodArgumentValueList.Insert(index, parsedValue);
                }
            }

            return _reflectionHandler.InvokeConsoleCommand(userCommand, methodArgumentValueList.ToArray());
        }
    }
}
