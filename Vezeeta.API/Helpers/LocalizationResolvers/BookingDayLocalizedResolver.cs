using AutoMapper;
using Microsoft.Extensions.Localization;
using Vezeeta.API.Dto.BookingDtos;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Resources;

namespace Vezeeta.API.Helpers.LocalizationResolvers
{
    public class BookingDayLocalizedResolver:IValueResolver<Booking, BookingReturnDto, string>
    {


        private readonly IStringLocalizer<DayResources> _localizer;

        public BookingDayLocalizedResolver(IStringLocalizer<DayResources> localizer)
        {
            _localizer = localizer;
        }


        public string Resolve(Booking source, BookingReturnDto destination, string destMember, ResolutionContext context)
        {
            var localizedDay = _localizer[source.Time.Appointments.Day.ToString()];
            return localizedDay;
        }
    }

    
}
