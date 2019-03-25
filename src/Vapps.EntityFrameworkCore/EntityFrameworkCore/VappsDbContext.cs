using Abp.IdentityServer4;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vapps.Addresses;
using Vapps.Advert.AdvertAccounts;
using Vapps.Advert.AdvertStatistics;
using Vapps.Authorization.Accounts;
using Vapps.Authorization.Roles;
using Vapps.Authorization.Users;
using Vapps.DataStatistics;
using Vapps.ECommerce.Catalog;
using Vapps.ECommerce.Customers;
using Vapps.ECommerce.Orders;
using Vapps.ECommerce.Payments;
using Vapps.ECommerce.Products;
using Vapps.ECommerce.Shippings;
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

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<ProductCategory> ProductCategorys { get; set; }

        public virtual DbSet<ProductAttribute> ProductAttributes { get; set; }

        public virtual DbSet<ProductPicture> ProductPictures { get; set; }

        public virtual DbSet<ProductAttributeMapping> ProductAttributeMappings { get; set; }

        public virtual DbSet<ProductAttributeCombination> ProductAttributeCombinations { get; set; }

        public virtual DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }

        public virtual DbSet<PredefinedProductAttributeValue> PredefinedProductAttributeValues { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Shipment> Shipments { get; set; }
        public virtual DbSet<ShipmentItem> ShipmentItems { get; set; }
        public virtual DbSet<Logistics> Logisticses { get; set; }
        public virtual DbSet<TenantLogistics> TenantLogisticses { get; set; }
        public virtual DbSet<OrderPayment> OrderPayments { get; set; }

        public virtual DbSet<AdvertAccount> AdvertAccounts { get; set; }
        public virtual DbSet<AdvertDailyStatistic> AdvertDailyStatistics { get; set; }
        public virtual DbSet<AdvertDailyStatisticItem> AdvertDailyStatisticItems { get; set; }

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

            InitECommerceEntities(modelBuilder);
            InitAdvertEntities(modelBuilder);

            modelBuilder.ConfigurePersistedGrantEntity();
        }

        private static void InitECommerceEntities(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<Product>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.IsDeleted });
                b.Property(e => e.Price).HasColumnType("decimal(18, 4)");
                b.Property(e => e.GoodCost).HasColumnType("decimal(18, 4)");
                b.Property(e => e.Height).HasColumnType("decimal(18, 4)");
                b.Property(e => e.Width).HasColumnType("decimal(18, 4)");
                b.Property(e => e.Length).HasColumnType("decimal(18, 4)");
                b.Property(e => e.Weight).HasColumnType("decimal(18, 4)");
            });

            modelBuilder.Entity<ProductCategory>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.ProductId });
            });

            modelBuilder.Entity<ProductPicture>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.ProductId });
            });

            modelBuilder.Entity<ProductAttribute>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.IsDeleted });
            });

            modelBuilder.Entity<ProductAttributeValue>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.ProductId, e.ProductAttributeMappingId, e.IsDeleted });

                b.HasOne(o => o.ProductAttributeMapping)
                  .WithMany(m => m.Values)
                  .HasForeignKey(c => c.ProductAttributeMappingId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProductAttributeMapping>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.ProductId, e.IsDeleted });

                b.HasOne(o => o.ProductAttribute)
                 .WithMany()
                 .HasForeignKey(c => c.ProductAttributeId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PredefinedProductAttributeValue>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.ProductAttributeId });
            });

            modelBuilder.Entity<ProductAttributeCombination>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.ProductId });

                b.Property(e => e.OverriddenPrice).HasColumnType("decimal(18, 4)");
                b.Property(e => e.OverriddenGoodCost).HasColumnType("decimal(18, 4)");
            });

            modelBuilder.Entity<Customer>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.IsDeleted });
            });

            modelBuilder.Entity<Order>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.IsDeleted });
                b.HasIndex(e => new { e.TenantId, e.OrderStatus });
                b.HasIndex(e => new { e.TenantId, e.PaymentStatus });
                b.HasIndex(e => new { e.TenantId, e.ShippingStatus });

                b.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 4)");
                b.Property(e => e.SubTotalDiscountAmount).HasColumnType("decimal(18, 4)");

                b.Property(e => e.RefundedAmount).HasColumnType("decimal(18, 4)");
                b.Property(e => e.RewardAmount).HasColumnType("decimal(18, 4)");
                b.Property(e => e.ShippingAmount).HasColumnType("decimal(18, 4)");
                b.Property(e => e.SubtotalAmount).HasColumnType("decimal(18, 4)");

                b.Property(e => e.TotalAmount).HasColumnType("decimal(18, 4)");

                b.Property(e => e.PaymentMethodAdditionalFee).HasColumnType("decimal(18, 4)");
            });

            modelBuilder.Entity<OrderItem>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.OrderId, e.IsDeleted });
                b.HasIndex(e => new { e.TenantId, e.ProductId, e.IsDeleted });

                b.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 4)");
                b.Property(e => e.UnitPrice).HasColumnType("decimal(18, 4)");
                b.Property(e => e.Price).HasColumnType("decimal(18, 4)");
                b.Property(e => e.OriginalProductCost).HasColumnType("decimal(18, 4)");
                b.Property(e => e.Weight).HasColumnType("decimal(18, 4)");
                b.Property(e => e.Volume).HasColumnType("decimal(18, 4)");

                b.HasOne(e => e.Order)
                 .WithMany(o => o.Items)
                 .HasForeignKey(c => c.OrderId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderPayment>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.OrderId, e.IsDeleted });

                b.Property(e => e.Amount).HasColumnType("decimal(18, 4)");
            });

            modelBuilder.Entity<Shipment>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.IsDeleted });

                b.Property(e => e.TotalWeight).HasColumnType("decimal(18, 4)");
                b.Property(e => e.TotalVolume).HasColumnType("decimal(18, 4)");

                b.HasOne(e => e.Order)
                 .WithMany(o => o.Shipments)
                 .HasForeignKey(c => c.OrderId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ShipmentItem>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.IsDeleted });
            });

            modelBuilder.Entity<Logistics>(b =>
            {
                b.HasIndex(e => new { e.IsDeleted });
            });

            modelBuilder.Entity<TenantLogistics>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });
        }

        private static void InitAdvertEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdvertAccount>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.IsDeleted });
            });

            modelBuilder.Entity<AdvertDailyStatistic>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.AdvertAccountId, e.IsDeleted });

                b.Property(e => e.ThDisplayCost).HasColumnType("decimal(18, 4)");
                b.Property(e => e.ThDisplayCost).HasColumnType("decimal(18, 4)");
                b.Property(e => e.ClickPrice).HasColumnType("decimal(18, 4)");

                b.HasMany(e => e.Items)
                .WithOne()
                .HasForeignKey(c => c.AdvertDailyStatisticId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AdvertDailyStatisticItem>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.AdvertDailyStatisticId, e.IsDeleted });

                b.Property(e => e.ThDisplayCost).HasColumnType("decimal(18, 4)");
                b.Property(e => e.ThDisplayCost).HasColumnType("decimal(18, 4)");
                b.Property(e => e.ClickPrice).HasColumnType("decimal(18, 4)");
            });
        }
    }
}