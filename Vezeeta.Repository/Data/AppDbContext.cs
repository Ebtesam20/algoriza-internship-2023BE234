using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Entities;

namespace Vezeeta.Repository.Data
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<Times> Times { get; set; }
        public DbSet<DiscountCodeCoupon> DiscountCodeCoupons{ get; set; }
        public DbSet<Booking> Bookings { get; set; }



    }
}
