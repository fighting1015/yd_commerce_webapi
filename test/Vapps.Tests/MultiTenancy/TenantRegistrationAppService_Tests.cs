﻿using Abp.Timing;
using Castle.MicroKernel.Registration;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Shouldly;
using System;
using System.Threading.Tasks;
using Vapps.Editions;
using Vapps.MultiTenancy;
using Vapps.MultiTenancy.Dto;
using Vapps.Payments;
using Vapps.Payments.Cache;
using Xunit;

namespace Vapps.Tests.MultiTenancy
{
    public class TenantRegistrationAppService_Tests : AppTestBase
    {
        private readonly ITenantRegistrationAppService _tenantRegistrationAppService;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;

        //private IPaymentCache _fakePaymentCache;
        private readonly string samplePaymentId = Guid.NewGuid().ToString();

        public TenantRegistrationAppService_Tests()
        {
            RegisterFakePaymentCache();
            _tenantRegistrationAppService = Resolve<ITenantRegistrationAppService>();
            _subscriptionPaymentRepository = Resolve<ISubscriptionPaymentRepository>();
        }

        private void RegisterFakePaymentCache()
        {
            //_fakePaymentCache = Substitute.For<IPaymentCache>();
            //_fakePaymentCache.GetCacheItemOrNull(SubscriptionPaymentGatewayType.WeChat, samplePaymentId).Returns(new PaymentCacheItem(SubscriptionPaymentGatewayType.WeChat, PaymentPeriodType.Monthly, EditionPaymentType.BuyNow, samplePaymentId));
            //_fakePaymentCache.GetCacheItemOrNull(SubscriptionPaymentGatewayType.WeChat, samplePaymentId).Returns(new PaymentCacheItem(SubscriptionPaymentGatewayType.WeChat, PaymentPeriodType.Annual, EditionPaymentType.BuyNow, samplePaymentId));
            //LocalIocManager.IocContainer.Register(Component.For<IPaymentCache>().Instance(_fakePaymentCache).IsDefault());
        }

        [Fact]
        public async Task SubscriptionEndDateUtc_ShouldBe_Null_After_Free_Registration()
        {
            //Arrange
            var edition = new SubscribableEdition
            {
                DisplayName = "Free Edition"
            };

            await UsingDbContextAsync(async context =>
            {
                context.SubscribableEditions.Add(edition);
                await context.SaveChangesAsync();
            });

            //Act
            var registerResult = await _tenantRegistrationAppService.RegisterTenant(new RegisterTenantInput
            {
                //EditionId = edition.Id,
                //AdminEmailAddress = "admin@volosoft.com",
                Password = "123qwe",
                //Name = "Volosoft",
                PhoneNumber = "18588662102",
                //SubscriptionStartType = SubscriptionStartType.Free,
                TenancyName = "Volosoft"
            });

            //Assert
            await UsingDbContextAsync(async context =>
            {
                var tenant = await context.Tenants.FirstOrDefaultAsync(t => t.Id == registerResult.TenantId);
                tenant.ShouldNotBe(null);
                tenant.SubscriptionEndDateUtc.HasValue.ShouldBe(false);
            });
        }

        [Fact]
        public async Task Cannot_Register_To_Free_Edition_As_Trial()
        {
            //Arrange
            var edition = new SubscribableEdition
            {
                DisplayName = "Free Edition"
            };

            await UsingDbContextAsync(async context =>
            {
                context.SubscribableEditions.Add(edition);
                await context.SaveChangesAsync();
            });

            var exception = await Assert.ThrowsAsync<Exception>(async () => await _tenantRegistrationAppService.RegisterTenant(new RegisterTenantInput
            {
                //EditionId = edition.Id,
                //AdminEmailAddress = "admin@volosoft.com",
                Password = "123qwe",
                //name = "Volosoft",
                PhoneNumber = "18588662102",
                //SubscriptionStartType = SubscriptionStartType.Trial,
                TenancyName = "Volosoft"
            }));

            exception.Message.ShouldBe("Trial is not available for this edition !");
        }

