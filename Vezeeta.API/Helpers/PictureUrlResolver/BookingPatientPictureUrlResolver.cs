using AutoMapper;
using AutoMapper.Execution;
using Vezeeta.API.Dto.DoctorDtos;
using Vezeeta.Core.Entities;

namespace Vezeeta.API.Helpers.PictureUrlResolver
{
    public class BookingPatientPictureUrlResolver : IValueResolver<Booking, DoctorBookingDto, string>
    {
        private readonly IConfiguration _configuration;

        public BookingPatientPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Booking source, DoctorBookingDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Patient.Image))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.Patient.Image}";
            }
            else
                return string.Empty;
        }
    }
}
