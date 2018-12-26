using System.Collections.Generic;

namespace Vapps
{
    public static class FileStorageConsts
    {
        public const string IMAGE_BUCKET = "image";
        public const string STATIC_BUCKET = "static";

        public static Dictionary<string, string> BUCKET_DOMAIN = new Dictionary<string, string>
        {
            { "image","http://image.vapps.com.cn"},
            { "static","http://media.vapps.com.cn"}
        };
    }
}
