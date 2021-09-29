using SmartParkingAbstract.ViewModels.DataImport;
using SmartParkingAbstract.ViewModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.Services.Parking
{
    public interface IParkingLaneService
    {
        Task<ParkingLaneViewModel> CreateParkingLane(CreateUpdateParkingLaneViewModel model);
        Task<IEnumerable<ParkingLaneViewModel>> GetParkingLanes(Guid parkingId);
        Task<ParkingLaneViewModel> GetParkingLaneById(Guid laneId);
        Task<ParkingLaneViewModel> UpdateParkingLane(CreateUpdateParkingLaneViewModel model);
        Task<ParkingLaneViewModel> DeleteParkingLane(Guid laneId);
        Task<IEnumerable<ParkingLaneViewModel>> ImportData(IEnumerable<ParkingLaneDataImport> data); 
    }
}
