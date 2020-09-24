using CommandLine;

namespace ENGyn.NodesTestPlatform.Commands
{
    /// <summary>
    /// This model class represents the command to close the program execution on interactive mode
    /// </summary>
    [Verb("exit", HelpText = "Close the program on interactive mode")]
    public class Exit
    {
        [Option('t', "timeOut", Default = 0, Required = false, HelpText = "Sets timeout time to close the program")]
        public int Timeout { get; set; }
    }
}
