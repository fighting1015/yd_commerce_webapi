using Abp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.ECommerce.Products;

namespace Vapps.ECommerce.Prices
{
    public class PriceCalculator : VappsDomainServiceBase, IPriceCalculator
    {
        public readonly IProductAttributeManager _productAttributeManager;

        public PriceCalculator(IProductAttributeManager productAttributeManager)
        {
            this._productAttributeManager = productAttributeManager;
        }

        ///// <summary>
        ///// Gets the final price
        ///// </summary>
        ///// <param name="product">Product</param>
        ///// <param name="customer">The customer</param>
        ///// <param name="additionalCharge">Additional charge</param>
        ///// <param name="includeDiscounts">A value indicating whether include discounts or not for final price computation</param>
        ///// <param name="quantity">Shopping cart item quantity</param>
        ///// <returns>Final price</returns>
        //decimal GetFinalPrice(Product product,
        //decimal additionalCharge = decimal.Zero,
        //bool includeDiscounts = true,
        //int quantity = 1);

        ///// <summary>
        ///// Gets the final price
        ///// </summary>
        ///// <param name="product">Product</param>
        ///// <param name="additionalCharge">Additional charge</param>
        ///// <param name="includeDiscounts">A value indicating whether include discounts or not for final price computation</param>
        ///// <param name="quantity">Shopping cart item quantity</param>
        ///// <param name="discountAmount">Applied discount amount</param>
        ///// <returns>Final price</returns>
        //decimal GetFinalPrice(Product product,
        //    decimal additionalCharge,
        //    bool includeDiscounts,
        //    int quantity,
        //    out decimal discountAmount);

        /// <summary>
        /// Gets the product cost (one item)
        /// </summary>
        /// <param name="attributesXml">Shopping cart item attributes in Json</param>
        /// <returns>Product cost (one item)</returns>
        public virtual async Task<decimal> GetProductCostAsync(Product product, string attributesJson)
        {
            if (attributesJson.IsNullOrWhiteSpace())
                return product.GoodCost;

            decimal cost = 0;
            var attributeValues = await _productAttributeManager.FindCombinationByAttributesJsonAsync(attributesJson);

            if (attributeValues != null && attributeValues.OverriddenGoodCost.HasValue)
                cost = attributeValues.OverriddenGoodCost.Value;

            return cost;
        }

        /// <summary>
        /// Get a price adjustment of a product attribute value
        /// </summary>
        /// <param name="value">Product attribute value</param>
        /// <returns>Price adjustment</returns>
        public virtual async Task<decimal> GetProductPriceAsync(Product product, string attributesJson)
        {
            if (attributesJson.IsNullOrWhiteSpace())
                return product.Price;

            decimal price = 0;
            var attributeValues = await _productAttributeManager.FindCombinationByAttributesJsonAsync(attributesJson);

            if (attributeValues != null && attributeValues.OverriddenGoodCost.HasValue)
                price = attributeValues.OverriddenPrice.Value;

            return price;

        }
    }
}
