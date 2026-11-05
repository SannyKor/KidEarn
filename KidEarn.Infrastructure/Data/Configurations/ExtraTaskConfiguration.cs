using KidEarn.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Infrastructure.Data.Configurations
{
    public class ExtraTaskConfiguration : IEntityTypeConfiguration<ExtraTask>
    {
        public void Configure(EntityTypeBuilder<ExtraTask> builder)
        {
            builder.ToTable("ExtraTasks");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(t => t.Description)
                .HasMaxLength(500);

            builder.Property(t => t.RewardAmount)
                .HasPrecision(18, 2);

            builder.Property(t => t.State)
                .IsRequired();

            builder.HasOne(t => t.Child)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.ChildId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
