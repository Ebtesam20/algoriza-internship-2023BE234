using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vezeeta.API.Dto.BookingDtos;
using Vezeeta.API.Dto.PatientDtos;
using Vezeeta.API.Dtos;
using Vezeeta.API.Errors;
using Vezeeta.API.Helpers;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Repository;
using Vezeeta.Core.Services;
using Vezeeta.Core.Specifications;
using Vezeeta.Core.Specifications.BookingSpecifications;
using Vezeeta.Core.Specifications.PatientSpecifications;
using Vezeeta.Service;

namespace Vezeeta.API.Controllers.AdminControllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatientsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //You can Using localization in gender , Accept-Language = ar
        #region Patients
        [HttpGet("GetAllPatients")]
        public async Task<ActionResult<PatientToReturnDto>> GetAllPatients([FromQuery] SpecParams specParams)
        {
            var spec = new PatientsSpecification(specParams);
            var patients = await _unitOfWork.Repository<ApplicationUser>().GetAllAsyncSpec(spec);
            var patientsResults = new List<PatientToReturnDto>();
            if (patients != null && patients.Any())
            {
                foreach (var patient in patients)
                {

                    var patientsResult = _mapper.Map<ApplicationUser, PatientToReturnDto>(patient);

                    patientsResults.Add(patientsResult);
                }
                return Ok(patientsResults);
            }
            return NotFound(new ApiResponse(404));
        }

        [HttpGet("GetPatientById/{patientId}")]
        public async Task<ActionResult<PatientProfileReturnDto>> GetPatientById(string patientId)
        {
            var patient = await _unitOfWork.Repository<ApplicationUser>().GetByIdStringAsync(patientId);

            if (patient is null)
                return NotFound(new ApiResponse(404));

            var booking = await _unitOfWork.Repository<Booking>().GetAllAsyncSpec(new BookingTimeAndPatientCheckup(patientId));
            if (booking is null)
                return NotFound(new ApiResponse(404));

            var patientDetailsDto = new PatientProfileReturnDto
            {
                Details = _mapper.Map<PatientToReturnDto>(patient),
                Requests = _mapper.Map<List<BookingReturnDto>>(booking)
            };

            return Ok(patientDetailsDto);

        }
        #endregion





    }



}
