using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Vapps.Authorization.Users;

namespace Vapps.Organizations.Dto
{
    [AutoMapFrom(typeof(User))]
    public class OrganizationUnitUserListDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public long ProfilePictureId { get; set; }

        public DateTime AddedTime { get; set; }
    }
}