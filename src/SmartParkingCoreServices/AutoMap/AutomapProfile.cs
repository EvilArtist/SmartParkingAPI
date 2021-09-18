using AutoMapper;
using SmartParking.Share.Constants;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Operation;
using SmartParkingAbstract.ViewModels.Parking;
using SmartParkingAbstract.ViewModels.Parking.PriceBook;
using SmartParkingCoreModels.Operation;
using SmartParkingCoreModels.Parking;
using SmartParkingCoreModels.Parking.PriceBook;
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
                .ForMember(x => x.CardCount, y=> y.MapFrom(z=> (z.Cards == null) ? 0 : z.Cards.Count));

            CreateMap<SubscriptionTypeViewModel, SubscriptionType>()
                .ReverseMap()
                .ForMember(x => x.CardCount, y => y.MapFrom(z => (z.Cards == null) ? 0 : z.Cards.Count));
            CreateMap<CreateUpdateSubscriptionTypeViewModel, SubscriptionType>().ReverseMap();

            CreateMap<CreateCardViewModel, Card>();
            CreateMap<UpdateCardViewModel, Card>();
            CreateMap<CardViewModel, Card>()
                .ReverseMap()
                .ForMember(x => x.SubscriptionTypeName, y => y.MapFrom(z => z.SubscriptionType.Name))
                .ForMember(x => x.VehicleTypeName, y => y.MapFrom(z => z.VehicleType.Name));

            CreateMap<CreatePriceConditionViewModel, PriceListDefaultCondition>()
                .ForMember(x => x.PriceConditionType, y => y.MapFrom(z => z.ConditionType))
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.StartTime, y => y.MapFrom(z => z.StartTime.ToTimeSpan()))
                .ForMember(x => x.EndTime, y => y.MapFrom(z => z.EndTime.ToTimeSpan()));

            CreateMap<CreatePriceConditionViewModel, PriceListWeekdayCondition>()
                .ForMember(x => x.PriceConditionType, y => y.MapFrom(z => z.ConditionType))
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.StartTime, y => y.MapFrom(z => z.StartTime.ToTimeSpan()))
                .ForMember(x => x.EndTime, y => y.MapFrom(z => z.EndTime.ToTimeSpan()));

            CreateMap<CreatePriceConditionViewModel, PriceListHollidayCondition>()
                .ForMember(x => x.PriceConditionType, y => y.MapFrom(z => z.ConditionType))
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.StartTime, y => y.MapFrom(z => z.StartTime.ToTimeSpan()))
                .ForMember(x => x.EndTime, y => y.MapFrom(z => z.EndTime.ToTimeSpan()));

            CreateMap<CreatePriceConditionViewModel, PriceListDurationCondition>()
                .ForMember(x => x.PriceConditionType, y => y.MapFrom(z => z.ConditionType))
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.StartTime, y => y.MapFrom(z => z.StartTime.ToTimeSpan()))
                .ForMember(x => x.EndTime, y => y.MapFrom(z => z.EndTime.ToTimeSpan()));

            CreateMap<CreatePriceCalcutationViewModel, PriceCalculation>()
                .ForMember(x => x.Type, y => y.MapFrom(z => z.FormularType));

            CreateMap<CreateUpdatePriceViewModel, PriceList>()
                .ForMember(x => x.Condition, y => y.Ignore());

            CreateMap<PriceListCondition, PriceConditionViewModel>()
                .ForMember(x => x.StartTime, y => y.MapFrom(z => ApiTime.FromTimeSpan(z.StartTime)))
                .ForMember(x => x.EndTime, y => y.MapFrom(z => ApiTime.FromTimeSpan(z.EndTime)));
            CreateMap<PriceListDefaultCondition, PriceConditionViewModel>()
                .ForMember(x => x.StartDate, y => y.Ignore())
                .ForMember(x => x.EndDate, y => y.Ignore())
                .ForMember(x => x.Days, y => y.Ignore())
                .ForMember(x => x.ConditionType, y => y.MapFrom(z => z.PriceConditionType))
                .ForMember(x => x.StartTime, y => y.MapFrom(z => ApiTime.FromTimeSpan(z.StartTime)))
                .ForMember(x => x.EndTime, y => y.MapFrom(z => ApiTime.FromTimeSpan(z.EndTime)));

            CreateMap<PriceListHollidayCondition, PriceConditionViewModel>()
                .ForMember(x => x.StartDate, y => y.Ignore())
                .ForMember(x => x.EndDate, y => y.Ignore())
                .ForMember(x => x.Days, y => y.Ignore())
                .ForMember(x => x.ConditionType, y => y.MapFrom(z => z.PriceConditionType))
                .ForMember(x => x.StartTime, y => y.MapFrom(z => ApiTime.FromTimeSpan(z.StartTime)))
                .ForMember(x => x.EndTime, y => y.MapFrom(z => ApiTime.FromTimeSpan(z.EndTime)));

            CreateMap<PriceListWeekdayCondition, PriceConditionViewModel>()
                .ForMember(x => x.StartDate, y => y.Ignore())
                .ForMember(x => x.EndDate, y => y.Ignore())
                .ForMember(x => x.ConditionType, y => y.MapFrom(z => z.PriceConditionType))
                .ForMember(x => x.StartTime, y => y.MapFrom(z => ApiTime.FromTimeSpan(z.StartTime)))
                .ForMember(x => x.EndTime, y => y.MapFrom(z => ApiTime.FromTimeSpan(z.EndTime)));


            CreateMap<PriceListDurationCondition, PriceConditionViewModel>()
                .ForMember(x => x.Condition, y => y.MapFrom(z => z.StartDate.ToString("MM/dd/yyyy") + " - " + z.EndDate.ToString("MM/dd/yyyy")))
                .ForMember(x => x.Days, y => y.Ignore())
                .ForMember(x => x.ConditionType, y => y.MapFrom(z => z.PriceConditionType))
                .ForMember(x => x.StartTime, y => y.MapFrom(z => ApiTime.FromTimeSpan(z.StartTime)))
                .ForMember(x => x.EndTime, y => y.MapFrom(z => ApiTime.FromTimeSpan(z.EndTime)));

            CreateMap<PriceCalculation, PriceCalculationViewModel>()
                .ForMember(x => x.FormularType, z => z.MapFrom(y => y.Type))
                .ForMember(x => x.Unit, y=>y.MapFrom(z => PriceBookContants.UnitMap[z.Type]));

            CreateMap<PriceList, PriceBookViewModel>();

            CreateMap<ParkingRecord, ParkingRecordDetailViewModel>()
                .ReverseMap();
            CreateMap<PriceItem, PriceItemViewModel>().ReverseMap();
            CreateMap<ParkingRecordStatus, ParkingRecordStatusViewModel>().ReverseMap();
        }
    }
}
