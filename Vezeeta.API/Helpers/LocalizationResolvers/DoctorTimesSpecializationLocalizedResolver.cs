using AutoMapper;
using Microsoft.Extensions.Localization;
using Vezeeta.API.Dto.BookingDtos;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Resources;

namespace Vezeeta.API.Helpers.LocalizationResolvers
{
    public class DoctorTimesSpecializationLocalizedResolver : IValueResolver<Times, DoctorAppointmentsDto, string>
    {


        private readonly IStringLocalizer<SpecializationResources> _localizer;

        public DoctorTimesSpecializationLocalizedResolver(IStringLocalizer<SpecializationResources> localizer)
        {
            _localizer = localizer;
        }

        public string Resolve(Times source, DoctorAppointmentsDto destination, string destMember, ResolutionContext context)
        {
            var localizedSpecializeName = _localizer[source.Appointments.Doctor.Specialization.SpecializeName];

            return localizedSpecializeName;
        }
    }
}
