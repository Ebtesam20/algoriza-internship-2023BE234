using AutoMapper;
using AutoMapper.Execution;
using Vezeeta.API.Dto.PatientDtos;
using Vezeeta.Core.Entities;

namespace Vezeeta.API.Helpers.PictureUrlResolver
{
    public class PatientPictureUrlResolver : IValueResolver<ApplicationUser, PatientToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public PatientPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(ApplicationUser source, PatientToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Image))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.Image}";
            }
            else
                return string.Empty;

        }
    }
}
