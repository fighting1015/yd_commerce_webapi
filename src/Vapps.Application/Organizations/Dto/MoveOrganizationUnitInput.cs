using System.ComponentModel.DataAnnotations;

namespace Vapps.Organizations.Dto
{
    public class MoveOrganizationUnitInput
    {
        /// <summary>
        /// 组织单位Id(大于0)
        /// </summary>
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        /// <summary>
        /// 目标父节点Id(可空)
        /// </summary>
        public long? NewParentId { get; set; }
    }
}