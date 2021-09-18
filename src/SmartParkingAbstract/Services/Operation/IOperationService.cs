using SmartParkingAbstract.ViewModels.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.Services.Operation
{
    public interface IOperationService
    {
        Task<ParkingRecordDetailViewModel> CheckIn(CheckInParkingRecord checkInParkingRecord);
        Task<ParkingRecordDetailViewModel> CheckOut(CheckOutParkingRecord checkInParkingRecord);
        Task<ParkingRecordDetailViewModel> AllowVehicleIn(Guid recordId);
        Task<ParkingRecordDetailViewModel> AllowVehicleOut(Guid recordId);
        Task<ParkingRecordDetailViewModel> UpdateCheckinRecordInfo(UpdateRecordInfoViewModel recordInfo);
        Task<ParkingRecordDetailViewModel> UpdateCheckoutRecordInfo(UpdateRecordInfoViewModel recordInfo);
    }
}
