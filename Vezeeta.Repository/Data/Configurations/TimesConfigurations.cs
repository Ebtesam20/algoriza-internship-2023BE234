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
    public class TimesConfigurations : IEntityTypeConfiguration<Times>
    {
        public void Configure(EntityTypeBuilder<Times> builder)
        {
            builder.HasOne(P => P.Appointments).WithMany().HasForeignKey(P => P.AppointmentsId);
            builder.Property(a => a.Time)
            .HasColumnType("time");
        }
    }
}
