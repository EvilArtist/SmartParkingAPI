using SmartParkingAbstract.ViewModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.Services.Parking
{
    public interface ISlotTypeConfigurationService
    {
        Task<IEnumerable<SlotTypeConfigViewModel>> GetSlotTypeConfigs(string clientId, Guid parkingId);
        Task<IEnumerable<SlotTypeConfigViewModel>> CreateOrUpdateSlotTypeConfigs(UpdateSlotTypeConfigsViewModel model);
    }
}
