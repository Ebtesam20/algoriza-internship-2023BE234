using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Entities
{
    public class Times
    {
        public int Id { get; set; }
        public int AppointmentsId { get; set; }
        public Appointments Appointments { get; set; }
        public TimeSpan Time { get; set; }
    }
}
