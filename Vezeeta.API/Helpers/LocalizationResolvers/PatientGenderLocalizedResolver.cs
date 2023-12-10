using AutoMapper;
using Microsoft.Extensions.Localization;
using Vezeeta.API.Dto.PatientDtos;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Resources;

namespace Vezeeta.API.Helpers.LocalizationResolvers
{
    public class PatientGenderLocalizedResolver:IValueResolver< ApplicationUser, PatientToReturnDto, string>
    {


        private readonly IStringLocalizer<GenderResources> _localizer;

        public PatientGenderLocalizedResolver(IStringLocalizer<GenderResources> localizer)
        {
            _localizer = localizer;
        }


        public string Resolve(ApplicationUser source, PatientToReturnDto destination, string destMember, ResolutionContext context)
        {
            var localizedGender = _localizer[source.Gender.ToString()];

            return localizedGender;
        }
    }
}
