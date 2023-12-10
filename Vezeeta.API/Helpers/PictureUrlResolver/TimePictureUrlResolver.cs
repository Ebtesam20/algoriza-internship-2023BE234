using AutoMapper;
using AutoMapper.Execution;
using Vezeeta.API.Dto.BookingDtos;
using Vezeeta.Core.Entities;

namespace Vezeeta.API.Helpers.PictureUrlResolver
{
    public class TimePictureUrlResolver : IValueResolver<Times, DoctorAppointmentsDto, string>
    {
        private readonly IConfiguration _configuration;

        public TimePictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Times source, DoctorAppointmentsDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Appointments.Doctor.User.Image))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.Appointments.Doctor.User.Image}";
            }
            else
                return string.Empty;
        }
    }
}
