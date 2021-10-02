using SmartParking.Share.Constants;
using SmartParkingAbstract.ViewModels.DataImport;
using SmartParkingAbstract.ViewModels.Parking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.Services.Parking
{
    public interface ISerialPortService
    {
        Task<SerialPortConfigViewModel> CreateConfig(SerialPortConfigViewModel configurationViewModel);
        Task<IEnumerable<SerialPortConfigViewModel>> SearchSerialPortConfig(DeviceStatus? status);
        Task<SerialPortConfigViewModel> GetSerialPortConfigById(Guid id);
        Task<IEnumerable<SerialPortConfigViewModel>> ImportData(IEnumerable<MultigateDataImport> data);
    }
}
