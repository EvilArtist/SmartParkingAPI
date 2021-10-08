using SmartParkingAbstract.ViewModels.DataImport;
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
        Task<IEnumerable<SubscriptionTypeViewModel>> GetSubscriptionTypes();
        Task<SubscriptionTypeViewModel> GetSubscriptionTypeById( Guid id);
        Task<SubscriptionTypeViewModel> CreateSubscriptionType(CreateUpdateSubscriptionTypeViewModel model);
        Task<SubscriptionTypeViewModel> UpdateSubscriptionType(CreateUpdateSubscriptionTypeViewModel model);
        Task<IEnumerable<SubscriptionTypeViewModel>> ImportData(SubscriptionTypeDataImport data);
    }
}
