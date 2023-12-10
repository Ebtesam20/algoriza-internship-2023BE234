using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Entities;

namespace Vezeeta.Core.Specifications.DiscountCodeSpec
{
    public class DiscountCodeExistanceSpecifications:BaseSpecification<DiscountCodeCoupon>
    {
        public DiscountCodeExistanceSpecifications(string discountCodeName):base(D => D.DiscountCode == discountCodeName)
        {

        }
    }
}
