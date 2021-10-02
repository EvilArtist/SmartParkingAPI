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
    public interface ICameraConfigService
    {
        Task<IEnumerable<CameraConfigurationViewModel>> GetCamerasAsync(DeviceStatus? deviceStatus);
        Task<CameraConfigurationViewModel> CreateCameraAsync(CameraConfigurationViewModel model);
        Task<CameraConfigurationViewModel> GetCameraById(Guid id);
        Task<IEnumerable<CameraProtocolTypeViewModel>> GetCameraProtocols();
        Task<IEnumerable<CameraConfigurationViewModel>> ImportData(IEnumerable<CameraDataImport> data);
    }
}
