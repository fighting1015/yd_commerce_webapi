using Abp.IdentityServer4;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vapps.Addresses;
using Vapps.Authorization.Accounts;
using Vapps.Authorization.Roles;
using Vapps.Authorization.Users;
using Vapps.DataStatistics;
using Vapps.ECommerce.Catalog;
using Vapps.ECommerce.Stores;
using Vapps.Editions;
using Vapps.Media;
using Vapps.MultiTenancy;
using Vapps.MultiTenancy.Accounting;
using Vapps.Payments;
using Vapps.SMS;
using Vapps.States;
using Vapps.Storage;
using Vapps.WeChat.Core.TemplateMessages;
using Vapps.WeChat.Core.Users;

namespace Vapps.EntityFrameworkCore
{
    public class VappsDbContext : AbpZeroDbContext<Tenant, Role, User, VappsDbContext>, IAbpPersistedGrantDbContext
    {
        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<LoginAttempt> LoginAttempts { get; set; }

        /// <summary>
        /// External User logins.
        /// </summary>
        public virtual DbSet<ExternalUserLogin> ExternalUserLogins { get; set; }

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SMSTemplate> SMSTemplates { get; set; }

        public virtual DbSet<SMSTemplateItem> SMSTemplateItems { get; set; }

        public virtual DbSet<Picture> Pictures { get; set; }

        public virtual DbSet<PictureGroup> PictureGroups { get; set; }

        public virtual DbSet<Province> Provinces { get; set; }

        public virtual DbSet<City> Citys { get; set; }

        public virtual DbSet<District> Districts { get; set; }

        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<AccountAddress> AccountAddresses { get; set; }

        public virtual DbSet<TemplateMessage> TemplateMessages { get; set; }

        public virtual DbSet<TemplateMessageItem> TemplateMessageItems { get; set; }

        public virtual DbSet<WeChatUser> WeChatUsers { get; set; }

        public virtual DbSet<UniversalDataStatistics> UniversalDataStatisticses { get; set; }

        public virtual DbSet<Store> Stores { get; set; }

        public virtual DbSet<StoreMapping> StoreMappings { get; set; }

        public virtual DbSet<Category> Categorys { get; set; }

        public VappsDbContext(DbContextOptions<VappsDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //批量修改表名前缀
            modelBuilder.ChangeAbpTablePrefix<Tenant, Role, User>("");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                b.HasIndex(e => new { e.IsDeleted, e.TenantId, e.NormalizedEmailAddress });
                b.HasIndex(e => new { e.IsDeleted, e.TenantId, e.NormalizedUserName });
                b.HasIndex(e => new { e.IsDeleted, e.TenantId, e.PhoneNumber });

                b.HasMany(e => e.Settings)
                 .WithOne()
                 .HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<SMSTemplate>(b =>
            {
                b.HasIndex(e => new { e.SmsProvider });
                b.HasIndex(e => new { e.IsDeleted });
                b.HasIndex(e => new { e.IsActive });
            });

            modelBuilder.Entity<SMSTemplateItem>(b =>
            {
                b.HasIndex(e => new { e.TemplateMessageId });
            });

            modelBuilder.Entity<Picture>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.GroupId });
            });

            modelBuilder.Entity<PictureGroup>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<City>(b =>
            {
                b.HasIndex(e => new { e.ProvinceId });
            });

            modelBuilder.Entity<District>(b =>
            {
                b.HasIndex(e => new { e.CityId });
            });


            modelBuilder.Entity<WeChatUser>(b =>
            {
                b.HasIndex(e => new { e.IsDeleted, e.MpId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { e.PaymentId, e.Gateway });
            });

            modelBuilder.Entity<UniversalDataStatistics>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.Date, e.DataType });
            });

            modelBuilder.Entity<LoginAttempt>();

            modelBuilder.Entity<Store>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.IsDeleted });
            });

            modelBuilder.Entity<StoreMapping>(b =>
            {
                b.HasIndex(e => new { e.StoreId, e.EntityName });
                b.HasOne(o => o.Store)
                 .WithMany()
                 .HasForeignKey(c => c.StoreId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Category>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.IsDeleted });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}