using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Customers
{
    public class CustomerManager : VappsDomainServiceBase, ICustomerManager
    {
        public IRepository<Customer, long> CustomerRepository { get; }

        public IQueryable<Customer> Customeres => CustomerRepository.GetAll().AsNoTracking();

        public CustomerManager(IRepository<Customer, long> customerRepository)
        {
            this.CustomerRepository = customerRepository;
        }

        #region Customer

        /// <summary>
        /// 根据手机查找客户
        /// </summary>
        /// <param name="phoneNumer"></param>
        /// <returns></returns>
        public async Task<Customer> FindByPhoneNumerAsync(string phoneNumer)
        {
            return await CustomerRepository.FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumer);
        }

        /// <summary>
        /// 根据id查找客户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Customer> FindByIdAsync(long id)
        {
            return await this.CustomerRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据id获取客户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Customer> GetByIdAsync(long id)
        {
            return await CustomerRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="customer"></param>
        public async Task CreateAsync(Customer customer)
        {
            await CustomerRepository.InsertAsync(customer);
        }

        /// <summary>
        /// 修改客户
        /// </summary>
        /// <param name="customer"></param>
        public async Task UpdateAsync(Customer customer)
        {
            await CustomerRepository.UpdateAsync(customer);
        }

        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="customer"></param>
        public async Task DeleteAsync(Customer customer)
        {
            await CustomerRepository.DeleteAsync(customer);
        }

        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="id"></param>
        public async Task DeleteAsync(long id)
        {
            if (id <= 0)
                return;

            var order = await CustomerRepository.FirstOrDefaultAsync(id);

            if (order != null)
                await CustomerRepository.DeleteAsync(order);
        }

        #endregion
    }
}
