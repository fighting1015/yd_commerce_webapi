using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System.Threading.Tasks;

namespace Vapps.WeChat.Core.TemplateMessages
{
    public class TemplateMessageManager : DomainService
    {
        protected IRepository<TemplateMessage> TemplateMessageRepository { get; private set; }
        protected IRepository<TemplateMessageItem> TemplateMessageItemRepository { get; private set; }

        public TemplateMessageManager(IRepository<TemplateMessage> templateMessageRepository,
            IRepository<TemplateMessageItem> templateMessageItemRepository)
        {
            TemplateMessageRepository = templateMessageRepository;
            TemplateMessageItemRepository = templateMessageItemRepository;
        }

        /// <summary>
        /// 根据Id查找模板消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TemplateMessage> FindByIdAsync(int id)
        {
            return await TemplateMessageRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据Id获取模板消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TemplateMessage> GetByIdAsync(int id)
        {
            return await TemplateMessageRepository.GetAsync(id);
        }

        /// <summary>
        /// 创建模板消息
        /// </summary>
        /// <param name="templateMessage"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(TemplateMessage templateMessage)
        {
            await TemplateMessageRepository.InsertAsync(templateMessage);
        }

        /// <summary>
        /// 更新模板消息
        /// </summary>
        /// <param name="templateMessage"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(TemplateMessage templateMessage)
        {
            await TemplateMessageRepository.UpdateAsync(templateMessage);
        }

        /// <summary>
        /// 删除模板消息
        /// </summary>
        /// <param name="templateMessage"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(TemplateMessage templateMessage)
        {
            await TemplateMessageRepository.DeleteAsync(templateMessage);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(int id)
        {
            var templateMessage = await TemplateMessageRepository.GetAsync(id);
            await TemplateMessageRepository.DeleteAsync(templateMessage);
        }

        /// <summary>
        /// 根据Id获取模板消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TemplateMessageItem> GetTemplateMessageItemByName(int templateMessageId, string name)
        {
            if (templateMessageId <= 0 || string.IsNullOrEmpty(name))
                return null;

            return await TemplateMessageItemRepository.FirstOrDefaultAsync(t => t.TemplateMessageId == templateMessageId && t.DataName == name);
        }

        /// <summary>
        /// 根据Id删除模板消息项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task DeleteItemAsync(int id)
        {
            var item = await TemplateMessageItemRepository.GetAsync(id);

            await TemplateMessageItemRepository.DeleteAsync(item);
        }
    }
}
