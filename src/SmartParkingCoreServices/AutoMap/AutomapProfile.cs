using AutoMapper;
using SmartParking.Share.Constants;
using SmartParking.Share.Models;
using SmartParkingAbstract.ViewModels.Customers;
using SmartParkingAbstract.ViewModels.DataImport;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Operation;
using SmartParkingAbstract.ViewModels.Parking;
using SmartParkingAbstract.ViewModels.Parking.PriceBooks;
using SmartParkingCoreModels.Customers;
using SmartParkingCoreModels.Operation;
using SmartParkingCoreModels.Parking;
using SmartParkingCoreModels.Parking.PriceBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreServices.AutoMap
{
    public class AutomapProfile : Profile
    {
        public AutomapProfile()
        {
            CreateMap<ParkingConfig, ParkingViewModel>().ReverseMap();

            CreateMap<SlotType, SlotTypeViewModel>().ReverseMap();

            CreateMap<SerialPortConfiguration, SerialPortConfigViewModel>()
                .ForMember(x => x.Status, y => y.MapFrom(z => z.Status.ToString()))
                .ReverseMap()
                .ForMember(x => x.Status, y => y.MapFrom(z => Enum.Parse<DeviceStatus>(z.Status)));

            CreateMap<CameraConfiguration, CameraConfigurationViewModel>()
               .ForMember(x => x.Status, y => y.MapFrom(z => z.Status.ToString()))
               .ReverseMap()
               .ForMember(x => x.Status, y => y.MapFrom(z => Enum.Parse<DeviceStatus>(z.Status)));

            CreateMap<CameraProtocolType, CameraProtocolTypeViewModel>().ReverseMap();

            CreateMap<SlotTypeConfiguration, SlotTypeConfigViewModel>().ReverseMap();

            CreateMap<UpdateSlotTypeConfigViewModel, SlotTypeConfiguration>().ReverseMap();

            CreateMap<CreateUpdateVehicleTypeViewModel, VehicleType>().ReverseMap();
            CreateMap<VehicleTypeViewModel, VehicleType>()
                .ReverseMap()
                .ForMember(x => x.CardCount, y => y.MapFrom(z => (z.Cards == null) ? 0 : z.Cards.Count));

            CreateMap<SubscriptionTypeViewModel, SubscriptionType>()
                .ReverseMap()
                .ForMember(x => x.CardCount, y => y.MapFrom(z => (z.Cards == null) ? 0 : z.Cards.Count));
            CreateMap<CreateUpdateSubscriptionTypeViewModel, SubscriptionType>().ReverseMap();

            CreateMap<CreateCardViewModel, Card>();
            CreateMap<UpdateCardViewModel, Card>();
            CreateMap<CardViewModel, Card>().ReverseMap();
            CreateMap<CardStatusViewModel, CardStatus>().ReverseMap();

            CreatePriceMapping();

            CreateMap<ParkingLane, ParkingLaneViewModel>().ReverseMap();

            CreateMap<ParkingRecord, ParkingRecordDetailViewModel>()
                .ReverseMap();
            CreateMap<PriceItem, PriceItemViewModel>().ReverseMap();
            CreateMap<ParkingRecordStatus, ParkingRecordStatusViewModel>().ReverseMap();

            CreateMap<CameraDataImport, CameraConfiguration>().ForMember(x => x.Protocol, y => y.Ignore());
            CreateMap<MultigateDataImport, SerialPortConfiguration>();

            CreateMap<SlotTypeDataImport, SlotType>();

            CreateMap<CustomerTypeViewModel, CustomerType>()
                .ReverseMap();
            CreateMap<CustomerViewModel, Customer>()
                .ReverseMap();
            CreateMap<CustomerDataImport, Customer>();
            CreateMap<Subscription, SubscriptionViewModel>()
                 .ForMember(x => x.AssignedCard, y => y.Ignore())
                 .ReverseMap()
                 .ForMember(x => x.AssignedCard, y => y.Ignore());
            CreateMap<Vehicle, VehicleViewModel>()
                 .ForMember(x => x.Subscription, y => y.Ignore())
                 .ReverseMap()
                .ForMember(x => x.Subscription, y => y.Ignore());
        }

        private void CreatePriceMapping()
        {
            CreateMap<PriceBook, PriceBookViewModel>();
            CreateMap<PriceList, PriceListViewModel>()
                .ForMember(x => x.StartTime, y => y.MapFrom(z => ApiTime.FromTimeSpan(z.StartTime)))
                .ForMember(x => x.EndTime, y => y.MapFrom(z => ApiTime.FromTimeSpan(z.EndTime)))
                .ForMember(x => x.Offset, y => y.MapFrom(z => ApiTime.FromTimeSpan(z.Offset)));
            CreateMap<PriceCalculation, PriceCalculationViewModel>()
                .ForMember(x => x.Unit, y => y.MapFrom(z => PriceBookContants.UnitMap[z.FormularType]));
            CreateMap<PriceBookCondition, PriceConditionViewModel>();

            CreateMap<PriceListDefaultCondition, PriceConditionViewModel>()
                .ForMember(x => x.StartDate, y => y.Ignore())
                .ForMember(x => x.EndDate, y => y.Ignore())
                .ForMember(x => x.Days, y => y.Ignore())
                .ForMember(x => x.ConditionType, y => y.MapFrom(z => z.PriceConditionType));

            CreateMap<PriceListHollidayCondition, PriceConditionViewModel>()
                .ForMember(x => x.StartDate, y => y.Ignore())
                .ForMember(x => x.EndDate, y => y.Ignore())
                .ForMember(x => x.Days, y => y.Ignore())
                .ForMember(x => x.ConditionType, y => y.MapFrom(z => z.PriceConditionType));

            CreateMap<PriceListWeekdayCondition, PriceConditionViewModel>()
                .ForMember(x => x.StartDate, y => y.Ignore())
                .ForMember(x => x.EndDate, y => y.Ignore())
                .ForMember(x => x.ConditionType, y => y.MapFrom(z => z.PriceConditionType));

            CreateMap<PriceListDurationCondition, PriceConditionViewModel>()
                .ForMember(x => x.Condition, y => y.MapFrom(z => z.StartDate.ToString("MM/dd/yyyy") + " - " + z.EndDate.ToString("MM/dd/yyyy")))
                .ForMember(x => x.Days, y => y.Ignore())
                .ForMember(x => x.ConditionType, y => y.MapFrom(z => z.PriceConditionType));

            CreateMap<CreateUpdatePriceBookViewModel, PriceBook>()
                .ForMember(x => x.Condition, y => y.Ignore())
                .ForMember(x => x.PriceLists, y => y.Ignore());

            CreateMap<CreatePriceListViewModel, PriceList>()
                .ForMember(x => x.StartTime, y => y.MapFrom(z => z.StartTime.ToTimeSpan()))
                .ForMember(x => x.EndTime, y => y.MapFrom(z => z.EndTime.ToTimeSpan()))
                .ForMember(x => x.Offset, y => y.MapFrom(z => z.Offset.ToTimeSpan()));

            CreateMap<CreatePriceCalcutationViewModel, PriceCalculation>();

            CreateMap<CreatePriceConditionViewModel, PriceListDefaultCondition>()
                .ForMember(x => x.PriceConditionType, y => y.MapFrom(z => z.ConditionType))
                .ForMember(x => x.Id, y => y.Ignore());

            CreateMap<CreatePriceConditionViewModel, PriceListWeekdayCondition>()
                .ForMember(x => x.PriceConditionType, y => y.MapFrom(z => z.ConditionType))
                .ForMember(x => x.Id, y => y.Ignore());

            CreateMap<CreatePriceConditionViewModel, PriceListHollidayCondition>()
                .ForMember(x => x.PriceConditionType, y => y.MapFrom(z => z.ConditionType))
                .ForMember(x => x.Id, y => y.Ignore());

            CreateMap<CreatePriceConditionViewModel, PriceListDurationCondition>()
                .ForMember(x => x.PriceConditionType, y => y.MapFrom(z => z.ConditionType))
                .ForMember(x => x.Id, y => y.Ignore());
        }
    }
}
