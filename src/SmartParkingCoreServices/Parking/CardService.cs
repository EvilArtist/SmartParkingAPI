using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartParking.Share.Constants;
using SmartParking.Share.Extensions;
using SmartParkingAbstract.Services.Parking;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking;
using SmartParkingCoreModels.Data;
using SmartParkingCoreModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreServices.Parking
{
    public class CardService : ICardService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        private readonly Expression<Func<Card, CardViewModel>> selector = x => new CardViewModel(x.ClientId)
        {
            Id = x.Id,
            IdentityCode = x.IdentityCode,
            Name = x.Name,
            Status = x.Status.Name,
            SubscriptionTypeName = x.Subscription == null || x.Subscription.SubscriptionType == null ?
                SystemSubscriptionType.DefaultSubscriptionType.Name : 
                x.Subscription.SubscriptionType.Name,
            VehicleTypeName = x.VehicleType.Name,
            SubscriptionTypeId = x.Subscription == null ?
                SystemSubscriptionType.DefaultSubscriptionType.Code :
                x.Subscription.SubscriptionTypeId,
            VehicleTypeId = x.VehicleTypeId
        };

        public CardService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<CardViewModel> CreateCard(CreateCardViewModel model)
        {
            Card newCard = mapper.Map<Card>(model);
            bool isCardAdded = await dbContext.Cards.AnyAsync(x => x.IdentityCode == model.IdentityCode);
            if (isCardAdded)
            {
                throw new Exception("Thẻ đã được thêm");
            }
            var cardStatus = await dbContext.CardStatuses.Where(x => x.Code == CardStatusCode.Active).FirstOrDefaultAsync();
            newCard.CardStatusId = cardStatus.Id;
            var result = await dbContext.AddAsync(newCard);
            await dbContext.SaveChangesAsync();
            dbContext.Entry(result.Entity).Reference(x => x.Subscription.SubscriptionType).Load();
            dbContext.Entry(result.Entity).Reference(x => x.VehicleType).Load();
            return mapper.Map<CardViewModel>(result.Entity);
        }

        public async Task<CardViewModel> GetCardById(string clientId, Guid cardId)
        {
            var result = await dbContext.Cards
                .Where(x => x.Id == cardId && x.ClientId == clientId)
                .Select(selector)
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<CardViewModel> GetCardByCode(string clientId, string code)
        {
            var result = await dbContext.Cards
                .Where(x => x.IdentityCode == code && x.ClientId == clientId)
                .Select(selector)
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<CardViewModel> GetCardByName(string clientId, string cardName)
        {
            var result = await dbContext.Cards
                .Where(x => x.Name == cardName && x.ClientId == clientId)
                .Select(selector)
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<QueryResultModel<CardViewModel>> GetCards(QueryModel queryModel)
        {
            var data = dbContext.Cards
                .Where(x => x.ClientId == queryModel.ClientId);
            if(!string.IsNullOrEmpty(queryModel.QueryString))
            {
                data = data.Where(x => x.Name.Contains(queryModel.QueryString));
            }

            foreach (var refinement in queryModel.Refinements)
            {
                if (refinement.Key == CardRefinements.ParkingId)
                {
                    var parkingId = Guid.Parse(refinement.Value);
                    var parkingAssignment = dbContext.CardParkingAssignments
                        .Where(x => x.ParkingId == parkingId)
                        .Select(x => x.CardId)
                        .Distinct();
                    data = data.Join(parkingAssignment, x => x.Id, y => y, (x, y) => x);
                }

                if (refinement.Key == CardRefinements.NotAssignedToParkingId)
                {
                    var parkingId = Guid.Parse(refinement.Value);
                    var parkingAssignment = dbContext.CardParkingAssignments
                        .Where(x => x.ParkingId == parkingId)
                        .Select(x => x.CardId)
                        .Distinct();
                }

                if (refinement.Key == CardRefinements.CardStatus)
                {
                    var cardStatus = refinement.Value;

                    data = data.Where(x => x.Status.Code == cardStatus);
                }
            }
            var totalCount = await data.CountAsync();
            var result = await data
                .Include(x=>x.Status)
                .Include(x=>x.Subscription)
                .ThenInclude(y => y.SubscriptionType)
                .Include(x=>x.VehicleType)
                .OrderBy(x => x.Name)
                .Select(selector)
                .PagedBy(queryModel.Page, queryModel.PageSize)
                .ToListAsync();
            return new QueryResultModel<CardViewModel>(result)
            {
                Page = queryModel.Page,
                TotalCount = totalCount
            };
        }

        public async Task<CardViewModel> UpdateCard(UpdateCardViewModel model)
        {
            var card = await dbContext.Cards
                .Where(x => x.Id == model.Id && x.ClientId == model.ClientId)
                .FirstOrDefaultAsync();

            mapper.Map(model, card);
            var result = dbContext.Update(card);
            await dbContext.SaveChangesAsync();

            dbContext.Entry(result.Entity).Reference(x => x.Subscription).Load();
            dbContext.Entry(result.Entity).Reference(x => x.Subscription.SubscriptionType).Load();
            dbContext.Entry(result.Entity).Reference(x => x.VehicleType).Load();
            return mapper.Map<CardViewModel>(result.Entity);
        }
    }
}
