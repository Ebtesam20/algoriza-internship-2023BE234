using AutoMapper;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Vezeeta.API.Dto.BookingDtos;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Resources;

namespace Vezeeta.API.Helpers.LocalizationResolvers
{
    public class BookingSpecializationLocalizedValueResolver : IValueResolver<Booking, BookingReturnDto, string>
    {


        private readonly IStringLocalizer<SpecializationResources> _localizer;

        public BookingSpecializationLocalizedValueResolver(IStringLocalizer<SpecializationResources> localizer)
        {
            _localizer = localizer;
        }

        public string Resolve(Booking source, BookingReturnDto destination, string destMember, ResolutionContext context)
        {
           
            var localizedSpecializeName = _localizer[source.Time.Appointments.Doctor.Specialization.SpecializeName];

            return localizedSpecializeName;
        }

    }


}
