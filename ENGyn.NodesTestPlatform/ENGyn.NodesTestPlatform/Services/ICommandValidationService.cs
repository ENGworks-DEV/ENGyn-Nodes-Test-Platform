using System.Collections.Generic;
using System.Reflection;

namespace ENGyn.NodesTestPlatform.Services
{
    public interface ICommandValidationService
    {
        bool ValidateLibraryCommand<T>(Dictionary<string, Dictionary<string, T>> commandsLibrary, string libraryName, string commandName);
        bool ValidateProvidedArgumentsCount(IList<ParameterInfo> arguments, int providedArgumentCount, out string validationMessage)
    }
}
