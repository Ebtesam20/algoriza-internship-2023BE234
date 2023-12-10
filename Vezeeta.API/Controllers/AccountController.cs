using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vezeeta.API.Dto;
using Vezeeta.API.Dtos;
using Vezeeta.API.Errors;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Repository;
using Vezeeta.Core.Services;
using Vezeeta.Service;

namespace Vezeeta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenServices _tokenServices;
    

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ITokenServices tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenServices = tokenService;
        
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
                return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = await _tokenServices.GenerateToken(user, _userManager)
            });

        }

        #region Register By Social Media (Google)
        [AllowAnonymous]
        [HttpGet("ExternalLogin")]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(ExternalLoginCallback)),
                Items =
        {
            { "returnUrl", returnUrl },
            { "scheme", provider },
        },
            };

            return Challenge(properties, provider);
        }
        [AllowAnonymous]
        [HttpGet("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            // Retrieve the external login information
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // Handle the error, redirect to an error page, or perform other actions
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "info error!" }
                });
            }

            // Attempt to sign in the user with the external login provider
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                // User is successfully signed in, generate or return a token
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                var token = await _tokenServices.GenerateToken(user, _userManager); // Replace with your actual token generation logic
                return Ok(new { Token = token });
            }

            if (result.IsLockedOut)
            {
                // Handle the case where the user is locked out
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "Lockout!" }
                });
            }


            var newUser = new ApplicationUser
            {
                UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                Image = info.Principal.FindFirstValue("profile_picture_claim_type"),
                Gender = Enum.Parse<Gender>(info.Principal.FindFirstValue("gender_claim_type")),
                DateOfBirth = DateTime.Parse(info.Principal.FindFirstValue("date_of_birth_claim_type")),
                AccountType = AccountType.Patient,
            };

            // Persist the new user in the database
            var createResult = await _userManager.CreateAsync(newUser);
            await _userManager.AddToRoleAsync(newUser, "Patient");
            if (createResult.Succeeded)
            {
                // Add the external login to the new user
                var addLoginResult = await _userManager.AddLoginAsync(newUser, info);
                if (addLoginResult.Succeeded)
                {
                    // Sign in the new user
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    var token = await _tokenServices.GenerateToken(newUser, _userManager); // Replace with your actual token generation logic
                    return Ok(new { Token = token });
                }
            }

            // Handle the case where user creation or login fails
            return BadRequest(new ApiValidationErrorResponse()
            {
                Errors = new string[] { "External Login Failure!" }
            });
        } 
        #endregion
    }
}
