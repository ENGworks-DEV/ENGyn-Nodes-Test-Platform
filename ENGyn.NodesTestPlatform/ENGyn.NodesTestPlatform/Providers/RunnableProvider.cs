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
        private string _art;
        private CommandsLoader _commandsLoader;
        private CommandValidation _commandValidation;
        private Dictionary<string, Dictionary<string, IList<ParameterInfo>>> _commandLibaries;

        public RunnableProvider()
        {
            _commandValidation = new CommandValidation();
            _commandsLoader = new CommandsLoader();
            _commandLibaries = _commandsLoader.LoadAndGetLibraries();
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
                    Execute(consoleCommand.GetCommand());
                    ConsolePrompt.WriteToConsole(consoleInput);
                }
                catch (Exception ex)
                {
                    ConsolePrompt.WriteToConsole(ex.Message, ConsoleColor.Yellow);
                }
            }
        }

        public string Execute(Command command)
        {
            // Validate command
            bool validCommandLibraryFlag = _commandValidation.ValidateLibraryCommand(_commandLibaries, command.LibraryClassName, command.Name);

            if (validCommandLibraryFlag)
            {
                // command arguments list
                var argumentValueList = new List<object>();
                var argumentList = _commandLibaries[command.LibraryClassName][command.Name];

                // Validate argument count
                var validArgsCountFlag = _commandValidation.ValidateProvidedArgumentsCount(argumentList, command.Arguments.Count());
                
                // Check for invalid argument count
                if(!validArgsCountFlag)
                {
                    throw new ArgumentException("Missing required argument. Use command 'help' to see commands info");
                }

                // If command contains arguments
                if (argumentList.Count > 0)
                {
                    foreach (var argument in argumentList)
                    {
                        argumentValueList.Add(argument.DefaultValue);
                    }
                    
                    for(int index = 0; index < command.Arguments.Count(); index++ )
                    {
                        var commandArgument = argumentList.ElementAt(index);
                        var argumentType = commandArgument.ParameterType;
                        
                        try
                        {
                            // TODO argument reasign and Convertion to right parameter type
                            // Fun fact: Create a generic Tryparse to convert from string to any primitive type of C#
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            } 
            else
            {
                throw new ArgumentException(@"Unknown command. Use command 'help' to see commands info");
            }

            // Getting params based on their characteristics
            var requiredParams = paramInfoList.Where(p => p.IsOptional == false);
            var optinalParams = paramInfoList.Where(p => p.IsOptional == true);

            return "TODO finalize execute";
        }
    }
}
