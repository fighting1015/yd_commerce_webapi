namespace Vapps.Dto
{
    /// <summary>
    /// 批量操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BatchInput<T>
    {
        /// <summary>
        /// id数组
        /// </summary>
        public T[] Ids { get; set; }
    }
}
