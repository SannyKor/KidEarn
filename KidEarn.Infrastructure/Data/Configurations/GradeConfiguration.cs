using KidEarn.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Infrastructure.Data.Configurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.ToTable("Grades");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Value)
                .IsRequired();

            builder.Property(g => g.DateReceived)
                .IsRequired();

            builder.Property(g => g.Comment)
                .HasMaxLength(250);

            builder.HasOne(g => g.Child)
                .WithMany(c => c.Grades)
                .HasForeignKey(g => g.ChildId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(g => g.Subject)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
