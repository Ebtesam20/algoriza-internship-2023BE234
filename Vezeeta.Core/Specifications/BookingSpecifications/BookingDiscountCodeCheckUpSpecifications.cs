using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Entities;

namespace Vezeeta.Core.Specifications.BookingSpecifications
{
    public class BookingDiscountCodeCheckUpSpecifications : BaseSpecification<Booking>
    {
        public BookingDiscountCodeCheckUpSpecifications(string patientId, int discountcode)
            : base(B => B.PatientId == patientId && B.DiscountCodeCouponId == discountcode)
        {

        }
        public BookingDiscountCodeCheckUpSpecifications(int discountcode)
            : base(B => B.DiscountCodeCouponId == discountcode)
        {

        }

        public BookingDiscountCodeCheckUpSpecifications(string patientId) : base(B => B.PatientId == patientId)
        {

        }




    }
}
