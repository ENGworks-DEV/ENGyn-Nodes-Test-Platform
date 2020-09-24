using System;
using System.Collections.Generic;
using System.Reflection;

namespace ENGyn.NodesTestPlatform.Services
{
    public interface IReflectionService
    {
        IList<Type> LoadAndGetCommandVerbs();
        IList<Type> GetMethodArgumentTypes(MethodInfo method);
        MethodInfo FindMethodInAssembly(Assembly assembly, string methodName);
        MethodInfo[] FindMethodInAssembly2(Assembly assembly, string methodName);
    }
}
