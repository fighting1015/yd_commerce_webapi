using System;
using System.Collections.Generic;
using System.Text;
using Vapps.ECommerce.Catalog;
using Vapps.ECommerce.Products;

namespace Vapps.Tests.Product
{
    public abstract class ProductAppServiceTestBase : AppTestBase
    {
        protected readonly IProductAppService ProductAppService;

        protected ProductAppServiceTestBase() : base()
        {
            ProductAppService = Resolve<IProductAppService>();
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

                    context.PredefinedProductAttributeValues.Add(CreatePredefinedProductAttributeValueEntity(1,"红色"));
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

    }
}
