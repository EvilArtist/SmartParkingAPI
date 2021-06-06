using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking.SlotType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.Services.Parking
{
    public interface ISlotTypeService
    {
        Task<SlotTypeViewModel> CreateSlotTypeAsync(SlotTypeViewModel model);
        Task<IEnumerable<SlotTypeViewModel>> SearchSlotTypesAsync(string clientId);
        Task<SlotTypeViewModel> GetSlotTypeByIdAsync(Guid id, string clientId);
        Task<bool> DeleteSlotTypeAsync(EntityDeleteViewModel deleteViewModel);
        Task<SlotTypeViewModel> UpdateSlotTypeAsync(SlotTypeViewModel model);

    }
}
