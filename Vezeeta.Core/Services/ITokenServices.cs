using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Entities;

namespace Vezeeta.Core.Services
{
    public interface ITokenServices
    {
        Task<string> GenerateToken(ApplicationUser user, UserManager<ApplicationUser> userManager);
    }
}
