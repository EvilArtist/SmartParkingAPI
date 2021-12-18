using SmartParkingAbstract.ViewModels.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.Parking.PriceBooks
{
    public class PriceBookViewModel : MultiTanentModel
    {
        public string Name { get; set; }
        public PriceConditionViewModel Condition { get; set; }
        public VehicleTypeViewModel VehicleType { get; set; }
        public SubscriptionTypeViewModel SubscriptionType { get; set; }
        public List<PriceListViewModel> PriceLists { get; set; }
        public bool Active { get; set; }
    }

    public class CreateUpdatePriceBookViewModel : MultiTanentModel
    {
        public string Name { get; set; }
        public CreatePriceConditionViewModel Condition { get; set; }
        public List<CreatePriceListViewModel> PriceLists { get; set; }
        public Guid VehicleTypeId { get; set; }
        public Guid SubscriptionTypeId { get; set; }
        public bool Active { get; set; }
    }
}
