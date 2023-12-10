using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vezeeta.API.Dto;
using Vezeeta.API.Dto.PatientDtos;
using Vezeeta.API.Errors;
using Vezeeta.API.Helpers;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Repository;
using Vezeeta.Core.Services;

namespace Vezeeta.API.Controllers.PatientControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientRegisterationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenServices _tokenServices;


        public PatientRegisterationController(UserManager<ApplicationUser> userManager,
             ITokenServices tokenServices)
        {
            _userManager = userManager;
            _tokenServices = tokenServices;
        }


        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register([FromForm] RegisterPatientDto register)
        {
            if (CheckEmailExists(register.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "This email is already exist!" }
                }); ;
            #region Check Gender value
            if (!Enum.TryParse(register.Gender, out Gender gender))
            {
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "Invalid Gender!" }
                });
            }
            #endregion
            var imageUrl = "";
            if (register.ImageUrl is not null)
            {

                if (register.ImageUrl != null && register.ImageUrl.Length > 0)
                {

                    imageUrl = await DocumentSettings.SaveImageFileAsync(register.ImageUrl);
                }
            }
            var user = new ApplicationUser()
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                PhoneNumber = register.PhoneNumber,
                UserName = register.Email.Split('@')[0],
                Image = imageUrl,
                Gender = gender,
                DateOfBirth = register.DateOfBirth,
                AccountType = AccountType.Patient,

            };
            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            await _userManager.AddToRoleAsync(user, "Patient");

            return Ok(new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = await _tokenServices.GenerateToken(user, _userManager)
            });
        }

        [HttpGet("checkemail")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
