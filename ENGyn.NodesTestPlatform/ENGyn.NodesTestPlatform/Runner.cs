using ENGyn.NodesTestPlatform.Providers;
using ENGyn.NodesTestPlatform.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENGyn.NodesTestPlatform
{
    public class Runner
    {
        private RunnableProvider _runnableProvider;

        public Runner()
        {
            _runnableProvider = new RunnableProvider();
        }

        public void start()
        {
            string startArt = DesignArt.CreateArt();
            _runnableProvider.WriteToConsole(startArt, ConsoleColor.Green);
            _runnableProvider.Run();
        }
    }
}
