using SmartParkingAbstract.ViewModels.General;
using System;

namespace SmartParkingAbstract.ViewModels.Parking.PriceBooks
{
    public class PriceListViewModel
    {
        public string Name { get; set; }
        public ApiTime StartTime { get; set; }
        public ApiTime EndTime { get; set; }
        public ApiTime Offset { get; set; }
        public PriceCalculationViewModel Calculation { get; set; }
    }

    public class CreatePriceListViewModel
    {
        public string Name { get; set; }
        public ApiTime StartTime { get; set; }
        public ApiTime EndTime { get; set; }
        public ApiTime Offset { get; set; }
        public CreatePriceCalcutationViewModel Calculation { get; set; }
    }
}