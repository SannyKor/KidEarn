using KidEarn.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Infrastructure.Data.Configurations
{
    public class DepositAccountConfiguration : IEntityTypeConfiguration<DepositAccount>
    {
        public void Configure(EntityTypeBuilder<DepositAccount> builder)
        {
            builder.ToTable("DepositAccounts");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Balance)
                .HasPrecision(18, 2)
                .HasDefaultValue(0.00m);

            builder.Property(d => d.InterestRate)
                .HasPrecision(5, 4) // Дозволяє зберігати точні відсотки, наприклад 0.0500 (5%)
                .HasDefaultValue(0.0500m);
        }
    }
}
