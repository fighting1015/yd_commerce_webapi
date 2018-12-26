using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Vapps.Configuration;
using Vapps.Web;

namespace Vapps.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class VappsDbContextFactory : IDesignTimeDbContextFactory<VappsDbContext>
    {
        public VappsDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<VappsDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            VappsDbContextConfigurer.Configure(builder, configuration.GetConnectionString(VappsConsts.ConnectionStringName));
            
            return new VappsDbContext(builder.Options);
        }
    }
}