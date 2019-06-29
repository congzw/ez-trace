using System;

namespace EzTrace.Common
{
    public static class ConsoleStringExtensions
    {
        public static void WriteLine(this string output, int tabCount = 0)
        {
            AppendTab(tabCount);
            if (string.IsNullOrWhiteSpace(output))
            {
                Console.WriteLine("");
                return;
            }
            Console.WriteLine(output);
        }
        public static void WriteLineFormat(this string format, int tabCount = 0, params object[] arg)
        {
            AppendTab(tabCount);
            if (string.IsNullOrWhiteSpace(format))
            {
                Console.WriteLine("");
                return;
            }
            Console.WriteLine(format, arg);
        }

        private static void AppendTab(int tabCount)
        {
            var pushValue = "";
            for (int i = 0; i < tabCount; i++)
            {
                pushValue += "\t";
            }
            Console.Write("{0}", pushValue);
        }
    }
}
