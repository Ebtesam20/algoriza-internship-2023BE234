using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int SpecializationId { get; set; }
        public Specialization Specialization{ get; set; }
    }
}
