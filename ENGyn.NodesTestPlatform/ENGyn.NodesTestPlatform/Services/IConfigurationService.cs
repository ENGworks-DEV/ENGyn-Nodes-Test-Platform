using System.IO;

namespace ENGyn.NodesTestPlatform.Services
{
    public interface IConfigurationService
    {
        DirectoryInfo CreatesProjectDirectory(string fullDirectoryPath);
        void CreateProjectDirectoriesAndFiles(string projectName);
        bool CheckProjectDirectories(string projectName);
    }
}
