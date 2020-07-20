using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data
{
    public class DesignTimeCoreContextFactory : IDesignTimeDbContextFactory<CoreContext>
    {
        public CoreContext CreateDbContext(string[] args)
        {
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Orders;Trusted_Connection=True;";

            var optionsBuilder = new DbContextOptionsBuilder<CoreContext>()
                .UseSqlServer(connectionString);

            return new CoreContext(optionsBuilder.Options);
        }
    }
}
