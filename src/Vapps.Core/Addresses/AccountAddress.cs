using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.Addresses
{
    [Table("AccountAddresses")]
    public class AccountAddress : CreationAuditedEntity<long>
    {
        /// <summary>
        /// 地址Id
        /// </summary>
        public long AddressId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [ForeignKey("AddressId")]
        public Address Address { get; set; }

        /// <summary>
        /// 账号Id
        /// </summary>
        public long AccountId { get; set; }
    }
}
