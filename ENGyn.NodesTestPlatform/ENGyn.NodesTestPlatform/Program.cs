using ENGyn.NodesTestPlatform.Providers;
using ENGyn.NodesTestPlatform.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ENGyn.NodesTestPlatform
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = typeof(Program).Name;
            RunnableProvider runnableProvider = new RunnableProvider();
            runnableProvider.Run();
        }
    }
}
