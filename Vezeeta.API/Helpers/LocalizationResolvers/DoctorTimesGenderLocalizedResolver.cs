using AutoMapper;
using Microsoft.Extensions.Localization;
using Vezeeta.API.Dto.BookingDtos;
using Vezeeta.API.Dto.DoctorDtos;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Resources;

namespace Vezeeta.API.Helpers.LocalizationResolvers
{
    public class DoctorTimesGenderLocalizedResolver : IValueResolver<Times, DoctorAppointmentsDto, string>
    {


        private readonly IStringLocalizer<GenderResources> _localizer;

        public DoctorTimesGenderLocalizedResolver(IStringLocalizer<GenderResources> localizer)
        {
            _localizer = localizer;
        }

        public string Resolve(Times source, DoctorAppointmentsDto destination, string destMember, ResolutionContext context)
        {
            var localizedGender = _localizer[source.Appointments.Doctor.User.Gender.ToString()];

            return localizedGender;
        }
    }
}
