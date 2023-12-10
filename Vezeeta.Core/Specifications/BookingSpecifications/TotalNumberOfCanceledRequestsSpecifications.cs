using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Entities;

namespace Vezeeta.Core.Specifications.BookingSpecifications
{
    public class TotalNumberOfCanceledRequestsSpecifications : BaseSpecification<Booking>
    {
        public TotalNumberOfCanceledRequestsSpecifications() : base(B => B.RequestType == RequestType.Canceled)
        {

        }
    }
}
