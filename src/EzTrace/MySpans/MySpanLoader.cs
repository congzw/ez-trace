using System.Collections.Generic;
using System.IO;

namespace EzTrace.MySpans
{
    public class MySpanLoader
    {
        public MySpanStorage Storage { get; set; }

        public MySpanLoader(MySpanStorage storage)
        {
            Storage = storage;
        }

        public IList<MyRecord> GetMyRecords()
        {
            var myRecords = new List<MyRecord>();
            var folderPath = Storage.GetTraceFolderPath();
            if (!Directory.Exists(folderPath))
            {
                return myRecords;
            }

            var files = Directory.GetFiles(folderPath, "*.json");
            foreach (var file in files)
            {
                var myRecord = Storage.Get(file);
                if (myRecord != null)
                {
                    myRecords.Add(myRecord);
                }
            }
            return myRecords;
        }
    }
}
