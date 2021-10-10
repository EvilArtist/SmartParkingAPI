using SmartParkingAbstract.ViewModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.Customers
{
    public class CustomerViewModel
    {
        public Guid Id { get; set; }
        public string CustomerCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomerTypeCode { get; set; }
        public CustomerTypeViewModel CustomerType { get; set; }
        public virtual ICollection<SubscriptionViewModel> Subscriptions { get; set; }
    }

    public class CustomerTypeViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class SubscriptionViewModel
    {
        public DateTime? LastExtendDate { get; set; }
        public DateTime? NextExtendDate { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public decimal Amount { get; set; }
        public Guid? VehicleId { get; set; }
        public VehicleViewModel Vehicle { get; set; }
        public Guid CardId { get; set; }
        public CardViewModel AssignedCard { get; set; }
        public Guid CustomerId { get; set; }
        public CustomerViewModel Customer { get; set; }
        public Guid SubscriptionTypeId { get; set; }
        public SubscriptionTypeViewModel SubscriptionType { get; set; }
    }
}
