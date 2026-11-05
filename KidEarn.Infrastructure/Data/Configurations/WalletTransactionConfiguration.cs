using KidEarn.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Infrastructure.Data.Configurations
{
    public class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
    {
        public void Configure(EntityTypeBuilder<WalletTransaction> builder)
        {
            builder.ToTable("WalletTransactions");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Amount)
                .HasPrecision(18, 2);

            builder.Property(w => w.Type)
                .IsRequired();

            builder.Property(w => w.Description)
                .IsRequired()
                .HasMaxLength(250);

            builder.HasOne(w => w.Child)
                .WithMany(c => c.Transactions)
                .HasForeignKey(w => w.ChildId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
