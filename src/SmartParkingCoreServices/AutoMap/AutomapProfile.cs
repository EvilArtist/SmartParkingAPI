using AutoMapper;
using SmartParking.Share.Constants;
using SmartParkingAbstract.ViewModels.Parking.CameraConfiguration;
using SmartParkingAbstract.ViewModels.Parking.SerialPortConfiguration;
using SmartParkingAbstract.ViewModels.Parking.SlotType;
using SmartParkingCoreModels.Parking;
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
            CreateMap<SlotType, SlotTypeViewModel>()
                .ReverseMap();
            CreateMap<SerialPortConfiguration, SerialPortConfigViewModel>()
                .ForMember(x => x.Status, y => y.MapFrom(z => z.Status.ToString()))
                .ReverseMap()
                .ForMember(x => x.Status, y => y.MapFrom(z => Enum.Parse<DeviceStatus>(z.Status)));
            CreateMap<CameraConfiguration, CameraConfigurationViewModel>()
               .ForMember(x => x.Status, y => y.MapFrom(z => z.Status.ToString()))
               .ReverseMap()
               .ForMember(x => x.Status, y => y.MapFrom(z => Enum.Parse<DeviceStatus>(z.Status)));

            CreateMap<CameraProtocolType, CameraProtocolTypeViewModel>();
        }
    }
}
