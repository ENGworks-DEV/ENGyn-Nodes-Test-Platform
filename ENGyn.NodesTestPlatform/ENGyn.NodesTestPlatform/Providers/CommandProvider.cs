using ENGyn.NodesTestPlatform.Commands;
using ENGyn.NodesTestPlatform.Services;
using ENGyn.NodesTestPlatform.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ENGyn.NodesTestPlatform.Providers
{
    public class CommandProvider : ICommandService
    {
        private readonly IReflectionService _reflectionService;
        private readonly IConfigurationService _configurationService;
        private string _currentExecutionDirectory;

        public CommandProvider()
        {
            _reflectionService = new ReflectionProvider();
            _configurationService = new ConfigurationProvider();
            _currentExecutionDirectory = Environment.CurrentDirectory;
        }

        /// <summary>
        /// Initializes a new node test platform project
        /// </summary>
        /// <param name="init">Init command instance</param>
        public void Init(Init init)
        {
            bool projectNameIsValid = Regex.Match(init.ProjectName, @"^[^\s]+$").Success;

            if (projectNameIsValid)
            {
                string fullProjectDirectoryPath = $@"{_currentExecutionDirectory}\{init.ProjectName}";
                string dllFolderSubdirectory = $@"{_currentExecutionDirectory}\{init.ProjectName}\dlls";
                string testFolderSubdirectory = $@"{_currentExecutionDirectory}\{init.ProjectName}\tests";

                _configurationService.CreatesProjectDirectory(fullProjectDirectoryPath);
                _configurationService.CreatesProjectDirectory(dllFolderSubdirectory);
                _configurationService.CreatesProjectDirectory(testFolderSubdirectory);

                ConsolePrompt.WriteToConsole($@"Project: {init.ProjectName} created, use cd command to get inside the folder and add some dll's on test folder", ConsoleColor.Green);
            }
            else
            {
                throw new ArgumentException("Project name is not valid");
            }
        }

        /// <summary>
        /// Test Command Method. This method invoke reflection and executes the test
        /// </summary>
        /// <param name="test">Test command instance</param>
        public void Test(Test test)
        {
            string jsonFile = string.Empty;

            // Loading json arguments from location.
            using (StreamReader reader = new StreamReader(test.Arguments))
            {
                jsonFile = reader.ReadToEnd();
            }

            // Loading Assembly
            Assembly assemblyToTest = Assembly.LoadFrom($@"{_currentExecutionDirectory}\dlls\{test.Dll}");

            // Find method matches
            IList<MethodInfo> matchedMethods = _reflectionService.FindMethodInAssembly(assemblyToTest, test.Method);

            // deserializing json
            var converter = new ExpandoObjectConverter();
            JsonConvert.DeserializeObject<ExpandoObject>(jsonFile, converter);
        }
    }
}
