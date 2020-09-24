using CommandLine;

namespace ENGyn.NodesTestPlatform.Commands
{
    /// <summary>
    /// This class represents the test command and contains all the "flags" arguments that can be used with it
    /// </summary>
    [Verb("test", HelpText = "Initialize node tests")]
    public class Test
    {
        [Option('t', "type", Required = true, HelpText = "Indicates what type of assert are gonna be used on the test")]
        public string Type { get; set; }

        [Option('m', "method", Required = true, HelpText = "Name of the method that are going to be tested")]
        public string Method { get; set; }

        [Option('c', "case", Required = false, HelpText = "Name of the test case to apply")]
        public string Case { get; set; }

        [Option('a', "args", Required = true, HelpText = "Path to json arguments file")]
        public string Arguments { get; set; }

        [Option('r', "result", Required = true, HelpText = "Path to json expected result file")]
        public string Result { get; set; }
        [Option]
        public string Expected { get; set; }
    }
}
