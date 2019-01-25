using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.ECommerce.Products.Dto;
using Vapps.MultiTenancy;
using Vapps.Tests.Product;

namespace Vapps.Tests.Product
{

    public class ProductAppService_Product_Test : ProductAppServiceTestBase
    {
        [MultiTenantFact]
        public async Task Should_Create_Product_For_Tenant()
        {
            LoginAsDefaultTenantAdmin();

            var defaultTenantId = (await GetTenantAsync(Tenant.DefaultTenantName)).Id;
            //Arrange
            AbpSession.TenantId = defaultTenantId;

            var productDto = new CreateOrUpdateProductInput()
            {
                Name = "美度一支黑",
                ShortDescription = "美度一支黑",
                FullDescription = "美度一支黑",
                Price = 99,
                GoodCost = 19
            };
            productDto.Categories.Add(new ProductCategoryDto()
            {
                CategoryId = 1,
            });
            productDto.Categories.Add(new ProductCategoryDto()
            {
                CategoryId = 2,
            });

            var attribute1 = new ProductAttributeDto()
            {
                Id = 1,
            };

            //attribute1.Values.Add(new ProductAttributeValueDto()
            //{
            //    AttributeId = 1,
            //    Name = "红色",
            //});

            //attribute1.Values.Add(new ProductAttributeValueDto()
            //{
            //    AttributeId = 0,
            //    Name = "黑色",
            //});

            //productDto.Attributes.Add(attribute1);

            //var attribute2 = new ProductAttributeDto()
            //{
            //    Id = 0,
            //    Name = "尺码",
            //};

            //attribute2.Values.Add(new ProductAttributeValueDto()
            //{
            //    AttributeId = 0,
            //    Name = "大码",
            //});

            //attribute2.Values.Add(new ProductAttributeValueDto()
            //{
            //    AttributeId = 0,
            //    Name = "小码",
            //});

            //productDto.Attributes.Add(attribute2);

            //var combin1 = new AttributeCombinationDto()
            //{
            //    Sku = "12345",
            //    ThirdPartySku = "12345",
            //    OverriddenPrice = 199,
            //    OverriddenGoodCost = 29,
            //    StockQuantity = 100,
            //};

            //combin1.Attributes.Add(new ProductAttributeDto()
            //{
            //    Id = 1,
            //    Values = new List<ProductAttributeValueDto> { attribute1.Values.First() }
            //});

            //productDto.AttributeCombinations.Add();

            //Try to update with existing name
            await ProductAppService.CreateOrUpdateProduct(productDto);

            //Assert
            //var pictureAfterUpdate = await GetPictureByKeyOrNullAsync("1/test.jpg");
            //pictureAfterUpdate.Name.ShouldBe("test2.jpg");

        }
    }
}
