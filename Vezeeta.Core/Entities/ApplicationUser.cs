using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Entities
{
    public class ApplicationUser: IdentityUser
    {
       public string FirstName { get; set; }
       public string LastName { get; set; }
       public string? Image { get; set; }
       public Gender Gender { get; set; }
       public DateTime DateOfBirth { get; set; }
       public AccountType AccountType { get; set; }
       public DateTime DateOfCreation { get; set; } = DateTime.Now;
       


       





    }
}
