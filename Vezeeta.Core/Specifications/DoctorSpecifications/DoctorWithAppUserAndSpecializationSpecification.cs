using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Entities;

namespace Vezeeta.Core.Specifications.DoctorSpecifications
{
    public class DoctorWithAppUserAndSpecializationSpecification : BaseSpecification<Doctor>
    {
        public DoctorWithAppUserAndSpecializationSpecification(SpecParams specParams) : base(D =>
            string.IsNullOrEmpty(specParams.Search) || D.User.FirstName.ToLower().Contains(specParams.Search)
             || D.User.LastName.ToLower().Contains(specParams.Search) || D.User.Email.ToLower().Contains(specParams.Search)
             || D.Specialization.SpecializeName.ToLower().Contains(specParams.Search))
        {
            Includes.Add(D => D.User);
            Includes.Add(D => D.Specialization);


            ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
        }

        public DoctorWithAppUserAndSpecializationSpecification(int id) : base(D => D.Id == id)
        {
            Includes.Add(D => D.User);
            Includes.Add(D => D.Specialization);
        }
    }
}
