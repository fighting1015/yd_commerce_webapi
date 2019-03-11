using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Customers
{
    public interface ICustomerManager
    {
        IRepository<Customer, long> CustomerRepository { get; }
        IQueryable<Customer> Customeres { get; }

        #region Customer

        /// <summary>
        /// 根据手机查找客户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Customer> FindByPhoneNumerAsync(string phoneNumer);

        /// <summary>
        /// 根据id查找客户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Customer> FindByIdAsync(long id);

        /// <summary>
        /// 根据id获取客户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Customer> GetByIdAsync(long id);

        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="customer"></param>
        Task CreateAsync(Customer customer);

        /// <summary>
        /// 修改客户
        /// </summary>
        /// <param name="customer"></param>
        Task UpdateAsync(Customer customer);

        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="customer"></param>
        Task DeleteAsync(Customer customer);

        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(long id);

        #endregion
    }
}
