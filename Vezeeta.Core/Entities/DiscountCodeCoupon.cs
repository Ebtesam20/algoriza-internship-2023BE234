using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Entities
{
    public class DiscountCodeCoupon
    {
        public int Id { get; set; }
        public string DiscountCode { get; set; }
        public int NumOfCompletedRequest { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal value { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
