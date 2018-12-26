using Vapps.EntityFrameworkCore;

namespace Vapps.Tests.TestDatas
{
    public class TestDataBuilder
    {
        private readonly VappsDbContext _context;
        private readonly int _tenantId;

        public TestDataBuilder(VappsDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            new TestOrganizationUnitsBuilder(_context, _tenantId).Create();
            new TestTenantAndUserBuilder(_context).Create();
            new TestEditionsBuilder(_context).Create();
            _context.SaveChanges();
        }
    }
}
