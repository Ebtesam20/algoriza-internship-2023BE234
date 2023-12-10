using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Vezeeta.API.Dto;
using Vezeeta.API.Dto.DoctorDtos;
using Vezeeta.API.Dtos;
using Vezeeta.API.Email_Service;
using Vezeeta.API.Errors;
using Vezeeta.API.Helpers;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Repository;
using Vezeeta.Core.Services;
using Vezeeta.Core.Specifications;
using Vezeeta.Core.Specifications.BookingSpecifications;
using Vezeeta.Core.Specifications.DoctorSpecifications;
using Vezeeta.Core.Specifications.SpecializationSpecifications;
using Vezeeta.Service;

namespace Vezeeta.API.Controllers.AdminControllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenServices _tokenServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DoctorsController(UserManager<ApplicationUser> userManager, ITokenServices tokenServices,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _tokenServices = tokenServices;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("checkemail")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }

        #region Doctor

        [HttpPost("AddDoctor")]
        public async Task<ActionResult<UserDto>> AddDoctor([FromForm] RegisterDoctorDto registerDoctor)
        {

            if (CheckEmailExists(registerDoctor.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "This email is already exist!" }
                });

            #region Check Gender value
            if (!Enum.TryParse(registerDoctor.Gender, out Gender gender))
            {
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "Invalid Gender!" }
                });
            }
            #endregion

            #region Check Specialization Input
            var specialize = await _unitOfWork.Repository<Specialization>().GetEntityAsyncSpec(new CheckSpecializationSpecifications(registerDoctor.Specialization));
            if (specialize is null)
            {
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "Invalid Specialization input!" }
                });
            }
            #endregion

            var imageUrl = "";
            if (registerDoctor.ImageUrl != null && registerDoctor.ImageUrl.Length > 0)
            {
                imageUrl = await DocumentSettings.SaveImageFileAsync(registerDoctor.ImageUrl);
            }
            var user = new ApplicationUser()
            {
                FirstName = registerDoctor.FirstName,
                LastName = registerDoctor.LastName,
                Email = registerDoctor.Email,
                PhoneNumber = registerDoctor.PhoneNumber,
                UserName = registerDoctor.Email.Split('@')[0],
                Image = imageUrl,
                Gender = gender,
                AccountType = AccountType.Doctor,
                DateOfBirth = registerDoctor.DateOfBirth,

            };

            var result = await _userManager.CreateAsync(user, registerDoctor.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            await _userManager.AddToRoleAsync(user, "Doctor");


            var doctor = new Doctor()
            {
                Price = 0,
                UserId = user.Id,
                SpecializationId = specialize.Id
            };
            await _unitOfWork.Repository<Doctor>().AddAsync(doctor);
            await _unitOfWork.Complete();

            #region Send Email To Doctor

            var emailBody = new StringBuilder();
            emailBody.AppendLine($"Hello Dr. {user.FirstName} {user.LastName},");
            emailBody.AppendLine("Your account has been created successfully,");
            emailBody.AppendLine($"Your are now a member of our site, you can login with this email");
            emailBody.AppendLine($"Your Password: {registerDoctor.Password}");
            emailBody.AppendLine(", please don't share it.");

            var Email = new Email()
            {
                Subject = "Vezeeta Admin",
                To = user.Email,
                Body = emailBody.ToString()
            };
            EmailSettings.SendEmail(Email);

            #endregion

            return Ok(new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = await _tokenServices.GenerateToken(user, _userManager)
            });
        }

        //You can use localization here
        [HttpGet("GetAllDoctors")]
        public async Task<ActionResult<DoctorToReturnDto>> GetAllDoctors([FromQuery] SpecParams specParams)
        {
            var spec = new DoctorWithAppUserAndSpecializationSpecification(specParams);
            var doctors = await _unitOfWork.Repository<Doctor>().GetAllAsyncSpec(spec);
            var doctorResults = new List<DoctorToReturnDto>();

            if (doctors != null && doctors.Any())
            {
                foreach (var doctor in doctors)
                {
                    var doctorResult = _mapper.Map<Doctor, DoctorToReturnDto>(doctor);
                    doctorResults.Add(doctorResult);
                }
                return Ok(doctorResults);
            }
            else
                return NotFound();
        }

        //You can use localization here
        [HttpGet("GetDoctorById/{id}")]
        public async Task<ActionResult<DoctorToReturnDto>> GetDoctorById(int id)
        {
            var spec = new DoctorWithAppUserAndSpecializationSpecification(id);
            var doctor = await _unitOfWork.Repository<Doctor>().GetEntityAsyncSpec(spec);
            if (doctor is not null)
            {
                var DoctorResult = _mapper.Map<Doctor, DoctorToReturnDto>(doctor);
                return Ok(new { details = DoctorResult });
            }

            else
                return NotFound();
        }

        [HttpPut("UpdateDoctor")]
        public async Task<ActionResult<bool>> UpdateDoctor([FromForm] DoctorUpdateDto doctorUpdateDto)
        {
            if (doctorUpdateDto is null)
                return BadRequest(new ApiResponse(400));
            else
            {
                //check for doctor if exist
                var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(doctorUpdateDto.Id);
                if(doctor is  null) { return NotFound(new ApiResponse(404)); }

                var user = await _userManager.FindByIdAsync(doctor.UserId);

                if (user is not null && user.AccountType == AccountType.Doctor)
                {
                    #region Check Gender value
                    if (!Enum.TryParse(doctorUpdateDto.Gender, out Gender gender))
                    {
                        return BadRequest(new ApiValidationErrorResponse()
                        {
                            Errors = new string[] { "Invalid Gender!" }
                        });
                    }
                    #endregion

                    #region Check Specialization Input
                    var specialize = await _unitOfWork.Repository<Specialization>().GetEntityAsyncSpec(new CheckSpecializationSpecifications(doctorUpdateDto.Specialization));
                    if (specialize is null)
                    {
                        return BadRequest(new ApiValidationErrorResponse()
                        {
                            Errors = new string[] { "Invalid Specialization input!" }
                        });
                    }
                    #endregion
                    var imageUrl = "";
                    if (doctorUpdateDto.ImageUrl != null && doctorUpdateDto.ImageUrl.Length > 0)
                    {
                        imageUrl = await DocumentSettings.SaveImageFileAsync(doctorUpdateDto.ImageUrl);
                    }

                    user.FirstName = doctorUpdateDto.FirstName;
                    user.LastName = doctorUpdateDto.LastName;
                    user.Image = imageUrl;
                    user.Email = doctorUpdateDto.Email;
                    user.PhoneNumber = doctorUpdateDto.Phone;
                    user.Gender = gender;
                    user.DateOfBirth = doctorUpdateDto.DateOfBirth;

                    var result = await _userManager.UpdateAsync(user);

                    doctor.SpecializationId = specialize.Id;
                    _unitOfWork.Repository<Doctor>().Update(doctor);
                    await _unitOfWork.Complete();

                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest(new ApiValidationErrorResponse()
                        {
                            Errors = new string[] { "Update Failed" }
                        });
                    }
                }
                else
                    return NotFound(new ApiResponse(404));


            }
        }

        [HttpDelete("DeleteDoctor/{id}")]
        public async Task<ActionResult<bool>> DeleteDoctor(string id)
        {
            var doctor = await _userManager.FindByIdAsync(id);

            var bookingdoctorSpec = new BookingWithDoctorSpecifications(id);
            var bookingDoctor = await _unitOfWork.Repository<Booking>().GetEntityAsyncSpec(bookingdoctorSpec);
            if (bookingDoctor is not null)
            {
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "Sorry, This Doctor has booking, You cannot delete it!" }
                });
            }
            if (doctor is not null && doctor.AccountType == AccountType.Doctor)
            {
                await _userManager.DeleteAsync(doctor);
                return Ok(true);
            }
            else
                return BadRequest(new ApiResponse(400));
        }



        #endregion








    }
}
