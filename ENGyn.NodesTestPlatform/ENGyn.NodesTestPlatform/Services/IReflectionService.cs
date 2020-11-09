using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ENGyn.NodesTestPlatform.Services
{
    public interface IReflectionService
    {
        IList<Type> LoadAndGetCommandVerbs();
        IList<Type> GetMethodArgumentTypes(MethodInfo method);
        IList<Tuple<MethodInfo, object>> FindMethodInAssembly(Assembly assembly, string methodName);
        Tuple<MethodInfo, object> GetCorrectMethod(IList<Tuple<MethodInfo, object>> foundMethods, dynamic deserializedParams);
        object ExecuteMethod(MethodInfo methodToInvoke, JArray parameters, object instanceObject = null);
    }
}
