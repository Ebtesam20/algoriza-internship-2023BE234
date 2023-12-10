using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Entities;

namespace Vezeeta.Core.Specifications.TimesSpecifications
{
    public class TimeWithAppointmentSpecifications : BaseSpecification<Times>
    {
        public TimeWithAppointmentSpecifications(SpecParams specParams) : base(T =>
            string.IsNullOrEmpty(specParams.Search) || T.Appointments.Doctor.User.FirstName.ToLower().Contains(specParams.Search)
             || T.Appointments.Doctor.User.LastName.ToLower().Contains(specParams.Search) || T.Appointments.Doctor.User.Email.ToLower().Contains(specParams.Search)
             || T.Appointments.Doctor.Specialization.SpecializeName.ToLower().Contains(specParams.Search))
        {
            Includes.Add(T => T.Appointments);
            Includes.Add(T => T.Appointments.Doctor);
            Includes.Add(T => T.Appointments.Doctor.Specialization);
            Includes.Add(T => T.Appointments.Doctor.User);

            ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
        }
        public TimeWithAppointmentSpecifications(int id) : base(T => T.Id == id)
        {
            Includes.Add(T => T.Appointments);
            Includes.Add(T => T.Appointments.Doctor);
            Includes.Add(T => T.Appointments.Doctor.Specialization);
            Includes.Add(T => T.Appointments.Doctor.User);
        }

    }
}
