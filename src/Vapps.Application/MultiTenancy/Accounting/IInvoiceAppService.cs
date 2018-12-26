using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Vapps.MultiTenancy.Accounting.Dto;

namespace Vapps.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
