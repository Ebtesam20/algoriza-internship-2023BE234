using AutoMapper;
using Microsoft.Extensions.Localization;
using Vezeeta.API.Dto.BookingDtos;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Resources;

namespace Vezeeta.API.Helpers.LocalizationResolvers
{
    public class DoctorTimesDayLocalizedResolver : IValueResolver<Times, List<AppointmentTimeToreturnDto>, string>
    {


        private readonly IStringLocalizer<DayResources> _localizer;

        public DoctorTimesDayLocalizedResolver(IStringLocalizer<DayResources> localizer)
        {
            _localizer = localizer;
        }

        public string Resolve(Times source, List<AppointmentTimeToreturnDto> destination, string destMember, ResolutionContext context)
        {
            if (source.Appointments != null)
            {
               
                var localizedDay = _localizer[source.Appointments.Day.ToString()];
                return localizedDay;
            }

            return null; 
        }
    }
}
