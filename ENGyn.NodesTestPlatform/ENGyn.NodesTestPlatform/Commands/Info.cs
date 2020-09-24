using CommandLine;

namespace ENGyn.NodesTestPlatform.Commands
{
    /// <summary>
    /// Custom info class that represents the "info" command
    /// </summary>
    [Verb("info", HelpText = "Shows info about the testing tool")]
    public class Info
    {
        [Option('v', "version", Default = false, HelpText = "Shows the version of the Command Line and Core Tester", Required = false)]
        public string Version { get; set; }

        [Option('a', "about", Default = false, HelpText = "Shows detailed information about the tool", Required = false)]
        public string About { get; set; }
    }
}
