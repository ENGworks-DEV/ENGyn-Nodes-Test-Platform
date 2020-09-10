using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENGyn.NodesTestPlatform.Commands
{
    /// <summary>
    /// This class contains all the methods that correspond to each command that can be executed from the application.
    /// </summary>
    public static class DefaultCommands
    {
        public static string Test(int id, string data)
        {
            return string.Format("Test executed to method {0} with data {1}", id, data);
        }

        public static string GetDate(DateTime date)
        {
            return string.Format("Date and time is: {0}", date);
        }
    }
}
