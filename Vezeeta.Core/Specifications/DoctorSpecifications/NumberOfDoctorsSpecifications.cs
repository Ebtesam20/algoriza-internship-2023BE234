﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Entities;

namespace Vezeeta.Core.Specifications.DoctorSpecifications
{
    public class NumberOfDoctorsSpecifications: BaseSpecification<ApplicationUser>
    {
        public NumberOfDoctorsSpecifications():base(P => P.AccountType == AccountType.Doctor)
        {

        }
    }
}
