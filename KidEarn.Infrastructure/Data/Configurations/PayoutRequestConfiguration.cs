using KidEarn.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Infrastructure.Data.Configurations
{
    public class PayoutRequestConfiguration : IEntityTypeConfiguration<PayoutRequest>
    {
        public void Configure(EntityTypeBuilder<PayoutRequest> builder)
        {
            builder.ToTable("PayoutRequests");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Amount)
                .HasPrecision(18, 2);

            builder.Property(p => p.Purpose)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.RejectionReason)
                .HasMaxLength(250);

            builder.Property(p => p.Status)
                .IsRequired();

            builder.HasOne(p => p.Child)
                .WithMany(c => c.PayoutRequests)
                .HasForeignKey(p => p.ChildId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
