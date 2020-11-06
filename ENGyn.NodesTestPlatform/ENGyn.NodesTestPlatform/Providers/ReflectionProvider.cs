using CommandLine;
using ENGyn.NodesTestPlatform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ENGyn.NodesTestPlatform.Providers
{
    public class ReflectionProvider : IReflectionService
    {
        private const string _verbsNamespace = "ENGyn.NodesTestPlatform.Commands";

        #region Implemented from interface

        /// <summary>
        /// Gets all the models located on Commands Folder that represents available commands to execute on the Nodes Testing Platform
        /// </summary>
        /// <returns>List of type classes that represents every available command</returns>
        public IList<Type> LoadAndGetCommandVerbs()
        {
            var result = from types in Assembly.GetExecutingAssembly().GetTypes()
                         where types.IsClass && types.Namespace == _verbsNamespace && types.GetCustomAttribute<VerbAttribute>() != null
                         select types;

            return result.ToList();
        }

        /// <summary>
        /// Get a list of types that represents each method argument
        /// </summary>
        /// <param name="method">Method to be executed</param>
        /// <returns>A List containing all the argument types</returns>
        public IList<Type> GetMethodArgumentTypes(MethodInfo method)
        {
            List<Type> result = new List<Type>();
            ParameterInfo[] parameters = method.GetParameters();

            foreach (ParameterInfo p in parameters)
            {
                result.Add(p.GetType());
            }

            return result;
        }

        /// <summary>
        /// Search in all the assembly classes for a method that match with provided name. Normally this method
        /// will return only just one item. But there might be ocassions where an assembly contains overloaded methods
        /// in that case will return more than one Method info item
        /// </summary>
        /// <param name="assembly">Assembly to search</param>
        /// <param name="methodName">Exact name of the method</param>
        /// <returns>A MethodInfo Array containing at least one method that match the provided name</returns>
        /// <exception cref="MissingMethodException">Thrown when there's not match with the provided name</exception>
        public IList<MethodInfo> FindMethodInAssembly(Assembly assembly, string methodName)
        {
            IList<MethodInfo> methodInfo = new List<MethodInfo>();
            
            // Gets all the static classes: For CLR static classes are abstract and sealed.
            Type[] publicStaticClasses = assembly.GetTypes().Where(
                p => p.IsClass && 
                p.IsAbstract && 
                p.IsSealed && 
                p.IsPublic).ToArray();

            foreach (Type loadedClass in publicStaticClasses)
            {
                loadedClass.GetMethods().Where(p => p.Name == methodName && p.IsPublic && p.IsStatic).ToList().ForEach(p => methodInfo.Add(p));
            }

            if (methodInfo != null) return methodInfo;

            // Get all classes that needs an instace to run.
            Type[] publicClasses = assembly.GetTypes().Where(
                p => p.IsClass && 
                !p.IsAbstract && 
                !p.IsSealed && 
                p.IsPublic).ToArray();

            foreach (Type loadedClass in publicClasses)
            {
                loadedClass.GetMethods().Where(p => p.Name == methodName && p.IsPublic).ToList().ForEach(p => methodInfo.Add(p)); ;
            }

            if (methodInfo != null)
                return methodInfo;
            else
                throw new MissingMethodException($"Method: {methodName} doesn't exists on this assembly");
        }

        /// <summary>
        /// Gets correct method from when overloads are found. It analyzes method signature and compares with
        /// param types from the deserialized Json file.
        /// </summary>
        /// <param name="foundMethods">Method info array with found matches</param>
        /// <param name="deserializedParams">dynamic object that represents the deserialized Json file</param>
        /// <returns>The correct method to execute</returns>
        public MethodInfo GetCorrectMethod(IList<MethodInfo> foundMethods, dynamic deserializedParams)
        {
            MethodInfo correctMethod = null;
            var jsonParams = (IDictionary<string, object>)deserializedParams;

            foreach (MethodInfo method in foundMethods)
            {
                ParameterInfo[] methodParams = method.GetParameters();
                bool methodResult = (methodParams.Length == jsonParams.Count) || ValidateJsonParameters(methodParams, jsonParams);
                if (methodResult) return method;
            }

            return correctMethod;
        }

        /// <summary>
        /// Validates if the method signature match with the json parameters.
        /// </summary>
        /// <param name="methodParams">Parameter info array that contains the method arguments info</param>
        /// <param name="jsonParams">Dictionary that contains all the json paramteres</param>
        /// <returns>boolean flag that indicates if the method is valid or not. Returns true for valid methods</returns>
        private bool ValidateJsonParameters(ParameterInfo[] methodParams, IDictionary<string, object> jsonParams)
        {
            bool result = true;
            for (int i = 0; i < jsonParams.Count; ++i)
            {
                result &= methodParams[i].ParameterType.Equals(jsonParams.Values.ElementAt(i).GetType());
            }
            return result;
        }

        #endregion
    }
}
