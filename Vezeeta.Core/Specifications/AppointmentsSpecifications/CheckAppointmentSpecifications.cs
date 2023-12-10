using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Entities;

namespace Vezeeta.Core.Specifications.AppointmentsSpecifications
{
    public class CheckAppointmentSpecifications : BaseSpecification<Appointments>
    {
        public CheckAppointmentSpecifications(int id, Days day) : base(A => A.DoctorId == id && A.Day == day) { }
    }
}
