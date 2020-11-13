namespace ENGyn.NodesTestPlatform.Services
{
    public interface IRunnableService
    {
        void Run(string[] args);
        void Execute(object verb);
    }
}
