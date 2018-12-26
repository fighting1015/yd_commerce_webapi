namespace Vapps
{
    public static class BooleanExtensions
    {
        /// <summary>
        ///  布尔类型转字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToLowerString(this bool value)
        {
            return value.ToString().ToLowerInvariant();
        }

    }
}
