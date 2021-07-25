using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartParking.Share.Constants;
using SmartParking.Share.Extensions;
using SmartParkingAbstract.Services.General;
using SmartParkingAbstract.Services.Parking.PriceBook;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking;
using SmartParkingAbstract.ViewModels.Parking.PriceBook;
using SmartParkingCoreModels.Data;
using SmartParkingCoreModels.Parking.PriceBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreServices.Parking.PriceBook
{
    public class PriceBookService : IPriceBookService
    {

        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IHelpers helpers;

        public PriceBookService(ApplicationDbContext dbContext, IMapper mapper, IHelpers helpers)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.helpers = helpers;
        }

        public async Task<PriceBookViewModel> CreatePriceBooks(CreateUpdatePriceViewModel model)
        {
            PriceList priceList = mapper.Map<PriceList>(model);
            priceList.Condition = CreateNewCondition(model);
            priceList.Calculation.Name = model.Name + " " + model.Calculation.FormularType.ToString();
            var result = await dbContext.AddAsync(priceList);
            await dbContext.SaveChangesAsync();
            await result.Reference(x => x.SubscriptionType).LoadAsync();
            await result.Reference(x => x.VehicleType).LoadAsync();

            return mapper.Map<PriceBookViewModel>(result.Entity);
        }

        private PriceListCondition CreateNewCondition(CreateUpdatePriceViewModel model)
        {
            PriceListCondition condition = model.Condition.ConditionType switch
            {
                PriceCondition.Default => mapper.Map<PriceListDefaultCondition>(model.Condition),
                PriceCondition.Weekday => mapper.Map<PriceListWeekdayCondition>(model.Condition),
                PriceCondition.Holliday => mapper.Map<PriceListHollidayCondition>(model.Condition),
                PriceCondition.Duration => mapper.Map<PriceListDurationCondition>(model.Condition),
                _ => mapper.Map<PriceListDefaultCondition>(model.Condition),
            };
            condition.Name = model.Name + " " + model.Condition.ConditionType.ToString();
            if(condition is PriceListWeekdayCondition)
            {
                (condition as PriceListWeekdayCondition).Days = model.Condition.Days;
            }
            return condition;
        }

        public async Task<PriceBookViewModel> GetPriceBookById(string clientId, Guid id)
        {
            var query = dbContext.PriceLists
                .Include(x => x.VehicleType)
                .Include(x => x.SubscriptionType)
                .Include(x => x.Condition)
                .Include(x => x.Calculation)
                .Where(x => x.ClientId == clientId &&
                    x.Id == id 
                );
            var result = await query.FirstOrDefaultAsync();
            var viewModel = mapper.Map<PriceBookViewModel>(result);
            viewModel.Condition = mapper.Map<PriceConditionViewModel>(result.Condition);
            return viewModel;
        }

        public async Task<QueryResultModel<PriceBookViewModel>> GetPriceBooks(PriceListQuery queryParam)
        {
            var query = dbContext.PriceLists
                .Include(x=>x.VehicleType)
                .Include(x=>x.SubscriptionType)
                .Include(x=>x.Condition)
                .Include(x=>x.Calculation)
                .Where(x => x.ClientId == queryParam.ClientId &&
                    x.VehicleTypeId == queryParam.VehicleTypeId &&
                    x.SubscriptionTypeId == queryParam.SubscriptionTypeId
                );
            var totalCount = await query.CountAsync();
            var result = await query
                .OrderBy(x=> x.Condition.Name)
                .ThenBy(x=>x.Calculation.Name)
                .OrderBy(x=>x.Name)
                .PagedBy(queryParam.Page, queryParam.PageSize)
                .ToListAsync();
            var data = mapper.Map<List<PriceList>, List<PriceBookViewModel>>(result);
            return new QueryResultModel<PriceBookViewModel>(data)
            {
                Page = queryParam.Page,
                TotalCount = totalCount
            };
        }

        public IEnumerable<EnumViewModel> GetPriceCondition()
        {
            var values = Enum.GetValues<PriceCondition>();
            return values.Select(x => new EnumViewModel
            {
                Value = (int)x,
                Description = helpers.GetEnumDescription(x)
            });
        }

        public IEnumerable<EnumViewModel> GetPriceFomulars()
        {
            var values = Enum.GetValues<PriceFormular>();
            var UnitMap = new Dictionary<PriceFormular, string>
            {
                {PriceFormular.ByDate, "Ngày" },
                {PriceFormular.ByMonth, "Tháng" },
                {PriceFormular.ByTime, "Giờ" },
                {PriceFormular.ByTurn, "Lượt" },
                {PriceFormular.ByWeek, "Tuần" },
            };
            return values.Select(x => new FormularTypeEnumViewModel
            {
                Unit = UnitMap[x],
                Value = (int)x,
                Description = helpers.GetEnumDescription(x)
            });
        }

        public async Task<PriceBookViewModel> UpdatePriceBooks(CreateUpdatePriceViewModel model)
        {
            var priceList = await dbContext.PriceLists
                .Include(x => x.VehicleType)
                .Include(x => x.SubscriptionType)
                .Include(x => x.Condition)
                .Include(x => x.Calculation)
                .Where(x => x.ClientId == model.ClientId &&
                    x.Id == model.Id
                ).FirstOrDefaultAsync();
            mapper.Map(model, priceList);
            
            dbContext.Remove(priceList.Condition);
            priceList.Condition = CreateNewCondition(model);
            priceList.Calculation = mapper.Map<PriceCalculation>(model.Calculation);
            var result = dbContext.Update(priceList);
            await dbContext.SaveChangesAsync();
            await result.Reference(x => x.SubscriptionType).LoadAsync();
            await result.Reference(x => x.VehicleType).LoadAsync();

            return mapper.Map<PriceBookViewModel>(result.Entity);
        }
    }
}
