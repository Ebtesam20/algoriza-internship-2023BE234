using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Entities;

namespace Vezeeta.Core.Specifications.BookingSpecifications
{
    public class TotalNumberOfPendingRequestsSpecifications : BaseSpecification<Booking>
    {
        public TotalNumberOfPendingRequestsSpecifications() : base(B => B.RequestType == RequestType.Pending)
        {

        }
    }
}
