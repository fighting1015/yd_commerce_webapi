using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace Vapps.EntityFrameworkCore
{
    public static class VappsDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<VappsDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<VappsDbContext> builder, DbConnection connection)
        {
            builder.UseMySql(connection);
        }
    }
}