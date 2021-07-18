using SmartParkingAbstract.ViewModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.Services.Parking
{
    public interface ISubscriptionTypeService
    {
        Task<IEnumerable<SubscriptionTypeViewModel>> GetSubscriptionTypes(string clientId);
        Task<SubscriptionTypeViewModel> GetSubscriptionTypeById(string clientId, Guid id);
        Task<SubscriptionTypeViewModel> CreateSubscriptionType(CreateUpdateSubscriptionTypeViewModel model);
        Task<SubscriptionTypeViewModel> UpdateSubscriptionType(CreateUpdateSubscriptionTypeViewModel model);
    }
}
