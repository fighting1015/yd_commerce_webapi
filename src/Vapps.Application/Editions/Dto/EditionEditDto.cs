using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace Vapps.Editions.Dto
{
    /// <summary>
    /// �汾��Ϣ�༭ DTO
    /// </summary>
    [AutoMap(typeof(SubscribableEdition))]
    public class EditionEditDto
    {
        /// <summary>
        /// Id(�ɿ�)
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// ��ʾ����
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// �¼۸�
        /// </summary>
        public decimal? MonthlyPrice { get; set; }

        /// <summary>
        /// ���ȼ۸�
        /// </summary>
        public decimal? SeasonPrice { get; set; }

        /// <summary>
        /// ��۸�
        /// </summary>
        public decimal? AnnualPrice { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public int? TrialDayCount { get; set; }


        public int? WaitingDayAfterExpire { get; set; }

        /// <summary>
        /// ���ں�
        /// </summary>
        public int? ExpiringEditionId { get; set; }
    }
}