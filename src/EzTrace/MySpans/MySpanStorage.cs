using System;
using System.IO;
using System.Linq;
using EzTrace.Common;

namespace EzTrace.MySpans
{
    public interface IMySpanStorage
    {
        string AutoCreateFilePath();
        MyRecord Get(string filePath);
        void AddOrUpdate(string filePath, MyRecord myRecord);
    }

    public class MySpanStorage : IMySpanStorage
    {
        public MySpanStorage(IJsonFileHelper jsonFileHelper, Func<string> getTraceFolderPath)
        {
            JsonFile = jsonFileHelper;
            GetClock = () => DateTime.Now;
            GetTraceFolderPath = getTraceFolderPath;
        }

        public IJsonFileHelper JsonFile { get; set; }

        public Func<DateTime> GetClock { get; set; }

        public Func<string> GetTraceFolderPath { get; set; }

        public string AutoCreateFilePath()
        {
            var folder = GetTraceFolderPath();
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var filePath = Path.Combine(folder, $"{GetClock():yyyy-MM-dd_HH}.json");
            return filePath;
        }

        public MyRecord Get(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            return JsonFile.Load<MyRecord>(filePath);
        }
        
        public void AddOrUpdate(string filePath, MyRecord addRecord)
        {
            if (addRecord == null)
            {
                return;
            }
            var oldRecord = Get(filePath) ?? new MyRecord();

            var processDic = oldRecord.Processes.ToDictionary(x => x.CreateKey(), StringComparer.OrdinalIgnoreCase);
            foreach (var process in addRecord.Processes)
            {
                var key = process.CreateKey();
                if (!processDic.ContainsKey(key))
                {
                    processDic.Add(key, process);
                }
            }
            
            var spanDic = oldRecord.Spans.ToDictionary(x => x.SpanId, StringComparer.OrdinalIgnoreCase);
            foreach (var span in addRecord.Spans)
            {
                if (!spanDic.ContainsKey(span.SpanId))
                {
                    spanDic.Add(span.SpanId, span);
                }
            }

            var saveRecord = new MyRecord();
            saveRecord.Processes = processDic.Values.ToList();
            saveRecord.Spans = spanDic.Values.ToList();
            JsonFile.Save(saveRecord, filePath);
            RecordRefresh(saveRecord.Spans.Count, GetClock());
        }
        
        private void RecordRefresh(int count, DateTime clock)
        {
            var folder = GetTraceFolderPath();
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var filePath = Path.Combine(folder, "_flush.txt");
            File.AppendAllText(filePath,  $@"{clock:yyyy-MM-dd HH:mm:ss} flush span count: {count}{Environment.NewLine}");
        }
    }
}