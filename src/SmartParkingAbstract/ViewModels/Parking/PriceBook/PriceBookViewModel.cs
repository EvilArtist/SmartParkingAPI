using SmartParkingAbstract.ViewModels.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.Parking.PriceBook
{
    public class PriceBookViewModel : MultiTanentModel
    {
        public string Name { get; set; }
        public PriceConditionViewModel Condition { get; set; }
        public PriceCalculationViewModel Calculation { get; set; }
        public VehicleTypeViewModel VehicleType { get; set; }
        public SubscriptionTypeViewModel SubscriptionType { get; set; }

    }

    public class CreateUpdatePriceViewModel : MultiTanentModel
    {
        public string Name { get; set; }
        public CreatePriceConditionViewModel Condition { get; set; }
        public CreatePriceCalcutationViewModel Calculation { get; set; }
        public Guid VehicleTypeId { get; set; }
        public Guid SubscriptionTypeId { get; set; }
    }
}
