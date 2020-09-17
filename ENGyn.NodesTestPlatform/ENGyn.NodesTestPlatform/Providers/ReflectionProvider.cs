using ENGyn.NodesTestPlatform.Models;
using ENGyn.NodesTestPlatform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ENGyn.NodesTestPlatform.Providers
{
    public class ReflectionHandler : IReflectionHandlerService
    {
        private readonly string _commandsNamespace = "ENGyn.NodesTestPlatform.Commands";
        private readonly Dictionary<string, Dictionary<string, IList<ParameterInfo>>> _commandLibaries;
        
        public ReflectionHandler()
        {
            _commandLibaries = new Dictionary<string, Dictionary<string, IList<ParameterInfo>>>();
        }

        /// <summary>
        /// Initialize the loading of the assemblies and locate the command methods available to execute
        /// </summary>
        /// <returns>A dictionary of class and method libraries</returns>
        public Dictionary<string, Dictionary<string, IList<ParameterInfo>>> LoadAndGetLibraries()
        {
            var types = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == _commandsNamespace
                        select t;
            var commandClasses = types.ToList();

            foreach (var commandClass in commandClasses)
            {
                var methods = commandClass.GetMethods(BindingFlags.Static | BindingFlags.Public);
                var methodDictionary = new Dictionary<string, IList<ParameterInfo>>();
                foreach (var method in methods)
                {
                    string commandName = method.Name;
                    methodDictionary.Add(commandName, method.GetParameters());
                }

                _commandLibaries.Add(commandClass.Name, methodDictionary);
            }
            return _commandLibaries;
        }

        /// <summary>
        /// Invoke an existing method in a loaded class using reflecion  
        /// </summary>
        /// <param name="userCommand">Command object provided by user input</param>
        /// <param name="args">Command arguments</param>
        /// <returns>The result of execution as string</returns>
        public string InvokeConsoleCommand(Command userCommand, object[] args)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type commandLibraryClass = assembly.GetType($"{_commandsNamespace}.{userCommand.LibraryClassName}");

            try
            {
                var result = commandLibraryClass.InvokeMember(
                    userCommand.Name,
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod,
                    null, null,
                    args
                );

                return (result != null) ? result.ToString() : string.Empty;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
