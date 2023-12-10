using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Entities;

namespace Vezeeta.Core.Specifications.DoctorSpecifications
{
    public class DoctorIdSpecifications : BaseSpecification<Doctor>
    {
        public DoctorIdSpecifications(string userid) : base(D => D.UserId == userid)
        {
            Includes.Add(D => D.User);
            Includes.Add(D => D.Specialization);
        }
    }
}
