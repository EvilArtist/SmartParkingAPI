using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartParking.Share.Constants;
using SmartParkingAbstract.Services.Operation;
using SmartParkingAbstract.ViewModels.Operation;
using SmartParkingAbstract.ViewModels.Parking.PriceBooks;
using SmartParkingCoreModels.Data;
using SmartParkingCoreModels.Parking.PriceBooks;
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

        public PriceCalculationService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #region Calculation
        public async Task<IEnumerable<PriceItemViewModel>> Calculate(PriceCalculationParam param)
        {
            var clientId = param.ClientId;
            var query = dbContext.PriceBooks
                .Include(x => x.Condition)
                .Include(x => x.PriceLists)
                .Where(x => x.ClientId == clientId &&
                    x.Active &&
                    x.VehicleTypeId == param.VehicleTypeId &&
                    x.SubscriptionTypeId == param.SubscriptionTypeId &&
                    x.PriceLists.Count > 0
                ).OrderBy(x=>x.Condition.PriceConditionType);
            var priceBooks = await query.ToListAsync();
            var startTime = param.CheckinTime;
            var endTime = param.CheckoutTime;
            List<PriceCondition> orderOfConditionType = new(){ PriceCondition.Holliday, PriceCondition.Duration, PriceCondition.Weekday, PriceCondition.Default};
            List<PriceItemViewModel> priceItems = new();
            while (startTime < endTime)
            {
                var applicablePriceBook = priceBooks
                    .Where(price => MapConditionType(price, startTime))
                    .OrderBy(price => orderOfConditionType.IndexOf(price.Condition.PriceConditionType))
                    .ThenBy(price => price.UpdateTime)
                    .FirstOrDefault();
                
                if (applicablePriceBook == null)
                {
                    PriceItemViewModel applicablePrice = GetDefaultPrice(startTime, endTime);
                    priceItems.Add(applicablePrice);
                    startTime = endTime > applicablePrice.EndTime ? applicablePrice.EndTime: endTime;
                }
                else
                {
                    foreach (var priceList in applicablePriceBook.PriceLists.OrderBy(x=>x.StartTime))
                    {
                        if(IsMapTimeCondition(priceList, startTime))
                        {
                            var applicableEndTime = priceList.Overnight ? startTime.Date.AddDays(1) + priceList.EndTime : startTime.Date + priceList.EndTime;

                            PriceItemViewModel applicablePrice = new()
                            {
                                StartTime = startTime,
                                EndTime = endTime > applicableEndTime ? applicableEndTime : endTime,
                                HourBlock = priceList.Calculation.HourBlock,
                                Name = "DEFAULT",
                                Price = priceList.Calculation.PayPrice,
                                Type = priceList.Calculation.FormularType
                            };
                            priceItems.Add(applicablePrice);
                            startTime = applicablePrice.EndTime;
                            if(startTime >= endTime)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            return priceItems;
        }

        private static PriceItemViewModel GetDefaultPrice(DateTime startTime, DateTime endTime) => new()
        {
            StartTime = startTime,
            EndTime = endTime > startTime.Date.AddDays(1) ? startTime.Date.AddDays(1) : endTime,
            HourBlock = 24,
            Name = "DEFAULT",
            Price = 0,
            Type = PriceFormular.ByDate
        };

        private static bool MapConditionType(PriceBook priceBooks, DateTime startTime)
        {
            if (priceBooks.Condition is PriceListWeekdayCondition weekdayCondition)
            {
                return weekdayCondition.Days.Any(z => z == startTime.DayOfWeek);
            }
            else if (priceBooks.Condition is PriceListHollidayCondition)
            {
                return true;
            }
            else if (priceBooks.Condition is PriceListDurationCondition priceListDurationCondition)
            {
                return priceListDurationCondition.StartDate.Date <= startTime.Date && startTime.Date <= priceListDurationCondition.EndDate.Date;
            }
            return true;
        }

        private static bool IsMapTimeCondition(PriceList priceCondition, DateTime startTime)
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
