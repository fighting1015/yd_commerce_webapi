using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vapps.Common.Dto;
using Vapps.Dto;
using Vapps.SMS.Dto;

namespace Vapps.SMS
{
    public interface ISMSTemplateAppService
    {
        /// <summary>
        /// 获取所有短信模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<SMSTemplateListDto>> GetSMSTemplates(GetSMSTemplatesInput input);

        /// <summary>
        /// 获取所有可用短信模板(下拉框)
        /// </summary>
        /// <returns></returns>
        Task<List<SelectListItemDto<long>>> GetAvailableSMSTemplates();

        /// <summary>
        /// 获取短信模板详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetSMSTemplateForEditDto> GetSMSTemplateForEdit(NullableIdDto<long> input);

        /// <summary>
        /// 获取可用短信供应商
        /// </summary>
        /// <returns></returns>
        List<SMSProviderInfoDto> GetSMSProviders();

        /// <summary>
        /// 新建/更新短信模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateSMSTemplateInput input);

        /// <summary>
        /// 删除短信模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<long> input);
    }
}
