using ENGyn.NodesTestPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ENGyn.NodesTestPlatform.Core
{
    public class CommandHandler
    {
        private readonly string _commandsNamespace = "ENGyn.NodesTestPlatform.Commands";

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
