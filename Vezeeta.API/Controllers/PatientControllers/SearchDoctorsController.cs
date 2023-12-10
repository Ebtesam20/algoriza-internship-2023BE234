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
using Vezeeta.Core.Specifications.DiscountCodeSpec;
using Vezeeta.Core.Specifications.TimesSpecifications;

namespace Vezeeta.API.Controllers.PatientControllers
{
    [Authorize(Roles = "Patient")]
    [Route("api/Patient/[controller]")]
    [ApiController]
    public class SearchDoctorsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SearchDoctorsController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        [HttpPost("Booking")]
        public async Task<ActionResult<bool>> Booking([FromQuery]int timeId, string? discountCode)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);


            #region Check if input time is valid or not
            var timeSpec = new TimeWithAppointmentSpecifications(timeId);
            var Time = await _unitOfWork.Repository<Times>().GetEntityAsyncSpec(timeSpec);
            if (Time is null)
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "This Time doesnt exist!" }
                });
            #endregion

            #region Check if Time is booked or not
            var bookingtimespec = new BookingTimeAndPatientCheckup(timeId);
            var bookingTime = await _unitOfWork.Repository<Booking>().GetEntityAsyncSpec(bookingtimespec);
            if (bookingTime is not null)
            {
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "This Time is already booked!" }
                });
            }
            #endregion

            #region Check if patient try booking with the same doctor without complete the previous booking first
            var bookingdoctorspec = new BookingTimeAndPatientCheckup(Time.Appointments.DoctorId, user.Id);
            var bookingSameDoctor = await _unitOfWork.Repository<Booking>().GetEntityAsyncSpec(bookingdoctorspec);
            if (bookingSameDoctor is not null)
            {
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "Sorry, You cannot book with the same doctor until you complete the booking first!" }
                });
            }
            #endregion

            #region Check if input discount code is null
            if (string.IsNullOrEmpty(discountCode))
            {
                var Booking = new Booking()
                {
                    PatientId = user.Id,
                    TimeId = timeId,
                    Price = Time.Appointments.Doctor.Price,
                    FinalPrice = Time.Appointments.Doctor.Price,
                    RequestType = RequestType.Pending

                };
                await _unitOfWork.Repository<Booking>().AddAsync(Booking);
                await _unitOfWork.Complete();
                return Ok(true);
            }
            #endregion

            else
            {
                var discountSpec = new DiscountCodeSpecifications(discountCode);
                var dicountCodeCoupon = await _unitOfWork.Repository<DiscountCodeCoupon>().GetEntityAsyncSpec(discountSpec);

                #region Check if discount is exist or not
                if (dicountCodeCoupon is null)
                    return BadRequest(new ApiValidationErrorResponse()
                    {
                        Errors = new string[] { "This Discount Code doesnt exist!" }
                    });
                #endregion

                #region Check if discount code is used before or not
                var existSpec = new BookingDiscountCodeCheckUpSpecifications(user.Id, dicountCodeCoupon.Id);
                var existDiscountCode = await _unitOfWork.Repository<Booking>().GetEntityAsyncSpec(existSpec);
                if (existDiscountCode is not null)
                    return BadRequest(new ApiValidationErrorResponse()
                    {
                        Errors = new string[] { "This Discount Code was used before!" }
                    });
                #endregion

                #region Check if discount code is active or not
                if (dicountCodeCoupon.IsActive == false)
                    return BadRequest(new ApiValidationErrorResponse()
                    {
                        Errors = new string[] { "This Discount Code is not active!" }
                    });
                #endregion

                var patientSpec = new BookingDiscountCodeCheckUpSpecifications(user.Id);
                var patient = await _unitOfWork.Repository<Booking>().GetAllAsyncSpec(patientSpec);
                #region Check if Booking meets discount code requirements or not
                if (patient.Count(P => P.RequestType == RequestType.Completed) >= dicountCodeCoupon.NumOfCompletedRequest)
                {
                    decimal finalPrice = 0;
                    if (dicountCodeCoupon.DiscountType == DiscountType.Value)
                    {
                        if (dicountCodeCoupon.value < Time.Appointments.Doctor.Price)
                            finalPrice = Time.Appointments.Doctor.Price - dicountCodeCoupon.value;
                        else
                            finalPrice = dicountCodeCoupon.value - Time.Appointments.Doctor.Price;
                        var Booking = new Booking()
                        {
                            PatientId = user.Id,
                            TimeId = timeId,
                            Price = Time.Appointments.Doctor.Price,
                            DiscountCodeCouponId = dicountCodeCoupon.Id,
                            FinalPrice = finalPrice,
                            RequestType = RequestType.Pending

                        };
                        await _unitOfWork.Repository<Booking>().AddAsync(Booking);
                        await _unitOfWork.Complete();
                        return Ok(true);
                    }

                    else
                    {
                        var Booking = new Booking()
                        {
                            PatientId = user.Id,
                            TimeId = timeId,
                            DiscountCodeCouponId = dicountCodeCoupon.Id,
                            Price = Time.Appointments.Doctor.Price,
                            FinalPrice = Time.Appointments.Doctor.Price - Time.Appointments.Doctor.Price * (dicountCodeCoupon.value / 100),
                            RequestType = RequestType.Pending

                        };
                        await _unitOfWork.Repository<Booking>().AddAsync(Booking);
                        await _unitOfWork.Complete();
                        return Ok(true);
                    }
                }
                else
                    return BadRequest(new ApiValidationErrorResponse()
                    {
                        Errors = new string[] { "Sorry, Your Booking doesnt meet requirements to apply Discount code!" }
                    });
                #endregion
            }


        }



        [HttpGet("GetAll")]
        public async Task<ActionResult<DoctorAppointmentsDto>> GetAll([FromQuery] SpecParams specParams)
        {
            var spec = new TimeWithAppointmentSpecifications(specParams);
            var doctorTimes = await _unitOfWork.Repository<Times>().GetAllAsyncSpec(spec);

            if (doctorTimes is null || !doctorTimes.Any())
            {
                return NotFound(new ApiResponse(404));
            }

            var doctorDtos = _mapper.Map<List<DoctorAppointmentsDto>>(doctorTimes);
            return Ok(doctorDtos);

        }
    }
}
