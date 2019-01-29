using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.Dto;

namespace Vapps.ECommerce.Products.Dto
{
    public class GetProductsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品Sku
        /// </summary>
        public string Sku { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id DESC";
            }
        }
    }
}
