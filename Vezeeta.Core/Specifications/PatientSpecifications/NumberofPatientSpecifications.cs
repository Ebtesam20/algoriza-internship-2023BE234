using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Entities;

namespace Vezeeta.Core.Specifications.PatientSpecifications
{
    public class NumberofPatientSpecifications : BaseSpecification<ApplicationUser>
    {
        public NumberofPatientSpecifications() : base(P => P.AccountType == AccountType.Patient)
        {

        }
    }
}
