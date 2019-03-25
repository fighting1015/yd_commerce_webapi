using System.Threading.Tasks;
using Vapps.ECommerce.Products;

namespace Vapps.ECommerce.Prices
{
    public interface IPriceCalculator
    {
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
        //    decimal additionalCharge = decimal.Zero,
        //    bool includeDiscounts = true,
        //    int quantity = 1);

        ///// <summary>
        ///// Gets the final price
        ///// </summary>
        ///// <param name="product">Product</param>
        ///// <param name="customer">The customer</param>
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
        /// <param name="product"></param>
        /// <param name="attributesJson"></param>
        /// <returns></returns>
        Task<decimal> GetProductCostAsync(Product product, string attributesJson);

        /// <summary>
        /// Get a price adjustment of a product attribute value
        /// </summary>
        /// <param name="product"></param>
        /// <param name="attributesJson"></param>
        /// <returns></returns>
        Task<decimal> GetProductPriceAsync(Product product, string attributesJson);
    }
}
