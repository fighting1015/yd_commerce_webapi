namespace Vapps.Common
{
    public class FileInfo
    {
        /// <summary>
        /// 文件 key(前缀/文件名)
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 文件哈希值
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long FSize { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// 上传时间戳
        /// </summary>
        public long PutTime { get; set; }
    }
}
