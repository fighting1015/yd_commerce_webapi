using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Addresses
{
    public interface IAddressManager
    {
        IRepository<Address, long> AddressRepository { get; }

        IQueryable<Address> Addresss { get; }

        /// <summary>
        /// 根据id 查找地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Address> FindByIdAsync(long id);

        /// <summary>
        /// 根据id 获取地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Address> GetByIdAsync(long id);

        /// <summary>
        /// 添加地址
        /// </summary>
        /// <param name="address"></param>
        Task CreateAsync(Address address);

        /// <summary>
        /// 修改地址
        /// </summary>
        /// <param name="address"></param>
        Task UpdateAsync(Address address);

        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="address"></param>
        Task DeleteAsync(Address address);

        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(long id);
    }
}
