using CommandLine;

namespace ENGyn.NodesTestPlatform.Commands
{
    /// <summary>
    /// This class represents the test command and contains all the "flags" arguments that can be used with it
    /// </summary>
    [Verb("test", HelpText = "Initialize node tests")]
    public class Test
    {
        [Option('d', "dll", HelpText = "Full name of dll to test")]
        public string Dll { get; set; }
    }
}
