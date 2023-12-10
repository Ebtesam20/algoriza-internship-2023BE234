using AutoMapper;
using Vezeeta.API.Dto.DoctorDtos;
using Vezeeta.Core.Entities;

namespace Vezeeta.API.Helpers.PictureUrlResolver
{
    public class DoctorPictureUrlResolver : IValueResolver<Doctor, DoctorToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public DoctorPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Doctor source, DoctorToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.User.Image))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.User.Image}";
            }
            else
                return string.Empty;

        }


    }
}
