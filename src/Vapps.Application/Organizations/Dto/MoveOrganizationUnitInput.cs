using System.ComponentModel.DataAnnotations;

namespace Vapps.Organizations.Dto
{
    public class MoveOrganizationUnitInput
    {
        /// <summary>
        /// ��֯��λId(����0)
        /// </summary>
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        /// <summary>
        /// Ŀ�길�ڵ�Id(�ɿ�)
        /// </summary>
        public long? NewParentId { get; set; }
    }
}