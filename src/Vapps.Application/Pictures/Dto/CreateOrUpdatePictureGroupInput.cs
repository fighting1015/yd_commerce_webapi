using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Vapps.Pictures.Dto
{
    public class CreateOrUpdatePictureGroupInput : NullableIdDto<long>
    {
        [Required]
        public string Name { get; set; }
    }
}
