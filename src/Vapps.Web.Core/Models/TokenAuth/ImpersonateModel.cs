using System.ComponentModel.DataAnnotations;

namespace Vapps.Web.Models.TokenAuth
{
    /// <summary>
    /// ģ���¼
    /// </summary>
    public class ImpersonateModel
    {
        /// <summary>
        /// �⻧Id
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// �⻧Id
        /// </summary>
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }
    }
}