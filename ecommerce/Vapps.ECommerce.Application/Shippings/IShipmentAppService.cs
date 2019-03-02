using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using Vapps.Dto;
using Vapps.ECommerce.Shippings.Dto;
using Vapps.ECommerce.Shippings.Dto.Shipments;

namespace Vapps.ECommerce.Shippings
{
    public interface IShipmentAppService
    {
        /// <summary>
        /// 获取所有发货记录
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<ShipmentListDto>> GetShipments(GetShipmentsInput input);

        /// <summary>
        /// 获取发货记录
        /// </summary>
        /// <returns></returns>
        Task<GetShipmentForEditOutput> GetShipmentForEdit(NullableIdDto<int> input);

        /// <summary>
        /// 获取发货记录
        /// </summary>
        /// <returns></returns>
        Task<EntityDto<long>> CreateOrUpdateShipment(CreateOrUpdateShipmentInput input);

        /// <summary>
        /// 删除发货记录
        /// </summary>
        /// <returns></returns>
        Task DeleteShipment(BatchInput<int> input);
    }
}
