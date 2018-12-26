using Abp.AutoMapper;
using System;
using Vapps.Enums;

namespace Vapps.Authorization.Accounts.Cache
{
    [AutoMapFrom(typeof(Account))]
    public class AccountCacheItem
    {
        public int? TenantId { get; set; }

        public long UserId { get; set; }

        public long? UserLinkId { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public string NickName { get; set; }

        public int ProvinceId { get; set; }

        public string Province { get; set; }

        public int CityId { get; set; }

        public string City { get; set; }

        public int DistrictId { get; set; }

        public string District { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int SelectedAddress { get; set; }

        public GenderType Gender { get; set; }

        public virtual long ProfilePictureId { get; set; }

        public string ProfilePictureUrl { get; set; }

        public DateTime? LastActiveTime { get; set; }
    }
}
