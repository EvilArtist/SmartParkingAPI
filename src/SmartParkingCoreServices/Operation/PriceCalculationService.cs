using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartParking.Share.Constants;
using SmartParkingAbstract.Services.Operation;
using SmartParkingAbstract.ViewModels.Operation;
using SmartParkingAbstract.ViewModels.Parking.PriceBook;
using SmartParkingCoreModels.Data;
using SmartParkingCoreModels.Parking.PriceBook;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartParking.Share.Constants.IdentityConstants;

namespace SmartParkingCoreServices.Operation
{
    public class PriceCalculationService : IPriceCalculationService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public PriceCalculationService(
            ApplicationDbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        #region Calculation
        public async Task<IEnumerable<PriceItemViewModel>> Calculate(PriceCalculationParam param)
        {
            var clientId = "ESPACE";//httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ClientId).Value;
            var query = dbContext.PriceLists
                .Include(x => x.Condition)
                .Include(x => x.Calculation)
                .Where(x => x.ClientId == clientId &&
                    x.VehicleTypeId == param.VehicleTypeId &&
                    x.SubscriptionTypeId == param.SubscriptionTypeId
                ).OrderBy(x=>x.Condition.StartTime);
            var priceList = await query.ToListAsync();
            var startTime = param.CheckinTime;
            var endTime = param.CheckoutTime;
            List<PriceCondition> orderOfConditionType =new(){ PriceCondition.Holliday, PriceCondition.Duration, PriceCondition.Weekday, PriceCondition.Default};
            List<PriceItemViewModel> priceItems = new();
            while (startTime < endTime)
            {
                var applicablePrice = priceList
                    .Where(price => IsMapCondition(price.Condition, startTime))
                    .Where(price => MapConditionType(price, startTime))
                    .OrderBy(price => orderOfConditionType.IndexOf(price.Condition.PriceConditionType))
                    .ThenBy(price => price.Condition.StartTime - price.Condition.EndTime)
                    .ThenBy(price => price.Condition.StartTime)
                    .ThenBy(price => price.Condition.EndTime)
                    .Select(price => {
                        var endTimeByCondition = startTime.Date + price.Condition.EndTime;
                        if (price.Condition.Overnight || price.Condition.FullDay)
                        {
                            endTimeByCondition = endTimeByCondition.AddDays(1);
                        }
                        var priceItemEndTime = endTime < endTimeByCondition ? endTime : endTimeByCondition;
                        return new PriceItemViewModel() {
                            StartTime = startTime,
                            EndTime = priceItemEndTime,
                            HourBlock = price.Calculation.HourBlock,
                            Name = price.Name,
                            Price = price.Calculation.Price,
                            Type = price.Calculation.Type
                        };
                    }).FirstOrDefault();
                if (applicablePrice == null)
                {
                    var nextApplicablePrice = priceList
                    .Where(price => MapConditionType(price, startTime))
                    .OrderBy(price => orderOfConditionType.IndexOf(price.Condition.PriceConditionType))
                    .ThenBy(price => price.Condition.StartTime - startTime.TimeOfDay)
                    .ThenBy(price => price.Condition.EndTime)
                    .Select(price => {
                        var endTimeByCondition = startTime.Date + price.Condition.EndTime;
                        if (price.Condition.Overnight || price.Condition.FullDay)
                        {
                            endTimeByCondition.AddDays(1);
                        }
                        var priceItemEndTime = endTime < endTimeByCondition ? endTime : endTimeByCondition;
                        return new PriceItemViewModel()
                        {
                            StartTime = startTime,
                            EndTime = priceItemEndTime,
                            HourBlock = price.Calculation.HourBlock,
                            Name = price.Name,
                            Price = price.Calculation.Price,
                            Type = price.Calculation.Type
                        };
                    }).FirstOrDefault();
                    applicablePrice = new PriceItemViewModel()
                    {
                        StartTime = startTime,
                        EndTime = nextApplicablePrice?.StartTime ?? startTime.Date.AddDays(1),
                        HourBlock = 24,
                        Name = "DEFAULT",
                        Price = nextApplicablePrice?.Price ?? 0,
                        Type = PriceFormular.ByDate
                    };
                }
                priceItems.Add(applicablePrice);
                startTime = applicablePrice.EndTime;
            }
            return priceItems;
        }

        private static bool MapConditionType(PriceList price, DateTime startTime)
        {
            if (price.Condition is PriceListWeekdayCondition weekdayCondition)
            {
                return weekdayCondition.Days.Any(z => z == startTime.DayOfWeek);
            }
            else if (price.Condition is PriceListHollidayCondition)
            {
                return true;
            }
            else if (price.Condition is PriceListDurationCondition priceListDurationCondition)
            {
                return priceListDurationCondition.StartDate.Date <= startTime.Date && startTime.Date <= priceListDurationCondition.EndDate.Date;
            }
            return true;
        }

        private static bool IsMapCondition(PriceListCondition priceCondition, DateTime startTime)
        {
            if (priceCondition.FullDay)
            {
                return true;
            }
            if (priceCondition.Overnight)
            {
                return priceCondition.StartTime >= startTime.TimeOfDay || priceCondition.EndTime <= startTime.TimeOfDay;
            }
            else
            {
                return priceCondition.StartTime <= startTime.TimeOfDay && priceCondition.EndTime > startTime.TimeOfDay;
            }
        }
        #endregion
        public decimal GetTotal(IEnumerable<PriceItemViewModel> priceItems)
        {
            return priceItems.Sum(x => GetPrice(x));
        }

        private static decimal GetPrice(PriceItemViewModel item)
        {
            decimal price;
            if (item.Type == PriceFormular.ByTime)
            {
                var blockTime = item.HourBlock;
                var block = (item.EndTime - item.StartTime).TotalHours / blockTime;
                price = (decimal)(block * item.Price);
            }
            else
            {
                price = (decimal)item.Price;
            }
            return price;
        }
    }
}
