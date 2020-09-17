using System;

namespace ENGyn.NodesTestPlatform.Utils
{
    public static class ConsolePrompt
    {
        private static readonly string promptMark = "eng >";

        public static string ReadFromConsole(string promptMessage = "")
        {
            // Show a prompt, and get input:
            Console.Write($"{promptMark} {promptMessage}");
            return Console.ReadLine();
        }

        public static void WriteToConsole(string message, ConsoleColor color = ConsoleColor.White)
        {
            if (message.Length > 0)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }
}
