using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartParking.Share.Constants;
using SmartParking.Share.Exceptions;
using SmartParkingAbstract.Services.Operation;
using SmartParkingAbstract.ViewModels.Operation;
using SmartParkingAbstract.ViewModels.Parking.PriceBook;
using SmartParkingCoreModels.Data;
using SmartParkingCoreModels.Operation;
using SmartParkingCoreServices.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartParking.Share.Constants.IdentityConstants;

namespace SmartParkingCoreServices.Operation
{
    public class OperationService : MultitanentService, IOperationService
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;
        private readonly IPriceCalculationService priceCalculationService;

        public OperationService(IMapper mapper, 
            ApplicationDbContext dbContext, 
            IHttpContextAccessor httpContextAccessor,
            IPriceCalculationService priceCalculationService) : base(httpContextAccessor)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.priceCalculationService = priceCalculationService;
        }

        public async Task<ParkingRecordDetailViewModel> CheckIn(CheckInParkingRecord checkInParkingRecord)
        {
            var clientId = GetClientId();
            var userId = GetCurrentUserId();
            var card = await dbContext.Cards
                .Include(x=>x.Status)
               .Where(x => x.IdentityCode == checkInParkingRecord.CardCode)
               .FirstOrDefaultAsync();
            if (card == null)
            {
                throw new CardNotFoundException("Thẻ Không hợp lệ");
            }
            var isCardAssignedToParking = await dbContext.CardParkingAssignments
                .AnyAsync(x => x.CardId == card.Id && x.ParkingId == checkInParkingRecord.ParkingId);
            if (!isCardAssignedToParking)
            {
                throw new CardNotAssignedException("Thẻ chưa được gán với bãi xe");
            }
            if (card.Status.Code == CardStatusCode.Parking)
            {
                throw new CardNotAssignedException("Thẻ đang trong bãi xe");
            }
            else if (card.Status.Code == CardStatusCode.Lock)
            {
                throw new CardNotAssignedException("Thẻ đã bị khoá");
            }

            ParkingRecord record = new()
            {
                CardId = card.Id,
                CheckinTime = DateTime.Now,
                CheckinEmployeeId = userId,
                CheckinParkingLaneId = checkInParkingRecord.ParkingLaneId,
                ClientId = clientId,
                VehicleTypeId = card.VehicleTypeId,
                SubscriptionTypeId = card.SubscriptionTypeId,
                ParkingId = checkInParkingRecord.ParkingId,
                StatusCode = ParkingRecordStatusConstants.Checkin
            };
            var result = await dbContext.ParkingRecords.AddAsync(record);
                         
            await dbContext.SaveChangesAsync();
            return mapper.Map<ParkingRecord, ParkingRecordDetailViewModel>(result.Entity);
        }

        public async Task<ParkingRecordDetailViewModel> CheckOut(CheckOutParkingRecord checkoutParkingRecord)
        {
            var clientId = GetClientId();
            var userId = GetCurrentUserId();
            ParkingRecord record = await dbContext.ParkingRecords
                .Where(x => x.ClientId == clientId &&
                    x.Card.IdentityCode == checkoutParkingRecord.CardCode &&
                    x.StatusCode == ParkingRecordStatusConstants.Parking)
                .FirstOrDefaultAsync();
            if (record == null)
            {
                throw new RecordNotFoundException("Không tìm thấy thông tin xe vào tương ứng");
            }

            record.CheckoutEmployeeId = userId;
            record.CheckoutParkingLaneId = checkoutParkingRecord.ParkingLaneId;
            record.CheckoutTime = DateTime.Now;
            var priceCalculationParam = new PriceCalculationParam()
            {
                CheckinTime = record.CheckinTime,
                CheckoutTime = DateTime.Now,
                VehicleTypeId = record.VehicleTypeId,
                SubscriptionTypeId = record.SubscriptionTypeId,
            };
            var priceItems = await priceCalculationService.Calculate(priceCalculationParam);
            foreach (var item in priceItems)
            {
                PriceItem priceItem = mapper.Map<PriceItem>(item);
                priceItem.ParkingRecordId = record.Id;
                await dbContext.AddAsync(priceItem);
            }
            record.TotalAmount = priceCalculationService.GetTotal(priceItems);
            record.StatusCode = ParkingRecordStatusConstants.Checkout;
            var result = dbContext.Update(record);
            await dbContext.SaveChangesAsync();
            return mapper.Map<ParkingRecord, ParkingRecordDetailViewModel>(result.Entity);
        }

        public async Task<ParkingRecordDetailViewModel> AllowVehicleIn(Guid recordId)
        {
            var clientId = GetClientId();
            ParkingRecord record = await dbContext.ParkingRecords
                .Where(x => x.ClientId == clientId &&
                    x.Id == recordId )
                .FirstOrDefaultAsync();
            if (record == null)
            {
                throw new RecordNotFoundException("Không tìm thấy thông tin xe vào tương ứng");
            }
            if (record.StatusCode != ParkingRecordStatusConstants.Checkin)
            {
                throw new InvalidStatusRecordException();
            }
            record.StatusCode = ParkingRecordStatusConstants.Parking;
            await dbContext.Entry(record).Reference(x => x.Card).LoadAsync();
            await dbContext.Entry(record).Reference(x => x.CheckinParkingLane).LoadAsync();
            var card = record.Card;
            var cardStatus = await dbContext.CardStatuses.Where(x => x.Code == CardStatusCode.Parking)
                    .FirstOrDefaultAsync();

            card.CardStatusId = cardStatus.Id;
            dbContext.Update(card);
            dbContext.Update(record);
            await dbContext.SaveChangesAsync();
            return mapper.Map<ParkingRecordDetailViewModel>(record);
        }

        public async Task<ParkingRecordDetailViewModel> AllowVehicleOut(Guid recordId)
        {
            var clientId = GetClientId();
            ParkingRecord record = await dbContext.ParkingRecords
                .Where(x => x.ClientId == clientId &&
                    x.Id == recordId)
                .FirstOrDefaultAsync();
            if (record == null)
            {
                throw new RecordNotFoundException("Không tìm thấy thông tin xe ra tương ứng");
            }
            if (record.StatusCode != ParkingRecordStatusConstants.Checkin)
            {
                throw new InvalidStatusRecordException();
            }
            record.StatusCode = ParkingRecordStatusConstants.Complete;
            await dbContext.Entry(record).Reference(x => x.Card).LoadAsync();
            await dbContext.Entry(record).Reference(x => x.CheckoutParkingLane).LoadAsync();
            var card = record.Card;
            var cardStatus = await dbContext.CardStatuses.Where(x => x.Code == CardStatusCode.Checkout)
                    .FirstOrDefaultAsync();

            card.CardStatusId = cardStatus.Id;
            dbContext.Update(card);
            dbContext.Update(record);
            await dbContext.SaveChangesAsync();
            return mapper.Map<ParkingRecordDetailViewModel>(record);
        }

        public async Task<ParkingRecordDetailViewModel> UpdateCheckinRecordInfo(UpdateRecordInfoViewModel recordInfo)
        {
            var clientId = GetClientId();
            ParkingRecord record = await dbContext.ParkingRecords
                .Where(x => x.ClientId == clientId &&
                    x.Id == recordInfo.RecordId)
                .FirstOrDefaultAsync();
            if (record == null)
            {
                throw new RecordNotFoundException("Không tìm thấy thông tin xe vào tương ứng");
            }
            if (record.StatusCode != ParkingRecordStatusConstants.Checkin && record.StatusCode != ParkingRecordStatusConstants.Parking)
            {
                throw new InvalidStatusRecordException();
            }
            record.CheckinPlateNumber = recordInfo.LicensePlate;
            record.URLCheckinFrontImage = "";
            record.URLCheckinBackImage = "";
            dbContext.Update(record);
            await dbContext.SaveChangesAsync();
            return mapper.Map<ParkingRecordDetailViewModel>(record);
        }

        public async Task<ParkingRecordDetailViewModel> UpdateCheckoutRecordInfo(UpdateRecordInfoViewModel recordInfo)
        {
            var clientId = GetClientId();
            ParkingRecord record = await dbContext.ParkingRecords
                .Where(x => x.ClientId == clientId &&
                    x.Id == recordInfo.RecordId)
                .FirstOrDefaultAsync();
            if (record == null)
            {
                throw new RecordNotFoundException("Không tìm thấy thông tin xe vào tương ứng");
            }
            if (record.StatusCode != ParkingRecordStatusConstants.Checkin && record.StatusCode != ParkingRecordStatusConstants.Parking)
            {
                throw new InvalidStatusRecordException();
            }
            record.CheckinPlateNumber = recordInfo.LicensePlate;
            record.URLCheckinFrontImage = "";
            record.URLCheckinBackImage = "";
            dbContext.Update(record);
            await dbContext.SaveChangesAsync();
            return mapper.Map<ParkingRecordDetailViewModel>(record);
        }
    }
}
