using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vapps.ECommerce.Products;
using Vapps.ECommerce.Products.Dto;
using Vapps.MultiTenancy;

namespace Vapps.Tests.Product
{
    public class ProductAttributeAppService_ProductAttribute_Test : ProductAppServiceTestBase
    {
        [MultiTenantFact]
        public async Task Should_Create_ProductAttribute_For_Tenant()
        {
            LoginAsDefaultTenantAdmin();

            var defaultTenantId = (await GetTenantAsync(Tenant.DefaultTenantName)).Id;
            //Arrange
            AbpSession.TenantId = defaultTenantId;

            var attribute = new CreateOrUpdateAttributeInput()
            {
                Name = "尺寸",
                DisplayOrder = 1
            };

            //productDto.AttributeCombinations.Add();

            //Try to update with existing name
            await ProductAttributeAppService.CreateOrUpdateAttribute(attribute);

            //Assert
            var attributeAfterCreate = await GetProductAttributeByIdOrNullAsync(3);
            attributeAfterCreate.Name.ShouldBe("尺寸");
        }

        [MultiTenantFact]
        public async Task Should_Create_PredefinedProductAttributeValue_For_Tenant()
        {
            LoginAsDefaultTenantAdmin();

            var defaultTenantId = (await GetTenantAsync(Tenant.DefaultTenantName)).Id;
            //Arrange
            AbpSession.TenantId = defaultTenantId;

            var attributeValue = new CreateOrUpdateAttributeValueInput()
            {
                AttributeId = 1,
                Name = "紫色",
                DisplayOrder = 1
            };

            //productDto.AttributeCombinations.Add();

            //Try to update with existing name
            await ProductAttributeAppService.CreateOrUpdateAttributeValue(attributeValue);

            //Assert
            var attributeAfterCreate = await GetProductAttributeByIdOrNullAsync(2);
            attributeAfterCreate.Name.ShouldBe("紫色");
        }
    }
}
