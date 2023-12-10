using AutoMapper;
using Vezeeta.API.Dto.BookingDtos;
using Vezeeta.Core.Entities;

namespace Vezeeta.API.Helpers.PictureUrlResolver
{
    public class BookingPictureUrlResolver : IValueResolver<Booking, BookingReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public BookingPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Booking source, BookingReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Time.Appointments.Doctor.User.Image))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.Time.Appointments.Doctor.User.Image}";
            }
            else
                return string.Empty;

        }


    }
}
