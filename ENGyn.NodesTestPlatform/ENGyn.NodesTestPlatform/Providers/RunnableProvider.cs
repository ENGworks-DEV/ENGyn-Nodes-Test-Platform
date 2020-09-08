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
        private Dictionary<string, Dictionary<string, IList<ParameterInfo>>> _commandLibaries;
        private CommandsLoader _commandsLoader;

        public RunnableProvider()
        {
            _commandsLoader = new CommandsLoader();
            _commandLibaries = _commandsLoader.LoadAndGetLibraries();
            _art = DesignArt.CreateArt();
        }

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
            return string.Format("Executed the {0} command", command.Name);
        }
    }
}
