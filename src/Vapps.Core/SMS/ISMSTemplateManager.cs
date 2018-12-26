using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.Authorization.Users;
using Vapps.Identity.Cache;
using Vapps.SMS.Cache;

namespace Vapps.SMS
{
    public interface ISMSTemplateManager
    {
        IRepository<SMSTemplate, long> SMSTemplateRepository { get; }

        IQueryable<SMSTemplate> SMSTemplates { get; }

        /// <summary>
        /// 获取填充指令后的短信模板
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        SendSMSTemplateResult GetSMSTemplateResultById(long id, User user = null,
           VerificationCodeCacheItem verificationCode = null);

        /// <summary>
        /// 根据模板代码获取短信模板
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<SMSTemplate> FindByCodeAsync(string code);

        /// <summary>
        /// 根据Id获取短信模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SMSTemplate> FindByIdAsync(long id);

        /// <summary>
        /// 根据Id获取短信模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SMSTemplate> GetByIdAsync(long id);

        /// <summary>
        /// 创建短信模板
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        Task CreateAsync(SMSTemplate template);

        /// <summary>
        /// 更新短信模板
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        Task UpdateAsync(SMSTemplate template);

        /// <summary>
        /// 删除短信模板
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        Task DeleteAsync(SMSTemplate template);
    }
}
