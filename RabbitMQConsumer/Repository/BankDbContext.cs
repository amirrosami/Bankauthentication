using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQConsumer.Repository
{
    public class BankDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=gina.iran.liara.ir,30640;Initial Catalog=BankDB;User Id=sa;Password=ivT6QmlFY6Y5q7hfdnaTgoYQ;");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Users> Users { get; set; }

        // public Users GetUsers(string username)
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
