using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using Vapps.Dto;
using Vapps.ECommerce.Customers.Dto;

namespace Vapps.ECommerce.Customers
{
    public interface ICustomerAppService
    {
        /// <summary>
        /// 获取所有客户
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<CustomerListDto>> GetCustomers(GetCustomersInput input);

        /// <summary>
        /// 获取客户详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<CustomerDetailDto> GetCustomerForEdit(NullableIdDto<int> input);

        /// <summary>
        /// 创建或更新客户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<EntityDto<long>> CreateOrUpdateCustomer(CreateOrUpdateCustomerInput input);

        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteCustomer(BatchInput<int> input);
    }
}
