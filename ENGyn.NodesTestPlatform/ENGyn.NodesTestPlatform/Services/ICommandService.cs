using ENGyn.NodesTestPlatform.Commands;

namespace ENGyn.NodesTestPlatform.Services
{
    public interface ICommandService
    {
        void Test(Test test);
        void Init(Init init);
    }
}
