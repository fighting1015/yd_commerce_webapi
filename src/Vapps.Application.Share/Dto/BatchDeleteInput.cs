namespace Vapps.Dto
{
    public class BatchDeleteInput<T>
    {
        /// <summary>
        /// id数组
        /// </summary>
        public T[] Ids { get; set; }
    }
}
