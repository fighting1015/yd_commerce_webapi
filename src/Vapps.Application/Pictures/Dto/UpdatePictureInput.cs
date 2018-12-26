using Abp.Application.Services.Dto;

namespace Vapps.Pictures.Dto
{
    public class UpdatePictureInput : EntityDto<long>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分组Id
        /// </summary>
        public int? GroupId { get; set; }
    }
}
