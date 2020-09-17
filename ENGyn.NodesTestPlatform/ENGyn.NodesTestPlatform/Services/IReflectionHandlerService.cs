using ENGyn.NodesTestPlatform.Models;
using System.Collections.Generic;
using System.Reflection;

namespace ENGyn.NodesTestPlatform.Services
{
    public interface IReflectionHandlerService
    {
        Dictionary<string, Dictionary<string, IList<ParameterInfo>>> LoadAndGetLibraries();
        string InvokeConsoleCommand(Command userCommand, object[] args);
    }
}
