using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Vezeeta.API.Errors;
using Vezeeta.API.Helpers;
using Vezeeta.Core.Repository;
using Vezeeta.Core.Resources;
using Vezeeta.Repository;

namespace Vezeeta.API.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "");

            services.AddSingleton<IStringLocalizer<DayResources>, StringLocalizer<DayResources>>();
            services.AddSingleton<IStringLocalizer<GenderResources>, StringLocalizer<GenderResources>>();
            services.AddSingleton<IStringLocalizer<SpecializationResources>, StringLocalizer<SpecializationResources>>();
            //services.AddScoped<ILogger,typeof(Logger)>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();





            services.AddAutoMapper(typeof(MappingProfiles));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actioncontext) =>
                {
                    var errors = actioncontext.ModelState.Where(P => P.Value.Errors.Count > 0)
                                                         .SelectMany(P => P.Value.Errors)
                                                         .Select(E => E.ErrorMessage)
                                                         .ToArray();
                    var ValidationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);
                };
            });

            return services;
        }
    }
}
