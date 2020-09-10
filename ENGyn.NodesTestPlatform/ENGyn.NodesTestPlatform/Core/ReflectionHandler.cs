using ENGyn.NodesTestPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ENGyn.NodesTestPlatform.Core
{
    public class ReflectionHandler
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
    }
}
