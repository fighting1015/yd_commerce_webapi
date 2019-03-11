using Abp.Application.Services.Dto;

namespace Vapps.ECommerce.Customers.Dto
{
    public class CreateOrUpdateCustomerInput : EntityDto<long>
    {
        /// <summary>
        /// 头像Id
        /// </summary>
        public virtual int AvatarPictureId { get; set; }

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
        public virtual decimal TotalConsumes { get; set; }

        /// <summary>
        /// 购买频次
        /// </summary>
        public virtual decimal TotalOrderNum { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
