using CommandLine;

namespace ENGyn.NodesTestPlatform.Commands
{
    /// <summary>
    /// This model class represents the interactive command used to run the application interactive mode.
    /// </summary>
    [Verb("inter", isDefault: false, HelpText = "Start the application on interactive mode")]
    public class Interactive
    {
        [Option('v',"verbose", Default = false, HelpText = "Indicates if interactive mode output is verbose", Required = false)]
        public bool Verbose { get; set; }
    }
}
