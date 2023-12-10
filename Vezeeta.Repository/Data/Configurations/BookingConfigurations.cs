using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Entities;

namespace Vezeeta.Repository.Data.Configurations
{
    public class BookingConfigurations : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Property(B => B.FinalPrice).HasColumnType("decimal(18,2)");
            builder.Property(B => B.Price).HasColumnType("decimal(18,2)");

            builder.HasOne(B => B.Patient).WithMany().HasForeignKey(B => B.PatientId);
            builder.HasOne(B => B.Time).WithMany().HasForeignKey(B => B.TimeId);
            builder.HasOne(B => B.DiscountCodeCoupon).WithMany().HasForeignKey(B => B.DiscountCodeCouponId);
        }
    }
}
