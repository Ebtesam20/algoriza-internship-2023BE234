using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Entities;

namespace Vezeeta.Core.Specifications.PatientSpecifications
{
    public class PatientsSpecification : BaseSpecification<ApplicationUser>
    {
        public PatientsSpecification(SpecParams specParams) : base(P => P.AccountType == AccountType.Patient && (string.IsNullOrEmpty(specParams.Search) || P.FirstName.ToLower().Contains(specParams.Search)
             || P.LastName.ToLower().Contains(specParams.Search) || P.Email.ToLower().Contains(specParams.Search)))
        {
            ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
        }

        public PatientsSpecification(string id) : base(P => P.Id == id)
        {

        }
    }
}
