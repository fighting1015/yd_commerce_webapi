using Abp.MultiTenancy;
using System;
using Vapps.Editions.Cache;

namespace Vapps.MultiTenancy
{
    public class VappsTenantCacheItem : TenantCacheItem
    {
        public new const string CacheName = "TenantCache";
        public new const string ByNameCacheName = "TenantByNameCache";

        public string Tagline { get; set; }

        public long LogoId { get; set; }

        public string Description { get; set; }

        public string LogoUrl { get; set; }

        public long BackgroundPictureId { get; set; }

        public string BackgroundPictureUrl { get; set; }

        public DateTime? SubscriptionEndDateUtc { get; set; }

        public SubscribableEditionCacheItem Edition { get; set; }

        public bool IsInTrialPeriod { get; set; }

        public bool HadTrialed { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
