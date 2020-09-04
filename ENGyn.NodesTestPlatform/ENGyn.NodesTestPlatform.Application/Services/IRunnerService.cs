using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENGyn.NodesTestPlatform.Application.Services
{
    public interface IRunnerService
    {
        void Run();
        string Execute(string command);
        void WriteToConsole(string message, ConsoleColor color);
        string ReadFromConsole(string promptMessage);
    }
}
