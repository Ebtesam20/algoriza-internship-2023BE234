using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Numerics;
using System.Text;
using Vezeeta.API.Dto;
using Vezeeta.API.Dto.DoctorDtos;
using Vezeeta.API.Dtos;
using Vezeeta.API.Email_Service;
using Vezeeta.API.Errors;
using Vezeeta.API.Helpers;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Repository;
using Vezeeta.Core.Resources;
using Vezeeta.Core.Services;
using Vezeeta.Core.Specifications;
using Vezeeta.Core.Specifications.BookingSpecifications;
using Vezeeta.Core.Specifications.DoctorSpecifications;
using Vezeeta.Core.Specifications.PatientSpecifications;
using static System.Net.Mime.MediaTypeNames;

namespace Vezeeta.API.Controllers.AdminControllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
       
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer<SpecializationResources> _localizer;

        public DashboardController( IUnitOfWork unitOfWork, IConfiguration configuration, IStringLocalizer<SpecializationResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _localizer = localizer;
        }



        #region Dashboard
        [HttpGet("NumOfDoctors")]
        public async Task<int> NumOfDoctors([FromQuery] string? timeRange)
        {
            var spec = new NumberOfDoctorsSpecifications();
            var doctors = (IEnumerable<ApplicationUser>)await _unitOfWork.Repository<ApplicationUser>().GetAllAsyncSpec(spec);
            switch (timeRange?.ToLower())
            {
                case "24hours":
                    doctors = doctors.Where(d => d.DateOfCreation >= DateTime.Now.AddHours(-24));
                    break;

                case "week":
                    doctors = doctors.Where(d => d.DateOfCreation >= DateTime.Now.AddDays(-7));
                    break;

                case "month":
                    doctors = doctors.Where(d => d.DateOfCreation >= DateTime.Now.AddMonths(-1));
                    break;

                case "year":
                    doctors = doctors.Where(d => d.DateOfCreation >= DateTime.Now.AddYears(-1));
                    break;

                default:
                    return doctors.Count();

            }
            return doctors.Count();
        }

        [HttpGet("NumOfPatients")]
        public async Task<int> NumOfPatients([FromQuery] string? timeRange)
        {
            var spec = new NumberofPatientSpecifications();
            var patients = (IEnumerable<ApplicationUser>)await _unitOfWork.Repository<ApplicationUser>().GetAllAsyncSpec(spec);
            switch (timeRange?.ToLower())
            {
                case "24hours":
                    patients = patients.Where(d => d.DateOfCreation >= DateTime.Now.AddHours(-24));
                    break;

                case "week":
                    patients = patients.Where(d => d.DateOfCreation >= DateTime.Now.AddDays(-7));
                    break;

                case "month":
                    patients = patients.Where(d => d.DateOfCreation >= DateTime.Now.AddMonths(-1));
                    break;

                case "year":
                    patients = patients.Where(d => d.DateOfCreation >= DateTime.Now.AddYears(-1));
                    break;

                default:
                    return patients.Count();

            }
            return patients.Count();
        }

        [HttpGet("NumOfRequests")]
        public async Task<ActionResult<NumberOfRequestsDto>> NumOfRequests([FromQuery] string? timeRange)
        {
            var totalRequestSpec = new TotalNumberOfRequestsSpecifications();
            var requests =(IEnumerable<Booking>) await _unitOfWork.Repository<Booking>().GetAllAsyncSpec(totalRequestSpec);

            var pendingRequestSpec = new TotalNumberOfPendingRequestsSpecifications();
            var pendingRequests = (IEnumerable<Booking>)await _unitOfWork.Repository<Booking>().GetAllAsyncSpec(pendingRequestSpec);

            var completedRequestSpec = new TotalNumberOfCompletedRequestsSpecifications();
            var completedRequests = (IEnumerable<Booking>)await _unitOfWork.Repository<Booking>().GetAllAsyncSpec(completedRequestSpec);

            var canceledRequestSpec = new TotalNumberOfCanceledRequestsSpecifications();
            var canceledRequests = (IEnumerable<Booking>)await _unitOfWork.Repository<Booking>().GetAllAsyncSpec(canceledRequestSpec);

            switch (timeRange?.ToLower())
            {
                case "24hours":
                    requests = requests.Where(d => d.DateOfCreation >= DateTime.Now.AddHours(-24));
                    pendingRequests = pendingRequests.Where(d => d.DateOfCreation >= DateTime.Now.AddHours(-24));
                    completedRequests = completedRequests.Where(d => d.DateOfCreation >= DateTime.Now.AddHours(-24));
                    canceledRequests = canceledRequests.Where(d => d.DateOfCreation >= DateTime.Now.AddHours(-24));
                    break;

                case "week":
                    requests = requests.Where(d => d.DateOfCreation >= DateTime.Now.AddDays(-7));
                    pendingRequests = pendingRequests.Where(d => d.DateOfCreation >= DateTime.Now.AddDays(-7));
                    completedRequests = completedRequests.Where(d => d.DateOfCreation >= DateTime.Now.AddDays(-7));
                    canceledRequests = canceledRequests.Where(d => d.DateOfCreation >= DateTime.Now.AddDays(-7));
                    break;

                case "month":
                    requests = requests.Where(d => d.DateOfCreation >= DateTime.Now.AddMonths(-1));
                    pendingRequests = pendingRequests.Where(d => d.DateOfCreation >= DateTime.Now.AddMonths(-1));
                    completedRequests = completedRequests.Where(d => d.DateOfCreation >= DateTime.Now.AddMonths(-1));
                    canceledRequests = canceledRequests.Where(d => d.DateOfCreation >= DateTime.Now.AddMonths(-1));
                    break;

                case "year":
                    requests = requests.Where(d => d.DateOfCreation >= DateTime.Now.AddYears(-1));
                    pendingRequests = pendingRequests.Where(d => d.DateOfCreation >= DateTime.Now.AddYears(-1));
                    completedRequests = completedRequests.Where(d => d.DateOfCreation >= DateTime.Now.AddYears(-1));
                    canceledRequests = canceledRequests.Where(d => d.DateOfCreation >= DateTime.Now.AddYears(-1));
                    break;

                default:
                    return Ok(new NumberOfRequestsDto()
                    {
                        TotalNumberOfRequests = requests.Count(),
                        NumberOfPendingRequests = pendingRequests.Count(),
                        NumberOfCompletedRequests = completedRequests.Count(),
                        NumberOfCanceledRequests = canceledRequests.Count(),
                    });

            }

            return Ok(new NumberOfRequestsDto()
            {
                TotalNumberOfRequests = requests.Count(),
                NumberOfPendingRequests = pendingRequests.Count(),
                NumberOfCompletedRequests = completedRequests.Count(),
                NumberOfCanceledRequests = canceledRequests.Count(),
            });


        }

        [HttpGet("Top5Specializations")]
        public async Task<ActionResult<IEnumerable<object>>> GetTop5Specializations()
        {
            var spec = new BookingTimeAndPatientCheckup();
            var bookings = await _unitOfWork.Repository<Booking>().GetAllAsyncSpec(spec);
            if (bookings is null)
                return NotFound(new ApiResponse(404));

            var topSpecializations = bookings
                .GroupBy(b => b.Time.Appointments.Doctor.Specialization.Id)
                .Select(group => new
                {
                    SpecializationName =_localizer[group.FirstOrDefault()?.Time.Appointments.Doctor.Specialization.SpecializeName].Value,
                    NumberOfRequests = group.Count()
                })
                .OrderByDescending(result => result.NumberOfRequests)
                .Take(5)
                .ToList();

            return Ok(topSpecializations);
        }

        [HttpGet("Top10Doctors")]
        public async Task<ActionResult<IEnumerable<Top10Doctors>>> GetTop10Doctors()
        {
            var spec = new BookingTimeAndPatientCheckup();
            var bookings = await _unitOfWork.Repository<Booking>().GetAllAsyncSpec(spec);
            if (bookings is null)
                return NotFound(new ApiResponse(404));

            var topDoctors = bookings
                .GroupBy(b => b.Time.Appointments.Doctor.Id)
                .Select(group => new Top10Doctors
                {
                    Image = $"{_configuration["ApiBaseUrl"]}{group.FirstOrDefault()?.Time.Appointments.Doctor.User.Image}",
                    FullName = $"{group.FirstOrDefault()?.Time.Appointments.Doctor.User.FirstName} {group.FirstOrDefault()?.Time.Appointments.Doctor.User.LastName}", // Assuming you have a property like FullName
                    Specialization =_localizer[group.FirstOrDefault()?.Time.Appointments.Doctor.Specialization.SpecializeName],
                    NumberOfRequests = group.Count(),

                })
             .OrderByDescending(result => result.NumberOfRequests)
             .Take(10)
             .ToList();

            return Ok(topDoctors);



        }
        #endregion






























    }
}
