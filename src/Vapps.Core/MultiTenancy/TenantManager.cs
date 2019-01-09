using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Notifications;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using Vapps.Authorization.Roles;
using Vapps.Authorization.Users;
using Vapps.Editions;
using Vapps.Identity;
using Vapps.MultiTenancy.Demo;
using Vapps.Notifications;
using Vapps.Payments;

namespace Vapps.MultiTenancy
{
    /// <summary>
    /// Tenant manager.
    /// </summary>
    public class TenantManager : AbpTenantManager<Tenant, User>
    {
        public IAbpSession AbpSession { get; set; }

        private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;
        private readonly IAppNotifier _appNotifier;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<SubscribableEdition> _subscribableEditionRepository;
        private readonly IRepository<SubscriptionPayment, long> _subscriptionPaymentRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUserEmailer _userEmailer;
        private readonly RoleManager _roleManager;
        private readonly TenantDemoDataBuilder _demoDataBuilder;
        private readonly UserManager _userManager;
        private readonly UserStore _userStore;

        public TenantManager(
            IAbpZeroDbMigrator abpZeroDbMigrator,
            IAbpZeroFeatureValueStore featureValueStore,
            IAppNotifier appNotifier,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IPasswordHasher<User> passwordHasher,
            IRepository<SubscriptionPayment, long> subscriptionPaymentRepository,
            IRepository<Tenant> tenantRepository,
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IUserEmailer userEmailer,
            IRepository<SubscribableEdition> subscribableEditionRepository,
            EditionManager editionManager,
            RoleManager roleManager,
            TenantDemoDataBuilder demoDataBuilder,
            UserManager userManager,
            UserStore userStore) : base(
                tenantRepository,
                tenantFeatureRepository,
                editionManager,
                featureValueStore
            )
        {
            AbpSession = NullAbpSession.Instance;

            _unitOfWorkManager = unitOfWorkManager;
            _roleManager = roleManager;
            _userEmailer = userEmailer;
            _demoDataBuilder = demoDataBuilder;
            _userManager = userManager;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _appNotifier = appNotifier;
            _abpZeroDbMigrator = abpZeroDbMigrator;
            _passwordHasher = passwordHasher;
            _userStore = userStore;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _subscribableEditionRepository = subscribableEditionRepository;
        }

        /// <summary>
        /// 创建租户
        /// </summary>
        /// <param name="tenancyName"></param>
        /// <param name="name"></param>
        /// <param name="adminPassword"></param>
        /// <param name="adminEmailAddress"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="connectionString"></param>
        /// <param name="isActive"></param>
        /// <param name="editionId"></param>
        /// <param name="shouldChangePasswordOnNextLogin"></param>
        /// <param name="sendActivationEmail"></param>
        /// <param name="subscriptionEndDate"></param>
        /// <param name="isInTrialPeriod"></param>
        /// <param name="emailActivationLink"></param>
        /// <returns></returns>
        public virtual async Task<int> CreateWithAdminUserAsync(
            string tenancyName,
            string name,
            string adminPassword,
            string adminEmailAddress,
            string phoneNumber,
            string connectionString,
            bool isActive,
            int? editionId,
            bool shouldChangePasswordOnNextLogin,
            bool sendActivationEmail,
            DateTime? subscriptionEndDate,
            bool isInTrialPeriod,
            string emailActivationLink)
        {
            int newTenantId;
            long newAdminId;

            await CheckEditionAsync(editionId, isInTrialPeriod);

            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                //Create tenant
                var tenant = new Tenant(tenancyName, name)
                {
                    IsActive = isActive,
                    EditionId = editionId,
                    SubscriptionEndDateUtc = subscriptionEndDate?.ToUniversalTime(),
                    IsInTrialPeriod = isInTrialPeriod,
                    ConnectionString = connectionString.IsNullOrWhiteSpace() ? null : SimpleStringCipher.Instance.Encrypt(connectionString)
                };

                await CreateAsync(tenant);
                await _unitOfWorkManager.Current.SaveChangesAsync(); //To get new tenant's id.

                //Create tenant database
                _abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);

