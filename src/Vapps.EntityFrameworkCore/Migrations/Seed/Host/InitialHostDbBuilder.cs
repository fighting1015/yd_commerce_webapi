using Vapps.EntityFrameworkCore;

namespace Vapps.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly VappsDbContext _context;

        public InitialHostDbBuilder(VappsDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
            new DefaultPictureGroupCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
