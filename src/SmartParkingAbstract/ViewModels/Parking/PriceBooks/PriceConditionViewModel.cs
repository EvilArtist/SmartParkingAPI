using SmartParking.Share.Constants;
using SmartParkingAbstract.ViewModels.General;
using System;

namespace SmartParkingAbstract.ViewModels.Parking.PriceBooks
{
    public class PriceConditionViewModel: MultiTanentModel
    {
        public PriceCondition ConditionType { get; set; }
        public string Name { get; set; }
        public string Condition { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DayOfWeek[] Days { get; set; }
    }

    public class CreatePriceConditionViewModel: MultiTanentModel
    {
        public PriceCondition ConditionType { get; set; }
        public string Name { get; set; }
        public string Condition { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DayOfWeek[] Days { get; set; }
    }
}
