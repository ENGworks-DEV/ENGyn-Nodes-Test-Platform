using ENGyn.NodesTestPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENGyn.NodesTestPlatform.Services
{
    interface IRunnableService
    {
        void Run();
        string Execute(Command command);
        void WriteToConsole(string message, ConsoleColor color);
        string ReadFromConsole(string promptMessage);
    }
}
