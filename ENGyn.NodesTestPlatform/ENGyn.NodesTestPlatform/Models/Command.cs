using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENGyn.NodesTestPlatform.Models
{
    public class Command
    {
        public string Name { get; set; }
        public string LibraryClassName { get; set; }
        public IList<string> Arguments { get; set; }
    }
}
