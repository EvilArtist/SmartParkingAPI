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
        Task<ParkingRecordDetailViewModel> AllowVehicleEnter(Guid recordId);
        Task<ParkingRecordDetailViewModel> AllowVehicleExit(Guid recordId);
        Task<ParkingRecordDetailViewModel> UpdateCheckinRecordInfo(UpdateRecordInfoViewModel recordInfo);
        Task<ParkingRecordDetailViewModel> UpdateCheckoutRecordInfo(UpdateRecordInfoViewModel recordInfo);
        Task<ParkingRecordDetailViewModel> DeclineVehicleEnter(Guid recordId);
        Task<ParkingRecordDetailViewModel> DeclineVehicleExit(Guid recordId);

        Task LockDevice(string deviceName, string connectId);
        Task<string> GetActiveClient(string deviceName);
        Task UnlockDevice(string connectId);
    }
}
