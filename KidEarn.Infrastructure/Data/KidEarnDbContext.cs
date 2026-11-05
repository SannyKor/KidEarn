using KidEarn.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace KidEarn.Infrastructure.Data
{
    public class KidEarnDbContext : DbContext
    {
        public KidEarnDbContext(DbContextOptions<KidEarnDbContext> options)
            : base(options)
        {
        }

        public DbSet<Child> Children => Set<Child>();
        public DbSet<Subject> Subjects => Set<Subject>();
        public DbSet<Grade> Grades => Set<Grade>();
        public DbSet<ExtraTask> ExtraTasks => Set<ExtraTask>();
        public DbSet<DepositAccount> DepositAccounts => Set<DepositAccount>();
        public DbSet<WalletTransaction> WalletTransactions => Set<WalletTransaction>();
        public DbSet<PayoutRequest> PayoutRequests => Set<PayoutRequest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Сканує поточну збірку (KidEarn.Infrastructure) і реєструє всі класи, 
            // що реалізують IEntityTypeConfiguration<T>
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
