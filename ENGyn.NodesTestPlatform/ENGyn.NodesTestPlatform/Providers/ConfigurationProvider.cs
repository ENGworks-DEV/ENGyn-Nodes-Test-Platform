using ENGyn.NodesTestPlatform.Services;
using System;
using System.IO;

namespace ENGyn.NodesTestPlatform.Providers
{
    public class ConfigurationProvider : IConfigurationService
    {
        public DirectoryInfo createsProjectDirectory(string directoryName)
        {
            DirectoryInfo directory = new DirectoryInfo(directoryName);

            try
            {
                if (!Directory.Exists(directoryName))
                {
                    directory.Create();
                }

                return directory;
            }
            catch (IOException ex)
            {
                throw new IOException("The process failed trying to create project's directory", ex);
            }
        }
    }
}
