using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Castle.Core.Logging;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vapps.Authorization.Accounts;
using Vapps.Authorization.Accounts.Cache;
using Vapps.Authorization.Users;
using Vapps.Messages;
using Vapps.WeChat.Core;
using Vapps.WeChat.Core.TemplateMessages.Cache;
using Vapps.WeChat.Core.Users;
using Vapps.WeChat.Operation;

namespace Vapps.WeChat.TemplateMessage
{
    public class TemplateMessageSender : ITemplateMessageSender, ITransientDependency
    {
        private readonly ILogger _logger;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly ITokenizer _tokenizer;
        private readonly ITemplateMessageCache _templateMessageCache;

        private readonly IUserAccountManager _userAccountManager;
        private readonly ITenantCache _tenantCache;
        private readonly IAccountCache _accountCache;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly WeChatUserManager _weChatUserManager;
        private readonly UserStore _userStore;
        private readonly WeChatCommonHepler _wechatCommonHepler;
        private readonly CommonOperation _commonOperation;

        public TemplateMessageSender(ILogger logger,
            IMessageTokenProvider messageTokenProvider,
            ITokenizer tokenizer,
            ITemplateMessageCache templateMessageCache,
            IUserAccountManager userAccountManager,
            ITenantCache tenantCache,
            IAccountCache accountCache,
            IUnitOfWorkManager unitOfWorkManager,
            UserStore userStore,
            WeChatUserManager weChatUserManager,
            WeChatCommonHepler wechatCommonHepler,
            CommonOperation commonOperation)
        {
            this._logger = logger;
            this._tokenizer = tokenizer;
            this._wechatCommonHepler = wechatCommonHepler;
            this._messageTokenProvider = messageTokenProvider;
            this._templateMessageCache = templateMessageCache;
            this._userAccountManager = userAccountManager;
            this._weChatUserManager = weChatUserManager;
            this._tenantCache = tenantCache;
            this._userStore = userStore;
            this._commonOperation = commonOperation;
            this._accountCache = accountCache;
            this._unitOfWorkManager = unitOfWorkManager;
        }

        #region Methods


        #endregion

        #region Utilities

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="toUserId"></param>
        /// <param name="messageId"></param>
        /// <param name="user"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        private async Task<WeChatOperationResult> SendTemplateMesage(string toUserId,
            int messageId = 0,
            User user = null,
            AccountCacheItem account = null)
        {
            var result = new WeChatOperationResult();

            var message = _templateMessageCache.Get(messageId);

            return await SendTemplateMesageAsync(toUserId: toUserId,
                message: message,
                user: user,
                account: account);
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="toUserId"></param>
        /// <param name="message"></param>
        /// <param name="user"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        private async Task<WeChatOperationResult> SendTemplateMesageAsync(string toUserId,
            TemplateMessageCacheItem message,
            User user = null,
            AccountCacheItem account = null)
        {
            var result = new WeChatOperationResult();

            if (message == null)
                return result;
            try
            {
                var data = GetTemplateMessageData(message: message,
                user: user,
                account: account);


                string urlValue = GetTemplateMessageItemValue(message.Url,
                user: user,
                account: account);

                var sendResult = await TemplateApi.SendTemplateMessageAsync(await _commonOperation.GetAccessTokenAsync(), toUserId, message.TemplateId, urlValue, data);

                if (sendResult.errcode != Senparc.Weixin.ReturnCode.请求成功)
                    result.Fail();
                else
                    result.Success();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                //await _commonOperation.CheckAccessTokenExpiresAsync(ex);

                //result.Fail(ex.Message, (int)ex.JsonResult.errcode);
            }

            return result;
        }

        /// <summary>
        /// 获取模板消息data
        /// </summary>
        /// <param name="message"></param>
        /// <param name="user"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        private Dictionary<string, TemplateDataItem> GetTemplateMessageData(TemplateMessageCacheItem message,
            User user = null,
            AccountCacheItem account = null)
        {
            Dictionary<string, TemplateDataItem> data = new Dictionary<string, TemplateDataItem>();
            string firstDataValue = GetTemplateMessageItemValue(message.FirstData,
                user: user,
                account: account);

            data.Add(WeChatConsts.FirstDataName, new TemplateDataItem(firstDataValue, message.FirstDataColor));

            foreach (var item in message.Items)
            {
                string value = GetTemplateMessageItemValue(item.DataValue,
                    user: user,
                    account: account);

                data.Add(item.DataName, new TemplateDataItem(value, item.Color));
            }

            string remarkDataValue = GetTemplateMessageItemValue(message.RemarkData,
                user: user,
                account: account);

            data.Add(WeChatConsts.RemarkDataName, new TemplateDataItem(remarkDataValue, message.RemarkDataColor));

            return data;
        }

        /// <summary>
        /// 获取模板消息子项的值
        /// </summary>
        /// <param name="valueWithToken"></param>
        /// <param name="user"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        private string GetTemplateMessageItemValue(string valueWithToken,
            User user = null,
            AccountCacheItem account = null)
        {
            var tokens = new List<Token>();

            if (user != null)
                _messageTokenProvider.AddUserTokens(tokens, user);

            //if (account != null)
            //    _messageTokenProvider.add(tokens, product, _workContext.WorkingLanguage.Id);

            string value = _tokenizer.Replace(valueWithToken, tokens, false);

            return value;
        }

        #endregion

    }
}
