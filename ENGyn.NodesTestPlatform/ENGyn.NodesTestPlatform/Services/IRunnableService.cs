using ENGyn.NodesTestPlatform.Models;

namespace ENGyn.NodesTestPlatform.Services
{
    public interface IRunnableService
    {
        void Run();
        string Execute(Command command);
    }
}
