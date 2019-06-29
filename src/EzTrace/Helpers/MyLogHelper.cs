using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace EzTrace.Helpers
{
    public class MyLogHelper
    {
        public Action<string> ShowMessage { get; set; }
        public bool WithPrefix { get; set; }
        public string Category { get; set; }
        public MyLogHelper(string category)
        {
            Category = category;
            WithPrefix = true;
        }

        public void Info(string message, string preFix = "[Info]", string category = null)
        {
            var value = WithPrefix ? $"{category}{preFix} => {message}" : $"{message}";
            Trace.WriteLine(value);
            ShowMessage?.Invoke(value);
        }

        public void InfoException(Exception ex, string preFix = "[Exception]", string category = null)
        {
            var value = WithPrefix ? $"{category}{preFix} => {ex.Message}" : $"{ex.Message}";
            Trace.WriteLine(value);
            ShowMessage?.Invoke(value);
        }

        public void InfoObj(Object obj, string preFix = "[Info][Object]", string category = null)
        {
            var sb = new StringBuilder();
            LookupProperties(obj, sb);
            var value = WithPrefix ? $"{category}{preFix} => {sb}" : $"{sb}";
            Trace.WriteLine(value);
            ShowMessage?.Invoke(value);
        }

        public void InfoEmptyLine()
        {
            var line = "----------------------------------";
            Trace.WriteLine(line);
            ShowMessage?.Invoke(line);
        }


        private static void LookupProperties(Object obj, StringBuilder sb)
        {
            if (obj != null && sb != null)
            {
                Type collectedObjType = obj.GetType();
                PropertyInfo[] collectedObjPropertyInfos = collectedObjType.GetProperties();
                foreach (PropertyInfo collectedObjPropertyInfo in collectedObjPropertyInfos)
                {
                    object value = collectedObjPropertyInfo.GetValue(obj, null);
                    sb.AppendFormat("{0}={1};", collectedObjPropertyInfo.Name, value);
                }
            }
        }

        public static MyLogHelper Create(string category)
        {
            return new MyLogHelper(category);
        }

        public static MyLogHelper Instance = new MyLogHelper("[Default]");
    }
}