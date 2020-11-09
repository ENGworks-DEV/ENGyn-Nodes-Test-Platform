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
        const string _RegexLettersAndNumbersNoWhitespaces = @"^[A-Za-z0-9]+$";

        public CommandProvider(IReflectionService reflectionService, IConfigurationService configurationService)
        {
            _reflectionService = reflectionService;
            _configurationService = configurationService;
            _currentExecutionDirectory = Environment.CurrentDirectory;
        }

        /// <summary>
        /// Initializes a new node test platform project
        /// </summary>
        /// <param name="init">Init command instance</param>
        public void Init(Init init)
        {
            bool projectNameIsValid = Regex.Match(init.ProjectName, _RegexLettersAndNumbersNoWhitespaces).Success;

            if (projectNameIsValid)
            {
                _configurationService.CreateProjectDirectoriesAndFiles(init.ProjectName);
                ConsolePrompt.WriteToConsole($@"Project: {init.ProjectName} created, use cd command to get inside the folder and add some dll's on test folder", ConsoleColor.Green);
            }
            else
            {
                throw new ArgumentException("Invalid project name");
            }
        }

        /// <summary>
        /// Test Command Method. This method invoke reflection and executes the test
        /// </summary>
        /// <param name="test">Test command instance</param>
        public void Test(Test test)
        {
            if (!_configurationService.CheckProjectDirectories())
            {
                throw new DirectoryNotFoundException("Project not found. Use 'ntp init' to create a new project or check for the right project location");
            }

            string fileData = string.Empty;

            // Loading json arguments from location.
            using (var reader = new StreamReader($@"{_currentExecutionDirectory}\config.json"))
            {
                fileData = reader.ReadToEnd();
            }

            // deserializing json
            dynamic fileDataDeserilized = JsonConvert.DeserializeObject<dynamic>(fileData);
            var testMethods = fileDataDeserilized.methods;

            // Loading Assembly
            Assembly assemblyToTest = Assembly.LoadFrom($@"{_currentExecutionDirectory}\dlls\{test.Dll}");


            // Passing through all test methods on the config file
            foreach (var testMethod in testMethods)
            {                
                
                IList<Tuple<MethodInfo, object>> matchedMethods = _reflectionService.FindMethodInAssembly(assemblyToTest, (string)testMethod["name"]);
                Tuple<MethodInfo, object> methodToExecute = _reflectionService.GetCorrectMethod(matchedMethods, testMethod["arguments"]);
                _reflectionService.ExecuteMethod(methodToExecute.Item1, testMethod["arguments"], methodToExecute.Item2);
            }
        }
    }
}
