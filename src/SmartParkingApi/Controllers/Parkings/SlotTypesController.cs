using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartParkingAbstract.Services.Parking;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking;
using SmartParkingCoreModels.Data;
using SmartParkingCoreModels.Parking;
using static SmartParking.Share.Constants.IdentityConstants;

namespace SmartParkingApi.Controllers.Parkings
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotTypesController : ControllerBase
    {
        private readonly ISlotTypeService slotTypeService;

        public SlotTypesController(ISlotTypeService slotTypeService)
        {
            this.slotTypeService = slotTypeService;
        }

        // GET: api/SlotTypes
        [HttpGet]
        public async Task<ServiceResponse<IEnumerable<SlotTypeViewModel>>> GetSlotTypes()
        {
            try
            {
                var result = await slotTypeService.SearchSlotTypesAsync();
                return ServiceResponse<IEnumerable<SlotTypeViewModel>>.Success(result);
            }
            catch (Exception e)
            {
                var error = new ServiceError()
                {
                    ErrorCode = "500",
                    ErrorMessage = e.Message
                };
                return ServiceResponse<IEnumerable<SlotTypeViewModel>>.Fail(error);
            }
           
        }

        [HttpGet("{id}")]
        public async Task<ServiceResponse<SlotTypeViewModel>> GetSlotType(Guid id)
        {
            try
            {
                var result = await slotTypeService.GetSlotTypeByIdAsync(id);
                return ServiceResponse<SlotTypeViewModel>.Success(result);
            }
            catch (Exception e)
            {
                var error = new ServiceError()
                {
                    ErrorCode = "500",
                    ErrorMessage = e.Message
                };
                return ServiceResponse<SlotTypeViewModel>.Fail(error);
            }
        }

        // POST: api/SlotTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ServiceResponse<SlotTypeViewModel>> PostSlotType(SlotTypeViewModel slotType)
        {
            try
            {
                var result = await slotTypeService.CreateSlotTypeAsync(slotType);
                return ServiceResponse<SlotTypeViewModel>.Success(result);
            }
            catch (Exception e)
            {
                var error = new ServiceError()
                {
                    ErrorCode = "500",
                    ErrorMessage = e.Message
                };
                return ServiceResponse<SlotTypeViewModel>.Fail(error);
            }
        }

        [HttpPost("update")]
        public async Task<ServiceResponse<SlotTypeViewModel>> UpdateSlotType(SlotTypeViewModel slotType)
        {
            try
            {
                var result = await slotTypeService.UpdateSlotTypeAsync(slotType);
                return ServiceResponse<SlotTypeViewModel>.Success(result);
            }
            catch (Exception e)
            {
                var error = new ServiceError()
                {
                    ErrorCode = "500",
                    ErrorMessage = e.Message
                };
                return ServiceResponse<SlotTypeViewModel>.Fail(error);
            }
        }

        // DELETE: api/SlotTypes/5
        [HttpPost("delete")]
        public async Task<ServiceResponse<bool>> DeleteSlotType(EntityDeleteViewModel deleteViewModel)
        {
            try
            {
                var result = await slotTypeService.DeleteSlotTypeAsync(deleteViewModel);
                if (result)
                {
                    return ServiceResponse<bool>.Success(result);
                }
                else
                {
                    var error = new ServiceError()
                    {
                        ErrorCode = "500",
                        ErrorMessage = "Something went wrong"
                    };
                    return ServiceResponse<bool>.Fail(error);
                }
            }
            catch (Exception e)
            {
                var error = new ServiceError()
                {
                    ErrorCode = "500",
                    ErrorMessage = e.Message
                };
                return ServiceResponse<bool>.Fail(error);
            }
        }

        [HttpGet("available")]
        public async Task<ServiceResponse<IEnumerable<SlotTypeViewModel>>> GetAvailableSlotType(Guid parkingId)
        {
            try
            {
                var result = await slotTypeService.GetSlotTypesAvailableAsync(parkingId);
                return ServiceResponse<IEnumerable<SlotTypeViewModel>>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<IEnumerable<SlotTypeViewModel>>.Fail(e);
            }
        }
    }
}
