using AutoMapper;
using Microsoft.Extensions.Localization;
using Vezeeta.API.Dto.BookingDtos;
using Vezeeta.API.Dto.DoctorDtos;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Resources;

namespace Vezeeta.API.Helpers.LocalizationResolvers
{
    public class BookingPatientDayLocalizedResolver : IValueResolver<Booking, DoctorBookingDto, string>
    {


        private readonly IStringLocalizer<DayResources> _localizer;

        public BookingPatientDayLocalizedResolver(IStringLocalizer<DayResources> localizer)
        {
            _localizer = localizer;
        }

        public string Resolve(Booking source, DoctorBookingDto destination, string destMember, ResolutionContext context)
        {
            var localizedDay = _localizer[source.Time.Appointments.Day.ToString()];
            return localizedDay;
        }
    }
}
