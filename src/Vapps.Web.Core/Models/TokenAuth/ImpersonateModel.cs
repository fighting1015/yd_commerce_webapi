using System.ComponentModel.DataAnnotations;

namespace Vapps.Web.Models.TokenAuth
{
    /// <summary>
    /// Ä£ÄâµÇÂ¼
    /// </summary>
    public class ImpersonateModel
    {
        /// <summary>
        /// ×â»§Id
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// ×â»§Id
        /// </summary>
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }
    }
}