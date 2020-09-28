using ENGyn.NodesTestPlatform.Commands;
using ENGyn.NodesTestPlatform.Services;
using ENGyn.NodesTestPlatform.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ENGyn.NodesTestPlatform.Providers
{
    public class CommandProvider : ICommandService
    {
        private readonly IReflectionService _reflectionService;

        public CommandProvider()
        {
            _reflectionService = new ReflectionProvider();
        }


        public void Info(Info info)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Start the application on interactive mode
        /// </summary>
        /// <param name="interactive">Interactive command instance</param>
        public void Interactive(Interactive interactive)
        {
            // TODO interactive mode
            throw new NotImplementedException();
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

            // Deserializing JSON
            var parsedJSON = JObject.Parse(jsonFile);

            // Loading Assembly
            Assembly assemblyToTest = Assembly.LoadFrom("Tests\\ENGyn.Nodes.Generic.dll");

            // Find method matches
            IList<MethodInfo> matchedMethods = _reflectionService.FindMethodInAssembly(assemblyToTest, test.Method);

            _reflectionService.GetCorrectMethod(matchedMethods, parsedJSON);
        }
    }
}
