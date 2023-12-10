using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Entities;

namespace Vezeeta.Core.Specifications.DiscountCodeSpec
{
    public class DiscountCodeSpecifications : BaseSpecification<DiscountCodeCoupon>
    {
        public DiscountCodeSpecifications(string discountCode) : base(D => D.DiscountCode == discountCode)
        {
        }

    }
}
