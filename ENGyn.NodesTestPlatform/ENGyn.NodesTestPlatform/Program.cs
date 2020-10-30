using ENGyn.NodesTestPlatform.Providers;

namespace ENGyn.NodesTestPlatform
{
    class Program
    {
        static void Main(string[] args)
        {
            var reflectionProvider = new ReflectionProvider();
            var configurationProvider = new ConfigurationProvider();
            var commandProvider = new CommandProvider(reflectionProvider, configurationProvider);
            var runnableProvider = new RunnableProvider(reflectionProvider, commandProvider);
            runnableProvider.Run(args);
        }
    }
}
