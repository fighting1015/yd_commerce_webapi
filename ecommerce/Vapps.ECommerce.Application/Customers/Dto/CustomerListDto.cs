using Abp.Application.Services.Dto;
using System;

namespace Vapps.ECommerce.Customers.Dto
{
    public class CustomerListDto : EntityDto
    {
        /// <summary>
        /// 头像链接
        /// </summary>
        public virtual string AvatarUrl { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// 消费总额
        /// </summary>
        public decimal TotalConsumes { get; set; }

        /// <summary>
        /// 购买频次
        /// </summary>
        public decimal TotalOrderNum { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 首次购买时间/创建时间
        /// </summary>
        public DateTime CreateOn { get; set; }
    }
}
