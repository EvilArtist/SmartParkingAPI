using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParkingAbstract.Services.Parking;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SmartParking.Share.Constants.IdentityConstants;

namespace SmartParkingApi.Controllers.Parkings
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardService cardService;

        public CardsController(ICardService cardService)
        {
            this.cardService = cardService;
        }

        [HttpPost("search")]
        public async Task<ServiceResponse<QueryResultModel<CardViewModel>>> SearchParking(QueryModel queryModel)
        {
            try
            {
                var result = await cardService.GetCards(queryModel);
                return ServiceResponse<QueryResultModel<CardViewModel>>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<QueryResultModel<CardViewModel>>.Fail(e);
            }
        }

        [HttpPost("create")]
        public async Task<ServiceResponse<CardViewModel>> CreateParking(CreateCardViewModel model)
        {
            try
            {
                var result = await cardService.CreateCard(model);
                return ServiceResponse<CardViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<CardViewModel>.Fail(e);
            }
        }

        [HttpGet("{cardId}")]
        public async Task<ServiceResponse<CardViewModel>> GetParkingById(Guid cardId)
        {
            try
            {
                var result = await cardService.GetCardById( cardId);
                return ServiceResponse<CardViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<CardViewModel>.Fail(e);
            }
        }

        [HttpGet]
        public async Task<ServiceResponse<CardViewModel>> GetParkingByName([FromQuery]string name)
        {
            try
            {
                var result = await cardService.GetCardByName(name);
                return ServiceResponse<CardViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<CardViewModel>.Fail(e);
            }
        }

        [HttpPost("update")]
        public async Task<ServiceResponse<CardViewModel>> UpdateParking(UpdateCardViewModel model)
        {
            try
            {
                var result = await cardService.UpdateCard(model);
                return ServiceResponse<CardViewModel>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<CardViewModel>.Fail(e);
            }
        }
    }
}
