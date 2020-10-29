using System.IO;

namespace ENGyn.NodesTestPlatform.Services
{
    public interface IConfigurationService
    {
        DirectoryInfo CreatesProjectDirectory(string directoryName);
    }
}
