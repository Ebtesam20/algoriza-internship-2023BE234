using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Entities;

namespace Vezeeta.Repository.Data_Seeding
{
    public class SeedingAdmin
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "Patient", "Doctor" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

        }

        public static async Task SeedAdminUser(UserManager<ApplicationUser> userManager)
        {
            var adminUser = await userManager.FindByEmailAsync("ebtesammahmoud200@gmail.com");

            if (adminUser is null)
            {
                adminUser = new ApplicationUser
                {
                    FirstName= "Ebtesam",
                    LastName= "Mahmoud",
                    Image = null,
                    Gender= Gender.Female,
                    DateOfBirth = DateTime.Now,
                    AccountType= AccountType.Admin,
                    Email = "ebtesammahmoud200@gmail.com",
                    UserName = "Ebtesam.Mahmoud",
                    PhoneNumber = "0123456789"
                };

                await userManager.CreateAsync(adminUser, "P@ssw0rd"); 
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
