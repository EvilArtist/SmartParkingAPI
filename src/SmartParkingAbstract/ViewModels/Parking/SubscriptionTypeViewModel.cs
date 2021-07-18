using SmartParkingAbstract.ViewModels.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.Parking
{
    public class SubscriptionTypeViewModel : MultiTanentModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CardCount { get; set; }
        public SubscriptionTypeViewModel()
        {

        }
        public SubscriptionTypeViewModel(string clientId): base(clientId)
        {

        }
    }

    public class CreateUpdateSubscriptionTypeViewModel : MultiTanentModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
