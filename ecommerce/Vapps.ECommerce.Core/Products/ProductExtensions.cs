using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.Media;

namespace Vapps.ECommerce.Products
{
    public static class ProductExtensions
    {
        /// <summary>
        /// Get product picture (for shopping cart and order details pages)
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="attributesJson">Atributes (in XML format)</param>
        /// <param name="pictureManager">Picture service</param>
        /// <param name="productAttributeParser">Product attribute service</param>
        /// <returns>Picture</returns>
        public static async Task<string> GetProductDefaultPictureUrl(this Product product, string attributesJson,
            IPictureManager pictureManager,
            IProductAttributeParser productAttributeParser)
        {
            if (product == null)
                throw new Exception("product");
            if (pictureManager == null)
                throw new Exception("pictureManager");
            if (productAttributeParser == null)
                throw new Exception("productAttributeParser");

            var pvaValues = await productAttributeParser.ParseProductAttributeValuesAsync(product.Id, attributesJson);
            foreach (var pvaValue in pvaValues)
            {
                var pvavPictureUrl = await pictureManager.GetPictureUrlAsync(pvaValue.PictureId);
                return pvavPictureUrl;
            }

            if (pvaValues == null || !pvaValues.Any())
            {
                var pPicture = product.Pictures.FirstOrDefault();

                return await pictureManager.GetPictureUrlAsync(pPicture.PictureId);
            }

            return string.Empty;
        }
    }
}