                //We are working entities of new tenant, so changing tenant filter
                using (_unitOfWorkManager.Current.SetTenantId(tenant.Id))
                {
                    //Create static roles for new tenant
                    CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));
                    await _unitOfWorkManager.Current.SaveChangesAsync(); //To get static role ids

                    //grant all permissions to admin role
                    var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
                    await _roleManager.GrantAllPermissionsAsync(adminRole);

                    //User role should be default
                    var userRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.User);
                    userRole.IsDefault = true;
                    CheckErrors(await _roleManager.UpdateAsync(userRole));

                    //Create admin user for the tenant
                    var adminUser = User.CreateTenantAdminUser(tenant.Id, adminEmailAddress ?? string.Empty, tenancyName, phoneNumber);
                    adminUser.ShouldChangePasswordOnNextLogin = shouldChangePasswordOnNextLogin;
                    adminUser.IsActive = true;

                    //Create admin user for the tenant
                    if (adminPassword.IsNullOrEmpty())
                    {
                        adminPassword = User.CreateRandomPassword();
                    }
                    else
                    {
                        await _userManager.InitializeOptionsAsync(AbpSession.TenantId);
                        foreach (var validator in _userManager.PasswordValidators)
                        {
                            CheckErrors(await validator.ValidateAsync(_userManager, adminUser, adminPassword));
                        }
                    }

                    adminUser.Password = _passwordHasher.HashPassword(adminUser, adminPassword);

                    using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                    {
                        CheckErrors(await _userManager.CreateAsync(adminUser));
                    }

                    await _unitOfWorkManager.Current.SaveChangesAsync(); //To get admin user's id

                    //Assign admin user to admin role!
                    CheckErrors(await _userManager.AddToRoleAsync(adminUser, adminRole.Name));

                    //Notifications
                    await _appNotifier.WelcomeToTheApplicationAsync(adminUser);

                    //Send activation email
                    if (sendActivationEmail && !adminUser.IsActive && !adminUser.EmailAddress.IsNullOrWhiteSpace())
                    {
                        adminUser.SetNewEmailConfirmationCode();
                        await _userEmailer.SendEmailActivationLinkAsync(adminUser, emailActivationLink, adminPassword);
                    }

                    await _unitOfWorkManager.Current.SaveChangesAsync();

                    await _demoDataBuilder.BuildForAsync(tenant);

                    newTenantId = tenant.Id;
                    newAdminId = adminUser.Id;
                }

                await uow.CompleteAsync();
            }

            //Used a second UOW since UOW above sets some permissions and _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync needs these permissions to be saved.
            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                using (_unitOfWorkManager.Current.SetTenantId(newTenantId))
                {
                    //await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(new UserIdentifier(newTenantId, newAdminId));
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    await uow.CompleteAsync();
                }
            }

            return newTenantId;
        }

        /// <summary>
        /// 给当前用户创建租户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tenancyName"></param>
        /// <param name="connectionString"></param>
        /// <param name="isActive"></param>
        /// <param name="editionId"></param>
        /// <param name="sendActivationEmail"></param>
        /// <param name="subscriptionEndDate"></param>
        /// <param name="isInTrialPeriod"></param>
        /// <param name="emailActivationLink"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<int> CreateWithExistUserAsync(
            long userId,
            string tenancyName,
            string connectionString,
            bool isActive,
            int? editionId,
            bool sendActivationEmail,
            DateTime? subscriptionEndDate,
            bool isInTrialPeriod,
            string emailActivationLink)
        {
            int newTenantId;
            long newAdminId;

            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                await _userManager.UserStore.UserRepository.EnsureCollectionLoadedAsync(user, u => u.Roles);
                await _userManager.UserStore.UserRepository.EnsureCollectionLoadedAsync(user, u => u.Claims);
                await _userManager.UserStore.UserRepository.EnsureCollectionLoadedAsync(user, u => u.Permissions);
                await _userManager.UserStore.UserRepository.EnsureCollectionLoadedAsync(user, u => u.Settings);
                await _userManager.UserStore.UserRepository.EnsureCollectionLoadedAsync(user, u => u.Logins);
                await CheckEditionAsync(editionId, isInTrialPeriod);

                //Create tenant
                var tenant = new Tenant(tenancyName, tenancyName)
                {
                    IsActive = isActive,
                    EditionId = editionId,
                    SubscriptionEndDateUtc = subscriptionEndDate?.ToUniversalTime(),
                    IsInTrialPeriod = isInTrialPeriod,
                    ConnectionString = connectionString.IsNullOrWhiteSpace() ? null : SimpleStringCipher.Instance.Encrypt(connectionString)
                };

                await CreateAsync(tenant);
                await _unitOfWorkManager.Current.SaveChangesAsync(); //To get new tenant's id.

                //Create tenant database
                _abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);

                //We are working entities of new tenant, so changing tenant filter
                using (_unitOfWorkManager.Current.SetTenantId(tenant.Id))
                {
                    //Create static roles for new tenant
                    CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));
                    await _unitOfWorkManager.Current.SaveChangesAsync(); //To get static role ids

                    //grant all permissions to admin role
                    var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
                    await _roleManager.GrantAllPermissionsAsync(adminRole);

                    //User role should be default
                    var userRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.User);
                    userRole.IsDefault = true;
                    CheckErrors(await _roleManager.UpdateAsync(userRole));

                    user.TenantId = tenant.Id;
                    user.UserName = tenancyName;
                    user.IsMainUser = true;
                    user.Roles.Clear();
                    user.Claims.Clear();
                    user.Permissions.Clear();
                    user.Settings.Each(t => t.TenantId = tenant.Id);
                    user.Logins.Each(t => t.TenantId = tenant.Id);

                    user.SetNormalizedNames();
                    CheckErrors(await _userManager.Update4PlatformAsync(user));

                    await _unitOfWorkManager.Current.SaveChangesAsync(); //To get admin user's id

                    //Assign admin user to admin role!
                    CheckErrors(await _userManager.AddToRoleAsync(user, adminRole.Name));

                    //Notifications
                    await _appNotifier.WelcomeToTheApplicationAsync(user);

                    //Send activation email
                    if (sendActivationEmail && !user.EmailAddress.IsNullOrWhiteSpace())
                    {
                        user.SetNewEmailConfirmationCode();
                        await _userEmailer.SendEmailActivationLinkAsync(user, emailActivationLink);
                    }

                    await _unitOfWorkManager.Current.SaveChangesAsync();

                    await _demoDataBuilder.BuildForAsync(tenant);

                    newTenantId = tenant.Id;
                    newAdminId = user.Id;
                }

                await uow.CompleteAsync();
            }

            //Used a second UOW since UOW above sets some permissions and _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync needs these permissions to be saved.
            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                using (_unitOfWorkManager.Current.SetTenantId(newTenantId))
                {
                    //await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(new UserIdentifier(newTenantId, newAdminId));
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    await uow.CompleteAsync();
                }
            }

            return newTenantId;
        }

        /// <summary>
        /// 修改租户名称
        /// </summary>
        /// <param name="tenant"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public virtual async Task ChangeTenantNameAsync(Tenant tenant, string newName)
        {
            var existTenant = await FindByTenancyNameAsync(newName);
            if (existTenant != null && existTenant != tenant)
                throw new UserFriendlyException(L("DuplicateTenantName"));

            tenant.Name = newName;
            tenant.TenancyName = newName;
        }

        public override async Task CreateAsync(Tenant tenant)
        {
            await ValidateTenantAsync(tenant);

            if (await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenant.TenancyName) != null)
            {
                throw new UserFriendlyException(string.Format(L("DuplicateTenantName"), tenant.TenancyName));
            }

            await TenantRepository.InsertAsync(tenant);
        }

        public new async Task UpdateAsync(Tenant tenant)
        {
            if (await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenant.TenancyName && t.Id != tenant.Id) != null)
            {
                throw new UserFriendlyException(string.Format(L("DuplicateTenantName"), tenant.TenancyName));
            }

            await TenantRepository.UpdateAsync(tenant);
        }

        #region Utilities

        protected override Task ValidateTenancyNameAsync(string tenancyName)
        {
            if (!Regex.IsMatch(tenancyName, User.VappsTenancyNameRegex))
            {
                throw new UserFriendlyException(L("InvalidTenancyName"));
            }

            return Task.FromResult(0);
        }

        public async Task CheckEditionAsync(int? editionId, bool isInTrialPeriod)
        {
            if (!editionId.HasValue || !isInTrialPeriod)
            {
                return;
            }

            var edition = await _subscribableEditionRepository.GetAsync(editionId.Value);
            if (!edition.IsFree)
            {
                return;
            }

            var error = LocalizationManager.GetSource(VappsConsts.ServerSideLocalizationSourceName).GetString("FreeEditionsCannotHaveTrialVersions");
            throw new UserFriendlyException(error);
        }


        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<SubscriptionPayment> GetLastPaymentAsync(Expression<Func<SubscriptionPayment, bool>> predicate)
        {
            return await _subscriptionPaymentRepository.GetAll().LastOrDefaultAsync(predicate);
        }


        public async Task<Tenant> UpdateTenantAsync(int tenantId, bool isActive, bool isInTrialPeriod, PaymentPeriodType? paymentPeriodType, int editionId, EditionPaymentType editionPaymentType)
        {
            var tenant = await FindByIdAsync(tenantId);

            tenant.IsActive = isActive;
            tenant.IsInTrialPeriod = isInTrialPeriod;
            tenant.EditionId = editionId;

            if (paymentPeriodType.HasValue)
            {
                tenant.UpdateSubscriptionDateForPayment(paymentPeriodType.Value, editionPaymentType);
            }

            return tenant;
        }

        public async Task<Tenant> TrialEditionAsync(int tenantId, int editionId, DateTime endDateUtc)
        {
            var tenant = await FindByIdAsync(tenantId);
            if(tenant.HadTrialed)
                throw new UserFriendlyException(L("Edition.Trialed.HadTrialed"));

            tenant.EditionId = editionId;
            tenant.IsInTrialPeriod = true;
            tenant.HadTrialed = true;
            tenant.SubscriptionEndDateUtc = endDateUtc;

            return tenant;
        }

        public async Task<EndSubscriptionResult> EndSubscriptionAsync(Tenant tenant, SubscribableEdition edition, DateTime nowUtc)
        {
            if (tenant.EditionId == null || tenant.HasUnlimitedTimeSubscription())
            {
                throw new Exception($"Can not end tenant {tenant.TenancyName} subscription for {edition.DisplayName} tenant has unlimited time subscription!");
            }

            Debug.Assert(tenant.SubscriptionEndDateUtc != null, "tenant.SubscriptionEndDateUtc != null");

            var subscriptionEndDateUtc = tenant.SubscriptionEndDateUtc.Value;
            if (!tenant.IsInTrialPeriod)
            {
                subscriptionEndDateUtc = tenant.SubscriptionEndDateUtc.Value.AddDays(edition.WaitingDayAfterExpire ?? 0);
            }

            if (subscriptionEndDateUtc >= nowUtc)
            {
                throw new Exception($"Can not end tenant {tenant.TenancyName} subscription for {edition.DisplayName} since subscription has not expired yet!");
            }

            if (edition.ExpiringEditionId.HasValue)
            {
                tenant.EditionId = edition.ExpiringEditionId.Value;
                tenant.SubscriptionEndDateUtc = null;

                await UpdateAsync(tenant);

                return EndSubscriptionResult.AssignedToAnotherEdition;
            }

            tenant.IsActive = false;
            tenant.IsInTrialPeriod = false;

            await UpdateAsync(tenant);

            return EndSubscriptionResult.TenantSetInActive;
        }

        private new string L(string name)
        {
            return LocalizationManager.GetString(VappsConsts.ServerSideLocalizationSourceName, name);
        }

        #endregion
    }
}
