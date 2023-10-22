using BankAuthentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankAuthentication.Repository
{
    public class BankContext:DbContext
    {
        public BankContext(DbContextOptions<BankContext> options):base(options)  
        {
        }

        public DbSet<Users>  Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey("UserId");

            });
        }
    }
}
