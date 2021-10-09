using SmartParkingAbstract.ViewModels.Customers;
using SmartParkingAbstract.ViewModels.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.Parking
{
    public class CardViewModel : MultiTanentModel
    {
        public string Name { get; set; }
        public string IdentityCode { get; set; }
        public Guid VehicleTypeId { get; set; }
        public VehicleTypeViewModel VehicleType { get; set; }
        public string StatusCode { get; set; }
        public CardStatusViewModel Status { get; set; }
        public Guid? SubscriptionId { get; set; }
        public SubscriptionViewModel Subscription { get; set; }

        public CardViewModel() : base()
        {

        }
    }

    public class CreateCardViewModel : MultiTanentModel
    {
        public string IdentityCode { get; set; }
        public string Name { get; set; }
        public Guid VehicleTypeId { get; set; }
    }

    public class UpdateCardViewModel : MultiTanentModel
    {
        public string Name { get; set; }
        public Guid VehicleTypeId { get; set; }
    }

    public class CardStatusViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

}
