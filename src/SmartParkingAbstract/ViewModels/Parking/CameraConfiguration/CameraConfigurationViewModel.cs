using SmartParkingAbstract.ViewModels.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.Parking.CameraConfiguration
{
    public class CameraConfigurationViewModel: MutiTanentModel
    {
        public string CameraName { get; set; }
        public string ServerName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string URLTemplate { get; set; }
        public int CameraId { get; set; }
        public int StreamId { get; set; }
        public string Status { get; set; }
        public string Oem { get; set; }
        public Guid ProtocolId { get; set; }
        public CameraProtocolTypeViewModel Protocol { get; set; }
    }

    public class CameraProtocolTypeViewModel
    {
        public Guid Id { get; set; }
        public string ProtocolName { get; set; }
        public string Url { get; set; }
    }
}
