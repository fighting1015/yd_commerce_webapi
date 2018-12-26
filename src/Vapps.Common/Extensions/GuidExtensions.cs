using System;

namespace Vapps.Extensions
{
    public static class GuidExtensions
    {
        /// <summary>  
        /// 根据GUID获取19位的唯一数字序列  
        /// </summary>  
        /// <returns></returns>  
        public static long ToLongId(this Guid value)
        {
            byte[] buffer = value.ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}
