using Abp.Application.Services.Dto;
using Abp.Timing;
using System;
using Vapps.Payments;

namespace Vapps.Sessions.Dto
{
    public class TenantLoginInfoDto : EntityDto
    {
        /// <summary>
        /// 租户名称(系统名称)
        /// </summary>
        public string TenancyName { get; set; }

        /// <summary>
        /// 租户名称(显示用)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Logo Id
        /// </summary>
        public long LogoId { get; set; }

        /// <summary>
        /// Logo url 
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// Logo 文件类型
        /// </summary>
        public string LogoFileType { get; set; }

        /// <summary>
        /// 是否试用中（在试用有效期内）
        /// </summary>
        public bool IsInTrialPeriod { get; set; }

        /// <summary>
        /// 是否曾经试用过
        /// </summary>
        public bool HadTrialed { get; set; }

        /// <summary>
        /// 版本信息
        /// </summary>
        public EditionInfoDto Edition { get; set; }

        /// <summary>
        /// 创建时间(Utc)
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreationTimeString { get; set; }

        /// <summary>
        /// 订阅结束时间
        /// </summary>
        public DateTime? SubscriptionEndDateUtc { get; set; }

        /// <summary>
        /// 订阅结束时间(字符串)
        /// </summary>
        public string SubscriptionDateString { get; set; }

        public bool IsInTrial()
        {
            return IsInTrialPeriod;
        }

        public bool SubscriptionIsExpiringSoon()
        {
            if (SubscriptionEndDateUtc.HasValue)
            {
                return Clock.Now.ToUniversalTime().AddDays(AppConsts.SubscriptionExpireNootifyDayCount) >= SubscriptionEndDateUtc.Value;
            }

            return false;
        }

        public int GetSubscriptionExpiringDayCount()
        {
            if (!SubscriptionEndDateUtc.HasValue)
            {
                return 0;
            }

            return Convert.ToInt32(SubscriptionEndDateUtc.Value.ToUniversalTime().Subtract(Clock.Now.ToUniversalTime()).TotalDays);
        }
    }
}