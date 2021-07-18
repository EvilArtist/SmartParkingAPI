using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartParkingAbstract.Services.Parking;
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
    public class SubscriptionTypeService : ISubscriptionTypeService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly Expression<Func<SubscriptionType, SubscriptionTypeViewModel>> selector = x => new SubscriptionTypeViewModel(x.ClientId)
        {
            Id = x.Id,
            Description = x.Description,
            Name = x.Name,
            CardCount = x.Cards.Count,
        };

        public SubscriptionTypeService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<SubscriptionTypeViewModel> CreateSubscriptionType(CreateUpdateSubscriptionTypeViewModel model)
        {
            var newSubscriptionType = mapper.Map<SubscriptionType>(model);
            var result = await dbContext.AddAsync(newSubscriptionType);
            await dbContext.SaveChangesAsync();
            return mapper.Map<SubscriptionTypeViewModel>(result.Entity);
        }

        public async Task<SubscriptionTypeViewModel> GetSubscriptionTypeById(string clientId, Guid id)
        {
            var subscription = await dbContext.SubscriptionTypes
                .Where(x => x.ClientId == clientId && x.Id == id)
                .Select(selector)
                .FirstOrDefaultAsync();
            return subscription;
        }

        public async Task<IEnumerable<SubscriptionTypeViewModel>> GetSubscriptionTypes(string clientId)
        {
            var subscriptions = await dbContext.SubscriptionTypes
                .Where(x => x.ClientId == clientId)
                .Select(selector)
                .ToListAsync();
            return subscriptions;
        }

        public async Task<SubscriptionTypeViewModel> UpdateSubscriptionType(CreateUpdateSubscriptionTypeViewModel model)
        {
            var subscription = await dbContext.SubscriptionTypes
                .Where(x => x.ClientId == model.ClientId && x.Id == model.Id)
                .FirstOrDefaultAsync();
            mapper.Map(model, subscription);
            var result = dbContext.Update(subscription);
            await dbContext.SaveChangesAsync();
            return mapper.Map<SubscriptionTypeViewModel>(result.Entity);
        }
    }
}
