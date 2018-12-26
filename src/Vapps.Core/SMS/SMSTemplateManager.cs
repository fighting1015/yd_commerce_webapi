using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Vapps.Authorization.Users;
using Vapps.Identity.Cache;
using Vapps.Messages;
using Vapps.SMS.Cache;

namespace Vapps.SMS
{
    public class SMSTemplateManager : VappsDomainServiceBase, ISMSTemplateManager
    {
        public IRepository<SMSTemplate, long> SMSTemplateRepository { get; }
        public IQueryable<SMSTemplate> SMSTemplates => SMSTemplateRepository.GetAll().AsNoTracking();
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly ISMSTemplateCache _smsTemplateCache;

        public SMSTemplateManager(IRepository<SMSTemplate, long> smsTemplateRepository,
            IMessageTokenProvider messageTokenProvider,
            ISMSTemplateCache smsTemplateCache)
        {
            this.SMSTemplateRepository = smsTemplateRepository;
            this._messageTokenProvider = messageTokenProvider;
            this._smsTemplateCache = smsTemplateCache;
        }

        /// <summary>
        /// 获取填充指令后的短信模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual SendSMSTemplateResult GetSMSTemplateResultById(long id, User user = null,
           VerificationCodeCacheItem verificationCode = null)
        {
            var smsTempalte = _smsTemplateCache.Get(id);

            if (smsTempalte == null)
                return null;

            var sendSmsTempalteResult = ObjectMapper.Map<SendSMSTemplateResult>(smsTempalte);
            foreach (var item in sendSmsTempalteResult.Items)
            {
                item.DataItemValue = _messageTokenProvider.ReplaceToken(templateWithToken: item.DataItemValue,
                    user: user, verificationCode: verificationCode);
            }

            return sendSmsTempalteResult;
        }

        public virtual async Task<SMSTemplate> FindByCodeAsync(string code)
        {
            return await SMSTemplateRepository.FirstOrDefaultAsync(tempalte => tempalte.TemplateCode == code);
        }

        public virtual async Task<SMSTemplate> FindByIdAsync(long id)
        {
            return await SMSTemplateRepository.FirstOrDefaultAsync(id);
        }

        public virtual async Task<SMSTemplate> GetByIdAsync(long id)
        {
            return await SMSTemplateRepository.GetAsync(id);
        }

        public virtual async Task CreateAsync(SMSTemplate template)
        {
            await SMSTemplateRepository.InsertAsync(template);
        }

        public virtual async Task UpdateAsync(SMSTemplate template)
        {
            await SMSTemplateRepository.UpdateAsync(template);
        }

        public virtual async Task DeleteAsync(SMSTemplate template)
        {
            await SMSTemplateRepository.DeleteAsync(template);
        }
    }
}
