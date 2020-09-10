using ENGyn.NodesTestPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ENGyn.NodesTestPlatform.Core
{
    /// <summary>
    /// Contains all the methods used to validate Command inputs provided by the user
    /// </summary>
    public class CommandValidation
    {
        /// <summary>
        /// Validates if given a commandsLibrary dictionary, contains the provided libraryName and command name
        /// </summary>
        /// <typeparam name="T">Represents the Type of the "Value" item on internal dictionary</typeparam>
        /// <param name="commandsLibrary">Dictionary conaining all the available libraries with commands to execute</param>
        /// <param name="libraryName">Selected library name</param>
        /// <param name="commandName">Command name to execute</param>
        /// <returns>Boolean result true if valid otherwise false</returns>
        public bool ValidateLibraryCommand<T>(Dictionary<string, Dictionary<string, T>> commandsLibrary, string libraryName, string commandName)
        {
            var validationResult = true;
            validationResult &= commandsLibrary.ContainsKey(libraryName);
            if (validationResult)
            {
                var commandMethod = commandsLibrary[libraryName];
                validationResult &= commandMethod.ContainsKey(commandName);
            }
            return validationResult;
        }
        
        /// <summary>
        /// Validates if the provided arguments count math the number of required arguments
        /// </summary>
        /// <param name="arguments">List of command arguments</param>
        /// <param name="providedArgumentCount">provided arguments count present on users Command</param>
        /// <returns>Bolean flag true if valid otherwise false</returns>
        public bool ValidateProvidedArgumentsCount(IList<ParameterInfo> arguments, int providedArgumentCount)
        {
            var requiredArguments = arguments.Where(arg => arg.IsOptional == false);
            var optionalArguments = arguments.Where(arg => arg.IsOptional == true);
            int requiredCount = requiredArguments.Count();
            int optionalCount = optionalArguments.Count();
            return requiredCount > providedArgumentCount;
        }
    }
}
