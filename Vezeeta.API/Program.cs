using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Vezeeta.API.Extensions;
using Vezeeta.API.Middlewares;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Repository;
using Vezeeta.Repository;
using Vezeeta.Repository.Data;
using Vezeeta.Repository.Data_Seeding;

namespace Vezeeta.API
{
    public class Program
    {
       
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Configure Services

            builder.Services.AddControllers()
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            //.AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.PropertyNamingPolicy = null;
            //})
            .AddDataAnnotationsLocalization();
           
                            

            //builder.Services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    var supportedCultures = new[]
            //    {
            //       new CultureInfo("en"),
            //       new CultureInfo("ar"),

            //    };

            //    options.DefaultRequestCulture = new RequestCulture(culture: supportedCultures[0]);
            //    options.SupportedCultures = supportedCultures;
            //});

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(); 

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddIdentityServices(builder.Configuration);

           
            builder.Services.AddApplicationServices();


            #endregion

            var app = builder.Build();

            #region Update Database
            using var Scope = app.Services.CreateScope();
            var Service = Scope.ServiceProvider;
            var LoggerFactory = Service.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbContext = Service.GetRequiredService<AppDbContext>();
                await dbContext.Database.MigrateAsync();

                var roleManager = Service.GetRequiredService<RoleManager<IdentityRole>>();
                await SeedingAdmin.SeedRoles(roleManager);

                var userManager = Service.GetRequiredService<UserManager<ApplicationUser>>();
                await SeedingAdmin.SeedAdminUser(userManager);

                
            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error occured during apply the migration");
                throw;
            }
            #endregion

            // Configure the HTTP request pipeline.
           // app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();



            var supportedCultures = new[] { "en", "ar" };

            var localizationOptions = new RequestLocalizationOptions()
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures)
                .SetDefaultCulture(supportedCultures[0]); // Set a default culture as fallback

            localizationOptions.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
            {
                var userLangs = context.Request.Headers["Accept-Language"].ToString();
                var firstLang = userLangs.Split(',').FirstOrDefault();
                var defaultCulture = supportedCultures.Contains(firstLang) ? firstLang : supportedCultures[0];
                return Task.FromResult(new ProviderCultureResult(defaultCulture));
            }));
            app.UseRequestLocalization(localizationOptions);
           
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}