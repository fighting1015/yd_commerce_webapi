using Abp.Auditing;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vapps.Authorization.Users;
using Vapps.Editions.Cache;
using Vapps.Media;
using Vapps.MultiTenancy;
using Vapps.Sessions.Dto;

namespace Vapps.Sessions
{
    public class SessionAppService : VappsAppServiceBase, ISessionAppService
    {
        private readonly IPictureManager _pictureManager;
        private readonly ISubscribableEditionCache _subscribableEditionCache;

        private readonly VappsTenantCache _tenantCache;

        public SessionAppService(IPictureManager pictureManager,
            ISubscribableEditionCache subscribableEditionCache,
            VappsTenantCache tenantCache)
        {
            this._pictureManager = pictureManager;
            this._subscribableEditionCache = subscribableEditionCache;
            this._tenantCache = tenantCache;
        }

        /// <summary>
        /// 获取最近登录信息
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>
                    {

                    }
                }
            };

            if (AbpSession.TenantId.HasValue)
            {
                var tenant = _tenantCache.Get(AbpSession.GetTenantId());
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(tenant);

                //output.Tenant = ObjectMapper
                //                    .Map<TenantLoginInfoDto>(await TenantManager
                //                        .Tenants
                //                        .Include(t => t.Edition)
                //                        .FirstAsync(t => t.Id == AbpSession.GetTenantId()));

                //output.Tenant.LogoUrl = await _pictureManager.GetPictureUrlAsync(output.Tenant.LogoId);
            }

            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
            }

            if (output.Tenant == null)
            {
                return output;
            }

            if (output.Tenant.Edition != null)
            {
                output.Tenant.Edition.IsHighestEdition = await IsEditionHighest(output.Tenant.Edition.Id);
            }

            output.Tenant.SubscriptionDateString = GetTenantSubscriptionDateString(output);
            output.Tenant.CreationTimeString = output.Tenant.CreationTime.ToString("d");

            return output;
        }

        private async Task<bool> IsEditionHighest(int editionId)
        {
            var topEdition = await _subscribableEditionCache.GetHighestAsync();
            if (topEdition == null)
            {
                return false;
            }

            return editionId == topEdition.Id;
        }

        private string GetTenantSubscriptionDateString(GetCurrentLoginInformationsOutput output)
        {
            return output.Tenant.SubscriptionEndDateUtc == null
                ? L("Unlimited")
                : output.Tenant.SubscriptionEndDateUtc?.ToString("d");
        }

        /// <summary>
        /// 更新登录状态
        /// </summary>
        /// <returns></returns>
        public async Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken()
        {
            if (AbpSession.UserId <= 0)
            {
                throw new Exception(L("ThereIsNoLoggedInUser"));
            }

            var user = await UserManager.GetUserAsync(AbpSession.ToUserIdentifier());
            user.SetSignInToken();
            return new UpdateUserSignInTokenOutput
            {
                SignInToken = user.SignInToken,
                EncodedUserId = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Id.ToString())),
                EncodedTenantId = user.TenantId.HasValue
                    ? Convert.ToBase64String(Encoding.UTF8.GetBytes(user.TenantId.Value.ToString()))
                    : ""
            };
        }
    }
}