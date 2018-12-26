using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Addresses
{
    public class AddressManager : VappsDomainServiceBase, IAddressManager
    {
        public IRepository<Address, long> AddressRepository { get; }

        public IQueryable<Address> Addresss => AddressRepository.GetAll().AsNoTracking();

        /// <summary>
        /// 根据id 查找地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Address> FindByIdAsync(long id)
        {
            return await AddressRepository.FirstOrDefaultAsync(id);

        }

        /// <summary>
        /// 根据id 获取地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Address> GetByIdAsync(long id)
        {
            return await AddressRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加地址
        /// </summary>
        /// <param name="address"></param>
        public async Task CreateAsync(Address address)
        {
            await AddressRepository.InsertAsync(address);
        }

        /// <summary>
        /// 修改地址
        /// </summary>
        /// <param name="address"></param>
        public async Task UpdateAsync(Address address)
        {
            await AddressRepository.UpdateAsync(address);
        }

        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="address"></param>
        public async Task DeleteAsync(Address address)
        {
            await AddressRepository.DeleteAsync(address);

        }

        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="id"></param>
        public async Task DeleteAsync(long id)
        {
            var address = await GetByIdAsync(id);

            await AddressRepository.DeleteAsync(address);
        }
    }
}
