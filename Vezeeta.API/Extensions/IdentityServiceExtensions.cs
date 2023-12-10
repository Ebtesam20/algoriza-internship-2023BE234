using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Services;
using Vezeeta.Repository.Data;
using Vezeeta.Service;

namespace Vezeeta.API.Extensions
{
    public static class IdentityServiceExtensions
    {
        
            public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
            {
                services.AddScoped<ITokenServices, TokenServices>();
                services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {

                })
                    .AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidIssuer = configuration["JWT:ValidIssuer"],
                            ValidateAudience = true,
                            ValidAudience = configuration["JWT:ValidAudience"],
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                        };
                    })
                    .AddGoogle(options =>
                    {
                        options.ClientId = configuration["Authentication:Google:ClientId"];
                        options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                    });


            return services;
            }
        
    }
}
