using System;

namespace Util.ConsoleUtils
{
    public static class ConsoleExtensions
    {
        public static void WriteLineWait(string value)
        {
            Console.WriteLine(value);
            Console.ReadLine();
        }

        public static void WriteLinePos(string value, int left = 0, int top = 0)
        {
            Console.SetCursorPosition(left, top);
            Console.WriteLine(value);
        }
    }
}
