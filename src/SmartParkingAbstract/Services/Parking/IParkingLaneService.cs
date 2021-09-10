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
        Task<IEnumerable<ParkingLaneViewModel>> GetParkingLanes(string clientId, Guid parkingId);
        Task<ParkingLaneViewModel> GetParkingLaneById(string clientId, Guid laneId);
        Task<ParkingLaneViewModel> UpdateParkingLane(CreateUpdateParkingLaneViewModel model);
        Task<ParkingLaneViewModel> DeleteParkingLane(string clientId, Guid laneId);
    }
}
