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

            long pictureId = 0;
            if (!attributesJson.IsNullOrEmpty())
            {
                var jsonAttributeList = JsonConvert.DeserializeObject<List<JsonProductAttribute>>(attributesJson);

                var pvaValues = await productAttributeParser.ParseProductAttributeValuesAsync(product.Id, jsonAttributeList);
                if (pvaValues != null && pvaValues.Any())
                {
                    pictureId = pvaValues.First().PictureId;
                }
            }

            if (pictureId == 0)
            {
                var pPicture = product.Pictures.FirstOrDefault();

                if (pPicture != null)
                    pictureId = pPicture.PictureId;
            }

            return await pictureManager.GetPictureUrlAsync(pictureId); ;
        }

        /// <summary>
        /// 获取商品体积
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static decimal GetVolume(this Product product)
        {
            return product.Length * product.Weight * product.Height;
        }
    }
}
