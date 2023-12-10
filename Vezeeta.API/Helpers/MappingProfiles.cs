using AutoMapper;
using Microsoft.Extensions.Localization;
using System.Numerics;
using Vezeeta.API.Dto.BookingDtos;
using Vezeeta.API.Dto.DiscountCodeDtos;
using Vezeeta.API.Dto.DoctorDtos;
using Vezeeta.API.Dto.PatientDtos;
using Vezeeta.API.Helpers.LocalizationResolvers;
using Vezeeta.API.Helpers.PictureUrlResolver;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Resources;
using static System.Net.Mime.MediaTypeNames;

namespace Vezeeta.API.Helpers
{
    public class MappingProfiles: Profile
    {
        private readonly IStringLocalizer<DayResources> _dayLocalizer;
        private readonly IStringLocalizer<GenderResources> _genderLocalizer;
        private readonly IStringLocalizer<SpecializationResources> _specializeLocalizer;

        public MappingProfiles(IStringLocalizer<DayResources> dayLocalizer, IStringLocalizer<GenderResources> genderLocalizer
            , IStringLocalizer<SpecializationResources> specializeLocalizer)
        {
            _dayLocalizer = dayLocalizer;
            _genderLocalizer = genderLocalizer;
            _specializeLocalizer = specializeLocalizer;
        }
      
        public MappingProfiles()
        {
          

            CreateMap<DiscountCodeInputDto, DiscountCodeCoupon>();

         

            CreateMap<ApplicationUser, PatientToReturnDto>()
                      .ForMember(d => d.FullName, o => o.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                      .ForMember(d => d.Gender, o => o.MapFrom<PatientGenderLocalizedResolver>())
                      .ForMember(d => d.Image, o => o.MapFrom<PatientPictureUrlResolver>());

            CreateMap<Doctor, DoctorToReturnDto>()
                     .ForMember(d => d.FullName, o => o.MapFrom(s => $"{s.User.FirstName} {s.User.LastName}"))
                     .ForMember(d => d.Image, o => o.MapFrom<DoctorPictureUrlResolver>())
                     .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.User.PhoneNumber))
                     .ForMember(d => d.Email, o => o.MapFrom(s => s.User.Email))
                     .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.User.DateOfBirth))
                     .ForMember(d => d.Gender, o => o.MapFrom<DoctorGenderLocalizedResolver>())  
                     .ForMember(d => d.Specialization, o => o.MapFrom<DoctorSpecializationLocalizedResolver>()); 

            CreateMap<Booking, BookingReturnDto>()
                .ForMember(d => d.DoctorName, o => o.MapFrom(s => $"{s.Time.Appointments.Doctor.User.FirstName} {s.Time.Appointments.Doctor.User.LastName}"))
                .ForMember(d => d.Image, o => o.MapFrom<BookingPictureUrlResolver>())
                .ForMember(d => d.Specialization, o => o.MapFrom<BookingSpecializationLocalizedValueResolver>())
                .ForMember(d => d.Day, o => o.MapFrom<BookingDayLocalizedResolver>())
                .ForMember(d => d.Time, o => o.MapFrom(s => s.Time.Time))
                .ForMember(d => d.DiscountCode, o => o.MapFrom(s => s.DiscountCodeCoupon.DiscountCode))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.RequestType.ToString()));

            CreateMap<Times, AppointmentTimeToreturnDto>().ForMember(d => d.Day, o => o.MapFrom(s => _dayLocalizer[s.Appointments.Day.ToString()]));
           
            CreateMap<Times, DoctorAppointmentsDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => $"{s.Appointments.Doctor.User.FirstName} {s.Appointments.Doctor.User.LastName}"))
            .ForMember(d => d.Gender, o => o.MapFrom<DoctorTimesGenderLocalizedResolver>())//MapFrom(s => s.Appointments.Doctor.User.Gender.ToString()))
            .ForMember(d => d.Specialize, o => o.MapFrom<DoctorTimesSpecializationLocalizedResolver>())//MapFrom(s => s.Appointments.Doctor.Specialization.SpecializeName))
            .ForMember(d => d.Email, o => o.MapFrom(s => s.Appointments.Doctor.User.Email))
            .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.Appointments.Doctor.User.PhoneNumber))
            .ForMember(d => d.Price, o => o.MapFrom(s => s.Appointments.Doctor.Price))
            .ForMember(d => d.Image, o => o.MapFrom<TimePictureUrlResolver>())
            .ForMember(d => d.Appointments, o => o.MapFrom(s => new List<AppointmentTimeToreturnDto>    
            {
                new AppointmentTimeToreturnDto
                {
                    Day = s.Appointments.Day.ToString(),
                    Times = new List<DoctorTimeToReturnDto>
                    {
                        new DoctorTimeToReturnDto
                        {
                            Id = s.Id,
                            Time = s.Time
                        }
                    }
                }
            }));

            CreateMap<Booking, DoctorBookingDto>()
            .ForMember(d => d.PatientName, o => o.MapFrom(s => $"{s.Patient.FirstName} {s.Patient.LastName}"))
            .ForMember(d => d.Gender, o => o.MapFrom<BookingPatientGenderLocalizedResolver>())//MapFrom(s => s.Patient.Gender.ToString()))
            .ForMember(d => d.Email, o => o.MapFrom(s => s.Patient.Email))
            .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.Patient.PhoneNumber))
            .ForMember(d => d.Image, o => o.MapFrom<BookingPatientPictureUrlResolver>())
            .ForMember(d => d.Day, o => o.MapFrom<BookingPatientDayLocalizedResolver>())//.MapFrom(s => s.Time.Appointments.Day.ToString()))
            .ForMember(d => d.Time, o => o.MapFrom(s => s.Time.Time))
            .ForMember(d => d.Age, o => o.MapFrom(s => (DateTime.Now.Year) - s.Patient.DateOfBirth.Year));

             CreateMap<Booking, PatientProfileReturnDto>()
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => new PatientToReturnDto
            {
                Image = src.Patient.Image,
                FullName = $"{src.Patient.FirstName} {src.Patient.LastName}",
                Email = src.Patient.Email,
                PhoneNumber = src.Patient.PhoneNumber,
                Gender =_genderLocalizer[ src.Patient.Gender.ToString()],
                DateOfBirth = src.Patient.DateOfBirth
            }))
            .ForMember(dest => dest.Requests, opt => opt.MapFrom(r => new List<BookingReturnDto>
             {
                new BookingReturnDto{
                 Image = r.Time.Appointments.Doctor.User.Image, 
                 DoctorName = $"{r.Time.Appointments.Doctor.User.FirstName} {r.Time.Appointments.Doctor.User.LastName}",
                 Specialization = r.Time.Appointments.Doctor.Specialization.SpecializeName,
                 Day = _dayLocalizer[ r.Time.Appointments.Day.ToString()],
                 Time = r.Time.Time,
                 Price = r.Price,
                 DiscountCode = r.DiscountCodeCoupon.DiscountCode,
                 FinalPrice = r.FinalPrice,
                 Status = r.RequestType.ToString()
                }
             }));
           
        }

     
    }
}
