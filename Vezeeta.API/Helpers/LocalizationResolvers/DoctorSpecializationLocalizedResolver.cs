using AutoMapper;
using Microsoft.Extensions.Localization;
using Vezeeta.API.Dto.BookingDtos;
using Vezeeta.API.Dto.DoctorDtos;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Resources;

namespace Vezeeta.API.Helpers.LocalizationResolvers
{
    public class DoctorSpecializationLocalizedResolver : IValueResolver<Doctor, DoctorToReturnDto, string>
    {


        private readonly IStringLocalizer<SpecializationResources> _localizer;

        public DoctorSpecializationLocalizedResolver(IStringLocalizer<SpecializationResources> localizer)
        {
            _localizer = localizer;
        }


        public string Resolve(Doctor source, DoctorToReturnDto destination, string destMember, ResolutionContext context)
        {
            var localizedSpecializeName = _localizer[source.Specialization.SpecializeName];
           
            return localizedSpecializeName;
        }
    }
}
