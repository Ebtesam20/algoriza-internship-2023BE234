using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using Vezeeta.API.Dto.DoctorDtos;
using Vezeeta.API.Errors;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Repository;
using Vezeeta.Core.Services;
using Vezeeta.Core.Specifications;
using Vezeeta.Core.Specifications.BookingSpecifications;

namespace Vezeeta.API.Controllers.DoctorControllers
{
    [Authorize(Roles = "Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorBookingController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DoctorBookingController(UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet("GetAllBooking")]
        public async Task<ActionResult<DoctorBookingDto>> GetAllBooking([FromQuery] SpecParams doctorSpecParams)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var doctor = await _userManager.FindByEmailAsync(email);

            var spec = new BookingWithDoctorSpecifications(doctor.Id, doctorSpecParams);
            var bookings = await _unitOfWork.Repository<Booking>().GetAllAsyncSpec(spec);

            if (bookings is null || !bookings.Any())
            {
                return NotFound(new ApiResponse(404));
            }

            var doctorDtos = _mapper.Map<List<DoctorBookingDto>>(bookings);
            return Ok(doctorDtos);

        }



        [HttpPut("ConfirmCheckup")]
        public async Task<ActionResult<bool>> ConfirmCheckup(int bookingId)
        {
            var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(bookingId);

            if (booking is null)
                return NotFound(new ApiResponse(404));

            if (booking.RequestType == RequestType.Completed)
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "This Booking is already Completed!" }
                });

            else if (booking.RequestType == RequestType.Canceled)
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "This Booking is already Canceled!" }
                });

            booking.RequestType = RequestType.Completed;

            _unitOfWork.Repository<Booking>().Update(booking);
            await _unitOfWork.Complete();
            return Ok(true);
        }
    }
}
