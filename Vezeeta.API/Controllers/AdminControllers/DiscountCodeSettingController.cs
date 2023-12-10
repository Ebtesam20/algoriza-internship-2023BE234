using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Vezeeta.API.Dto.DiscountCodeDtos;
using Vezeeta.API.Errors;
using Vezeeta.Core.Entities;
using Vezeeta.Core.Repository;
using Vezeeta.Core.Services;
using Vezeeta.Core.Specifications;
using Vezeeta.Core.Specifications.BookingSpecifications;
using Vezeeta.Core.Specifications.DiscountCodeSpec;

namespace Vezeeta.API.Controllers.AdminControllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class DiscountCodeSettingController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DiscountCodeSettingController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #region Setting

        [HttpPost("AddDiscountCodeCoupon")]
        public async Task<ActionResult<bool>> AddDiscountCode(DiscountCodeInputDto discountCodeDto)
        {
            if (discountCodeDto is null)
                return BadRequest(new ApiResponse(400));
            if (!Enum.TryParse(discountCodeDto.DiscountType, out DiscountType discountTypeEnum))
            {
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "Invalid Discount Type!" }
                });
            }
            #region Check for Discount Type
            if (discountTypeEnum == DiscountType.Value && discountCodeDto.value <= 0)
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "Value must be greater than 0!" }
                });
            if (discountTypeEnum == DiscountType.Percentage && (discountCodeDto.value <= 0 || discountCodeDto.value > 100))
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "Precentage must be greater than 0% and less than 100%!" }
                });

            #endregion

            #region Check if there is a discount code with the sam name or not
            var discountCodeExist = await _unitOfWork.Repository<DiscountCodeCoupon>()
                   .GetEntityAsyncSpec(new DiscountCodeExistanceSpecifications(discountCodeDto.DiscountCode));
            if (discountCodeExist is not null)
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "There is a discount code coupon with the same name!" }
                });
            #endregion

            else
            {

                var discountCoupon = _mapper.Map<DiscountCodeInputDto, DiscountCodeCoupon>(discountCodeDto);
                await _unitOfWork.Repository<DiscountCodeCoupon>().AddAsync(discountCoupon);
                await _unitOfWork.Complete();
                return Ok(true);

            }
        }

        [HttpPut("UpdateDiscountCodeCoupon")]
        public async Task<ActionResult<bool>> UpdateDiscountCode(DiscountCodeUpdateDto discountCodeDto)
        {

            if (discountCodeDto is null)
                return BadRequest(new ApiResponse(400));

            else
            {
                var existingDiscountCode = await _unitOfWork.Repository<DiscountCodeCoupon>().GetByIdAsync(discountCodeDto.Id);
                if (existingDiscountCode is null)
                    return NotFound(new ApiResponse(404));

                var discountSpec = new BookingDiscountCodeCheckUpSpecifications(discountCodeDto.Id);
                var discountExist = await _unitOfWork.Repository<Booking>().GetEntityAsyncSpec(discountSpec);
                if (discountExist is not null && discountExist.RequestType != RequestType.Canceled && discountExist.DiscountCodeCoupon.IsActive == true)
                    return BadRequest(new ApiValidationErrorResponse()
                    {
                        Errors = new string[] { "Sorry, This Discount Code has been applied by patient/s, You cannot update it!" }
                    });
                if (!Enum.TryParse(discountCodeDto.DiscountType, out DiscountType discountTypeEnum))
                {
                    return BadRequest(new ApiValidationErrorResponse()
                    {
                        Errors = new string[] { "Invalid Discount Type!" }
                    });
                }

                existingDiscountCode.DiscountCode = discountCodeDto.DiscountCode;
                existingDiscountCode.DiscountType = discountTypeEnum;
                existingDiscountCode.NumOfCompletedRequest = discountCodeDto.NumOfCompletedRequest;
                existingDiscountCode.value = discountCodeDto.value;


                _unitOfWork.Repository<DiscountCodeCoupon>().Update(existingDiscountCode);
                await _unitOfWork.Complete();
                return Ok(true);
            }
        }


        [HttpDelete("DeleteDiscountCodeCoupon/{id}")]
        public async Task<ActionResult<bool>> DeleteDiscountCode(int id)
        {
            var discountCode = await _unitOfWork.Repository<DiscountCodeCoupon>().GetByIdAsync(id);
            if (discountCode is null)
                return NotFound(new ApiResponse(404));

            var discountSpec = new BookingDiscountCodeCheckUpSpecifications(id);
            var discountExist = await _unitOfWork.Repository<Booking>().GetEntityAsyncSpec(discountSpec);
            if (discountExist is not null && discountExist.RequestType != RequestType.Canceled && discountExist.DiscountCodeCoupon.IsActive == true)
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "Sorry, This Discount Code has been applied by patient/s, You cannot delete it!" }
                });

            _unitOfWork.Repository<DiscountCodeCoupon>().Delete(discountCode);
            await _unitOfWork.Complete();
            return Ok(true);
        }


        [HttpPut("DeactivateDiscountCodeCoupon/{id}")]
        public async Task<ActionResult<bool>> DeactivateDiscountCode(int id)
        {
            var discountCode = await _unitOfWork.Repository<DiscountCodeCoupon>().GetByIdAsync(id);
            if (discountCode is null)
                return NotFound(new ApiResponse(404));
            discountCode.IsActive = false;
            _unitOfWork.Repository<DiscountCodeCoupon>().Update(discountCode);
            await _unitOfWork.Complete();
            return Ok(true);
        }




        #endregion
    }
}
