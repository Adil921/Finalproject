using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Control.Panel.Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<SearchHistory> SearchHistorys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Suppress the warning for pending model changes
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _ = modelBuilder.Entity<Client>().HasData([
                new Client{
                    ClientId = 1,
                    FirstName = "Adil",
                    LastName = "shah",
                    MobileNumber = "+92 3483439712",
                    PersonalId =   "12345678912",
                    Role= "Admin",
                    Sex = "Male",
                    Email = "admin@systems.com",
                    Password = PasswordHashHandler.HashPassword("abc123"),
                }
                ]);


        }

    }
}
