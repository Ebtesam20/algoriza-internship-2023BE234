using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Entities;

namespace Vezeeta.Core.Specifications.SpecializationSpecifications
{
    public class CheckSpecializationSpecifications:BaseSpecification<Specialization>
    {
        public CheckSpecializationSpecifications(string specializeName) : base(S => S.SpecializeName == specializeName)
        {

        }
    }
}
