using AutoMapper;
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
        }
    }
}
