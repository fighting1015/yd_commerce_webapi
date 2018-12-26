namespace Vapps.Authorization.Accounts.Dto
{
    public class IsTenantAvailableOutput
    {
        /// <summary>
        /// 状态
        /// </summary>
        public TenantAvailabilityState State { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        public int? TenantId { get; set; }

        public IsTenantAvailableOutput()
        {

        }

        public IsTenantAvailableOutput(TenantAvailabilityState state, int? tenantId = null)
        {
            State = state;
            TenantId = tenantId;
        }
    }
}