using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.Services.Parking
{
    public interface IVehicleTypeService
    {
        Task<VehicleTypeViewModel> CreateVehicleType(CreateUpdateVehicleTypeViewModel model);
        Task<VehicleTypeViewModel> UpdateVehicleType(CreateUpdateVehicleTypeViewModel model);
        Task<IEnumerable<VehicleTypeViewModel>> GetVehicleTypes(string clientId);
        Task<VehicleTypeViewModel> GetVehicleTypeById(string clientId, Guid id);
    }
}
