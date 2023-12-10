using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Entities;

namespace Vezeeta.Core.Specifications.BookingSpecifications
{
    public class BookingTimeAndPatientCheckup : BaseSpecification<Booking>
    {
        public BookingTimeAndPatientCheckup(string patientId, int timeId)
            : base(B => B.PatientId == patientId && B.TimeId == timeId)
        {

        }
        public BookingTimeAndPatientCheckup(int timeId)
            : base(B => B.TimeId == timeId)
        {

        }
        public BookingTimeAndPatientCheckup()
        {
            Includes.Add(B => B.Time);
            Includes.Add(B => B.Patient);
            Includes.Add(B => B.DiscountCodeCoupon);
            Includes.Add(B => B.Time.Appointments);
            Includes.Add(B => B.Time.Appointments.Doctor);
            Includes.Add(B => B.Time.Appointments.Doctor.Specialization);
            Includes.Add(B => B.Time.Appointments.Doctor.User);
        }

        public BookingTimeAndPatientCheckup(string patientId) : base(B => B.PatientId == patientId)
        {
            Includes.Add(B => B.Time);
            Includes.Add(B => B.Patient);
            Includes.Add(B => B.DiscountCodeCoupon);
            Includes.Add(B => B.Time.Appointments);
            Includes.Add(B => B.Time.Appointments.Doctor);
            Includes.Add(B => B.Time.Appointments.Doctor.Specialization);
            Includes.Add(B => B.Time.Appointments.Doctor.User);
        }

        public BookingTimeAndPatientCheckup(int doctorid, string patientid) : base(B => B.PatientId == patientid && B.Time.Appointments.DoctorId == doctorid && B.RequestType == RequestType.Pending)
        {
            //Includes.Add(B => B.Time);
            //Includes.Add(B => B.Time.Appointments);
            //Includes.Add(B => B.Time.Appointments.Doctor);
        }
    }
}
