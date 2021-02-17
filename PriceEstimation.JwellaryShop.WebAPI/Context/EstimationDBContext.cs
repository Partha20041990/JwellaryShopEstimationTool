using Microsoft.EntityFrameworkCore;
using PriceEstimation.JwellaryShop.WebApi.Entities;

namespace PriceEstimation.JwellaryShop.WebApi.Context
{
    public class EstimationDBContext:DbContext
    {
        public EstimationDBContext(DbContextOptions<EstimationDBContext> dbContextOptions)
            : base(dbContextOptions) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);
        }
    }
}
