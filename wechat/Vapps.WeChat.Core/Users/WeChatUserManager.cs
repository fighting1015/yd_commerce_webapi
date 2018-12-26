using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System.Linq;
using System.Threading.Tasks;
using Vapps.Authorization.Users;

namespace Vapps.WeChat.Core.Users
{
    public class WeChatUserManager : DomainService
    {
        public IRepository<WeChatUser, long> WeChatUserRepository { get; private set; }

        public IQueryable<WeChatUser> WeChatUsers => WeChatUserRepository.GetAll();

        public WeChatUserManager(IRepository<WeChatUser, long> wechatUserRepository)
        {
            WeChatUserRepository = wechatUserRepository;
        }

        /// <summary>
        /// 根据Id查找微信用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<WeChatUser> FindByIdAsync(int id)
        {
            return await WeChatUserRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据 open id 查找微信用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<WeChatUser> FindByOnenIdAsync(string openId)
        {
            return await WeChatUserRepository.FirstOrDefaultAsync(wu => wu.OpenId == openId);
        }


        /// <summary>
        /// 根据Id获取微信用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<WeChatUser> FindByUserIdAndMpIdAsync(long userId, int mpId)
        {
            if (userId <= 0)
                return null;

            return await WeChatUserRepository.FirstOrDefaultAsync(t => t.UserId == userId && t.MpId == mpId);
        }


        /// <summary>
        /// 根据Id获取微信用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<WeChatUser> GetByIdAsync(int id)
        {
            return await WeChatUserRepository.GetAsync(id);
        }

        /// <summary>
        /// 创建或更新微信用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual async Task CreateOrUpdateAsync(ExternalLoginEvent eventData)
        {
            var wechatUser = await FindByOnenIdAsync(eventData.ExternalLoginInfo.ProviderKey);
            if (wechatUser == null)
            {
                wechatUser = new WeChatUser()
                {
                    OpenId = eventData.ExternalLoginInfo.ProviderKey,
                    Authorization = true,
                    MpId = 0,
                    UserId = eventData.User.Id,
                };
                await CreateAsync(wechatUser);
            }
            else
            {
                if (wechatUser.UserId != eventData.User.Id)
                    wechatUser.UserId = eventData.User.Id;

                if (!wechatUser.Authorization)
                    wechatUser.Authorization = true;

                await UpdateAsync(wechatUser);
            }
        }

        /// <summary>
        /// 创建微信用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(WeChatUser user)
        {
            await WeChatUserRepository.InsertAsync(user);
        }

        /// <summary>
        /// 更新微信用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(WeChatUser user)
        {
            await WeChatUserRepository.UpdateAsync(user);
        }

        /// <summary>
        /// 删除微信用户
        /// </summary>
        /// <param name="templateMessage"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(WeChatUser user)
        {
            await WeChatUserRepository.DeleteAsync(user);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(int id)
        {
            var templateMessage = await WeChatUserRepository.GetAsync(id);
            await WeChatUserRepository.DeleteAsync(templateMessage);
        }
    }
}