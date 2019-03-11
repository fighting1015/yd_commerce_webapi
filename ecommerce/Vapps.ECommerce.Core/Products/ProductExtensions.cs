using Abp;
using Newtonsoft.Json;
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
        /// 获取产品图片
        /// </summary>
        /// <param name="product">产品</param>
        /// <param name="attributesJson">属性json字符串</param>
        /// <param name="pictureManager"></param>
        /// <param name="productAttributeParser"></param>
        /// <returns>Picture</returns>
        public static async Task<string> GetProductDefaultPictureUrl(this Product product, string attributesJson,
            IPictureManager pictureManager,
            IProductAttributeParser productAttributeParser)
        {
            if (product == null)
                throw new AbpException("product");
            if (pictureManager == null)
                throw new AbpException("pictureManager");
            if (productAttributeParser == null)
                throw new AbpException("productAttributeParser");

            if (!attributesJson.IsNullOrEmpty())
            {
                var jsonAttributeList = JsonConvert.DeserializeObject<List<JsonProductAttribute>>(attributesJson);

                var pvaValues = await productAttributeParser.ParseProductAttributeValuesAsync(product.Id, jsonAttributeList);
                if (pvaValues != null && pvaValues.Any())
                {
                    var pvavPictureUrl = await pictureManager.GetPictureUrlAsync(pvaValues.First().PictureId);
                    return pvavPictureUrl;
                }
            }

            var pPicture = product.Pictures.FirstOrDefault();

            if (pPicture != null)
                return await pictureManager.GetPictureUrlAsync(pPicture.PictureId);

            return string.Empty;
        }
    }
}
