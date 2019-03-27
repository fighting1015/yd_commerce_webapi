using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.Advert.AdvertAccounts
{
    [Table("AdvertAccounts")]
    public class AdvertAccount : FullAuditedEntity<long>, IMustHaveTenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// 店铺Id
        /// </summary>
        public virtual int StoreId { get; set; }

        /// <summary>
        /// 第三方Id
        /// </summary>
        public virtual string ThirdpartyId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual long ProductId { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// 渠道
        /// </summary>
        public virtual AdvertChannel Channel { get; set; }

        /// <summary>
        /// 数据自动同步
        /// </summary>
        public virtual bool DataAutoSync { get; set; }

        /// <summary>
        /// 历史消耗
        /// </summary>
        public virtual decimal TotalCost { get; set; }

        /// <summary>
        /// 总下单数
        /// </summary>
        public virtual decimal TotalOrder { get; set; }

        public string AccessToken { get; set; }

        public virtual DateTime? AccessTokenExpiresIn { get; set; }

        public virtual string RefreshToken { get; set; }

        public virtual DateTime? RefreshTokenExpiresIn { get; set; }

        /// <summary>
        /// 账户余额
        /// </summary>
        public virtual decimal Balance { get; set; }

        public bool IsAuth()
        {
            return !this.AccessToken.IsNullOrEmpty();
        }

        public bool IsAuthExpires()
        {
            return !this.AccessTokenExpiresIn.HasValue || (this.AccessTokenExpiresIn < DateTime.UtcNow);
        }
    }
}
