using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterManagement.Domain.OrderAgg;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MasterManagement.Infrastructure.EFCore.Mapping
{
    public class PaymentMapping : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.OrderId).IsRequired();
            builder.Property(p => p.Amount).IsRequired();
            builder.Property(p => p.PaymentDate).IsRequired();
            builder.Property(p => p.TransactionId).HasMaxLength(500).IsRequired();
            builder.Property(p => p.IsSucceeded).IsRequired();
            builder.Property(p => p.IsCanceled).IsRequired();
        }
    }
}

   

