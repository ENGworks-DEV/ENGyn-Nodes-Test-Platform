using System;
using System.Collections.Generic;
using System.Reflection;

namespace ENGyn.NodesTestPlatform.Services
{
    public interface IReflectionService
    {
        IList<Type> LoadAndGetCommandVerbs();
        IList<Type> GetMethodArgumentTypes(MethodInfo method);
        IList<MethodInfo> FindMethodInAssembly(Assembly assembly, string methodName);
    }
}
