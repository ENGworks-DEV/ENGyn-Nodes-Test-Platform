using ENGyn.NodesTestPlatform.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENGyn.NodesTestPlatform
{
    class Program
    {
        static void Main(string[] args)
        {
            string art = DesignArt.CreateArt();
            Runner runner = new Runner();

            runner.WriteToConsole(art, ConsoleColor.Green);
            runner.Run();
        }
    }
}
