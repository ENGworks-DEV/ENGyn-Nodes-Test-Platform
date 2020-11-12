using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ENGyn.NodesTestPlatform.Services
{
    public interface IReflectionService
    {
        IList<Type> LoadAndGetCommandVerbs();
        IList<Type> GetMethodArgumentTypes(MethodInfo method);
        IList<Tuple<MethodInfo, object>> MatchMethodsInAssebly(Assembly assembly, string methodName);
        Tuple<MethodInfo, object, IList> GetCorrectMethod(IList<Tuple<MethodInfo, object>> foundMethods, JArray jsonParams);
        object ExecuteMethod(MethodInfo methodToInvoke, IList parameters, object instanceObject = null);
    }
}
