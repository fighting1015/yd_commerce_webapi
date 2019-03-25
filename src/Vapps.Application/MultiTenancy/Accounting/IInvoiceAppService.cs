using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Vapps.MultiTenancy.Accounting.Dto;

namespace Vapps.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        /// <summary>
        /// 获取 收据/发票 信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        /// <summary>
        /// 创建 收据/发票
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateInvoice(CreateInvoiceDto input);
    }
}
