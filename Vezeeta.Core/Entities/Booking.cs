using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }
        public int TimeId { get; set; }
        public Times Time { get; set; }
        public decimal Price { get; set; }
        public int? DiscountCodeCouponId { get; set; }
        public DiscountCodeCoupon? DiscountCodeCoupon { get; set; }
        public decimal FinalPrice { get; set; }
        public RequestType RequestType { get; set; }
        public DateTime DateOfCreation { get; set; } = DateTime.Now;


    }
}
