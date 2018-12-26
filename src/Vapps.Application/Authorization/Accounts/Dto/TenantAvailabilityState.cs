namespace Vapps.Authorization.Accounts.Dto
{
    public enum TenantAvailabilityState
    {
        /// <summary>
        /// 有效的
        /// </summary>
        Available = 1,

        /// <summary>
        /// 无效的
        /// </summary>
        InActive,

        /// <summary>
        /// 没有找到
        /// </summary>
        NotFound
    }
}