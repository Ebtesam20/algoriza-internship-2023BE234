using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using Vezeeta.API.Dto.BookingDtos;
using Vezeeta.API.Errors;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Repository;
using Vezeeta.Core.Services;
using Vezeeta.Core.Specifications;
using Vezeeta.Core.Specifications.BookingSpecifications;

namespace Vezeeta.API.Controllers.PatientControllers
{
    [Authorize(Roles = "Patient")]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientBookingController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatientBookingController(UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetBooking")]
        public async Task<ActionResult<BookingReturnDto>> GetBooking()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            var spec = new BookingTimeAndPatientCheckup(user.Id);
            var bookings = await _unitOfWork.Repository<Booking>().GetAllAsyncSpec(spec);
            if (bookings is null)
                return NotFound(new ApiResponse(404));

            var bookingsResult = new List<BookingReturnDto>();
            foreach (var booking in bookings)
            {
                var bookingItem = _mapper.Map<Booking, BookingReturnDto>(booking);
                bookingsResult.Add(bookingItem);
            }

            return Ok(bookingsResult);
        }


        [HttpPut("CancelBooking")]
        public async Task<ActionResult<bool>> CancelBooking(int bookingId)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
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
            
            if(booking.PatientId != user.Id)
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "This is not your Booking You Cannot cancel it!" }
                });

            booking.RequestType = RequestType.Canceled;

            _unitOfWork.Repository<Booking>().Update(booking);
            await _unitOfWork.Complete();
            return Ok(true);

        }
    }
}
