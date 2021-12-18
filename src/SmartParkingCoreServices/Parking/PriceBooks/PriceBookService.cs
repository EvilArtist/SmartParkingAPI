using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartParking.Share.Constants;
using SmartParking.Share.Exceptions;
using SmartParking.Share.Extensions;
using SmartParkingAbstract.Services.General;
using SmartParkingAbstract.Services.Parking.PriceBook;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking;
using SmartParkingAbstract.ViewModels.Parking.PriceBooks;
using SmartParkingCoreModels.Data;
using SmartParkingCoreModels.Parking.PriceBooks;
using SmartParkingCoreServices.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreServices.Parking.PriceBooks
{
    public class PriceBookService : MultitanentService, IPriceBookService
    {

        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IHelpers helpers;

        public PriceBookService(ApplicationDbContext dbContext,
            IMapper mapper,
            IHelpers helpers,
            IHttpContextAccessor httpContextAccessor): base(httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.helpers = helpers;
        }

        public async Task<PriceBookViewModel> CreatePriceBook(CreateUpdatePriceBookViewModel model)
        {
            var validation = ValidatePriceBook(model);
            if (validation.Any())
            {
                throw new PricebookValidationException(validation.First().Error.ErrorCode, validation.First().Error.ErrorMessage);
            }
            PriceBook priceBook = mapper.Map<PriceBook>(model);
            priceBook.PriceLists = mapper.Map<ICollection<PriceList>>(model.PriceLists);
            priceBook.Condition = CreateNewCondition(model);
            var result = await dbContext.AddAsync(priceBook);
            await dbContext.SaveChangesAsync();
            await result.Reference(x => x.SubscriptionType).LoadAsync();
            await result.Reference(x => x.VehicleType).LoadAsync();

            return mapper.Map<PriceBookViewModel>(result.Entity);
        }

        private PriceBookCondition CreateNewCondition(CreateUpdatePriceBookViewModel model)
        {
            PriceBookCondition condition = model.Condition.ConditionType switch
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

        public async Task<PriceBookViewModel> UpdatePriceBooks(CreateUpdatePriceBookViewModel model)
        {
            var validation = ValidatePriceBook(model);
            if (validation.Any())
            {
                throw new PricebookValidationException(validation.First().Error.ErrorCode, validation.First().Error.ErrorMessage);
            }
            PriceBook priceBook = await dbContext.PriceBooks
                .Include(x => x.Condition)
                .Include(x => x.PriceLists)
                .Where(x => x.Id == model.Id && x.ClientId == ClientId)
                .FirstOrDefaultAsync();

            mapper.Map(model, priceBook);
            dbContext.Remove(priceBook.Condition);
            priceBook.Condition = CreateNewCondition(model);
            mapper.Map(model.PriceLists, priceBook.PriceLists);
            dbContext.UpdateRange(priceBook.PriceLists);
            var result = dbContext.Update(priceBook);

            await dbContext.SaveChangesAsync();
            await result.Reference(x => x.SubscriptionType).LoadAsync();
            await result.Reference(x => x.VehicleType).LoadAsync();

            return mapper.Map<PriceBookViewModel>(result.Entity);
        }

        public async Task<PriceBookViewModel> GetPriceBookById(Guid id)
        {
            var query = dbContext.PriceBooks
                .Include(x => x.VehicleType)
                .Include(x => x.SubscriptionType)
                .Include(x => x.Condition)
                .Include(x => x.PriceLists)
                .Where(x => x.ClientId == ClientId && x.Id == id);
            var result = await query.FirstOrDefaultAsync();
            var viewModel = mapper.Map<PriceBookViewModel>(result);
            viewModel.Condition = mapper.Map<PriceConditionViewModel>(result.Condition);
            return viewModel;
        }

        public async Task<QueryResultModel<PriceBookViewModel>> GetPriceBooks(PriceListQuery queryParam)
        {
            var query = dbContext.PriceBooks
                .Include(x => x.Condition)
                .Where(x => x.ClientId == ClientId &&
                x.VehicleTypeId == queryParam.VehicleTypeId &&
                x.SubscriptionTypeId == queryParam.SubscriptionTypeId)
                .OrderBy(x => x.Name)
                .PagedBy(queryParam.Page, queryParam.PageSize);
            var priceBooks = await query.ToListAsync();
            var data = mapper.Map<IEnumerable<PriceBookViewModel>>(priceBooks);
            var total = await query.CountAsync();
            return new QueryResultModel<PriceBookViewModel>(data)
            {
                Page = queryParam.Page,
                TotalCount = total
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
                {PriceFormular.Monthly, "Tháng" },
                {PriceFormular.ByTime, "Giờ" },
                {PriceFormular.ByTurn, "Lượt" },
                {PriceFormular.ByWeek, "Tuần" },
                {PriceFormular.Quarterly, "Quý" },
                {PriceFormular.Annual, "Năm" },
            };
            return values.Select(x => new FormularTypeEnumViewModel
            {
                Unit = UnitMap[x],
                Value = (int)x,
                Description = helpers.GetEnumDescription(x)
            });
        }
        #region private methods
        protected virtual IEnumerable<ValidationViewModel<CreatePriceListViewModel>> ValidatePriceBook(CreateUpdatePriceBookViewModel priceBookModel)
        {
            List<ValidationViewModel<CreatePriceListViewModel>> validations = new();
            var priceList = priceBookModel.PriceLists.OrderBy(x => x.StartTime).ToList();
            if(priceList.Count == 0)
            {
                validations.Add(new ValidationViewModel<CreatePriceListViewModel>()
                {
                    Field = null,
                    Error = new ServiceError()
                    {
                        ErrorCode = "PRICELIST_EMPTY",
                        ErrorMessage = "Bảng giá trống"
                    }
                });
            }
            for (int i = 0; i < priceList.Count; i++)
            {
                var nextIndex = (i + 1) % priceList.Count;
                if (!priceList[i].EndTime.Equals(priceList[nextIndex].StartTime))
                {
                    validations.Add(new ValidationViewModel<CreatePriceListViewModel>()
                    {
                        Field = priceList[i],
                        Error = new ServiceError()
                        {
                            ErrorCode = "PRICE_MISSING_OR_OVELLAP",
                            ErrorMessage = $"Bảng giá bị trùng hoặc thiếu khoảng thời gian {priceList[i].Name} to {priceList[nextIndex].Name}"
                        }
                    });
                }
            }
            return validations;
        }
        #endregion
    }
}
