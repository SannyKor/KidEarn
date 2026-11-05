using KidEarn.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Infrastructure.Data.Configurations
{
    public class ChildConfiguration : IEntityTypeConfiguration<Child>
    {
        public void Configure(EntityTypeBuilder<Child> builder)
        {
            builder.ToTable("Children");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.DailyBaseAllowance)
                .HasPrecision(18, 2)
                .HasDefaultValue(0.00m);

            builder.Property(c => c.CurrentWeeklyMultiplier)
                .HasPrecision(5, 2)
                .HasDefaultValue(1.00m);

            builder.Property(c => c.NutBalance)
                .HasPrecision(18, 2)
                .HasDefaultValue(0.00m);

            builder.Property(c => c.ParentId)
                .IsRequired()
                .HasMaxLength(450); // Стандартна довжина Id для ASP.NET Core Identity

            // Зв'язок 1:1 з депозитним рахунком
            builder.HasOne(c => c.DepositAccount)
                .WithOne(d => d.Child)
                .HasForeignKey<DepositAccount>(d => d.ChildId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
