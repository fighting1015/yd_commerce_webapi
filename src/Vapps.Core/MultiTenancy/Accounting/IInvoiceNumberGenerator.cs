using System.Threading.Tasks;
using Abp.Dependency;

namespace Vapps.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}