        [Fact]
        public async Task Should_Subscribe_To_Edition_As_Trial_If_Trial_Is_Available()
        {
            //Arrange
            var utcNow = Clock.Now.ToUniversalTime();
            var trialDayCount = 10;
            var edition = new SubscribableEdition
            {
                DisplayName = "Standard Edition",
                TrialDayCount = trialDayCount,
                MonthlyPrice = 9,
                AnnualPrice = 99
            };

            await UsingDbContextAsync(async context =>
            {
                context.SubscribableEditions.Add(edition);
                await context.SaveChangesAsync();
            });

            var result = await _tenantRegistrationAppService.RegisterTenant(new RegisterTenantInput
            {
                //EditionId = edition.Id,
                //AdminEmailAddress = "admin@volosoft.com",
                Password = "123qwe",
                //Name = "Volosoft",
                PhoneNumber = "18588662102",
                //SubscriptionStartType = SubscriptionStartType.Trial,
                TenancyName = "Volosoft"
            });

            //Assert
            await UsingDbContextAsync(async context =>
            {
                var tenant = await context.Tenants.FirstOrDefaultAsync(t => t.Id == result.TenantId);
                tenant.ShouldNotBe(null);
                tenant.SubscriptionEndDateUtc.HasValue.ShouldBe(true);
                tenant.SubscriptionEndDateUtc.Value.Date.ShouldBe(utcNow.Date.AddDays(trialDayCount));
            });
        }

        [Fact]
        public async Task Monthly_Registratin_Should_Add_30_Days_To_SubscriptionEndDate()
        {
            await Paid_Registratin_Should_Add_N_Days_To_SubscriptionEndDate(PaymentPeriodType.Monthly);
        }

        [Fact]
        public async Task Annual_Registratin_Should_Add_365_Days_To_SubscriptionEndDate()
        {
            await Paid_Registratin_Should_Add_N_Days_To_SubscriptionEndDate(PaymentPeriodType.Annual);
        }

        private async Task Paid_Registratin_Should_Add_N_Days_To_SubscriptionEndDate(PaymentPeriodType paymentPeriodType)
        {
            //Arrange
            var utcNow = Clock.Now.ToUniversalTime();
            var trialDayCount = 10;
            var edition = new SubscribableEdition
            {
                DisplayName = "Gold Edition",
                TrialDayCount = trialDayCount,
                MonthlyPrice = 19,
                AnnualPrice = 199
            };

            await UsingDbContextAsync(async context =>
            {
                context.SubscribableEditions.Add(edition);
                await context.SaveChangesAsync();
            });

            //Don't test payment here.
            //_fakePaymentCache.AddCacheItem(new PaymentCacheItem(SubscriptionPaymentGatewayType.WeChat, paymentPeriodType, EditionPaymentType.BuyNow, samplePaymentId));
            _subscriptionPaymentRepository.Insert(new SubscriptionPayment
            {
                PaymentPeriodType = paymentPeriodType,
                EditionId = edition.Id,
                Amount = edition.MonthlyPrice.Value,
                Gateway = SubscriptionPaymentGatewayType.WeChat,
                PaymentId = samplePaymentId,
                Status = SubscriptionPaymentStatus.Completed,
            });

            var result = await _tenantRegistrationAppService.RegisterTenant(new RegisterTenantInput
            {
                //EditionId = edition.Id,
                //AdminEmailAddress = "admin@volosoft.com",
                Password = "123qwe",
                //Name = "Volosoft",
                PhoneNumber = "18588662102",
                //SubscriptionStartType = SubscriptionStartType.Paid,
                TenancyName = "Volosoft",
                //Gateway = SubscriptionPaymentGatewayType.Paypal,
                //PaymentId = samplePaymentId
            });

            //Assert
            await UsingDbContextAsync(async context =>
            {
                var tenant = await context.Tenants.FirstOrDefaultAsync(t => t.Id == result.TenantId);
                tenant.ShouldNotBe(null);
                tenant.SubscriptionEndDateUtc.HasValue.ShouldBe(true);
                tenant.SubscriptionEndDateUtc.Value.Date.ShouldBe(utcNow.Date.AddDays((int)paymentPeriodType));
            });
        }
    }
}