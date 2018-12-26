using System.Collections.Generic;

namespace Vapps.Common
{
    public class StorageDirectoryInfo
    {
        public StorageDirectoryInfo()
        {
            Items = new List<FileInfo>();
            CommonPrefixes = new List<string>();
        }

        public string SkipCount { get; set; }

        public List<FileInfo> Items { get; set; }

        public List<string> CommonPrefixes { get; set; }
    }
}
