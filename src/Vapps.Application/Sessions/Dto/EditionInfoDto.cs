using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations.Schema;
using Vapps.Editions;

namespace Vapps.Sessions.Dto
{
    public class EditionInfoDto : EntityDto
    {
        /// <summary>
        /// ��ʾ����
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public int? TrialDayCount { get; set; }

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
        /// �Ƿ���߰汾
        /// </summary>
        public bool IsHighestEdition { get; set; }

        /// <summary>
        /// �Ƿ����
        /// </summary>
        public bool IsFree { get; set; }

        public void SetEditionIsHighest(SubscribableEdition topEdition)
        {
            if (topEdition == null)
            {
                return;
            }

            IsHighestEdition = Id == topEdition.Id;
        }
    }
}