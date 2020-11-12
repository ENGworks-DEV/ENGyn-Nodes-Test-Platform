using CommandLine;
using ENGyn.NodesTestPlatform.Services;
using ENGyn.NodesTestPlatform.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ENGyn.NodesTestPlatform.Providers
{
    public class ReflectionProvider : IReflectionService
    {
        private const string _verbsNamespace = "ENGyn.NodesTestPlatform.Commands";

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
        /// Search in all the assembly classes for methods that matches the provided name.
        /// </summary>
        /// <param name="assembly">Assembly to search</param>
        /// <param name="methodName">Name of the method</param>
        /// <returns>A MethodInfo Array containing at least one method that match the provided name</returns>
        /// <exception cref="MissingMethodException">Thrown when there's not match with the provided name</exception>
        public IList<Tuple<MethodInfo, object>> MatchMethodsInAssebly(Assembly assembly, string methodName)
        {
            IList<Tuple<MethodInfo, object>> methods = new List<Tuple<MethodInfo, object>>();
            
            // This search returns all the static classes from assembly (For CLR static classes are abstract and sealed).
            Type[] publicStaticClasses = assembly.GetTypes().Where(
                p => p.IsClass && 
                p.IsAbstract && 
                p.IsSealed && 
                p.IsPublic).ToArray();

            foreach (Type loadedClass in publicStaticClasses)
            {
                loadedClass.GetMethods().Where(p => p.Name == methodName && p.IsPublic && p.IsStatic).ToList().ForEach(p => 
                {
                    methods.Add(new Tuple<MethodInfo, object>(p, null));
                });
            }

            if (!methods.Count.Equals(0)) return methods;

            // This search returns all the classes that needs an instance to be invoked (For CLR those clases are the public ones. Not sealed and Abstract)
            Type[] publicClasses = assembly.GetTypes().Where(
                p => p.IsClass && 
                !p.IsAbstract && 
                !p.IsSealed && 
                p.IsPublic).ToArray();

            foreach (Type loadedClass in publicClasses)
            {
                loadedClass.GetMethods().Where(p => p.Name == methodName && p.IsPublic).ToList().ForEach(p =>
                {
                    var loadedClassInstance = Activator.CreateInstance(loadedClass);
                    methods.Add(new Tuple<MethodInfo, object>(p, loadedClassInstance));
                });
            }

            if (!methods.Count.Equals(0))
                return methods;
            else
                throw new MissingMethodException($"Method: {methodName} doesn't exists on this assembly");
        }

        /// <summary>
        /// Iterates thorugh a List of Tuples that contains methodInfo and object isntance. And analyze which of
        /// this methods is the correct to execute depending of their arguments count and arguments types
        /// </summary>
        /// <param name="foundMethods">List of tuples that contains the method to analyze and the instance of that method</param>
        /// <param name="jsonParams">A Newtonsoft's JArray that contains the parameters for the method</param>
        /// <returns>A tuple that contains the methodInfo to execute, the method instance and a IList that contains the necessary parameters to invoke the method.</returns>
        public Tuple<MethodInfo, object, IList> GetCorrectMethod(IList<Tuple<MethodInfo, object>> foundMethods, JArray jsonParams)
        {
            foreach (var method in foundMethods)
            {
                ParameterInfo[] methodParams = method.Item1.GetParameters();
                if (methodParams.Length == jsonParams.Count)
                {
                    var convertedParameters = ParserHelper.ConvertParametersToSignatureTypes(jsonParams, methodParams);

                    if (ValidateArgumentsAndSignature(methodParams, jsonParams, convertedParameters))
                    {
                        return Tuple.Create(method.Item1, method.Item2, convertedParameters);
                    }
                }
            }

            throw new ArgumentException("Arguments error: The provided arguments doesn't match the count or the types of method signature");
        }

        /// <summary>
        /// Validates if the method signature match with the json parameters.
        /// </summary>
        /// <param name="methodParams">Parameter info array that contains the method arguments info</param>
        /// <param name="jsonParameters">JArray that contains method paramteres used to test</param>
        /// <param name="convertedParameters">A IList that contain the converted parameters from json/param>
        /// <returns>boolean flag that indicates if the method is valid or not. Returns true for valid methods</returns>
        private bool ValidateArgumentsAndSignature(ParameterInfo[] methodParams, JArray jsonParameters, IList convertedParameters)
        {
            bool result = true;
            for (int i = 0; i < jsonParameters.Count; ++i)
            {
                result &= methodParams[i].ParameterType.Equals(convertedParameters[i].GetType());
            }
            return result;
        }

        /// <summary>
        /// Executes a method using reflection
        /// </summary>
        /// <param name="methodToInvoke">Method to be invoked</param>
        /// <param name="parameters">Parameters of the method</param>
        /// <param name="instanceObject">Class instance of the method</param>
        /// <returns></returns>
        public object ExecuteMethod(MethodInfo methodToInvoke, IList parameters, object instanceObject = null)
        {
            object[] methodParameters = new object[parameters.Count];
            parameters.CopyTo(methodParameters, 0);
            var result = methodToInvoke.Invoke(instanceObject, methodParameters);
            return result;
        }
    }
}
