using System;
using System.ComponentModel.DataAnnotations;
using Abp.MultiTenancy;
using Vapps.Authorization.Users;
using Vapps.Payments;
using Vapps.Editions;
using Abp.Timing;

namespace Vapps.MultiTenancy
{
    /// <summary>
    /// Represents a Tenant in the system.
    /// A tenant is a isolated customer for the application
    /// which has it's own users, roles and other application entities.
    /// </summary>
    public class Tenant : AbpTenant<User>
    {
        /// <summary>
        /// Max length of the <see cref="TenancyName"/> property.
        /// </summary>
        public const int NewMaxTenancyNameLength = 20;

        public const int MaxLogoMimeTypeLength = 64;

        //Can add application specific tenant properties here

        public DateTime? SubscriptionEndDateUtc { get; set; }

        public bool IsInTrialPeriod { get; set; }

        public bool HadTrialed { get; set; }

        /// <summary>
        /// Logo 图片 Id
        /// </summary>
        public virtual long LogoId { get; set; }

        /// <summary>
        /// 背景图片 Id
        /// </summary>
        public virtual long BackgroundPictureId { get; set; }

        /// <summary>
        /// 宣传语
        /// </summary>
        [StringLength(24)]
        public virtual string Tagline { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; set; }

        protected Tenant()
        {

        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {

        }

        public virtual bool HasLogo()
        {
            return LogoId > 0;
        }

        public virtual bool HasBackgroundPicture()
        {
            return BackgroundPictureId > 0;
        }

        public void ClearLogo()
        {
            LogoId = 0;
        }

        public void ClearBackgroundPicture()
        {
            BackgroundPictureId = 0;
        }

        public void UpdateSubscriptionDateForPayment(PaymentPeriodType paymentPeriodType, EditionPaymentType editionPaymentType)
        {
            switch (editionPaymentType)
            {
                case EditionPaymentType.NewRegistration:
                case EditionPaymentType.BuyNow:
                    {
                        SubscriptionEndDateUtc = Clock.Now.ToUniversalTime().AddDays((int)paymentPeriodType);
                        break;
                    }
                case EditionPaymentType.Extend:
                    ExtendSubscriptionDate(paymentPeriodType);
                    break;
                case EditionPaymentType.Upgrade:
                    if (HasUnlimitedTimeSubscription())
                    {
                        SubscriptionEndDateUtc = Clock.Now.ToUniversalTime().AddDays((int)paymentPeriodType);
                    }
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void ExtendSubscriptionDate(PaymentPeriodType paymentPeriodType)
        {
            if (SubscriptionEndDateUtc == null)
            {
                throw new InvalidOperationException("Can not extend subscription date while it's null!");
            }

            if (IsSubscriptionEnded())
            {
                SubscriptionEndDateUtc = Clock.Now.ToUniversalTime();
            }

            SubscriptionEndDateUtc = SubscriptionEndDateUtc.Value.AddDays((int)paymentPeriodType);
        }

        private bool IsSubscriptionEnded()
        {
            return SubscriptionEndDateUtc < Clock.Now.ToUniversalTime();
        }

        public int CalculateRemainingDayCount()
        {
            return SubscriptionEndDateUtc != null ? (SubscriptionEndDateUtc.Value - Clock.Now.ToUniversalTime()).Days : 0;
        }

        public bool HasUnlimitedTimeSubscription()
        {
            return SubscriptionEndDateUtc == null;
        }
    }
}