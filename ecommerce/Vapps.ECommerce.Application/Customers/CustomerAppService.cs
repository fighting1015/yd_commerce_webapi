using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Vapps.Authorization;
using Vapps.Dto;
using Vapps.ECommerce.Customers.Dto;
using Vapps.Media;

namespace Vapps.ECommerce.Customers
{
    [AbpAuthorize(BusinessCenterPermissions.CustomerManage.Customer.Self)]
    public class CustomerAppService : VappsAppServiceBase, ICustomerAppService
    {
        private readonly ICustomerManager _customerManager;
        private readonly ICacheManager _cacheManager;
        private readonly IPictureManager _pictureManager;

        public CustomerAppService(ICustomerManager customerManager,
            ICacheManager cacheManager,
            IPictureManager pictureManager)
        {
            this._customerManager = customerManager;
            this._cacheManager = cacheManager;
            this._pictureManager = pictureManager;
        }

        /// <summary>
        /// 获取所有客户
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<CustomerListDto>> GetCustomers(GetCustomersInput input)
        {
            var query = _customerManager
               .Customeres
               .WhereIf(!input.Name.IsNullOrWhiteSpace(), r => r.Name.Contains(input.Name))
               .WhereIf(!input.PhoneNumber.IsNullOrWhiteSpace(), r => r.PhoneNumber.Contains(input.PhoneNumber))
               .WhereIf(input.ConsumesForm > 0, r => r.TotalConsumes > input.ConsumesForm)
               .WhereIf(input.ConsumesTo > 0, r => r.TotalConsumes < input.ConsumesTo);

            var customerCount = await query.CountAsync();

            var customers = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var catalogyListDtos = ObjectMapper.Map<List<CustomerListDto>>(customers);
            return new PagedResultDto<CustomerListDto>(
                customerCount,
                catalogyListDtos);
        }

        /// <summary>
        /// 获取客户详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<CustomerDetailDto> GetCustomerForEdit(NullableIdDto<int> input)
        {
            var customer = await _customerManager.GetByIdAsync(input.Id.Value);

            return PrepareCustomerDetailDto(customer);

        }

        /// <summary>
        /// 创建或更新客户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EntityDto<long>> CreateOrUpdateCustomer(CreateOrUpdateCustomerInput input)
        {
            Customer customer;
            if (input.Id > 0)
            {
                customer = await UpdateCustomerAsync(input);
            }
            else
            {
                customer = await CreateCustomerAsync(input);
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return new EntityDto<long> { Id = customer.Id };
        }

        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteCustomer(BatchInput<int> input)
        {
            if (input.Ids == null || input.Ids.Count() <= 0)
            {
                return;
            }

            foreach (var id in input.Ids)
            {
                await _customerManager.DeleteAsync(id);
            }
        }

        #region Utility

        /// <summary>
        /// 客户详情
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        private CustomerDetailDto PrepareCustomerDetailDto(Customer customer)
        {
            var dto = ObjectMapper.Map<CustomerDetailDto>(customer);

            return dto;
        }

        /// <summary>
        /// 更新客户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.CustomerManage.Customer.Edit)]
        protected virtual async Task<Customer> UpdateCustomerAsync(CreateOrUpdateCustomerInput input)
        {
            var logistics = await _customerManager.FindByIdAsync(input.Id);

            ObjectMapper.Map(input, logistics);

            await _customerManager.UpdateAsync(logistics);

            return logistics;
        }

        /// <summary>
        /// 添加客户
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.CustomerManage.Customer.Create)]
        protected virtual async Task<Customer> CreateCustomerAsync(CreateOrUpdateCustomerInput input)
        {
            var customer = new Customer()
            {
                Name = input.Name,
                PhoneNumber = input.PhoneNumber,
                AvatarPictureId = input.AvatarPictureId,
                TotalConsumes = input.TotalConsumes,
                TotalOrderNum = input.TotalOrderNum,
            };

            await _customerManager.CreateAsync(customer);

            return customer;
        }

        #endregion
    }
}
