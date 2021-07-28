using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.Services.Parking
{
    public interface IParkingService
    {
        Task<QueryResultModel<ParkingViewModel>> GetParkings(QueryModel query);
        Task<ParkingViewModel> CreateParking(CreateUpdateParkingViewModel model);
        Task<ParkingDetailsModel> GetParkingById(string clientId, Guid parkingId);
        Task<ParkingViewModel> UpdateParking(CreateUpdateParkingViewModel model);
        Task<int> AssignCards(CardParkingAssignmentViewModel assignment);
        Task<int> RemoveCards(CardParkingAssignmentViewModel assignment);
    }
}
