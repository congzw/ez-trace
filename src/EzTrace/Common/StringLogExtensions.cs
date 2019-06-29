using System;
using System.Diagnostics;

namespace EzTrace.Common
{
    public static class StringLogExtensions
    {
        public static Action<string> ShowFunc = message => Trace.WriteLine(message);
        public static void WriteLine(this string output, int tabCount = 0)
        {
            var appendTab = AppendTab(tabCount);
            if (string.IsNullOrWhiteSpace(output))
            {
                ShowFunc("");
                return;
            }
            ShowFunc(appendTab + output);
        }
        public static void WriteLineFormat(this string format, int tabCount = 0, params object[] arg)
        {
            var appendTab = AppendTab(tabCount);
            if (string.IsNullOrWhiteSpace(format))
            {
                ShowFunc("");
                return;
            }
            ShowFunc(appendTab + string.Format(format, arg));
        }
        private static string AppendTab(int tabCount)
        {
            var pushValue = "";
            for (int i = 0; i < tabCount; i++)
            {
                pushValue += "\t";
            }

            return pushValue;
        }
    }
}
