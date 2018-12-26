using System.ComponentModel.DataAnnotations;

namespace Vapps.Authorization.Accounts.Dto
{
    public class ImpersonateInput
    {
        /// <summary>
        /// �⻧Id(�ɿ�)
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// �û�Id(����0)
        /// </summary>
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }
    }
}