using System.IO;

namespace ENGyn.NodesTestPlatform.Services
{
    public interface IConfigurationService
    {
        DirectoryInfo createsProjectDirectory(string directoryName);
    }
}
