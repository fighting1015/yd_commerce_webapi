using Abp.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Vapps.Addresses;
using Vapps.Enums;

namespace Vapps.Authorization.Accounts
{
    /// <summary>
    /// Represents an account in system
    /// </summary>
    public class Account : UserAccount
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public virtual int ProvinceId { get; set; }

        public virtual string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public virtual int CityId { get; set; }

        public virtual string City { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public virtual int DistrictId { get; set; }

        public virtual string District { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public virtual DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// 最后选中地址
        /// </summary>
        public virtual int SelectedAddress { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual GenderType Gender { get; set; }

        /// <summary>
        /// 头像Id
        /// </summary>
        public long ProfilePictureId { get; set; }

        /// <summary>
        /// 头像Url(第三方,更新备用,不能直接用作显示)
        /// </summary>
        public virtual string ProfilePictureUrl { get; set; }

        /// <summary>
        /// 最后活跃时间
        /// </summary>
        public virtual DateTime? LastActiveTime { get; set; }

        /// <summary>
        /// 地址集合 
        /// </summary>
        [ForeignKey("AccountId")]
        public virtual ICollection<AccountAddress> AccountAddresses { get; set; }
    }
}
