using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vapps.ECommerce.Catalog;
using Vapps.ECommerce.Products;

namespace Vapps.Tests.Product
{
    public abstract class ProductAppServiceTestBase : AppTestBase
    {
        protected readonly IProductAppService ProductAppService;
        protected readonly IProductAttributeAppService ProductAttributeAppService;

        protected ProductAppServiceTestBase() : base()
        {
            ProductAppService = Resolve<IProductAppService>();
            ProductAttributeAppService = Resolve<IProductAttributeAppService>();
        }

        protected void CreatePictureAndGroup()
        {

            UsingDbContext(
                context =>
                {
                    context.Categorys.Add(CreateCategoryEntity("服装"));
                    context.Categorys.Add(CreateCategoryEntity("上衣"));

                    context.ProductAttributes.Add(CreatePictureEntity("颜色"));
                    context.ProductAttributes.Add(CreatePictureEntity("规格"));

                    context.PredefinedProductAttributeValues.Add(CreatePredefinedProductAttributeValueEntity(1, "红色"));
                    context.PredefinedProductAttributeValues.Add(CreatePredefinedProductAttributeValueEntity(2, "大瓶"));
                });
        }

        protected Category CreateCategoryEntity(string name)
        {
            return new Category
            {
                Name = name,
            };
        }

        protected ProductAttribute CreatePictureEntity(string name)
        {
            return new ProductAttribute
            {
                Name = name,
            };
        }

        protected PredefinedProductAttributeValue CreatePredefinedProductAttributeValueEntity(long productAttributeId, string name)
        {
            return new PredefinedProductAttributeValue
            {
                ProductAttributeId = productAttributeId,
                Name = name,
            };
        }

        protected async Task<ProductAttribute> GetProductAttributeByIdOrNullAsync(long id)
        {
            return await UsingDbContextAsync(async context =>
               await context.ProductAttributes
                   .FirstOrDefaultAsync(u =>
                           u.Id == id &&
                           u.TenantId == AbpSession.TenantId
                   ));
        }


        protected async Task<ProductAttribute> GetProductAttributeByNameOrNullAsync(string name)
        {
            return await UsingDbContextAsync(async context =>
               await context.ProductAttributes
                   .FirstOrDefaultAsync(u =>
                           u.Name == name &&
                           u.TenantId == AbpSession.TenantId
                   ));
        }

        protected async Task<PredefinedProductAttributeValue> GetPredefinedProductAttributeByNameOrNullAsync(long attributeId, string name)
        {
            return await UsingDbContextAsync(async context =>
               await context.PredefinedProductAttributeValues
                   .FirstOrDefaultAsync(p =>
                           p.ProductAttributeId == attributeId &&
                           p.Name == name &&
                           p.TenantId == AbpSession.TenantId
                   ));
        }
    }
}
