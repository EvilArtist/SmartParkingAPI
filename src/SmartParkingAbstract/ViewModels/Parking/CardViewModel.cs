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
        public string SubscriptionTypeName { get; set; }
        public string VehicleTypeName { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public string LicencePalate { get; set; }
        public Guid SubscriptionId { get; set; }
        public Guid SubscriptionTypeId { get; set; }
        public Guid VehicleTypeId { get; set; }

        public CardViewModel(string clientId) : base(clientId)
        {

        }
        public CardViewModel() : base()
        {

        }
    }

    public class CreateCardViewModel : MultiTanentModel
    {
        public string IdentityCode { get; set; }
        public string Name { get; set; }
        public Guid SubscriptionTypeId { get; set; }
        public Guid VehicleTypeId { get; set; }
    }

    public class UpdateCardViewModel : MultiTanentModel
    {
        public string Name { get; set; }
        public Guid SubscriptionTypeId { get; set; }
        public Guid VehicleTypeId { get; set; }
    }
}
