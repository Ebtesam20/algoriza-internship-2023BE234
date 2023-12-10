using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using Vezeeta.API.Dto.AppointmentsDtos;
using Vezeeta.API.Dto.TimesDtos;
using Vezeeta.API.Errors;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Repository;
using Vezeeta.Core.Services;
using Vezeeta.Core.Specifications;
using Vezeeta.Core.Specifications.AppointmentsSpecifications;
using Vezeeta.Core.Specifications.BookingSpecifications;
using Vezeeta.Core.Specifications.DoctorSpecifications;

namespace Vezeeta.API.Controllers.DoctorControllers
{
    [Authorize(Roles = "Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorSettingController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;


        public DoctorSettingController(UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }



        #region Appointment

        [HttpPost("AddAppointment")]
        public async Task<ActionResult<bool>> AddAppointment(AddAppointmentDto appointmentDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            var spec = new DoctorIdSpecifications(user.Id);
            var doctor = await _unitOfWork.Repository<Doctor>().GetEntityAsyncSpec(spec);
            if (doctor is null)
                return NotFound();

           

            //Check if price changed or not
            if (doctor.Price != appointmentDto.Price)
            {
                doctor.Price = appointmentDto.Price;
                _unitOfWork.Repository<Doctor>().Update(doctor);
                await _unitOfWork.Complete();
            }
      
            foreach (var dayWithTimes in appointmentDto.Days)
            {
                //Check for the day
                if (!Enum.TryParse(dayWithTimes.Day, out Days day))
                {
                    return BadRequest(new ApiValidationErrorResponse()
                    {
                        Errors = new string[] { "Invalid Day Input!" }
                    });
                }
                //Check if this appointment is already exist or not
                var Appointmentspec = new CheckAppointmentSpecifications(doctor.Id, day);
                var CheckAppointment = await _unitOfWork.Repository<Appointments>().GetAllAsyncSpec(Appointmentspec);
                if (CheckAppointment.FirstOrDefault() is not null)
                    return BadRequest(new ApiValidationErrorResponse()
                    {
                        Errors = new string[] { "This Appointment is already Exist!" }
                    });
                var appointment = new Appointments
                {
                    DoctorId = doctor.Id,
                    Day = day
                };

                await _unitOfWork.Repository<Appointments>().AddAsync(appointment);
                await _unitOfWork.Complete();

                foreach (var timeSlot in dayWithTimes.Times)
                {
                    var time = new Times
                    {
                        Appointments = appointment,
                        Time = timeSlot.Time
                    };

                    await _unitOfWork.Repository<Times>().AddAsync(time);
                    await _unitOfWork.Complete();
                }
            }


            return Ok(true);


        }


        [HttpPut("UpdateAppointment")]
        public async Task<ActionResult<bool>> UpdateAppointment(TimeUpdateDto timeUpdate)
        {
            if (timeUpdate is null)
                return BadRequest(new ApiResponse(400));

            var existingtime = await _unitOfWork.Repository<Times>().GetByIdAsync(timeUpdate.Id);
            if (existingtime is null)
                return NotFound(new ApiResponse(400));
            var checkTimeSpec = new BookingTimeAndPatientCheckup(existingtime.Id);
            var checkTimeBooking = await _unitOfWork.Repository<Booking>().GetEntityAsyncSpec(checkTimeSpec);
            if (checkTimeBooking is not null && (checkTimeBooking.RequestType == RequestType.Completed || checkTimeBooking.RequestType == RequestType.Pending))
            {
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "Sorry, This Time is already booked, You cannot update it!" }
                });
            }
            if (!Enum.TryParse(timeUpdate.Day, out Days day))
            {
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "Invalid Day Input!" }
                });
            }

            //Update On Time
            existingtime.Time = timeUpdate.Time;
           
            // Check if there is any updates on the Day in appointment Table and if there any change it will apply it
            var appointment = await _unitOfWork.Repository<Appointments>().GetByIdAsync(existingtime.AppointmentsId);
           
            if (appointment.Day != day)
                appointment.Day = day;


            _unitOfWork.Repository<Appointments>().Update(appointment);
            _unitOfWork.Repository<Times>().Update(existingtime);
            await _unitOfWork.Complete();

            return Ok(true);
        }


        [HttpDelete("DeleteTime")]
        public async Task<ActionResult<bool>> DeleteTime(int id)
        {
            var time = await _unitOfWork.Repository<Times>().GetByIdAsync(id);
            if (time is null)
                return NotFound(new ApiResponse(404));
            var checkTimeSpec = new BookingTimeAndPatientCheckup(id);
            var checkTimeBooking = await _unitOfWork.Repository<Booking>().GetEntityAsyncSpec(checkTimeSpec);
            if (checkTimeBooking is not null && (checkTimeBooking.RequestType == RequestType.Completed || checkTimeBooking.RequestType == RequestType.Pending))
            {
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "Sorry, This Time is already booked, You cannot delete it!" }
                });
            }
            _unitOfWork.Repository<Times>().Delete(time);
            await _unitOfWork.Complete();
            return Ok(true);
        }
        #endregion
    }
}
