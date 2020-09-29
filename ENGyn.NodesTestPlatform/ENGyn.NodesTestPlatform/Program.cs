using ENGyn.NodesTestPlatform.Providers;
using ENGyn.NodesTestPlatform.Services;

namespace ENGyn.NodesTestPlatform
{
    class Program
    {
        static void Main(string[] args)
        {
            IRunnableService runnable = new RunnableProvider();
            runnable.Run(args);
        }
    }
}
