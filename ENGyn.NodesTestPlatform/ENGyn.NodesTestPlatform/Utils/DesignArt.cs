using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENGyn.NodesTestPlatform.Utils
{
    public static class DesignArt
    {
        public static string CreateArt()
        {
            StringBuilder sb = new StringBuilder();

            // Creating FirstLine;
            sb.Append(@"    _______   ________               _   __          __         ").AppendLine();
            sb.Append(@"   / ____/ | / / ______  ______     / | / ____  ____/ ___  _____").AppendLine();
            sb.Append(@"  / __/ /  |/ / / __/ / / / __ \   /  |/ / __ \/ __  / _ \/ ___/").AppendLine();
            sb.Append(@" / /___/ /|  / /_/ / /_/ / / / /  / /|  / /_/ / /_/ /  __(__  ) ").AppendLine();
            sb.Append(@"/_____/_/ |_/\____/\__, /_/ /_/  /_/ |_/\____/\__,_/\___/____/  ").AppendLine();
            sb.Append(@"                  /____/                                        ").AppendLine();
            sb.AppendLine();

            // Second Line
            sb.Append(@"  ______          __     ____  __      __  ____                   ").AppendLine();
            sb.Append(@" /_  _____  _____/ /_   / __ \/ ____ _/ /_/ ______  _________ ___ ").AppendLine();
            sb.Append(@"  / / / _ \/ ___/ __/  / /_/ / / __ `/ __/ /_/ __ \/ ___/ __ `__ \").AppendLine();
            sb.Append(@" / / /  __(__  / /_   / ____/ / /_/ / /_/ __/ /_/ / /  / / / / / /").AppendLine();
            sb.Append(@"/_/  \___/____/\__/  /_/   /_/\__,_/\__/_/  \____/_/  /_/ /_/ /_/ ").AppendLine();
            sb.AppendLine();
            sb.Append("-------------------------------------------------------------------").AppendLine();

            sb.AppendLine();
            sb.Append("ENGworks. INC. All rights reserved");
            sb.AppendLine();

            string startupArt = sb.ToString();
            return startupArt;
        }
    }
}
