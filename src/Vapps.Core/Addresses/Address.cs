using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.Addresses
{
    [Table("Addresses")]
    public class Address : CreationAuditedEntity<long>
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public int ProvinceId { get; set; }

        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public int CityId { get; set; }

        public string City { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public int DistrictId { get; set; }

        public string District { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailAddress { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string ZipPostalCode { get; set; }
    }
}
