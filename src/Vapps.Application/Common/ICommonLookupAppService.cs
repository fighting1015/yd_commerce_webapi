using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Vapps.Common.Dto;
using Vapps.Dto;
using Vapps.Editions.Dto;

namespace Vapps.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        /// <summary>
        /// 获取版本信息(Combobox数据源)
        /// </summary>
        /// <param name="onlyFreeItems"></param>
        /// <returns></returns>
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);

        /// <summary>
        /// 获取默认版本名称
        /// </summary>
        /// <returns></returns>
        GetDefaultEditionNameOutput GetDefaultEditionName();

        /// <summary>
        /// 根据枚举名称获取下拉框数据源(值类型)
        /// </summary>
        /// <param name="enumName">枚举类型名称</param>
        /// <returns></returns>
        List<SelectListItemDto<int>> GetEnumSelectItem(string enumName);

        /// <summary>
        /// 根据枚举名称获取下拉框数据源(字符串)
        /// </summary>
        /// <param name="enumName">枚举类型名称</param>
        /// <returns></returns>
        List<SelectListItemDto<string>> GetEnumSelectItemString(string enumName);
    }
}