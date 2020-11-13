using CommandLine;
using ENGyn.NodesTestPlatform.Commands;
using ENGyn.NodesTestPlatform.Services;
using ENGyn.NodesTestPlatform.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ENGyn.NodesTestPlatform.Providers
{
    public class RunnableProvider : IRunnableService
    {
        private readonly IReflectionService _reflectionService;
        private readonly ICommandService _commandService;
        private readonly IList<Type> _CommandVerbsList;

        public RunnableProvider(IReflectionService reflectionService, ICommandService commandService)
        {
            _reflectionService = reflectionService;
            _commandService = commandService;
            _CommandVerbsList = _reflectionService.LoadAndGetCommandVerbs();
        }


        /// <summary>
        /// Commands entry point. Creates the parser instance and recives the CLI arguments
        /// </summary>
        public void Run(string[] args)
        {
            try
            {
                Parser.Default.ParseArguments(args, _CommandVerbsList.ToArray())
                    .WithParsed(Execute);
            }
            catch (ArgumentException ex)
            {
                ConsolePrompt.WriteToConsole(ex.Message, ConsoleColor.Yellow);
            }
            catch(Exception ex)
            {
                ConsolePrompt.WriteToConsole(ex.Message, ConsoleColor.Yellow);
            }
        }

        /// <summary>
        /// Executes the provided command verb. ie: test, info, exit...
        /// </summary>
        /// <param name="verb">Object that represents the verb to execute</param>
        public void Execute(object verb)
        {
            switch (verb)
            {
                case Test test:
                    _commandService.Test(test);
                    break;

                case Init init:
                    _commandService.Init(init);
                    break;

                default:
                    throw new ArgumentException("The provided command doesn't exists");
            }
        }
    }
}
