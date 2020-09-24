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

        /// <summary>
        /// Gets all the classes located on Models Folder that represents available commands to execute on the Nodes Testing Platform
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
        /// Search in all the assembly classes for a method that match with provided name
        /// </summary>
        /// <param name="assembly">Assembly to search</param>
        /// <param name="methodName">Exact name of the method</param>
        /// <returns>A method info object that correspond with the provided name</returns>
        /// <exception cref="MissingMethodException">Thrown when there's not match with the provided name</exception>
        public MethodInfo FindMethodInAssembly(Assembly assembly, string methodName)
        {
            MethodInfo methodInfo = null;
            
            // Gets all the static classes: For CLR static classes are abstract and sealed.
            Type[] publicStaticClasses = assembly.GetTypes().Where(
                p => p.IsClass && 
                p.IsAbstract && 
                p.IsSealed && 
                p.IsPublic).ToArray();

            foreach (Type loadedClass in publicStaticClasses)
            {
                var methodsss = loadedClass.GetMethods().Where(p => p.Name == methodName && p.IsPublic && p.IsStatic);
                methodInfo = loadedClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
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
                var methodsss = loadedClass.GetMethods().Where(p => p.Name == methodName && p.IsPublic);
                methodInfo = loadedClass.GetMethod(methodName, BindingFlags.Public);
            }

            if (methodInfo != null)
                return methodInfo;
            else
                throw new MissingMethodException($"Method: {methodName} doesn't exists on this assembly");
        }

        /// <summary>
        /// Search in all the assembly classes for a method that match with provided name
        /// </summary>
        /// <param name="assembly">Assembly to search</param>
        /// <param name="methodName">Exact name of the method</param>
        /// <returns>A method info object that correspond with the provided name</returns>
        /// <exception cref="MissingMethodException">Thrown when there's not match with the provided name</exception>
        public MethodInfo[] FindMethodInAssembly2(Assembly assembly, string methodName)
        {
            MethodInfo[] methodInfo = null;

            // Gets all the static classes: For CLR static classes are abstract and sealed.
            Type[] publicStaticClasses = assembly.GetTypes().Where(
                p => p.IsClass &&
                p.IsAbstract &&
                p.IsSealed &&
                p.IsPublic).ToArray();

            foreach (Type loadedClass in publicStaticClasses)
            {
                // TODO Lidiar con sobrecargas
                methodInfo = loadedClass.GetMethods().Where(p => p.Name == methodName && p.IsPublic && p.IsStatic).ToArray();
                //methodInfo = loadedClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
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
                methodInfo = loadedClass.GetMethods().Where(p => p.Name == methodName && p.IsPublic).ToArray();
                //methodInfo = loadedClass.GetMethod(methodName, BindingFlags.Public);
            }

            if (methodInfo != null)
                return methodInfo;
            else
                throw new MissingMethodException($"Method: {methodName} doesn't exists on this assembly");
        }
    }
}
