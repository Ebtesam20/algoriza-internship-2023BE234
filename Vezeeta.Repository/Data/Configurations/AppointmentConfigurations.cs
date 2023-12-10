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
    public class AppointmentConfigurations : IEntityTypeConfiguration<Appointments>
    {
        public void Configure(EntityTypeBuilder<Appointments> builder)
        {
            builder.HasOne(P => P.Doctor).WithMany().HasForeignKey(P => P.DoctorId);
            
        }
    }
}
