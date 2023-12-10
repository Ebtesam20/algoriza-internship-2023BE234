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
    public class DoctorConfiguratons : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.Property(P => P.Price).HasColumnType("decimal(18,2)");

            builder.HasOne(P => P.User).WithMany().HasForeignKey(P => P.UserId);
            builder.HasOne(P => P.Specialization).WithMany().HasForeignKey(P => P.SpecializationId);
        }
    }
}
