using AutoMapper;
using Microsoft.Extensions.Localization;
using Vezeeta.API.Dto.DoctorDtos;
using Vezeeta.API.Dto.PatientDtos;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Resources;

namespace Vezeeta.API.Helpers.LocalizationResolvers
{
    public class DoctorGenderLocalizedResolver : IValueResolver<Doctor, DoctorToReturnDto, string>
    {


        private readonly IStringLocalizer<GenderResources> _localizer;

        public DoctorGenderLocalizedResolver(IStringLocalizer<GenderResources> localizer)
        {
            _localizer = localizer;
        }

        public string Resolve(Doctor source, DoctorToReturnDto destination, string destMember, ResolutionContext context)
        {
            var localizedGender = _localizer[source.User.Gender.ToString()];

            return localizedGender;
        }
    }
    
}
