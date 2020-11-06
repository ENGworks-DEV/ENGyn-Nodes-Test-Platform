using ENGyn.NodesTestPlatform.Services;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace ENGyn.NodesTestPlatform.Providers
{
    public class ConfigurationProvider : IConfigurationService
    {
        const string _defaultValueMessage = "Put the expected result value";
        private string _currentExecutionDirectory;

        public ConfigurationProvider()
        {
            _currentExecutionDirectory = Environment.CurrentDirectory;
        }

        /// <summary>
        /// Checks if projects directories and files are created.
        /// </summary>
        /// <param name="projectName">Project's name</param>
        /// <returns>A boolean result that indicates if project directories and files exists. True if exists, false if does not</returns>
        public bool CheckProjectDirectories(string projectName)
        {
            var projectExistanceFlag = true;
            string projectSubdirectoryDllPath = $@"{_currentExecutionDirectory}\{projectName}\dlls";
            string projectConfigFilePath = $@"{_currentExecutionDirectory}\{projectName}\config.json";

            projectExistanceFlag &= Directory.Exists(projectSubdirectoryDllPath);
            projectExistanceFlag &= File.Exists(projectConfigFilePath);

            return projectExistanceFlag;
        }

        /// <summary>
        /// Checks if the project directives and 
        /// </summary>
        /// <param name="projectName"></param>
        public void CreateProjectDirectoriesAndFiles(string projectName)
        {
            string projectDirectoryPath = $@"{_currentExecutionDirectory}\{projectName}";
            string projectSubdirectoryDllPath = $@"{_currentExecutionDirectory}\{projectName}\dlls";
            CreatesProjectDirectory(projectDirectoryPath);
            CreatesProjectDirectory(projectSubdirectoryDllPath);
            CreateTestConfigFile(projectDirectoryPath, projectName);
        }

        /// <summary>
        /// Creates a new directory given a path
        /// </summary>
        /// <param name="fullDirectoryPath">Path where directory will be created</param>
        /// <returns></returns>
        /// <exception cref="IOException">Throw when a directory already exists</exception>
        public DirectoryInfo CreatesProjectDirectory(string fullDirectoryPath)
        {
            var directory = new DirectoryInfo(fullDirectoryPath);

            try
            {
                if (Directory.Exists(fullDirectoryPath))
                {
                    throw new IOException(@"There's a directory with the same name");
                } 
                else
                {
                    directory.Create();
                }

                return directory;
            }
            catch (IOException ex)
            {
                throw new IOException($@"Cannot create project: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new default config file given a folder path
        /// </summary>
        /// <param name="folderPath">Path of the folder where the config file will be created</param>
        /// <param name="projectName">Project's name</param>
        /// <exception cref="ArgumentNullException">Throw when path argument is null</exception>
        private void CreateTestConfigFile(string folderPath, string projectName)
        {
            if (folderPath.Equals(string.Empty) || folderPath.Equals(null)) {
                throw new ArgumentNullException("The file path can't be null");
            }

            // Method Config JSON structue
            JObject methods = new JObject(
                new JProperty("name", ""),
                new JProperty("testType", ""),
                new JProperty("arguments", new JArray()),
                new JProperty("result", "")
            );

            // JSON Config file structure generation
            JObject config = new JObject(
                new JProperty("config", new JObject(
                    new JProperty("projectName", projectName),
                    new JProperty("results", true)
                )),
                new JProperty("methods", new JArray(
                    methods
                ))
            );

            if (!File.Exists($@"{folderPath}\config.json"))
            {
                using (var fileWritter = File.CreateText($@"{folderPath}\config.json"))
                {
                    fileWritter.Write(config.ToString());
                }
            }
        }
    }
}
