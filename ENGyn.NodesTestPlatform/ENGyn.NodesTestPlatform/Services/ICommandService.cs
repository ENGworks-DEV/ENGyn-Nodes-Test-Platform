using ENGyn.NodesTestPlatform.Commands;

namespace ENGyn.NodesTestPlatform.Services
{
    public interface ICommandService
    {
        void Test(Test test);
        void Interactive(Interactive interactive);
        void Info(Info info);
    }
}
