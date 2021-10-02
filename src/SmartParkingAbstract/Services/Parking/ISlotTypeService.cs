using SmartParkingAbstract.ViewModels.DataImport;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking;
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
        Task<IEnumerable<SlotTypeViewModel>> SearchSlotTypesAsync();
        Task<SlotTypeViewModel> GetSlotTypeByIdAsync(Guid id);
        Task<bool> DeleteSlotTypeAsync(EntityDeleteViewModel deleteViewModel);
        Task<SlotTypeViewModel> UpdateSlotTypeAsync(SlotTypeViewModel model);
        Task<IEnumerable<SlotTypeViewModel>> GetSlotTypesAvailableAsync(Guid ParkingId);
        Task<IEnumerable<SlotTypeViewModel>> ImportData(IEnumerable<SlotTypeDataImport> data);
    }
}
