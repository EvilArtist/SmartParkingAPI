using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParkingAbstract.Services.Parking.PriceBook;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking.PriceBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SmartParking.Share.Constants.IdentityConstants;

namespace SmartParkingApi.Controllers.Parkings
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceBooksController : ControllerBase
    {
        private readonly IPriceBookService priceBookService;

        public PriceBooksController(IPriceBookService priceBookService)
        {
            this.priceBookService = priceBookService;
        }

        [HttpPost("search")]
        public async Task<ServiceResponse<QueryResultModel<PriceBookViewModel>>> SearchParking(PriceListQuery queryModel)
        {
            try
            {
                var result = await priceBookService.GetPriceBooks(queryModel);
                return ServiceResponse<QueryResultModel<PriceBookViewModel>>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<QueryResultModel<PriceBookViewModel>>.Fail(e);
            }
        }

        [HttpPost("create")]
        public async Task<ServiceResponse<PriceBookViewModel>> CreateParking(CreateUpdatePriceViewModel model)
        {
            try
            {
                var result = await priceBookService.CreatePriceBooks(model);
                return ServiceResponse<PriceBookViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<PriceBookViewModel>.Fail(e);
            }
        }

        [HttpGet("details/{priceBookId}")]
        public async Task<ServiceResponse<PriceBookViewModel>> GetParkingById(Guid priceBookId)
        {
            try
            {
                var result = await priceBookService.GetPriceBookById(priceBookId);
                return ServiceResponse<PriceBookViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<PriceBookViewModel>.Fail(e);
            }
        }

        [HttpPost("update")]
        public async Task<ServiceResponse<PriceBookViewModel>> UpdateParking(CreateUpdatePriceViewModel model)
        {
            try
            {
                var result = await priceBookService.UpdatePriceBooks(model);
                return ServiceResponse<PriceBookViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<PriceBookViewModel>.Fail(e);
            }
        }

        [HttpGet("formulars")]
        public ServiceResponse<IEnumerable<EnumViewModel>> GetFomularTypes()
        {
            return ServiceResponse<IEnumerable<EnumViewModel>>.Success(priceBookService.GetPriceFomulars());
        }

        [HttpGet("conditions")]
        public ServiceResponse<IEnumerable<EnumViewModel>> GetConditionTypes()
        {
            return ServiceResponse<IEnumerable<EnumViewModel>>.Success(priceBookService.GetPriceCondition());
        }
    }
}
