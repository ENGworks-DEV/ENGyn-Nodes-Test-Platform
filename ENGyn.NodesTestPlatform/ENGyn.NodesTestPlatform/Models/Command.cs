using System.Collections.Generic;

namespace ENGyn.NodesTestPlatform.Models
{
    public class Command
    {
        public string Name { get; set; }
        public string LibraryClassName { get; set; }
        public IList<string> Arguments { get; set; }
    }
}
