using System;
using System.Collections.Generic;
using System.Text;
using KidEarn.Domain.Entities;
using KidEarn.Infrastructure.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KidEarn.Infrastructure.Data.Configurations
{
    public class FamilyConfiguration : IEntityTypeConfiguration<Family>
    {
        public void Configure(EntityTypeBuilder<Family> builder)
        {
            builder.ToTable("Families");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(f => f.Children)
                .WithOne(c => c.Family)
                .HasForeignKey(c => c.FamilyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<ApplicationUser>()
                .WithOne(u => u.Family)
                .HasForeignKey(u => u.FamilyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
