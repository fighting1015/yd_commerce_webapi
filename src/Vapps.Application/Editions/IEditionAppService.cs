using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Vapps.Editions.Dto;

namespace Vapps.Editions
{
    public interface IEditionAppService : IApplicationService
    {
        Task<ListResultDto<EditionListDto>> GetEditions();

        Task<GetEditionEditOutput> GetEditionForEdit(NullableIdDto input);

        Task CreateOrUpdateEdition(CreateOrUpdateEditionDto input);

        Task DeleteEdition(EntityDto input);

        /// <summary>
        /// 获取版本信息(Combobox)
        /// </summary>
        /// <param name="selectedEditionId">选择版本Id</param>
        /// <param name="addAllItem">添加所有</param>
        /// <param name="onlyFree">只获取免费版</param>
        /// <returns></returns>
        Task<List<SubscribableEditionComboboxItemDto>> GetEditionComboboxItems(int? selectedEditionId = null, bool addAllItem = false, bool onlyFree = false);

        /// <summary>
        /// 获取版本信息(下拉框选项)
        /// </summary>
        /// <returns></returns>
        Task<List<SelectListItem<int>>> GetEditionSelectList();
    }
}