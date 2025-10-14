using System.Data.Entity;
using ProtechGroup.Infrastructure.Entities;

namespace ProtechGroup.Infrastructure.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("FlightBookingConnection")
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public DbSet<Airport> Airports { get; set; }
        public DbSet<SearchInput> SearchInputs { get; set; }
        public DbSet<SearchWSHistory> SearchWSHistorys { get; set; }
        public DbSet<ServiceFee> ServiceFees { get; set; }
        public DbSet<News> News { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Nếu cần mapping chi tiết:
            modelBuilder.Entity<Airport>().ToTable("Airport");
            modelBuilder.Entity<SearchInput>().ToTable("SearchInput");
            modelBuilder.Entity<SearchWSHistory>().ToTable("SearchWSHistory");
            modelBuilder.Entity<ServiceFee>().ToTable("ServiceFee");
            modelBuilder.Entity<News>().ToTable("News");

            // cấu hình column length nếu bạn muốn
            base.OnModelCreating(modelBuilder);
        }

    }
}
