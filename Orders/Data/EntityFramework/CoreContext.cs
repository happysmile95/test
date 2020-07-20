using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class CoreContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Orders;Trusted_Connection=True;");
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
