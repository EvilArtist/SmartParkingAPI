using SmartParking.Share.Constants;
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
        Task<IEnumerable<CameraConfigurationViewModel>> GetCamerasAsync(string clientId, DeviceStatus? deviceStatus);
        Task<CameraConfigurationViewModel> CreateCameraAsync(CameraConfigurationViewModel model);
        Task<CameraConfigurationViewModel> GetCameraById(Guid id, string clientId);
        Task<IEnumerable<CameraProtocolTypeViewModel>> GetCameraProtocols();
    }
}
