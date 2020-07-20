using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class CoreContext : DbContext
    {
        public CoreContext(DbContextOptions<CoreContext> options)
            : base(options)
        {
        }

        public DbSet<Good> Goods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Good>(x =>
            {
                x.ToTable("Goods");
                x.HasKey(e => e.Id);
                x.Ignore(e => e.OnOrdQty);
                x.Ignore(e => e.ShipQty);
                x.Ignore(e => e.RejectQty);
            });
        }
    }
}
