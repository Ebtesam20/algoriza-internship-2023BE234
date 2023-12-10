using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Entities;

namespace Vezeeta.Core.Specifications.BookingSpecifications
{
    public class BookingWithDoctorSpecifications : BaseSpecification<Booking>
    {
        public BookingWithDoctorSpecifications(string doctorid) : base(B => B.Time.Appointments.Doctor.UserId == doctorid)
        {

        }
        public BookingWithDoctorSpecifications(string doctorid, SpecParams specParams) : base(D =>
            (string.IsNullOrEmpty(specParams.Search) || D.Patient.FirstName.ToLower().Contains(specParams.Search)
             || D.Patient.LastName.ToLower().Contains(specParams.Search) || D.Patient.Email.ToLower().Contains(specParams.Search)
             || D.Time.Time.ToString().Contains(specParams.Search) || D.DateOfCreation.Year.ToString().Contains(specParams.Search)
             || D.DateOfCreation.Month.ToString().Contains(specParams.Search)|| D.DateOfCreation.Day.ToString().Contains(specParams.Search))

             && D.Time.Appointments.Doctor.UserId == doctorid)
        {
            Includes.Add(B => B.Time);
            Includes.Add(B => B.Patient);
            Includes.Add(B => B.Time.Appointments);

            ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
        }
    }
}
