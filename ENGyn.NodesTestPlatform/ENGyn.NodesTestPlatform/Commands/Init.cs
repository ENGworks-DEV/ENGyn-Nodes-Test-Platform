using CommandLine;

namespace ENGyn.NodesTestPlatform.Commands
{
    /// <summary>
    /// This class model represents the init command used to create a new test project
    /// </summary>
    [Verb("init", HelpText = "Initialize a new test project")]
    public class Init
    {
        [Option('n', "projectName",Required = true, HelpText = "Project's name")]
        public string ProjectName { get; set; }
    }
}
