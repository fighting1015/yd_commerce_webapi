namespace Vapps.ECommerce.Products
{

    /// <summary>
    /// Extensions
    /// </summary>
    public static class ProductAttributeExtensions
    {
        /// <summary>
        /// A value indicating whether this product attribute should have values
        /// </summary>
        /// <param name="productAttributeMapping">Product attribute mapping</param>
        /// <returns>Result</returns>
        public static bool ShouldHaveValues(this ProductAttributeMapping productAttributeMapping)
        {
            if (productAttributeMapping == null)
                return false;

            //other attribute controle types support values
            return true;
        }

        /// <summary>
        /// A value indicating whether this product attribute is non-combinable
        /// </summary>
        /// <param name="productAttributeMapping">Product attribute mapping</param>
        /// <returns>Result</returns>
        public static bool IsNonCombinable(this ProductAttributeMapping productAttributeMapping)
        {
            //When you have a product with several attributes it may well be that some are combinable,
            //whose combination may form a new SKU with its own inventory,
            //and some non-combinable are more used to add accesories

            if (productAttributeMapping == null)
                return false;

            //we can add a new property to "ProductAttributeMapping" entity indicating whether it's combinable/non-combinable
            //but we assume that attributes
            //which cannot have values (any value can be entered by a customer)
            //are non-combinable
            var result = !ShouldHaveValues(productAttributeMapping);
            return result;
        }
    }
}
