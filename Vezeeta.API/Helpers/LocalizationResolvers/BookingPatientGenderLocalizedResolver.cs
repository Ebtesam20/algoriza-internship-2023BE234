using AutoMapper;
using Microsoft.Extensions.Localization;
using Vezeeta.API.Dto.DoctorDtos;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Resources;

namespace Vezeeta.API.Helpers.LocalizationResolvers
{
    public class BookingPatientGenderLocalizedResolver : IValueResolver<Booking, DoctorBookingDto, string>
    {


        private readonly IStringLocalizer<GenderResources> _localizer;

        public BookingPatientGenderLocalizedResolver(IStringLocalizer<GenderResources> localizer)
        {
            _localizer = localizer;
        }

        public string Resolve(Booking source, DoctorBookingDto destination, string destMember, ResolutionContext context)
        {
            var localizedDay = _localizer[source.Patient.Gender.ToString().ToString()];
            return localizedDay;
        }
    }
}
